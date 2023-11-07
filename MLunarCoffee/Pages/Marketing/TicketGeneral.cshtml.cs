using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketGeneralModel : PageModel
    {
        private string key = "Marketing";
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
        public IActionResult OnPostInitialize()
        {
            try
            {
                string _dtPermissionCurrentPage = HttpContext.Request.Path;
                DataTable dt = Comon.Permission.Get_Control_IsShow_ByCurrentpage(
                    Comon.Permission.SelectDataFromKey(Comon.Global.sys_ListGeneral, key)
                    , Comon.Global.sys_PermissionControlMustHide_Table
                    , Session.GetUserSession(HttpContext).sys_PermissionControl
                    , _dtPermissionCurrentPage
                );

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
