using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class TokenRegister
    {
        public string Device { get; set; }
        public string User { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string VersionString { get; set; }
    }
}