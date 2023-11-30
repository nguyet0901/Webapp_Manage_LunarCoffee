
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class EmailSettingDetailModel : PageModel
    {
        public string _SerCurrentID { get; set; }
        public void OnGet()
        {
            _SerCurrentID = Request.Query["CurrentID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_Email_Setting_Load_Detail]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID));
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data, string CurrentID)
        {
            try
            {
                Email_Setting DataMain = JsonConvert.DeserializeObject<Email_Setting>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                var PassEncty = Encrypt.EncryptString(DataMain.Password, Settings.Secret);
                if (PassEncty == "")
                {
                    return Content("-1");
                }

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_Email_Setting_Insert]", CommandType.StoredProcedure
                            , "@DisplayName",SqlDbType.NVarChar, DataMain.DisplayName
                            , "@Mail",SqlDbType.NVarChar, DataMain.Mail
                            , "@Password",SqlDbType.NVarChar, PassEncty
                            , "@Host",SqlDbType.NVarChar, DataMain.Host
                            , "@Port",SqlDbType.Int, Convert.ToInt32(DataMain.Port)
                            , "@Is2ndAuthen", SqlDbType.Int, Convert.ToInt32(DataMain.Is2ndAuthen)
                            , "@CreateBy", SqlDbType.Int,user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_Email_Setting_Update]", CommandType.StoredProcedure
                            , "@DisplayName", SqlDbType.NVarChar, DataMain.DisplayName
                            , "@Mail", SqlDbType.NVarChar, DataMain.Mail
                            , "@Password", SqlDbType.NVarChar, PassEncty
                            , "@Host", SqlDbType.NVarChar, DataMain.Host
                            , "@Port", SqlDbType.Int, Convert.ToInt32(DataMain.Port)
                            , "@Is2ndAuthen", SqlDbType.Int, Convert.ToInt32(DataMain.Is2ndAuthen)
                            , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                            , "@Modified_By",SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public IActionResult OnPostDecryptPassword(string password)
        {
            try
            {
                string result = Encrypt.DecryptString(password, Settings.Secret);
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public class Email_Setting
        {
            public string DisplayName { get; set; }
            public string Mail { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public string Port { get; set; }
            public string Is2ndAuthen { get; set; }
        }
    }
}
