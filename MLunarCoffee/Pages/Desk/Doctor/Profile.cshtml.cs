using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static MLunarCoffee.Pages.Setting.AppCustomer.About.AboutDetailModel;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Desk.Doctor
{
    public class ProfileModel : PageModel
    {
        public string _CustomerID { get; set; } 
 
        public string _appid { get; set; }
        public string _RoomID { get; set; }

        private readonly IHubContext<NotiHub> hubContext;

        public ProfileModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string custid = Request.Query["CustomerID"];
            string appid = Request.Query["appid"];
            string roomid = Request.Query["RoomID"];
            _CustomerID = !string.IsNullOrEmpty(custid) ? custid : "0";
            _appid = !string.IsNullOrEmpty(appid) ? appid : "0";
            _RoomID = !string.IsNullOrEmpty(roomid) ? roomid : "0";
        }
 
        public async Task<IActionResult> OnPostLoadCustomer(int CustID, int AppID ,int RoomID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_ScheduleInfo]" ,CommandType.StoredProcedure
                        ,"@CustID" ,SqlDbType.Int ,CustID
                        ,"@AppID" ,SqlDbType.Int ,AppID
                        ,"@RoomID" ,SqlDbType.Int ,RoomID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostUpdateStatus(int AppID  ,int RoomID, int Type, int Status)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_ScheduleRealtime_UpdateStatus]" ,CommandType.StoredProcedure
                        ,"@Status" ,SqlDbType.Int ,Status
                        ,"@Type" ,SqlDbType.Int ,Type
                        ,"@AppID" ,SqlDbType.Int ,AppID
                        ,"@RoomID" ,SqlDbType.Int ,RoomID);
                }
                if (Type == 2)
                {
                    // Receipt patient
                    await NotiLocal.Send_MonitorRoom(hubContext, RoomID,"receipt");
                }
                else
                {
                    // Done : Move to other room
                    await NotiLocal.Send_MonitorRoom(hubContext ,RoomID ,"receipt");

                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
