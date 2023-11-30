using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.CustomerCare
{
    public class AfterTreatmentGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string branchID, int maxdate)
        {
            try
            {
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt =await  confunc.ExecuteDataTable("[sp_Report_CustomerCare_AfterTreatment]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                            , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                            , "@branchID", SqlDbType.NVarChar, branchID
                        );
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
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }
    }
}
