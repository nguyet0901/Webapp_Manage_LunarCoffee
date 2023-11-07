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

namespace MLunarCoffee.Pages.Setting.AppCustomer.Language
{
    public class AppCustomerEditLanguageModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase  conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_IniLanguage_Load]", CommandType.StoredProcedure
                        , "@ID",SqlDbType.Int, ID
                        );
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
