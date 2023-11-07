using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.General.Course.Payment
{
    public class PaymentListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
        }

        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Branch_Training_Load]", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
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
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadData(string branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Payment_LoadList]", CommandType.StoredProcedure,
                        "@BranchID", SqlDbType.Int, branchID,
                        "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                        "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                        "@UserId", SqlDbType.Int, user.sys_userid,
                        "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
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
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_ST_Payment_Delete", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, Convert.ToInt16(CurrentID),
                         "@UserID", SqlDbType.Int, user.sys_userid,
                         "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
