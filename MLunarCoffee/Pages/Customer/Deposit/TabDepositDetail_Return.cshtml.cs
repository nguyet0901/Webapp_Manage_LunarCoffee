using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Deposit
{
    public class TabDepositDetail_ReturnModel : PageModel
    {
        public string _CustomerDeposit { get; set; }
        public string _CurrentDepositID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            if (cus.ToString() != String.Empty)
            {
                _CustomerDeposit = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _CurrentDepositID = curr.ToString();
                }
                else
                {
                    _CurrentDepositID = "0";
                }
            }
            else
            {
                _CustomerDeposit = null;
                Response.Redirect("/assests/Error/index.html");
            }
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadInitialize(string CustomerID)
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
                    dt.TableName = "dtMethod";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Desposit_Reason_Return" ,CommandType.StoredProcedure);
                    dt.TableName = "dtReasonReturn";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Deposit_DetectMaxDeposit_Return" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
                    dt.TableName = "dtDetectMaxDeposit";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "dtMethodType";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Deposit_LoadDetail" ,CommandType.StoredProcedure ,
                      "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }


        }
        public async Task<IActionResult> OnPostExcuteData(string data ,string CustomerID ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DespositReturn DataMain = JsonConvert.DeserializeObject<DespositReturn>(data);
                DataTable dtCus = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtCus = await connFunc.ExecuteDataTable("YYY_sp_Customer_Deposit_Insert" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@Method" ,SqlDbType.Int ,DataMain.Method ,
                            "@PosType" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PosType) ,
                            "@TransferType" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TransferType) ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@ReasonReturn" ,SqlDbType.Int ,DataMain.ReasonReturn ,
                            "@BranchID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID ,
                            "@AmountDeposit" ,SqlDbType.Decimal ,(0 - DataMain.AmountDeposit) ,
                            "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"").Trim() ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@BankInvoiceCode" ,SqlDbType.NVarChar ,"" ,
                            "@dateCreated" ,SqlDbType.DateTime ,((user.sys_ChooseDateCustomer == 1) ? (Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.dateCreated.ToString())) : (Comon.Comon.GetDateTimeNow()))
                        );
                    }
                }
                else
                {
                    //using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    //{
                    //    dtCus = await connFunc.ExecuteDataTable("YYY_sp_Customer_Deposit_Update", CommandType.StoredProcedure,
                    //        "@CustomerID", SqlDbType.Int, CustomerID,
                    //        "@Method", SqlDbType.Int, DataMain.Method,
                    //        "@ReasonReturn", SqlDbType.Int, DataMain.ReasonReturn,
                    //        "@BranchID", SqlDbType.Int, user.sys_branchID,
                    //        "@AmountDeposit", SqlDbType.Decimal, (0 - DataMain.AmountDeposit),
                    //        "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                    //        "@Modified_By", SqlDbType.Int, user.sys_userid,
                    //        "@CurrentID", SqlDbType.Int, CurrentID
                    //    );
                    //}
                }
                return Content(Comon.DataJson.Datatable(dtCus));

            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public class DespositReturn
        {
            public int ID { get; set; }
            public string Content { get; set; }
            public int BranchID { get; set; }
            public int Method { get; set; }
            public int ReasonReturn { get; set; }
            public decimal AmountDeposit { get; set; }
            public string dateCreated { get; set; }
            public string PosType { get; set; }
            public string TransferType { get; set; }
            public int MedthodDetail { get; set; }
        }
    }
}
