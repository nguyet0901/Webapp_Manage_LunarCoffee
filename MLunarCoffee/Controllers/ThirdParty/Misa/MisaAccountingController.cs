using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Log;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using MLunarCoffee.Models.Invoice;
using MLunarCoffee.Service.Quartz;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MisaAccountingController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        private string BaseLink = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_Url : "";
        private string appId = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_UserName : "";
        private string SaveReceiptLink = "/api/sync/actopen/save";
        private string DeleteReceiptLink = "/api/sync/actopen/delete";
        private const int LimitSend = 100;
        private const int MaxBlock = 50;
        private const string companycode = "congtyvttechsolution";

        public MisaAccountingController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        #region //Save Receipt

        [Authorize]
        [Route("SaveAccountingReceipt")]
        [HttpPost]
        public async Task<IActionResult> SaveAccountingReceipt(MisaAccounting misaAccounting)
        {
            int userid = 0, branchid = 0, custid = 0;
            string vouchercode = "";
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

                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {

                            dt = await confunc.ExecuteDataTable("YYY_sp_MisaAccounting_LoadData" ,CommandType.StoredProcedure
                                 ,"@Type" ,SqlDbType.Int ,misaAccounting.type
                                 ,"@CurrentID" ,SqlDbType.Int ,misaAccounting.currentID
                                 );
                            if (dt != null && dt.Rows.Count != 0)
                            {
                                var parameter = generateReceiptParam(dt);
                                var jsonPara = JsonConvert.SerializeObject(parameter);
                                branchid = Convert.ToInt32(dt.Rows[0]["BranchID"]);
                                custid = Convert.ToInt32(dt.Rows[0]["CustID"]);
                                vouchercode = Convert.ToString(dt.Rows[0]["Code"]);
                                string ActionLog = Convert.ToInt32(dt.Rows[0]["IsUpdate"]) == 1 ? "u" : "i";
                                await writeLog(branchid
                                            ,userid
                                            ,custid
                                            ,vouchercode
                                            ,misaAccounting.page
                                            ,jsonPara
                                            ,""
                                            ,ActionLog).ConfigureAwait(false);

                                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseLink) })
                                {
                                    clientautho.DefaultRequestHeaders.Add("X-MISA-AccessToken" ,user.synthird_accounttoken);
                                    var content = new StringContent(jsonPara ,Encoding.UTF8 ,"application/json");
                                    await Task.Run(async () => await clientautho.PostAsync(SaveReceiptLink ,content).ConfigureAwait(false)).ConfigureAwait(false);
                                    return Content("1");
                                }
                            }
                        }
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                await writeLog(branchid
                            ,userid
                            ,custid
                            ,vouchercode
                            ,misaAccounting.page
                            ,""
                            ,ex.InnerException.Message
                            ,"ei").ConfigureAwait(false);
                return Content("0");
            }
        }

        [Authorize]
        [Route("SaveAccountingReceiptMulti")]
        [HttpPost]
        public async Task<IActionResult> SaveAccountingReceiptMulti(MisaAccountingMulti mis)
        {
            int userid = 0, branchid = 0, custid = 0;
            string vouchercode = "";
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

                        DataTable dtMisa = JsonConvert.DeserializeObject<dtMisaAccounting>(mis.data);
                        if (dtMisa.Rows.Count > 0 && dtMisa.Rows.Count < LimitSend)
                        {
                            var tasks = new List<Task>();
                            var tables = dtMisa.AsEnumerable().ToChunks(MaxBlock).Select(rows => rows.CopyToDataTable());
                            if (tables != null)
                            {
                                foreach (var ta in tables)
                                {
                                    if (ta != null)
                                    {
                                        tasks.Add(Task.Factory.StartNew(async () =>
                                        {
                                            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                            {

                                                dt = await confunc.ExecuteDataTable("YYY_sp_MisaAccounting_LoadDataMulti" ,CommandType.StoredProcedure
                                                     ,"@data" ,SqlDbType.Structured ,ta);
                                                if (dt != null && dt.Rows.Count != 0)
                                                {
                                                    var parameter = generateReceiptParam(dt);
                                                    var jsonPara = JsonConvert.SerializeObject(parameter);
                                                    var dateNow = Comon.Comon.GetDateTimeNow();
                                                    branchid = Convert.ToInt32(user.sys_branchID);
                                                    custid = 0;
                                                    vouchercode = $"SyncMulti_{dateNow.ToString("yyyy-MM-dd")}";
                                                    string ActionLog = Convert.ToInt32(dt.Rows[0]["IsUpdate"]) == 1 ? "u" : "i";
                                                    await writeLog(branchid
                                                                ,userid
                                                                ,custid
                                                                ,vouchercode
                                                                ,mis.page
                                                                ,jsonPara
                                                                ,""
                                                                ,ActionLog).ConfigureAwait(false);

                                                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseLink) })
                                                    {
                                                        clientautho.DefaultRequestHeaders.Add("X-MISA-AccessToken" ,user.synthird_accounttoken);
                                                        var content = new StringContent(jsonPara ,Encoding.UTF8 ,"application/json");
                                                        await Task.Run(async () => await clientautho.PostAsync(SaveReceiptLink ,content).ConfigureAwait(false)).ConfigureAwait(false);
                                                    }
                                                }
                                            }
                                        }));
                                    }
                                }
                                return Content("1");
                            }
                        }
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                await writeLog(branchid
                            ,userid
                            ,custid
                            ,vouchercode
                            ,mis.page
                            ,""
                            ,ex.InnerException.Message
                            ,"ei").ConfigureAwait(false);
                return Content("0");
            }
        }

        private async Task writeLog(int BranchID ,int UserID ,int CustID ,string Code ,string page ,string jsonContent ,string content ,string Action = "i")
        {
            var log = new Log()
            {
                BranchID = BranchID ,
                UserID = UserID ,
                CustomerID = CustID ,
                Type = "mis" ,
                Content = content ,
                Page = page ,
                Action = Action ,
                Value = Code ,
                JsonContent = jsonContent
            };
            await LogAction.InsertLog(log).ConfigureAwait(false);
        }

        private MisaAccountingReceipt generateReceiptParam(DataTable dtDetail)
        {
            List<MisaAccountingVoucher> vouchers = new List<MisaAccountingVoucher>(); //Only 1 voucher
            List<MisaAccountingDictionary> dictionaries = new List<MisaAccountingDictionary>(); //Have 2 object receipent and sender
            List<MisaAccountingVoucherDetail> details = new List<MisaAccountingVoucherDetail>(); //Only 1 voucher
            int id = 0, count = 0; string accRef = "";
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                var dr = dtDetail.Rows[i];
                var debitBranch = Convert.ToInt32(Convert.ToString(dr["IsDebitBranch"])) == 1 ? Convert.ToString(dr["BranchAlias"]) : "";
                var creditBranch = Convert.ToInt32(Convert.ToString(dr["IsCreditBranch"])) == 1 ? Convert.ToString(dr["BranchAlias"]) : "";

                if (id != Convert.ToInt32(dr["ID"].ToString()))
                {
                    int isFirst = id == 0 ? 1 : 0;
                    id = Convert.ToInt32(dr["ID"].ToString());
                    accRef = dr["AccountRef"].ToString();
                    vouchers.Add(generateVoucher(dr));
                    dictionaries.Add(generateDictionary(1 ,dr));
                    dictionaries.Add(generateDictionary(2 ,dr));
                    if (isFirst != 1)
                    {
                        vouchers.ElementAt(count).detail = details.ToArray();
                        details.Clear();
                        count++;
                    }
                }
                details.Add(new MisaAccountingVoucherDetail()
                {
                    account_object_id = accRef ,
                    account_object_code = Convert.ToString(dr["AccountCode"]) ,
                    account_object_name = Convert.ToString(dr["AccountName"]) ,
                    bank_account_id = Convert.ToString(dr["BankRef"]) ,
                    bank_account_number = Convert.ToString(dr["BankCode"]) ,
                    bank_name = Convert.ToString(dr["PayMethod"]) ,
                    sort_order = 1 ,
                    amount_oc = Convert.ToDecimal(Convert.ToString(dr["Amount"])) ,
                    amount = Convert.ToDecimal(Convert.ToString(dr["Amount"])) ,
                    description = Convert.ToString(dr["Description"]) ,
                    debit_account = Convert.ToString(dr["DebitAccount"]) + debitBranch ,
                    credit_account = Convert.ToString(dr["CreditAccount"]) + creditBranch ,
                });
                if (i == dtDetail.Rows.Count - 1)
                {
                    vouchers.ElementAt(count).detail = details.ToArray();
                }

            }

            MisaAccountingReceipt para = new MisaAccountingReceipt()
            {
                app_id = appId ,
                org_company_code = companycode ,
                voucher = vouchers.ToArray() ,
                dictionary = dictionaries.ToArray() ,
            };
            return para;
        }

        private MisaAccountingVoucher generateVoucher(DataRow dr)
        {
            MisaAccountingVoucher voucher = new MisaAccountingVoucher()
            {
                voucher_type = Convert.ToInt32(Convert.ToString(dr["VoucherType"])) ,
                org_refid = Convert.ToString(dr["GuidRef"]) ,
                branch_id = Convert.ToString(dr["MisaBranchID"]) != "" ? Convert.ToString(dr["MisaBranchID"]) : Convert.ToString(dr["GuidRef"]) ,
                branch_code = Convert.ToString(dr["BranchCode"]) ,
                branch_name = Convert.ToString(dr["BranchName"]) ,
                refdate = ((DateTime)dr["ReceiptsDate"]).ToString("yyyy-MM-dd") ,
                posted_date = ((DateTime)dr["ReceiptsDate"]).ToString("yyyy-MM-dd") ,
                org_reftype = Convert.ToInt32(Convert.ToString(dr["RefType"])) ,
                org_reftype_name = Convert.ToString(dr["RefTypeName"]) ,
                org_refno = Convert.ToString(dr["Code"]) ,
                reftype = Convert.ToInt32(Convert.ToString(dr["RefType"])) ,
                employee_id = Convert.ToString(dr["EmpRef"]) ,
                employee_code = Convert.ToString(dr["EmpCode"]) ,
                employee_name = Convert.ToString(dr["EmpName"]) ,
                account_object_id = Convert.ToString(dr["AccountRef"]) ,
                account_object_code = Convert.ToString(dr["AccountCode"]) ,
                account_object_name = Convert.ToString(dr["AccountName"]) ,
                account_object_contact_name = Convert.ToString(dr["AccountContactName"]) ,
                bank_account_id = Convert.ToString(dr["BankRef"]) ,
                bank_account_number = Convert.ToString(dr["BankCode"]) ,
                bank_name = Convert.ToString(dr["PayMethod"]) ,
                currency_id = "VND" ,
                total_amount_oc = Convert.ToDecimal(Convert.ToString(dr["TotalAmount"])) ,
                total_amount = Convert.ToDecimal(Convert.ToString(dr["TotalAmount"])) ,
                reason_type_id = Convert.ToInt32(Convert.ToString(dr["ReasonType"])) ,
                journal_memo = Convert.ToString(dr["GeneralNote"]) ,
                created_date = Convert.ToString(dr["Created"]) ,
                created_by = Convert.ToString(dr["CreatedName"]) ,
                modified_date = Convert.ToString(dr["Modified"]) ,
                modified_by = Convert.ToString(dr["ModifiedName"]) ,
                detail = null
            };
            return voucher;
        }
        private MisaAccountingDictionary generateDictionary(int type ,DataRow dr)
        {
            var result = new MisaAccountingDictionary()
            {
                inactive = false ,
                state = 1 ,
                created_date = Convert.ToString(dr["Created"]) ,
                created_by = Convert.ToString(dr["CreatedName"]) ,
                modified_date = Convert.ToString(dr["Modified"]) ,
                modified_by = Convert.ToString(dr["ModifiedName"]) ,
                account_object_type = Convert.ToInt16(Convert.ToString(dr["ObjectType"]))

            };
            switch (type)
            {
                case 1: //Cust
                    result.dictionary_type = 1;
                    result.account_object_id = Convert.ToString(dr["AccountRef"]);
                    result.account_object_code = Convert.ToString(dr["AccountCode"]);
                    result.account_object_name = Convert.ToString(dr["AccountName"]);
                    result.is_vendor = Convert.ToString(dr["IsSup"]) == "1";
                    result.is_customer = Convert.ToString(dr["IsCust"]) == "1";
                    result.is_employee = Convert.ToString(dr["IsEmp"]) == "1";

                    break;
                case 2: //Emp
                    result.dictionary_type = 1;
                    result.account_object_id = Convert.ToString(dr["EmpRef"]);
                    result.account_object_code = Convert.ToString(dr["EmpCode"]);
                    result.account_object_name = Convert.ToString(dr["EmpName"]);
                    result.is_vendor = false;
                    result.is_customer = false;
                    result.is_employee = true;
                    break;
                case 3: //Sup
                    result.dictionary_type = 1;
                    result.account_object_id = Convert.ToString(dr["AccountRef"]);
                    result.account_object_code = Convert.ToString(dr["AccountCode"]);
                    result.account_object_name = Convert.ToString(dr["AccountName"]);
                    result.is_vendor = true;
                    result.is_customer = false;
                    result.is_employee = false;
                    break;
            }
            return result;
        }
        #endregion

        #region //Delete

        [Authorize]
        [Route("DeleteReceipt")]
        [HttpPost]
        public async Task<IActionResult> DeleteReceipt(MisaAccountingDelete misaAccountingDelete)
        {
            try
            {
                if (BaseLink != null && BaseLink != "")
                {
                    GlobalUser user = Session.GetUserSession(HttpContext);
                    var token = Session.GetSession(HttpContext.Session ,"Token");
                    if (token == null)
                    {
                        return Content("0");
                    }
                    if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                        _config["Jwt:Issuer"].ToString() ,token))
                    {
                        var parameter = new MisaAccountingReceipt()
                        {
                            app_id = appId ,
                            org_company_code = companycode ,
                            voucher = new MisaAccountingVoucher[1]
                            {
                            new MisaAccountingVoucher()
                            {
                                voucher_type = misaAccountingDelete.voucherType,
                                org_refid = misaAccountingDelete.refid
                            }
                            }
                        };
                        using (HttpClient clientautho = new HttpClient())
                        {
                            clientautho.DefaultRequestHeaders.Add("X-MISA-AccessToken" ,user.synthird_accounttoken);
                            using (HttpRequestMessage request = new HttpRequestMessage()
                            {
                                Method = HttpMethod.Delete ,
                                RequestUri = new Uri(BaseLink + DeleteReceiptLink) ,
                                Content = new StringContent(JsonConvert.SerializeObject(parameter) ,Encoding.UTF8 ,"application/json")
                            })
                            {
                                var result = await clientautho.SendAsync(request);
                                string responseBody = await result.Content.ReadAsStringAsync();
                                var responseResult = new MisaResponse()
                                {
                                    refguid = parameter.voucher[0].org_refid ,
                                    contentResult = responseBody
                                };
                                return Content(JsonConvert.SerializeObject(responseResult));

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
                    List<MisaCallBackParaDetail> data = JsonConvert.DeserializeObject<List<MisaCallBackParaDetail>>(param.data);
                    switch (param.data_type)
                    {
                        case 1: //Save
                            return await HandleCallBack(false ,data);
                        case 2: //Delete
                            return await HandleCallBack(true ,data);
                        default:
                            result.Success = false;
                            result.ErrorCode = "InvalidParam";
                            result.ErrorMessage = "Do not support callback type";
                            break;
                    }
                    return Content(JsonConvert.SerializeObject(result));
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorCode = "Exception";
                    result.ErrorMessage = "Internal server error";
                    return Content(JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result.Success = false;
                result.ErrorCode = "No Support";
                result.ErrorMessage = "No Support";
                return Content(JsonConvert.SerializeObject(result));
            }
        }

        private async Task<IActionResult> HandleCallBack(bool isDelete ,List<MisaCallBackParaDetail> data)
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
                    foreach (MisaCallBackParaDetail item in data)
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
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_MisaAccounting_UpdateResult]" ,CommandType.StoredProcedure ,
                            "@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0 ? (DataTable)dtMain : null));
                    }
                    if (dt != null && dt.Rows[0]["Result"].ToString() == "1")
                    {
                        var result = new MisaCallBackResponse()
                        {
                            Success = true ,
                            ErrorCode = "" ,
                            ErrorMessage = ""
                        };
                        return Content(JsonConvert.SerializeObject(result));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                return Content("");
            }
            catch
            {
                var result = new MisaCallBackResponse()
                {
                    Success = false ,
                    ErrorCode = "Exception" ,
                    ErrorMessage = "Internal server error"
                };
                return Content(JsonConvert.SerializeObject(result));
            }
        }
        #endregion
    }
}
