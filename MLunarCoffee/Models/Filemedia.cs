using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Models
{
    public class Filemedia
    {
        [Required]
        public string Link { get; set; }

        [Required]
        public string Type { get; set; }
    }
  
}