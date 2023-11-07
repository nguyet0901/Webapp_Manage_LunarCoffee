using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment
{
    public class PaymentMedicineDetailModel : PageModel
    {
        public string _PaymentCustomerID { get; set; }
        public string _CustomerMedID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {

            var cus = Request.Query["CustomerID"];
            var medid = Request.Query["MedID"];
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _PaymentCustomerID = cus.ToString() != String.Empty ? cus.ToString() : "0";
            _CustomerMedID = medid.ToString() != String.Empty ? medid.ToString() : "0";

        }


        public async Task<IActionResult> OnPostLoadInitialize(string CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
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
                        dtMethodType = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtMethodType.TableName = "MethodType";
                        return dtMethodType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtPrescription = new DataTable();
                        dtPrescription = await confunc.ExecuteDataTable("YYY_sp_Customer_Medicine_Payment_LoadCombo" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                        dtPrescription.TableName = "Prescription";
                        return dtPrescription;
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
        public async Task<IActionResult> OnPostLoadDataMed(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Medicine_Payment_LoadListDetailV2]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,CurrentID);
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
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataDetail ,string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                string ChooseDate = user.sys_ChooseDateCustomer.ToString();
                PaymentMedicine dtMain = JsonConvert.DeserializeObject<PaymentMedicine>(data);
                DataTable DataDetail = JsonConvert.DeserializeObject<DataTable>(dataDetail);

                DataTable dtDetail = new DataTable();
                dtDetail.Columns.Add("IdDetail" ,typeof(Int32));
                dtDetail.Columns.Add("Price" ,typeof(decimal));
                for (int i = 0; i < DataDetail.Rows.Count; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    dr["IdDetail"] = Convert.ToInt32(DataDetail.Rows[i]["ID"].ToString());
                    dr["Price"] = Convert.ToDecimal(DataDetail.Rows[i]["Price"].ToString());
                    dtDetail.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Medicine_Payment_InsertV2]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@CustomerMedicineID" ,SqlDbType.Int ,dtMain.PrescriptionID ,
                            "@Amount" ,SqlDbType.Int ,dtMain.AmountPayment ,
                            "@PaymentMethodID" ,SqlDbType.Int ,dtMain.MethodID ,
                            "@PosType" ,SqlDbType.Int ,dtMain.MethodPosType ,
                            "@TransferType" ,SqlDbType.Int ,dtMain.MethodTransferType ,
                            "@MedthodDetail" ,SqlDbType.Int ,dtMain.MedthodDetail ,
                            "@BranchID" ,SqlDbType.Int ,dtMain.BranchID != 0 ? dtMain.BranchID : user.sys_branchID ,
                            "@CodeFormat" ,SqlDbType.Int ,Comon.Global.sys_CodePayment ,
                            "@Content" ,SqlDbType.NVarChar ,dtMain.Content.Replace("'" ,"") ,
                            "@BillCode" ,SqlDbType.Int ,dtMain.BillCode != null ? dtMain.BillCode.Replace("'" ,"") : "" ,
                            "@PriceDiscounted" ,SqlDbType.Int ,dtMain.PriceDiscounted ,
                            "@PriceRoot" ,SqlDbType.Int ,dtMain.PriceRoot ,
                            "@DiscountAmount" ,SqlDbType.Int ,dtMain.DiscountAmount ,
                            "@DiscountPercent" ,SqlDbType.Int ,dtMain.DiscountPercent ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Created" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(dtMain.DateCreated) ,
                            "@table_data" ,SqlDbType.Structured ,dtDetail.Rows.Count > 0 ? dtDetail : null
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class PaymentMedicine
        {

            public int PrescriptionID { get; set; }
            public decimal AmountPayment { get; set; }
            public string DateCreated { get; set; }
            public int BranchID { get; set; }
            public int MethodID { get; set; }
            public int MethodPosType { get; set; }
            public int MethodTransferType { get; set; }
            public int MedthodDetail { get; set; }
            public string BankInvoiceCode { get; set; }
            public string Content { get; set; }
            public string BillCode { get; set; }
            public int PriceDiscounted { get; set; } = 0;
            public int PriceRoot { get; set; } = 0;
            public int DiscountAmount { get; set; } = 0;
            public int DiscountPercent { get; set; } = 0;

        }

    }
}
