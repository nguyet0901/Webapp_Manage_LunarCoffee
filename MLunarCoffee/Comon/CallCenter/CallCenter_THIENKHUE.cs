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
    public static class CallCenter_THIENKHUE
    {

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


                if (Global.sys_CallCenterType == 4)
                {
                    string response = ExtensionByDate(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));


                    var results = JToken.Parse(response);

                    if (results.ToArray().Length <= 0)
                    {
                        return "0";
                    }

                    for (int i = 0; i < results.ToArray().Length; i++)
                    {
                        var result = results[i];
                        string cdr = result["cdr"].ToString();

                        if (result["main_cdr"] != null)
                        {
                            var main_cdr = result["main_cdr"];
                            var recordUrlMain = "";
                            recordUrlMain = main_cdr["recordfiles"].ToString();
                            if (recordUrlMain != "")
                            {
                                recordUrlMain = Global.sys_UrlRecord.ToString() + recordUrlMain.Trim('@');
                            }
                            else
                            {
                                recordUrlMain = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = main_cdr["src"].ToString();
                            drow["to"] = main_cdr["dst"].ToString();
                            drow["direction"] = main_cdr["userfield"].ToString();
                            drow["duration"] = main_cdr["billsec"].ToString();
                            drow["state"] = main_cdr["disposition"].ToString();
                            drow["recordUrl"] = recordUrlMain;
                            drow["start"] = main_cdr["start"];
                            dtM.Rows.Add(drow);

                            var n = 1;
                            while (result["sub_cdr_" + n.ToString()] != null)
                            {
                                if (result["sub_cdr_" + n.ToString()] != null)
                                {
                                    var sub_cdr = result["sub_cdr_" + n.ToString()];

                                    var recordUrl = "";
                                    recordUrl = sub_cdr["recordfiles"].ToString();
                                    if (recordUrl != "")
                                    {
                                        recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                                    }
                                    else
                                    {
                                        recordUrl = "null";
                                    }
                                    DataRow drowSub = dtM.NewRow();
                                    drowSub["from"] = sub_cdr["src"].ToString();
                                    drowSub["to"] = sub_cdr["dst"].ToString();
                                    drowSub["direction"] = sub_cdr["userfield"].ToString();
                                    drowSub["duration"] = sub_cdr["billsec"].ToString();
                                    drowSub["state"] = sub_cdr["disposition"].ToString();
                                    drowSub["recordUrl"] = recordUrl;
                                    drowSub["start"] = sub_cdr["start"];
                                    dtM.Rows.Add(drowSub);

                                }
                                n++;
                            }

                        }
                        else
                        {
                            var recordUrl = "";
                            recordUrl = result["recordfiles"].ToString();
                            if (recordUrl != "")
                            {
                                recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                            }
                            else
                            {
                                recordUrl = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["src"].ToString();
                            drow["to"] = result["dst"].ToString();
                            drow["direction"] = result["userfield"].ToString();
                            drow["duration"] = result["billsec"].ToString();
                            drow["state"] = result["disposition"].ToString();
                            drow["recordUrl"] = recordUrl;
                            drow["start"] = result["start"];
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

        private static string ExtensionByDate(string datefrom, string dateto)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                string url = String.Format(@"{0}/cdrapi?format=JSON&startTime={1}&endTime={2}", Global.sys_APIbaseurl, datefrom, dateto);
                string username = Global.sys_CallUserName.ToString();
                string password = Global.sys_CallPassword.ToString();
                string domain = Global.sys_APIbaseurl;
                string dir = String.Format(@"/cdrapi?format=JSON&startTime={0}&endTime={1}", datefrom, dateto);

                var resultText = DigestAuthFixer.GrabResponse(dir, domain, username, password);

                var obj = JObject.Parse(resultText);
                var resulf = obj["cdr_root"];
                return resulf.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }
        }

       

        #region // Get Data By Extension

        public static string GetLogByExtension(string extension)
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

                DateTime dateTo = Comon.GetDateTimeNow().AddDays(1); // H thuc chat  cua server
                DateTime dateFrom = Comon.GetDateTimeNow().AddDays(-3);

                if (Global.sys_CallCenterType == 4)
                {

                    string responseOutbound = ByExtension(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), "caller", extension);
                    string responseInbound = ByExtension(dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), "callee", extension);

                    var outbound = JToken.Parse(responseOutbound);
                    var inbound = JToken.Parse(responseInbound);
                    var results = outbound.Concat(inbound).ToArray();

                    if (results.ToArray().Length <= 0)
                    {
                        return "0";
                    }
                    for (int i = 0; i < results.ToArray().Length; i++)
                    {
                        var result = results[i];
                        string cdr = result["cdr"].ToString();

                        if (result["main_cdr"] != null)
                        {
                            var main_cdr = result["main_cdr"];
                            var recordUrlMain = "";
                            recordUrlMain = main_cdr["recordfiles"].ToString();
                            if (recordUrlMain != "")
                            {
                                recordUrlMain = Global.sys_UrlRecord.ToString() + recordUrlMain.Trim('@');
                            }
                            else
                            {
                                recordUrlMain = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = main_cdr["src"].ToString();
                            drow["to"] = main_cdr["dst"].ToString();
                            drow["direction"] = main_cdr["userfield"].ToString();
                            drow["duration"] = main_cdr["billsec"].ToString();
                            drow["state"] = main_cdr["disposition"].ToString();
                            drow["recordUrl"] = recordUrlMain;
                            drow["start"] = main_cdr["start"];
                            dtM.Rows.Add(drow);

                            var n = 1;
                            while (result["sub_cdr_" + n.ToString()] != null)
                            {
                                if (result["sub_cdr_" + n] != null)
                                {
                                    var sub_cdr = result["sub_cdr_" + n.ToString()];

                                    var recordUrl = "";
                                    recordUrl = sub_cdr["recordfiles"].ToString();
                                    if (recordUrl != "")
                                    {
                                        recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                                    }
                                    else
                                    {
                                        recordUrl = "null";
                                    }
                                    DataRow drowSub = dtM.NewRow();
                                    drowSub["from"] = sub_cdr["src"].ToString();
                                    drowSub["to"] = sub_cdr["dst"].ToString();
                                    drowSub["direction"] = sub_cdr["userfield"].ToString();
                                    drowSub["duration"] = sub_cdr["billsec"].ToString();
                                    drowSub["state"] = sub_cdr["disposition"].ToString();
                                    drowSub["recordUrl"] = recordUrl;
                                    drowSub["start"] = sub_cdr["start"];
                                    dtM.Rows.Add(drowSub);

                                }
                                n++;
                            }

                        }
                        else
                        {
                            var recordUrl = "";
                            recordUrl = result["recordfiles"].ToString();
                            if (recordUrl != "")
                            {
                                recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                            }
                            else
                            {
                                recordUrl = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["src"].ToString();
                            drow["to"] = result["dst"].ToString();
                            drow["direction"] = result["userfield"].ToString();
                            drow["duration"] = result["billsec"].ToString();
                            drow["state"] = result["disposition"].ToString();
                            drow["recordUrl"] = recordUrl;
                            drow["start"] = result["start"];
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
        private static string ByExtension(string datefrom, string dateto, string type, string extension)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                string username = Global.sys_CallUserName.ToString();
                string password = Global.sys_CallPassword.ToString();
                string domain = Global.sys_APIbaseurl;
                string dir = String.Format(@"/cdrapi?format=JSON&{0}={1}&startTime={2}&endTime={3}", type, extension, datefrom, dateto);

                var resultText = DigestAuthFixer.GrabResponse(dir, domain, username, password);

                var obj = JObject.Parse(resultText);
                var resulf = obj["cdr_root"];
                return resulf.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

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

                if (Global.sys_CallCenterType == 4)
                {
                    string responseOutbound = ByPhone("caller", phone);
                    string responseInbound = ByPhone("callee", phone);

                    var outbound = JToken.Parse(responseOutbound);
                    var inbound = JToken.Parse(responseInbound);
                    var results = outbound.Concat(inbound).ToArray();

                    if (results.ToArray().Length <= 0)
                    {
                        return "0";
                    }
                    for (int i = 0; i < results.ToArray().Length; i++)
                    {
                        var result = results[i];
                        string cdr = result["cdr"].ToString();

                        if (result["main_cdr"] != null)
                        {
                            var main_cdr = result["main_cdr"];
                            var recordUrlMain = "";
                            recordUrlMain = main_cdr["recordfiles"].ToString();
                            if (recordUrlMain != "")
                            {
                                recordUrlMain = Global.sys_UrlRecord.ToString() + recordUrlMain.Trim('@');
                            }
                            else
                            {
                                recordUrlMain = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = main_cdr["src"].ToString();
                            drow["to"] = main_cdr["dst"].ToString();
                            drow["direction"] = main_cdr["userfield"].ToString();
                            drow["duration"] = main_cdr["billsec"].ToString();
                            drow["state"] = main_cdr["disposition"].ToString();
                            drow["recordUrl"] = recordUrlMain;
                            drow["start"] = main_cdr["start"];
                            dtM.Rows.Add(drow);

                            var n = 1;
                            while (result["sub_cdr_" + n.ToString()] != null)
                            {
                                if (result["sub_cdr_" + n] != null)
                                {
                                    var sub_cdr = result["sub_cdr_" + n.ToString()];

                                    var recordUrl = "";
                                    recordUrl = sub_cdr["recordfiles"].ToString();
                                    if (recordUrl != "")
                                    {
                                        recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                                    }
                                    else
                                    {
                                        recordUrl = "null";
                                    }
                                    DataRow drowSub = dtM.NewRow();
                                    drowSub["from"] = sub_cdr["src"].ToString();
                                    drowSub["to"] = sub_cdr["dst"].ToString();
                                    drowSub["direction"] = sub_cdr["userfield"].ToString();
                                    drowSub["duration"] = sub_cdr["billsec"].ToString();
                                    drowSub["state"] = sub_cdr["disposition"].ToString();
                                    drowSub["recordUrl"] = recordUrl;
                                    drowSub["start"] = sub_cdr["start"];
                                    dtM.Rows.Add(drowSub);

                                }
                                n++;
                            }

                        }
                        else
                        {
                            var recordUrl = "";
                            recordUrl = result["recordfiles"].ToString();
                            if (recordUrl != "")
                            {
                                recordUrl = Global.sys_UrlRecord.ToString() + recordUrl.Trim('@');
                            }
                            else
                            {
                                recordUrl = "null";
                            }

                            DataRow drow = dtM.NewRow();
                            drow["from"] = result["src"].ToString();
                            drow["to"] = result["dst"].ToString();
                            drow["direction"] = result["userfield"].ToString();
                            drow["duration"] = result["billsec"].ToString();
                            drow["state"] = result["disposition"].ToString();
                            drow["recordUrl"] = recordUrl;
                            drow["start"] = result["start"];
                            dtM.Rows.Add(drow);
                        }

                    }
                }
                return JsonConvert.SerializeObject(dtM);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static string ByPhone(string type, string phone)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                string username = Global.sys_CallUserName.ToString();
                string password = Global.sys_CallPassword.ToString();
                string domain = Global.sys_APIbaseurl;
                string dir = String.Format(@"/cdrapi?format=JSON&{0}={1}", type, phone);

                var resultText = DigestAuthFixer.GrabResponse(dir, domain, username, password);

                var obj = JObject.Parse(resultText);
                var resulf = obj["cdr_root"];
                return resulf.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
