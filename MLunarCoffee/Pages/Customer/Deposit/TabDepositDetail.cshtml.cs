using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Customer.Deposit
{

    public class TabDepositDetailModel : PageModel
    {
        public string _CustomerDeposit { get; set; }
        public string _CurrentDepositID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public TabDepositDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
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
        public async Task<IActionResult> OnPostLoadInitialize()
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
                    return Content("[]");
                }
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(
            string data ,string CustomerID ,string CurrentID ,string AppToken ,string AppPlatform ,string AppUser)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                Desposit DataMain = JsonConvert.DeserializeObject<Desposit>(data);
                DataTable dtCus = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtCus = await connFunc.ExecuteDataTable("YYY_sp_Customer_Deposit_Insert" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@Method" ,SqlDbType.Int ,DataMain.Method ,
                            "@PosType" ,SqlDbType.Int ,DataMain.PosType ,
                            "@TransferType" ,SqlDbType.Int ,DataMain.TransferType ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@ReasonReturn" ,SqlDbType.Int ,0 ,
                            "@BranchID" ,SqlDbType.Int ,DataMain.BranchID != 0 ? DataMain.BranchID : user.sys_branchID ,
                            "@AmountDeposit" ,SqlDbType.Decimal ,DataMain.AmountDeposit ,
                            "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"").Trim() ,
                            "@BillCode" ,SqlDbType.NVarChar ,DataMain.BillCode != null ? DataMain.BillCode.Replace("'" ,"").Trim() : "" ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@BankInvoiceCode" ,SqlDbType.NVarChar ,DataMain.BankInvoiceCode ,
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
                    //        "@PosType", SqlDbType.Int, DataMain.PosType,
                    //        "@TransferType", SqlDbType.Int, DataMain.TransferType,
                    //        "@BranchID", SqlDbType.Int, user.sys_branchID,
                    //        "@AmountDeposit", SqlDbType.Decimal, DataMain.AmountDeposit,
                    //        "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                    //         "@BillCode", SqlDbType.NVarChar, DataMain.BillCode != null ? DataMain.BillCode.Replace("'", "").Trim() : "",
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
    }
    public class Desposit
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string BillCode { get; set; }

        public string CustomerName { get; set; }
        public int BranchID { get; set; }
        public int Method { get; set; }
        public decimal AmountDeposit { get; set; }
        public int PosType { get; set; }
        public int TransferType { get; set; }
        public int MedthodDetail { get; set; }
        public string BankInvoiceCode { get; set; }
        public string dateCreated { get; set; }
    }
}
