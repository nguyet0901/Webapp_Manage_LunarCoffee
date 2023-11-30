using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MLunarCoffee.Models.Model.IPAddress;
using MLunarCoffee.Comon.Crypt;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FileController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public FileController(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [Route("GetTemplate")]
        [HttpPost]
        public async Task<IActionResult> GetTemplate(Filemedia file)
        {
            try
            {
                var webRequest = WebRequest.Create(file.Link);

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent =await reader.ReadToEndAsync();
                    return Content(strContent);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
