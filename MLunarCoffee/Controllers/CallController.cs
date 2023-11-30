using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.TokenJWT;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;


namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CallController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        public CallController(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpPost("InCommingCall")]

        public async Task<IActionResult> InCommingCall(InComming inComming)
        {
            try
            {
                if (inComming.phone != "")
                {
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("MLU_sp_Calling_GetInCommingCall", CommandType.StoredProcedure,
                            "@Phone", SqlDbType.NVarChar, inComming.phone);
                    }
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        [Authorize]
        [HttpPost("OutCommingCall")]

        public async Task<IActionResult> OutCommingCall(OutComming outComming)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Calling_GetOutCommingCall",
                        CommandType.StoredProcedure,
                        "@Customer", SqlDbType.Int, outComming.cus,
                        "@Ticket", SqlDbType.Int, outComming.tic,
                        "@Line", SqlDbType.NVarChar, outComming.line);
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
        [HttpPost("DetectPhone")]
        public async Task<IActionResult> DetectPhone(OutComming outComming)
        {
            try
            {
                string x = outComming.linefromcall;
                string x1 = outComming.linkfromcall;
                if (outComming.cus != "")
                {
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("MLU_sp_CallOutSide_GetPhone", CommandType.StoredProcedure,
                            "@CustID", SqlDbType.NVarChar, outComming.cus);
                    }
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (Global.sys_CallOnlyHTTP == "1")
                        {
                            using (HttpClient clientautho = new HttpClient() { })
                            {
                                var result = await clientautho.GetAsync(outComming.linkfromcall + "?LineToCall=" + outComming.linefromcall + "&Phone=" + dt.Rows[0][0].ToString());
                                string responseBody = await result.Content.ReadAsStringAsync();
                                return Content("0");
                            }
                        }
                        else return Content(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        return Content("-1");
                    }
                }
                return Content("-1");
            }
            catch (Exception ex)
            {
                return Content("-1");
            }
        }

        [AllowAnonymous]
        [Route("StoreLogs")]
        [HttpPost]
        public async Task<IActionResult> StoreLogs(LogCall logCall)
        {
            try
            {
                string RootUsername = Global.sys_Callafter_Username;
                string RootPassword = Global.sys_Callafter_Password;
                var auth = Request.Headers["Authorization"].ToString();
                if (!auth.ToLower().StartsWith("basic "))
                    return Unauthorized();

                var credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6)));

                string UserName = credentials.Split(':')[0];
                string Password = credentials.Split(':')[1];

                if (RootUsername != UserName || RootPassword != Password)
                    return Unauthorized();

                DataTable dt = new DataTable();
                var targets = logCall.call_targets != null ? JsonConvert.SerializeObject(logCall.call_targets) : "";
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_LogCall_Insert]", CommandType.StoredProcedure
                        , "@answered_time", SqlDbType.NVarChar, logCall.answered_time
                        , "@call_id", SqlDbType.NVarChar, logCall.call_id
                        , "@call_status", SqlDbType.NVarChar, logCall.call_status
                        , "@call_targets", SqlDbType.NVarChar, targets
                        , "@callee", SqlDbType.NVarChar, logCall.callee
                        , "@callee_domain", SqlDbType.NVarChar, logCall.callee_domain
                        , "@caller", SqlDbType.NVarChar, logCall.caller
                        , "@caller_display_name", SqlDbType.NVarChar, logCall.caller_display_name
                        , "@caller_domain", SqlDbType.NVarChar, logCall.caller_domain
                        , "@extension_final" , SqlDbType.NVarChar, logCall.extension_final
                        , "@did_cid", SqlDbType.NVarChar, logCall.did_cid
                        , "@direction", SqlDbType.NVarChar, logCall.direction
                        , "@ended_reason", SqlDbType.NVarChar, logCall.ended_reason
                        , "@ended_time", SqlDbType.NVarChar, logCall.ended_time
                        , "@fail_code", SqlDbType.NVarChar, logCall.fail_code
                        , "@final_dest", SqlDbType.NVarChar, logCall.final_dest
                        , "@outbound_caller_id", SqlDbType.NVarChar, logCall.outbound_caller_id
                        , "@related_callid1", SqlDbType.NVarChar, logCall.related_callid1
                        , "@related_callid2", SqlDbType.NVarChar, logCall.related_callid2
                        , "@ring_duration", SqlDbType.NVarChar, logCall.ring_duration
                        , "@ring_time", SqlDbType.NVarChar, logCall.ring_time
                        , "@start_time", SqlDbType.NVarChar, logCall.start_time
                        , "@talk_duration", SqlDbType.NVarChar, logCall.talk_duration
                        , "@tenant_id", SqlDbType.NVarChar, logCall.tenant_id
                        , "@tenant_name", SqlDbType.NVarChar, logCall.tenant_name
                        , "@audio_file_name", SqlDbType.NVarChar, logCall.audio_file_name
                        , "@audio_file_path", SqlDbType.NVarChar, logCall.audio_file_path
                    );
                }
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["result"].ToString() != "0")
                    {
                        return StatusCode(StatusCodes.Status200OK, new { result = 1, message = "Success" });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { result = 0, message = "Handling failures" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "BadRequest" });
            }
        }
    }
}

public class OutComming
{
    public string cus { get; set; }
    public string tic { get; set; }
    public string line { get; set; }
    public string linefromcall { get; set; }
    public string linkfromcall { get; set; }
}
public class InComming
{
    public string phone { get; set; }
}
public class LogCall
{
    public string answered_time { get; set; } = "";
    public string call_id { get; set; } = "";
    public string call_status { get; set; } = "";
    public LogCallTarget[] call_targets { get; set; }
    public string callee { get; set; } = "";
    public string callee_domain { get; set; } = "";
    public string caller { get; set; } = "";
    public string caller_display_name { get; set; } = "";
    public string caller_domain { get; set; } = "";
    public string extension_final { get; set; } = "";
    public string did_cid { get; set; } = "";
    public string direction { get; set; } = "";
    public string ended_reason { get; set; } = "";
    public string ended_time { get; set; } = "";
    public string fail_code { get; set; } = "";
    public string final_dest { get; set; } = "";
    public string outbound_caller_id { get; set; } = "";
    public string related_callid1 { get; set; } = "";
    public string related_callid2 { get; set; } = "";
    public string ring_duration { get; set; } = "";
    public string ring_time { get; set; } = "";
    public string start_time { get; set; } = "";
    public string talk_duration { get; set; } = "";
    public string tenant_id { get; set; } = "";
    public string tenant_name { get; set; } = "";
    public string audio_file_name { get; set; } = "";
    public string audio_file_path { get; set; } = "";
}

public class LogCallTarget
{
    public string add_time { get; set; } = ""; //thời gian bắt đầu cuộc gọi (định dạng UNIX)

    public string answered_time { get; set; } = ""; //thời gian trả lời cuộc gọi(định dạng UNIX)
    public string end_reason { get; set; } = "";
    public string ended_time { get; set; } = "";
    public string fail_code { get; set; } = "";
    public string related_callid { get; set; } = "";
    public string ring_duration { get; set; } = "";
    public string ring_time { get; set; } = "";
    public string status { get; set; } = "";
    public string talk_duration { get; set; } = "";
    public string target_number { get; set; } = "";
    public string trunk_name { get; set; } = "";
}
