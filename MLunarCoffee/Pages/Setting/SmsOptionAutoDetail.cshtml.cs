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
    public class SmsOptionAutoDetailModel : PageModel
    {
        public string _OptionCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _OptionCurrentID = (curr != null) ? curr.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_Option_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                    dt.TableName = "Detail";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadPara("Schedule Type");
                    dt.TableName = "ScheduleType";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
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

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                SmsOptionAuto DataMain = JsonConvert.DeserializeObject<SmsOptionAuto>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_OptionAuto_Update]", CommandType.StoredProcedure,
                        "@AutoDate", SqlDbType.Int, DataMain.AutoDate != "-1" ? DataMain.AutoDate : "0",
                        "@Auto", SqlDbType.Int, DataMain.Auto,
                        "@AutoHour", SqlDbType.Int, DataMain.AutoHour,
                        "@AutoScheduleType" , SqlDbType.Int, DataMain.AutoScheduleType ,
                        "@LimitSMS", SqlDbType.Int, DataMain.LimitSMS,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@CurrentID ", SqlDbType.Int, CurrentID);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }

    public class SmsOptionAuto
    {
        public string AutoDate { get; set; }
        public string AutoHour { get; set; }
        public int Auto { get; set; }
        public int AutoScheduleType { get; set; }
        public string LimitSMS { get; set; }
    }
}
