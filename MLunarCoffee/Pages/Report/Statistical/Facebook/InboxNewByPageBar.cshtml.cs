using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SourceMain.Pages.Report.Statistical.Facebook
{
    public class InboxNewByPageBarModel : PageModel
    {
        public string _dateTo { get; set; }
        public string _dateFrom { get; set; }
        public string _TokenPage { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _TokenPage = Request.Query["TokenPage"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(string TokenPage, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[ZZZ_sp_Report_PageFacebook_Page]", CommandType.StoredProcedure,
                        "@TokenPage", SqlDbType.Int, TokenPage
                       , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt, Formatting.Indented));
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
