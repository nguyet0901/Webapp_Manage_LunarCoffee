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

namespace MLunarCoffee.Pages.Student.General
{
    public class Desk : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadPayment(string branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_GenPaymentlist]", CommandType.StoredProcedure
                        , "@branchID", SqlDbType.Int, branchID
                        , "@Createdby", SqlDbType.Int, user.sys_userid
                      );
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadCourse(string branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_GenCourceStudent]", CommandType.StoredProcedure
                        , "@branchID", SqlDbType.Int, branchID
                        , "@Createdby", SqlDbType.Int, user.sys_userid);
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadClass(string branchID,string day)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_GenClassTime]", CommandType.StoredProcedure
                        , "@branchID", SqlDbType.Int, branchID
                         , "@Day", SqlDbType.Int, day
                        , "@Createdby", SqlDbType.Int, user.sys_userid
                      );
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
            catch (Exception)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostDeletePayment(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_ST_Payment_Delete", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                         "@UserID", SqlDbType.Int, user.sys_userid,
                         "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
