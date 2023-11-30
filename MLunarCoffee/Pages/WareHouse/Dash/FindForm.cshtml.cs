using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Dash
{
    public class FindForm : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadData(string code)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_FindForm]",CommandType.StoredProcedure

                        ,"@code",SqlDbType.NVarChar,code);
                
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
     
    }
}
