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

namespace MLunarCoffee.Pages.Permission
{
    public class DetailBlockGroupModel : PageModel
    {
        public string _Group { get; set; }
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt =await connFunc.ExecuteDataTable("[YYY_sp_User_Group_LoadList]", CommandType.StoredProcedure);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }
        
         public async Task<IActionResult> OnPostLoadDataDetail(string GroupID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt =await connFunc.ExecuteDataTable("[YYY_sp_Per_GeExtentDetail]", CommandType.StoredProcedure
                      , "@GroupID", SqlDbType.Int, GroupID);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

        
         public async Task<IActionResult> OnPostExecuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("YYY_sp_PermissionDetailBlock_Execute", CommandType.StoredProcedure,
                          "@GroupID", SqlDbType.Int, CurrentID,
                          "@Data", SqlDbType.Structured, dataDetail.Rows.Count > 0 ? dataDetail : null
                      );

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error");
            }

        }
    }
}
