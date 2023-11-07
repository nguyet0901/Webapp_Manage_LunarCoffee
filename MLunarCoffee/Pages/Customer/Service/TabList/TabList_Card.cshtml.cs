using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_CardModel : PageModel
    {
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadataCard(int CustomerID, int id, int limit, int beginID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_Customer_Card_LoadList", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@id", SqlDbType.Int, id
                      , "@limit", SqlDbType.Int, limit
                      , "@beginID", SqlDbType.Int, beginID);
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItemCard(int id, int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Card_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@CustomerID", SqlDbType.Int, CustomerID,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLockAndUnlockCard(int id, int type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_ServiceCard_ChangeLock]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type", SqlDbType.Int, type,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
