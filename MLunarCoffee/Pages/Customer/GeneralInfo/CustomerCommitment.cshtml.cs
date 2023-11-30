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

namespace MLunarCoffee.Pages.Customer.GeneralInfo
{
    //[ResponseCache(Duration = 86400)]
    public class CustomerCommitmentModel : PageModel {
        public void OnGet ( ) {
        }
         public async Task<IActionResult> OnPostLoadata ( string custID ) {
            try {
                DataTable dt = new DataTable ();
                if (custID != "0") {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        dt =await connFunc.ExecuteDataTable ("MLU_sp_Customer_Commitment_LoadList", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex) {
                return Content ("0");
            }
        }
         public async Task<IActionResult> OnPostDeleteItemCommitment ( string id ) {
            try {
                DataTable dt = new DataTable ();
                var user = Session.GetUserSession (HttpContext);
                if (id != "0") {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        dt =await connFunc.ExecuteDataTable ("MLU_sp_Customer_Commitment_Delete", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, id
                            , "@UserID", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                return Content ("1");
            }
            catch (Exception ex) {
                return Content ("0");
            }
        }

    }
}
