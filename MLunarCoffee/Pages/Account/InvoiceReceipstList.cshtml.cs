using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account
{
    public class InvoiceReceipstListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

         public async Task<IActionResult> OnPostLoadComboInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    if (dt != null && dt.Rows.Count != 0)
                        dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Account_Reason]", CommandType.StoredProcedure);
                    dt.TableName = "AccountReason";
                    ds.Tables.Add(dt);
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
            catch
            {
                return Content("[]");
            }
        }
    
         public async Task<IActionResult> OnPostLoadataAccount(string date, int branchid)
        {
            var user = Session.GetUserSession(HttpContext);
            DataSet ds = new DataSet();
            string dateFrom = "";
            string dateTo = "";
            if (date.Contains(" to "))
            {
                date = date.Replace(" to ", "@");
                dateFrom = date.Split('@')[0];
                dateTo = date.Split('@')[1];
            }
            else
            {
                dateFrom = date;
                dateTo = date;
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Account_Receipts_LoadList]", CommandType.StoredProcedure,
                  "@UserID", SqlDbType.Int, user.sys_userid
                  , "@branchid", SqlDbType.Int, branchid
                  , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                  , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                  , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
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
  
         public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Account_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@BranchID", SqlDbType.Int, user.sys_branchID
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
