using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{

    public class PaymentRecord
    {
        public string ID { get; set; }//
        public Decimal Amount { get; set; }//
        public Decimal AmountDepositUsing { get; set; }//
        public string Method { get; set; }//
        public string BranchName { get; set; }//
        public string Cashier { get; set; }//
        public string Content { get; set; }//
        public int Type { get; set; }//
        public int PointUse { get; set; }//
        public Decimal PriceUse { get; set; }//
        public string Sign { get; set; }//
        
        public DateTime Created { get; set; }//
     
    }
}