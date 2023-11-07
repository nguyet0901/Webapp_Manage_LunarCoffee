using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Authentication
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResult
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
    }
    public class RegisterAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}