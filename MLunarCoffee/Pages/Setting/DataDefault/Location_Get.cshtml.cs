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
namespace MLunarCoffee.Pages.Setting.DataDefault
{
    public class Location_GetModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Setting_Location_LoadList]" ,CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data,string type)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMainMain = new DataTable();
                dtMainMain.Columns.Add("Code" ,typeof(String));
                dtMainMain.Columns.Add("Name" ,typeof(String));
                dtMainMain.Columns.Add("Value" ,typeof(String)); 

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMainMain.NewRow();
                    dr["Code"] = DataMain.Rows[i]["Code"];
                    dr["Name"] = DataMain.Rows[i]["Name"];
                    dr["Value"] = DataMain.Rows[i]["Value"];
                    dtMainMain.Rows.Add(dr);
                }
               

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Location_Update]" ,CommandType.StoredProcedure ,
                        "@table_dataData" ,SqlDbType.Structured ,(dtMainMain.Rows.Count > 0) ? dtMainMain : null
                        ,"@Type", SqlDbType.Int, type
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
