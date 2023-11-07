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

namespace MLunarCoffee.Pages.Dash
{
    //[ResponseCache(Duration = 86400)]
    public class Dash_MasterModel : PageModel {
        public void OnGet ( ) {

        }

       
         public async Task<IActionResult> OnPostLoadataDashMaster ( ) {
            var user = Session.GetUserSession (HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                dt = await confunc.ExecuteDataTable ("[YYY_sp_Dash_Manage_LoadByGroup]", CommandType.StoredProcedure
                    , "@GroupID", SqlDbType.Int, user.sys_RoleID);
            }
            if (dt != null) {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else {
                return Content ("[]");
            }
        }
    }
}
