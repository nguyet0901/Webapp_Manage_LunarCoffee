using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;


namespace MLunarCoffee.Comon
{
    public static class CallCenterOutsite
    {
        public static int ExecuteCall(string LinkClickToCall, string callextension, string phonenumber, string key, string domain, string callextensionpass)
        {
            try
            {
                var url = "";
                switch (Global.sys_CallCenterType)
                {
                    case 4:
                        url = CallType4(LinkClickToCall, callextension, phonenumber);
                        return ExcuteCallMethodGet(url);
                    case 6:
                        url = CallType6(LinkClickToCall, callextension, phonenumber, key, domain);
                        return ExcuteCallMethodGet(url);
                    case 9:
                        url = LinkClickToCall;
                        string json = "{\"username\":\""+ domain + "\","
                                      + "\"password\":\"" + key + "\","
                                      + "\"number\":\"" + phonenumber + "\","
                                      + "\"extension\":\"" + callextension + "\""
                                      + "}";
                        return ExcuteCallMethodPost(url, json);
                    case 10:
                        url = LinkClickToCall;
                        string jsontype10 = "{\"src\":\"" + callextension + "\","
                                      + "\"to\":\"" + phonenumber + "\","
                                      + "\"domain\":\"" + domain + "\","
                                      + "\"extension\":\"" + callextension + "\","
                                      + "\"auth\":\"" + callextensionpass + "\""
                                      + "}";
                        return ExcuteCallMethodPostType10(url, jsontype10);

                    default:
                        break;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static int ExcuteCallMethodPost(string url, string json)
        {
            try
            {
                if (url != "")
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,object>>(result);
                        if (tokenResult["success"].ToString() == "True")
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static int ExcuteCallMethodPostType10(string url, string json)
        {
            try
            {
                if (url != "")
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return 1;
                    else
                        return 0;

                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static int ExcuteCallMethodGet(string url)
        {
            try
            {
                if (url != "")
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (responseString == "Success")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static string CallType4(string LinkClickToCall, string callextension, string phonenumber)
        {
            try
            {
                var url = "";
                if(LinkClickToCall != "" && callextension != "" && phonenumber != "")
                {
                    url = LinkClickToCall + "?LineToCall=" + callextension + "&Phone=" + phonenumber;
                }

                return url;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        // VoidCound
        public static string CallType6(string LinkClickToCall, string callextension, string phonenumber, string key, string domain)
        {
            try
            {
                var url = "";
                if (LinkClickToCall != "" && callextension != "" && phonenumber != "" && key != "" && domain != "")
                {
                    url = LinkClickToCall + "/from_number/" + callextension + "/to_number/" + phonenumber + "/key/" + key + "/domain/" + domain;
                }
                return url;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #region //Log
        public static string GetOutboundLog(string phone)
        {
            try
            {
                if (Global.sys_CallCenterType == 4) return CallCenter_THIENKHUE.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 9) return CallCenter_SENBAC.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 10) return CallCenter_FPT.GetLogByPhone(phone);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static async Task<string>  GetOutboundLog_All(string date)
        {
            try
            {
                if (Global.sys_CallCenterType == 4) return CallCenter_THIENKHUE.GetLogByDate(date);
                if (Global.sys_CallCenterType == 9) return CallCenter_SENBAC.GetLogByDate(date);
                if (Global.sys_CallCenterType == 10) return await CallCenter_FPT.GetLogByDate(date);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GeLog_ByExntesion(string extension)
        {
            try
            {
                if (Global.sys_CallCenterType == 4) return CallCenter_THIENKHUE.GetLogByPhone(extension);
                if (Global.sys_CallCenterType == 9) return CallCenter_SENBAC.GetLogByPhone(extension);
                if (Global.sys_CallCenterType == 10) return CallCenter_FPT.GetLogByPhone(extension);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }

}
