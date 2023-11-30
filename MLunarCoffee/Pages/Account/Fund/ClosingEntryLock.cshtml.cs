using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account.Fund
{
    public class ClosingEntryLock : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadLock(string date ,int branchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Closing_Entry_LockLoad]" ,CommandType.StoredProcedure ,
                       "@date" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(date.ToString())
                      ,"@branchID" ,SqlDbType.Int ,branchid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string branchID ,string dateLock ,string content ,string amountCash ,string amountTrans ,string amountPOS
            ,string amountTotal ,string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                content = content != null ? content : "";
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Closing_Entry_Insert]" ,CommandType.StoredProcedure ,
                            "@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchID) ,
                            "@DateLock" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateLock) ,
                            "@Content" ,SqlDbType.NVarChar ,(content != null) ? content.Replace("'" ,"").Trim() : "" ,
                            "@AmountCash" ,SqlDbType.Decimal ,amountCash ,
                            "@AmountTrans" ,SqlDbType.Decimal ,amountTrans ,
                            "@AmountPOS" ,SqlDbType.Decimal ,amountPOS ,
                            "@AmountTotal" ,SqlDbType.Decimal ,amountTotal ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@sign" ,SqlDbType.NVarChar ,sign);
                    return Content(Comon.DataJson.GetValueRowProperty(dt ,0 ,"RESULT"));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
