using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Lock
{
    public class Lockfunction : PageModel
    {

        public void OnGet()
        {


        }
        public async Task<IActionResult> OnPostExecuted(string password)
        {
            try
            {
           
                string syspass = Encrypt.DecryptString(Global.sys_ClientSystempass, Settings.SemiSecret);
                if (password == syspass)
                {
                   string crypt = Encrypt.EncryptString(password+DateTime.Now.ToString("yyyyMMdd"), Settings.SemiSecret);
                   return Content(crypt);
                }
                else return Content("0");
            }
            catch (Exception ex)
            {

                return Content("0");
            }


        }

    }
}
