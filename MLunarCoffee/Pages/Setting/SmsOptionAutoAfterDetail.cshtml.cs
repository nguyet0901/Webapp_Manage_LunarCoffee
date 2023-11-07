using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class SmsOptionAutoAfterDetailModel : PageModel
    {
        public string _OptionCurrentID { get; set; }
        public string _OptionType { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string type = Request.Query["Type"];
            _OptionCurrentID = (curr != null) ? curr.ToString() : "0";
            _OptionType = (type != null) ? type.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_SMS_Option_LoadDetail]", CommandType.StoredProcedure,
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

        public IActionResult OnPostEncryptPassword(string password)
        {
            try
            {
                string result = Encrypt.EncryptString(password, Settings.Secret);
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string IsAuto, string CurrentID, string Type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_SMS_OptionAutoAfterAction_Update]" , CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@AutoSend", SqlDbType.Int, IsAuto,
                        "@Type", SqlDbType.NVarChar,Type,
                        "@CurrentID " , SqlDbType.Int, CurrentID);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
