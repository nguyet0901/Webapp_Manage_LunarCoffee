using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Medicine
{
    public class ReportMedicineModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _BranchID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _BranchID = Request.Query["branch"].ToString();
        }
        public async Task<IActionResult> OnPostLoadData(string dateFrom ,string dateTo ,string BranchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Customer_MedicineProd]" ,CommandType.StoredProcedure ,
                        "@branchID" ,SqlDbType.Int ,BranchID ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadMed(string dateFrom ,string dateTo ,string BranchID ,string ProductID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Customer_Medicine]" ,CommandType.StoredProcedure ,
                        "@branchID" ,SqlDbType.Int ,BranchID ,
                        "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID ,
                        "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch ,
                        "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                        "@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                        "@ProductID" ,SqlDbType.Int ,Convert.ToInt32(ProductID)
                        );
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadataDetail(string ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Customer_Medicine_Detail]" ,CommandType.StoredProcedure
                       ,"@ID" ,SqlDbType.Int ,Convert.ToInt32(ID)
                       );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
