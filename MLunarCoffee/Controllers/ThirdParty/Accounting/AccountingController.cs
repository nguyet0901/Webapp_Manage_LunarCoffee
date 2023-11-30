using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Accounting;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using MLunarCoffee.Models.ThirdParty.Account;
using MLunarCoffee.Service.Quartz;

namespace MLunarCoffee.Controllers.ThirdParty.Accounting
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        private string BaseLink = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_Url : "";
        private string AccountBranch = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_Brand : "";
        private string AccountUserName = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_UserName : "";
        private string SaveReceiptLink = "/api/sync/actopen/save";
        private string DeleteReceiptLink = "/api/sync/actopen/delete";
        private const int LimitSend = 100;
        private const int MaxBlock = 50;
        private const string companycode = "congtyvttechsolution";

        public AccountingController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        #region //Save Receipt

        [Authorize]
        [Route("LoadData")]
        [HttpPost]
        public async Task<IActionResult> LoadData(AccountVoucher accountPara)
        {
            DataTable dt = new DataTable();
            try
            {
                if (BaseLink != null && BaseLink != "")
                {
                    GlobalUser user = Session.GetUserSession(HttpContext);
                    if (user == null || user.synthird_accounttoken == null || user.synthird_accounttoken == "") return Content("0");

                    var token = Session.GetSession(HttpContext.Session ,"Token");
                    if (token == null)
                    {
                        return Content("0");
                    }
                    if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                        _config["Jwt:Issuer"].ToString() ,token))
                    {

                        DataSet ds = new DataSet();
                        var tasks = new List<Task<DataTable>>
                        {
                            Task.Run(async () =>
                            {
                                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                {
                                    DataTable dt = new DataTable();
                                    dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting]" ,CommandType.StoredProcedure ,
                                        "@AccountBranch" ,SqlDbType.NVarChar ,AccountBranch,
                                        "@ActionType" ,SqlDbType.NVarChar ,accountPara.action);
                                    dt.TableName = "DataSetting";
                                    return dt;
                                }
                            }) ,
                            Task.Run(async () =>
                            {
                                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                {
                                    DataTable dt = new DataTable();
                                    dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadData]" ,CommandType.StoredProcedure
                                        ,"@Type" ,SqlDbType.Int ,accountPara.type
                                        ,"@CurrentID" ,SqlDbType.Int ,accountPara.currentID
                                        ,"@isDel" ,SqlDbType.Int ,accountPara.action == "save" ? 0 : 1
                                        ,"@APIUserName" ,SqlDbType.NVarChar ,AccountUserName
                                        );
                                    dt.TableName = "DataMain";
                                    return dt;
                                }
                            })
                        };
                        var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                        foreach (var r in result)
                        {
                            ds.Tables.Add(r);
                        }
                        return Content(Comon.DataJson.Dataset(ds));
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
        [Route("LoadDataMulti")]
        [HttpPost]
        public async Task<IActionResult> LoadDataMulti(AccountVoucherMulti accountParas)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                if (BaseLink != null && BaseLink != "")
                {
                    GlobalUser user = Session.GetUserSession(HttpContext);
                    if (user == null || user.synthird_accounttoken == null || user.synthird_accounttoken == "") return Content("0");

                    var token = Session.GetSession(HttpContext.Session ,"Token");
                    if (token == null)
                    {
                        return Content("0");
                    }
                    if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                        _config["Jwt:Issuer"].ToString() ,token))
                    {

                        DataTable dtAcc = JsonConvert.DeserializeObject<dtAccountVoucher>(accountParas.data);
                        if (dtAcc.Rows.Count > 0 && dtAcc.Rows.Count < LimitSend)
                        {
                            var tasks = new List<Task<DataTable>>
                            {
                                Task.Run(async () =>
                                {
                                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                    {
                                        DataTable dt = new DataTable();
                                        dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting]" ,CommandType.StoredProcedure ,
                                            "@AccountBranch" ,SqlDbType.NVarChar ,AccountBranch,
                                            "@ActionType" ,SqlDbType.NVarChar ,accountParas.action);
                                        dt.TableName = "DataSetting";
                                        return dt;
                                    }
                                })
                            };
                            var tables = dtAcc.AsEnumerable().ToChunks(MaxBlock).Select(rows => rows.CopyToDataTable());
                            if (tables != null)
                            {
                                foreach (var ta in tables)
                                {
                                    if (ta != null)
                                    {
                                        tasks.Add(Task.Run(async () =>
                                        {
                                            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                            {

                                                dt = await confunc.ExecuteDataTable("MLU_sp_API_Account_LoadDataMulti" ,CommandType.StoredProcedure
                                                     ,"@data" ,SqlDbType.Structured ,ta
                                                     ,"@APIUserName" ,SqlDbType.NVarChar ,AccountUserName);
                                                return dt;
                                            }
                                        }));
                                    }
                                }
                                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                                foreach (var r in result)
                                {
                                    ds.Tables.Add(r);
                                }
                                return Content(Comon.DataJson.Dataset(ds));
                            }
                        }
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
        [Route("Excuted")]
        [HttpPost]
        public async Task<IActionResult> Excuted(AccountPara accPara)
        {
            int userid = 0;
            DataTable dt = new DataTable();
            try
            {
                if (BaseLink != null && BaseLink != "")
                {
                    GlobalUser user = Session.GetUserSession(HttpContext);
                    if (user == null || user.synthird_accounttoken == null || user.synthird_accounttoken == "") return Content("0");
                    userid = Convert.ToInt32(user.sys_userid);

                    var token = Session.GetSession(HttpContext.Session ,"Token");
                    if (token == null)
                    {
                        return Content("0");
                    }
                    if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                        _config["Jwt:Issuer"].ToString() ,token))
                    {
                        var tasks = new List<Task<int>>
                        {
                            Task.Run(async () =>
                            {
                                await AccountingAction.WriteLog(accPara.branchid
                                            ,userid
                                            ,accPara.custid
                                            ,accPara.topic
                                            ,accPara.page
                                            ,accPara.para
                                            ,""
                                            ,AccountingAction.getActionLog(accPara.action, accPara.isupdate)).ConfigureAwait(false);
                                 return 1;
                            }),
                            Task.Run(async () =>
                            {
                                if(accPara.action == "save")
                                {
                                     //await AccountingAction.UpdateVoucher(accPara.dataVoucher).ConfigureAwait(false);
                                     await AccountingAction.UpdateVoucher(accPara.dataVoucher);
                                }
                                return 1;
                            }),
                            Task.Run(async () =>
                            {
                                await AccountingAction.PushVoucher(AccountBranch, accPara.action, user.synthird_accounttoken, accPara.para, BaseLink).ConfigureAwait(false);
                                return 1;
                            })
                        };

                        var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);

                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                await AccountingAction.WriteLog(accPara.branchid
                            ,userid
                            ,accPara.custid
                            ,accPara.topic
                            ,accPara.page
                            ,accPara.para
                            ,ex.InnerException.Message
                            ,"ei").ConfigureAwait(false);
                return Content("0");
            }
        }

        #endregion
        #region //Result Callback

        [ServiceFilter(typeof(AuthorizeIPAddressAttribute))]
        [Route("ResultCallback")]
        [HttpPost]
        public async Task<IActionResult> ResultCallback(MisaCallBackPara param)
        {
            MisaCallBackResponse result = new MisaCallBackResponse();
            if (BaseLink != null && BaseLink != "")
            {
                try
                {
                    List<CallbackResult> data = JsonConvert.DeserializeObject<List<CallbackResult>>(param.data);
                    switch (param.data_type)
                    {
                        case 1: //Save
                            await HandleCallBack(false ,data).ConfigureAwait(false);
                            break;
                        case 2: //Delete
                            await HandleCallBack(true ,data).ConfigureAwait(false);
                            break;
                    }
                    result.Success = true;
                    result.ErrorCode = "";
                    result.ErrorMessage = "";
                    return Content(JsonConvert.SerializeObject(result));
                }
                catch (Exception ex)
                {
                    result.Success = true;
                    result.ErrorCode = "";
                    result.ErrorMessage = "";
                    return Content(JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                return Content("0");
            }
        }

        private async Task HandleCallBack(bool isDelete ,List<CallbackResult> data)
        {
            try
            {
                if (data != null)
                {
                    DataTable dtMain = new DataTable();
                    dtMain.Columns.Add("orgRefid");
                    dtMain.Columns.Add("isSuccess");
                    dtMain.Columns.Add("errorMessage");
                    dtMain.Columns.Add("isDelete");
                    DataTable dt = new DataTable();
                    foreach (CallbackResult item in data)
                    {
                        ////xử lý cập nhật trạng thái của từng org_refid vào đây
                        DataRow dr = dtMain.NewRow();
                        dr["orgRefid"] = item?.org_refid ?? "";
                        dr["isSuccess"] = (item?.success ?? false) ? 1 : 0;
                        dr["errorMessage"] = item?.error_message ?? "";
                        dr["isDelete"] = isDelete ? 1 : 0;
                        dtMain.Rows.Add(dr);
                    }
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_API_Account_Callback]" ,CommandType.StoredProcedure ,
                            "@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0 ? (DataTable)dtMain : null));
                    }
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}
