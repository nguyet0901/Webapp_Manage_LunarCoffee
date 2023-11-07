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

namespace MLunarCoffee.Pages.Customer
{
    public class CustomerImageByFolderModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public string _FolderID { get; set; }
        public string _TreatPlanID { get; set; }
        public void OnGet()
        {

            var folderid = Request.Query["FolderID"];
            var treatplanid = Request.Query["TreatPlanID"];

            _FolderID = folderid.ToString();
            _TreatPlanID = (treatplanid != "") ? treatplanid.ToString() : "";
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }

        
     
    }
}
