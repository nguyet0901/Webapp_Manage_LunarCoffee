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

namespace MLunarCoffee.Pages.Customer
{ 
    public class TakeCareDetailModel : PageModel {
        public string _TakeCareCustomerID { get; set; }
        public string _TakeCareDate { get; set; }
        public string _TreatID { get; set; }
        public void OnGet ( ) {
            var cus = Request.Query["CustomerID"];
            var date = Request.Query["Date"];
            var treatID = Request.Query["TreatID"];
            if (cus.ToString() != String.Empty) {
                _TakeCareCustomerID = cus.ToString();
                _TakeCareDate = date.ToString();
                _TreatID = treatID.ToString();
            }
            else {
                _TakeCareCustomerID = "0";
                _TakeCareDate = "";
                _TreatID = "0";
                Response.Redirect ("/assests/Error/index.html");
            }
        }
         public async Task<IActionResult> OnPostExcuteData ( string Date, string CustomerID, string content, string DateTreat, string TreatID ) {
            try {
                content = (content != null) ? content : "";
                var user = Session.GetUserSession (HttpContext);
                if (CustomerID != null) {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        await connFunc.ExecuteDataTable ("YYY_sp_CustomerCare_InsertValue_Treatment_Create", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                            "@Type", SqlDbType.Int, 5,
                            "@Created_By", SqlDbType.Int, user.sys_userid, 
                            "@Content", SqlDbType.NVarChar, content.Replace ("'", "").Trim (),
                            "@Typeinput", SqlDbType.Int, 88,
                            "@DateTreat", SqlDbType.DateTime, DateTreat,
                            "@TreatID", SqlDbType.Int, Convert.ToInt32(TreatID),
                            "@TimeCallBack", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD (Date).AddHours (9));
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
