using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Employee
{
    public class EfficiencyModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string date, string branch)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Employee_Efficiency]", CommandType.StoredProcedure,
                        "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                        "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                        "@branch", SqlDbType.Int, branch);

                }
                return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return Content("");
            }
            //return RevenueKTV.GetRevenue(date);
        }
    }
}
