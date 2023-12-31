using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Revenue.Labo
{
    public class LaboPieModel : PageModel
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

        public async Task<IActionResult> OnPostLoadata(int branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_LaboPie_v2]", CommandType.StoredProcedure,
                        "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                        "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                        "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID,
                        "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch,
                        "@branchID", SqlDbType.DateTime, Convert.ToInt32(branchID));
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }
    }
}
