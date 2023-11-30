using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Revenue.Detail
{
    public class AllBranchGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _BranchID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _BranchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }
        public async Task<IActionResult> OnPostLoadInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                        dt.TableName = "ServiceSource";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadata(string dateFrom ,string dateTo ,string BranchID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_RevenueDetail]" ,CommandType.StoredProcedure ,
                        "@BranchID" ,SqlDbType.Int ,BranchID ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo));
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
