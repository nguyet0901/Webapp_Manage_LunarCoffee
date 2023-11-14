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
    [Authorize]
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
                string ciphertext = Request.Headers["KeyCode"].Count() > 0 ? Request.Headers["KeyCode"] : "";
                var KeyCode = Encrypt.DecryptString(ciphertext ,Settings.PrivateKey);
                string shareKey = DateTime.Now.ToString("yyyyMMdd");
                if (KeyCode != shareKey)
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
    }
}
