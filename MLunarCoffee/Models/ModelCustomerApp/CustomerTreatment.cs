using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class CustomerTreatment
    {
        public string TreatName { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
        public string TeethTreat { get; set; }
        public string BrandName { get; set; }
        public int LastPercent { get; set; }
        public int TimeHaving { get; set; }
        public int TimeDone { get; set; }
        public string StageName { get; set; }
        public string AssistName { get; set; }
        public string ResultName { get; set; }
    }
}