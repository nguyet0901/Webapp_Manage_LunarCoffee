using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Fanpage.Comon.Session;

namespace Fanpage.Pages.Facebook.Setting
{
    public class FacebookGeneralModel : PageModel
    {
        private string key = "Facebook";
        public string _dtCurrentList { get; set; }
        public void OnGet()
        {
        }

         public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                string _dtPermissionCurrentPage = HttpContext.Request.Path;
                DataTable dt = Comon.Permission.Get_Control_IsShow_ByCurrentpage(
                    Comon.Permission.SelectDataFromKey(Comon.Global.sys_ListGeneral, key)
                    , Comon.Global.sys_PermissionControlMustHide_Table
                    , Session.GetUserSession(HttpContext).sys_PermissionControl
                    , _dtPermissionCurrentPage);
                return Content(JsonConvert.SerializeObject(dt, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
