using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Manufacture
{
    public class Manufacture_Tree
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public bool isParent { get; set; }
    }
}