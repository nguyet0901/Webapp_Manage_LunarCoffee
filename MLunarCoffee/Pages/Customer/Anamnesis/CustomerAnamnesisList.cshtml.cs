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

namespace MLunarCoffee.Pages.Customer.Anamnesis
{

    public class CustomerAnamnesisListModel : PageModel {
        public string _CustomerID { get; set; }
        public void OnGet ( ) {
            var CurrentID = Request.Query["CustomerID"];
            if (CurrentID.ToString () != String.Empty) {
                _CustomerID = CurrentID.ToString ();
            }
            else {
                Response.Redirect ("/assests/Error/index.html");
            }
        }
         public async Task<IActionResult> OnPostLoadataPatientHistory ( string CustomerID ) {
            DataTable dt = new DataTable ();
            var user = Session.GetUserSession (HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                dt =await  confunc.ExecuteDataTable ("[MLU_sp_Customer_Anamnesis_LoadList]", CommandType.StoredProcedure,
                  "@Customer_ID", SqlDbType.Int, CustomerID,
                  "@UserId", SqlDbType.Int, user.sys_userid
                  , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
            }
            if (dt != null) {
                return Content (Comon.DataJson.Datatable(dt));
            }
            else {
                return Content ("");
            }
        }
         public async Task<IActionResult> OnPostDeleteItem ( int id ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                    await connFunc.ExecuteDataTable ("[MLU_sp_Customer_Anamnesis_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow (),
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
