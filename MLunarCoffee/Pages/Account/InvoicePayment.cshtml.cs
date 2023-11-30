using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Account
{
    public class InvoicePaymentModel : PageModel
    {
        public string CurrentPath { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
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
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    if (dt != null && dt.Rows.Count != 0)
                        return Content(Comon.DataJson.Datatable(dt));
                    else
                        return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadComboCashier(string branchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_Cashier", CommandType.StoredProcedure
                        , "@Branch_ID", SqlDbType.Int, Convert.ToInt16(branchID)
                        , "@isAllBranch", SqlDbType.Int, 0);
                    if (dt != null && dt.Rows.Count != 0)
                        return Content(Comon.DataJson.Datatable(dt));
                    else
                        return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoad(string branchid, string branchtoken, string cashierid, string dateFrom, string dateTo, int maxDate)
        {

            try
            {
                var Date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var Date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var TotalDate = (Date_To - Date_From).TotalDays;
                if (TotalDate > maxDate)
                {
                    return Content("0");
                }

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_Account_GeneralIniv2]", CommandType.StoredProcedure
                        , "@branchid", SqlDbType.Int, Convert.ToInt32(branchid)
                        , "@branchToken", SqlDbType.NVarChar, branchtoken
                        , "@cashierID", SqlDbType.Int, Convert.ToInt32(cashierid)
                        , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

    }
}
