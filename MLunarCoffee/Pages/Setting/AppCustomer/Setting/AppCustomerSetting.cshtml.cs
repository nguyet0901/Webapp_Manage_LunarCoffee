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

namespace MLunarCoffee.Pages.Setting.AppCustomer.Setting
{
    public class AppCustomerSettingModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_IniSetting_Load]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
