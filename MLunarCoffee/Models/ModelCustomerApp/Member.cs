using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{

    public class Member
    {
        public string Name { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public string ColorCode { get; set; }
        public string Image { get; set; }
        public string BackgroundGradientStart { get; set; }
        public string BackgroundGradientEnd { get; set; }

        
    }
}