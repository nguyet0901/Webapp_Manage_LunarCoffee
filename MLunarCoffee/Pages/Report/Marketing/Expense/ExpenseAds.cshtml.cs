using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Marketing.Expense
{
    public class ExpenseAdsModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
        }
        public async Task<IActionResult> OnPostLoadata(string branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_rp_ExpenseAds]", CommandType.StoredProcedure
                       ,"@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
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
        public async Task<IActionResult> OnPostLoadataDetail(string branchID, string dateFrom, string dateTo, int serviceid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_ExpenseAds_Detail]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID)
                       , "@ServiceID", SqlDbType.Int, serviceid);
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
