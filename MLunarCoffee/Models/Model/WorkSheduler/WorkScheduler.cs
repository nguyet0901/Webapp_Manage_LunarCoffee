using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.WorkScheduler
{

    public class WorkScheduler
    {
        public string dayofweek { get; set; }
        public Boolean off { get; set; }
        public shifts[] shift { get; set; }
    }
    public class shifts
    {
        public string shift { get; set; }
        public string branch { get; set; }
    }

    public class WorkScheduler_TimeLine
    {
        public WorkScheduler [] workScheduler { get; set; }
        public DateTime Date { get; set; }
    }

    public class WorkScheduler_Extension
    {
        public Boolean off { get; set; }
        public shifts[] shift { get; set; }
    }

    public class WorkScheduler_TimeLine_Employee
    {
        public WorkScheduler[] workScheduler { get; set; }
        public Int32 EmployeeID { get; set; }
        public DateTime Date { get; set; }
    }

    public class WorkScheduler_Extension_Employee
    {
        public Boolean off { get; set; }
        public Int32 EmployeeID { get; set; }
        public shifts[] shift { get; set; }
    }
}