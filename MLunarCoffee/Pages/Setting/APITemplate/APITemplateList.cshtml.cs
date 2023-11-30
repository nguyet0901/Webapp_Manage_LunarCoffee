using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Setting.APITemplate
{
    public class APITemplateListModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostInit(string type)
        {
            try
            {
                DataTable dt = new DataTable();
                switch (type)
                {
                    case "misa":

                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadData]" ,CommandType.StoredProcedure
                                ,"@Type" ,SqlDbType.Int ,0
                                ,"@CurrentID" ,SqlDbType.Int ,0
                                ,"@isDel" ,SqlDbType.Int ,0
                                ,"@APIUserName" ,SqlDbType.NVarChar ,""
                                );
                        }
                        break;
                }
                List<Dictionary<string ,object>> rows = new List<Dictionary<string ,object>>();
                var row = new Dictionary<string ,object>();
                foreach (DataColumn col in dt.Columns)
                    row.Add(col.ColumnName ,"");
                rows.Add(row);

                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    IncludeFields = true
                };
                return Content(JsonSerializer.Serialize(rows ,jsonSerializerOptions));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string type ,string action)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting]" ,CommandType.StoredProcedure
                        ,"@AccountBranch" ,SqlDbType.NVarChar ,type
                        ,"@ActionType" ,SqlDbType.NVarChar ,action);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
