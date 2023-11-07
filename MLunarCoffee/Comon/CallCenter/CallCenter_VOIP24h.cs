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
    public static class CallCenter_VOIP24h //Type8 // VOIP24h
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

                var results = JToken.Parse(ByPhone(phone)).ToArray();
                if (results != null && results.Length != 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        DateTime date_Start = Convert.ToDateTime(result["calldate"].ToString());

                        DataRow drow = dtM.NewRow();
                        drow["from"] = result["src"].ToString();
                        drow["to"] = result["dst"].ToString();
                        drow["direction"] = result["type"].ToString();
                        drow["duration"] = result["billsec"].ToString();
                        drow["state"] = result["status"].ToString();
                        drow["recordUrl"] = (result["recording"].ToString() != "" ? result["recording"].ToString() : "null");
                        drow["start"] = result["calldate"].ToString();
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
        private static string ByPhone(string phone) 
        {
            try
            {
                string url = String.Format(@"{0}/dial/live?voip={1}&secret={2}&search={3}&length=10000"
                    , Global.sys_APIbaseurl, Global.sys_CallUserName, Global.sys_CallPassword, phone
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application /json";
                httpWebRequest.Method = "GET";


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["result"];
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
                    dateFrom = Comon.DateTimeDMY_To_YMDHHMMSS(date.Split('@')[0] + " 00:00:00");
                    dateTo = Comon.DateTimeDMY_To_YMDHHMMSS(date.Split('@')[1] + " 23:59:00");
                }
                else
                {
                    dateFrom = Comon.DateTimeDMY_To_YMDHHMMSS(date + " 00:00:00");
                    dateTo = Comon.DateTimeDMY_To_YMDHHMMSS(date + " 23:59:00");
                }
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
                string dateFromStamp = (dateFrom - epoch).TotalSeconds.ToString();
                string dateToStamp = (dateTo - epoch).TotalSeconds.ToString();
                var results = JToken.Parse(ByDate(dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), dateTo.ToString("yyyy-MM-dd HH:mm:ss"))).ToArray();
                if (results != null && results.Length != 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        var result = results[i];

                        DataRow drow = dtM.NewRow();
                        drow["from"] = result["src"].ToString();
                        drow["to"] = result["dst"].ToString();
                        drow["direction"] = result["type"].ToString();
                        drow["duration"] = result["billsec"].ToString();
                        drow["state"] = result["status"].ToString();
                        drow["recordUrl"] = (result["recording"].ToString() != "" ? result["recording"].ToString() : "null");
                        drow["start"] = result["calldate"].ToString();
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

        private static string ByDate(string datefrom, string dateto)
        {
            try
            {
                string url = String.Format(@"{0}/dial/live?voip={1}&secret={2}&date_start={3}&date_end={4}&length=10000"
                    , Global.sys_APIbaseurl, Global.sys_CallUserName, Global.sys_CallPassword, datefrom , dateto
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application /json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["result"];
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
