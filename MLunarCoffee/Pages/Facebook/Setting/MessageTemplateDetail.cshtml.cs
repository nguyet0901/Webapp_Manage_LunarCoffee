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
    public class MessageTemplateDetailModel : PageModel
    {
        public string _MessageTemplateID { get; set; } 
        public void OnGet()
        { 
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _MessageTemplateID = curr.ToString(); 
            }
            else
            {
                _MessageTemplateID = "0"; 
            }
        }

         public async Task<IActionResult> OnPostLoadData(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_pageFacebook_Message_Template_LoadDetail]", CommandType.StoredProcedure,
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        
         public async Task<IActionResult> OnPostExcuteData(string Title, string Message, string CurrentID)
        {
            try
            {
                Title = (Title != null ? Title : "");
                Message = (Message != null ? Message : "");
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_pageFacebook_Message_Template_Insert]", CommandType.StoredProcedure,
                              "@Title", SqlDbType.Int, Title,
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Description", SqlDbType.NVarChar, Message
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Title Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_pageFacebook_Message_Template_Update]", CommandType.StoredProcedure,
                            "@Title", SqlDbType.NVarChar, Title,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Description", SqlDbType.NVarChar, Message

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Title Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
}
