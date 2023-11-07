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
using System.Web;
using System.Xml;


namespace MLunarCoffee.Comon
{
    public static class CallCenter_SENBAC
    {
        #region // Get Data By Date
        public static string GetLogByDate(string date)
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

                string response = GetLog(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), "", "");


                JToken results = JToken.Parse(response);

                if (results["body"].ToArray().Length <= 0)
                {
                    return "0";
                }

                for (int i = 0; i < results["body"].ToArray().Length; i++)
                {
                    JToken result = results["body"][i];
                    DateTime start = DateTime.ParseExact(result["calldate"].ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    var startString = start.ToString("yyyy-MM-dd HH:mm:ss");
                    DataRow drow = dtM.NewRow();
                    drow["from"] = result["src"].ToString();
                    drow["to"] = result["dst"].ToString();
                    drow["direction"] = (result.Contains("direction") ? result["direction"].ToString() : "");
                    drow["duration"] = result["billsec"].ToString();
                    drow["state"] = result["disposition"].ToString();
                    drow["recordUrl"] = result["recording_file"].ToString();
                    drow["start"] = startString;
                    dtM.Rows.Add(drow);

                }
                return JsonConvert.SerializeObject(dtM);

                return "";
            }
            catch (Exception ex)
            {
                return "";
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

                string outbound = GetLog("", "", phone, "outbound");
                string inbound = GetLog("", "", phone, "inbound");
                JToken Joutbound = JToken.Parse(outbound);
                JToken Jinbound = JToken.Parse(inbound);

                var results = Joutbound["body"].ToArray().Concat(Jinbound["body"].ToArray());

                if (results.ToArray().Length <= 0)
                {
                    return "0";
                }

                for (int i = 0; i < results.ToArray().Length; i++)
                {
                    JToken result = results.ToArray()[i];
                    DateTime start = DateTime.ParseExact(result["calldate"].ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    var startString = start.ToString("yyyy-MM-dd HH:mm:ss");

                    DataRow drow = dtM.NewRow();
                    drow["from"] = result["src"].ToString();
                    drow["to"] = result["dst"].ToString();
                    drow["direction"] = (result.Contains("direction") ? result["direction"].ToString() : "");
                    drow["duration"] = result["billsec"].ToString();
                    drow["state"] = result["disposition"].ToString();
                    drow["recordUrl"] = result["recording_file"].ToString();
                    drow["start"] = startString;
                    dtM.Rows.Add(drow);

                }
                return JsonConvert.SerializeObject(dtM);

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        private static string GetLog(string datefrom, string dateto, string extension, string type)
        {
            try
            {
                string username = Global.sys_CallUserName.ToString();
                string password = Global.sys_CallPassword.ToString();
                string url = Global.sys_APIbaseurl;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"" + username + "\","
                                      + "\"password\":\"" + password + "\","
                                      + "\"fromDate\":\"" + datefrom + "\","
                                      + "\"toDate\":\"" + dateto + "\","
                                      + "\"src\":\"" + (type == "outbound" ? extension : "") + "\","
                                      + "\"dst\":\"" + (type == "inbound" ? extension : "") + "\","
                                      + "\"page\":\"1\","
                                      + "\"limit\":\"500\""
                                      + "}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                    if (tokenResult["success"].ToString() == "True")
                    {
                        return tokenResult["cdr"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
