using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{

    public class CustomerTab
    {
        public string Id { get; set; }//
        public string Name { get; set; }//
        public decimal Price { get; set; }//
        public int Time_Be_Treated { get; set; }
        public int Time_Can_Treat { get; set; }
        public string Type { get; set; }//
        public DateTime Created { get; set; }//
        public int PercentTreatment { get; set; }//

        public string GuideDocument { get; set; }//
        public string WarrantyDocument { get; set; }//
        public string Color_Back_War { get; set; }
        public string Color_Back_Guide { get; set; }
        public string Color_Back_Rating { get; set; }
        public string Rating { get; set; }
        
    }
}