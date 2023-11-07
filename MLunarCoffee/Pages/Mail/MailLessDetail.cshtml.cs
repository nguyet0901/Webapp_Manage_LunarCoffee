using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Mail
{
    public class MailLessDetailModel : PageModel
    {
        public string _MailCurentType { get; set; }
        public string DataMailComboType { get; set; }
        public void OnGet()
        {
            string typeid = Request.Query["Type"];
            _MailCurentType = (typeid == null) ? "1" : typeid.ToString();
            
        }

         public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[YYY_sp_Mail_Option_Combo_Load]", CommandType.StoredProcedure);
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

        
         public async Task<IActionResult> OnPostCheckContent(string content)
        {
            try
            {
                if (content == "" || content.Length < 5) return Content("Tin Nhắn Rổng Hoặc Số Ký Tự Quá Ít (Lớn Hơn 5)");
                return Content( Comon.Comon.CheckContentSMS(content));
            }
            catch (Exception ex)
            {
               return Content("-1");
            }
        }

        
         public async Task<IActionResult> OnPostExcuteData(string data, string title, string typeCare)
        {
            try
            {

                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("Index", typeof(Int32));
                dtMain.Columns.Add("Customer_ID", typeof(Int32));
                dtMain.Columns.Add("Email", typeof(String));
                dtMain.Columns.Add("AppID", typeof(Int32));
                dtMain.Columns.Add("Type", typeof(Int32));
                dtMain.Columns.Add("Title", typeof(String));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Index"] = i + 1;
                    dr["Customer_ID"] = Convert.ToInt32(DataMain.Rows[i]["CustID"]);
                    dr["Email"] = DataMain.Rows[i]["Email"];
                    dr["AppID"] = ((DataMain.Columns.Contains("AppID")) ? (Convert.ToInt32(DataMain.Rows[i]["AppID"])) : (0));
                    dr["Type"] = Convert.ToInt32(typeCare);
                    dr["Title"] = title;
                    dtMain.Rows.Add(dr);
                }



                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    var dt =await connFunc.ExecuteDataTable("YYY_sp_CustomerCare_Email_Insert", CommandType.StoredProcedure,
                          "@Created_By", SqlDbType.Int, user.sys_userid,
                          "@BranchID", SqlDbType.Int, user.sys_branchID,
                          "@table_data", SqlDbType.Structured, (dtMain.Rows.Count > 0) ? dtMain : null
                          );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
               return Content("-1");
            }
        }
        
         public async Task<IActionResult> OnPostSendData(string bodyEmail, string mailSubject, string EmailTo)
        {
            try
            {
                return Content( Comon.SendMail.SendEmail(EmailTo, mailSubject, bodyEmail).ToString());
            }
            catch (Exception ex)
            {
               return Content("-1");
            }
        }
    }
}
