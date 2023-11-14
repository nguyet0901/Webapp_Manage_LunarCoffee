using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeePage.Comon;
using CoffeePage.Comon.Crypt;
using CoffeePage.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoffeePage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [Authorize]
        [Route("GetList")]
        [HttpPost]
        public async Task<IActionResult> GetList(ProductPara para)
        {
            try
            {
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                string AccessToken = Encrypt.EncryptString(shareKey, Global.PrivateKey);
                var content = new StringContent(JsonConvert.SerializeObject(para), Encoding.UTF8, "application/json");
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.RootLink) })
                {
                    clientautho.DefaultRequestHeaders.Add("AccessToken", AccessToken);
                    var result = await clientautho.PostAsync("api/Product/GetList", content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    if (responseBody != "0")
                    {
                        return Content(responseBody);
                    }
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
