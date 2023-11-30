using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using MLunarCoffee.Models.Portal;


namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PortalController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        public PortalController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(Portal portal)
        {
            try
            {
                string validstring = Encrypt.DecryptString(portal.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_PortalAuthorise" ,CommandType.StoredProcedure
                         ,"@branch" ,SqlDbType.Int ,portal.branch
                         ,"@key" ,SqlDbType.NVarChar ,portal.key
                         ,"@tokenFCM" ,SqlDbType.NVarChar ,portal.tokenFCM);
                }

                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];
                    DataTable dtfunc = ds.Tables[1];
                    string key = dt.Rows[0]["Key"].ToString();
                    PortalFunction[] funcs = new PortalFunction[dtfunc.Rows.Count];
                    for (int i = 0; i < dtfunc.Rows.Count; i++)
                    {
                        funcs[i] = new PortalFunction() { code = dtfunc.Rows[i]["Code"].ToString() };
                    }
                    UserDTO validUser = new UserDTO()
                    {
                        UserName = portal.username ,
                        Password = portal.password ,
                        Role = "Portal"
                    };
                    string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString()
                               ,_config["Jwt:Issuer"].ToString() ,validUser);
                    if (generatedToken != null)
                    {
                        PortalResult portalResult = new PortalResult()
                        {
                            key = key ,
                            token = generatedToken ,
                            func = funcs
                        };
                        return Content(JsonConvert.SerializeObject(portalResult));
                    }
                    return Content("0");
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("GetIni")]
        public async Task<IActionResult> GetIni(Ini ini)
        {
            try
            {
                string validstring = Encrypt.DecryptString(ini.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_PortalIni" ,CommandType.StoredProcedure
                         ,"@branch" ,SqlDbType.NVarChar ,ini.branch);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("GetLog")]
        public async Task<IActionResult> GetLog(Ini ini)
        {
            try
            {
                string validstring = Encrypt.DecryptString(ini.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_PortalGetLog" ,CommandType.StoredProcedure
                         ,"@branch" ,SqlDbType.NVarChar ,ini.branch);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        [Authorize]
        [HttpPost("GetExam")]
        public async Task<IActionResult> GetExam(Ini ini)
        {
            try
            {
                string validstring = Encrypt.DecryptString(ini.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_PortalGetExam" ,CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("GetPrehistory")]
        public async Task<IActionResult> GetPrehistory(Ini ini)
        {
            try
            {
                string validstring = Encrypt.DecryptString(ini.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_PortalGetPrehistory" ,CommandType.StoredProcedure);
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("ProfileInsert")]
        public async Task<IActionResult> ProfileInsert(PortalProfile pro)
        {
            try
            {
                string validstring = Encrypt.DecryptString(pro.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_PortalProfileInsert" ,CommandType.StoredProcedure ,
                         "@Email1" ,SqlDbType.NVarChar ,pro.email1 ,
                         "@Address" ,SqlDbType.NVarChar ,pro.address ,
                         "@Phone1" ,SqlDbType.NVarChar ,pro.phone1 ,
                         "@Phone2" ,SqlDbType.NVarChar ,pro.phone2 ,
                         "@Name" ,SqlDbType.NVarChar ,pro.name ,
                         "@Gender_ID" ,SqlDbType.Int ,pro.gender_ID ,
                         "@Language_ID" ,SqlDbType.Int ,pro.language_ID ,
                         "@Branch_ID" ,SqlDbType.Int ,pro.branch_ID ,
                         "@Birthday" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMD(pro.birthday) ,
                         "@Created_By" ,SqlDbType.Int ,pro.created_By ,
                         "@CityID" ,SqlDbType.Int ,pro.cityID ,
                         "@District" ,SqlDbType.Int ,pro.district ,
                         "@Career" ,SqlDbType.Int ,pro.career ,
                         "@phonecode" ,SqlDbType.NVarChar ,pro.phonecode ,
                         "@NationalID" ,SqlDbType.Int ,pro.nationalID ,
                         "@diafree" ,SqlDbType.NVarChar ,pro.diafree != null ? pro.diafree : "" ,
                         "@diaimg" ,SqlDbType.NVarChar ,pro.diaimg != null ? pro.diaimg : "" ,
                         "@diadataSVG" ,SqlDbType.NVarChar ,pro.diadataSVG != null ? pro.diadataSVG : "" ,
                         "@diadataQues" ,SqlDbType.NVarChar ,pro.diadataQues != null ? pro.diadataQues : "" ,
                         "@diaNote" ,SqlDbType.NVarChar ,pro.diaNote ,
                         "@diaType" ,SqlDbType.NVarChar ,pro.diaType ,
                         "@preQuestion" ,SqlDbType.NVarChar ,pro.preQuestion != null ? pro.preQuestion : "" ,
                         "@representname" ,SqlDbType.NVarChar ,pro.representname != null ? pro.representname : "" ,
                         "@representphone" ,SqlDbType.NVarChar ,pro.representphone != null ? pro.representphone : "" ,
                         "@relationship" ,SqlDbType.Int ,pro.relationship != null ? pro.relationship : "0" ,
                         "@identitycard" ,SqlDbType.NVarChar ,pro.identitycard != null ? pro.identitycard : ""
                    );
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }




        [Authorize]
        [HttpPost("RatingInsert")]
        public async Task<IActionResult> RatingInsert(Ratingus rat)
        {
            try
            {
                string validstring = Encrypt.DecryptString(rat.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataTable dt = await Comon.Portal.excuteRating(rat);
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [HttpPost("RatingGetType")]
        public async Task<IActionResult> RatingGetType(Ratingtype rattype)
        {
            try
            {
                string validstring = Encrypt.DecryptString(rattype.apiKey ,Settings.SemiSecret);
                string sharekey = DateTime.Now.ToString("yyyyMMdd");
                if (validstring != sharekey)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized ,new { message = "Unauthorized" });
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    if (rattype.type != 0)
                    {
                        ds = await confunc.ExecuteDataSet("MLU_sp_PortalRating_LoadType" ,CommandType.StoredProcedure ,
                             "@Customer" ,SqlDbType.Int ,rattype.custid
                            );
                    }

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
