using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class PaymentBrokerDetailModel : PageModel
    {
        public string _PaymentCustomerID { get; set; }
        public string _BrokerID { get; set; }
        public string _CustomerDesti { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var broker = Request.Query["BrokerID"];
            var customerDesti = Request.Query["CustomerDesti"];
            var user = Session.GetUserSession(HttpContext);
            if (cus.ToString() != String.Empty)
            {
                _PaymentCustomerID = cus.ToString();
                if (broker.ToString() != String.Empty) _BrokerID = broker.ToString();
                else _BrokerID = "0";
                if (customerDesti.ToString() != String.Empty) _CustomerDesti = customerDesti.ToString();
                else _CustomerDesti = "0";
            }
            else
            {
                _PaymentCustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadPara("Method");
                    dt.TableName = "Method";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostLoadDataPayment(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_PaymentBroker_LoadNew_v2]" ,CommandType.StoredProcedure
                    ,"@CustomerID" ,SqlDbType.Int ,CustomerID);
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string CustomerID ,string point ,string type ,string CustomerDesti)
        {
            try
            {
                PaymentBrokerDetail DataMain = JsonConvert.DeserializeObject<PaymentBrokerDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(DataMain.DataDetail);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_PaymentBroker_Insert]" ,CommandType.StoredProcedure
                        ,"@BrokerID" ,SqlDbType.NVarChar ,DataMain.BrokerID
                        ,"@Customer_ID" ,SqlDbType.Int ,CustomerID
                        ,"@Amount" ,SqlDbType.Decimal ,DataMain.Amount
                        ,"@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID
                        ,"@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"")
                        ,"@Created_By" ,SqlDbType.Int ,user.sys_userid
                        ,"@Created" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Created)
                        ,"@IsChooseDate" ,SqlDbType.Int ,user.sys_ChooseDateCustomer
                        ,"@Method_ID" ,SqlDbType.Int ,DataMain.MethodID
                        ,"@TransferTypeID" ,SqlDbType.Int ,DataMain.TransferTypeID
                        ,"@PosTypeID" ,SqlDbType.Int ,DataMain.PosTypeID
                        ,"@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail
                        ,"@point" ,SqlDbType.Int ,point != null ? point : 0
                        ,"@type" ,SqlDbType.Int ,type != null ? type : 0
                        ,"@CustomerDesti" ,SqlDbType.Int ,CustomerDesti != null ? CustomerDesti : 0
                        ,"@DataDetail" ,SqlDbType.Structured ,dataDetail.Rows.Count > 0 ? dataDetail : null

                    );
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else
                    {
                        return Content("[]");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class PaymentBrokerDetail
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int BranchID { get; set; }
        public int MethodID { get; set; }
        public int TransferTypeID { get; set; }
        public int PosTypeID { get; set; }
        public int MedthodDetail { get; set; }
        public string Amount { get; set; }
        public int BrokerID { get; set; }
        public string TabID { get; set; }
        public string DataDetail { get; set; }
        public string Created { get; set; }
    }

}
