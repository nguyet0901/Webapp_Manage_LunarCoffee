
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Appointment
{
    public class AppointmentModel : PageModel
    {
        public int _branchID { get; set; }
        public string layout { get; set; }
        public string _setting_AppointmentDoctor { get; set; }
        public string CurrentPath { get; set; }

        private readonly IHubContext<NotiHub> hubContext;
        public AppointmentModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            CurrentPath = HttpContext.Request.Path;

            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

        public async Task<IActionResult> OnPostLoadComboMain(int branch)
        {
            try
            {
                var tasks = new List<Task<DataTable>>();
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                             , "@UserID", SqlDbType.Int, user.sys_userid
                             , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployee(14, branch, 0);
                        dt.TableName = "Doctor";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Appointment_LoadComboTime", CommandType.StoredProcedure);
                        dt.TableName = "Time";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_Sys_Tag_LoadCombo", CommandType.StoredProcedure);
                        dt.TableName = "Tag";
                        return dt;
                    }
                }));


                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadComboDoctor(int branch)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadEmployee(14, branch, 0);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostLoadScheduler(string datefrom, string dateto, int branch, int DoctorID, int AppID, int type, int IsTemp)
        {
            try
            {
                if (AppID == 0)
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[MLU_sp_v2_Appointment_Doctor_Schedule_LoadList]", CommandType.StoredProcedure,
                            "@DateFrom", SqlDbType.NVarChar, datefrom
                            , "@DateTo", SqlDbType.NVarChar, dateto
                            , "@DoctorID", SqlDbType.Int, DoctorID
                            , "@AppID", SqlDbType.Int, AppID
                            , "@Type", SqlDbType.Int, type
                            , "@IsTemp", SqlDbType.Int, IsTemp
                            , "@Branch", SqlDbType.Int, branch);
                    }
                    if (ds != null)
                    {
                        return Content(Comon.DataJson.Dataset(ds));
                    }
                    else
                    {
                        return Content("[]");
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    var user = Session.GetUserSession(HttpContext);
                    AppID = Math.Abs(AppID);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_v2_Appointment_Doctor_Schedule_LoadList]", CommandType.StoredProcedure,
                            "@DateFrom", SqlDbType.NVarChar, datefrom
                            , "@DateTo", SqlDbType.NVarChar, dateto
                            , "@DoctorID", SqlDbType.Int, DoctorID
                            , "@AppID", SqlDbType.Int, AppID
                            , "@Type", SqlDbType.Int, type
                            , "@IsTemp", SqlDbType.Int, IsTemp
                            , "@Branch", SqlDbType.Int, branch);
                    }
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else
                    {
                        return Content("[]");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostUpdateScheduler(DateTime datefrom, DateTime dateto, int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    if (id > 0)
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Doctor_Schedule_Update]", CommandType.StoredProcedure,
                                  "@DateFrom", SqlDbType.DateTime, datefrom
                                  , "@DateTo", SqlDbType.DateTime, dateto
                                  , "@ID", SqlDbType.Int, id
                                  , "@UserID", SqlDbType.Int, user.sys_userid);

                        if (dt.Rows[0]["Result"].ToString() == "1")
                        {
                            string branchid = dt.Rows[0]["Branch"].ToString();
                            string CustName = dt.Rows[0]["CustName"].ToString();
                            string CustID = dt.Rows[0]["CustID"].ToString();
                            string CustCode = dt.Rows[0]["CustCode"].ToString();

                            DateTime DateFrom_Old = Convert.ToDateTime(dt.Rows[0]["OldDate"].ToString()).Date;
                            DateTime DateFrom = datefrom.Date;
                            DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                            int Doctor = Convert.ToInt32(dt.Rows[0]["Doctor"].ToString());
                            await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 0, Convert.ToInt32(branchid)
                                , Convert.ToInt32(id)
                                , DateFrom
                                , "update"
                                , user.sys_userid);
                            if (DateFrom_Old == DateNow || DateFrom == DateNow)
                            {
                                if (DateFrom_Old == DateNow && DateFrom == DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext, Convert.ToInt32(branchid), Convert.ToInt32(id)
                                        , Convert.ToInt32(CustID), CustCode, CustName
                                        , 0, Doctor, "edit");
                                }
                                else if (DateFrom_Old == DateNow && DateFrom != DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext, Convert.ToInt32(branchid), Convert.ToInt32(id)
                                        , Convert.ToInt32(CustID), CustCode, CustName
                                        , 0, Doctor, "moveout");
                                }
                                else
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext, Convert.ToInt32(branchid), Convert.ToInt32(id)
                                        , Convert.ToInt32(CustID), CustCode, CustName
                                        , 0, Doctor, "movein");
                                }
                            }
                            return Content(dt.Rows[0]["ID"].ToString());
                        }
                        else return Content("0");
                    }
                    else
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Doctor_Schedule_Temp_Update]", CommandType.StoredProcedure,
                                "@DateFrom", SqlDbType.DateTime, datefrom
                                , "@DateTo", SqlDbType.DateTime, dateto
                                , "@ID", SqlDbType.Int, id
                                , "@UserID", SqlDbType.Int, user.sys_userid);
                        if (dt.Rows[0]["RESULT"].ToString() == "1")
                        {
                            await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 1, Convert.ToInt32(dt.Rows[0]["Branch"].ToString())
                               , Convert.ToInt32(id)
                               , datefrom
                               , "update"
                               , user.sys_userid);
                            return Content(dt.Rows[0]["ID"].ToString());
                        }
                        else return Content("0");
                    }
                }
            }
            catch (Exception)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDeleteNote(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Doctor_Schedule_Temp_Delete]", CommandType.StoredProcedure
                       , "@ID", SqlDbType.Int, id
                       , "@UserID", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    if (Convert.ToInt32(dt.Rows[0]["RESULT"]) == id)
                    {
                        await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 1, Convert.ToInt32(dt.Rows[0]["Branch"].ToString())
                               , Convert.ToInt32(id)
                               , Comon.Comon.GetDateTimeNow()
                               , "delete"
                               , user.sys_userid);
                    }

                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("-1");
                }
            }
            catch (Exception)
            {
                return Content("-1");
            }

        }

        public async Task<IActionResult> OnPostCheckSchedulerIsEdit(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Doctor_Schedule_CheckAppointment]", CommandType.StoredProcedure
                       , "@ID", SqlDbType.Int, id);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("-1");
                }
            }
            catch (Exception)
            {
                return Content("-1");
            }

        }

        public async Task<IActionResult> OnPostLoadWorkTimeDoctor(DateTime datefrom, DateTime dateto, string tokenDocID)
        {
            try
            {
                tokenDocID = (tokenDocID != null ? tokenDocID : "");
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_v2_Work_Schedule_Appointment_Doctor]", CommandType.StoredProcedure,
                          "@DateFrom", SqlDbType.DateTime, datefrom
                          , "@DateTo", SqlDbType.DateTime, dateto
                          , "@TokenDocID", SqlDbType.NVarChar, tokenDocID
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadDataAction(int appointment)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Schedule_Action_LoadDetail]", CommandType.StoredProcedure,
                        "@Current_ID", SqlDbType.Int, appointment);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadtemdata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_Temp_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));

                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception e)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostSearchCustomer(string search)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_AppointmentTemp_SearchCustomer]", CommandType.StoredProcedure,
                      "@textsearch", SqlDbType.NVarChar, search
                        , "@BranchToken", SqlDbType.Int, user.sys_AreaBranch
                        , "@IsAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@IsCustNotViewByBranch", SqlDbType.Int, Global.sys_CustomerNotViewByBranch);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostExcuteTemp(string data, string CurrentID)
        {
            try
            {
                ScheduleTempDetail DataMain = JsonConvert.DeserializeObject<ScheduleTempDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_Appointment_Temp_Insert", CommandType.StoredProcedure,
                             "@branch_ID", SqlDbType.Int, DataMain.branch_ID,
                             "@Doctor_ID", SqlDbType.Int, DataMain.Doctor_ID,
                             "@Created_By", SqlDbType.Int, user.sys_userid,
                             "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", ""),
                             "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", ""),
                             "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", ""),
                             "@Date_from", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from),
                             "@Date_to", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).AddMinutes(Convert.ToInt32(DataMain.numberDateTo)),
                             "@ServiceCare_ID", SqlDbType.NVarChar, DataMain.ServiceCare);

                        if (dt.Rows[0]["RESULT"].ToString() == "1")
                        {
                            await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 1, Convert.ToInt32(DataMain.branch_ID)
                               , Convert.ToInt32(dt.Rows[0]["ID"].ToString())
                               , Convert.ToDateTime(dt.Rows[0]["DateFrom"])
                               , "insert"
                               , user.sys_userid);
                            return Content(Comon.DataJson.Datatable(dt));
                        }
                        else return Content("0");
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_Appointment_Temp_Update", CommandType.StoredProcedure,
                           "@ID", SqlDbType.Int, CurrentID,
                           "@Created_By", SqlDbType.Int, user.sys_userid,
                           "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", ""),
                           "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", ""),
                           "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", ""),
                           "@ServiceCare_ID", SqlDbType.NVarChar, DataMain.ServiceCare,
                           "@Date_from", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from),
                           "@Date_to", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).AddMinutes(Convert.ToInt32(DataMain.numberDateTo)),
                           "@Doctorid", SqlDbType.Int, DataMain.Doctor_ID);
                    }
                    if (dt.Rows[0]["RESULT"].ToString() == "1")
                    {
                        await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 1, Convert.ToInt32(DataMain.branch_ID)
                           , Convert.ToInt32(CurrentID)
                           , Convert.ToDateTime(dt.Rows[0]["DateFrom"])
                           , "update"
                           , user.sys_userid);
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else return Content("0");
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class ScheduleTempDetail
    {
        public string branch_ID { get; set; }
        public int Doctor_ID { get; set; }
        public string Note { get; set; }
        public string ServiceCare { get; set; }
        public string Date_from { get; set; }
        public int numberDateTo { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
