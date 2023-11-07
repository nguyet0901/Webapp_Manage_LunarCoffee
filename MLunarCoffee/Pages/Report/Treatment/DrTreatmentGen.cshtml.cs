using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Treatment
{
    public class DrTreatmentGen : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _sourceID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _sourceID = Request.Query["SourceID"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }
        public async Task<IActionResult> OnPostLoadata(string branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_TreatmenAllowDr]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID)
                       , "@DentistCosmetic", SqlDbType.Int, Global.sys_DentistCosmetic != null ? Global.sys_DentistCosmetic : 0);
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
        public async Task<IActionResult> OnPostLoadataDetailCust(string branchID, string DocID, string type, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_TreatmenAllowDr_Detail_ByCust]", CommandType.StoredProcedure
                       , "@DocID", SqlDbType.Int, Convert.ToInt32(DocID)
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
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
        public async Task<IActionResult> OnPostLoadataDetailTreat(string branchID, string DocID, string type, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_TreatmenAllowDr_Detail_ByTreat]", CommandType.StoredProcedure
                       , "@DocID", SqlDbType.Int, Convert.ToInt32(DocID)
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
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
