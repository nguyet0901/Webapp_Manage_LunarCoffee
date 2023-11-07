using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SourceMain.Pages.ToDo.WorkingProject
{
    public class TodoSettingModel : PageModel
    {
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
        }
    }
}
