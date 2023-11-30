using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Report.Account
{
    public class BalanceCusModel : PageModel
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
        public async Task<IActionResult> OnPostLoadata(int branchID ,string dateFrom ,string dateTo ,string sersourceID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Accountant_Liability_Cosmetic]" ,CommandType.StoredProcedure ,
                        "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@branchID" ,SqlDbType.Int ,branchID
                       ,"@sersourceID" ,SqlDbType.Int ,sersourceID
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
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDataDetail(int branchID ,int custID ,string dateFrom ,string dateTo ,string sersourceID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Accountant_Liability_Cosmetic_Detail]" ,CommandType.StoredProcedure ,
                        "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@branchID" ,SqlDbType.Int ,branchID
                       ,"@CustID" ,SqlDbType.Int ,custID
                       ,"@sersourceID" ,SqlDbType.Int ,sersourceID
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
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
