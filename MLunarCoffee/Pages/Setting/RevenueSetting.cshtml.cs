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
    public class RevenueSettingModel : PageModel
    {

        public void OnGet()
        {


        }


        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_Revenue]", CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable DataSettings = new DataTable();
                DataSettings.Columns.Add("Name", typeof(string));
                DataSettings.Columns.Add("Value", typeof(Int32));
                DataSettings.Columns.Add("IsPayroll", typeof(Int32));
                DataSettings.Columns.Add("IsExcludeCost", typeof(Int32));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = DataSettings.NewRow();
                    dr["Name"] = DataMain.Rows[i]["Name"];
                    dr["Value"] = Convert.ToInt32(DataMain.Rows[i]["Value"]);
                    dr["IsPayroll"] = Convert.ToInt32(DataMain.Rows[i]["IsPayroll"]);
                    dr["IsExcludeCost"] = Convert.ToInt32(DataMain.Rows[i]["IsExcludeCost"]);
                    DataSettings.Rows.Add(dr);
                }

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_RevenueSetting_Update", CommandType.StoredProcedure,
                         "@Created_By", SqlDbType.Int, user.sys_userid,
                         "@Table", SqlDbType.Structured, DataSettings != null ? DataSettings : null
                    );
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
