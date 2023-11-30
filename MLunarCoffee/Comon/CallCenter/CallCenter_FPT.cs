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
using System.Threading.Tasks;
using System.Web;
using System.Xml;


namespace MLunarCoffee.Comon
{
    public static class CallCenter_FPT
    { 
        #region // Get Data By Date
        public static async Task<string> GetLogByDate(string date)
        {
            try
            {
                DataTable dtM = new DataTable();
                dtM.Columns.Add("STT", typeof(string));
                dtM.Columns.Add("from", typeof(string));
                dtM.Columns.Add("to", typeof(string));
                dtM.Columns.Add("direction", typeof(string));
                dtM.Columns.Add("duration", typeof(string));
                dtM.Columns.Add("state", typeof(string));
                dtM.Columns.Add("recordUrl", typeof(string));
                dtM.Columns.Add("start", typeof(string));

                DateTime dateFrom = DateTime.MinValue;
                DateTime dateTo = DateTime.MinValue;
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = Comon.StringDMY_To_DateTime(date.Split('@')[0]);
                    dateTo = Comon.StringDMY_To_DateTime(date.Split('@')[1]).AddDays(1);
                }
                else
                {
                    dateFrom = Comon.StringDMY_To_DateTime(date);
                    dateTo = Comon.StringDMY_To_DateTime(date).AddDays(1);
                }

                string response =await GetLog(dateFrom, dateTo, "", "");


                JToken results = JToken.Parse(response);

                if (results.ToArray().Length <= 0)
                {
                    return "0";
                }

                for (int i = 0; i < results.ToArray().Length; i++)
                {
                    JToken result = results[i];

                    var date_Start = Comon.UnixTimeStampToDateTime(Convert.ToDouble(result["start"]));

                    string callee = "";
                    if (result["call_targets"] != null && result["call_targets"].ToString().Length > 0) { 
                        var call_targets = result["call_targets"];
                        var targets = JToken.Parse(call_targets.ToString());
                        var length = targets.ToArray().Length - 1;
                        callee = targets[length]["target_number"].ToString();
                    } else
                    {
                        callee = result["callee"].ToString();
                    }

                    DataRow drow = dtM.NewRow();
                    drow["from"] = result["caller"].ToString();
                    drow["to"] = callee;
                    drow["direction"] = result["direction"].ToString();
                    drow["duration"] = result["duration"].ToString();
                    drow["state"] = result["call_status"].ToString();
                    drow["recordUrl"] = Global.sys_LinkDownloadAudio.ToString();
                    drow["start"] = date_Start;
                    dtM.Rows.Add(drow); 
                }
                return JsonConvert.SerializeObject(dtM);
                 
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        #endregion
        #region // Get Data By Phone
        public static string GetLogByPhone(string phone)
        {
            try
            {
                DataTable dtM = new DataTable();
                dtM.Columns.Add("STT", typeof(string));
                dtM.Columns.Add("from", typeof(string));
                dtM.Columns.Add("to", typeof(string));
                dtM.Columns.Add("direction", typeof(string));
                dtM.Columns.Add("duration", typeof(string));
                dtM.Columns.Add("state", typeof(string));
                dtM.Columns.Add("recordUrl", typeof(string));
                dtM.Columns.Add("start", typeof(string));

                string outbound = "";// GetLog("", "", phone, "extension");
                //string inbound = GetLog("", "", phone, "phonenumber");
                JToken Joutbound = JToken.Parse(outbound);
                //JToken Jinbound = JToken.Parse(inbound);

                //var results = Joutbound["records"].ToArray().Concat(Jinbound["records"].ToArray());
                var results = Joutbound["records"].ToArray();
                if (results.ToArray().Length <= 0)
                {
                    return "0";
                }

                for (int i = 0; i < results.ToArray().Length; i++)
                {
                    JToken result = results.ToArray()[i]; 

                    DataRow drow = dtM.NewRow();
                    drow["from"] = result["caller"].ToString();
                    drow["to"] = result["callee"].ToString();
                    drow["direction"] = "";
                    drow["duration"] = result["duration"].ToString();
                    drow["state"] = result["status"].ToString();
                    drow["recordUrl"] = (result["filepath"].ToString() != "" ? Global.sys_LinkDownloadAudio.ToString() + result["filepath"].ToString() : "");
                    drow["start"] = result["start_time"].ToString();
                    dtM.Rows.Add(drow);

                }
                return JsonConvert.SerializeObject(dtM);
                 
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        private static async Task<string> GetLog(DateTime dateFrom, DateTime dateTo, string extension, string type)
        {
            try
            {
                var dateFromTimespan = new DateTimeOffset(dateFrom).ToUnixTimeSeconds();
                var dateToTimespan = new DateTimeOffset(dateTo).ToUnixTimeSeconds();

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[MLU_sp_LogCall_LoadList]", CommandType.StoredProcedure,
                        "@dateFrom", SqlDbType.NVarChar, dateFromTimespan
                      , "@dateTo", SqlDbType.NVarChar, dateToTimespan
                      , "@extension", SqlDbType.NVarChar, extension
                    );
                }
                if (dt != null)
                {
                    return JsonConvert.SerializeObject(dt);
                }
                return "[]";
            }
            catch (Exception ex)
            {
                return "[]";
            }
        }


        public static string GetLog_Audio ( Microsoft.AspNetCore.Http.HttpContext httpContext, string date, string extension, string pass ) {
            try {
                string access_token = GetAccessToken (httpContext,extension, pass);
                string url = Global.sys_UrlRecord;

                DateTime dateFrom = DateTime.MinValue;
                DateTime dateTo = DateTime.MinValue;
                if (date.Contains (" to ")) {
                    date = date.Replace (" to ", "@");
                    dateFrom = Comon.StringDMY_To_DateTime (date.Split ('@')[0]);
                    dateTo = Comon.StringDMY_To_DateTime (date.Split ('@')[1]).AddDays (1);
                }
                else {
                    dateFrom = Comon.StringDMY_To_DateTime (date);
                    dateTo = Comon.StringDMY_To_DateTime (date).AddDays (1);
                }


                if (url != "") {
                    string link_url = String.Format (@"{0}?pagination={1}&pagesize={2}&start_time={3}&end_time={4}"
                                           , url, 1, Global.sys_CallLimit, dateFrom.ToString ("yyyy-MM-dd") + " 00:00:00", dateTo.ToString ("yyyy-MM-dd") + " 00:00:00"
                                       );
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create (link_url);
                    httpWebRequest.ContentType = "application /json";
                    httpWebRequest.Headers["access_token"] = access_token;
                    httpWebRequest.Method = "GET";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        var result = streamReader.ReadToEnd ();
                        var obj = JObject.Parse (result);
                        var resulf = obj;
                        return resulf.ToString ();
                    }
                }
                return "[]";
            }
            catch (Exception ex) {
                return "[]";
            }
        }

        public static string GetAccessToken ( Microsoft.AspNetCore.Http.HttpContext httpContext,string extension, string pass ) {
            try {
                var user = Session.Session.GetUserSession (httpContext);
                string username = extension;
                string password = pass;
                string Domain = Global.sys_DomainAPI.ToString ();
                string url = Global.sys_APIbaseurl;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create (url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
                    string json = "{\"extension_number\":\"" + username + "\","
                                      + "\"sip_password\":\"" + password + "\","
                                      + "\"domain\":\"" + Domain + "\""
                                      + "}";
                    streamWriter.Write (json);
                    streamWriter.Flush ();
                    streamWriter.Close ();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
                using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                    var result = streamReader.ReadToEnd ();
                    var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>> (result);
                    if (tokenResult["access_token"].ToString ().Length > 0) {
                        return tokenResult["access_token"].ToString ();
                    }
                    else {
                        return "";
                    }
                }

            }
            catch (Exception ex) {
                return "";
            }
        }



    }
}
