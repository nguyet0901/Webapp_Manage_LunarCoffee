using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class ChangeRoomModel : PageModel
    { 
        public string _AppDetailID { get; set; }
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
         
         public async Task<IActionResult> OnPostInitializeData(string CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_RoomStatus_Choosing_Room]", CommandType.StoredProcedure
                        , "@Branchid", SqlDbType.Int, user.sys_branchID
                        , "@appid", SqlDbType.Int, CurrentID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
         
         public async Task<IActionResult> OnPostExcuteData(string RoomID,string Chairid, string CurrentID)
        {
            try
            {
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
                        dt =await connFunc.ExecuteDataTable("[MLU_sp_Appoinment_SetUpRoom]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@RoomID", SqlDbType.Int, RoomID,
                             "@Chairid" ,SqlDbType.Int ,Chairid ,
                            "@Created_By" , SqlDbType.Int, user.sys_userid 
                        );
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
