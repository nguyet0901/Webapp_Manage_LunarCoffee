using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Models.Customer;

namespace MLunarCoffee.Controllers.Customer
{

    [ApiController, Authorize]
    [Route("api/Customer/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly IHubContext<NotiHub> hubContext;
        public AppointmentController(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        #region // NextStatusApp
        [HttpPost("ChangeStatus_Next")]
        public async Task<IActionResult> ChangeStatus_Next(AppStatus appStatus)
        {            
            try
            {                
                DataTable dt = new DataTable();                
                AppStatusDetail DataMain = (appStatus.AppDetail);
                var user = Session.GetUserSession(HttpContext);
                if (appStatus.CurrentID == "0")
                {
                    return Content("0");
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Appoinment_ChangeStatus]" ,CommandType.StoredProcedure ,
                           "@CurrentID" ,SqlDbType.Int ,appStatus.CurrentID ,
                           "@Status" ,SqlDbType.Int ,DataMain.StatusID ,
                           "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"").Trim() ,
                           "@ForectTime" ,SqlDbType.Int ,DataMain.ForectTime ,
                           "@BranchID" ,SqlDbType.Int ,user.sys_branchID ,
                           "@EmployeeID" ,SqlDbType.Int ,user.sys_employeeid ,
                           "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                           "@RoomID" ,SqlDbType.Int ,DataMain.RoomID ,
                           "@ChairID" ,SqlDbType.Int ,DataMain.ChairID ,
                           
                           "@DoctorID" ,SqlDbType.Int ,DataMain.DoctorID ,
                           "@AssistID" ,SqlDbType.Int ,DataMain.AssistID ,
                           "@DoctorID2" ,SqlDbType.Int ,DataMain.DoctorID2 ,
                           "@AssistID2" ,SqlDbType.Int ,DataMain.AssistID2 ,
                           "@DoctorID3" ,SqlDbType.Int ,DataMain.DoctorID3 ,
                           "@AssistID3" ,SqlDbType.Int ,DataMain.AssistID3 ,
                           "@DoctorID4" ,SqlDbType.Int ,DataMain.DoctorID4 ,
                           "@AssistID4" ,SqlDbType.Int ,DataMain.AssistID4 ,
                           "@IsCheckout" ,SqlDbType.Int ,DataMain.IsCheckout
                     );
                    }
                    if (dt.Rows.Count != 0)
                    {
                        Comon.SendNoti.SendNoti_OneToken_Noti_ChangeStatusAppointment(appStatus.AppPlatform
                        ,appStatus.AppToken
                        ,"Lịch Hẹn"
                        ,"Trạng Thái : " + dt.Rows[0][0].ToString()
                        ,"0"
                        ,appStatus.AppUser);
                    }
                    int doctorID = Convert.ToInt32(dt.Rows[0]["DoctorID"]);
                    int CurrentDoctor = Convert.ToInt32(dt.Rows[0]["CurrentDoctor"]);
                    int CurrentDoctorOld = Convert.ToInt32(dt.Rows[0]["CurrentDoctorOld"]);
                    string CustName = dt.Rows[0]["CustName"].ToString();
                    string CustID = dt.Rows[0]["CustID"].ToString();
                    string CustCode = dt.Rows[0]["CustCode"].ToString();
                    string OldRoom = dt.Rows[0]["OldRoom"].ToString();
                    string NewRoom = dt.Rows[0]["NewRoom"].ToString();
                    int OldRoomID = Convert.ToInt32(dt.Rows[0]["OldRoomID"].ToString());
                    int NewRoomID = Convert.ToInt32(dt.Rows[0]["NewRoomID"].ToString());

                    int OldChairID = Convert.ToInt32(dt.Rows[0]["OldChairID"].ToString());
                    int NewChairID = Convert.ToInt32(dt.Rows[0]["NewChairID"].ToString());

                    if (doctorID != 0)
                    {
                        await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,user.sys_branchID ,Convert.ToInt32(appStatus.CurrentID) ,DateTime.Now ,"change_status" ,0);
                    }

                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,user.sys_branchID ,Convert.ToInt32(appStatus.CurrentID)
                        ,Convert.ToInt32(CustID) ,CustCode ,CustName
                        ,Convert.ToInt32(DataMain.StatusID) ,doctorID ,"edit");
                    if (CurrentDoctor != CurrentDoctorOld && doctorID != CurrentDoctor && doctorID != CurrentDoctorOld)
                    {
                        int DoctorID_Main = (CurrentDoctor != 0) ? CurrentDoctor : CurrentDoctorOld;
                        await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,user.sys_branchID ,Convert.ToInt32(appStatus.CurrentID)
                        ,Convert.ToInt32(CustID) ,CustCode ,CustName
                        ,Convert.ToInt32(DataMain.StatusID) ,DoctorID_Main ,"edit");
                    }
                    if (OldRoomID != NewRoomID || OldChairID != NewChairID)
                    {
                        await NotiLocal.Noti_Parti_Appointment_Room(hubContext ,CurrentDoctor ,user.sys_branchID ,Convert.ToInt32(appStatus.CurrentID)
                                     ,Convert.ToInt32(CustID) ,CustCode ,CustName
                                     ,OldRoomID ,NewRoomID ,OldRoom ,NewRoom
                                     ,OldChairID,NewChairID
                                     ,"change");
                    }
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }
        #endregion

        #region // BackStatusApp
        [HttpPost("ChangeStatus_Back")]
        public async Task<IActionResult> ChangeStatus_Back(AppStatus appStatus)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var appID = appStatus.CurrentID;
                if (appID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("YYY_sp_Customer_Appointment_BackStatus" ,CommandType.StoredProcedure ,
                            "@appID" ,SqlDbType.Int ,appID ,
                            "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                            "@BranchID" ,SqlDbType.Int ,user.sys_branchID ,
                            "@EmployeeID" ,SqlDbType.Int ,user.sys_employeeid
                        );
                    }
                }
                if (ds != null)
                {
                    string result = ds.Tables[0].Rows[0]["RESULT"].ToString();
                    if (result == "1")
                    {
                        DataTable dt = ds.Tables[1];
                        string status = dt.Rows[0]["Status"].ToString();
                        string doctor = dt.Rows[0]["Doctor"].ToString();
                        string CustName = dt.Rows[0]["CustName"].ToString();
                        string CustID = dt.Rows[0]["CustID"].ToString();
                        string CustCode = dt.Rows[0]["CustCode"].ToString();
                        int CurrentDoctor = Convert.ToInt32(dt.Rows[0]["CurrentDoctor"].ToString());

                        int CurrentRoomID = Convert.ToInt32(dt.Rows[0]["CurrentRoomID"].ToString());
                        string CurrentRoom = dt.Rows[0]["CurrentRoom"].ToString();
                        string BackRoom = dt.Rows[0]["BackRoom"].ToString();
                        int BackRoomID = dt.Rows[0]["BackRoomID"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["BackRoomID"].ToString()) : 0;
                       
                        int CurrentChairID = dt.Rows[0]["CurrentChairID"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["CurrentChairID"].ToString()) : 0;
                        int BackChairID = dt.Rows[0]["BackChairID"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["BackChairID"].ToString()) : 0;
                        if (doctor != "0")
                        {
                            await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,user.sys_branchID ,Convert.ToInt32(appID) ,DateTime.Now ,"change_status" ,0);
                        }
                        await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,user.sys_branchID ,Convert.ToInt32(appID)
                            ,Convert.ToInt32(CustID) ,CustCode ,CustName
                            ,Convert.ToInt32(status != "" ? status : "1")
                            ,Convert.ToInt32(doctor)
                            ,"edit");

                        if (BackRoomID != CurrentRoomID || (BackRoomID == CurrentRoomID && CurrentChairID != BackChairID))
                        {
                            await NotiLocal.Noti_Parti_Appointment_Room(hubContext ,CurrentDoctor ,user.sys_branchID ,Convert.ToInt32(appID)
                                          ,Convert.ToInt32(CustID) ,CustCode ,CustName
                                         ,CurrentRoomID ,BackRoomID ,CurrentRoom ,BackRoom
                                         ,CurrentChairID,BackChairID 
                                         ,"change");
                        }
                        return Content(status);
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                else return Content("0");


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion
    }
}
