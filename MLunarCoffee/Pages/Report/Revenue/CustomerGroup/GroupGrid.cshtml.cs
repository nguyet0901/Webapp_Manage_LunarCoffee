using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;


namespace MLunarCoffee.Pages.Report.Revenue.CustomerGroup
{
    public class GroupGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _TokenBranch { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _TokenBranch = Request.Query["branchToken"].ToString();
        }
        public async Task<IActionResult> OnPostLoadata(string branchToken ,string dateFrom ,string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_Group]" ,CommandType.StoredProcedure
                        ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                        ,"@branchToken" ,SqlDbType.NVarChar ,branchToken != null ? branchToken.Replace("'" ,"").Trim() : ""
                        ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@GroupID" ,SqlDbType.Int ,0
                        ,"@Limit" ,SqlDbType.Int ,0
                        ,"@BeginID" ,SqlDbType.Int ,0
                        ,"@IsRevenue" ,SqlDbType.Int ,0);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string branchToken ,string dateFrom ,string dateTo, int groupID, int isRevenue, int limit, int beginID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_Group]" ,CommandType.StoredProcedure
                        ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                        ,"@branchToken" ,SqlDbType.NVarChar ,branchToken != null ? branchToken.Replace("'" ,"").Trim() : ""
                        ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)

                        ,"@GroupID" ,SqlDbType.Int , groupID
                        ,"@Limit" ,SqlDbType.Int , limit
                        ,"@BeginID" ,SqlDbType.Int , beginID
                        ,"@IsRevenue" ,SqlDbType.Int , isRevenue
                        );
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
