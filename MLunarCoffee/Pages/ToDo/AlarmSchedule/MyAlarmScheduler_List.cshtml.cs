using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using SourceMain.Models.Model.WorkScheduler;
using SourceMain.Models.Model.AlrmScheduler;
using SourceMain.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace SourceMain.Pages.ToDo.AlarmSchedule
{
    public class MyAlarmScheduler_ListModel : PageModel
    {
        public string _AlarmScheduler_List { get; set; }
        public string _CurrentEmployeeID { get; set; }
        public string _StatusScheduler_List { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public MyAlarmScheduler_ListModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _CurrentEmployeeID = user.sys_employeeid.ToString();

        }
        public async Task<IActionResult> OnPostInitializeDate()
        {
            AlrmScheduler_List[] list = new AlrmScheduler_List[5];
            list[0] = new AlrmScheduler_List() { type = 0, text = "TD", title = "Today" };
            list[1] = new AlrmScheduler_List() { type = 1, text = "D", title = "Fixed Date" };
            list[2] = new AlrmScheduler_List() { type = 2, text = "DL", title = "Daily" };
            list[3] = new AlrmScheduler_List() { type = 3, text = "WL", title = "Weekly" };
            list[4] = new AlrmScheduler_List() { type = 4, text = "ML", title = "Monthly" };
            _AlarmScheduler_List = JsonConvert.SerializeObject(list);

            return Content(_AlarmScheduler_List);
        }
        public async Task<IActionResult> OnPostInitialize()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(String));// ID cua alram
            dt.Columns.Add("Name", typeof(String));
            DataRow dr = dt.NewRow(); dr["ID"] = "2"; dr["Name"] = "All Status"; dt.Rows.Add(dr);
            dr = dt.NewRow(); dr["ID"] = "0"; dr["Name"] = "Not yet"; dt.Rows.Add(dr);
            dr = dt.NewRow(); dr["ID"] = "1"; dr["Name"] = "Done"; dt.Rows.Add(dr);
            _StatusScheduler_List = JsonConvert.SerializeObject(dt);


            return Content(_StatusScheduler_List);
        }

        public async Task<IActionResult> OnPostLoadScheduler(int empid, string date)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[YYY_sp_Todo_AlarmSchedule_Load_List_ByDay]", CommandType.StoredProcedure,
                        "@CreateadBy", SqlDbType.Int, user.sys_userid
                        , "@Employee", SqlDbType.Int, empid
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                    );
                }
                AlrmScheduler[] timeline = Comon.Comon.GetArrayTimeLine_AlarmScheduler(ds.Tables[0]);
                AlrmScheduler_Extension[] extension = Comon.Comon.GetArrayTimeLine_AlarmSchedulerExtension(ds.Tables[1]);
                string stridateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]).ToString("yyyy-MM-dd");
                string stridateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]).ToString("yyyy-MM-dd");
                string[] resulf = new string[4] {
                    JsonConvert.SerializeObject(timeline)
                    , JsonConvert.SerializeObject(extension)
                    , stridateFrom
                    , stridateTo };
                return Content( JsonConvert.SerializeObject(resulf));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostLoadSchedulerDetail(string date, string id)
        {
            try
            {

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[YYY_sp_Todo_AlarmSchedule_Load_Detail_ByDay]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid
                        , "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(date)
                        , "@ID", SqlDbType.Int, id
                    );
                }
                return Content(JsonConvert.SerializeObject(ds));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostInsertCommentDetail (string date, string id, string empwork, string empcreated, string title, string content)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Insert_Comment]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid
                         , "@EmpID", SqlDbType.Int, user.sys_employeeid
                        , "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(date)
                        , "@Content", SqlDbType.NVarChar, content
                        , "@ID", SqlDbType.Int, id
                    );
                }
                // Người thêm là người được giao,
                if (empwork == user.sys_employeeid.ToString())
                {
                    if (empcreated != empwork)
                        await NotiLocal.Noti_Parti_Emp(hubContext,Convert.ToInt32(empcreated), title, content);
                }
                // Người thêm là người giao
                else
                {
                    await NotiLocal.Noti_Parti_Emp(hubContext,Convert.ToInt32(empwork), title, content);
                }

                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostChangeStatus(int id, int status, string date)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                   await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Update_Status]", CommandType.StoredProcedure,
                        "@alrmid", SqlDbType.Int, id,
                         "@status", SqlDbType.Int, status,
                         "@Created_By", SqlDbType.Int, user.sys_userid,
                       "@Date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMD(date)
                   );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostUpdateCheckList(int id, string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                  await  connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Update_CheckList]", CommandType.StoredProcedure,
                        "@alrmid", SqlDbType.Int, id,
                         "@Data", SqlDbType.NVarChar, data,
                         "@Created_By", SqlDbType.Int, user.sys_userid
                   );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                  await  connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostActiveItem(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Active]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadAlarmScheduleToday()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Noti_LoadList]", CommandType.StoredProcedure,
                             "@empID", SqlDbType.Int, user.sys_employeeid
                          );
                }
                if (dt != null)
                {
                    return Content( JsonConvert.SerializeObject(InvestigateAlarmSchedule(dt)));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public static DataTable InvestigateAlarmSchedule(DataTable dt)
        {
            DateTime now = DateTime.Now;
            DataTable dtreut = new DataTable();
            dtreut.Columns.Add("ID", typeof(String));
            dtreut.Columns.Add("Title", typeof(String));
            dtreut.Columns.Add("Content", typeof(String));
            dtreut.Columns.Add("TimeAlarm", typeof(String));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtreut.NewRow();
                dr["ID"] = dt.Rows[i]["ID"];
                dr["Title"] = dt.Rows[i]["Title"];
                dr["Content"] = dt.Rows[i]["Content"];
                var temp = Newtonsoft.Json.Linq.JToken.Parse(dt.Rows[i]["data"].ToString());
                DateTime date = Convert.ToDateTime(temp[0]["date"].ToString());
                dr["TimeAlarm"] = temp[0]["hour"].ToString();
                switch (temp[0]["type"].ToString())
                {
                    case "1":

                        if (date.ToString().Split(' ')[0] == now.ToString().Split(' ')[0])
                        {
                            dtreut.Rows.Add(dr);
                        }
                        break;
                    case "2":
                        dtreut.Rows.Add(dr);
                        break;
                    case "3":
                        var nowWeek = now.DayOfWeek;
                        int dateweek = Convert.ToInt32(temp[0]["dayofweek"].ToString());
                        if (Convert.ToInt32(nowWeek.GetHashCode()) == dateweek)
                        {
                            dtreut.Rows.Add(dr);
                        }
                        break;
                    case "4":
                        if (now.ToString().Split(' ')[0].Split('/')[1] == temp[0]["day"].ToString())
                        {
                            dtreut.Rows.Add(dr);
                        }
                        break;
                    default:
                        break;
                }

            }
            return dtreut;
        }
    }
}
