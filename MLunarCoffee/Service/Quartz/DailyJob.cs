using System;

namespace MLunarCoffee.Service.Quartz
{
    public class DailyJob
    {
        public DailyJob (Type type)
        {
            //Common.Logs("MyJob at"+DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")
            //    ,"MyJob"+DateTime.Now.ToString("hhmmss"));
            Type = type;
        }
        public Type Type{ get; set; }
    }
}
