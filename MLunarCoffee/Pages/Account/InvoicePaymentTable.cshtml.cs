using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account
{
    public class InvoicePaymentTableModel : PageModel
    {
        public string _BranchID { get; set; }
        public string _BranchToken { get; set; }
        public string _CashierID { get; set; }
        public string _DateFrom { get; set; }
        public string _DateTo { get; set; }
        public string _CurrentPath { get; set; }
        public void OnGet()
        {
            string BranchID = Request.Query["BranchID"];
            string BranchToken = Request.Query["BranchToken"];
            string CashierID = Request.Query["CashierID"];
            string DateFrom = Request.Query["DateFrom"];
            string DateTo = Request.Query["DateTo"];
            var user = Session.GetUserSession(HttpContext);
            DateTime DataNow = Comon.Comon.GetDateTimeNow();

            _BranchID = BranchID != null ? BranchID : user.sys_branchID.ToString();
            _BranchToken = BranchToken != null ? BranchToken : "";
            _CashierID = CashierID != null ? CashierID : "0";
            _DateFrom = DateFrom != null ? DateFrom.Replace("x" ," ") : DataNow.ToString("dd-MM-yyyy") + " 00:00:00";
            _DateTo = DateTo != null ? DateTo.Replace("x" ," ") : DataNow.ToString("dd-MM-yyyy") + " 23:59:59";
            _CurrentPath = HttpContext.Request.Path != null ? HttpContext.Request.Path : "";
        }

        public async Task<IActionResult> OnPostInitialize()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_LoadCombo_Para_All]" ,CommandType.StoredProcedure
                 ,"@paraTypeName" ,SqlDbType.NVarChar ,"Method");
                dt.TableName = "Method";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                dt.TableName = "MethodType";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Account_Reason]" ,CommandType.StoredProcedure);
                dt.TableName = "AccountReason";
                ds.Tables.Add(dt);
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

        public async Task<IActionResult> OnPostLoadataReceipt(string dateTo ,string dateFrom ,int branchid ,string branchtoken ,int cashierid ,int limit ,int currentid ,int type)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>
                {
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[YYY_sp_Accountant_ReceiptPaymentLoad]" ,CommandType.StoredProcedure ,
                                "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateFrom)
                              ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateTo)
                              ,"@branchID" ,SqlDbType.Int ,branchid
                              ,"@branchToken" ,SqlDbType.NVarChar ,branchtoken
                              ,"@cashierID" ,SqlDbType.Int ,cashierid
                              ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                              ,"@Limit" ,SqlDbType.Int ,limit
                              ,"@CurrentID" ,SqlDbType.Int ,currentid
                              ,"@type" ,SqlDbType.Int ,type
                              ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
                            dt.TableName = "Master";
                            return dt;
                        }
                    }) ,
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[YYY_sp_Accountant_ReceiptPaymentLoadDetail]" ,CommandType.StoredProcedure ,
                               "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateFrom)
                              ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateTo)
                              ,"@branchID" ,SqlDbType.Int ,branchid
                              ,"@branchToken" ,SqlDbType.NVarChar ,branchtoken
                              ,"@cashierID" ,SqlDbType.Int ,cashierid);
                            dt.TableName = "Detail";
                            return dt;
                        }
                    })
                };
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostAccountantCensorship(string CurentID ,string TypeCensorship ,string Value)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Accountant_Censorship]" ,CommandType.StoredProcedure
                         ,"@CurentID" ,SqlDbType.Int ,CurentID
                         ,"@TypeCensorship" ,SqlDbType.Int ,TypeCensorship
                         ,"@Value" ,SqlDbType.Int ,Value
                         ,"@Check_By" ,SqlDbType.Int ,user.sys_userid
                         );
                }
                if (dt != null && dt.Rows.Count > 0)
                    return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostDelete(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_Delete]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,ID ,
                          "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                          "@userID" ,SqlDbType.Int ,user.sys_userid ,
                          "@BranchID" ,SqlDbType.Int ,user.sys_branchID
                      );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostUpdateSign(int ID ,string Sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_Update_Sign]" ,CommandType.StoredProcedure ,
                         "@CurrentID" ,SqlDbType.Int ,ID ,
                         "@userID" ,SqlDbType.Int ,user.sys_userid ,
                         "@Sign" ,SqlDbType.NVarChar ,Sign
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
