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

namespace MLunarCoffee.Pages.Setting.AppCustomer.Setting
{
    public class AppCustomerSettingDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            _CurrentID = Request.Query["CurrentID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_ColorCollect_Load]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[YYY_sp_IniSetting_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, ID
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data, int CurrentID)
        {
            try
            {
                Setting_Detail DataMain = JsonConvert.DeserializeObject<Setting_Detail>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_IniSetting_Update]", CommandType.StoredProcedure
                        , "@Value", SqlDbType.NVarChar, DataMain.Value
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        , "@Modified_By",SqlDbType.Int, user.sys_userid
                        , "@Description", SqlDbType.NVarChar, DataMain.Description
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class Setting_Detail
        {
            public string Value { get; set; }
            public string Description { get; set; }
        }
    }
}
