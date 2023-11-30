using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Revenue.Branch
{
    public class TokenBranchGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _TokenBranch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _TokenBranch = Request.Query["branchToken"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string tokenBranch)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_Revenue_By_Branch_Token]", CommandType.StoredProcedure,
                        "@branchToken", SqlDbType.NVarChar, tokenBranch
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
