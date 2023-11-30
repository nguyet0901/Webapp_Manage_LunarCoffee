using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Media
{
    public class TeethChart
    {
        public int detailid { get; set; }
        public string diseaseid { get; set; }
        public string face { get; set; }
        public string pathcircle { get; set; }
        public string teethid { get; set; }
        public string type { get; set; }
        public string typecircle { get; set; }
        public string root { get; set; }
        public string userid { get; set; }
        public string note { get; set; }
        public string proposed_treat { get; set; }
        public string classification { get; set; }
    }
     public class TeethChartv2
    {
        public int Detailid { get; set; }
        public string Diseaseid { get; set; }
        public string Classification { get; set; }
        public string Teethid { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public string Part { get; set; }
       
        public string Treat { get; set; }
    }
}
