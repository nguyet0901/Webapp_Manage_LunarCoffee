using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.WareHouse.Inventory
{
    public class TicketModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadTicket(string WareID, string ProductID, string DateFrom, string DateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_rp_WareHouse_Norm_TicketAll_V2]", CommandType.StoredProcedure,
                      "@WareID", SqlDbType.Int, WareID,
                      "@ProductID", SqlDbType.Int, ProductID,
                      "@DateFrom", SqlDbType.Int, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateFrom),
                      "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateTo),
                      "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID,
                       "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
