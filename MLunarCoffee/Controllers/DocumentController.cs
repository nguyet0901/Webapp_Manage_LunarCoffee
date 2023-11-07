using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Crypt;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public DocumentController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Parameters for API getform
        /// </summary>
        /// <syntaxstring>templateid/commandid/id/idstring/exchangeRate/currencyUnit</syntaxstring>
        /// <url>api/document/getform/syntaxstring</url>
        /// <encrypt name="encryptLink">api/Document/getForm/kEVZvCryCcW4kK6Fug2MXpqR7cewtEoU1psAcssBjw6YxuTu6mlacMpbpCazJz8e</encrypt>
        /// <decrypt name="decryptLink">api/Document/getForm/121/11/635//23000/USD//</decrypt>
        //#endregion

        [AllowAnonymous]
        [Route("getform/{syntaxstring}")]
        [HttpGet]
        public async Task<ContentResult> getForm(string syntaxstring)
        {
            try
            {
                //var syntaxEncrypt = Encrypt.encryptApiDocPara("121/11/635//23000/USD", Settings.SemiSecret);

                string validstring = Encrypt.decryptApiDocPara(syntaxstring, Settings.SemiSecret);
                int templateid = Convert.ToInt32(validstring.Split("/")[0].ToString());
                int commandid = Convert.ToInt32(validstring.Split("/")[1].ToString());
                int id = Convert.ToInt32(validstring.Split("/")[2].ToString());
                string idstring = validstring.Split("/")[3].ToString();
                string exchangeRate = validstring.Split("/")[4].ToString();
                string currencyUnit = validstring.Split("/")[5].ToString();
                string wwwPath = this._environment.WebRootPath;
                string contentPath = this._environment.ContentRootPath;
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataSet ds = new DataSet();
                    ds = await confunc.ExecuteDataSet("[YYY_sp_API_Document_GetDataForm]", CommandType.StoredProcedure
                      , "@templateID", SqlDbType.Int, templateid
                      , "@CommandID", SqlDbType.Int, commandid
                      , "@id", SqlDbType.Int, id
                      , "@idstring", SqlDbType.NVarChar, idstring
                      );
                    if (ds != null)
                    {
                        int lengthDataSet = (int)ds.Tables.Count;
                        string result = (int)ds.Tables[lengthDataSet - 1].Rows[0][0] == 1 ? JsonConvert.SerializeObject(ds) : "0";
                        using (var reader = System.IO.File.OpenText(wwwPath + @"/DocumentForm/templateform.html"))
                        {
                            var html = await reader.ReadToEndAsync();
                            html = html.Replace("[PV_FormData]", result)
                                .Replace("[PV_ExchangeRate]", exchangeRate)
                                .Replace("[PV_CurrencyUnit]", currencyUnit);
                            return base.Content(html, "text/html");
                        }
                    }
                    else return Content("");
                }
            }
            catch (Exception ex)
            {
                return base.Content("");
            }
        }
    }
}
