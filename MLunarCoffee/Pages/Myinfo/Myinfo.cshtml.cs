using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Myinfo
{
    public class Myinfo : PageModel
    {
        
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostGetData( )
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_System_GetDataIni]",CommandType.StoredProcedure
                        ,"@UserID",SqlDbType.Int,user.sys_userid );

                }
                return Content(Comon.DataJson.Datatable(dt));

            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
