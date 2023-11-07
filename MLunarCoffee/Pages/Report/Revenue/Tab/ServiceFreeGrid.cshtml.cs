using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SourceMain.Pages.Report.RevenuePayment.Tab
{
    public class ServiceFreeGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _BranchID { get; set; }
        public string _SheetID { get; set; }
        public string _TokenBranchUser { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _BranchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
            _TokenBranchUser = Request.Query["branchTokenUser"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string branchID, string TokenBranchUser)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[ZZZ_sp_Report_Customer_ServiceFree_V2]", CommandType.StoredProcedure,
                        "@branchID", SqlDbType.Int, branchID
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        , "@branchTokenUser", SqlDbType.NVarChar, TokenBranchUser
                        );

                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt, Formatting.Indented));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }

        }
    }
}
