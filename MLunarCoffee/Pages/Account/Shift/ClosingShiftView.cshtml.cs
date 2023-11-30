using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Account.Shift
{
    public class ClosingShiftViewModel : PageModel
    {
        public string _ClosingEntryID { get; set; }
        public void OnGet()
        {
            string current = Request.Query["CurrentID"];
            _ClosingEntryID = current != null ? current.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInitialize(string currentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Closing_Shift_View_Load]", CommandType.StoredProcedure,
                       "@ID", SqlDbType.Int, currentID
                        , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                        , "@UserId", SqlDbType.Int, user.sys_userid
                       );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string date, int branchid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Account_Shifts_Load]", CommandType.StoredProcedure,
                                "@date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(date.ToString())
                                , "@branchID", SqlDbType.Int, branchid
                                , "@getView", SqlDbType.Int, 1
                                );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int ID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Closing_Shift_History_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, ID,
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
