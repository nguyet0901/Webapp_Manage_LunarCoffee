using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MLunarCoffee.Models.Model.IPAddress;
using MLunarCoffee.Comon.Crypt;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;
using MLunarCoffee.Models.Invoice;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MInvoice : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        // private string Baselink = Comon.Global.sys_Einvoice_Url;
       // private string Authorlink = "/api/Account/Login";
        private string Syntaxlink = "/api/Invoice68/GetTypeInvoiceSeries";
        private string Executelink32 = "/api/InvoiceApi78/Save";
        private string Executelink78 = "/api/InvoiceApi78/SaveV2";
        public MInvoice(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }
        //[Authorize]
        //[Route("Author")]
        //[HttpPost]
        //public async Task<IActionResult> Author()
        //{
        //    try
        //    {
        //        GlobalUser user = Session.GetUserSession(HttpContext);
        //        string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
        //        var token = Session.GetSession(HttpContext.Session, "Token");
        //        if (token == null)
        //        {
        //            return Content("0");
        //        }
        //        if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
        //            _config["Jwt:Issuer"].ToString(), token))
        //        {
        //            MInvoiceauthor author = new MInvoiceauthor()
        //            {
        //                username = Comon.Global.sys_HookEInvoice.Invoice_Username,
        //                password = Comon.Global.sys_HookEInvoice.Invoice_Password
        //            };
        //            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
        //            {
        //                var content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");
        //                var result = await clientautho.PostAsync(Authorlink, content);
        //                string responseBody = await result.Content.ReadAsStringAsync();
        //                return Content(responseBody);
        //            }
        //        }
        //        return Content("0");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        [Authorize]
        [Route("Syntax")]
        [HttpPost]
        public async Task<IActionResult> Syntax(EInvoicesyntax obj)
        {
            try
            {
                //GlobalUser user = Session.GetUserSession(HttpContext);
                string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {
                    if (obj.type != 0) Syntaxlink = Syntaxlink + "?type=" + obj.type;
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
                    {
                        clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + obj.token);
                        var result = await clientautho.GetAsync(Syntaxlink);
                        string responseBody = await result.Content.ReadAsStringAsync();
                        return Content(responseBody);
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #region // Issuse invoice
        [Authorize]
        [Route("EInvoice")]
        [HttpPost]
        public async Task<IActionResult> EInvoice(EInvoice obj)
        {
            try
            {
                string typett = obj.typett;
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataSet ds = new DataSet();
                        ds = await confunc.ExecuteDataSet("YYY_sp_Einvoice_Paymentservice", CommandType.StoredProcedure
                             , "@PaymentID", SqlDbType.Int, obj.paymentid);
                        if (ds != null)
                        {
                            DataTable dtResult = ds.Tables[0];
                            if (dtResult.Rows[0][0].ToString() == "1")
                            {
                                switch (typett)
                                {
                                    case "tt32":
                                        {
                                            return await EInvoice32(obj, ds);
                                        }
                                        break;
                                    case "tt78":
                                        {
                                            return await EInvoice78(obj, ds);
                                        }
                                        break;
                                    default:
                                        {
                                            return Content("0");
                                        }
                                        break;
                                }
                            }
                            else return Content("0");
                        }
                        else return Content("0");
                    }



                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        private async Task<IActionResult> EInvoice32(EInvoice obj, DataSet ds)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                DataTable dtDetail = ds.Tables[1];
                DataTable dtMaster = ds.Tables[2];
                if (dtMaster.Rows.Count != 1) return Content("0");
                if (dtDetail.Rows.Count == 0) return Content("0");
                M_Detail32[] m_details = new M_Detail32[dtDetail.Rows.Count];
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    M_Detail32 m_detail = new M_Detail32()
                    {
                        tchat = Convert.ToInt32(dtDetail.Rows[i]["Type"].ToString()),
                        stt_rec0 = dtDetail.Rows[i]["Index"].ToString(),
                        inv_itemCode = dtDetail.Rows[i]["ItemCode"].ToString(),
                        inv_itemName = dtDetail.Rows[i]["ItemName"].ToString(),
                        //inv_unitCode = dtDetail.Rows[i]["UnitCode"].ToString(),
                        inv_quantity = Convert.ToDecimal(dtDetail.Rows[i]["Quantity"].ToString()),
                        inv_unitPrice = Convert.ToDecimal(dtDetail.Rows[i]["UnitPrice"].ToString()),
                        inv_discountPercentage = Convert.ToDecimal(dtDetail.Rows[i]["DiscountPercentage"].ToString()),
                        inv_discountAmount = Convert.ToDecimal(dtDetail.Rows[i]["DiscountAmount"].ToString()),
                        inv_TotalAmountWithoutVat = Convert.ToDecimal(dtDetail.Rows[i]["TotalAmountWithoutVat"].ToString()),
                        ma_thue = Convert.ToDecimal(dtDetail.Rows[i]["Vat"].ToString()),
                        inv_vatAmount = Convert.ToDecimal(dtDetail.Rows[i]["VatAmount"].ToString()),
                        inv_TotalAmount = Convert.ToDecimal(dtDetail.Rows[i]["TotalAmount"].ToString())
                    };
                    m_details[i] = m_detail;
                }
                M_DetailData32 m_Data = new M_DetailData32()
                {
                    data = m_details
                };
                M_Info32 info = new M_Info32()
                {
                    inv_invoiceIssuedDate = dtMaster.Rows[0]["IssuedDate"].ToString(),
                    inv_invoiceSeries = Global.sys_HookEInvoice.Invoice_Code, // obj.invoiceseries,
                    inv_invoiceNumber = obj.invoicenumber,
                    inv_currencyCode = dtMaster.Rows[0]["CurrencyCode"].ToString(),
                    inv_paymentMethodName = dtMaster.Rows[0]["MethodName"].ToString(),
                    inv_buyerDisplayName = dtMaster.Rows[0]["CustName"].ToString(),
                    inv_buyerAddressLine = dtMaster.Rows[0]["CustAddress"].ToString(),
                    inv_buyerEmail = dtMaster.Rows[0]["CustEmail"].ToString(),
                    ma_dt = dtMaster.Rows[0]["CustCode"].ToString(),
                    inv_discountAmount = Convert.ToDecimal(dtMaster.Rows[0]["DiscountAmount"].ToString()),
                    inv_TotalAmountWithoutVat = Convert.ToDecimal(dtMaster.Rows[0]["TotalAmountWithoutVat"].ToString()),
                    inv_vatAmount = Convert.ToDecimal(dtMaster.Rows[0]["VatAmount"].ToString()),
                    inv_TotalAmount = Convert.ToDecimal(dtMaster.Rows[0]["TotalAmount"].ToString()),
                    details = new M_DetailData32[1] { m_Data }
                };
                M_Data32 invoice = new M_Data32()
                {
                    editmode = obj.editmode,
                    data = new M_Info32[1] { info }
                };
                string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
                    clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.synthird_minvoicetoken);
                    var result = await clientautho.PostAsync(Executelink32, content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return Content(responseBody);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        private async Task<IActionResult> EInvoice78(EInvoice obj, DataSet ds)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                DataTable dtDetail = ds.Tables[1];
                DataTable dtMaster = ds.Tables[2];
                if (dtMaster.Rows.Count != 1) return Content("0");
                if (dtDetail.Rows.Count == 0) return Content("0");
                M_Detail78[] m_details = new M_Detail78[dtDetail.Rows.Count];
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    M_Detail78 m_detail = new M_Detail78()
                    {
                        tchat = Convert.ToInt32(dtDetail.Rows[i]["Type"].ToString()),
                        stt = dtDetail.Rows[i]["Index"].ToString(),
                        ma = dtDetail.Rows[i]["ItemCode"].ToString(),
                        ten = dtDetail.Rows[i]["ItemName"].ToString(),
                        sluong = Convert.ToDecimal(dtDetail.Rows[i]["Quantity"].ToString()),
                        dgia = Convert.ToDecimal(dtDetail.Rows[i]["UnitPrice"].ToString()),
                        tlckhau = Convert.ToDecimal(dtDetail.Rows[i]["DiscountPercentage"].ToString()),
                        stckhau = Convert.ToDecimal(dtDetail.Rows[i]["DiscountAmount"].ToString()),
                        thtien = Convert.ToDecimal(dtDetail.Rows[i]["TotalAmountWithoutVat"].ToString()),
                        tsuat = Convert.ToDecimal(dtDetail.Rows[i]["Vat"].ToString()),
                        tthue = Convert.ToDecimal(dtDetail.Rows[i]["VatAmount"].ToString()),
                        tgtien = Convert.ToDecimal(dtDetail.Rows[i]["TotalAmount"].ToString())
                    };
                    m_details[i] = m_detail;
                }
                M_DetailData78 m_Data = new M_DetailData78()
                {
                    data = m_details
                };
                M_Info78 info = new M_Info78()
                {
                    tdlap = dtMaster.Rows[0]["IssuedDate"].ToString(),
                    khieu = Global.sys_HookEInvoice.Invoice_Code,
                    shdon = obj.invoicenumber,
                    dvtte = dtMaster.Rows[0]["CurrencyCode"].ToString(),
                    htttoan = dtMaster.Rows[0]["MethodName"].ToString(),
                    tnmua = dtMaster.Rows[0]["CustName"].ToString(),
                    dchi = dtMaster.Rows[0]["CustAddress"].ToString(),
                    email = dtMaster.Rows[0]["CustEmail"].ToString(),
                    mnmua = dtMaster.Rows[0]["CustCode"].ToString(),
                    ttcktmai = Convert.ToDecimal(dtMaster.Rows[0]["DiscountAmount"].ToString()),
                    tgtcthue = Convert.ToDecimal(dtMaster.Rows[0]["TotalAmountWithoutVat"].ToString()),
                    tgtthue = Convert.ToDecimal(dtMaster.Rows[0]["VatAmount"].ToString()),
                    tgtttbso = Convert.ToDecimal(dtMaster.Rows[0]["TotalAmount"].ToString()),
                    details = new M_DetailData78[1] { m_Data }
                };
                M_Data78 invoice = new M_Data78()
                {
                    editmode = obj.editmode,
                    data = new M_Info78[1] { info }
                };
                //GlobalUser user = Session.GetUserSession(HttpContext);
                string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
                    clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.synthird_minvoicetoken);
                    var result = await clientautho.PostAsync(Executelink78, content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return Content(responseBody);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        #endregion

        [Authorize]
        [Route("ECancel")]
        [HttpPost]
        public async Task<IActionResult> ECancel(EInvoice obj)
        {
            try
            {
                string typett = obj.typett;
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {
                    switch (typett)
                    {
                        case "tt32":
                            {
                                return await ECancel32(obj);
                            }
                            break;
                        case "tt78":
                            {
                                return await ECancel78(obj);
                            }
                            break;
                        default:
                            {
                                return Content("0");
                            }
                            break;
                    }


                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        private async Task<IActionResult> ECancel32(EInvoice obj)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                M_CancelInfo32 info = new M_CancelInfo32()
                {
                    inv_invoiceSeries = Global.sys_HookEInvoice.Invoice_Code,
                    inv_invoiceNumber = obj.invoicenumber.ToString(),
                };

                M_CancelData32 invoice = new M_CancelData32()
                {
                    editmode = obj.editmode,
                    data = new M_CancelInfo32[1] { info }
                };
                //GlobalUser user = Session.GetUserSession(HttpContext);
                string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
                    clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.synthird_minvoicetoken);
                    var result = await clientautho.PostAsync(Executelink32, content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return Content(responseBody);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        private async Task<IActionResult> ECancel78(EInvoice obj)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                M_CancelInfo78 info = new M_CancelInfo78()
                {
                    khieu = Global.sys_HookEInvoice.Invoice_Code,
                    shdon = obj.invoicenumber.ToString(),
                };

                M_CancelData78 invoice = new M_CancelData78()
                {
                    editmode = obj.editmode,
                    data = new M_CancelInfo78[1] { info }
                };
                //GlobalUser user = Session.GetUserSession(HttpContext);
                string Baselink = Comon.Global.sys_HookEInvoice.Invoice_Url;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Baselink) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
                    clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.synthird_minvoicetoken);
                    var result = await clientautho.PostAsync(Executelink78, content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return Content(responseBody);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
