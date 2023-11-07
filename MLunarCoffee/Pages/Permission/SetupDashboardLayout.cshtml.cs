using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Permission
{
    public class SetupDashboardLayoutModel : PageModel
    {
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

       
         public async Task<IActionResult> OnPostLoadLayoutByGroup(int groupID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                ds = await connFunc.ExecuteDataSet("[YYY_sp_Dash_Manage_LoadByGroup]", CommandType.StoredProcedure
                    , "@GroupID", SqlDbType.Int, groupID);
            }
            if (ds != null)
            {
               return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
               return Content("[]");
            }
        }
       
         public async Task<IActionResult> OnPostExecuteData(string dataLayout, string CurrentID, string Type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataTable("YYY_sp_Dashboard_Layout_Group_Insert", CommandType.StoredProcedure,
                          "@GroupID", SqlDbType.Int, CurrentID,
                          "@tokenFunction", SqlDbType.NVarChar, dataLayout,
                          "@Type", SqlDbType.NVarChar, Type,
                          "@UserID", SqlDbType.Int, user.sys_userid
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
