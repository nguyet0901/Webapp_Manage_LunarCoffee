using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fanpage.Comon.Session;

namespace Fanpage.Pages.Facebook.Setting
{
    public class FanpageListModel : PageModel
    {
        public string _defaultAvatar { get; set; }
        public string _URL_PortClient { get; set; }
        public string _Face_App_ID { get; set; }
        protected static string _URL_Write_File_PageFB = "ftp://103.48.191.137:7071/IDPageKeyCode/Keycode.xml";
        public void OnGet()
        {
            _defaultAvatar ="";
            _URL_PortClient = Comon.Global.sys_URL_PortClient;
            _Face_App_ID = Comon.Global.sys_Face_AppID;
        }
         public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Ticket_PageFacebook_Load]", CommandType.StoredProcedure,
                      "@UserID", SqlDbType.Int, user.sys_userid);
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

         public async Task<IActionResult> OnPostRegistrationPage(string KeyPage, string Url, int PageId, int Value, string NamePage, string userToken)
        {
            try
            {
                KeyPage = (KeyPage != null ? KeyPage : "");
                Url = (Url != null ? Url : "");
                PageId = (PageId != null ? PageId : 0);
                Value = (Value != null ? Value : 0);
                NamePage = (NamePage != null ? NamePage : "");
                userToken = (userToken != null ? userToken : "");
                string longTokenPage = "";
                int result = 0;
                if (KeyPage != "" && KeyPage != null)
                {
                    string userlongtoken = "";
                    userlongtoken = GetLongUser(userToken);
                    result += Comon.ComonFacebookXml.UpdateFileFTP_XML(KeyPage, Url, NamePage, _URL_Write_File_PageFB);
                    longTokenPage = UpdateLongKey(PageId, KeyPage, userlongtoken);
                    result += Subscribed_App(KeyPage, longTokenPage);
                }
                if (result > 1)
                {
                    result += ActiveItem(PageId, Value);
                    //CacheHelper.Set_Face_Key_List();
                    int isGetMess =await Check_Insert_Facebook_Mess(KeyPage);
                    if (isGetMess == 0) //Comon.Facebook.MessageConnect_GetList(KeyPage.ToString(), longTokenPage);
                    return Content("1");
                }
                return Content("");
            }
            catch (Exception e)
            {
                return Content("");
            }
        }

         public async Task<IActionResult> OnPostDeleteItem(int id, string KeyPage)
        {
            try
            {
                if (Comon.ComonFacebookXml.CheckingFileFTP_XML(KeyPage, _URL_Write_File_PageFB) == 1) return Content("0"); // code tồn tại, không thể xóa

                int result1 = Comon.ComonFacebookXml.DeleteFileFTP_XML(KeyPage, _URL_Write_File_PageFB);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataTable("[MLU_sp_Ticket_PageFacebook_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                //CacheHelper.Set_Face_Key_List();
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

         public async Task<IActionResult> OnPostUnregisterItem(int id, string KeyPage)
        {
            try
            {
                if (Comon.ComonFacebookXml.DeleteFileFTP_XML(KeyPage, _URL_Write_File_PageFB) == 1)
                {
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        connFunc.ExecuteDataTable("[MLU_sp_Ticket_PageFacebook_Unregister]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, id,
                            "@UserID", SqlDbType.Int, user.sys_userid
                        );
                    }
                    //CacheHelper.Set_Face_Key_List();
                    return Content("1");
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        private string UpdateLongKey(int PageId, string pageKey, string userLongToken)
        {
            try
            {
                string long_token = "";
                string url = @"https://graph.facebook.com/v5.0/{page-id}?fields=access_token&access_token={token}";
                url = url.Replace("{token}", userLongToken).Replace("{page-id}", pageKey);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    long_token = JToken.Parse(responseString)["access_token"].ToString();
                }
                DateTime now = DateTime.Now;
                now = now.AddSeconds(5184000);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataTable("[MLU_sp_Ticket_PageFacebook_Update_LongKey]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, PageId,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@LongKey", SqlDbType.NVarChar, long_token,
                        "@ExpireDate", SqlDbType.DateTime, now
                    );
                }

                //Comon.CacheHelper.Set_Face_Key(pageKey, long_token);
                return long_token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static string GetLongUser(string access_Token)
        {
            try
            {
                string long_token = "";
                string url = @"https://graph.facebook.com/v5.0/oauth/access_token?grant_type=fb_exchange_token&client_id={appid}&client_secret={secret}&fb_exchange_token={token}";
                url = url.Replace("{appid}", Comon.Global.sys_Face_AppID).Replace("{secret}", Comon.Global.sys_Face_Secret);
                url = url.Replace("{token}", access_Token);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    long_token = JToken.Parse(responseString)["access_token"].ToString();
                }
                return long_token;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        private static int ActiveItem(int PageId, int Value)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataTable("[MLU_sp_PageFacebook_Active]", CommandType.StoredProcedure,
                        "@pageid", SqlDbType.Int, PageId,
                        "@value", SqlDbType.Int, Value
                    );
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        private static int Subscribed_App(string pageKey, string access_token)
        {
            try
            {
                string result = "";
                string url = @"https://graph.facebook.com/v5.0/{page-id}/subscribed_apps";
                url = url.Replace("{page-id}", pageKey);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"subscribed_fields\":\"messages,messaging_postbacks,feed\"," +
                                  "\"access_token\":\"" + access_token + "\"}";

                    streamWriter.Write(json);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = JToken.Parse(responseString)["success"].ToString();
                    return (result == "True") ? 1 : 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private static async Task<int>  Check_Insert_Facebook_Mess(string page)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Checking_Data_Exist]", CommandType.StoredProcedure,
                        "@page", SqlDbType.NVarChar, page
                    );
                }
                if (dt != null && dt.Rows.Count == 1 && dt.Rows[0][0].ToString() != "0")
                    return 1;
                else return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
