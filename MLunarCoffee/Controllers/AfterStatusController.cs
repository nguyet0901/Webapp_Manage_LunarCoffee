using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.AfterStatus;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SMS;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using MLunarCoffee.Models.Model.AfterStatusPara;
using MLunarCoffee.Models.Portal;

namespace MLunarCoffee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AfterStatusController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public AfterStatusController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        
        [Route("bookingSchedule")]
        [HttpPost]
        public async Task<IActionResult> bookingSchedule(AfterStatusPara afterStatus)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Schedule_Next_Customer" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt64(afterStatus?.custID ?? "0"));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("evaluate")]
        [HttpPost]
        public async Task<IActionResult> evaluate(AfterStatusPara afterStatus)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_PortalRating_LoadType" ,CommandType.StoredProcedure
                        ,"@Customer" ,SqlDbType.Int ,afterStatus.custID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [HttpPost("evaluateInsert")]
        public async Task<IActionResult> evaluateInsert(Ratingus rat)
        {
            try
            {
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

        [Route("evaluateInPortal")]
        [HttpPost]
        public async Task<IActionResult> evaluateInPortal(AfterStatusPara afterStatus)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Portal_GetProfileInDay" ,CommandType.StoredProcedure
                        ,"@BranchID" ,SqlDbType.Int ,afterStatus.branchID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("treatinday")]
        [HttpPost]
        public async Task<IActionResult> treatinday(AfterStatusPara afterStatus)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Customer_LoadInfo_TreatmentInDay" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,afterStatus.custID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Route("sendSMSTreat")]
        [HttpPost]
        public async Task<IActionResult> sendSMSTreat(AfterStatusPara para)
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
                        template_id = para.templateID ,
                        template_type = ""
                    });

                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()))
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = Convert.ToInt32(para.custID) ,
                            template_type = "treatinday"
                        });
                        if (templatekey != null && templatekey.Rows.Count > 0)
                        {
                            List<SMSMareach> items = new List<SMSMareach>();
                            for (int i = 0; i < templatekey.Rows.Count; i++)
                            {
                                DataRow dataKey = templatekey.Rows[i];
                                if (templatekey.Columns.Contains("TreatProcess") && templatekey.Rows.Count > 0)
                                {
                                    templatekey.Rows[i]["TreatProcess"] = AfterStatus.getTreatProcess(Convert.ToDouble(dataKey["TreatPercentDetail"])
                                        ,Convert.ToDouble(dataKey["PercentOfService"])
                                        ,Convert.ToString(dataKey["TeethChoosing"])
                                        ,Convert.ToInt32(dataKey["Quantity"])
                                        ,Convert.ToInt32(dataKey["TreatIndex"])
                                        ,Convert.ToInt32(dataKey["TimeToTreatment"]));
                                }
                                string smsContent = Comon.Comon.GetRepTemp(Comon.Comon.ConvertRowsToDictionary(templatekey, dataKey) ,template.Content.ToString());
                                string smsPhone = templatekey.Columns.Contains("Phone") ? dataKey["Phone"].ToString() : "";
                                string smsAmount = templatekey.Columns.Contains("Amount") ? dataKey["Amount"].ToString() : "0";
                                items.Add(new SMSMareach()
                                {
                                    Phone = smsPhone ,
                                    Content = smsContent
                                });
                            }
                            if (items != null && items.Count != 0)
                            {
                                SMSMareach[] SmsItem = items.ToArray();
                                SMSMarMulti listsend = new SMSMarMulti()
                                {
                                    SmsItem = SmsItem,
                                };
                                if(template.IsZNS == 0)
                                    return Content(await SMSAction.SendMultiExecute(listsend ,HttpContext));
                                else
                                    return Content(await SMSAction.SendZNSMultiExecute(listsend ,HttpContext));
                            }
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

        [Route("sendSMSEval")]
        [HttpPost]
        public async Task<IActionResult> sendSMSEval(AfterStatusPara para)
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
                        template_id = para.templateID ,
                        template_type = ""
                    });

                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()))
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = Convert.ToInt32(para.custID) ,
                            template_type = "evaluating"
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
                                CustomerID = para.custID ,
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

        [Route("sendSMSNextSchedule")]
        [HttpPost]
        public async Task<IActionResult> sendSMSNextSchedule(AfterStatusPara para)
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
                        template_id = para.templateID,
                        template_type = ""
                    });

                    if (template != null && !String.IsNullOrEmpty(template.Content.ToString()))
                    {
                        var templatekey = await SMSAction.GetKeyTemplate(new SMSKeyTemplate()
                        {
                            cust_id = Convert.ToInt32(para.custID) ,
                            template_type = "NextSchedule"
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
                                CustomerID = para.custID,
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

    }
}
