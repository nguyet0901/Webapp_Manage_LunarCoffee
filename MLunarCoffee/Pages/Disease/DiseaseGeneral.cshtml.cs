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

namespace MLunarCoffee.Pages.Disease
{

    public class DiseaseGeneralModel : PageModel {
        private string key = "Disease";
         public string _imageFeature_Disease { get; set; }
        public string _dtCurrentList { get; set; }
        public string CurrentPath { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
             _imageFeature_Disease = Comon.Global.sys_HTTPImageRoot + "Sys_Disease_Feature/";
        }
        public async Task<IActionResult> OnPostGetData () {
            try {
                string _dtPermissionCurrentPage = HttpContext.Request.Path;
                DataTable dt = Comon.Permission.Get_Control_IsShow_ByCurrentpage (
                    Comon.Permission.SelectDataFromKey (Comon.Global.sys_ListGeneral, key)
                    , Comon.Global.sys_PermissionControlMustHide_Table
                    , ((GlobalUser)Session.GetUserSession (HttpContext)).sys_PermissionControl
                    , _dtPermissionCurrentPage);
               return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex) {
                return Content ("0");
            }
        }
    }
}
