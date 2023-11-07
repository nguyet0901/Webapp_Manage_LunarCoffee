using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Student.Course
{
    public class CourseClassTimeDetailModel : PageModel
    {
        public string DayInt { get; set; }
        public string ID { get; set; }
        public string TimeEnd { get; set; }
        public string TimeStart { get; set; }
        public string Teacher { get; set; }
        public string Manuclass { get; set; }
        public string Mindate { get; set; }
        public string Maxdate { get; set; }
        public void OnGet()
        {
            ID = Request.Query["ID"].ToString();
            Mindate = Request.Query["Mindate"];
            Maxdate = Request.Query["Maxdate"];
            Teacher = Request.Query["Teacher"];
            if (ID != "0")
            {
                DayInt = Request.Query["DayInt"];
                TimeEnd = Request.Query["TimeEnd"];
                TimeStart = Request.Query["TimeStart"];
                
                Manuclass = Request.Query["Manuclass"];

            }
            else
            {
                DayInt = "";TimeEnd = "";TimeStart = ""  ;Manuclass = "" ;
            }
        }
    }
}
