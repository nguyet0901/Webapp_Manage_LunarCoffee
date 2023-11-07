using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class NotificationType
    {
        public int ID { get; set; }
        public string Title_Done { get; set; }
        public string Content_Done { get; set; }
        public string Title_Failure { get; set; }
        public string Content_Failure { get; set; }
    }
}