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

namespace MLunarCoffee.Pages.Customer.Payment.PaymentList
{

    public class PaymentList_CommissionCardModel : PageModel {
        public void OnGet ( ) {

        }

         public async Task<IActionResult> OnPostLoadataTabCommissionCard ( int CustomerID, string Limit, string BeginID) {
            DataTable dt1 = new DataTable ();
            var user = Session.GetUserSession (HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                dt1 =await  confunc.ExecuteDataTable ("[MLU_sp_Customer_Card_Commission_LoadList]", CommandType.StoredProcedure,
                    "@Customer_ID", SqlDbType.Int, CustomerID
                    , "@UserId", SqlDbType.Int, user.sys_userid
                    , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                    , "@Limit", SqlDbType.Int, Convert.ToInt32(Limit)
                    , "@BeginID", SqlDbType.Int, Convert.ToInt32(BeginID)
                    );
            }

            if (dt1 != null) {
                return Content(Comon.DataJson.Datatable(dt1));
            }
            else {
                return Content ("");
            }
        }

         public async Task<IActionResult> OnPostDeleteItemCommissionCard ( int id ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                    await connFunc.ExecuteDataTable ("[MLU_sp_Customer_Card_Commission_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content ("1");
            }
            catch (Exception ex) {
                return Content ("0");
            }
        }

    }
}
