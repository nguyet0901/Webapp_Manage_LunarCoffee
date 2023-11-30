using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Crypt;

namespace MLunarCoffee.Pages.Login
{
    public class ScanQRInfoModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostCheckPass(string password ,string crypt)
        {
            try
            {
                string pass = Encrypt.DecryptString(crypt ,Settings.Secret);
                if (pass != "" && pass.Equals(password)) return Content("1");

                return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
