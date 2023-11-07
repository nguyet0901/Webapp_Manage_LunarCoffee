using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;


namespace MLunarCoffee.Pages.Appointment
{
    public class AppointmentCancelDetailModel : PageModel
    {
        public string _DataReasonCancel { get; set; }
        public string _AppDetailID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public AppointmentCancelDetailModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _AppDetailID = curr.ToString();
            }
            else
            {
                _AppDetailID = "0";
            }
        }
         public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("YYY_sp_Appointment_LoadReasonCancel", CommandType.StoredProcedure);
                }
                if (dt != null && dt.Rows.Count != 0)
                    return Content(Comon.DataJson.Datatable(dt));
                else
                    return Content("");

            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData (string data, string CurrentID)
        {

            try
            {
                AppCancel DataMain = JsonConvert.DeserializeObject<AppCancel>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    return Content("0");
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("[YYY_sp_Appoinment_Cancel]", CommandType.StoredProcedure,
                          "@CurrentID", SqlDbType.Int, CurrentID,
                          "@Status", SqlDbType.Int, DataMain.StatusID,
                          "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                          "@Created_By", SqlDbType.Int, user.sys_userid
                    );
                    }
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        string branchid = dt.Rows[0]["Branch"].ToString();
                        string doctor = dt.Rows[0]["Doctor"].ToString();
                        string CustName = dt.Rows[0]["CustName"].ToString();
                        string CustID = dt.Rows[0]["CustID"].ToString();
                        string CustCode = dt.Rows[0]["CustCode"].ToString();
                        string RoomID = dt.Rows[0]["RoomID"].ToString();
                        DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                        DateTime DateFrom_Old = Convert.ToDateTime(dt.Rows[0]["OldDate"]).Date;
                        if (doctor != "0")
                        {
                            await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext,0, Convert.ToInt32(branchid), Convert.ToInt32(CurrentID), DateFrom_Old, "cancel", 0);
                        }
                        if (DateFrom_Old == DateNow)
                        {
                            await NotiLocal.Noti_Parti_Appointment_Branch(hubContext,Convert.ToInt32(branchid), Convert.ToInt32(CurrentID)
                                , Convert.ToInt32(CustID), CustCode, CustName
                                , 0, Convert.ToInt32(doctor), "cancel"
                            );
                            if (Convert.ToInt32(RoomID) != 0)
                            {
                                //Comon.PushNoti.Noti_Parti_Appointment_Room(Convert.ToInt32(branchid), Convert.ToInt32(CurrentID)
                                //, Convert.ToInt32(CustID), CustCode, CustName
                                //, Convert.ToInt32(RoomID), "cancelbook");
                            }
                        }
                    }

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

    }
    public class AppCancel
    {
        public int StatusID { get; set; }
        public string Content { get; set; }


    }

}
