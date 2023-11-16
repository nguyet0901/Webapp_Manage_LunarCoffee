using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using MLunarCoffee.Models;
using System.Linq;
using MLunarCoffee.Comon.Crypt;
using Newtonsoft.Json;
using MLunarCoffee.Models.Product;

namespace MLunarCoffee.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [Route("GetList")]
        [HttpPost]
        public async Task<IActionResult> GetList(ProductPara pros)
        {
            try
            {
                string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
                var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (AccessToken != shareKey)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var ds = new DataSet();
                        ds = await confunc.ExecuteDataSet("[MLU_SP_Product_GetList]" ,CommandType.StoredProcedure
                            ,"@TypeID", SqlDbType.Int,pros.TypeID
                            ,"@ProductID" , SqlDbType.Int,pros.ProductID
                            ,"@HasDisable" , SqlDbType.Int,pros.HasDisable
                            ,"@pagingNumber" , SqlDbType.Int,pros.PagingNumber
                            ,"@textSearch" , SqlDbType.NVarChar,pros.TextSearch
                            ,"@limit" , SqlDbType.Int,pros.Limit
                            );
                        return JsonConvert.SerializeObject(ds);
                    }
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("GetDetail/{id}")]
        [HttpPost]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
                var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (AccessToken != shareKey)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Product_Detail]" ,CommandType.StoredProcedure
                            ,"@ProductID" ,SqlDbType.Int ,id
                            );
                        return JsonConvert.SerializeObject(dt);
                    }
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("ExcutedData")]
        [HttpPost]
        public async Task<IActionResult> ExcutedData(ProductParaExec para)
        {
            try
            {
                string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
                var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (AccessToken != shareKey)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await Task.Factory.StartNew(async () =>
                {
                    var dt = new DataTable();
                    if (para.CurrentID == 0)
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_Product_Insert" ,CommandType.StoredProcedure);
                        }
                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_Product_Update" ,CommandType.StoredProcedure);
                        }
                    }
                    return JsonConvert.SerializeObject(dt);
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("Delete{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
                var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (AccessToken != shareKey)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Product_Delete]" ,CommandType.StoredProcedure
                            ,"@ProductID" ,SqlDbType.Int ,id
                            );
                        return JsonConvert.SerializeObject(dt);
                    }
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("Disable{id}")]
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
                var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (AccessToken != shareKey)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await Task.Factory.StartNew(async () =>
                {
                    using (var confunc = new ExecuteDataBase())
                    {
                        var dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Product_Detail]" ,CommandType.StoredProcedure
                            ,"@ProductID" ,SqlDbType.Int ,id
                            );
                        return JsonConvert.SerializeObject(dt);
                    }
                });
                return Content(await result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
