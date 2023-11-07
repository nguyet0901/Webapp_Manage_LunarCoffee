using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
namespace MLunarCoffee.Pages.Report.ServiceCat.Revenue
{
    public class RevenueGeneral : PageModel
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
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[YYY_sp_rp_RevenueSerCatGeneral]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
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
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadataday(string branchID, string datefromInt, string datetoInt)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[YYY_sp_rp_RevenueSerCatGeneral_Day]", CommandType.StoredProcedure
                       , "@DateFromInt", SqlDbType.DateTime, Convert.ToInt32(datefromInt)
                       , "@DateToInt", SqlDbType.DateTime, Convert.ToInt32(datetoInt)
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
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
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadataDetail(string branchID,string type, string ServiceCat, string dateFrom, string dateTo)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_rp_RevenueSerCatGeneral_Detail]", CommandType.StoredProcedure
                       , "@ServiceCat", SqlDbType.Int, Convert.ToInt32(ServiceCat)
                       , "@type", SqlDbType.Int, Convert.ToInt32(type)
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
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
                return Content("[]");
            }
        }

    }
}
