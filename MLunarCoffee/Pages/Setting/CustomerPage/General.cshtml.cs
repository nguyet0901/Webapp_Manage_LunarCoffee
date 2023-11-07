using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.CustomerPage
{
    public class GeneralModel : PageModel
    {
        private string key = "CustomerGate";

        public string layout { get; set; }

        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

        public async Task<IActionResult> OnPostLoadList()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string _dtPermissionCurrentPage = HttpContext.Request.Path;
                dt = Comon.Permission.Get_Control_IsShow_ByCurrentpage(
                    Comon.Permission.SelectDataFromKey(Comon.Global.sys_ListGeneral, key)
                    , Comon.Global.sys_PermissionControlMustHide_Table
                    , user.sys_PermissionControl
                    , _dtPermissionCurrentPage);
                dt.TableName = "Menu";
                ds.Tables.Add(dt);
                if(ds != null)
                    return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
