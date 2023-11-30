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
    public class ConfigOptionDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Config_Option_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
            catch
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                ConfigOption DataMain = JsonConvert.DeserializeObject<ConfigOption>(data);
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Option_Update]", CommandType.StoredProcedure,
                    "@Modified_By", SqlDbType.Int, user.sys_userid,
                     "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                     "@CurrentID", SqlDbType.Int, CurrentID,
                     "@Value ", SqlDbType.NVarChar, DataMain.Value
                );
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class ConfigOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
    }
}
