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

namespace MLunarCoffee.Pages.Customer.History
{

    public class HistoryList_SMSModel : PageModel {
        public int _SMSHistoryCustomerID { get; set; }
        public int _SMSHistoryTicketID { get; set; }
        public void OnGet ( ) {
            try {
                var _cus = Request.Query["CustomerID"];
                var _tic = Request.Query["TicketID"];

                _SMSHistoryTicketID = Convert.ToInt32 (_tic.ToString () == String.Empty ? "0" : _tic.ToString ());
                _SMSHistoryCustomerID = Convert.ToInt32 (_cus.ToString () == String.Empty ? "0" : _cus.ToString ());

            }
            catch (Exception ex) {
                Response.Redirect ("/assests/Error/index.html");
            }
        }

         public async Task<IActionResult> OnPostLoadSMSHistory ( string TicketID, string CustomerID ) {
            try {
                DataTable dt = new DataTable ();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                    dt =await  confunc.ExecuteDataTable ("[YYY_sp_Ticket_GetInfo_LoadSMS]", CommandType.StoredProcedure,
                      "@Ticket", SqlDbType.Int, Convert.ToInt32 (TicketID)
                      , "@Customer_ID", SqlDbType.Int, Convert.ToInt32 (CustomerID));
                }
                if (dt != null) {
                    return Content (Comon.DataJson.Datatable(dt));

                }
                else {
                    return Content ("");
                }
            }
            catch (Exception ex) {
                return Content ("");
            }

        }
    }
}
