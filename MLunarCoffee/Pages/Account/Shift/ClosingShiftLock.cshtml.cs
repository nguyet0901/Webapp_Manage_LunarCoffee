using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account.Shift
{
    public class ClosingShiftLockModel : PageModel
    {
        public string _BranchID { get; set; }
        public void OnGet()
        {
            string branchid = Request.Query["BranchID"];
            _BranchID = !String.IsNullOrEmpty(branchid) ? branchid : "0";
        }

        public async Task<IActionResult> OnPostInitilize(int branchid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Closing_Shift_LoadInit]" ,CommandType.StoredProcedure ,
                         "@branchID" ,SqlDbType.Int ,branchid);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadLock(string date ,int branchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Closing_Shift_LockLoad]" ,CommandType.StoredProcedure ,
                       "@date" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(date.ToString())
                      ,"@branchID" ,SqlDbType.Int ,branchid);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Account_Shifts_Load]" ,CommandType.StoredProcedure ,
                       "@date" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(date.ToString())
                      ,"@branchID" ,SqlDbType.Int ,branchid
                      ,"@getView" ,SqlDbType.Int ,0
                      );
                    ds.Tables.Add(dt);
                }

                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string branchID ,string dateLock ,string content ,string amountCash ,string amountTransfer ,string amountPOS
            ,string amountTotal ,string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                content = content != null ? content : "";
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Closing_Shift_Insert]" ,CommandType.StoredProcedure ,
                        "@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchID) ,
                        "@DateLock" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateLock) ,
                        "@Content" ,SqlDbType.NVarChar ,(content != null) ? content.Replace("'" ,"").Trim() : "" ,
                        "@AmountCash" ,SqlDbType.Decimal ,amountCash ,
                        "@AmountTransfer" ,SqlDbType.Decimal ,amountTransfer ,
                        "@AmountPOS" ,SqlDbType.Decimal ,amountPOS ,
                        "@AmountTotal" ,SqlDbType.Decimal ,amountTotal ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@sign" ,SqlDbType.NVarChar ,sign);
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
