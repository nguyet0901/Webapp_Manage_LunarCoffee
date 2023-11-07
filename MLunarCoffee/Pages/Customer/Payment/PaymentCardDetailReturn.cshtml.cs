using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class PaymentCardDetailReturnModel : PageModel
    {
        public string _PaymentCustomerID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            if (cus.ToString() != String.Empty)
            {
                _PaymentCustomerID = cus.ToString();
            }
            else
            {
                _PaymentCustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }


        }
        public async Task<IActionResult> OnPostLoadComboMain(string CustomerID)
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
                    dt.TableName = "dataPaymentMethod";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Card_Payment_Return_LoadNew]" ,CommandType.StoredProcedure
                    ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                    dt.TableName = "dataCard";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Desposit_Reason_Return]" ,CommandType.StoredProcedure);
                    dt.TableName = "dataReasonReturn";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "MethodType";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data ,string customerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                string ChooseDate = user.sys_ChooseDateCustomer.ToString();
                CardReturnDetail DataMain = JsonConvert.DeserializeObject<CardReturnDetail>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Card_Payment_Return_Insert]" ,CommandType.StoredProcedure ,
                            "@Customer_ID" ,SqlDbType.Int ,customerID ,
                            "@CardID" ,SqlDbType.Int ,DataMain.CardID ,
                            "@Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Amount) ,
                            "@PaymentMethod_ID" ,SqlDbType.Int ,DataMain.MethodID ,
                            "@PosType" ,SqlDbType.Int ,DataMain.PosTypeID ,
                            "@TransferType" ,SqlDbType.Int ,DataMain.TransferTypeID ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@ReasonReturn" ,SqlDbType.Int ,DataMain.ReasonReturn ,
                            "@Branch_ID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.BranchID) != 0 ? Convert.ToInt32(DataMain.BranchID) : user.sys_branchID ,
                            "@CodeFormat" ,SqlDbType.Int ,Comon.Global.sys_CodePayment ,
                            "@Content" ,SqlDbType.NVarChar ,DataMain.Content != null ? DataMain.Content.Replace("'" ,"") : "" ,
                            //"@BankInvoiceCode" ,SqlDbType.NVarChar ,DataMain.BankInvoiceCode ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Created" ,SqlDbType.DateTime ,((ChooseDate == "1") ? Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.DateCreated) : (Comon.Comon.GetDateTimeNow()))
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
    public class CardReturnDetail
    {
        public int CardID { get; set; }
        public decimal Amount { get; set; }
        public int BranchID { get; set; }
        public int MethodID { get; set; }
        public int PosTypeID { get; set; }
        public int TransferTypeID { get; set; }
        public int MedthodDetail { get; set; }
        public int ReasonReturn { get; set; }
        public string DateCreated { get; set; }
        public string Content { get; set; }
        //public string BankInvoiceCode { get; set; }
    }
}
