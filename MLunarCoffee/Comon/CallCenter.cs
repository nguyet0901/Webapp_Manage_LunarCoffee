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
    public static class CallCenter
    {
        public static string GetOutboundLog(string phone) {
            try
            {
                if (Global.sys_CallCenterType == 2) return CallCenter_CMC.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 3) return CallCenter_CMC_V2.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 5) return CallCenter_TACA.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 7) return CallCenter_CloudFone.GetLogByPhone(phone);
                if (Global.sys_CallCenterType == 8) return CallCenter_VOIP24h.GetLogByPhone(phone);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GetOutboundLog_All(string date, string page, string extension)
        {
            try
            {
                if (Global.sys_CallCenterType == 2) return CallCenter_CMC.GetLogByDate(date);
                if (Global.sys_CallCenterType == 3) return CallCenter_CMC_V2.GetLogByDate(date, page, extension);
                if (Global.sys_CallCenterType == 5) return CallCenter_TACA.GetLogByDate(date);
                if (Global.sys_CallCenterType == 7) return CallCenter_CloudFone.GetLogByDate(date);
                if (Global.sys_CallCenterType == 8) return CallCenter_VOIP24h.GetLogByDate(date);
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
                if (Global.sys_CallCenterType == 2) return CallCenter_CMC.GetLogByPhone(extension);
                if (Global.sys_CallCenterType == 3) return CallCenter_CMC_V2.GetLogByExtension(extension);
                if (Global.sys_CallCenterType == 5) return CallCenter_TACA.GetLogByPhone(extension);
                if (Global.sys_CallCenterType == 7) return CallCenter_CloudFone.GetLogByExtension(extension);
                if (Global.sys_CallCenterType == 8) return CallCenter_VOIP24h.GetLogByPhone(extension);
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// Get CurrentStatus
        /// </summary>
        /// <returns></returns>
        ///

        private static string GetCurrentStatus()
        {
            try
            {
                string url = String.Format(@"{0}:{1}/api/v1/extensions?domain={2}"
              , Global.sys_CallHost, Global.sys_CallPort, Global.sys_CallDomain);

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

        public static string CurrentStatus()
        {
            try
            {
                string resulf = GetCurrentStatus();
                //DataTable dtM = new DataTable();
                //dtM.Columns.Add("STT", typeof(string));
                //dtM.Columns.Add("from", typeof(string));
                //dtM.Columns.Add("to", typeof(string));
                //dtM.Columns.Add("direction", typeof(string));
                //dtM.Columns.Add("duration", typeof(string));
                //dtM.Columns.Add("state", typeof(string));
                //dtM.Columns.Add("recordUrl", typeof(string));
                //dtM.Columns.Add("start", typeof(string));

                //DateTime dateTo = Comon.GetDateTimeNow().AddDays(1); // H thuc chat  cua server
                //DateTime dateFrom = Comon.GetDateTimeNow().AddDays(-3);

                //if (Global.sys_CallCenterType == 1)
                //{

                //    // Lam sau

                //}
                //if (Global.sys_CallCenterType == 2)
                //{
                //    string resulf = GetDataAll_Type2();
                //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(resulf, (typeof(DataTable)));
                //    var Rows = (from row in dt.AsEnumerable()
                //                orderby row["start_epoch"] descending
                //                select row);
                //    dt = Rows.AsDataView().ToTable();
                //    if (dt != null && dt.Rows.Count != 0)
                //    {
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            DateTime date_Start = Comon.UnixTimeStampToDateTime_ToDT(Convert.ToDouble(dt.Rows[i]["start_epoch"]));
                //            string from = dt.Rows[i]["caller_id_number"].ToString();
                //            string to = dt.Rows[i]["destination_number"].ToString();
                //            if (date_Start >= dateFrom && date_Start <= dateTo && (from == extension || to == extension))
                //            {
                //                DataRow drow = dtM.NewRow();
                //                drow["from"] = from;
                //                drow["to"] = to;
                //                drow["direction"] = dt.Rows[i]["direction"].ToString();
                //                drow["duration"] = dt.Rows[i]["billsec"].ToString();
                //                drow["state"] = dt.Rows[i]["call_status"].ToString();
                //                string filename = dt.Rows[i]["record_path"].ToString();
                //                drow["recordUrl"] = (filename != "null" && filename != "") ? String.Format(@"{0}{1}", Global.sys_UrlRecord, filename) : "null";
                //                // drow["recordUrl"] = Base64Encode(dtResulf.Rows[i]["record_path"].ToString());
                //                drow["start"] = Comon.UnixTimeStampToDateTime(Convert.ToDouble(dt.Rows[i]["start_epoch"]));
                //                dtM.Rows.Add(drow);
                //            }
                //        }
                //    }
                //    return JsonConvert.SerializeObject(dtM);
                //}
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
