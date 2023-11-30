using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment.PaymentList
{

    public class PaymentList_CardModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        //public int _TinyShow { get; set; }
        public void OnGet()
        {
            // var typeshow = Request.Query["typeshow"];
            //if (typeshow.ToString () != String.Empty) {
            //    _TinyShow = 1;
            //}
            //else _TinyShow = 0;
            _dtPermissionCurrentPage = HttpContext.Request.Path;

        }


        public async Task<IActionResult> OnPostLoadataPaymentCard(string CustomerID ,int IsTiny ,int id ,int limit ,int beginID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Card_Payment_LoadList]" ,CommandType.StoredProcedure ,
                      "@Customer_ID" ,SqlDbType.Int ,CustomerID
                      ,"@UserId" ,SqlDbType.Int ,user.sys_userid
                      ,"@IsTiny" ,SqlDbType.Int ,IsTiny
                      ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                      ,"@id" ,SqlDbType.Int ,id
                      ,"@limit" ,SqlDbType.Int ,limit
                      ,"@beginID" ,SqlDbType.Int ,beginID);
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
        public async Task<IActionResult> OnPostGetSign_Payment(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PaymentCard_Get_Sign]" ,CommandType.StoredProcedure ,
                        "@PaidID" ,SqlDbType.Int ,id
                    );
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateSign_PaymentCard(int id ,string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Card_Payment_Update_Sign]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid ,
                         "@Sign" ,SqlDbType.NVarChar ,sign
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDeleteItemCard(int id ,string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Card_Payment_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid ,
                        "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                        "@BranchID" ,SqlDbType.Int ,user.sys_branchID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        #region  // Mark Color Payment

        public async Task<IActionResult> OnPostPaymentCardMarkColor(int id ,string colorcode)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_Card_Update_MarkColor]" ,CommandType.StoredProcedure ,
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
