using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;

namespace MLunarCoffee.Controllers.Data
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataPrintController : Controller
    {

        private string BaseUrl = "https://api-data.vttechsolution.com/";
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public DataPrintController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("GetTypeList")]
        [HttpPost]
        public async Task<IActionResult> GetTypeList()
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
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Print/GetTypeList" ,null);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                    }
                    return Content("[]");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("GetComandList")]
        [HttpPost]
        public async Task<IActionResult> GetComandList()
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
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Print/GetComandList" ,null);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                    }
                    return Content("[]");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("GetList/{isDenCos}")]
        [HttpPost]
        public async Task<IActionResult> GetList(int isDenCos)
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
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync($"api/Print/GetList/{isDenCos}" ,null);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                    }
                    return Content("[]");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("GetItem/{code}")]
        [HttpPost]
        public async Task<IActionResult> GetItem(string code)
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
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync($"api/Print/GetItem/{code}" ,null);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                    }
                    return Content("[]");
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
