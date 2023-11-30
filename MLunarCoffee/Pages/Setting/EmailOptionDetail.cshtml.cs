using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class EmailOptionDetailModel : PageModel
    {
        public string _OptionCurrentID { get; set; }
        public string _Optionsystem { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string system = Request.Query["system"];
            _OptionCurrentID = (curr != null) ? curr.ToString() : "0";
            _Optionsystem = (system != null) ? system.ToString() : "0";
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Email_Option_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Email_Option_Manualdelete]", CommandType.StoredProcedure,
                           "@CurrentID", SqlDbType.Int, id,
                          "@Modified_By", SqlDbType.Int, user.sys_userid);
                }
                return Content(dt.Rows[0]["RESULT"].ToString());
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                EmailOption DataMain = JsonConvert.DeserializeObject<EmailOption>(data);
                var user = Session.GetUserSession(HttpContext);
                string checkContentSMSVi = Comon.Comon.CheckContentSMS(DataMain.Value.Replace("'", "").Trim());
                string checkContentSMSEng = Comon.Comon.CheckContentSMS(DataMain.ValueEng.Replace("'", "").Trim());
                if (checkContentSMSVi != "1")
                {
                    return Content(checkContentSMSVi);
                };
                if (checkContentSMSEng != "1")
                {
                    return Content(checkContentSMSEng);
                };
                int system = Convert.ToInt32(DataMain.System);
                if (system == 1)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Email_Option_Update]", CommandType.StoredProcedure,
                            "@Value", SqlDbType.NVarChar, DataMain.Value.Trim(),
                            "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Trim(),
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@MailSubject", SqlDbType.NVarChar, DataMain.MailSubject.Replace("'", "").Trim(),
                            "@MailSubjectEn", SqlDbType.NVarChar, DataMain.MailSubjectEn.Replace("'", "").Trim(),
                            "@LinkVn", SqlDbType.NVarChar, DataMain.LinkVn.Replace("'", "").Trim(),
                            "@NameLinkVn", SqlDbType.NVarChar, DataMain.NameLinkVn.Replace("'", "").Trim(),
                            "@LinkEn", SqlDbType.NVarChar, DataMain.LinkEn.Replace("'", "").Trim(),
                            "@NameLinkEn", SqlDbType.NVarChar, DataMain.NameLinkEn.Replace("'", "").Trim(),
                            "@CurrentID ", SqlDbType.Int, CurrentID
                        );
                        return Content("1");
                    }
                }
                else
                {
                    if (CurrentID == "0")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Email_Option_Manualinsert]", CommandType.StoredProcedure,
                               "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                               "@Value", SqlDbType.NVarChar, DataMain.Value.Trim(),
                               "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Trim(),
                               "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                               "@MailSubject", SqlDbType.NVarChar, DataMain.MailSubject.Replace("'", "").Trim(),
                               "@MailSubjectEn", SqlDbType.NVarChar, DataMain.MailSubjectEn.Replace("'", "").Trim(),
                               "@LinkVn", SqlDbType.NVarChar, DataMain.LinkVn.Replace("'", "").Trim(),
                               "@NameLinkVn", SqlDbType.NVarChar, DataMain.NameLinkVn.Replace("'", "").Trim(),
                               "@LinkEn", SqlDbType.NVarChar, DataMain.LinkEn.Replace("'", "").Trim(),
                               "@NameLinkEn", SqlDbType.NVarChar, DataMain.NameLinkEn.Replace("'", "").Trim(),
                               "@Created_By", SqlDbType.Int, user.sys_userid);
                            return Content(dt.Rows[0]["RESULT"].ToString());
                        }

                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Email_Option_Manualupdate]", CommandType.StoredProcedure,
                               "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                               "@Value", SqlDbType.NVarChar, DataMain.Value.Trim(),
                               "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Trim(),
                               "@MailSubject", SqlDbType.NVarChar, DataMain.MailSubject.Replace("'", "").Trim(),
                               "@MailSubjectEn", SqlDbType.NVarChar, DataMain.MailSubjectEn.Replace("'", "").Trim(),
                               "@LinkVn", SqlDbType.NVarChar, DataMain.LinkVn.Replace("'", "").Trim(),
                               "@NameLinkVn", SqlDbType.NVarChar, DataMain.NameLinkVn.Replace("'", "").Trim(),
                               "@LinkEn", SqlDbType.NVarChar, DataMain.LinkEn.Replace("'", "").Trim(),
                               "@NameLinkEn", SqlDbType.NVarChar, DataMain.NameLinkEn.Replace("'", "").Trim(),
                               "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                               "@Modified_By", SqlDbType.Int, user.sys_userid,
                               "@CurrentID ", SqlDbType.Int, CurrentID);
                            return Content(dt.Rows[0]["RESULT"].ToString());
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
    public class EmailOption
    {
        public string System { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueEng { get; set; }
        public string Note { get; set; }
        public string MailSubject { get; set; }
        public string MailSubjectEn { get; set; }
        public string LinkVn { get; set; }
        public string NameLinkVn { get; set; }
        public string LinkEn { get; set; }
        public string NameLinkEn { get; set; }
    }
 
   
}