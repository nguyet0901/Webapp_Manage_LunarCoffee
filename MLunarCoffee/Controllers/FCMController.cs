using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using System;
using System.Threading.Tasks;
using MLunarCoffee.Models.ModelCustomerApp;
using System.Net.Http;
using MLunarCoffee.Comon;
using Newtonsoft.Json;
using System.Text;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FCMController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public FCMController(IConfiguration config,ITokenService tokenService)
        {
            _config=config;
            _tokenService=tokenService;
        }

        [Authorize]
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> Send(FCMNoti info)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session,"Token");
                if(token==null)
                {
                    return Content("0");
                }
                if(_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(),token))
                {
 
                    using(HttpClient clientautho = new HttpClient() { BaseAddress=new Uri(Global.sys_APIClient.Url) })
                    {
                        clientautho.DefaultRequestHeaders.Add("SecretKey",Global.fcm_server_key);
                        clientautho.DefaultRequestHeaders.Add("Authorization","Bearer "+user.sys_TokenAPI);
                        var content = new StringContent(JsonConvert.SerializeObject(info),Encoding.UTF8,"application/json");
                        var result = await clientautho.PostAsync("/api/FCMWeb/Send",content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        return Content(responseBody);
                    }
                    
                }
                return Content("0");
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("MobileNoti")]
        [HttpPost]
        public async Task<IActionResult> MobileNoti(AppNotiIns notiIns)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {

                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                    {
                        clientautho.DefaultRequestHeaders.Add("SecretKey", Global.mobile_secretkey);
                        clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                        var content = new StringContent(JsonConvert.SerializeObject(notiIns), Encoding.UTF8, "application/json");
                        var result = await clientautho.PostAsync("/api/FCM/FCMNoti", content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        return Content(responseBody);
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
        [Route("MobileTopic")]
        [HttpPost]
        public async Task<IActionResult> MobileTopic(AppNotiTopic notiIns)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {

                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                    {
                        clientautho.DefaultRequestHeaders.Add("SecretKey", Global.mobile_secretkey);
                        clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                        var content = new StringContent(JsonConvert.SerializeObject(notiIns), Encoding.UTF8, "application/json");
                        var result = await clientautho.PostAsync("/api/FCM/FCMNotiTopic", content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        return Content(responseBody);
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
