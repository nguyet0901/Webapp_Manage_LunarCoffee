using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class PaymentCardDetailModel : PageModel
    {
        public string _PaymentCustomerID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _dataPaymentMethod { get; set; }
        public string _dataCard { get; set; }
        public string _dataPaymentPostType { get; set; }
        public string _dataPaymentTransferType { get; set; }
        public string _depositPayment { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public PaymentCardDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }

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

        public async Task<IActionResult> OnPostLoadInitialize(string CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
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
                    dt = await confunc.ExecuteDataTable("MLU_sp_Customer_Card_Payment_LoadNew" ,CommandType.StoredProcedure
                    ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                    dt.TableName = "CustCard";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Deposit_LoadPayment]" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                    dt.TableName = "CustDeposit";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "MethodType";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string customerID ,string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                string ChooseDate = user.sys_ChooseDateCustomer.ToString();
                CardDetail DataMain = JsonConvert.DeserializeObject<CardDetail>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Card_Payment_Insert]" ,CommandType.StoredProcedure ,
                            "@Customer_ID" ,SqlDbType.Int ,customerID ,
                            "@CardID" ,SqlDbType.Int ,DataMain.CardID ,
                            "@Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Amount) ,
                            "@AmountDeposit" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.AmountDeposit) ,
                            "@PaymentMethod_ID" ,SqlDbType.Int ,DataMain.MethodID ,
                            "@PosType" ,SqlDbType.Int ,DataMain.PosTypeID ,
                            "@TransferType" ,SqlDbType.Int ,DataMain.TransferTypeID ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID ,
                            "@CodeFormat" ,SqlDbType.Int ,Comon.Global.sys_CodePayment ,
                            "@Content" ,SqlDbType.NVarChar ,DataMain.Content != null ? DataMain.Content.Replace("'" ,"") : "" ,
                            "@billCode" ,SqlDbType.NVarChar ,DataMain.BillCode != null ? DataMain.BillCode.Replace("'" ,"") : "" ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@BankInvoiceCode" ,SqlDbType.NVarChar ,DataMain.BankInvoiceCode ,
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
        public class CardDetail
        {
            public int CardID { get; set; }
            public decimal Amount { get; set; }
            public decimal AmountDeposit { get; set; }
            public int BranchID { get; set; }
            public int MethodID { get; set; }
            public int PosTypeID { get; set; }
            public int TransferTypeID { get; set; }
            public int MedthodDetail { get; set; }
            public string DateCreated { get; set; }
            public string Content { get; set; }
            public string BillCode { get; set; }
            public string BankInvoiceCode { get; set; }
        }
    }
}
