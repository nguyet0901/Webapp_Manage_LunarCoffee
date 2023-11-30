using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Settings.Source
{
    public class SourceListModel : PageModel
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
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_Settings_Source_LoadList", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("MLU_sp_ST_Settings_Source_Delete", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, Convert.ToInt16(CurrentID),
                         "@UserID", SqlDbType.Int, user.sys_userid
                     );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
