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
using Microsoft.AspNetCore.Http;

namespace MLunarCoffee.Pages.Disease
{
 
    public class DentistChartTeethDetailModel : PageModel {
        public string _CurrentID { get; set; }

        public void OnGet ( ) {
            var curr = Request.Query["CurrentID"];
            if (curr.ToString () != String.Empty) {
                _CurrentID = curr.ToString ();
               
            }
            else {
                _CurrentID = "0";
            }
        }

         public async Task<IActionResult> OnPostLoadata ( int id ) {
            DataTable dt = new DataTable ();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                dt =await  confunc.ExecuteDataTable ("[MLU_sp_Dentist_Chart_Teeth_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32 (id == 0 ? 0 : id));
            }
            if (dt != null) {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else {
                return Content("0");
            }

        }
 
         public async Task<IActionResult> OnPostExcuteData ( string TeethName,  string CurrentID ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                    DataTable dt =await connFunc.ExecuteDataTable ("[MLU_sp_Dentist_Chart_Teeth_Update]", CommandType.StoredProcedure,
                        "@TeethName", SqlDbType.NVarChar, TeethName.Replace ("'", "").Trim (),
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                    if (dt.Rows.Count > 0) {
                        if (dt.Rows[0][0].ToString () != "0") {
                            return Content ("0");
                        }
                        else {
                            return Content ("1");
                        }
                    }
                    else {
                        return Content ("1");
                    }
                }

            }
            catch (Exception ex) {
                return Content ("0");
            }
        }
    }
}
