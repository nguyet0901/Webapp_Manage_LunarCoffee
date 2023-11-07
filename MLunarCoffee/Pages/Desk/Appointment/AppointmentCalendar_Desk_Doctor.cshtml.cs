using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Desk.Appointment
{
    public class AppointmentCalendar_Desk_DoctorModel : PageModel
    {
        public int _viewByDoctor { get; set; }
        public int _branchID { get; set; }
        public int _employeeID_Main { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public AppointmentCalendar_Desk_DoctorModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string ViewByDoctor = Request.Query["ViewDoctor"];
            _viewByDoctor = ViewByDoctor != null ? Convert.ToInt32(ViewByDoctor) : 0;
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            _employeeID_Main = user.sys_employeeid;
        }
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtBranch = new DataTable();
                    dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                    dtBranch.TableName = "Branch";
                    ds.Tables.Add(dtBranch);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtDoctor = new DataTable();
                    dtDoctor = await confunc.LoadEmployee(14 ,user.sys_branchID ,user.sys_AllBranchID);
                    dtDoctor.TableName = "Doctor";
                    ds.Tables.Add(dtDoctor);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtServiceCare = new DataTable();
                    dtServiceCare = await confunc.ExecuteDataTable("YYY_sp_ServiceCare_LoadCombo" ,CommandType.StoredProcedure);
                    dtServiceCare.TableName = "ServiceCare";
                    ds.Tables.Add(dtServiceCare);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtServiceCare = new DataTable();
                    dtServiceCare = await confunc.ExecuteDataTable("YYY_sp_Appointment_LoadReasonCancel" ,CommandType.StoredProcedure);
                    dtServiceCare.TableName = "ReasonCancel";
                    ds.Tables.Add(dtServiceCare);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadScheduler(string doctorID ,string branchID ,string date_from ,string date_to ,string AppID ,string ViewType, string SerCareID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Schedule_Calendar_LoadList]" ,CommandType.StoredProcedure ,
                          "@doctorID" ,SqlDbType.Int ,doctorID
                        ,"@branchid" ,SqlDbType.Int ,branchID
                        ,"@date_from" ,SqlDbType.DateTime ,date_from
                        ,"@date_to" ,SqlDbType.DateTime ,date_to
                        ,"@ViewType" ,SqlDbType.NVarChar ,ViewType
                        ,"@SerCareID" ,SqlDbType.Int ,SerCareID
                        ,"@AppID" ,SqlDbType.Int ,AppID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostChangeDateApp(string CustomerID ,string CurrentID ,string start ,string end)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    return Content("Error");
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Appoinment_Change_Date_InSameBranch]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                          "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                          "@Date_From" ,SqlDbType.Int ,Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:mm") ,
                          "@Date_to" ,SqlDbType.NVarChar ,Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm") ,
                          "@Created_By" ,SqlDbType.Int ,user.sys_userid);
                    }
                    string result = dt.Rows[0][0].ToString();
                    if (result == "1")
                    {
                        DateTime DateFrom = Convert.ToDateTime(start).Date;
                        DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                        DateTime DateFrom_Old = Convert.ToDateTime(dt.Rows[0]["OldDate"]).Date;

                        string CustName = dt.Rows[0]["CustName"].ToString();
                        string CustCode = dt.Rows[0]["CustCode"].ToString();
                        int branchid = Convert.ToInt32(dt.Rows[0]["Branch"].ToString());
                        int doctor = Convert.ToInt32(dt.Rows[0]["Doctor"].ToString());
                        if (DateFrom_Old == DateNow || DateFrom == DateNow)
                        {
                            if (DateFrom_Old == DateNow && DateFrom == DateNow)
                            {
                                await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,Convert.ToInt32(branchid) ,Convert.ToInt32(CurrentID)
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,doctor ,"edit");
                            }
                            else if (DateFrom_Old == DateNow && DateFrom != DateNow)
                            {
                                await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,Convert.ToInt32(branchid) ,Convert.ToInt32(CurrentID)
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,doctor ,"moveout");
                            }
                            else
                            {
                                await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,Convert.ToInt32(branchid) ,Convert.ToInt32(CurrentID)
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,doctor ,"movein");
                            }
                        }
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("Error");
            }

        }
    }
}
