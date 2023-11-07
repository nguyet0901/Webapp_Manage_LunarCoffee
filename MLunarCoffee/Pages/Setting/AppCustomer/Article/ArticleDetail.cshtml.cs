using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.AppCustomer.Article
{
    public class ArticleDetailModel : PageModel
    {
        public string _ArticleDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _ArticleDetailID = curr != null ? curr.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_AC_Article_LoadDetail]", CommandType.StoredProcedure,
                    "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Article DataMain = JsonConvert.DeserializeObject<Article>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_Article_Insert]", CommandType.StoredProcedure,
                              "@Header", SqlDbType.NVarChar, DataMain.Header.Replace("'", "").Trim(),
                              "@Sub", SqlDbType.NVarChar, DataMain.Sub.Replace("'", "").Trim(),
                              "@FeatureImage", SqlDbType.NVarChar, DataMain.FeatureImage,
                              "@Image", SqlDbType.NVarChar, DataMain.Image,
                              "@Source", SqlDbType.NVarChar, DataMain.Source,
                              "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                              "@Type", SqlDbType.NVarChar, "article",
                              "@UserID", SqlDbType.Int, user.sys_userid
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() == "0")
                            {
                                return Content("Tiêu Đề Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_Article_Update]", CommandType.StoredProcedure,
                              "@Header", SqlDbType.NVarChar, DataMain.Header,
                              "@Sub", SqlDbType.NVarChar, DataMain.Sub,
                              "@FeatureImage", SqlDbType.NVarChar, DataMain.FeatureImage,
                              "@Image", SqlDbType.NVarChar, DataMain.Image,
                              "@Source", SqlDbType.NVarChar, DataMain.Source,
                              "@Content", SqlDbType.NVarChar, DataMain.Content,
                              "@UserID", SqlDbType.Int, user.sys_userid,
                              "@currentID", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() == "0")
                            {
                                return Content("Tiêu Đề Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class Article
        {
            public string Header { get; set; }
            public string Sub { get; set; }
            public string FeatureImage { get; set; }
            public string Image { get; set; }
            public string Source { get; set; }
            public string Content { get; set; }

        }
    }
}
