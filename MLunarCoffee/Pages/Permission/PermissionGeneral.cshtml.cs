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

namespace MLunarCoffee.Pages.Permission
{
    public class PermissionGeneralModel : PageModel
    {
        public string _versionofWebApplication { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            _versionofWebApplication = Comon.Global.sys_DBVersion;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }
         public async Task<IActionResult> OnPostLoadFunction()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt =await connFunc.ExecuteDataTable("[YYY_sp_AuthorizeFunction]", CommandType.StoredProcedure);

                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
