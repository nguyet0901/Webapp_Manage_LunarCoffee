using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Employee.Timekeeping
{
    public class TimekeepingGridModel : PageModel
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

        public async Task<IActionResult> OnPostLoadata(string dateFrom ,string dateTo ,string BranchID ,string typedate)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Employee_TimekeepingGrid]" ,CommandType.StoredProcedure ,
                        "@branchID" ,SqlDbType.Int ,BranchID ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        );
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
