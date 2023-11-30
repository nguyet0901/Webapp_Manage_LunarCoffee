using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class PaymentDetailModel : PageModel
    {

        //public string _dataPaymentDetail { get; set; }
        //public string _depositPayment { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string _disable_enter_amount_payment { get; set; }
        public string _dtPermissionCurrentPage { get; set; }

        private readonly IHubContext<NotiHub> hubContext;
        public PaymentDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            //var cus = Request.Query["CustomerID"];
            var user = Session.GetUserSession(HttpContext);
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _disable_enter_amount_payment = Comon.Global.sys_disable_enter_amount_payment.ToString();
            _dtPermissionCurrentPage = HttpContext.Request.Path;

        }


        public async Task<IActionResult> OnPostLoadInitialize(string CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtPaymentDetail = new DataTable();
                        dtPaymentDetail = await confunc.ExecuteDataTable("MLU_sp_Customer_PaymentDetail_LoadNew" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                            ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(Comon.Global.sys_DentistCosmetic));
                        dtPaymentDetail.TableName = "PaymentDetail";
                        return dtPaymentDetail;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMethod = new DataTable();
                        dtMethod = await confunc.LoadPara("Method");
                        dtMethod.TableName = "Method";
                        return dtMethod;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMethodType = new DataTable();
                        dtMethodType = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtMethodType.TableName = "MethodType";
                        return dtMethodType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDeposit = new DataTable();
                        dtDeposit = await confunc.ExecuteDataTable("[MLU_sp_Customer_Deposit_LoadPayment]" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                        dtDeposit.TableName = "Deposit";
                        return dtDeposit;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataDetail ,string CustomerID ,string AppToken ,string AppPlatform ,string AppUser)
        {
            try
            {
                PaymentDetail DataMain = JsonConvert.DeserializeObject<PaymentDetail>(data);
                DataTable DataDetailPayment = JsonConvert.DeserializeObject<dtPaymentDetailService>(dataDetail);
                DataTable DataValid = new DataTable();
                DataValid.Columns.Add("ID" ,typeof(Int32));
                DataValid.Columns.Add("Amount" ,typeof(Decimal));

                DataTable DataDetail = new DataTable();
                DataDetail.Columns.Add("TabID" ,typeof(Int32));
                DataDetail.Columns.Add("Amount" ,typeof(Decimal));
                DataDetail.Columns.Add("Note" ,typeof(String));
                DataDetail.Columns.Add("IsPaidFree" ,typeof(Int32));
                for (int i = 0; i < DataDetailPayment.Rows.Count; i++)
                {

                    DataRow dr = DataDetail.NewRow();
                    dr["TabID"] = Convert.ToInt32(DataDetailPayment.Rows[i]["TabID"]);
                    dr["Amount"] = Convert.ToDecimal(DataDetailPayment.Rows[i]["Amount"]);
                    dr["Note"] = Convert.ToString(DataDetailPayment.Rows[i]["Note"]);
                    dr["IsPaidFree"] = Convert.ToInt32(DataDetailPayment.Rows[i]["IsPaidFree"]);
                    DataDetail.Rows.Add(dr);

                    DataRow drva = DataValid.NewRow();
                    drva["ID"] = Convert.ToInt32(DataDetailPayment.Rows[i]["TabID"]);
                    drva["Amount"] = Convert.ToDecimal(DataDetailPayment.Rows[i]["Amount"]);
                    DataValid.Rows.Add(drva);
                }

                var user = Session.GetUserSession(HttpContext);


                if (!await Comon.Option_Extension.Extension.CheckCashierTime(Convert.ToInt32(CustomerID) ,Convert.ToInt32(user.sys_branchID) ,user.sys_Rule_OptionExtension.TimeInvoice.Value.ToString()))
                    return Content("IsLocked");
                DataTable dtCus = new DataTable();
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtCus = await connFunc.ExecuteDataTable("MLU_sp_Customer_Payment_Insert2" ,CommandType.StoredProcedure ,
                          "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                          "@Amount" ,SqlDbType.Decimal ,DataMain.Amount ,
                          "@PaymentMethod_ID" ,SqlDbType.Int ,DataMain.MethodID ,
                          "@PosType" ,SqlDbType.Int ,DataMain.PosType ,
                          "@TransferType" ,SqlDbType.Int ,DataMain.TransferType ,
                          "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                          "@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID ,
                          "@CodeFormat" ,SqlDbType.Int ,Comon.Global.sys_CodePayment ,
                          "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"") ,
                          "@BillCode" ,SqlDbType.NVarChar ,DataMain.BillCode != null ? DataMain.BillCode.Replace("'" ,"") : "" ,
                          "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                          "@Created" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                          "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer.ToString() ,
                          "@dateCreated" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.dateCreated.ToString()) ,
                          "@PaymentDeposit" ,SqlDbType.Decimal ,DataMain.PaymentDeposit ,
                          "@BankInvoiceCode" ,SqlDbType.NVarChar ,DataMain.BankInvoiceCode ,
                          "@table_data" ,SqlDbType.Structured ,(DataDetail.Rows.Count > 0 ? DataDetail : null) ,
                          "@valid_data" ,SqlDbType.Structured ,(DataValid.Rows.Count > 0 ? DataValid : null)
                      );
                }
                return Content(Comon.DataJson.Datatable(dtCus));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }
    public class PaymentDetail
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string BillCode { get; set; }
        public string CustomerName { get; set; }
        public int BranchID { get; set; }
        public int MethodID { get; set; }
        public int PosType { get; set; }
        public int TransferType { get; set; }
        public int MedthodDetail { get; set; }
        public string Amount { get; set; }
        public string PaymentDeposit { get; set; }
        public string dateCreated { get; set; }
        public string BankInvoiceCode { get; set; }
    }
    internal class dtPaymentDetailService : DataTable
    {
        public dtPaymentDetailService()
        {
            Columns.Add("TabID" ,typeof(Int32));
            Columns.Add("Amount" ,typeof(Decimal));
            Columns.Add("Note" ,typeof(String));
            Columns.Add("IsPaidFree" ,typeof(Int32));
        }
    }
}
