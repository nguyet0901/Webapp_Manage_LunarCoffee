using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.AppCustomer.Desk.User
{
    public class UserListModel : PageModel
    {

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_UserPortal_LoadList]", CommandType.StoredProcedure);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
