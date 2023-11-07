using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SourceMain.Pages.Report.ReportAll
{
    public class CustomerActivity_ByAppointmentModel : PageModel
    {
        public string _date { get; set; }
        public string _BranchID { get; set; }
        public string _SheetID { get; set; }
        public int _keyTreamentPercent { get; set; }
        public void OnGet()
        {
            _date = Request.Query["dateFrom"].ToString();
            _BranchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
            _keyTreamentPercent = Comon.Global.sys_PercentTreatment;
        }

         public async Task<IActionResult> OnPostLoadata(string date, string BranchID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[ZZZ_sp_Report_Customer_Activity_Appointment]", CommandType.StoredProcedure,
                        "@branchID", SqlDbType.Int, BranchID
                        , "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(date));
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
