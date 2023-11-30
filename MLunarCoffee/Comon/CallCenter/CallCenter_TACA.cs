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
    public static class CallCenter_TACA //Type5 // TACA
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

                if (Global.sys_CallCenterType == 5)
                {
                    string resulfOutbound = ByPhone("destination_number", phone);
                    string resulfInbound = ByPhone("caller_id_number", phone);

                    var outbound = JToken.Parse(resulfOutbound);
                    var inbound = JToken.Parse(resulfInbound);
                    var results = outbound.Concat(inbound).ToArray();

                    if (results != null && results.Length != 0)
                    {
                        for (int i = 0; i < results.Length; i++)
                        {
                            var result = results[i];
                            var recordUrlMain = "null";
                            recordUrlMain = result["record_path"].ToString();
                            if (recordUrlMain != "")
                            {
                                recordUrlMain = Global.sys_UrlRecord.ToString() + recordUrlMain;
                            }
                            else
                            {
                                recordUrlMain = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["caller_id_number"].ToString();
                            drow["to"] = result["destination_number"].ToString();
                            drow["direction"] = result["direction"].ToString();
                            drow["duration"] = result["billsec"].ToString();
                            drow["state"] = result["call_status"].ToString();
                            drow["recordUrl"] = recordUrlMain;
                            drow["start"] = result["start_stamp"].ToString();
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
        private static string ByPhone(string typeBound, string phone) 
        {
            try
            { 
                string url = String.Format(@"{0}/api/v2/cdr?domain_name={1}&limit={2}&offset=0&{3}={4}"
                    , Global.sys_APIbaseurl, Global.sys_DomainAPI, Global.sys_CallLimit, typeBound, phone
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application /json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers["api-key"] = Global.sys_CallApi_Key;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var obj = JObject.Parse(result);
                    var resulf = obj["data"];
                    if (resulf.ToString() == "")
                    {
                        resulf = "[]";
                    }
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
                    dateTo = Comon.StringDMY_To_DateTime(date.Split('@')[1]);
                }
                else
                {
                    dateFrom = Comon.StringDMY_To_DateTime(date);
                    dateTo = Comon.StringDMY_To_DateTime(date);
                }

                if (Global.sys_CallCenterType == 5)
                {
                    string resulf = ByDate(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));

                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(resulf, (typeof(DataTable)));
                    var Rows = (from row in dt.AsEnumerable()
                                orderby row["start_stamp"] descending
                                select row);
                    dt = Rows.AsDataView().ToTable();

                    if (dt != null && dt.Rows.Count != 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var recordUrlMain = "null";
                            recordUrlMain = dt.Rows[i]["record_path"].ToString();
                            if (recordUrlMain != "")
                            {
                                recordUrlMain = Global.sys_UrlRecord.ToString() + recordUrlMain;
                            }
                            else
                            {
                                recordUrlMain = "null";
                            }
                            DataRow drow = dtM.NewRow();
                            drow["from"] = dt.Rows[i]["caller_id_number"].ToString();
                            drow["to"] = dt.Rows[i]["destination_number"].ToString();
                            drow["direction"] = dt.Rows[i]["direction"].ToString();
                            drow["duration"] = dt.Rows[i]["billsec"].ToString();
                            drow["state"] = dt.Rows[i]["call_status"].ToString();
                            drow["recordUrl"] = recordUrlMain;
                            drow["start"] = dt.Rows[i]["start_stamp"].ToString();
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

        private static string ByDate(string datefrom, string dateto)
        {
            try
            {                
                string url = String.Format(@"{0}/api/v2/cdr?domain_name={1}&from={2}&to={3}&limit=500&offset=0"
                    , Global.sys_APIbaseurl, Global.sys_DomainAPI, datefrom + " 00:00:00", dateto + " 23:59:59"
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application /json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers["api-key"] = Global.sys_CallApi_Key;

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
