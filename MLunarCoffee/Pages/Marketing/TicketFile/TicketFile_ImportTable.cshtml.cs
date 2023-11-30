using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_ImportTableModel : PageModel
    {
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
        }
    }
}
