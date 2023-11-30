using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Facebook
{

    public class Profile
    {
        public string id { get; set; } 
        public string first_name { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string profile_pic { get; set; }
    }
}