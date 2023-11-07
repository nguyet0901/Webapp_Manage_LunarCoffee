using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.General
{
    public class GeneralGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _SheetID { get; set; }
        public string _sys_TreatManualamount { get; set; }

        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
            _sys_TreatManualamount = Comon.Global.sys_TreatManualamount.ToString();
        }

        public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_Generalv2]", CommandType.StoredProcedure,
                       "@branchID", SqlDbType.Int, branchID,
                       "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID,
                       "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch,
                       "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                       "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
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
            catch (Exception)
            {
                return Content("0");
            }

        }
    }
}
