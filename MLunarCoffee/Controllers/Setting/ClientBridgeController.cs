using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Models;
using MLunarCoffee.Models.ClientBridge;

namespace MLunarCoffee.Controllers.Setting
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClientBridgeController : Controller
    {

        #region // Extend Branch
        [ServiceFilter(typeof(AuthorizeIPAddressAttribute))]
        [Route("ExtentBranchTime")]
        [HttpPost]
        public async Task<IActionResult> ExtentBranchTime(BranchTimeExtention bte)
        {
            try
            {
                //string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                //var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.SemiSecret);
                //if (KeyCode != Global.APICODE)
                //{
                //    return StatusCode(StatusCodes.Status403Forbidden);
                //}
                //var result = await Task.Factory.StartNew(async () =>
                //{
                //    using (var confunc = new ExecuteDataBase())
                //    {
                //        var dt = new DataTable();
                //        dt = await confunc.ExecuteDataTable("[MLU_sp_API_Branch_UseTime]" ,CommandType.StoredProcedure
                //            ,"@Month" ,SqlDbType.Int ,bte.Time);
                //        return DataJson.GetFirstValue(dt);
                //    }
                //});
                return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [ServiceFilter(typeof(AuthorizeIPAddressAttribute))]
        [Route("GetExtentBranchTime")]
        [HttpGet]
        public async Task<IActionResult> GetExtentBranchTime()
        {
            try
            {
                return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion

        #region //Diagnose Template
        [ServiceFilter(typeof(AuthorizeIPAddressAttribute))]
        [Route("UploadDiagnoseTemplate")]
        [HttpPost]
        public async Task<IActionResult> UploadDiagnoseTemplate([FromForm] DiagnoseTemplate diagnoseTemplate)
        {
            try
            {
                return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion

        #region // Endpoint 
        [ServiceFilter(typeof(AuthorizeIPAddressAttribute))]
        [Route("EndpointsAllow")]
        [HttpPost]
        public async Task<IActionResult> EndpointsAllow()
        {

            try
            {
                return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion

    }
}
