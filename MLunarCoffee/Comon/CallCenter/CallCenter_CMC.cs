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
    public static class CallCenter_CMC //Type2 // CMC 1 ( chung thuc qua user,pass)
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

                if (Global.sys_CallCenterType == 2)
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
                            drow["start"] = Comon.UnixTimeStampToDateTime(Convert.ToDouble(result["start_epoch"]));
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
                //if (phone.Length != 10) return "[]";
                string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string url = String.Format(@"{0}:{1}/api/v2/cdrs?domain={2}&limit={3}&from=&to={4}&offset=0&{5}={6}"
                    , Global.sys_CallHost, Global.sys_CallPort, Global.sys_CallDomain, Global.sys_CallLimit, today, typeBound, phone
                );

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Credentials = new NetworkCredential(Global.sys_CallUserName, Global.sys_CallPassword);
                httpWebRequest.Method = "GET";
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
                    dateTo = Comon.StringDMY_To_DateTime(date.Split('@')[1]).AddDays(1);
                }
                else
                {
                    dateFrom = Comon.StringDMY_To_DateTime(date);
                    dateTo = Comon.StringDMY_To_DateTime(date).AddDays(1);
                }

                if (Global.sys_CallCenterType == 2)
                {
                    string resulf = ByDate(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(resulf, (typeof(DataTable)));
                    var Rows = (from row in dt.AsEnumerable()
                                orderby row["start_epoch"] descending
                                select row);
                    dt = Rows.AsDataView().ToTable();
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DateTime date_Start = Comon.UnixTimeStampToDateTime_ToDT(Convert.ToDouble(dt.Rows[i]["start_epoch"]));
                            if (date_Start >= dateFrom && date_Start <= dateTo)
                            {
                                DataRow drow = dtM.NewRow();
                                drow["from"] = dt.Rows[i]["caller_id_number"].ToString();
                                drow["to"] = dt.Rows[i]["destination_number"].ToString();
                                drow["direction"] = dt.Rows[i]["direction"].ToString();
                                drow["duration"] = dt.Rows[i]["billsec"].ToString();
                                drow["state"] = dt.Rows[i]["call_status"].ToString();
                                string filename = (dt.Rows[i].Table.Columns.Contains("record_path")) ? dt.Rows[i]["record_path"].ToString() : "";
                                drow["recordUrl"] = (filename != "null" && filename != "") ? String.Format(@"{0}{1}", Global.sys_UrlRecord, filename) : "null";

                                drow["start"] = Comon.UnixTimeStampToDateTime(Convert.ToDouble(dt.Rows[i]["start_epoch"]));
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

        private static string ByDate(string datefrom, string dateto)
        {
            try
            {
                string url = String.Format(@"{0}:{1}/api/v2/cdrs?domain={2}&limit={3}&from={4}&to={5}&offset=0"
                    , Global.sys_CallHost, Global.sys_CallPort, Global.sys_CallDomain, Global.sys_CallLimit, datefrom + " 00:00:00", dateto + " 00:00:00"
                );
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Credentials = new NetworkCredential(Global.sys_CallUserName, Global.sys_CallPassword);
                httpWebRequest.Method = "GET";
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
