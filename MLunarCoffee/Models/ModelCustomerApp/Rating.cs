using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Rating
    {
        public string UserID { get; set; }
        public string Text { get; set; }
        public string Star { get; set; }
        public string Branch { get; set; }
        public string Star_Emp { get; set; }
        public string Star_Service { get; set; }
        public string TabID { get; set; }
        public string BranchID { get; set; }
        public string IsDelete { get; set; }
        public DateTime Created { get; set; }
    }
}