using System.ComponentModel.DataAnnotations;

namespace MLunarCoffee.Models.ThirdParty.ICD
{
    public class ICDSearching
    {
        [Required]
        public string textSearch { get; set; }
        public string? MaNhom { get; set; } = "";
    }
    public class ICDToken
    {
        [Required]
        public string token { get; set; }
    }
}
