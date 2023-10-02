using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LunarCoffee.Models
{
    public class ExportViewModel
    {
        [Required]
        public string Code { get; set; }
        public string WareName { get; set; }
        public int Quantity { get; set; }
        public int IsExport { get; set; }
        public DateTime? DateExport { get; set; }
    }
}