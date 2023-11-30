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
 
    public class TreatmentListModel : PageModel {
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet ( ) {
            _dtPermissionCurrentPage = HttpContext.Request.Path;
 
        }
    }
}
