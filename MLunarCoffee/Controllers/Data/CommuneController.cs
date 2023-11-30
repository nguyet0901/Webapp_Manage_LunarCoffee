using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models.Data;
namespace MLunarCoffee.Controllers.Data
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommuneController : Controller
    {
        //private string BaseUrl = "https://api-data.vttechsolution.com/";
        private string BaseUrl = "https://localhost:5001/";
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public CommuneController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        [Authorize]
        [Route("GetList")]
        [HttpPost]
        public async Task<IActionResult> GetGetList(Commune commune)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    var content = new StringContent(JsonConvert.SerializeObject(commune) ,Encoding.UTF8 ,"application/json");

                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Commune/GetList" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                        return Content("[]");
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}

