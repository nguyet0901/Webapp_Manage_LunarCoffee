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
    [ResponseCache(Duration = 1)]
    public class CustomerScheduleModel : PageModel {
 
        public void OnGet ( ) {
 
        }
 
         public async Task<IActionResult> OnPostLoadata ( string custID  ) {
            try {
                DataTable dt = new DataTable ();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                    dt =await connFunc.ExecuteDataTable ("YMLU_sp_Customer_LoadInfo_App", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, custID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex) {
                return Content ("0");
            }
        }
    }
}
