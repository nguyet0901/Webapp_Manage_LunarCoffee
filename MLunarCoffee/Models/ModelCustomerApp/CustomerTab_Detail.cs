using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{

    public class CustomerTab_Detail
    {
        public string ServiceName { get; set; }//
        public string Description { get; set; }//
        public string TypeName { get; set; }//
        public string Number { get; set; }//
        public string TimeToTreatment { get; set; }//
        public string Avatar { get; set; }//

        public decimal Price { get; set; }//
        public decimal Discount { get; set; }//
        public decimal DiscountCTKM { get; set; }//
        public decimal AmountUsingCard { get; set; }//
        public string UsingCard { get; set; }//
        public decimal PriceFinal { get; set; }//
        public decimal AmountUsing { get; set; }//
        public DateTime Created { get; set; }//
        public int Point { get; set; }//
    }
}