using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.AppCustomer.TimePeriod
{
    public class TimePeriodDetailModel : PageModel
    {
        public async Task<IActionResult> OnPostExecute(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("ID", typeof(int));
                dtMain.Columns.Add("HourStart", typeof(String));
                dtMain.Columns.Add("HourEnd", typeof(String));
                dtMain.Columns.Add("State", typeof(int));
                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["ID"] = Convert.ToInt32(DataMain.Rows[i]["ID"]);
                    dr["HourStart"] = DataMain.Rows[i]["HourStart"];
                    dr["HourEnd"] = DataMain.Rows[i]["HourEnd"];
                    dr["State"] = Convert.ToInt32(DataMain.Rows[i]["State"]);
                    dtMain.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_AC_TimePeriod_Update]", CommandType.StoredProcedure,
                        "@table", SqlDbType.Structured, (dtMain.Rows.Count > 0) ? dtMain : null);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
