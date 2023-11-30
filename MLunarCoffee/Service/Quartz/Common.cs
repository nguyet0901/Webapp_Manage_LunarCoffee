using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Log;
using MLunarCoffee.Models;
using MLunarCoffee.Models.API.APIClient;
using MLunarCoffee.Models.Invoice;


namespace MLunarCoffee.Service.Quartz
{
    public static class Common
    {
        const int MaxBlock = 50;


        const string TokenApi = "Token";
        const string SMS_Birthday = "Birthday";
        const string SMS_RemindAppointment = "RemindAppointment";

        const string ACTION_GETTOKEN = "GETTOKEN";
        const string ACTION_BEGIN = "BEGIN";
        const string ACTION_DIVIDE = "DIVIDE";
        const string ACTION_SEND = "SEND";


        public async static void ExecuteDailyLog()
        {
            try
            {

                if (Global.sys_DBTOCATCHSQL != null && Global.sys_DBTOCATCHSQL != "")
                {
                    ExecuteDaily();
                    var task_api = GetTokenApi(Global.sys_APIClient.Url ,Global.sys_APIClient.UserName ,Global.sys_APIClient.Password);
                    await Task.WhenAll(task_api);
                    JToken apiToken = await task_api;
                    if (apiToken == null || apiToken["Token"] == null) return;
                    string APIToken = apiToken["Token"].ToString();
                    List<SMSZNSBrandName> multisms = GetBrandName(apiToken);
                    if (APIToken != null && APIToken != "" && multisms != null && multisms.Count > 0)
                    {
                        var task_birth = ExecuteBirthday(APIToken ,multisms);
                        var task_appointment = ExecuteAppointment(APIToken ,multisms);
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        public async static void ExecuteDaily()
        {
            try
            {
                await ExecuteLog().ConfigureAwait(false);
                if (Global.sys_HookEInvoice != null && Global.sys_HookEInvoice.Invoice_Brand != "")
                {
                    await Third_MInvoice().ConfigureAwait(false);
                }
                if (Global.sys_HookAccount != null && Global.sys_HookAccount.Account_Brand != "")
                {
                    await Third_Account().ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private static async Task<bool> ExecuteBirthday(string token ,List<SMSZNSBrandName> smslist)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Auto_Tool_Birthday" ,CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    DataTable dtResult = ds.Tables[0];
                    if (dtResult.Rows[0]["RESULT"].ToString() == "1")
                    {
                        await LogSystem.LogSys(SMS_Birthday, ACTION_BEGIN, "" , "").ConfigureAwait(false);
                        string template = dtResult.Rows[0]["TEAMPLATE"].ToString();
                        string templateEng = dtResult.Rows[0]["TEAMPLATEENG"].ToString();
                        DataTable dtData = ds.Tables[1];
                        foreach (var sms in smslist)
                        {
                            DataTable dt = GetDataByBrandName(dtData ,sms.sys_SMSBrandName);
                            if (dt != null && dt.Rows.Count != 0)
                            {
                                await DevideTableSMS(dt ,template ,templateEng ,token ,sms , SMS_Birthday);
                            }
                        }
                        // In normal case, having sms brandname but not custom specific config yet.
                        DataTable dtDefault = GetDataByBrandName(dtData ,"");
                        if (dtDefault != null && dtDefault.Rows.Count != 0)
                        {
                            await DevideTableSMS(dtDefault ,template ,templateEng ,token ,smslist[0] ,SMS_Birthday);
                        }

                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(SMS_Birthday, ACTION_BEGIN, "", ex.ToString()).ConfigureAwait(false);
                return false;
            }

        }
        private static async Task<bool> ExecuteAppointment(string token ,List<SMSZNSBrandName> smslist)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Auto_Tool_RemindAppointment" ,CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    DataTable dtResult = ds.Tables[0];
                    if (dtResult.Rows[0]["RESULT"].ToString() == "1")
                    {
                        await LogSystem.LogSys(SMS_RemindAppointment ,ACTION_BEGIN, "" ,"").ConfigureAwait(false);
                        string template = dtResult.Rows[0]["TEAMPLATE"].ToString();
                        string templateEng = dtResult.Rows[0]["TEAMPLATEENG"].ToString();
                        int IsZNS = Convert.ToInt32(dtResult.Rows[0]["IsZNS"].ToString());
                        DataTable dtData = ds.Tables[1];
                        foreach (var sms in smslist)
                        {
                            DataTable dt = GetDataByBrandName(dtData ,sms.sys_SMSBrandName);
                            if (dt != null && dt.Rows.Count != 0)
                            {
                                if (IsZNS == 0) await DevideTableSMS(dt ,template ,templateEng ,token ,sms , SMS_RemindAppointment);
                                else await DevideTableZNS(dt ,template ,templateEng ,token ,sms ,SMS_RemindAppointment);
                            }
                        }
                        // In normal case, having sms brandname but not custom specific config yet.
                        if (IsZNS == 0)
                        {
                            DataTable dtDefault = GetDataByBrandName(dtData ,"");
                            if (dtDefault != null && dtDefault.Rows.Count != 0)
                            {
                                await DevideTableSMS(dtDefault ,template ,templateEng ,token ,smslist[0] , SMS_RemindAppointment);
                            }
                        }


                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(SMS_RemindAppointment, ACTION_BEGIN, "Error", ex.ToString()).ConfigureAwait(false);
                return false;
            }

        }
        private static async Task<bool> ExecuteLog()
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("MLU_sp_Auto_Tool_Log" ,CommandType.StoredProcedure ,
                        "@IsOutside" , SqlDbType.Int, Global.sys_IsOutside);
                }
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        #region // Register Thirdpary
        /// <summary>
        /// Get token M invoice
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> Third_MInvoice()
        {
            try
            {
                MInvoiceauthor author = new MInvoiceauthor()
                {
                    username = Comon.Global.sys_HookEInvoice.Invoice_Username ,
                    password = Comon.Global.sys_HookEInvoice.Invoice_Password
                };
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Comon.Global.sys_HookEInvoice.Invoice_Url) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(author) ,Encoding.UTF8 ,"application/json");
                    var result = await clientautho.PostAsync("/api/Account/Login" ,content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    var jsonData = (JObject)JsonConvert.DeserializeObject(responseBody);
                    var token = jsonData["token"].Value<string>();
                    if (token != null && token != "")
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            await confunc.ExecuteDataTable("MLU_sp_Auto_ThirdPartToken" ,CommandType.StoredProcedure
                                 ,"@TypeHook" ,SqlDbType.NVarChar ,"INVOICE"
                                 ,"@Token" ,SqlDbType.NVarChar ,token
                                );
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }

        }


        /// <summary>
        /// Get token Account
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> Third_Account()
        {
            try
            {
                MisaAccountingAutho author = new MisaAccountingAutho()
                {
                    app_id = Comon.Global.sys_HookAccount.Account_Api ,
                    access_code = Comon.Global.sys_HookAccount.Account_Accesscode ,
                    org_company_code = ""
                };
                using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Comon.Global.sys_HookAccount.Account_Url) })
                {
                    var content = new StringContent(JsonConvert.SerializeObject(author) ,Encoding.UTF8 ,"application/json");
                    var result = await clientautho.PostAsync("/api/oauth/actopen/connect" ,content);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);
                    string token = jsonResponse["Data"] != null ? JObject.Parse(jsonResponse["Data"].ToString())["access_token"].ToString() : "";
                    if (token != null && token != "")
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            await confunc.ExecuteDataTable("MLU_sp_Auto_ThirdPartToken" ,CommandType.StoredProcedure
                                 ,"@TypeHook" ,SqlDbType.NVarChar ,"ACCOUNT"
                                 ,"@Token" ,SqlDbType.NVarChar ,token
                                );
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        #endregion


        #region//  Devide Table
        private static async Task<bool> DevideTableSMS(DataTable dtData ,string template ,string templateEng ,string token ,SMSZNSBrandName brand ,string type)
        {
            try
            {
                await LogSystem.LogSys(type ,ACTION_DIVIDE, "", "").ConfigureAwait(false);
                if (dtData != null && dtData.Rows.Count != 0 && template != "")
                {
                    var tables = dtData.AsEnumerable().ToChunks(MaxBlock).Select(rows => rows.CopyToDataTable());
                    if (tables != null)
                    {
                        foreach (var ta in tables)
                        {
                            if (ta != null)
                            {
                                List<SMSZNSContentEach> items = new List<SMSZNSContentEach>();
                                for (int i = 0; i < ta.Rows.Count; i++)
                                {
                                    int national = Convert.ToInt32(ta.Rows[i]["NationalID"]);
                                    string content = template;
                                    if (national == 4 || national == 0)
                                    {
                                        content = template;
                                    }
                                    else
                                    {
                                        content = templateEng;
                                    }
                                    string phone = ta.Rows[i]["Phone"].ToString();

                                    if (phone.Length >= 9 && phone.Length <= 11)
                                    {
                                        switch (type)
                                        {
                                            case SMS_Birthday:
                                                {
                                                    content = content.Replace("#CompanyName#" ,ta.Rows[i]["CompanyName"].ToString());
                                                    content = content.Replace("#CustCode#" ,ta.Rows[i]["CustCode"].ToString());
                                                    content = content.Replace("#CustName#" ,ta.Rows[i]["CustName"].ToString());
                                                }
                                                break;
                                            case  SMS_RemindAppointment:
                                                {
                                                    content = content.Replace("#AppDay#" ,ta.Rows[i]["AppDay"].ToString());
                                                    content = content.Replace("#AppHour#" ,ta.Rows[i]["AppHour"].ToString());
                                                    content = content.Replace("#BranchAddress#" ,ta.Rows[i]["BranchAddress"].ToString());
                                                    content = content.Replace("#BranchHotline#" ,ta.Rows[i]["BranchHotline"].ToString());
                                                    content = content.Replace("#CustCode#" ,ta.Rows[i]["CustCode"].ToString());
                                                    content = content.Replace("#CustName#" ,ta.Rows[i]["CustName"].ToString());
                                                    content = content.Replace("#CompanyName#" ,ta.Rows[i]["CompanyName"].ToString());
                                                    content = content.Replace("#ServiceCare#" ,ta.Rows[i]["ServiceCareName"].ToString());
                                                    content = content.Replace("#Vocative#" ,ta.Rows[i]["Vocative"].ToString());
                                                    content = content.Replace("#AppType#" ,ta.Rows[i]["AppType"].ToString());
                                                    content = content.Replace("#AppBranchName#" ,ta.Rows[i]["AppBranchName"].ToString());
                                                    content = content.Replace("#TreatIndexNext#" ,ta.Rows[i]["TreatIndexNext"].ToString());
                                                }
                                                break;
                                            default: break;
                                        }

                                        items.Add(new SMSZNSContentEach()
                                        {
                                            Phone = phone ,
                                            Content = content
                                        });
                                    }
                                }
                                if (items != null && items.Count != 0)
                                {
                                    SMSZNSContentEach[] SmsItem = items.ToArray();
                                    SMSZNSContentMulti listsend = new SMSZNSContentMulti()
                                    {
                                        SmsItem = SmsItem
                                    };
                                    await SMSAction(token ,brand ,listsend, type).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(type, ACTION_DIVIDE, "", ex.ToString()).ConfigureAwait(false);
                return false;
            }
        }

        private static async Task<bool> DevideTableZNS(DataTable dtData ,string template ,string templateEng ,string token ,SMSZNSBrandName brand ,string type)
        {
            try
            {
                await LogSystem.LogSys(type, ACTION_DIVIDE, "", "").ConfigureAwait(false);
                if (dtData != null && dtData.Rows.Count != 0 && template != "")
                {
                    var tables = dtData.AsEnumerable().ToChunks(MaxBlock).Select(rows => rows.CopyToDataTable());
                    if (tables != null)
                    {
                        foreach (var ta in tables)
                        {
                            if (ta != null)
                            {
                                List<SMSZNSContentEach> items = new List<SMSZNSContentEach>();
                                for (int i = 0; i < ta.Rows.Count; i++)
                                {
                                    int national = Convert.ToInt32(ta.Rows[i]["NationalID"]);
                                    string content = template;
                                    if (national == 4 || national == 0)
                                    {
                                        content = template;
                                    }
                                    else
                                    {
                                        content = templateEng;
                                    }
                                    string phone = ta.Rows[i]["Phone"].ToString();

                                    if (phone.Length >= 9 && phone.Length <= 11)
                                    {
                                        switch (type)
                                        {
                                            case SMS_Birthday:
                                                {
                                                    content = content.Replace("#CompanyName#" ,ta.Rows[i]["CompanyName"].ToString());
                                                    content = content.Replace("#CustCode#" ,ta.Rows[i]["CustCode"].ToString());
                                                    content = content.Replace("#CustName#" ,ta.Rows[i]["CustName"].ToString());
                                                }
                                                break;
                                            case SMS_RemindAppointment:
                                                {
                                                    content = content.Replace("#AppDay#" ,ta.Rows[i]["AppDay"].ToString());
                                                    content = content.Replace("#AppHour#" ,ta.Rows[i]["AppHour"].ToString());
                                                    content = content.Replace("#BranchAddress#" ,ta.Rows[i]["BranchAddress"].ToString());
                                                    content = content.Replace("#BranchHotline#" ,ta.Rows[i]["BranchHotline"].ToString());
                                                    content = content.Replace("#CustCode#" ,ta.Rows[i]["CustCode"].ToString());
                                                    content = content.Replace("#CustName#" ,ta.Rows[i]["CustName"].ToString());
                                                    content = content.Replace("#CompanyName#" ,ta.Rows[i]["CompanyName"].ToString());
                                                    content = content.Replace("#ServiceCare#" ,ta.Rows[i]["ServiceCareName"].ToString());
                                                    content = content.Replace("#Vocative#" ,ta.Rows[i]["Vocative"].ToString());
                                                    content = content.Replace("#AppType#" ,ta.Rows[i]["AppType"].ToString());
                                                    content = content.Replace("#AppBranchName#" ,ta.Rows[i]["AppBranchName"].ToString());
                                                    content = content.Replace("#TreatIndexNext#" ,ta.Rows[i]["TreatIndexNext"].ToString());
                                                }
                                                break;
                                            default: break;
                                        }

                                        items.Add(new SMSZNSContentEach()
                                        {
                                            Phone = phone ,
                                            Content = content
                                        });
                                    }
                                }
                                if (items != null && items.Count != 0)
                                {
                                    SMSZNSContentEach[] SmsItem = items.ToArray();
                                    SMSZNSContentMulti listsend = new SMSZNSContentMulti()
                                    {
                                        SmsItem = SmsItem
                                    };
                                    await ZNSAction(token ,brand ,listsend, type).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(type, ACTION_DIVIDE, "", ex.ToString()).ConfigureAwait(false);
                return false;
            }
        }

        #endregion

        #region // API Author
        private static async Task<JToken> GetTokenApi(string baseurl ,string username ,string password)
        {
            try
            {
                var r = await PostApi(baseurl ,"/api/author/Authomulti" ,username ,password);
                var db = JsonConvert.DeserializeObject<JToken>(r);
                await LogSystem.LogSys(TokenApi ,ACTION_GETTOKEN , r, "").ConfigureAwait(false);
                return db;
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(TokenApi ,ACTION_GETTOKEN ,"" ,ex.ToString()).ConfigureAwait(false);
                return null;
            }

        }
        private static async Task<string> PostApi(string baseurl ,string url ,string username ,string password)
        {
            try
            {
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri(baseurl) })
                {
                    APIAuthen sms = new APIAuthen() { Name = username ,Password = password ,Type = "MLunarCoffee" };
                    var content = new StringContent(JsonConvert.SerializeObject(sms) ,Encoding.UTF8 ,"application/json");
                    var result = await client.PostAsync(url ,content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return (resultContent);
                }
            }
            catch(Exception ex)
            {
                await LogSystem.LogSys(TokenApi ,ACTION_GETTOKEN ,"" ,ex.ToString()).ConfigureAwait(false);
                return null;
            }
            
        }
        #endregion

        #region // SMS Action
        private static async Task<bool> SMSAction(string token ,SMSZNSBrandName brand ,SMSZNSContentMulti sms, string type)
        {
            try
            {
                await LogSystem.LogSys(type, ACTION_SEND, "", "").ConfigureAwait(false);
                if (Global.sys_DBTOCATCHSQL.ToLower() != "vttechdemo")
                {
                    sms.CallbackMultiUrl = Comon.Global.sys_SMSCallbackSystem != null ? Global.sys_SMSCallbackSystem : "";
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                    {
                        clientautho.DefaultRequestHeaders.Add("ThirdParty" ,brand.sys_SMSThirdParty);
                        clientautho.DefaultRequestHeaders.Add("UserName" ,brand.sys_SMSUserName);
                        clientautho.DefaultRequestHeaders.Add("Password" ,brand.sys_SMSPassword);
                        clientautho.DefaultRequestHeaders.Add("BrandName" ,brand.sys_SMSBrandName);
                        clientautho.DefaultRequestHeaders.Add("ShareKey" ,brand.sys_SMSShareKey);
                        clientautho.DefaultRequestHeaders.Add("UrlLogin" ,brand.sys_SMSLogin);
                        clientautho.DefaultRequestHeaders.Add("UrlSend" ,brand.sys_SMSSendSMS);
                        clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + token.ToString());
                        var content = new StringContent(JsonConvert.SerializeObject(sms) ,Encoding.UTF8 ,"application/json");
                        var result = await clientautho.PostAsync("/api/Sms/SMSSendMulti" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(type, ACTION_SEND, "", ex.ToString()).ConfigureAwait(false);
                return false;
            }

        }
        private static async Task<bool> ZNSAction(string token ,SMSZNSBrandName brand ,SMSZNSContentMulti sms, string type)
        {
            try
            {
                await LogSystem.LogSys(type, ACTION_SEND, "", "").ConfigureAwait(false);
                if (Global.sys_DBTOCATCHSQL.ToLower() != "vttechdemo")
                {
                    sms.CallbackMultiUrl = Comon.Global.sys_SMSCallbackSystem != null ? Global.sys_SMSCallbackSystem : "";
                    using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                    {
                        clientautho.DefaultRequestHeaders.Add("Zuser" ,brand.sys_Zuser);
                        clientautho.DefaultRequestHeaders.Add("Zpassword" ,brand.sys_Zpassword);
                        clientautho.DefaultRequestHeaders.Add("Zid" ,brand.sys_Zid);
                        clientautho.DefaultRequestHeaders.Add("Zreqid" ,brand.sys_Zreqid);
                        clientautho.DefaultRequestHeaders.Add("Zurl" ,brand.sys_ZUrl);
                        clientautho.DefaultRequestHeaders.Add("ZThirdParty" ,brand.sys_ZThirdParty);
                        clientautho.DefaultRequestHeaders.Add("Authorization" ,"Bearer " + token.ToString());
                        var content = new StringContent(JsonConvert.SerializeObject(sms) ,Encoding.UTF8 ,"application/json");
                        var result = await clientautho.PostAsync("/api/ZNS/ZNSSendMulti" ,content);
                        string responseBody = await result.Content.ReadAsStringAsync();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await LogSystem.LogSys(type, ACTION_SEND, "", ex.ToString()).ConfigureAwait(false);
                return false;
            }
        }
        #endregion

        #region // Other
        private static List<SMSZNSBrandName> GetBrandName(JToken apiToken)
        {
            try
            {
                List<SMSZNSBrandName> multisms = new List<SMSZNSBrandName>();
                if (apiToken != null && apiToken.HasValues != false)
                {
                    DataTable dtsms = JsonConvert.DeserializeObject<DataTable>(apiToken.SelectToken("MultiSMS").ToString());
                    if (dtsms != null && dtsms.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dtsms.Rows)
                        {
                            var r = new SMSZNSBrandName();
                            r.sys_SMSThirdParty = dataRow["ThirdParty"].ToString();
                            r.sys_SMSUserName = dataRow["UserName"].ToString();
                            r.sys_SMSPassword = dataRow["Password"].ToString();
                            r.sys_SMSBrandName = dataRow["BrandName"].ToString();
                            r.sys_SMSShareKey = dataRow["ShareKey"].ToString();
                            r.sys_SMSLogin = dataRow["UrlLogin"].ToString();
                            r.sys_SMSSendSMS = dataRow["UrlSend"].ToString();
                            r.sys_Zuser = dataRow["Zuser"].ToString();
                            r.sys_Zpassword = dataRow["Zpassword"].ToString();
                            r.sys_Zid = dataRow["Zid"].ToString();
                            r.sys_Zreqid = dataRow["Zreqid"].ToString();
                            r.sys_ZThirdParty = dataRow["ZThirdParty"].ToString();
                            r.sys_ZUrl = dataRow["ZUrl"].ToString();
                            multisms.Add(r);
                        }

                    }


                }
                return multisms;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private static DataTable GetDataByBrandName(DataTable dtData ,string brandname)
        {
            try
            {
                var results = from myRow in dtData.AsEnumerable()
                              where myRow.Field<string>("SMSBrandName") == brandname
                              select myRow;
                DataTable dt = results.CopyToDataTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable ,int chunkSize)
        {
            int itemsReturned = 0;
            var list = enumerable.ToList();
            int count = list.Count;
            while (itemsReturned < count)
            {
                int currentChunkSize = Math.Min(chunkSize ,count - itemsReturned);
                yield return list.GetRange(itemsReturned ,currentChunkSize);
                itemsReturned += currentChunkSize;
            }
        }
        #endregion
    }
}
