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

namespace MLunarCoffee.Pages.Labo.Setting.Properties
{
    public class SettingPropertiesDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            _CurrentID = Request.Query["CurrentID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Labo_ProperSetting_LoadDetail]", CommandType.StoredProcedure
                        ,"@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string CurrentID, string Data)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(Data);
                var user = Session.GetUserSession(HttpContext);

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Labo_ProperSetting_Insert]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, DataMain.Rows[0]["Name"]
                            , "@Create_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Labo_ProperSetting_Update]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, DataMain.Rows[0]["Name"]
                            , "@Create_By", SqlDbType.Int, user.sys_userid
                            , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        );
                    }
                }
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
