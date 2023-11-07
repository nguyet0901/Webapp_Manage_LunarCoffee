using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Appointment
    {
        public string Id { get; set; }
        public DateTime DateFrom { get; set; }
        public string ServiceTreatment { get; set; }
        public string ServiceCare { get; set; }
        public string ServiceChoose { get; set; }
        public string BranchName { get; set; }
        public string TypeName { get; set; }
        public string TypeID { get; set; }
        public string Content { get; set; }
        public string PeriodTime { get; set; }
        public string Confirm { get; set; }
    }
}