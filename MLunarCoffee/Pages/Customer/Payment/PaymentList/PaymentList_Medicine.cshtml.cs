using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment.PaymentList
{

    public class PaymentList_MedicineModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }

        public void OnGet()
        {
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadataPaymentMedicine(string CustomerID ,int id ,int limit ,int beginID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_Customer_Medicine_Payment_LoadList" ,CommandType.StoredProcedure ,
                  "@Customer_ID" ,SqlDbType.Int ,CustomerID
                  ,"@UserId" ,SqlDbType.Int ,user.sys_userid
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

        public async Task<IActionResult> OnPostDeleteItemMedicine(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Medicine_Payment_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
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
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PaymentMedicine_Get_Sign]" ,CommandType.StoredProcedure ,
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
        public async Task<IActionResult> OnPostUpdateSign_PaymentMedicine(int id ,string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_Medicine_Payment_Update_Sign" ,CommandType.StoredProcedure ,
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
    }
}
