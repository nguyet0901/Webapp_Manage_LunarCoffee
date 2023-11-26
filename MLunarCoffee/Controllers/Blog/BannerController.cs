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
using MLunarCoffee.Models.Warehouse;
using MLunarCoffee.Models.Banner;

namespace MLunarCoffee.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class BannerController : Controller
    {
        [Route("GetList")]
        [HttpPost]
        public async Task<IActionResult> GetList(BannerPara pros)
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
                        ds = await confunc.ExecuteDataSet("[MLU_SP_Banner_GetList]" ,CommandType.StoredProcedure
                            ,"@BannerID" , SqlDbType.Int,pros.BannerID
                            ,"@pagingNumber" , SqlDbType.Int,pros.PagingNumber
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
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Banner_Detail]" ,CommandType.StoredProcedure
                            ,"@BannerID" ,SqlDbType.Int ,id
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

        //[Route("ExcutedData")]
        //[HttpPost]
        //public async Task<IActionResult> ExcutedData(BannerParaExec para)
        //{
        //    try
        //    {
        //        string ciphertext = Request.Headers["AccessToken"].Count() > 0 ? Request.Headers["AccessToken"] : "";
        //        var AccessToken = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
        //        string shareKey = DateTime.Now.ToString("yyyyMMdd");
        //        if (AccessToken != shareKey)
        //        {
        //            return StatusCode(StatusCodes.Status403Forbidden);
        //        }

        //        var result = await Task.Factory.StartNew(async () =>
        //        {
        //            var dt = new DataTable();
        //            Banner DataMain = JsonConvert.DeserializeObject<Banner>(para.data);
        //            Unit DataUnit = JsonConvert.DeserializeObject<Unit>(para.dataUnit);
        //            if (para.CurrentID == 0)
        //            {
        //                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //                {
        //                    dt = await connFunc.ExecuteDataTable("YYY_sp_Banner_Insert" ,CommandType.StoredProcedure
        //                        ,"@Image", SqlDbType.NVarChar,DataMain.Image
        //                        ,"@TypeID" , SqlDbType.Int,DataMain.TypeID
        //                        ,"@Media" , SqlDbType.NVarChar,DataMain.Media
        //                        ,"@Name" , SqlDbType.NVarChar,DataMain.Name
        //                        ,"@NameNosign" , SqlDbType.NVarChar,DataMain.NameNosign
        //                        ,"@Code" , SqlDbType.NVarChar,DataMain.Code
        //                        ,"@UnitID" , SqlDbType.Int,DataMain.UnitID
        //                        ,"@N1" , SqlDbType.Decimal ,DataMain.N1
        //                        ,"@N2" , SqlDbType.Decimal ,DataMain.N2
        //                        ,"@N3" , SqlDbType.Decimal,DataMain.N3
        //                        ,"@IsMaterial" , SqlDbType.Int,DataMain.IsMaterial
        //                        ,"@Property" , SqlDbType.NVarChar,DataMain.Property
        //                        ,"@Description" , SqlDbType.NVarChar,DataMain.Description
        //                        ,"@CostPrice" , SqlDbType.Decimal ,DataMain.CostPrice
        //                        ,"@PriceToSale" , SqlDbType.Decimal,DataMain.PriceToSale
        //                        ,"@Note" , SqlDbType.NVarChar,DataMain.Note
        //                        ,"@UserID" , SqlDbType.Int,1
        //                        ,"@dataUnit" , SqlDbType.Structured,DataUnit
        //                        );
        //                }
        //            }
        //            else
        //            {
        //                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //                {
        //                    dt = await connFunc.ExecuteDataTable("YYY_sp_Banner_Update" ,CommandType.StoredProcedure
        //                        ,"@Image" ,SqlDbType.NVarChar ,DataMain.Image
        //                        ,"@TypeID" ,SqlDbType.Int ,DataMain.TypeID
        //                        ,"@Media" ,SqlDbType.NVarChar ,DataMain.Media
        //                        ,"@Name" ,SqlDbType.NVarChar ,DataMain.Name
        //                        ,"@NameNosign" ,SqlDbType.NVarChar ,DataMain.NameNosign
        //                        ,"@Code" ,SqlDbType.NVarChar ,DataMain.Code
        //                        ,"@UnitID" ,SqlDbType.Int ,DataMain.UnitID
        //                        ,"@N1" ,SqlDbType.Decimal ,DataMain.N1
        //                        ,"@N2" ,SqlDbType.Decimal ,DataMain.N2
        //                        ,"@N3" ,SqlDbType.Decimal ,DataMain.N3
        //                        ,"@IsMaterial" ,SqlDbType.Int ,DataMain.IsMaterial
        //                        ,"@Property" ,SqlDbType.NVarChar ,DataMain.Property
        //                        ,"@Description" ,SqlDbType.NVarChar ,DataMain.Description
        //                        ,"@CostPrice" ,SqlDbType.Decimal ,DataMain.CostPrice
        //                        ,"@PriceToSale" ,SqlDbType.Decimal ,DataMain.PriceToSale
        //                        ,"@Note" ,SqlDbType.NVarChar ,DataMain.Note
        //                        ,"@UserID" ,SqlDbType.Int ,1
        //                        ,"@dataUnit" ,SqlDbType.Structured ,DataUnit
        //                        ,"@CurrentID" ,SqlDbType.Int ,para.CurrentID)
        //                    ;
        //                }
        //            }
        //            return JsonConvert.SerializeObject(dt);
        //        });
        //        return Content(await result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

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
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Banner_Delete]" ,CommandType.StoredProcedure
                            ,"@BannerID" ,SqlDbType.Int ,id
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
                        dt = await confunc.ExecuteDataTable("[MLU_SP_Banner_Detail]" ,CommandType.StoredProcedure
                            ,"@BannerID" ,SqlDbType.Int ,id
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
