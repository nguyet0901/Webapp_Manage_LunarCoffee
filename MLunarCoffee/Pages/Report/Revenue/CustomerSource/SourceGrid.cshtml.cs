using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Revenue.CustomerSource
{
    public class SourceGrid : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _TokenBranch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _TokenBranch = Request.Query["branchToken"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int branchID ,string branchToken ,string dateFrom ,string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_Sourcev2]" ,CommandType.StoredProcedure ,
                        "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@branchToken" ,SqlDbType.NVarChar ,branchToken != null ? branchToken.Replace("'" ,"").Trim() : "" ,
                        "@branchID" ,SqlDbType.DateTime ,Convert.ToInt32(branchID));
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadataBySerCat(int branchID ,string branchToken ,string dateFrom ,string dateTo ,int sourceID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Report_Sourcev2_By_ServiceType]" ,CommandType.StoredProcedure ,
                        "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@branchToken" ,SqlDbType.NVarChar ,branchToken != null ? branchToken.Replace("'" ,"").Trim() : "" ,
                        "@SourceID" ,SqlDbType.DateTime ,Convert.ToInt32(sourceID) ,
                        "@branchID" ,SqlDbType.DateTime ,Convert.ToInt32(branchID));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(int branchID ,string branchToken ,int sourceID ,string dateFrom ,string dateTo ,string beginID ,string limit ,int isRevenue)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                string storeName = (isRevenue == 0) ? "YYY_sp_Report_Sourcev2_Detail_Target" : "YYY_sp_Report_Sourcev2_Detail_Revenue";
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet($"[{storeName}]" ,CommandType.StoredProcedure ,
                        "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@branchToken" ,SqlDbType.NVarChar ,branchToken != null ? branchToken.Replace("'" ,"").Trim() : "" ,
                        "@branchID" ,SqlDbType.DateTime ,Convert.ToInt32(branchID) ,
                        "@SourceID" ,SqlDbType.DateTime ,Convert.ToInt32(sourceID) ,
                        "@Limit" ,SqlDbType.Int ,limit ,
                        "@BeginID" ,SqlDbType.Int ,beginID);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
