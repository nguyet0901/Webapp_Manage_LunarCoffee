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
    public static class CallCenter_CMC_V2 // Type 3 // CMC 2
    {
        #region // Get Log By Phone
        public static string GetLogByPhone(string phone) //Lay log theo sdt
        {
            try
            {
                DataTable dtM = new DataTable();
                dtM.Columns.Add("from", typeof(string));
                dtM.Columns.Add("to", typeof(string));
                dtM.Columns.Add("direction", typeof(string));
                dtM.Columns.Add("duration", typeof(string));
                dtM.Columns.Add("state", typeof(string));
                dtM.Columns.Add("recordUrl", typeof(string));
                dtM.Columns.Add("start", typeof(string));
                 
                if (Global.sys_CallCenterType == 3)
                { 
                    var outbound = JToken.Parse(ByPhone("outbound", phone));
                    var inbound = JToken.Parse(ByPhone("inbound", phone));
                    var results = outbound.Concat(inbound).ToArray(); 

                    if (results != null && results.Length != 0)
                    {
                        for (int i = 0; i < results.Length; i++)
                        {
                            var result = results[i];
                            
                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["extension"].ToString();
                            drow["to"] = result["phonenumber"].ToString();
                            drow["direction"] = result["direction"].ToString();
                            drow["duration"] = result["duration"].ToString();
                            drow["state"] = result["state"].ToString();
                            drow["recordUrl"] = (result["recordUrl"].ToString() != "null") ? result["recordUrl"].ToString() : "";
                            drow["start"] = result["startedAt"].ToString().Replace("T", " ");
                            dtM.Rows.Add(drow);
                        }
                    } 
                    return JsonConvert.SerializeObject(dtM);
                }
                 
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static string ByPhone(string typebound, string phone)
        {
            try
            {
                string url = String.Format(@"{0}/call-logs/{1}", Global.sys_APIbaseurl, typebound);
                DateTime date = new DateTime(2000, 01, 01);
                var dateFromTimespan = new DateTimeOffset(date).ToUnixTimeMilliseconds();
                long timespan = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Authorization"] = Global.sys_CallApi_Key;
                httpWebRequest.Headers["Domain"] = Global.sys_DomainAPI;
               
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"fromTime\":\"" + dateFromTimespan + "\""
                                  + ",\"queryTime\":\"" + timespan + "\""
                                  + ",\"phonenumber\":\"" + phone + "\""
                                  + "}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["data"]["items"];
                    return resulf.ToString();
                }

            }
            catch (Exception ex)
            {
                return "[]";
            }
        }
        #endregion

        #region // Get Log By Extension
        public static string GetLogByExtension(string extension) //Lay log theo extension
        {
            try
            {
                DataTable dtM = new DataTable();
                dtM.Columns.Add("from", typeof(string));
                dtM.Columns.Add("to", typeof(string));
                dtM.Columns.Add("direction", typeof(string));
                dtM.Columns.Add("duration", typeof(string));
                dtM.Columns.Add("state", typeof(string));
                dtM.Columns.Add("recordUrl", typeof(string));
                dtM.Columns.Add("start", typeof(string));

                if (Global.sys_CallCenterType == 3)
                {
                    var outbound = JToken.Parse(ByExtension("outbound", extension));
                    var inbound = JToken.Parse(ByExtension("inbound", extension));
                    var results = outbound.Concat(inbound).ToArray();

                    if (results != null && results.Length != 0)
                    {
                        for (int i = 0; i < results.Length; i++)
                        {
                            var result = results[i];

                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["extension"].ToString();
                            drow["to"] = result["phonenumber"].ToString();
                            drow["direction"] = result["direction"].ToString();
                            drow["duration"] = result["duration"].ToString();
                            drow["state"] = result["state"].ToString();
                            drow["recordUrl"] = (result["recordUrl"].ToString() != "null") ? result["recordUrl"].ToString() : "";
                            drow["start"] = result["startedAt"].ToString().Replace("T", " ");
                            dtM.Rows.Add(drow);
                        }
                    }
                    return JsonConvert.SerializeObject(dtM);
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static string ByExtension(string typebound, string extension)
        {
            try
            {
                string url = String.Format(@"{0}/call-logs/{1}", Global.sys_APIbaseurl, typebound);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Authorization"] = Global.sys_CallApi_Key;
                httpWebRequest.Headers["Domain"] = Global.sys_DomainAPI;
                long timespan = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                DateTime date = new DateTime(2000, 01, 01);
                var dateFromTimespan = new DateTimeOffset(date).ToUnixTimeMilliseconds();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"fromTime\":\"" + dateFromTimespan + "\""
                                  + ",\"queryTime\":\"" + timespan + "\""
                                  + ",\"extension\":\"" + extension + "\""
                                  + "}"; 
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["data"]["items"];
                    return resulf.ToString();
                }

            }
            catch (Exception ex)
            {
                return "[]";
            }
        }
        #endregion

        #region // Get Log By date
        public static string GetLogByDate(string date, string page, string extension)
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

                if (Global.sys_CallCenterType == 3)
                {
                    var outbound = JToken.Parse(ExtensionByDate("outbound", dateFrom, dateTo, extension, page));
                    var results = outbound.ToArray();

                    if (results != null && results.Length != 0)
                    {
                        for (int i = 0; i < results.Length; i++)
                        {
                            var result = results[i];
                            DateTime date_Start = Convert.ToDateTime(result["startedAt"].ToString().Replace("T", " "));
                            if (date_Start >= dateFrom && date_Start <= dateTo)
                            {
                                DataRow drow = dtM.NewRow();
                                drow["from"] = result["extension"].ToString();
                                drow["to"] = result["phonenumber"].ToString();
                                drow["direction"] = result["direction"].ToString();
                                drow["duration"] = result["duration"].ToString();
                                drow["state"] = result["state"].ToString();
                                drow["recordUrl"] = (result["recordUrl"].ToString() != "null") ? result["recordUrl"].ToString() : "";
                                drow["start"] = date_Start.ToString("dd-MM-yyyy HH:mm:ss");
                                dtM.Rows.Add(drow);
                            }
                        }
                    }

                    return JsonConvert.SerializeObject(dtM);

                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static string ExtensionByDate(string typebound, DateTime dateFrom, DateTime dateTo, string extension, string page)
        {
            try
            {
                string url = String.Format(@"{0}/call-logs/{1}", Global.sys_APIbaseurl, typebound);

                var dateFromTimespan = new DateTimeOffset(dateFrom).ToUnixTimeMilliseconds();
                var dateToTimespan = new DateTimeOffset(dateTo).ToUnixTimeMilliseconds();

                long timespan = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Authorization"] = Global.sys_CallApi_Key;
                httpWebRequest.Headers["Domain"] = Global.sys_DomainAPI;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"fromTime\":\"" + dateFromTimespan + "\""
                                  + ",\"queryTime\":\"" + dateToTimespan + "\""
                                  + ",\"extension\":\"" + extension + "\""
                                  + ",\"page\":\"" + page + "\""
                                  + "}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["data"]["items"];
                    return resulf.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }
}
