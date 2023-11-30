using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Facebook
{
    public class User
    {
        public string accessToken { get; set; }
        public string data_access_expiration_time { get; set; }
        public string expiresIn { get; set; }
        public string userID { get; set; }
        public string status { get; set; }
    }
}