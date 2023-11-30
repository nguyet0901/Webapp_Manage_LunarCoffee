using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Student.Course
{
    public class CourseClassTimedow : PageModel
    {
        public string Mindate { get; set; }
        public string Maxdate { get; set; }
        public string ID { get; set; }
        public string MainTeacher { get; set; }
        public void OnGet()
        {
            ID = Request.Query["ID"];
            Mindate = Request.Query["Mindate"];
            MainTeacher = Request.Query["MainTeacher"];
            Maxdate = Request.Query["Maxdate"];
        }
    }
}
