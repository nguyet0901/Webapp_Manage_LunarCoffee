using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Service
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int IsFeature { get; set; }
        public Decimal Price { get; set; }
        public DateTime Created { get; set; }
        public string BranchString { get; set; }

    }
    
    
}