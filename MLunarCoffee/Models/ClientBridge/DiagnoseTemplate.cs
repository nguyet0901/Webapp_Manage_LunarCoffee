using System.ComponentModel.DataAnnotations;

namespace MLunarCoffee.Models.ClientBridge
{
    public class DiagnoseTemplate
    {
        public string CanvasType { get; set; }
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }
        public string Feature { get; set; }
        public string SVG { get; set; }
        public int FormID { get; set; } = 0;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Image { get; set; }
        public int WidthImg { get; set; }
        public int HeightImg { get; set; }
    }
}
