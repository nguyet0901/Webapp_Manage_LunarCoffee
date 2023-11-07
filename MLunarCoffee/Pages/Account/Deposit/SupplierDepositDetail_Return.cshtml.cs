using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Account.Deposit
{
    public class SupplierDepositDetail_ReturnModel : PageModel
    {
        public string SubID { get; set; }
        public void OnGet()
        {
            SubID = Request.Query["SupID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadInitialize(string SupplierID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
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
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Supplier_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Desposit_Reason_Return", CommandType.StoredProcedure);
                    dt.TableName = "dtReasonReturn";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Supplier_MaxDeposit_Return", CommandType.StoredProcedure
                        , "@SupplierID", SqlDbType.Int, Convert.ToInt32(SupplierID)
                        );
                    dt.TableName = "AmountSupplierDeposit";
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
                DespositReturn DataMain = JsonConvert.DeserializeObject<DespositReturn>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Supplier_Deposit_Insert", CommandType.StoredProcedure
                        , "@SupplierID", SqlDbType.Int, Convert.ToInt32(DataMain.SupplierID)
                        , "@BranchID", SqlDbType.Int, Convert.ToInt32(DataMain.BranchID)
                        , "@PaymentMethodID", SqlDbType.Int, Convert.ToInt32(DataMain.PaymentMethodID)
                        , "@Amount", SqlDbType.Decimal, (0 - DataMain.Amount)
                        , "@Note", SqlDbType.NVarChar, DataMain.Note
                        , "@Reason_Return", SqlDbType.Int, DataMain.Reason_Return
                        , "@DateCreated", SqlDbType.DateTime, ((user.sys_ChooseDateCustomer == 1) ? (Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Created.ToString())) : (Comon.Comon.GetDateTimeNow()))
                        , "@Created_By", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class DespositReturn
        {
            public int SupplierID { get; set; }
            public int BranchID { get; set; }
            public int PaymentMethodID { get; set; }
            public decimal Amount { get; set; }
            public string Note { get; set; }
            public int Reason_Return { get; set; }
            public string Created { get; set; }
        }
    }
}
