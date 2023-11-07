using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Facebook
{
    public class Page
    {
        public PageDetail [] page { get; set; }
    }
    public class PageDetail
    {
        public string id { get; set; }
        public string access_token { get; set; }
        public string name { get; set; }
        public string picture { get; set; } 
        public string Process { get; set; }  
        public string Active { get; set; }
        public string State { get; set; }
        public string link { get; set; }
    }
}