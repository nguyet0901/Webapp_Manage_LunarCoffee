using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DailyController : ControllerBase
    {
        [Authorize]
        [HttpPost("Run")]
        public async Task<IActionResult> Run()
        {
            try
            {
                
               return Content("1");
                
            }
            catch(Exception)
            {
                return Content("0");
            }
        }

       
    }
}
