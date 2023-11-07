using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TopicController : ControllerBase
    {
        const string baseaddress = "https://iid.googleapis.com";
        const string FCMPrefix = "Facebook";
        public TopicController(IWebHostEnvironment env)
        {
  
        }
         
 
        
        [Authorize]
        [HttpPost("RegisterTopic")]
        public async Task<IActionResult> RegisterTopic(TopicFB topicFB)
        {
            try
            {
                string topic = FCMPrefix  + topicFB.pageid;
                string uri = "/iid/v1/" + topicFB.token + "/rel/topics/" + topic;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(baseaddress) })
                {
                    clientautho.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key" ,"=" + Global.fcm_server_key);
                    var result = await clientautho.PostAsync(uri ,null);
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        [Authorize]
        [HttpPost("UnregisterTopic")]
        public async Task<IActionResult> UnregisterTopic(TopicFB topicFB)
        {
            try
            {
                string topic = FCMPrefix + "/" + topicFB.pageid;
                string uri = "/iid/v1/" + topicFB.token + "/rel/topics/" + topic;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(baseaddress) })
                {
                    clientautho.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key" ,"=" + Global.fcm_server_key);
                    var result = await clientautho.DeleteAsync(uri);
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }


    }
}
