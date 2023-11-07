using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer
{

    public class StatusListModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public string _StatusCustomerID { get; set; }
        public void OnGet()
        {

            _dtPermissionCurrentPage = HttpContext.Request.Path;
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _StatusCustomerID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerStatus_LoadType]" ,CommandType.StoredProcedure);
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadataStatus(string CustomerID, int id, int limit, int beginID, int Type, int TypeParent)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Status_Load", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@id", SqlDbType.Int, id
                      , "@limit", SqlDbType.Int, limit
                      , "@beginID", SqlDbType.Int, beginID
                      , "@Type", SqlDbType.Int,Type
                      , "@TypeParent" ,SqlDbType.Int ,TypeParent
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
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_Status_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
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
