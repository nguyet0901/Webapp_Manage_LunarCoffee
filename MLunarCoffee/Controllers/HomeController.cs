using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class HomeController : ControllerBase
    {
        public HomeController(IWebHostEnvironment env)
        {
            CurrentEnvironment = env;
        }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        //private IMemoryCache cache;
        //private readonly IHubContext<NotiHub> _notificationHubContext;
        //public HomeController()
        //{
        //this.cache = cache;
        //_notificationHubContext = notificationHubContext;
        //}
        [Authorize]
        [HttpPost("KeepAlive")]
        public ActionResult KeepAlive()
        {
            return Content("1");
        }

        [Authorize]
        [HttpPost("SessionData")]
        public async Task<IActionResult> SessionData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_Session_Load", CommandType.StoredProcedure);
                }
                //RebuildLocalization();
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("UpdateGroupUser")]
        public async Task<IActionResult> UpdateGroupUser(UserGroup userGroup)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_User_UpdateGroup", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, userGroup.UserID,
                        "@GroupID", SqlDbType.Int, userGroup.GroupID);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        #region // Rebuild location language

        [Authorize]
        [HttpPost("RebuildLocalization")]
        public async Task<IActionResult> RebuildLocalization()
        {
            try
            {
                if (CurrentEnvironment.IsDevelopment())
                {
                    await System_ResxToJS(CurrentEnvironment);
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }


        private static async Task System_ResxToJS(IWebHostEnvironment environment)
        {
            try
            {
                string uploadDir = Path.Combine(environment.WebRootPath, "Language");
                var fileNameen = "OutResource.en" + ".js";
                string filePathen = Path.Combine(uploadDir, fileNameen);
                await SystemWrite_Json(@".\LocalizationResources\ExpressLocalizationResource.en.resx", filePathen);

                var fileNamevn = "OutResource.vi" + ".js";
                string filePathvn = Path.Combine(uploadDir, fileNamevn);
                await SystemWrite_Json(@".\LocalizationResources\ExpressLocalizationResource.vi.resx", filePathvn);

            }
            catch (Exception ex)
            {

            }

        }
        private static async Task SystemWrite_Json(string resxurl, string resxname)
        {
            try
            {
                List<Resourceval> resources = new List<Resourceval>();
                using (ResXResourceReader resx = new ResXResourceReader(resxurl))
                {

                    foreach (DictionaryEntry d in resx)
                    {
                        JObject obj = (JObject)JToken.FromObject(d);
                        resources.Add(new Resourceval()
                        {
                            Key = obj["Key"].ToString(),
                            Value = obj["Value"].ToString(),
                        });
                    }
                    System.IO.File.WriteAllText(resxname, "var DataOutlang = " + JsonConvert.SerializeObject(resources));
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion




        [Authorize]
        [HttpPost("TokenUser")]
        public async Task<IActionResult> TokenUser(UserToken user)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_TokenUser", CommandType.StoredProcedure
                         , "@UserID", SqlDbType.Int, user.UserID);
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    return Content(dt.Rows[0]["TokenFcm"].ToString());
                }
                else return Content("0");

            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("TokenMultiUser")]
        public async Task<IActionResult> TokenMultiUser(UsersToken user)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_TokenMultiUser", CommandType.StoredProcedure
                         , "@StringUserID", SqlDbType.NVarChar, user.stringuserid);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("0");

            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("MobileCust")]
        public async Task<IActionResult> MobileCust(MobileCust cust)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_TokenMobile_Customer", CommandType.StoredProcedure
                         , "@Customer", SqlDbType.Int, cust.CustomerID);
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("0");

            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
