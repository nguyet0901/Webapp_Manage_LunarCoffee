using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Monitor
{
    public class RoomStatusModel : PageModel
    {
        public int _BranchID { get; set; }
        public string _CurrentFloor { get; set; }
        public string _CurrentRoom { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            _CurrentFloor = (user.sys_floorID).ToString();
            _CurrentRoom = (user.sys_roomID).ToString();
        }


        public async Task<IActionResult> OnPostInit(int RoomID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Branch_LoadDetail]" ,CommandType.StoredProcedure
                     ,"@ID" ,SqlDbType.Int ,user.sys_branchID);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Manufacture_Room_Info]" ,CommandType.StoredProcedure ,"@RoomID" ,SqlDbType.Int ,RoomID);
                    dt.TableName = "Room";
                    ds.Tables.Add(dt);
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadApps(int RoomID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Monitor_RoomStatus_Schedule]" ,CommandType.StoredProcedure ,"@RoomID" ,SqlDbType.Int ,RoomID);
                    dt.TableName = "CustWaiting";
                    ds.Tables.Add(dt);
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
