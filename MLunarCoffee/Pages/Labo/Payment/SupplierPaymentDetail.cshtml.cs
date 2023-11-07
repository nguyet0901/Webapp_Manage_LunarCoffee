using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Payment
{
    public class SupplierPaymentDetailModel : PageModel
    {
        public string _PaymentSupplierID { get; set; }
        public string _PaymentBranchID { get; set; }
        public string _SupplierName { get; set; }
        public string _BranchName { get; set; }
        public string _PaymentCurrentID { get; set; }

        public string _dataPayment { get; set; }
        public string _dataPaymentDetail { get; set; }
        public string _dataPaymentMethod { get; set; }
        public string _ChooseDate { get; set; }

        public void OnGet()
        { 
            string Sup = Request.Query["SupplierID"];
            string branchid = Request.Query["BranchID"];
            var user = Session.GetUserSession(HttpContext);
            _ChooseDate = user.sys_ChooseDateCustomer.ToString();

            if (Sup != null)
            {
                _PaymentSupplierID = Sup.ToString();
                _PaymentBranchID = branchid.ToString(); 
                _PaymentCurrentID = "0";
                _dataPayment = null;
            }
            else
            {
                _PaymentSupplierID = null;
                Response.Redirect("/assests/Error/index.html");
            } 
        }
         
         public async Task<IActionResult> OnPostInitialize(string SupplierID, string BranchID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                { 
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Labo_Supplier_Payment_Info]", CommandType.StoredProcedure
                        , "@SupplierID", SqlDbType.Int, Convert.ToInt32(SupplierID)
                        , "@BranchID", SqlDbType.Int, BranchID);
                    dt.TableName = "InfoSupp";
                    ds.Tables.Add(dt);
                }
                DataTable dt1 = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt1 =await connFunc.ExecuteDataTable("[YYY_sp_LoadCombo_Para_Method]", CommandType.StoredProcedure,
                            "@Type", SqlDbType.Int, 2
                        );
                    dt1.TableName = "MethodPayment";
                    ds.Tables.Add(dt1);
                }
                return Content(Comon.DataJson.Dataset(ds)); 
            }
            catch (Exception ex)
            {
                return Content("0");
            } 

        }

         public async Task<IActionResult> OnPostSuppPaymentDetailLoad(string SupplierID, string BranchID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Labo_Supplier_PaymentDetail_LoadNew]", CommandType.StoredProcedure
                        , "@SupplierID", SqlDbType.Int, Convert.ToInt32(SupplierID)
                        , "@BranchID", SqlDbType.Int, BranchID
                      );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

         public async Task<IActionResult> OnPostExcuteData(string data, string dataDetail, string CurrentID, string SupplierID)
        {
            try
            {
                SupplierPaymentDetail DataMain = JsonConvert.DeserializeObject<SupplierPaymentDetail>(data);
                DataTable DataDetailPyament = new DataTable();
                DataDetailPyament = JsonConvert.DeserializeObject<DataTable>(dataDetail);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("[YYY_sp_Labo_Supplier_Payment_Insert]", CommandType.StoredProcedure,
                              "@SupplierID", SqlDbType.Int, SupplierID,
                              "@BranchID", SqlDbType.Int, user.sys_branchID,
                              "@Amount", SqlDbType.Decimal, DataMain.Amount,
                              "@BranchCode", SqlDbType.NVarChar, user.sys_branchCode,
                              "@PaymentMethod_ID", SqlDbType.Int, DataMain.Method_ID,
                              "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", ""),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@ChooseDateCustomer", SqlDbType.Int, user.sys_ChooseDateCustomer.ToString(),
                              "@dateCreated", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.dateCreated.ToString()),
                              "@table_data", SqlDbType.Structured, DataDetailPyament.Rows.Count > 0 ? DataDetailPyament : null
                          );
                    }
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }
    public class SupplierPaymentDetail
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int Method_ID { get; set; }
        public string Amount { get; set; }
        public string dateCreated { get; set; }
    }

}
