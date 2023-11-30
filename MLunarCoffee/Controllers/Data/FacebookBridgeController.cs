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
using MLunarCoffee.Models.ClientBridge;

namespace MLunarCoffee.Controllers.Data
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacebookBridgeController : Controller
    {
        //private string BaseUrl = "https://localhost:44306/";
        private string BaseUrl = "https://api-data.vttechsolution.com/";
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        public FacebookBridgeController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("SubcribeFanpage")]
        [HttpPost]
        public async Task<IActionResult> SubcribeFanpage(FbFanpagePara fanpage)
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
                    FbFanpage fbFanpage = new FbFanpage()
                    {
                        ClientName = Convert.ToString(Global.CLIENTNAME) ,
                        ClientID = Global.sys_DB_ID ,
                        Keypage = Convert.ToString(fanpage.Keypage) ,
                        LongAccess = Convert.ToString(fanpage.LongAccess) ,
                        PageLink = Convert.ToString(fanpage.PageLink) ,
                        PageName = Convert.ToString(fanpage.PageName) ,
                        Token = Convert.ToString(fanpage.Token) ,

                        UserID = fanpage.UserID ,
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fbFanpage) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Fanpage/Subscribe" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return Content("1");
                        }
                        return Content("0");
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("UnsubcribeFanpage")]
        [HttpPost]
        public async Task<IActionResult> UnsubcribeFanpage(FbFanpagePara fanpage)
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
                    FbFanpage fbFanpage = new FbFanpage()
                    {
                        ClientName = Convert.ToString(Global.CLIENTNAME) ,
                        ClientID = Global.sys_DB_ID ,
                        Keypage = Convert.ToString(fanpage.Keypage) ,
                        UserID = fanpage.UserID ,
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fbFanpage) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Fanpage/Unsubscribe" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return Content("1");
                        }
                        return Content("0");
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("LoadConversation")]
        [HttpPost]
        public async Task<IActionResult> LoadConversation(ConverInfo conver)
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
                    var content = new StringContent(JsonConvert.SerializeObject(conver) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/Loaddata" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody != "0")
                        {
                            return Content(responseBody);
                        }
                        return Content("0");
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("Conver")]
        [HttpPost]
        public async Task<int> Conver(FbConverPara fbConverPara)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    FbConver fbFanpage = new FbConver()
                    {

                        ClientID = Global.sys_DB_ID ,
                        KeyPage = fbConverPara.KeyPage ,
                        items = fbConverPara.items
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fbFanpage) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/Upload" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Authorize]
        [Route("Star")]
        [HttpPost]
        public async Task<int> Star(FbConverAction action)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(action) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/Star" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Authorize]
        [Route("Markread")]
        [HttpPost]
        public async Task<int> Markread(FbConverAction action)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(action) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/Markread" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Authorize]
        [Route("MarkSent")]
        [HttpPost]
        public async Task<int> MarkSent(FbConverAction action)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(action) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/MarkSent" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Authorize]
        [Route("MarkResponse")]
        [HttpPost]
        public async Task<IActionResult> MarkResponse(FbConverMessAction action)
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
                    var content = new StringContent(JsonConvert.SerializeObject(action) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/MarkResponse" ,content);
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

        [Authorize]
        [Route("Hide")]
        [HttpPost]
        public async Task<int> Hide(FbConverAction action)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(action) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/Hide" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Authorize]
        [Route("UpdateTag")]
        [HttpPost]
        public async Task<int> UpdateTag(FbTag tag)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return 0;
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(tag) ,Encoding.UTF8 ,"application/json");
                    string shareKey = DateTime.Now.ToString("yyyyMMdd");
                    string AccessToken = Encrypt.EncryptString(shareKey ,Settings.SemiSecret);
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
                    {
                        clientautho.DefaultRequestHeaders.Add("AccessToken" ,AccessToken);
                        var result = await clientautho.PostAsync("api/Conver/UpdateTag" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        if (responseBody == "1")
                        {

                            return 1;
                        }
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
