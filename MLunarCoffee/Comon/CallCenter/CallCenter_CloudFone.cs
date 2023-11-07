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
using System.Web;
using System.Xml;


namespace MLunarCoffee.Comon
{
    public static class CallCenter_CloudFone //Type6 // CloudFone
    {
        #region // Get Log By Phone
        public static string GetLogByPhone(string phone)
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

                string resulfOutbound = ByPhone("2", phone);
                string resulfInbound = ByPhone("1", phone);

                var outbound = JToken.Parse(resulfOutbound);
                var inbound = JToken.Parse(resulfInbound);
                var results = outbound.Concat(inbound).ToArray();


                if (results != null && results.Length != 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        DateTime date_Start = Convert.ToDateTime(result["ngayGoi"].ToString().Replace("T", " "));
                        DataRow drow = dtM.NewRow();
                        drow["from"] = result["soGoiDen"].ToString();
                        drow["to"] = result["soNhan"].ToString();
                        drow["direction"] = result["typecall"].ToString();
                        drow["duration"] = result["thoiGianThucGoi"].ToString();
                        drow["state"] = result["trangThai"].ToString();
                        drow["recordUrl"] = (result["linkFile"].ToString() != "" ? result["linkFile"].ToString() : "null");
                        drow["start"] = date_Start.ToString("dd-MM-yyyy HH:mm:ss");
                        dtM.Rows.Add(drow);
                    }
                }
                return JsonConvert.SerializeObject(dtM);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static string ByPhone(string typeBound, string phone) 
        {
            try
            {

                string url = String.Format(@"{0}/api/CloudFone/GetCallHistory"
                     , Global.sys_APIbaseurl
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string CallNum = (typeBound == "1" ? phone : ""); //gọi vào 
                string ReceiveNum = (typeBound == "2" ? phone : "");//gọi ra 

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{"
                                     + "\"ServiceName\" :\"" + Global.sys_DomainAPI + "\","
                                     + "\"AuthUser\" :\"" + Global.sys_CallUserName + "\","
                                     + "\"AuthKey\" :\"" + Global.sys_CallPassword + "\","
                                     + "\"TypeGet\" :\"0\","
                                     + "\"DateStart\" :\"" + "\","
                                     + "\"DateEnd\" :\"" + "\","
                                     + "\"CallNum\" :\""+ CallNum + "\","
                                     + "\"ReceiveNum\" :\"" + ReceiveNum + "\","
                                     + "\"Key\" :\"" + "\","
                                     + "\"PageIndex\" :\"1\","
                                     + "\"PageSize\" :\"200\","
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
                    var resulf = obj["data"];
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
        public static string GetLogByExtension(string phone)
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

                string resulfOutbound = ByExtension("2", phone);
                string resulfInbound = ByExtension("1", phone);

                var outbound = JToken.Parse(resulfOutbound);
                var inbound = JToken.Parse(resulfInbound);
                var results = outbound.Concat(inbound).ToArray();

                if (results != null && results.Length != 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        DateTime date_Start = Convert.ToDateTime(result["ngayGoi"].ToString().Replace("T", " "));
                        DataRow drow = dtM.NewRow();
                        drow["from"] = result["soGoiDen"].ToString();
                        drow["to"] = result["soNhan"].ToString();
                        drow["direction"] = result["typecall"].ToString();
                        drow["duration"] = result["thoiGianThucGoi"].ToString();
                        drow["state"] = result["trangThai"].ToString();
                        drow["recordUrl"] = (result["linkFile"].ToString() != "" ? result["linkFile"].ToString() : "null");
                        drow["start"] = date_Start.ToString("yyyy-MM-dd HH:mm:ss");
                        dtM.Rows.Add(drow);
                    }
                }
                return JsonConvert.SerializeObject(dtM);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static string ByExtension(string typeBound, string extension)
        {
            try
            {

                string url = String.Format(@"{0}/api/CloudFone/GetCallHistory"
                   , Global.sys_APIbaseurl
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string CallNum = (typeBound == "2" ? extension : ""); //gọi vào 
                string ReceiveNum = (typeBound == "1" ? extension : "");//gọi ra 

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{"
                                     + "\"ServiceName\" :\"" + Global.sys_DomainAPI + "\","
                                     + "\"AuthUser\" :\"" + Global.sys_CallUserName + "\","
                                     + "\"AuthKey\" :\"" + Global.sys_CallPassword + "\","
                                     + "\"TypeGet\" :\"0\","
                                     + "\"DateStart\" :\"" + "\","
                                     + "\"DateEnd\" :\"" + "\","
                                     + "\"CallNum\" :\"" + CallNum + "\","
                                     + "\"ReceiveNum\" :\"" + ReceiveNum + "\","
                                     + "\"Key\" :\"" + "\","
                                     + "\"PageIndex\" :\"1\","
                                     + "\"PageSize\" :\"200\","
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
                    var resulf = obj["data"];
                    return resulf.ToString();
                }
            }
            catch (Exception ex)
            {
                return "[]";
            }
        }
        #endregion

        #region // Get Log By Date
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

                var results = JToken.Parse(ByDate("0", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"))).ToArray();
                if (results != null && results.Length != 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        Regex regex = new Regex("\\<[^\\>]*\\>");
                        string from = regex.Replace(HttpUtility.HtmlDecode(result["soGoiDen"].ToString()), String.Empty);
                        from = Regex.Replace(from, "[^0-9]", String.Empty);
                        DateTime date_Start = Convert.ToDateTime(result["ngayGoi"].ToString().Replace("T", " "));
                        DataRow drow = dtM.NewRow();
                        drow["from"] = from;
                        drow["to"] = result["soNhan"].ToString();
                        drow["direction"] = (result["typecall"] != null ? result["typecall"].ToString() : "");
                        drow["duration"] = result["thoiGianThucGoi"].ToString();
                        drow["state"] = result["trangThai"].ToString();
                        drow["recordUrl"] = (result["linkFile"].ToString() != "" ? result["linkFile"].ToString() : "null");
                        drow["start"] = date_Start.ToString("yyyy-MM-dd HH:mm:ss");
                        dtM.Rows.Add(drow);
                    }
                }

                return JsonConvert.SerializeObject(dtM);
            }
            catch (Exception ex)
            {
                return "";
            }
        } 

        private static string ByDate(string typeBound, string datefrom, string dateto)
        {
            try
            {
                string url = String.Format(@"{0}/api/CloudFone/GetCallHistory"
                   , Global.sys_APIbaseurl
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{"
                                     + "\"ServiceName\" :\"" + Global.sys_DomainAPI + "\","
                                     + "\"AuthUser\" :\"" + Global.sys_CallUserName + "\","
                                     + "\"AuthKey\" :\"" + Global.sys_CallPassword + "\","
                                     + "\"TypeGet\" :\""+ typeBound + "\"," 
                                     + "\"DateStart\" :\"" + datefrom + "\","
                                     + "\"DateEnd\" :\"" + dateto + " 23:59:00\","
                                     + "\"CallNum\" :\"" + "\","
                                     + "\"ReceiveNum\" :\""  + "\","
                                     + "\"Key\" :\"" + "\","
                                     + "\"PageIndex\" :\"1\","
                                     + "\"PageSize\" :\"200\","
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
                    var resulf = obj["data"];
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
