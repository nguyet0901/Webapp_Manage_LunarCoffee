using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Account
{
    public class DebtByArisingGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _SheetID { get; set; }

        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Debt_By_Arise]", CommandType.StoredProcedure,
                        "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@DentistCosmetic", SqlDbType.Int, Global.sys_DentistCosmetic != null ? Global.sys_DentistCosmetic : 0
                       , "@branchID", SqlDbType.Int, branchID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
