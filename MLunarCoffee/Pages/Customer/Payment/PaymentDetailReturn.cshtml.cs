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

    public class PaymentDetailReturnModel : PageModel
    {

        public string _ChooseDateCustomer { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _dtPermissionCurrentPage = HttpContext.Request.Path;
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
                        DataTable dtPayment = new DataTable();
                        dtPayment = await confunc.ExecuteDataTable("YYY_sp_Customer_PaymentDetail_Return_LoadNew" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                        dtPayment.TableName = "dataPayment";
                        return dtPayment;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMethod = new DataTable();
                        dtMethod = await confunc.LoadPara("Method");
                        dtMethod.TableName = "dataMethod";
                        return dtMethod;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtReasonReturn = new DataTable();
                        dtReasonReturn = await confunc.ExecuteDataTable("YYY_sp_Customer_Desposit_Reason_Return" ,CommandType.StoredProcedure);
                        dtReasonReturn.TableName = "dtReasonReturn";
                        return dtReasonReturn;
                    }
                }));
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
                        DataTable dtMethodType = new DataTable();
                        dtMethodType = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtMethodType.TableName = "MethodType";
                        return dtMethodType;
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
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataDetail ,string CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PaymentDetailReturn DataMain = JsonConvert.DeserializeObject<PaymentDetailReturn>(data);
                DataTable DataDetailPayment = JsonConvert.DeserializeObject<dtPaymentDetailReturn>(dataDetail);
                DataTable DataDetail = new DataTable();
                DataDetail.Columns.Add("TabID" ,typeof(Int32));
                DataDetail.Columns.Add("Amount" ,typeof(Decimal));
                DataDetail.Columns.Add("IsBlock" ,typeof(Int32));

                DataTable DataValid = new DataTable();
                DataValid.Columns.Add("ID" ,typeof(Int32));
                DataValid.Columns.Add("Amount" ,typeof(Decimal));

                for (int i = 0; i < DataDetailPayment.Rows.Count; i++)
                {
                    DataRow dr = DataDetail.NewRow();
                    dr["TabID"] = Convert.ToInt32(DataDetailPayment.Rows[i]["TabID"]);
                    dr["Amount"] = Convert.ToDecimal(DataDetailPayment.Rows[i]["Amount"]);
                    dr["IsBlock"] = Convert.ToString(DataDetailPayment.Rows[i]["IsBlock"]);
                    DataDetail.Rows.Add(dr);

                    DataRow drva = DataValid.NewRow();
                    drva["ID"] = Convert.ToInt32(DataDetailPayment.Rows[i]["TabID"]);
                    drva["Amount"] = 0 - Convert.ToDecimal(DataDetailPayment.Rows[i]["Amount"]);
                    DataValid.Rows.Add(drva);
                }
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Payment_Return_Insert" ,CommandType.StoredProcedure ,
                          "@Customer_ID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID) ,
                          "@Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Amount) ,
                          "@PaymentMethod_ID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.MethodID) ,
                          "@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID ,
                          "@ReasonReturn" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ReasonReturn) ,
                          "@CodeFormat" ,SqlDbType.NVarChar ,Comon.Global.sys_CodePaymentReturn ,
                          "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"") ,
                          "@Created_By" ,SqlDbType.Int ,Convert.ToInt32(user.sys_userid) ,
                          "@Created" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.dateCreated) ,
                          "@ChooseDateCustomer" ,SqlDbType.DateTime ,user.sys_ChooseDateCustomer ,
                          "@table_data" ,SqlDbType.Structured ,DataDetail.Rows.Count > 0 ? DataDetail : null ,
                           "@valid_data" ,SqlDbType.Structured ,DataValid.Rows.Count > 0 ? DataValid : null ,
                           "@PosType" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PosType) ,
                           "@TransferType" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TransferType) ,
                           "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail
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
    public class PaymentDetailReturn
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int BranchID { get; set; }
        public int MethodID { get; set; }
        public string Amount { get; set; }
        public string ReasonReturn { get; set; }
        public string dateCreated { get; set; }
        public string PosType { get; set; }
        public string TransferType { get; set; }
        public int MedthodDetail { get; set; }
    }
    internal class dtPaymentDetailReturn : DataTable
    {
        public dtPaymentDetailReturn()
        {
            Columns.Add("TabID" ,typeof(Int32));
            Columns.Add("Amount" ,typeof(Decimal));
            Columns.Add("IsBlock" ,typeof(Int32));
        }
    }
}
