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

namespace MLunarCoffee.Pages.Disease
{
 
    public class DentistChartTeethListModel : PageModel {
        public void OnGet ( ) {
        }

      
         public async Task<IActionResult> OnPostLoadData ( ) {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_Teeth_LoadList]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
    }
}
