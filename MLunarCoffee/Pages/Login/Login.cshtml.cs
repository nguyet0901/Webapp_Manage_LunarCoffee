using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Login
{
    [BindProperties(SupportsGet = true)]
    public class LoginModel : PageModel
    {
        public string _ver { get; set; }
        public string _loginfrom { get; set; }
        public string _loginuser { get; set; }
        public string _loginpass { get; set; }
        public string _domain { get; set; }
        public void OnGet()
        {
            var ver = Request.Query["ver"];
            var loginfrom = Request.Query["loginfrom"];
            var loginuser = Request.Query["loginuser"];
            var loginpass = Request.Query["loginpass"];
            _ver = ver.ToString() != String.Empty ? ver.ToString() : "0";
            _loginfrom = loginfrom.ToString() != String.Empty ? loginfrom.ToString() : "0";
            _loginuser = loginuser.ToString() != String.Empty ? loginuser.ToString() : "";
            _loginpass = loginpass.ToString() != String.Empty ? loginpass.ToString() : "";
            _domain = HttpContext.Request.Host.Host;

        }
    }
}
