using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SMS;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SMSController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public SMSController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> Send(SMSContent info)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    if (info.znssms == "0") return Content(await SMSAction.SendExecute(info ,HttpContext));
                    else return Content(await SMSAction.SendZNSExecute(info ,HttpContext));
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("SendMulti")]
        [HttpPost]
        public async Task<IActionResult> SendMulti(SMSMarMulti info)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    return Content(await SMSAction.SendMultiExecute(info ,HttpContext));
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("Pending")]
        [HttpPost]
        public async Task<IActionResult> Pending(SMSPending sms)
        {
            try
            {

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_Insert]" ,CommandType.StoredProcedure ,
                        "@Customer_ID" ,SqlDbType.Int ,sms.customerID ,
                        "@Content" ,SqlDbType.NVarChar ,sms.content.Replace("'" ,"").Trim() ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@Type" ,SqlDbType.Int ,sms.typecare ,
                        "@TicketID" ,SqlDbType.Int ,sms.ticketID ,
                        "@Phone" ,SqlDbType.NVarChar ,sms.phone ,
                        "@AppID" ,SqlDbType.Int ,sms.appID ,
                        "@Value" ,SqlDbType.NVarChar ,sms.amount ,
                        "@Branch" ,SqlDbType.NVarChar ,user.sys_branchID ,
                        "@StudentID" ,SqlDbType.Int ,Convert.ToInt32(sms.studentID) ,
                        "@znssms" ,SqlDbType.Int ,Convert.ToInt32(sms.znssms)
                    );
                    return Content(dt.Rows[0][0].ToString());
                }

            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("SMSUpdateState")]
        [HttpPost]
        public async Task<IActionResult> SMSUpdateState(SMSSuccess sms)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_UpdateStatus]" ,CommandType.StoredProcedure ,
                         "@idsms" ,SqlDbType.Int ,sms.idsms
                         ,"@status" ,SqlDbType.Int ,sms.status);
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        #region // CALLBACK SMS 
        [AllowAnonymous]
        [Route("SMSCallback")]
        [HttpPost]
        public async Task<IActionResult> SMSCallback(SMSResultCallback sms)
        {
            try
            {
                string Secret = (Request.Headers["Secret"].Count() > 0) ? Request.Headers["Secret"] : "";
                if (Secret == Settings.Secret)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_UpdateStatusv2]"
                            ,CommandType.StoredProcedure ,
                             "@idsms" ,SqlDbType.Int ,sms.id
                             ,"@status" ,SqlDbType.Int ,sms.status
                             ,"@des" ,SqlDbType.NVarChar ,sms.description
                             );
                    }
                    return Content("1");
                }
                return Content("0");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        [AllowAnonymous]
        [Route("SMSMultiCallback")]
        [HttpPost]
        public async Task<IActionResult> SMSMultiCallback(SMSResultCallbackMulti sms)
        {
            try
            {
                string Secret = (Request.Headers["Secret"].Count() > 0) ? Request.Headers["Secret"] : "";
                if (Secret == Settings.Secret)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_UpdateStatusmulti]"
                            ,CommandType.StoredProcedure ,
                             "@phone" ,SqlDbType.NVarChar ,sms.phone
                             ,"@content" ,SqlDbType.NVarChar ,sms.content
                             ,"@status" ,SqlDbType.NVarChar ,sms.status
                             ,"@des" ,SqlDbType.NVarChar ,sms.description
                             ,"@IsZNS" ,SqlDbType.Int ,(sms.iszns != null && sms.iszns != "") ? sms.iszns : "0"
                             );
                    }
                    return Content("1");
                }
                return Content("0");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        #endregion

        #region // AFTER ACTION
        [Authorize]
        [Route("AfterPaid")]
        [HttpPost]
        public async Task<IActionResult> AfterPaid(SMSAfterPaid afterPaid)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var user = Session.GetUserSession(HttpContext);
                    var template = await SMSAction.GetTemplate(new SMSTemplate()
                    {
                        national_id = 0 ,
                        template_id = 0 ,
                        template_type = afterPaid.templatetype
                    });

                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()) && template.AllowSendPay == 1)
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = afterPaid.customerid ,
                            deposit_id = afterPaid.depositid ,
                            custcard_id = afterPaid.cardid ,
                            paymentcard_id = afterPaid.paymentcardid ,
                            paymentmed_id = afterPaid.paymentmedid ,
                            payment_id = afterPaid.paymentid ,
                            stupayment_id = afterPaid.stupaymentid,
                            template_type = afterPaid.templatetype
                        });

                        if (templatekey != null && templatekey.Rows.Count > 0)
                        {
                            DataRow dataKey = templatekey.Rows[0];
                            string smsContent = Comon.Comon.GetRepTemp(templatekey ,template.Content.ToString());
                            string smsPhone = templatekey.Columns.Contains("Phone") ? dataKey["Phone"].ToString() : "";
                            string smsAmount = templatekey.Columns.Contains("Amount") ? dataKey["Amount"].ToString() : "0";
                            SMSContent smsExecute = new SMSContent()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                Brandname = "" ,
                                CallbackID = "0" ,
                                CallbackUrl = ""
                            };
                            SMSContentPending smsExecutePending = new SMSContentPending()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                CustomerID = afterPaid.customerid.ToString() ,
                                UserID = user.sys_userid.ToString() ,
                                Amount = smsAmount ,
                                BranchID = user.sys_branchID.ToString() ,
                                IsZNS = template.IsZNS
                            };
                            return Content(await SMSAction.SendExecuteProcess(smsExecute ,smsExecutePending ,HttpContext));
                        }
                    }
                    return Content("0");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("AfterChange")]
        [HttpPost]
        public async Task<IActionResult> AfterChange(SMSAfterChangeCard afterchange)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var user = Session.GetUserSession(HttpContext);

                    var template = await SMSAction.GetTemplate(new SMSTemplate()
                    {
                        national_id = 0 ,
                        template_id = 0 ,
                        template_type = afterchange.templatetype
                    });
                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()) && template.AllowSendPay == 1)
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = afterchange.customerid ,
                            deposit_id = 0 ,
                            custcard_id = afterchange.cardid ,
                            payment_id = 0 ,
                            stupayment_id = 0
                        });
                        if (templatekey != null && templatekey.Rows.Count > 0)
                        {
                            DataRow dataKey = templatekey.Rows[0];
                            string smsAmount = !String.IsNullOrEmpty(afterchange.amount.ToString()) ? afterchange.amount.ToString() : "0";

                            if (templatekey.Columns.Contains("Amount") && templatekey.Rows.Count > 0)
                            {
                                templatekey.Rows[0]["Amount"] = smsAmount;
                            }
                            string smsContent = Comon.Comon.GetRepTemp(templatekey ,template.Content.ToString());
                            string smsPhone = templatekey.Columns.Contains("Phone") ? dataKey["Phone"].ToString() : "";
                            SMSContent smsExecute = new SMSContent()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                Brandname = "" ,
                                CallbackID = "0" ,
                                CallbackUrl = ""
                            };
                            SMSContentPending smsExecutePending = new SMSContentPending()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                CustomerID = afterchange.customerid.ToString() ,
                                UserID = user.sys_userid.ToString() ,
                                Amount = smsAmount ,
                                BranchID = user.sys_branchID.ToString() ,
                                IsZNS = template.IsZNS
                            };
                            return Content(await SMSAction.SendExecuteProcess(smsExecute ,smsExecutePending ,HttpContext));
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
        [Route("AfterAppointment")]
        [HttpPost]
        public async Task<IActionResult> AfterAppointment(SMSAfterAppointment afterApp)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    var user = Session.GetUserSession(HttpContext);
                    var template = await SMSAction.GetTemplate(new SMSTemplate()
                    {
                        national_id = 0 ,
                        template_id = 0 ,
                        template_type = afterApp.templatetype
                    });
                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()) && template.AllowSendApp == 1)
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = afterApp.customerid ,
                            app_id = afterApp.appid ,
                            template_type = afterApp.templatetype
                        });
                        if (templatekey != null && templatekey.Rows.Count > 0)
                        {
                            DataRow dataKey = templatekey.Rows[0];
                            string smsContent = Comon.Comon.GetRepTemp(templatekey ,template.Content.ToString());
                            string smsPhone = templatekey.Columns.Contains("Phone") ? dataKey["Phone"].ToString() : "";
                            SMSContent smsExecute = new SMSContent()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                Brandname = "" ,
                                CallbackID = "0" ,
                                CallbackUrl = ""
                            };
                            SMSContentPending smsExecutePending = new SMSContentPending()
                            {
                                Phone = smsPhone ,
                                Content = smsContent ,
                                CustomerID = afterApp.customerid.ToString() ,
                                UserID = user.sys_userid.ToString() ,
                                Amount = "" ,
                                BranchID = user.sys_branchID.ToString(),
                                IsZNS = template.IsZNS
                            };
                            return Content(await SMSAction.SendExecuteProcess(smsExecute ,smsExecutePending ,HttpContext));
                        }
                    }
                    return Content("0");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #endregion

        #region // ZNS GET LOG
        [Authorize]
        [Route("SMSGetLog")]
        [HttpPost]
        public async Task<IActionResult> SMSGetLog(SMSLog sms)
        {
            try
            {
                var token = Session.GetSession(HttpContext.Session ,"Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                    _config["Jwt:Issuer"].ToString() ,token))
                {
                    if (sms.znssms == "1") return Content(await SMSAction.LogZNSExecute(sms ,HttpContext));
                    else return Content("0");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion


    }
}
