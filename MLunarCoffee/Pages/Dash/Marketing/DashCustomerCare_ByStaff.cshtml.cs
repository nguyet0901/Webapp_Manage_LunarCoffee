using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace SourceMain.Pages.Dash.Marketing
{
    //[ResponseCache(Duration = 86400)]
    public class DashCustomerCare_ByStaffModel : PageModel {
        public void OnGet ( ) {
        }

     
         public async Task<IActionResult> OnPostLoadata ( string StaffID, string dateFrom, string dateTo ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                DataSet ds = new DataSet ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                    ds = await confunc.ExecuteDataSet ("[YYY_sp_Dash_CustomerCare]", CommandType.StoredProcedure,
                         "@BranchID", SqlDbType.Int, user.sys_branchID
                        , "@StaffID", SqlDbType.Int, user.sys_userid
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (dateTo)
                    );
                }

                if (ds != null) {
                    return Content (JsonConvert.SerializeObject (ds, Formatting.Indented));
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
