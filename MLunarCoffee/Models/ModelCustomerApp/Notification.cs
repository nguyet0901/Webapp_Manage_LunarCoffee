using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Notification
    {
        public string UserID { get; set; }
        public string CustomerID { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string BranchID { get; set; }
        public string NotiID { get; set; }
        public string TimePeriod { get; set; }
        
    }
    public class AppointmentItem
    {
        public string UserID { get; set; }
        //public string CustomerID { get; set; }
        //public string Type { get; set; }
        public string Message { get; set; }
        public string BranchID { get; set; }
        //public string NotiID { get; set; }
        public string TimePeriod { get; set; }
        public DateTime DateAppoint { get; set; }
        public string ServiceString { get; set; }

    }
}