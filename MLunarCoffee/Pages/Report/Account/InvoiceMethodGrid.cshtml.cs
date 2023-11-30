using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Account
{
    public class InvoiceMethodGridModel : PageModel
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


        public async Task<IActionResult> OnPostLoadata(string branchID ,string dateFrom ,string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_rp_InvoiceMethodGrid]" ,CommandType.StoredProcedure
                       ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                       ,"@IsMaster" ,SqlDbType.Int ,1
                       ,"@MethodID" ,SqlDbType.Int ,0
                       ,"@PosID" ,SqlDbType.Int ,0
                       ,"@TransferID" ,SqlDbType.Int ,0 );
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadataDetail(int branchID ,string dateFrom ,string dateTo ,string MethodID ,string PosID ,string TransferID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_rp_InvoiceMethodGrid]" ,CommandType.StoredProcedure
                       ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                       ,"@IsMaster" ,SqlDbType.Int ,0
                       ,"@MethodID" ,SqlDbType.Int ,Convert.ToInt32(MethodID)
                       ,"@PosID" ,SqlDbType.Int ,Convert.ToInt32(PosID)
                       ,"@TransferID" ,SqlDbType.Int ,Convert.ToInt32(TransferID));
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
