using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Branch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Suggest_Location { get; set; }
        public string Address { get; set; }
        public string CallCenter { get; set; }
        public string TimeOpen { get; set; }
        public string ImageJson { get; set; }
        public string Note { get; set; }
    }
    public class TimePeriod
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Rating_Branch
    {
        public int Rating { get; set; }
    }
}