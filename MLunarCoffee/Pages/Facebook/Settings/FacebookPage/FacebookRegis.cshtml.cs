using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace MLunarCoffee.Pages.Facebook.Settings.FacebookPage
{
    public class FacebookRegis : PageModel
    {
        public string _fbpageID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _fbpageID = curr.ToString();
            else
            {
                _fbpageID = "0";
            }


        }

        public async Task<IActionResult> OnPostRemoveSub(string pageid)
        {
            try
            {
                string urltoken = String.Format(@"/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials"
                    ,Comon.Global.sys_face_appid ,Comon.Global.sys_face_secretkey);
                using (HttpClient rg = new HttpClient() { BaseAddress = new Uri("https://graph.facebook.com") })
                {
                    var result = await rg.GetAsync(urltoken);
                    string responseBody = await result.Content.ReadAsStringAsync();
                    string token = JToken.Parse(responseBody)["access_token"].ToString();
                    string url = String.Format(@"/{0}/{1}/subscribed_apps?access_token={2}" ,Comon.Global.sys_face_version ,pageid ,token);

                    using (HttpClient re = new HttpClient() { BaseAddress = new Uri("https://graph.facebook.com") })
                    {
                        var res = await re.DeleteAsync(url);
                        string resbody = await res.Content.ReadAsStringAsync();
                        return Content(resbody);
                    }

                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostUpdateRegis(string pageid ,string status)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_FB_Fanpage_Regis]" ,CommandType.StoredProcedure ,
                      "@pageid" ,SqlDbType.NVarChar ,pageid ,
                       "@status" ,SqlDbType.Int ,status);
                }
                return Content("1");
            }
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostUpdateDown(string pageid ,string status)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_FB_Fanpage_Down]" ,CommandType.StoredProcedure ,
                      "@pageid" ,SqlDbType.NVarChar ,pageid ,
                       "@status" ,SqlDbType.Int ,status);
                }
                return Content("1");
            }
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostCheckDownload(string pageid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_FB_GetDownload]" ,CommandType.StoredProcedure ,
                      "@pageid" ,SqlDbType.NVarChar ,pageid);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostGetLongkey(string token)
        {
            try
            {

                string url = String.Format(@"/{0}/oauth/access_token?grant_type=fb_exchange_token&client_id={1}&client_secret={2}&fb_exchange_token={3}"
                            ,Comon.Global.sys_face_version ,Comon.Global.sys_face_appid ,Comon.Global.sys_face_secretkey ,token);

                using (HttpClient re = new HttpClient() { BaseAddress = new Uri("https://graph.facebook.com") })
                {
                    var res = await re.GetAsync(url);
                    string resbody = await res.Content.ReadAsStringAsync();
                    return Content(resbody);
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

}
