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

namespace MLunarCoffee.Pages.Dash.Revenue.CashOther
{
    //[ResponseCache(Duration = 86400)]
    public class CashOtherListModel : PageModel {
        public void OnGet ( ) {

        }
         
         public async Task<IActionResult> OnPostLoadata ( ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                DataSet ds = new DataSet ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {

                    ds = await confunc.ExecuteDataSet ("[MLU_sp_Dash_Revenues_Expenditures_Load]", CommandType.StoredProcedure,
                        "@datefrom", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow ()
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow ()
                        , "@BranchID", SqlDbType.DateTime, Convert.ToInt32 (user.sys_branchID));

                }
                if (ds != null) {
                    return Content (JsonConvert.SerializeObject (ds, Formatting.Indented));
                }
                else {
                    return Content ("[]");
                }
            }
            catch (Exception) {
                return Content("[]");
            }

        }
    }
}
