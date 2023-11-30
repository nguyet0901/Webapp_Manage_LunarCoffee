using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Settings.Examination
{
    public class ExaminationListModel : PageModel
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
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_Settings_Examination_LoadList", CommandType.StoredProcedure);
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
                    await connFunc.ExecuteDataTable("MLU_sp_ST_Settings_Examination_Delete", CommandType.StoredProcedure,
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
