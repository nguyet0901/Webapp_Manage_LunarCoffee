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

namespace MLunarCoffee.Pages.Setting.Treatment.TreatmentPercent
{
    public class TreatmentPercentDetailModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_SettingTreatmentPercent_Loadlist", CommandType.StoredProcedure);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);

                DataTable dtDetail = new DataTable();
                dtDetail.Columns.Add("idUpdate", typeof(Int32));
                dtDetail.Columns.Add("Name", typeof(Int32));
                dtDetail.Columns.Add("Value", typeof(Int32));
                dtDetail.Columns.Add("isNew", typeof(Int32));
                dtDetail.Columns.Add("isDelete", typeof(Int32));
                var user = Session.GetUserSession(HttpContext);

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    dr["idUpdate"] = Convert.ToInt32(DataMain.Rows[i]["idUpdate"]);
                    dr["Name"] = Convert.ToInt32(DataMain.Rows[i]["Name"]);
                    dr["Value"] = Convert.ToInt32(DataMain.Rows[i]["Value"]);
                    dr["isNew"] = Convert.ToInt32(DataMain.Rows[i]["isNew"]);
                    dr["isDelete"] = Convert.ToInt32(DataMain.Rows[i]["isDelete"]);
                    dtDetail.Rows.Add(dr);
                }

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_SettingTreatmentPercent_Edit", CommandType.StoredProcedure
                        , "@Table_Data", SqlDbType.Structured, dtDetail.Rows.Count > 0 ? dtDetail : null
                        , "@Modified_By", SqlDbType.Int, user.sys_userid);
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
