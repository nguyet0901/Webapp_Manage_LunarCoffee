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
                string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.SemiSecret);
                if (KeyCode != Global.APICODE)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_API_Branch_UseTime]" ,CommandType.StoredProcedure
                            ,"@Month" ,SqlDbType.Int ,bte.Time);
                        return DataJson.GetFirstValue(dt);
                    }
                });
                return Content(await result);
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
                string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.SemiSecret);
                if (KeyCode != Global.APICODE)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                var dtResult = new DataTable();

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var ds = new DataSet();
                        ds = await confunc.ExecuteDataSet("[YYY_sp_API_Branch_GetUseTime]" ,CommandType.StoredProcedure);
                        return DataJson.Dataset(ds);
                    }
                });
                return Content(await result);
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
                string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.SemiSecret);
                if (KeyCode != Global.APICODE)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                var dtResult = new DataTable();

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Diagnose_Template_Insert]" ,CommandType.StoredProcedure
                            ,"@CanvasType" ,SqlDbType.NVarChar ,diagnoseTemplate.CanvasType.Replace("'" ,"").Trim()
                            ,"@Name" ,SqlDbType.NVarChar ,diagnoseTemplate.Name.Replace("'" ,"").Trim()
                            ,"@Type" ,SqlDbType.NVarChar ,diagnoseTemplate.Type
                            ,"@SVG" ,SqlDbType.NVarChar ,diagnoseTemplate.SVG
                            ,"@FormID" ,SqlDbType.Int ,diagnoseTemplate.FormID
                            ,"@Width" ,SqlDbType.Int ,diagnoseTemplate.Width
                            ,"@Height" ,SqlDbType.Int ,diagnoseTemplate.Height
                            ,"@Image" ,SqlDbType.NVarChar ,diagnoseTemplate.Image
                            ,"@WidthImg" ,SqlDbType.Int ,diagnoseTemplate.WidthImg
                            ,"@HeightImg" ,SqlDbType.Int ,diagnoseTemplate.HeightImg
                            );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Result"] != null && dt.Rows[0]["Result"].ToString() != "1")
                            {
                                return "1";
                            }
                            else
                            {
                                return "0";
                            }
                        }
                        else
                        {
                            return "0";
                        }
                    }
                });
                return Content(await result);
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
                string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                string endpoints = Request.Headers["Endpoints"].Count() > 0 ? Request.Headers["Endpoints"] : "";
                var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.SemiSecret);
                var dtAPI = JsonConvert.DeserializeObject<DataTable>(endpoints);
                if (KeyCode != Global.APICODE)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                if (dtAPI == null || dtAPI.Rows.Count == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_API_Allow_Insert]" ,CommandType.StoredProcedure
                            ,"@Data" ,SqlDbType.Structured ,dtAPI);
                        return DataJson.GetFirstValue(dt);
                    }
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion

    }
}
