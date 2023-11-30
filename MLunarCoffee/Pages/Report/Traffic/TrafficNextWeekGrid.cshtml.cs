using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Traffic
{
    public class TrafficNextWeekGridModel : PageModel
    {
        public string _TokenBranch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _TokenBranch = Request.Query["branchToken"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string tokenBranch)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_TrafficNextWeek]", CommandType.StoredProcedure);

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
