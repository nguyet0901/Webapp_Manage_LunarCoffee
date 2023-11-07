using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.AppCustomer.Desk.Appointment
{
    public class AppHandleDetailModel : PageModel
    {
        public string _appID { get; set; }
        public string _phone { get; set; }
        public string _custID { get; set; }
        public void OnGet()
        {
            string AppID = Request.Query["appID"];
            string phone = Request.Query["phone"];
            string custID = Request.Query["custID"];
            _appID = AppID != null ? AppID : "0";
            _phone = phone != null ? phone : "";
            _custID = custID != null ? custID : "0";
        }
        public async Task<IActionResult> OnPostSearchCustomer(string textsearch, int custID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_AC_Schedule_SearchCustomer]", CommandType.StoredProcedure
                        , "@TextSearch", SqlDbType.NVarChar, textsearch.Replace("'", "").Trim()
                        , "@CustID", SqlDbType.Int, custID
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

        public async Task<IActionResult> OnPostExcuteData(int appID, int userID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_AC_Schedule_Customer_UpdateUser]", CommandType.StoredProcedure,
                        "@appID", SqlDbType.Int, appID,
                        "@userID", SqlDbType.Int, userID
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

        public async Task<IActionResult> OnPostCancelApp(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_Schedule_CancelApp]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@UserID", SqlDbType.Int, user.sys_userid);
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
