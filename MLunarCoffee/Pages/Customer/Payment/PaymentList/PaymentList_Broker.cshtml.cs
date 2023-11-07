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
    public class PaymentListBroker : PageModel
    {

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadDataBrokerList(int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_PaymentBroker_LoadList]", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID,
                      "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
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
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadBrokerInfo(int CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_PaymentBroker_LoadInfo]", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID);
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
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadDataDetailList(int CurrentID,int Type)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_PaymentBroker_LoadList_Detail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, CurrentID,
                       "@Type" ,SqlDbType.Int ,Type);
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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostUpdateSign_Broker(int id, string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_Payment_Broker_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                         "@Sign", SqlDbType.NVarChar, sign
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItemBroker(int id,int Type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt= await connFunc.ExecuteDataTable("[YYY_sp_Customer_PaymentBroker_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type" ,SqlDbType.Int ,Type ,
                        "@userID" , SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
