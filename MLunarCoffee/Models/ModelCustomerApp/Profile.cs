using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{

    public class Profile
    {
        public string Userid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Customerid { get; set; }
        public string Customercode { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Aboutuser { get; set; }
        public int Totalpointused { get; set; }
        public decimal Totalamountcard { get; set; }
        public decimal Totalamountpayment { get; set; }
        public string Totalcheckedin { get; set; }
        public string Totaltab { get; set; }
        public string Totalcard { get; set; }
        public string Totalpoint { get; set; }
        
        public string NotiString { get; set; }
    }
}