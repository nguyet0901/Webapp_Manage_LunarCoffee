using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment.PaymentList
{
    public class PaymentList_ServiceModel : PageModel
    {

        public string _dtPermissionCurrentPage { get; set; }
        // public int _TinyShow { get; set; }
        public void OnGet()
        {
            //  var typeshow = Request.Query["typeshow"];
            // _TinyShow = typeshow.ToString () != String.Empty ? 1 : 0;
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostCheckingLock(string CustomerID)
        {
            try
            {
                int _PaymentIsLock = 0;
                var user = Session.GetUserSession(HttpContext);
                bool _lock = await Comon.Option_Extension.Extension.CheckCashierTime(Convert.ToInt32(CustomerID) ,Convert.ToInt32(user.sys_branchID) ,user.sys_Rule_OptionExtension.TimeInvoice.Value.ToString());
                if (!_lock)
                    _PaymentIsLock = 1;
                else _PaymentIsLock = 0;
                return Content(_PaymentIsLock.ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataPayment(string CustomerID ,string CurrentID ,string CurrentType)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Payment_Service_Deposit_LoadList2]" ,CommandType.StoredProcedure ,
                      "@Customer_ID" ,SqlDbType.Int ,CustomerID
                      ,"@UserId" ,SqlDbType.Int ,user.sys_userid
                      ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                      ,"@CurrentType" ,SqlDbType.NVarChar ,CurrentType != null ? CurrentType : ""
                      ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
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

        public async Task<IActionResult> OnPostRemoveEInvoice(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Remove_EInvoice]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                         "@User" ,SqlDbType.Int ,user.sys_userid
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateEInvoice(int id ,string EInvoice)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Update_EInvoice]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                         "@User" ,SqlDbType.Int ,user.sys_userid ,
                        "@EInvoice" ,SqlDbType.NVarChar ,EInvoice
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostGetSign_Payment(int id ,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Get_Sign]" ,CommandType.StoredProcedure ,
                        "@PaidID" ,SqlDbType.Int ,id ,
                        "@type" ,SqlDbType.NVarChar ,type
                    );
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostUpdateSign_Payment(int id ,string sign ,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                switch (type)
                {
                    case "payment":
                        {
                            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                            {
                                dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Update_Sign]" ,CommandType.StoredProcedure ,
                                    "@CurrentID" ,SqlDbType.Int ,id ,
                                    "@userID" ,SqlDbType.Int ,user.sys_userid ,
                                     "@Sign" ,SqlDbType.NVarChar ,sign
                                );
                                return Content(Comon.DataJson.GetFirstValue(dt));
                            }
                        }
                        break;
                    case "deposit":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Deposit_Update_Sign]" ,CommandType.StoredProcedure ,
                            "@CurrentID" ,SqlDbType.Int ,id ,
                            "@userID" ,SqlDbType.Int ,user.sys_userid ,
                            "@Sign" ,SqlDbType.NVarChar ,sign
                            );
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        break;
                    default: break;
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostDeleteItem(int id ,string type ,int customerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                switch (type)
                {
                    case "payment":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Delete]" ,CommandType.StoredProcedure ,
                                "@CurrentID" ,SqlDbType.Int ,id ,
                                 "@CustomerID" ,SqlDbType.Int ,customerID ,
                                  "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                                "@userID" ,SqlDbType.Int ,user.sys_userid
                            );
                        }
                        break;
                    case "deposit":
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Deposit_Delete]" ,CommandType.StoredProcedure ,
                                "@CurrentID" ,SqlDbType.Int ,id ,
                                 "@CustomerID" ,SqlDbType.Int ,customerID ,
                                "@userID" ,SqlDbType.Int ,user.sys_userid
                            );
                        }
                        break;
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostUnlockpayment(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Extension_UnClock_Payment]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #region  // Mark Color Payment
        public async Task<IActionResult> OnPostPaymentMarkColor(int id ,string colorcode)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Update_MarkColor]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@ColorCode" ,SqlDbType.NVarChar ,colorcode
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #endregion
    }
}
