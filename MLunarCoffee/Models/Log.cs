using System;

namespace MLunarCoffee.Models
{
    public class Log
    {
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public string JsonContent { get; set; }
    }

    public class LogSys
    {
        public string Type { get; set; } = "";
        public string Action { get; set; } = "";
        public string Exception { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateSystem { get; set; }
    }
}
