using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Setting
{
    public class SettingCustomerDetailModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }
        public async Task<IActionResult> OnPostLoadData( )
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_SettingCustomerDetail_Loadlist", CommandType.StoredProcedure
                        );
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string CurrentID, string Data)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_SettingCustomerDetail_Update", CommandType.StoredProcedure
                        , "CurrentID",SqlDbType.Int, Convert.ToInt32(CurrentID)
                        , "Data",SqlDbType.NVarChar, Data
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
}
