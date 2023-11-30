using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Desk.Doctor
{
    public class DeskDoctorModel : PageModel
    {
        public int _BranchID { get; set; }
        public string _CurrentFloor { get; set; }
        public string _CurrentRoom { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public DeskDoctorModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            _CurrentFloor = (user.sys_floorID).ToString();
            _CurrentRoom = (user.sys_roomID).ToString();
        }

        public async Task<IActionResult> OnPostLoadRoom(int RoomID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Manufacture_Room_Info]" ,CommandType.StoredProcedure
                        ,"@RoomID" ,SqlDbType.Int ,RoomID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostLoadSchedule(int RoomID, int BranchID, int AppID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Appointment_InRoom]" ,CommandType.StoredProcedure
                        ,"@AppID" ,SqlDbType.Int ,AppID
                        ,"@BranchID" ,SqlDbType.Int ,BranchID
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow()
                        ,"@RoomID" ,SqlDbType.Int ,RoomID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadHistory(int RoomID ,int BranchID )
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Deskdoc_RoomHistory]" ,CommandType.StoredProcedure
                        ,"@BranchID" ,SqlDbType.Int ,BranchID
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow()
                        ,"@RoomID" ,SqlDbType.Int ,RoomID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
