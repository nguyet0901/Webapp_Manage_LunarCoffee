using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Account.Shift
{
    public class ClosingShiftHistoryModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = !string.IsNullOrEmpty(_layout) ? _layout : "";
        }

        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadHistory(string DateFrom, string DateTo, string branchid, string Limit, string BeginID, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Closing_Shift_History_LoadList]", CommandType.StoredProcedure,
                       "@DateFrom", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                        , "@DateTo", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                        , "@branchID", SqlDbType.Int, branchid
                        , "@Limit", SqlDbType.Int, Convert.ToInt32(Limit)
                        , "@BeginID", SqlDbType.Int, Convert.ToInt32(BeginID)
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        
                        );
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
