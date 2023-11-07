using System.ComponentModel.DataAnnotations;

namespace MLunarCoffee.Models.ClientBridge
{
    public class BranchTimeExtention
    {
        [Required]
        public int Time { get; set; }
    }
}
