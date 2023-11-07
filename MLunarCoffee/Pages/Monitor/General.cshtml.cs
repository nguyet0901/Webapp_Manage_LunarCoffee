using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Monitor
{
    public class GeneralModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }
    }
}
