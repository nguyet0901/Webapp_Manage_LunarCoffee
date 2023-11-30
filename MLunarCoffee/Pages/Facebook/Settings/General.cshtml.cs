using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Facebook.Settings.General
{
    public class GeneralModel : PageModel
    {
        public string layout { get; set; }
        public string Face_appid { get; set; }
        public string Face_version { get; set; }
        public void OnGet()
        {
            Face_appid = Comon.Global.sys_face_appid;
            Face_version = Comon.Global.sys_face_version;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }


    }
}
