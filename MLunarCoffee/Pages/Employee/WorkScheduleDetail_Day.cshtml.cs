using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models.Model.WorkScheduler;


namespace MLunarCoffee.Pages.Employee
{
    public class WorkScheduleDetail_DayModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _DateTimeCurrent { get; set; }
        public string _DateTimeLine { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string DateTimeLine = Request.Query["DateTimeLine"];
            string DateTimeCurrent = Request.Query["DateTimeCurrent"];
            if (curr != null)
            {
                _DateTimeCurrent = DateTimeCurrent.ToString();
                _DateTimeLine = DateTimeLine.ToString();
                _CurrentID = curr.ToString();
            }
            else
                Response.Redirect("/assests/Error/index.html");
        }
         public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Shift_Combo]", CommandType.StoredProcedure);
                    dt.TableName = "dataShift";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    var user = Session.GetUserSession(HttpContext);
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "dataBranch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
         public async Task<IActionResult> OnPostLoadDataDetail(string empid, string datetimeline, string datecurrent)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Work_Schedule_LoadUpdate_ParticularDay]", CommandType.StoredProcedure,
                      "@EmpID", SqlDbType.Int, Convert.ToInt32(empid)
                      , "@datetimeline", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(datetimeline)
                      , "@datecurrent", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(datecurrent + " 00:00")
                    );
                }
                int dayofweek = (Int32)Comon.Comon.DateTimeDMY_To_YMD(datecurrent + " 00:00").DayOfWeek;
                if (ds != null)
                {
                    WorkScheduler_Extension[] workDetail;


                    if (ds.Tables[0].Rows.Count == 0) // khong phai tu lich lam viec
                    {
                        workDetail = Comon.Comon.GetArrayTimeLine_Detail_Empty_Particular();

                    }
                    else // tu lich lam viec
                    {
                        workDetail = Comon.Comon.GetArrayTimeLine_Detail_Particular(ds.Tables[0], dayofweek);
                    }


                    WorkScheduler_Extension[] extension = Comon.Comon.GetArrayTimeLine_Detail_Particular_Ex(ds.Tables[1]);
                    return Content( JsonConvert.SerializeObject((extension != null) ? extension : workDetail));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception e)
            {
                return Content("[]");
            }
        }
         public async Task<IActionResult> OnPostExcuteData(string data, string empid, string date)
        {
            try
            {
                WorkScheduler_Extension[] DataMain = JsonConvert.DeserializeObject<WorkScheduler_Extension[]>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("MLU_sp_Work_Schedule_Extension_Update", CommandType.StoredProcedure,
                        "@EmpID", SqlDbType.NVarChar, empid,
                        "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(date + " 00:00"),
                        "@Created_By", SqlDbType.Int, user.sys_userid,
                        "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(DataMain)
                    );
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
