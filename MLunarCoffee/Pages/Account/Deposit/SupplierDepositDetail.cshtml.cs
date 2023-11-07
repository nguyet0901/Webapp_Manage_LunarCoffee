using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account.Deposit
{
    public class SupplierDepositDetailModel : PageModel
    {
        public string SupID { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            SupID = Request.Query["SupID"].ToString();
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadPara("Method");
                    dt.TableName = "Method";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "MethodType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Supplier_LoadCombo]" ,CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                Desposit DataMain = JsonConvert.DeserializeObject<Desposit>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Supplier_Deposit_Insert" ,CommandType.StoredProcedure
                        ,"@SupplierID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.SupplierID)
                        ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.BranchID)
                        ,"@PaymentMethodID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PaymentMethodID)
                        ,"@Amount" ,SqlDbType.Decimal ,DataMain.Amount
                        ,"@Note" ,SqlDbType.NVarChar ,DataMain.Note
                        ,"@MethodType" ,SqlDbType.Int ,Convert.ToInt32(DataMain.MethodType)
                        ,"@PosTypeID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PosTypeID)
                        ,"@TransferTypeID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TransferTypeID)
                        ,"@MedthodDetail" ,SqlDbType.Int ,Convert.ToInt32(DataMain.MedthodDetail)
                        ,"@BankInvoiceCode" ,SqlDbType.NVarChar ,DataMain.BankInvoiceCode
                        ,"@BillCode" ,SqlDbType.NVarChar ,DataMain.BillCode
                        ,"@DateCreated" ,SqlDbType.DateTime ,((user.sys_ChooseDateCustomer == 1) ? (Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.DateCreate.ToString())) : (Comon.Comon.GetDateTimeNow()))
                        ,"@Created_By" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class Desposit
    {
        public string SupplierID { get; set; }
        public string BranchID { get; set; }
        public string PaymentMethodID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string MethodType { get; set; }
        public string PosTypeID { get; set; }
        public string TransferTypeID { get; set; }
        public string MedthodDetail { get; set; }
        public string BankInvoiceCode { get; set; }
        public string BillCode { get; set; }
        public string DateCreate { get; set; }
    }
}
