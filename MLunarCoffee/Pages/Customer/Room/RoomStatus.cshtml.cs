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

namespace MLunarCoffee.Pages.Customer.Room
{
 
    public class RoomStatusModel : PageModel {
        public string _versionofWebApplication { get; set; }
        public string layout { get; set; }
        public void OnGet ( ) {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            _versionofWebApplication = Comon.Global.sys_DBVersion;
        }

    }
}
