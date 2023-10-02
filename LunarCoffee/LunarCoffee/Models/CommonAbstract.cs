using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LunarCoffee.Models
{
    public class CommonAbstract
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifieddBy { get; set; }
    }
}