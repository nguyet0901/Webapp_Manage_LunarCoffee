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
 
    public class ScheduleListModel : PageModel {
        public string _SchedulerCustomerID { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet ( ) {
            var v = Request.Query["CustomerID"];
            if (v.ToString () != String.Empty) {
                _SchedulerCustomerID = v.ToString ();
                _dtPermissionCurrentPage = HttpContext.Request.Path;
                         }
            else {
                Response.Redirect ("/assests/Error/index.html");
            }
        }


    }
}
