using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.WareHouse.ProductStatus
{
    public class ProductStatusGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _wareID { get; set; }
        public string _SheetID { get; set; }

        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _wareID = Request.Query["WareID"].ToString();
            _SheetID = Request.Query["sheet"].ToString();

        }
        public async Task<IActionResult> OnPostLoadata(int wareID, int monthExpired, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_WareHouse_ProductStatus]", CommandType.StoredProcedure,
                        "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@wareID", SqlDbType.Int, wareID
                       , "@monthExpired", SqlDbType.Int, monthExpired
                       );
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
        public async Task<IActionResult> OnPostLoadDataDetail(int wareID, int monthExpired, int packageID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_WareHouse_ProductStatusDetail]", CommandType.StoredProcedure,
                        "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@wareID", SqlDbType.Int, wareID
                       , "@packageID", SqlDbType.Int, packageID
                       , "@monthExpired", SqlDbType.Int, monthExpired
                       );
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
