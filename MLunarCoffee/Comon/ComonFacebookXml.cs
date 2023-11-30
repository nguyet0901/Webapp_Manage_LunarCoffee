using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MLunarCoffee.Comon
{
    public static class ComonFacebookXml
    {
        public static int UpdateFileFTP_XML(string KeyPage, string Url, string NamePage, string link)
        {
            try
            {
                NamePage = EscapeXml(Comon.ConvertToUnsign(NamePage));
                string linkpost = Url + "/api/noti/post";
                string key = "key_" + KeyPage;
                string item = String.Format("<{0} name=\"{1}\"><value>{2}</value></{0}>", key, NamePage, linkpost);
                //string root_ini = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root>";
                string root_ter = "</root>";
                string strResp = String.Empty;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(link);
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER, Global.sys_ALLFTP_PASSWORD);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                strResp = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if (strResp.Contains(key))
                {
                    // Có rồi không cho thay
                    return 1;
                }
                else
                {
                    strResp =  strResp.Replace(root_ter, "") + item + root_ter;
                    string result = UpdateFileFTP(strResp, link);
                    return (result == "1") ? 1 : 0;
                    // Chưa đăng ký
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int DeleteFileFTP_XML(string KeyPage, string link)
        {
            try
            {

                string keyFind = "</key_" + KeyPage + ">";
                string keyFind2 = "<key_" + KeyPage;
                string strResp = String.Empty;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(link);
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER, Global.sys_ALLFTP_PASSWORD);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                strResp = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if (strResp.Contains(keyFind))
                {
                    //strResp = strResp.Replace(root_ini, "").Replace(root_ter, "");
                    string[] _str = strResp.Split(new string[] { keyFind }, StringSplitOptions.None);
                    string s1 = _str[1];

                    string[] _str1 = _str[0].Split(new string[] { keyFind2 }, StringSplitOptions.None);
                    string s3 = _str1[0];
                    string s = s3  + s1;
                    string result = UpdateFileFTP(s, link);
                    return (result == "1") ? 1 : 0;
                }
                else
                {
                    // Không có key để xóa
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int CheckingFileFTP_XML(string KeyPage, string link)
        {
            try
            {

                string keyFind = "</key_" + KeyPage + ">";
                string strResp = String.Empty;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(link);
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER, Global.sys_ALLFTP_PASSWORD);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                strResp = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if (strResp.Contains(keyFind))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static string EscapeXml(this string s)
        {
            string toxml = s;
            if (!string.IsNullOrEmpty(toxml))
            {
                // replace literal values with entities
                toxml = toxml.Replace("&", "&amp;");
                toxml = toxml.Replace("'", "&apos;");
                toxml = toxml.Replace("\"", "&quot;");
                toxml = toxml.Replace(">", "&gt;");
                toxml = toxml.Replace("<", "&lt;");
            }
            return toxml;
        }
        public static string UpdateFileFTP(string Content, string Link)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Link);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER, Global.sys_ALLFTP_PASSWORD);

                using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(Content);
                    requestStream.Flush();
                    requestStream.Close();

                }
                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}