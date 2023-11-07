using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Commission.Bypaid
{
    public class BrokerModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }        
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();                        
            _SheetID = Request.Query["sheet"].ToString();
        }
        public async Task<IActionResult> OnPostLoadata(string branchID ,string dateFrom ,string dateTo ,int typedate)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Report_CommissionBrokerTotal]" ,CommandType.StoredProcedure                       
                       ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                       ,"@branchID" ,SqlDbType.Int ,Convert.ToInt32(branchID));
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

        public async Task<IActionResult> OnPostLoadataDetail(string branchID ,string BrokerId ,string dateFrom ,string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Report_CommissionBrokerDetail]" ,CommandType.StoredProcedure
                       ,"@brokerID" ,SqlDbType.Int ,Convert.ToInt32(BrokerId)
                       ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                       ,"@branchID" ,SqlDbType.Int ,Convert.ToInt32(branchID));
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
