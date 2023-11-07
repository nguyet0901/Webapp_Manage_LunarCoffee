using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account
{
    public class InvoiceDetailModel : PageModel
    {
        public string _InvoiceDetailID { get; set; }
        public string _InvoiceType { get; set; }
        public int _branchID { get; set; }
        public string _CurrentEmp { get; set; }

        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {
            var _user = Session.GetUserSession(HttpContext);
            _branchID = _user.sys_branchID;
            _ChooseDateCustomer = _user.sys_ChooseDateCustomer.ToString();
            string curr = Request.Query["CurrentID"];
            string type = Request.Query["Type"];

            _InvoiceDetailID = curr != null ? curr : "0";
            _InvoiceType = type != null ? type : "";
            _CurrentEmp = _user.sys_employeename;
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var _user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_Reason]" ,CommandType.StoredProcedure);
                    dt.TableName = "_DataComboReceiptype";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadPara("Method");
                    dt.TableName = "_DataComboMethod";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "_DataComboMethodType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,_user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1
                    );
                    dt.TableName = "_DataBranch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_EmployeeFull_v2" ,CommandType.StoredProcedure
                        ,"@isAllBranch" ,SqlDbType.Int ,1
                    );
                    dt.TableName = "_DataEmployee";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var _user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Account_LoadDetail]" ,CommandType.StoredProcedure
                            ,"@Current" ,SqlDbType.Int ,Convert.ToInt32(CurrentID ?? "0")
                            ,"@UserID" ,SqlDbType.Int ,_user.sys_userid
                            ,"@EditCustomerPassingDate" ,SqlDbType.Int ,_user.sys_EditCustomerPassingDate);
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
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                AccountDetail DataMain = JsonConvert.DeserializeObject<AccountDetail>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    var x = Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.DateCreated.ToString());
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_Insert]" ,CommandType.StoredProcedure ,

                            "@ReceiptsType" ,SqlDbType.Int ,DataMain.ReceiptsType ,
                            "@sign" ,SqlDbType.NVarChar ,DataMain.Sign ,
                            "@Recipient" ,SqlDbType.NVarChar ,DataMain.Receiver.Replace("'" ,"").Trim() ,
                            "@RecipientEmployeeID" ,SqlDbType.NVarChar ,DataMain.ReceiverEmployeeID ,
                            "@ReceiptsContent" ,SqlDbType.NVarChar ,DataMain.ReceiptsContent.Replace("'" ,"").Trim() ,
                            "@Amount" ,SqlDbType.Decimal ,DataMain.Amount ,
                            "@Method" ,SqlDbType.Int ,DataMain.Method ,
                            "@TransferTypeID" ,SqlDbType.Int ,DataMain.TransferTypeID ,
                            "@PosTypeID" ,SqlDbType.Int ,DataMain.PosTypeID ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID ,
                            "@dateCreated" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.DateCreated.ToString()) ,
                            "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer.ToString() ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid
                       );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_Update]" ,CommandType.StoredProcedure ,
                            "@ReceiptsType" ,SqlDbType.Int ,DataMain.ReceiptsType ,
                            "@Recipient" ,SqlDbType.NVarChar ,DataMain.Receiver.Replace("'" ,"").Trim() ,
                            "@RecipientEmployeeID" ,SqlDbType.NVarChar ,DataMain.ReceiverEmployeeID ,
                            "@sign" ,SqlDbType.NVarChar ,DataMain.Sign ,
                            "@ReceiptsContent" ,SqlDbType.NVarChar ,DataMain.ReceiptsContent.Replace("'" ,"").Trim() ,
                            "@Method" ,SqlDbType.Int ,DataMain.Method ,
                            "@TransferTypeID" ,SqlDbType.Int ,DataMain.TransferTypeID ,
                            "@PosTypeID" ,SqlDbType.Int ,DataMain.PosTypeID ,
                            "@MedthodDetail" ,SqlDbType.Int ,DataMain.MedthodDetail ,
                            "@Branch_ID" ,SqlDbType.Int ,DataMain.BranchID ,
                            "@amount" ,SqlDbType.Decimal ,DataMain.Amount ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID
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
    public class AccountDetail
    {
        public string Amount { get; set; }
        public string Receiver { get; set; }
        public int ReceiverEmployeeID { get; set; }
        public string ReceiptsContent { get; set; }
        public int ReceiptsType { get; set; }
        public int Method { get; set; }
        public int TransferTypeID { get; set; }
        public int PosTypeID { get; set; }
        public int MedthodDetail { get; set; }
        public int BranchID { get; set; }
        public int CreditAccount { get; set; }
        public int DebitAccount { get; set; }
        public string DateCreated { get; set; }
        public string Sign { get; set; }

    }
}
