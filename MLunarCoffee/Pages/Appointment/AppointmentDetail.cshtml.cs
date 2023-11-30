using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Appointment
{
    public class AppointmentDetailModel : PageModel
    {
        public string _CustomerAppointmentID { get; set; }
        public string _CurrentAppointmentID { get; set; }
        public string _TicketAppointmentID { get; set; }
        public string _IsTreatmentLess { get; set; }
        public string _TreatID { get; set; }
        public string _IsUpdate { get; set; }
        public int _BranhchID { get; set; }

        // From Temp
        public string _temp_app_Doctor { get; set; }
        public string _temp_app_Date_from { get; set; }
        public string _temp_app_numberDateTo { get; set; }
        public string _temp_app_branch_ID { get; set; }
        public string _temp_AC_AppointmentID { get; set; }
        public int _AllowPast { get; set; }
        public int _SchedulerSeft { get; set; }

        public string CurrentPath { get; set; }

        private readonly IHubContext<NotiHub> hubContext;
        public AppointmentDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranhchID = user.sys_branchID;
            _AllowPast = Comon.Global.sys_Allowappinday_past;
            _SchedulerSeft = Comon.Global.sys_UsingSchedulerSeft;


            string cus = Request.Query["CustomerID"];
            string curr = Request.Query["CurrentID"];
            string tic = Request.Query["TicketID"];
            string isTreatLess = Request.Query["IsTreatLess"];
            string treatID = Request.Query["treatID"];

            _temp_app_Doctor = (Request.Query["Doctor_ID"].ToString() != "") ? Request.Query["Doctor_ID"].ToString() : "";
            _temp_app_Date_from = (Request.Query["Date_from"].ToString() != "") ? Request.Query["Date_from"].ToString() : "";
            _temp_app_numberDateTo = (Request.Query["numberDateTo"].ToString() != "") ? Request.Query["numberDateTo"].ToString() : "";
            _temp_app_branch_ID = (Request.Query["branch_ID"].ToString() != "") ? Request.Query["branch_ID"].ToString() : "";
            _temp_AC_AppointmentID = (Request.Query["App_AC_ID"].ToString() != "") ? Request.Query["App_AC_ID"].ToString() : "0";

            CurrentPath = HttpContext.Request.Path;

            if (cus != null)
            {
                _CustomerAppointmentID = cus.ToString();
                _TicketAppointmentID = (tic != null) ? tic.ToString() : "0";
                _IsTreatmentLess = (isTreatLess != null) ? isTreatLess.ToString() : "0";
                _TreatID = (treatID != null) ? treatID.ToString() : "0";
                if (curr != null)
                {
                    _CurrentAppointmentID = curr.ToString();
                    _IsUpdate = "1";
                }
                else
                {
                    _CurrentAppointmentID = "0";
                    _IsUpdate = "0";
                }
            }
            else
            {
                _CustomerAppointmentID = "0";
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadCombo(string cusid ,string currentid ,string treatID ,string ticketid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadPara("Schedule Type");
                        dt.TableName = "Schedule_Type";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_EmployeeFull_v2" ,CommandType.StoredProcedure
                            ,"@BranchToken" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                            ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                            );
                        dt.TableName = "EmpFull";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                               ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                               ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Appointment_LoadComboTime" ,CommandType.StoredProcedure);
                        dt.TableName = "Time";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Appointment_LoadCustomerInfo" ,CommandType.StoredProcedure
                             ,"@CustomerID" ,SqlDbType.Int ,cusid);
                        dt.TableName = "Info";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Schedule_LoadCombo_ServiceTreatment" ,CommandType.StoredProcedure
                              ,"@CustomerID" ,SqlDbType.Int ,cusid);
                        dt.TableName = "Service_Treat";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_LoadExpectedAmount]" ,CommandType.StoredProcedure ,
                               "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(cusid));
                        dt.TableName = "DataInstallment";
                        return dt;
                    }
                }));


                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Schedule_Type_LoadCombo]" ,CommandType.StoredProcedure);
                        dt.TableName = "ScheduleType";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_Sys_Tag_LoadCombo]" ,CommandType.StoredProcedure);
                        dt.TableName = "Tag";
                        return dt;
                    }
                }));


                if (treatID != "0" && treatID != null)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[MLU_sp_Schedule_TreatmentContentNext]" ,CommandType.StoredProcedure ,
                                     "@TreatID" ,SqlDbType.Int ,treatID
                            );
                            dt.TableName = "ContentTreat";
                            return dt;
                        }
                    }));

                }

                if (ticketid != "0" && ticketid != null)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Ticket_LoadHistoryCare]" ,CommandType.StoredProcedure ,
                                     "@TicketID" ,SqlDbType.Int ,ticketid
                            );
                            dt.TableName = "TicketHisCare";
                            return dt;
                        }
                    }));

                }

                if (Convert.ToInt32(currentid) != 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_LoadToDetail]" ,CommandType.StoredProcedure ,
                                 "@App_ID" ,SqlDbType.Int ,Convert.ToInt32(currentid));
                            dt.TableName = "DataUpdate";
                            return dt;
                        }
                    }));
                }


                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }


        }

        public async Task<IActionResult> OnPostExcuteDataAppointmentData(string data ,string CurrentID ,string CustomerID ,string TicketID)
        {
            try
            {
                int allowpast = Comon.Global.sys_Allowappinday_past;
                TicketID = (TicketID != null ? TicketID : "");
                ScheduleDetail DataMain = JsonConvert.DeserializeObject<ScheduleDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("MLU_sp_v2_Customer_Appointment_Insert" ,CommandType.StoredProcedure ,
                         "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                         "@branch_ID" ,SqlDbType.Int ,DataMain.branch_ID ,
                         "@Doctor_ID" ,SqlDbType.Int ,DataMain.Doctor_ID ,
                         "@Assistant" ,SqlDbType.Int ,DataMain.Assistant ,
                         "@Doctor_ID2" ,SqlDbType.Int ,DataMain.Doctor_ID2 ,
                         "@Ref_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Ref_Amount) ,
                         "@Consult_ID" ,SqlDbType.Int ,DataMain.Consult_ID ,
                         "@Room" ,SqlDbType.Int ,DataMain.Room ,
                         "@TicketID" ,SqlDbType.Int ,TicketID ,
                         "@TypeSchedule" ,SqlDbType.Int ,DataMain.TypeSchedule ,
                         "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                         "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"") ,
                         "@Created" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                         "@Date_from" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from) ,
                         "@Date_to" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).AddMinutes(Convert.ToInt32(DataMain.numberDateTo)) ,
                         "@Old" ,SqlDbType.Int ,DataMain.old ,
                         "@ServiceCareID" ,SqlDbType.NVarChar ,DataMain.ServiceCare_ID ,
                         "@ServiceTreatID" ,SqlDbType.NVarChar ,DataMain.ServiceTreatment_ID ,
                         "@NoteForBranch" ,SqlDbType.NVarChar ,DataMain.NoteForBranch ,
                         "@ScheduleTypeID" ,SqlDbType.Int ,DataMain.ScheduleTypeID ,
                         "@TagTokenID" ,SqlDbType.NVarChar ,DataMain.TagTokenID ,
                         "@ACAppointmentID" ,SqlDbType.Int ,DataMain.AC_AppointmentID,
                         "@TreatIndex" ,SqlDbType.Int ,DataMain.TreatIndex ,
                         "@allowpast" ,SqlDbType.Int ,allowpast
                    );

                    }
                }

                else // Cập nhật lịch hẹn
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("MLU_sp_v2_Customer_Appointment_Update" ,CommandType.StoredProcedure ,
                            "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                            "@branch_ID" ,SqlDbType.Int ,DataMain.branch_ID ,
                            "@Doctor_ID" ,SqlDbType.Int ,DataMain.Doctor_ID ,
                            "@Assistant" ,SqlDbType.Int ,DataMain.Assistant ,
                            "@Doctor_ID2" ,SqlDbType.Int ,DataMain.Doctor_ID2 ,
                            "@Ref_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Ref_Amount) ,
                            "@Consult_ID" ,SqlDbType.Int ,DataMain.Consult_ID ,
                            "@TypeSchedule" ,SqlDbType.DateTime ,DataMain.TypeSchedule ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"") ,
                            "@History" ,SqlDbType.NVarChar ,DataMain.History ,
                            "@Modified" ,SqlDbType.Decimal ,Comon.Comon.GetDateTimeNow() ,
                            "@Date_from" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from) ,
                            "@Date_to" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).AddMinutes(Convert.ToInt32(DataMain.numberDateTo)) ,
                            "@ServiceCareID" ,SqlDbType.NVarChar ,DataMain.ServiceCare_ID ,
                            "@Old" ,SqlDbType.Int ,DataMain.old ,
                            "@NoteForBranch" ,SqlDbType.NVarChar ,DataMain.NoteForBranch ,
                            "@ServiceTreatID" ,SqlDbType.NVarChar ,DataMain.ServiceTreatment_ID ,
                            "@Current_ID" ,SqlDbType.Int ,CurrentID ,
                            "@ScheduleTypeID" ,SqlDbType.Int ,DataMain.ScheduleTypeID ,
                            "@TagTokenID" ,SqlDbType.NVarChar ,DataMain.TagTokenID ,
                            "@allowpast" ,SqlDbType.Int ,allowpast ,
                            "@Allowedit" ,SqlDbType.Int ,DataMain.Allowedit


                        );
                    }
                }
                string resulf = ds.Tables[0].Rows[0]["result"].ToString();
                if (resulf == "1")
                {
                    string id = ds.Tables[0].Rows[0]["id"].ToString();
                    string CustName = ds.Tables[0].Rows[0]["CustName"].ToString();
                    string CustCode = ds.Tables[0].Rows[0]["CustCode"].ToString();
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DateTime DateFrom = Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).Date;
                        DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                        int newBranch = Convert.ToInt32(DataMain.branch_ID);
                        int newDoctor = Convert.ToInt32(DataMain.Doctor_ID);
                        int newappid = Convert.ToInt32(id);

                        if (CurrentID == "0")
                        {
                            ds.Tables[0].Rows[0]["IsCheckReTreat"] = "1";
                            if (newDoctor != 0)
                            {
                                await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,newBranch ,newappid ,DateFrom ,"insert" ,0);
                            }

                            if (DateFrom == DateNow)
                            {
                                await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,newappid
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,newDoctor ,"new");
                                if (DataMain.Room != 0)
                                {
                                    //Comon.PushNoti.Noti_Parti_Appointment_Room(newBranch, newappid
                                    //, Convert.ToInt32(CustomerID), CustCode, CustName
                                    //, Convert.ToInt32(DataMain.Room), "newbook");
                                }
                            }
                        }
                        else
                        {
                            int oldbranchid = Convert.ToInt32(ds.Tables[0].Rows[0]["OldBranch"].ToString());
                            int oldDoctor = Convert.ToInt32(ds.Tables[0].Rows[0]["OldDoctor"].ToString());
                            int Room = Convert.ToInt32(ds.Tables[0].Rows[0]["Room"].ToString());
                            int CurrentID_Int = Convert.ToInt32(CurrentID);
                            int oldBranch = Convert.ToInt32(oldbranchid);
                            DateTime DateFrom_Old = Convert.ToDateTime(ds.Tables[0].Rows[0]["OldDate"].ToString()).Date;

                            if (newDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,newBranch ,newappid ,DateFrom ,"insert" ,0);
                            if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,oldBranch ,newappid ,DateFrom_Old ,"delete" ,0);

                            if (DateFrom_Old != DateFrom)
                            {
                                ds.Tables[0].Rows[0]["IsCheckReTreat"] = "1";
                            }

                            if (DateFrom_Old == DateNow || DateFrom == DateNow)
                            {
                                // Same branch, change still in datenow
                                if (oldbranchid == newBranch && DateFrom_Old == DateNow && DateFrom == DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,oldbranchid ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"edit");
                                }
                                // Same branch, move out datenow
                                else if (oldbranchid == newBranch && DateFrom_Old == DateNow && DateFrom != DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,oldbranchid ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"moveout");
                                }

                                // Same branch, move in datenow
                                else if (oldbranchid == newBranch && DateFrom_Old != DateNow && DateFrom == DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,oldbranchid ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"movein");
                                }
                                // Different branch,change still in datenow
                                else if (oldbranchid != newBranch && DateFrom_Old == DateNow && DateFrom == DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,oldbranchid ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"moveout");
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"movein");
                                }
                                // Different branch,move out datenow
                                else if (oldbranchid != newBranch && DateFrom_Old == DateNow && DateFrom != DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,oldbranchid ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"moveout");

                                }
                                // Different branch,move in datenow
                                else if (oldbranchid != newBranch && DateFrom_Old != DateNow && DateFrom == DateNow)
                                {
                                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,0 ,"movein");

                                }

                                //--------------------------------------------------------------------------------------//
                                //--------------------------------------------------------------------------------------//


                                // Same doctor, change still in datenow
                                if (oldDoctor == newDoctor && DateFrom_Old == DateNow && DateFrom == DateNow)
                                {
                                    if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                     ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                     ,0 ,oldDoctor ,"edit");
                                }
                                // Same doctor, move out datenow
                                else if (oldDoctor == newDoctor && DateFrom_Old == DateNow && DateFrom != DateNow)
                                {
                                    if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                     ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                     ,0 ,oldDoctor ,"moveout");
                                }
                                // Same doctor, move in datenow
                                else if (oldDoctor == newDoctor && DateFrom_Old != DateNow && DateFrom == DateNow)
                                {
                                    if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                     ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                     ,0 ,oldDoctor ,"movein");
                                }
                                // Different doctor,change still in datenow
                                else if (oldDoctor != newDoctor && DateFrom_Old == DateNow && DateFrom == DateNow)
                                {
                                    if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,oldDoctor ,"moveout");
                                    if (newDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                     ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                     ,0 ,newDoctor ,"movein");
                                }
                                // Different doctor,move out datenow
                                else if (oldDoctor != newDoctor && DateFrom_Old == DateNow && DateFrom != DateNow)
                                {
                                    if (oldDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                        ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                        ,0 ,oldDoctor ,"moveout");
                                }
                                // Different doctor,move in datenow
                                else if (oldDoctor != newDoctor && DateFrom_Old != DateNow && DateFrom == DateNow)
                                {
                                    if (newDoctor != 0) await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,newBranch ,CurrentID_Int
                                    ,Convert.ToInt32(CustomerID) ,CustCode ,CustName
                                    ,0 ,newDoctor ,"movein");
                                }

                            }


                        }


                    }
                }
                return Content(Comon.DataJson.Datatable(ds.Tables[0]));

            }
            catch (Exception ex)
            {
                return Content("Error");
            }

        }

        public async Task<IActionResult> OnPostGetInfoAppointment(string CustomerID ,string Date ,string BranchID ,string CurrentAppID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Appointment_LoadInfo_Branch_And_Customer]" ,CommandType.StoredProcedure
                            ,"@Customer_ID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                            ,"@branch_ID" ,SqlDbType.Int ,Convert.ToInt32(BranchID)
                            ,"@appCurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentAppID)
                            ,"@Date" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(Date)
                            );
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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

        public async Task<IActionResult> OnPostCheckDataDetail(string CustomerID ,string DateFrom ,string BranchID ,string CurrentAppID)
        {
            try
            {
                int multiapp = Comon.Global.sys_Allowmultiappinday;
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_Check_Data]" ,CommandType.StoredProcedure
                            ,"@Customer_ID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                            ,"@Branch_ID" ,SqlDbType.Int ,Convert.ToInt32(BranchID)
                            ,"@Date_from" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateFrom)
                            ,"@appCurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentAppID)
                              ,"@multiapp" ,SqlDbType.Int ,Convert.ToInt32(multiapp)
                            );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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

        public async Task<IActionResult> OnPostLoaData_Extension_Doctor(int BranchID ,int EmpID ,string dateFrom ,string dateTo ,int AppID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_Doctor_ByDay]" ,CommandType.StoredProcedure
                        ,"@BranchID" ,SqlDbType.Int ,BranchID
                        ,"@DoctorID" ,SqlDbType.Int ,EmpID
                        ,"@DateFrom" ,SqlDbType.DateTime ,dateFrom
                        ,"@DateTo" ,SqlDbType.DateTime ,dateTo
                        ,"@AppID" ,SqlDbType.Int ,AppID
                        );
                    dt.TableName = "Sheduler";
                    ds.Tables.Add(dt);
                }
                DataTable dt1 = new DataTable();
                dt1 = await Comon.WorkEmployee.LoadScheduler_By_Doctor_Branch(HttpContext ,EmpID ,BranchID ,dateFrom ,dateTo);
                dt1.TableName = "Work";
                ds.Tables.Add(dt1);
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostCheckAppReTreat(int CustomerID ,string Date_From)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Appointment_Check_AppRetreat_Exist" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,CustomerID
                      ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(Date_From).AddDays(1)
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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

        public async Task<IActionResult> OnPostDuplicate(string AppID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_Duplicate_LoadDetail]" ,CommandType.StoredProcedure ,
                        "@App_ID" ,SqlDbType.Int ,Convert.ToInt32(AppID));
                    dt.TableName = "DataApp";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }


        }
    }

    public class ScheduleDetail
    {
        public int Customer_ID { get; set; }
        public string branch_ID { get; set; }
        public string Doctor_ID { get; set; }
        public string Assistant { get; set; }

        public string Doctor_ID2 { get; set; }
        public decimal Ref_Amount { get; set; }

        public string Consult_ID { get; set; }
        public string TypeSchedule { get; set; }/// <summary> /// Tư vấn, điều trị /// </summary>
        public string ScheduleTypeID { get; set; } /// <summary> /// Lịch chốt, Lịch Tạm /// </summary>
        public int TreatIndex { get; set; }
        public string TagTokenID { get; set; }
        public string Note { get; set; }
        public string History { get; set; }
        public string ServiceCare_ID { get; set; }
        public string ServiceTreatment_ID { get; set; }
        public string Date_from { get; set; }
        public int numberDateTo { get; set; }
        public int old { get; set; }
        public string NoteForBranch { get; set; }
        public int Room { get; set; }
        public int AC_AppointmentID { get; set; }
        public int Allowedit { get; set; }


    }
}
