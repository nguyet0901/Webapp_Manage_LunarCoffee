using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Fanpage.Comon.Session;

namespace Fanpage.Pages.Facebook.Setting
{
    public class FacebookTagDetailModel : PageModel
    {
        public string _FacebookTagID { get; set; } 
        public void OnGet()
        { 
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _FacebookTagID = curr.ToString(); 
            }
            else
            {
                _FacebookTagID = "0"; 
            }
        }

         public async Task<IActionResult> OnPostLoadDetail(int id)
        {
            try {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_PageFacebook_Tag_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            } 
        }

 
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                PageFacebookColor DataMain = JsonConvert.DeserializeObject<PageFacebookColor>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_PageFacebook_Tag_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Color", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tag Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_PageFacebook_Tag_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Color", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim()

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tag Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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

    }
    public class PageFacebookColor
    {
        public string Name { get; set; }
        public string ColorCode { get; set; }
    }

}
