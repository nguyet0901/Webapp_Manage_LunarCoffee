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
    public class DashTicketFollowModel : PageModel {
        public void OnGet ( ) {
        }
        
         public async Task<IActionResult> OnPostLoadata ( string dateFrom, string dateTo ) {
            try {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                    dt =await  confunc.ExecuteDataTable ("[YYY_sp_Dash_Follow]", CommandType.StoredProcedure,
                         "@StaffID", SqlDbType.Int, user.sys_userid
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateTo)
                    );
                }

                if (dt != null) {
                    return Content(Comon.DataJson.Datatable(dt));
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
