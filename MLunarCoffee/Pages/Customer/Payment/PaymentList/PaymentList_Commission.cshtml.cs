using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.Payment.PaymentList
{

    public class PaymentList_CommissionModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostLoadataTabCommission(int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Commission_Employee_LoadList]", CommandType.StoredProcedure,
                        "@Customer_ID", SqlDbType.Int, CustomerID
                        , "@UserId", SqlDbType.Int, user.sys_userid
                        , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItemCms(int id, string type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                switch (type)
                {
                    case "consult":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("[YYY_sp_Customer_Commission_Delete]", CommandType.StoredProcedure,
                                "@CurrentID", SqlDbType.Int, id,
                                "@userID", SqlDbType.Int, user.sys_userid);
                        }
                        break;
                    case "tele":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("[YYY_sp_Customer_Tele_Commission_Delete]" , CommandType.StoredProcedure,
                                "@CurrentID", SqlDbType.Int, id,
                                "@userID", SqlDbType.Int, user.sys_userid);
                        }
                        break;
                    case "doctor":
                    case "assistant":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_Commission_Delete]", CommandType.StoredProcedure,
                                "@CurrentID", SqlDbType.Int, id,
                                "@userID", SqlDbType.Int, user.sys_userid);
                        }
                        break;
                    default:
                        return Content("0");
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
