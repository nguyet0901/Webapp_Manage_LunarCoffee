using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using SourceMain.Models.Model.WorkScheduler;

namespace SourceMain.Pages.Employee
{
    public class WorkSchedulerMySelfModel : PageModel
    {
        public string _dataBranch { get; set; }
        public string _dataShift { get; set; }
        public string _CurrentEmployeeID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _CurrentEmployeeID = user.sys_employeeid.ToString();
        }

         public async Task<IActionResult> OnPostLoadScheduler(int empid, string dateFrom, string dateTo)
        {
            return Content(await Comon.WorkEmployee.LoadScheduler(HttpContext,empid, dateFrom, dateTo));
        }

         public async Task<IActionResult> OnPostOffScheduler(int empid, string date)
        {
            return Content(await Comon.WorkEmployee.OffScheduler(HttpContext,empid, date));
        }
    }
}
