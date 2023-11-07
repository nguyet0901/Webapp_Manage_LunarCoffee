using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Comon.Crypt;

namespace MLunarCoffee.Pages.Customer
{
    public class MainCustomer : PageModel
    {

        public string _MainCustomerID { get; set; }
        public int _BranchID { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string sys_isDetailTeeth { get; set; }
        public string sys_AskHistory { get; set; }
        public string sys_AskCustGroup { get; set; }
        public string sys_ValidProfile { get; set; }
        public string sys_Before_16 { get; set; }
        public string sys_TimeInvoice { get; set; }
        public string sys_InputTeethChart { get; set; }
        public string Debt_By_Treatment { get; set; }
        public string sys_MoneyToPoint { get; set; }
        public string sys_PercentTreatmentShow { get; set; }
        public string sys_TreatManualamount { get; set; }
        public string sys_TreatManualIndex { get; set; }
        public string sys_Noruletreatindex { get; set; }
        public string sys_DepositNonPriority { get; set; }
        public string sys_AutoselectPro { get; set; }

        
        public string layout { get; set; }
        public int sys_IsMobileApp { get; set; }
        public int Resizeimageupload { get; set; }
        public int LimitSizeUploadInDay { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public MainCustomer(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                //string Minify = Session.GetSession(HttpContext.Session ,"Minify");

                OptionExtension optionExtension = user.sys_Rule_OptionExtension;
                Debt_By_Treatment = Global.sys_Debt_By_Treatment != null ? Global.sys_Debt_By_Treatment.ToString() : "0";
                sys_MoneyToPoint = Global.sys_MoneyToPoint != null ? Global.sys_MoneyToPoint.ToString() : "0";
                sys_PercentTreatmentShow = Global.sys_PercentTreatmentShow != null ? Global.sys_PercentTreatmentShow.ToString() : "0";
                sys_DepositNonPriority = Global.sys_DepositNonPriority != null ? Global.sys_DepositNonPriority.ToString() : "0";
                LimitSizeUploadInDay = Comon.Global.sys_LimitSizeUploadInDay;
                Resizeimageupload = Comon.Global.sys_IsOutside == 1 ? 0 : 1;

                sys_AskHistory = optionExtension.PreHistory != null ? optionExtension.PreHistory.Value.ToString() : "0";
                sys_AskCustGroup = optionExtension.CustomerGroup != null ? optionExtension.CustomerGroup.Value.ToString() : "0";
                sys_ValidProfile = optionExtension.ValidProfile != null ? optionExtension.ValidProfile.Value.ToString() : "0";
                sys_Before_16 = optionExtension.Before_16 != null ? optionExtension.Before_16.Value.ToString() : "0";
                sys_TimeInvoice = optionExtension.TimeInvoice != null ? optionExtension.TimeInvoice.Value.ToString() : "0";
                sys_InputTeethChart = optionExtension.InputTeethChart != null ? optionExtension.InputTeethChart.Value.ToString() : "0";
                sys_IsMobileApp = Global.sys_APIClient.IsMobileApp;
                sys_TreatManualamount = Comon.Global.sys_TreatManualamount.ToString();
                sys_TreatManualIndex = Comon.Global.sys_TreatManualIndex.ToString();
                sys_AutoselectPro = Comon.Global.sys_AutoselectPro.ToString();
 
                sys_Noruletreatindex = Comon.Global.sys_Noruletreatindex.ToString();
                var v = Request.Query["CustomerID"].ToString();
                v = int.TryParse(v, out int n) ? v : Encrypt.DecryptString(v ,Settings.LittleSecret);
                if (v != "")
                {
                    _MainCustomerID = v;
                    _BranchID = ((GlobalUser)Session.GetUserSession(HttpContext)).sys_branchID;
                    _dtPermissionCurrentPage = HttpContext.Request.Path;
                    sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
                    sys_isDetailTeeth = Comon.Global.sys_isDetailTeeth.ToString();
                }
                else
                {
                    _MainCustomerID = "0";
                    Response.Redirect("/assests/Error/index.html");
                }

                string _layout = Request.Query["layout"];
                layout = !string.IsNullOrEmpty(_layout) ? _layout : "";
            }
            catch (Exception ex)
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int CustomerID ,int UserTeleLevel ,int UserTeleGroup)
        {
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_LoadInfo]" ,CommandType.StoredProcedure ,
                      "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID == 0 ? 0 : CustomerID)
                      ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_branchID));
                    dt.TableName = "Info";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtMember = new DataTable();
                    dtMember = await confunc.ExecuteDataTable("[YYY_sp_MemberLoad]" ,CommandType.StoredProcedure);
                    dtMember.TableName = "Member";
                    ds.Tables.Add(dtMember);
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    int BranchID = Convert.ToInt32(dt.Rows[0]["BranchID"]); // Chi nhanh của khách hàng
                    int TeleID = Convert.ToInt32(dt.Rows[0]["TeleUserID"]); // Tele đang giữ ticket
                    int TeleGroupID = Convert.ToInt32(dt.Rows[0]["TeleGroupID"]); // Nhóm của tele này
                    int TotalAnam = Convert.ToInt32(dt.Rows[0]["TotalAnam"]); // Tổng Tiền Sử
                    int GroupID = Convert.ToInt32(dt.Rows[0]["GroupID"]); // Nhóm Khách hàng
                    int IsValid = Convert.ToInt32(dt.Rows[0]["IsValid"]); // Valid ho so nay chua
                    int isAllowTele = 1;
                    int isAllowBranch = 1;
                    if (Global.sys_Permission_Tele == 1)
                    {
                        if (UserTeleLevel == 2 && UserTeleGroup != TeleGroupID) isAllowTele = 0;
                        if (UserTeleLevel == 1 && user.sys_userid != TeleID) isAllowTele = 0;
                    }
                    if (Global.sys_CustomerNotViewByBranch == 1)
                    {
                        if (user.sys_AllBranchID == 0 && !("," + user.sys_AreaBranch + ",").Contains("," + BranchID.ToString() + ","))
                            isAllowBranch = 0;
                    }
                    if (isAllowTele == 1 && isAllowBranch == 1)
                    {
                        //if (user.sys_Rule_PreHistory == "1" && TotalAnam == 0)
                        //    dt.Rows[0]["IsNotiAnam"] = "ServiceIsLock";
                        //if (user.sys_Rule_CustomerGroup == "1" && GroupID == 0)
                        //    dt.Rows[0]["IsNotiGroup"] = "ServiceIsLock_EnterCustGroup";
                        //if (user.sys_Rule_ValidProfile == "1" && IsValid == 0)
                        //    dt.Rows[0]["IsValidProfile"] = "ServiceIsLock_ValidProfile";
                        return Content(Comon.DataJson.Dataset(ds));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadPaymentInfo(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_LoadPaymentInfo" ,CommandType.StoredProcedure
                       ,"@CurrentID" ,SqlDbType.Int ,CustomerID
                       ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(Comon.Global.sys_DentistCosmetic)
                       );
                }

                return Content(Comon.DataJson.Datatable(dt));
            }

            catch (Exception ex)
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostLoadCardinfo(string CustomerID ,string cardid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_LoadCardInfo" ,CommandType.StoredProcedure
                       ,"@CustomerID" ,SqlDbType.Int ,CustomerID
                       ,"@CardID" ,SqlDbType.Int ,cardid
                       );
                }

                return Content(Comon.DataJson.Datatable(dt));
            }

            catch (Exception ex)
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostCheckCustCode(string CustCode)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Check_CustCode]"
                       ,CommandType.StoredProcedure ,"@CustCode" ,SqlDbType.NVarChar ,CustCode);
                }
                if (dt != null & dt.Rows.Count > 0)
                    return Content(dt.Rows[0]["CustID"].ToString());
                return Content("0");
            }

            catch (Exception ex)
            {
                return Content("0");
            }

        }



        //public async Task<IActionResult> OnPostLoadIsCheckLockService(int CustomerID)
        //{
        //    try
        //    {
        //        int result = 0;

        //        if (!await Comon.Option_Extension.Extension.Check_Rule_PreHistory_FirstTime_Allowed(Convert.ToInt32(CustomerID)))
        //            result = 1;
        //        else result = 0;

        //        return Content(result.ToString());
        //    }

        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }

        //}

        //public async Task<IActionResult> OnPostLoadIsCheckLockServiceEnterCustGroup(int CustomerID)
        //{
        //    try
        //    {
        //        int result = 0;

        //        if (!await Comon.Option_Extension.Extension.Check_Rule_Customer_Group(Convert.ToInt32(CustomerID)))
        //            result = 1;
        //        else result = 0;

        //        return Content(result.ToString());
        //    }

        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }

        //}


        #region //Appointment Customer

        public async Task<IActionResult> OnPostLoadStatusExtra(string appID)
        {
            try
            {
                DataSet ds = new DataSet();
                if (appID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("YYY_sp_Customer_Appointment_LoadStatus_Extra" ,CommandType.StoredProcedure ,
                            "@appID" ,SqlDbType.Int ,appID
                        );
                    }
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadCustomerScheduleNext(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Schedule_Next_Customer]"
                       ,CommandType.StoredProcedure ,"@CustomerID" ,SqlDbType.Int ,CustomerID);
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }        
        #endregion

    }
}
