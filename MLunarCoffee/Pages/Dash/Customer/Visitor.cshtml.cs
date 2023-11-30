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

namespace MLunarCoffee.Pages.Dash.Customer
{
    //[ResponseCache(Duration = 86400)]
    public class VisitorModel : PageModel {
        public string _branch { get; set; }
        public void OnGet ( ) {
            var user = Session.GetUserSession (HttpContext);
            _branch = user.sys_branchID.ToString ();
        }

         public async Task<IActionResult> OnPostLoadata ( int branchID, string date ) {
            try {
                DataTable dt = new DataTable ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {

                    dt =await  confunc.ExecuteDataTable ("[MLU_sp_Dash_Vitor_Rate]", CommandType.StoredProcedure,
                        "@Branch", SqlDbType.DateTime, Convert.ToInt32 (branchID)
                        , "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (date)
                        , "@NumberMonth", SqlDbType.Int, 6);

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
