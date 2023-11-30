using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Marketing.Voucher
{
    public class ExpenseVoucherModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _BranchID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _BranchID = Request.Query["branch"].ToString();
        }
        public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string branchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_rp_ExpenseVoucher]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID)
                       );
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
            catch (Exception)
            {
                return Content("0");
            }
        }
       

    }
}
