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

namespace MLunarCoffee.Pages.Setting
{
    public class SyntaxListModel : PageModel
    {
        public string CurrentPath { get; set; } 
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    //ds = await connFunc.ExecuteDataTable("YYY_sp_v2_SyntaxList_load", CommandType.StoredProcedure);
                    ds = await confunc.ExecuteDataSet("[YYY_sp_v2_SyntaxList_load]", CommandType.StoredProcedure);
                }

                if (ds != null)
                {
                    //return Content(Comon.DataJson.Dataset(ds));
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            { 
                return Content("");
            }
        }
    }
}
