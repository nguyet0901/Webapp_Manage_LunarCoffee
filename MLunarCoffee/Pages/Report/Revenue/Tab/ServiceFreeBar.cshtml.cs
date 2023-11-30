using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.RevenuePayment.Tab
{
    public class ServiceFreeBarModel : PageModel
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

         public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_Customer_ServiceFree]", CommandType.StoredProcedure,
                        "@branchID", SqlDbType.Int, branchID
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        );

                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
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
