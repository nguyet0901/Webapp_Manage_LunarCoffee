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
    public class TrafficMonthGridModel : PageModel
    {
        public string _branch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(int branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_TrafficMonth]", CommandType.StoredProcedure
                       , "@branchID", SqlDbType.Int, Convert.ToInt32(branchID));

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
