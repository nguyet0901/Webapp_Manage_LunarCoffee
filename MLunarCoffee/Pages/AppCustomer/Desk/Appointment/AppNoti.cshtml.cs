using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.AppCustomer.Desk.Appointment
{
    public class AppNotiModel : PageModel
    {
        public string _CustomerID { get; set; }

        public void OnGet()
        {
            string custid = Request.Query["CustomerID"];
            _CustomerID = custid != null ? custid.ToString() : "0";
        }
    }
}
