using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MLunarCoffee.Models;
using MLunarCoffee.Service.Quartz;

namespace MLunarCoffee.Comon.SMS
{
    public static class SMSAction
    {
        const int MaxBlock = 50;
        public static async Task<string> SendExecute(SMSContent info ,HttpContext httpContext)
        {
            try
            {
                if (Comon.CheckContentSMS(info.Content) != "1")
                {
                    return "-1"; //Content khong hop le
                }
                else
                {
                    GlobalUser user = Session.Session.GetUserSession(httpContext);
                    if (Global.sys_HookSMSCallback != null && Global.sys_HookSMSCallback.SMS_Callback != "") info.CallbackUrl = Global.sys_HookSMSCallback.SMS_Callback;
                    List<SMSZNSBrandName> smsBrandName = user.sys_SMSBrandName;
                    string brandname = info.Brandname;
                    if (smsBrandName != null && smsBrandName.Count > 0)
                    {
                        string _ThirdParty = "", _SMSUserName = "", _SMSPassword = "", _SMSBrandName = "", _SMSShareKey = "", _SMSLogin = "", _SMSSendSMS = "";
                        if (brandname != null && brandname != "")
                        {
                            List<SMSZNSBrandName> _smsmul = smsBrandName.FindAll(e => e.sys_SMSBrandName == brandname);
                            if (_smsmul != null && _smsmul.Count == 1)
                            {
                                _ThirdParty = _smsmul[0].sys_SMSThirdParty;
                                _SMSUserName = _smsmul[0].sys_SMSUserName;
                                _SMSPassword = _smsmul[0].sys_SMSPassword;
                                _SMSBrandName = _smsmul[0].sys_SMSBrandName;
                                _SMSShareKey = _smsmul[0].sys_SMSShareKey;
                                _SMSLogin = _smsmul[0].sys_SMSLogin;
                                _SMSSendSMS = _smsmul[0].sys_SMSSendSMS;
                            }
                        }
                        else
                        {
                            SMSZNSBrandName _sms = smsBrandName[0];
                            _ThirdParty = _sms.sys_SMSThirdParty;
                            _SMSUserName = _sms.sys_SMSUserName;
                            _SMSPassword = _sms.sys_SMSPassword;
                            _SMSBrandName = _sms.sys_SMSBrandName;
                            _SMSShareKey = _sms.sys_SMSShareKey;
                            _SMSLogin = _sms.sys_SMSLogin;
                            _SMSSendSMS = _sms.sys_SMSSendSMS;
                        }


                        using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                        {
                            clientautho.DefaultRequestHeaders.Add("ThirdParty" ,_ThirdParty);
                            clientautho.DefaultRequestHeaders.Add("UserName" ,_SMSUserName);
                            clientautho.DefaultRequestHeaders.Add("Password" ,_SMSPassword);
                            clientautho.DefaultRequestHeaders.Add("BrandName" ,_SMSBrandName);
                            clientautho.DefaultRequestHeaders.Add("ShareKey" ,_SMSShareKey);
                            clientautho.DefaultRequestHeaders.Add("UrlLogin" ,_SMSLogin);
                            clientautho.DefaultRequestHeaders.Add("UrlSend" ,_SMSSendSMS);
                            clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + user.sys_TokenAPI);
                            var content = new StringContent(JsonConvert.SerializeObject(info) ,Encoding.UTF8 ,"application/json");
                            var result = await clientautho.PostAsync("/api/Sms/SMSSend" ,content);
                            string responseBody = await result.Content.ReadAsStringAsync();
                            return responseBody;
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }


            }
            catch (Exception)
            {
                return "0";
            }

        }
        public static async Task<string> SendZNSExecute(SMSContent info ,HttpContext httpContext)
        {
            try
            {

                GlobalUser user = Session.Session.GetUserSession(httpContext);
                if (Global.sys_HookSMSCallback != null && Global.sys_HookSMSCallback.SMS_Callback != "") info.CallbackUrl = Global.sys_HookSMSCallback.SMS_Callback;
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                {
                    clientautho.DefaultRequestHeaders.Add("Zuser" ,Global.sys_Zuser);
                    clientautho.DefaultRequestHeaders.Add("Zpassword" ,Global.sys_Zpassword);
                    clientautho.DefaultRequestHeaders.Add("Zid" ,Global.sys_Zid);
                    clientautho.DefaultRequestHeaders.Add("Zreqid" ,Global.sys_Zreqid);
                    clientautho.DefaultRequestHeaders.Add("Zurl" ,Global.sys_ZUrl);
                    clientautho.DefaultRequestHeaders.Add("ZThirdParty" ,Global.sys_ZThirdParty);
                    clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + user.sys_TokenAPI);
                    var content = new StringContent(JsonConvert.SerializeObject(info) ,Encoding.UTF8 ,"application/json");
                    var result = await clientautho.PostAsync("/api/Zns/ZNSSend" ,content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return responseBody;
                }

            }
            catch (Exception)
            {
                return "0";
            }

        }
        public static async Task<string> SMS_Pending(string customerid ,string userid ,string phone ,string amount ,string content ,string branchid)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_Insert]" ,CommandType.StoredProcedure ,
                        "@Customer_ID" ,SqlDbType.Int ,customerid ,
                        "@Content" ,SqlDbType.NVarChar ,content.Replace("'" ,"").Trim() ,
                        "@Created_By" ,SqlDbType.Int ,userid ,
                        "@Type" ,SqlDbType.Int ,0 ,
                        "@TicketID" ,SqlDbType.Int ,0 ,
                        "@Phone" ,SqlDbType.NVarChar ,phone ,
                        "@AppID" ,SqlDbType.Int ,0 ,
                        "@Value" ,SqlDbType.NVarChar ,amount ,
                        "@Branch" ,SqlDbType.NVarChar ,branchid ,
                        "@StudentID" ,SqlDbType.Int ,0 ,
                        "@znssms" ,SqlDbType.Int ,0
                    );
                    if (dt != null && dt.Rows.Count == 1 && dt.Rows[0][0].ToString() != "0")
                    {
                        return dt.Rows[0][0].ToString();
                    }
                    else return "0";
                }

            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static async Task<string> SMS_Pending(SMSContentPending infoPending)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_SMS_Insert]" ,CommandType.StoredProcedure ,
                        "@Customer_ID" ,SqlDbType.Int ,infoPending.CustomerID ,
                        "@Content" ,SqlDbType.NVarChar ,infoPending.Content.Replace("'" ,"").Trim() ,
                        "@Created_By" ,SqlDbType.Int ,infoPending.UserID ,
                        "@Type" ,SqlDbType.Int ,0 ,
                        "@TicketID" ,SqlDbType.Int ,0 ,
                        "@Phone" ,SqlDbType.NVarChar ,infoPending.Phone ,
                        "@AppID" ,SqlDbType.Int ,0 ,
                        "@Value" ,SqlDbType.NVarChar ,infoPending.Amount ,
                        "@Branch" ,SqlDbType.NVarChar ,infoPending.BranchID ,
                        "@StudentID" ,SqlDbType.Int ,0 ,
                        "@znssms" ,SqlDbType.Int,infoPending.IsZNS
                    );
                    if (dt != null && dt.Rows.Count == 1 && dt.Rows[0][0].ToString() != "0")
                    {
                        return dt.Rows[0][0].ToString();
                    }
                    else return "0";
                }

            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public static async Task<string> SendExecuteProcess(SMSContent infoSMS ,SMSContentPending infoPending ,HttpContext httpContext)
        {
            try
            {
                var pending = await SMS_Pending(infoPending);
                if(pending !=  null && pending != "0")
                {
                    infoSMS.CallbackID = pending.ToString();
                    if (infoPending.IsZNS == 0)
                        return await SendExecute(infoSMS ,httpContext);
                    else
                        return await SendZNSExecute(infoSMS ,httpContext);
                }
                return "0";
            }
            catch(Exception ex)
            {
                return "0";
            }
        }

        #region // Multi sms
        public static async Task<string> SendMultiExecute(SMSMarMulti sms ,HttpContext httpContext)
        {
            try
            {

                if (Global.sys_DBTOCATCHSQL.ToLower() != "vttechdemo")
                {
                    if (sms.SmsItem != null && sms.SmsItem.Length != 0)
                    {
                        var items = sms.SmsItem.AsEnumerable().ToChunks(MaxBlock).ToArray();
                        if (items != null)
                        {
                            string isSuccess = "1";
                            foreach (var item in items)
                            {
                                GlobalUser user = Session.Session.GetUserSession(httpContext);
                                sms.CallbackMultiUrl = Global.sys_SMSCallbackSystem != null ? Global.sys_SMSCallbackSystem : "";
                                sms.SmsItem = item.ToArray();
                                List<SMSZNSBrandName> smsBrandName = user.sys_SMSBrandName;
                                string brandname = sms.BrandName;
                                if (smsBrandName != null && smsBrandName.Count > 0)
                                {
                                    string _ThirdParty = "", _SMSUserName = "", _SMSPassword = "", _SMSBrandName = "", _SMSShareKey = "", _SMSLogin = "", _SMSSendSMS = "";
                                    if (brandname != null && brandname != "")
                                    {
                                        List<SMSZNSBrandName> _smsmul = smsBrandName.FindAll(e => e.sys_SMSBrandName == brandname);
                                        if (_smsmul != null && _smsmul.Count == 1)
                                        {
                                            _ThirdParty = _smsmul[0].sys_SMSThirdParty;
                                            _SMSUserName = _smsmul[0].sys_SMSUserName;
                                            _SMSPassword = _smsmul[0].sys_SMSPassword;
                                            _SMSBrandName = _smsmul[0].sys_SMSBrandName;
                                            _SMSShareKey = _smsmul[0].sys_SMSShareKey;
                                            _SMSLogin = _smsmul[0].sys_SMSLogin;
                                            _SMSSendSMS = _smsmul[0].sys_SMSSendSMS;
                                        }
                                    }
                                    else
                                    {
                                        SMSZNSBrandName _sms = smsBrandName[0];
                                        _ThirdParty = _sms.sys_SMSThirdParty;
                                        _SMSUserName = _sms.sys_SMSUserName;
                                        _SMSPassword = _sms.sys_SMSPassword;
                                        _SMSBrandName = _sms.sys_SMSBrandName;
                                        _SMSShareKey = _sms.sys_SMSShareKey;
                                        _SMSLogin = _sms.sys_SMSLogin;
                                        _SMSSendSMS = _sms.sys_SMSSendSMS;
                                    }
                                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                                    {
                                        clientautho.DefaultRequestHeaders.Add("ThirdParty" ,_ThirdParty);
                                        clientautho.DefaultRequestHeaders.Add("UserName" ,_SMSUserName);
                                        clientautho.DefaultRequestHeaders.Add("Password" ,_SMSPassword);
                                        clientautho.DefaultRequestHeaders.Add("BrandName" ,_SMSBrandName);
                                        clientautho.DefaultRequestHeaders.Add("ShareKey" ,_SMSShareKey);
                                        clientautho.DefaultRequestHeaders.Add("UrlLogin" ,_SMSLogin);
                                        clientautho.DefaultRequestHeaders.Add("UrlSend" ,_SMSSendSMS);
                                        clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + user.sys_TokenAPI);
                                        var content = new StringContent(JsonConvert.SerializeObject(sms) ,Encoding.UTF8 ,"application/json");
                                        var result = await clientautho.PostAsync("/api/Sms/SMSSendMulti" ,content);
                                        string responseBody = await result.Content.ReadAsStringAsync();
                                        isSuccess = (responseBody == "0") ? "0" : isSuccess;
                                    }
                                }
                                else isSuccess = "0";
                            }
                            return isSuccess;
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else return "0";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public static async Task<string> SendZNSMultiExecute(SMSMarMulti sms ,HttpContext httpContext)
        {
            try
            {
                if (Global.sys_DBTOCATCHSQL.ToLower() != "vttechdemo")
                {
                    if (sms.SmsItem != null && sms.SmsItem.Length != 0)
                    {
                        var items = sms.SmsItem.AsEnumerable().ToChunks(MaxBlock).ToArray();
                        if (items != null)
                        {
                            string isSuccess = "1";
                            foreach (var item in items)
                            {
                                GlobalUser user = Session.Session.GetUserSession(httpContext);
                                sms.SmsItem = item.ToArray();
                                if (Global.sys_HookSMSCallback != null && Global.sys_HookSMSCallback.SMS_Callback != "") sms.CallbackMultiUrl = Global.sys_SMSCallbackSystem;
                                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                                {
                                    clientautho.DefaultRequestHeaders.Add("Zuser" ,Global.sys_Zuser);
                                    clientautho.DefaultRequestHeaders.Add("Zpassword" ,Global.sys_Zpassword);
                                    clientautho.DefaultRequestHeaders.Add("Zid" ,Global.sys_Zid);
                                    clientautho.DefaultRequestHeaders.Add("Zreqid" ,Global.sys_Zreqid);
                                    clientautho.DefaultRequestHeaders.Add("Zurl" ,Global.sys_ZUrl);
                                    clientautho.DefaultRequestHeaders.Add("ZThirdParty" ,Global.sys_ZThirdParty);
                                    clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + user.sys_TokenAPI);
                                    var content = new StringContent(JsonConvert.SerializeObject(sms) ,Encoding.UTF8 ,"application/json");
                                    var result = await clientautho.PostAsync("/api/Zns/ZNSSendMulti" ,content);
                                    string responseBody = await result.Content.ReadAsStringAsync();
                                    isSuccess = (responseBody == "0") ? "0" : isSuccess;
                                }
                            }
                            return isSuccess;
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception)
            {
                return "0";
            }

        }

        #endregion

        #region // Get log
        public static async Task<string> LogZNSExecute(SMSLog info ,HttpContext httpContext)
        {
            try
            {

                GlobalUser user = Session.Session.GetUserSession(httpContext);
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                {
                    clientautho.DefaultRequestHeaders.Add("Zuser" ,Global.sys_Zuser);
                    clientautho.DefaultRequestHeaders.Add("Zpassword" ,Global.sys_Zpassword);
                    clientautho.DefaultRequestHeaders.Add("ZThirdParty" ,Global.sys_ZThirdParty);
                    clientautho.DefaultRequestHeaders.Add("ZUrlLog" ,Global.sys_ZUrlLog);
                    clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + user.sys_TokenAPI);
                    var content = new StringContent(JsonConvert.SerializeObject(info) ,Encoding.UTF8 ,"application/json");
                    var result = await clientautho.PostAsync("/api/zns/ZNSGetLog" ,content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return responseBody;
                }

            }
            catch (Exception)
            {
                return "0";
            }

        }
        #endregion

        #region // HANDLE GET TEMPLATE & KEY => REPLACE TEMPLATE
        public static async Task<SMSContentTemplate> GetTemplate(SMSTemplate sms)
        {
            try
            {
                DataTable dt = new DataTable();
                using (ExecuteDataBase connFunc = new ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_Option_LoadTemplate]" ,CommandType.StoredProcedure ,
                        "@NationalID" ,SqlDbType.Int ,sms.national_id ,
                        "@TemplateID" ,SqlDbType.Int ,sms.template_id ,
                        "@TemplateType" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(sms.template_type) ? sms.template_type.ToString() : "");
                }
                return GetTemplateData(dt);
            }
            catch (Exception)
            {
                return new SMSContentTemplate();
            }
        }
        public static async Task<DataTable> GetKeyTemplate(SMSKeyTemplate sms)
        {
            try
            {
                DataTable dt = new DataTable();
                switch (sms.template_type.ToLower())
                {
                    case "smscontentafterpayment":
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_SMS_AfterPayment" ,CommandType.StoredProcedure ,
                                "@CustID" ,SqlDbType.Int ,sms.cust_id ,
                                "@PaymentID" ,SqlDbType.Int ,sms.payment_id ,
                                "@PaymentCardID" ,SqlDbType.Int ,sms.paymentcard_id ,
                                "@PaymentMedID" ,SqlDbType.Int ,sms.paymentmed_id ,
                                "@DepositID" ,SqlDbType.Int ,sms.deposit_id ,
                                "@BranchID" ,SqlDbType.Int ,sms.branch_id ,
                                "@PayStudentID" ,SqlDbType.Int ,sms.stupayment_id ,
                                "@DentistCosmetic" ,SqlDbType.Int ,Global.sys_DentistCosmetic);
                        }
                        break;
                    case "smscontentafterappointment":
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_AfterSchedule]" ,CommandType.StoredProcedure ,
                                "@CustID" ,SqlDbType.Int ,sms.cust_id ,
                                "@AppID" ,SqlDbType.Int ,sms.app_id);
                        }
                        break;
                    //case "smscontentaftercashback":
                    //    using (ExecuteDataBase connFunc = new ExecuteDataBase())
                    //    {
                    //        dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_AfterSchedule]" ,CommandType.StoredProcedure ,
                    //            "@CustID" ,SqlDbType.Int ,sms.cust_id ,
                    //            "@AppID" ,SqlDbType.Int ,sms.app_id);
                    //    }
                    //    break;
                    //case "smscontentafterpaymentstudent":
                    //    using (ExecuteDataBase connFunc = new ExecuteDataBase())
                    //    {
                    //        dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_AfterSchedule]" ,CommandType.StoredProcedure ,
                    //            "@CustID" ,SqlDbType.Int, sms.cust_id,
                    //            "@AppID" ,SqlDbType.Int, sms.app_id);
                    //    }
                    //    break;

                    case "nextschedule":
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_Auto_AfterStatus_NextScheduleTemp" ,CommandType.StoredProcedure
                                ,"@CustID" ,SqlDbType.Int ,sms.cust_id);
                        }
                        break;
                    case "evaluating":
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_Auto_AfterStatus_LoadEvaluatingTemp" ,CommandType.StoredProcedure
                                ,"@CustID" ,SqlDbType.Int ,sms.cust_id);
                        }
                        break;
                    case "treatinday":
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("YYY_sp_Auto_AfterStatus_LoadTreatInDayTemp" ,CommandType.StoredProcedure
                                ,"@CustID" ,SqlDbType.Int ,sms.cust_id);
                        }
                        break;
                    default:
                        using (ExecuteDataBase connFunc = new ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_LoadKeySend]" ,CommandType.StoredProcedure ,
                                "@CustID" ,SqlDbType.Int ,sms.cust_id ,
                                "@CustCardID" ,SqlDbType.Int ,sms.custcard_id ,
                                "@AppID" ,SqlDbType.Int ,sms.app_id ,
                                "@PayStudentID" ,SqlDbType.Int ,sms.stupayment_id ,
                                "@TreatID" ,SqlDbType.Int ,sms.custtreat_id ,
                                "@BranchID" ,SqlDbType.Int ,sms.branch_id ,
                                "@PaymentID" ,SqlDbType.Int ,sms.payment_id ,
                                "@PaymentCardID" ,SqlDbType.Int ,sms.paymentcard_id ,
                                "@PaymentMedID" ,SqlDbType.Int ,sms.paymentmed_id ,
                                "@DepositID" ,SqlDbType.Int ,sms.deposit_id ,
                                "@MedicineID" ,SqlDbType.Int ,sms.medicine_id ,
                                "@TabID" ,SqlDbType.Int ,sms.tab_id ,
                                "@TreatmentDate" ,SqlDbType.DateTime ,!String.IsNullOrEmpty(sms.custtreat_date) ? Convert.ToDateTime(sms.custtreat_date) : null ,
                                "@DentistCosmetic" ,SqlDbType.Int ,Global.sys_DentistCosmetic);
                        }
                        break;
                }


                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static SMSContentTemplate GetTemplateData(DataTable dt)
        {
            try
            {
                SMSContentTemplate sms = new SMSContentTemplate();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    sms = new SMSContentTemplate()
                    {
                        Content = !String.IsNullOrEmpty(dr["Content"].ToString()) ? dr["Content"].ToString() : "" ,
                        IsZNS = Convert.ToInt32(dr["IsZNS"].ToString()) ,
                        AllowSendPay = Convert.ToInt32(dr["AllowSendPay"].ToString()) ,
                        AllowSendApp = Convert.ToInt32(dr["AllowSendApp"].ToString()) ,
                        AllowSendTab = Convert.ToInt32(dr["AllowSendTab"].ToString()) ,
                    };
                }
                return sms;
            }
            catch
            {
                return new SMSContentTemplate();
            }
        }
        #endregion

    }
}
