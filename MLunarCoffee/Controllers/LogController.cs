using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLunarCoffee.Comon.Log;
using MLunarCoffee.Models;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LogController : ControllerBase
    {
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(Log log)
        {
            try
            {
                await LogAction.InsertLog(log);
                return Content("1");

            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
