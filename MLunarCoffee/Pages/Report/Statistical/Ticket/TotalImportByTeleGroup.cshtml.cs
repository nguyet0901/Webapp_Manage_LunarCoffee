using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SourceMain.Pages.Report.Statistical.Ticket
{
    public class TotalImportByTeleGroupModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, int maxDates)
        {
            var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
            var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
            var totalDate = (date_To - date_From).TotalDays;
            if (totalDate > maxDates)
            {
                return Content("0");
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[ZZZ_sp_Report_TicketImport_Total_ByGroupTele]", CommandType.StoredProcedure
                            , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                            , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                    }
                    if (ds != null)
                    {
                        return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
                    }
                    else
                    {
                        return Content("");
                    }
                }
                catch (Exception)
                {
                    return Content("");
                }
            }

        }
    }
}
