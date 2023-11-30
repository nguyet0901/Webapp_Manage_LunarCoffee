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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Dash.Marketing
{
    //[ResponseCache(Duration = 86400)]
    public class DashTicketFollow_ByStaffModel : PageModel {
        public int _UserTeleID { get; set; }
        public void OnGet ( ) {
            var user = Session.GetUserSession (HttpContext);
            _UserTeleID = Convert.ToInt32 (user.sys_userid);
        }
     
         public async Task<IActionResult> OnPostLoadata ( string StaffID, string dateFrom, string dateTo ) {
            try {
                DataTable dt = new DataTable ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                    dt =await  confunc.ExecuteDataTable ("[MLU_sp_Dash_Follow]", CommandType.StoredProcedure,
                         "@StaffID", SqlDbType.Int, StaffID
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateTo)
                    );
                }

                if (dt != null) {
                    return Content (JsonConvert.SerializeObject (dt, Formatting.Indented));
                }
                else {
                    return Content ("[]");
                }
            }
            catch (Exception) {
                return Content ("[]");
            }

        }
    }
}
