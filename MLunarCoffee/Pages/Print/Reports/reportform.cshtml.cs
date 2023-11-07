using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Print.Reports
{
    public class reportformModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString() ?? "";
            _dateTo = Request.Query["dateTo"].ToString() ?? "";
            _branchID = Request.Query["branch"].ToString() ?? "";
        }
        public async Task<IActionResult> OnPostInitialize(string branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_PrintReport_LoadBranch", CommandType.StoredProcedure
                        , "@BranchID", SqlDbType.Int, Convert.ToInt16(branchID.ToString()));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
