using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class PushNotification
    {
        public string ID { get; set; }
        public string ArticleID { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}