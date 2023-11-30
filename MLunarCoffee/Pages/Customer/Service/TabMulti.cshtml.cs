using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Service
{
    public class TabMultiModel : PageModel
    {
        public string sys_EmployeeID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string _MultiTabCustomerID { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _PatientRecordID { get; set; }
        public string _TabCurrentID { get; set; }
        public string _Type { get; set; }
        public string _Plan { get; set; }
        public int sys_isMaxDiscountService { get; set; }
        public int sys_maxPerDiscountService { get; set; }
        public int sys_choosePriceMax { get; set; }
        public decimal sys_maxAmountDiscountService { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public int _UsingConfirmService { get; set; }
        public int _Card_Using_First { get; set; }
        public string sys_roundDiscountAmount { get; set; }
        public string sys_CardManual { get; set; }


        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            sys_CardManual = Comon.Global.sys_CardManual.ToString();

            sys_EmployeeID = user.sys_employeeid.ToString();
            sys_isMaxDiscountService = user.sys_isMaxDiscountService;
            sys_maxPerDiscountService = user.sys_maxPerDiscountService;
            sys_maxAmountDiscountService = user.sys_maxAmountDiscountService;
            _UsingConfirmService = user.sys_UsingConfirmService;
            _Card_Using_First = Comon.Global.sys_Card_Using_First;
            sys_choosePriceMax = Comon.Global.sys_Service_Choose_PriceMax;
            sys_roundDiscountAmount = Comon.Global.sys_roundDiscountAmount;

            var curr = Request.Query["CurrentID"];
            var cus = Request.Query["CustomerID"];
            var type = Request.Query["Type"];
            var plan = Request.Query["Plan"];
            var PatientRecordID = Request.Query["PatientRecordID"];

            if (plan.ToString() != String.Empty)
            {
                _Plan = plan.ToString();
            }
            else
            {
                _Plan = "0";
            }

            if (type.ToString() != String.Empty)
            {
                _Type = type.ToString();
            }
            else
            {
                _Type = "";
            }
            if (cus.ToString() != String.Empty)
            {
                _MultiTabCustomerID = cus.ToString();
                // LoadComboMain();
            }
            else
            {
                _MultiTabCustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
            if (PatientRecordID.ToString() != String.Empty) _PatientRecordID = PatientRecordID;
            else _PatientRecordID = "";
            if (curr.ToString() != String.Empty) _TabCurrentID = curr.ToString();
            else _TabCurrentID = "0";
            _dtPermissionCurrentPage = HttpContext.Request.Path;

        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCombo = new DataTable();
                        dtServiceCombo = await confunc.ExecuteDataTable("MLU_sp_Customer_Tab_Service_Combo_Load" ,CommandType.StoredProcedure);
                        dtServiceCombo.TableName = "ServiceCombo";
                        return dtServiceCombo;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDataServiceCat = new DataTable();
                        dtDataServiceCat = await confunc.ExecuteDataTable("MLU_sp_v2_Service_Type" ,CommandType.StoredProcedure);
                        dtDataServiceCat.TableName = "DataServiceCat";
                        return dtDataServiceCat;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDataTeeth = new DataTable();
                        dtDataTeeth = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_Load_Teeth]" ,CommandType.StoredProcedure);
                        dtDataTeeth.TableName = "DataTeeth";
                        return dtDataTeeth;
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

        public async Task<IActionResult> OnPostLoadComboMain_1(string CustomerID ,string PatientRecordID ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtReasonFree = new DataTable();
                        dtReasonFree = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_ReasonFree" ,CommandType.StoredProcedure);
                        dtReasonFree.TableName = "ReasonFree";
                        return dtReasonFree;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtReasonFree = new DataTable();
                        dtReasonFree = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_ReasonDiscount" ,CommandType.StoredProcedure);
                        dtReasonFree.TableName = "ReasonDiscount";
                        return dtReasonFree;
                    }
                }));
                //combo discount_voucher
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtVoucher = new DataTable();
                        dtVoucher = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_Voucher" ,CommandType.StoredProcedure ,
                            "@Branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID);
                        dtVoucher.TableName = "Voucher";
                        return dtVoucher;
                    }
                }));
                //combo discount
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDiscount = new DataTable();
                        dtDiscount = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_Discount" ,CommandType.StoredProcedure ,
                            "@Branch_ID" ,SqlDbType.Int ,user.sys_branchID);
                        dtDiscount.TableName = "Discount";
                        return dtDiscount;
                    }
                }));
                //COMBO BẢO HIỂM: INSURANCE
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtInsurance = new DataTable();
                        dtInsurance = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_Insurance" ,CommandType.StoredProcedure);
                        dtInsurance.TableName = "Insurance";
                        return dtInsurance;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDiscountTab = new DataTable();
                        dtDiscountTab = await confunc.ExecuteDataTable("MLU_sp_Customer_TabService_LoadComBo_ALL_Discount" ,CommandType.StoredProcedure);
                        dtDiscountTab.TableName = "DiscountTab";
                        return dtDiscountTab;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtPatient = new DataTable();
                        dtPatient = await confunc.ExecuteDataTable("[MLU_sp_Customer_PatientRecord_LoadCbb]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@PatientRecordID" ,SqlDbType.Int ,PatientRecordID);
                        dtPatient.TableName = "Patient";
                        return dtPatient;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCard = new DataTable();
                        dtCard = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_LoadCard]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID);
                        dtCard.TableName = "Card";
                        return dtCard;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroupCus = new DataTable();
                        dtGroupCus = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_LoadComBo_GroupCustomer]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID);
                        dtGroupCus.TableName = "GroupCus";
                        return dtGroupCus;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroupCus = new DataTable();
                        dtGroupCus = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_LoadComBo_MemberCustomer]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID);
                        dtGroupCus.TableName = "GroupMem";
                        return dtGroupCus;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtRule_Teeth = new DataTable();
                        dtRule_Teeth = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_Load_Teeth_Rule]" ,CommandType.StoredProcedure);
                        dtRule_Teeth.TableName = "Rule_Teeth";
                        return dtRule_Teeth;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCustomer_Rule_Teeth = new DataTable();
                        dtCustomer_Rule_Teeth = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_TeethRule_Customer]" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,CustomerID);
                        dtCustomer_Rule_Teeth.TableName = "Customer_Rule_Teeth";
                        return dtCustomer_Rule_Teeth;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtNote_Service = new DataTable();
                        dtNote_Service = await confunc.ExecuteDataTable("[MLU_sp_Customer_Tab_Load_NoteService]" ,CommandType.StoredProcedure);
                        dtNote_Service.TableName = "Note_Service";
                        return dtNote_Service;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceService = new DataTable();
                        dtSourceService = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                        dtSourceService.TableName = "SourceService";
                        return dtSourceService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtStaff = new DataTable();
                        dtStaff = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabMulti_LoadComboTele]" ,CommandType.StoredProcedure);
                        dtStaff.TableName = "Tele";
                        return dtStaff;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Tab_GetAmountPoint]" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,CustomerID);
                        dt.TableName = "AmountPoint";
                        return dt;
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

        public async Task<IActionResult> OnPostExecuteData(string data ,string CustomerID ,string Changing
           ,string Patient
           ,string Plan
           ,string Plan_Name ,string DateBegin ,string DateEnd
           ,string Plan_Note
           ,string ComboID
           ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                DataTable dt = new DataTable();
                dtMain.Columns.Add("Index" ,typeof(Int32));
                dtMain.Columns.Add("ServiceID" ,typeof(Int32));
                dtMain.Columns.Add("Note" ,typeof(String));
                dtMain.Columns.Add("Quantity" ,typeof(Int32));
                dtMain.Columns.Add("discountForCust" ,typeof(Int32));
                dtMain.Columns.Add("DiscountForCus_Amount" ,typeof(decimal));
                dtMain.Columns.Add("Discount_ID" ,typeof(Int32));
                dtMain.Columns.Add("ReasonDiscountID" ,typeof(Int32));
                dtMain.Columns.Add("Price_Unit" ,typeof(decimal));
                dtMain.Columns.Add("Price_Root" ,typeof(decimal));
                dtMain.Columns.Add("Discount_Amount" ,typeof(decimal));
                dtMain.Columns.Add("Discount_Per" ,typeof(Int32));
                dtMain.Columns.Add("Discount_Amount_Doctor" ,typeof(decimal));
                dtMain.Columns.Add("Price_Discounted" ,typeof(decimal));
                dtMain.Columns.Add("current_card_used" ,typeof(decimal));
                dtMain.Columns.Add("data_card_using" ,typeof(String));
                dtMain.Columns.Add("Telesale" ,typeof(Int32));
                dtMain.Columns.Add("Telesale2" ,typeof(Int32));
                dtMain.Columns.Add("Consult1" ,typeof(Int32));
                dtMain.Columns.Add("Consult2" ,typeof(Int32));
                dtMain.Columns.Add("Consult3" ,typeof(Int32));
                dtMain.Columns.Add("Consult4" ,typeof(Int32));
                dtMain.Columns.Add("KTV" ,typeof(Int32));
                dtMain.Columns.Add("teethChoosing" ,typeof(String));
                dtMain.Columns.Add("TeethType" ,typeof(Int32));
                dtMain.Columns.Add("TeethDenture" ,typeof(Int32));

                dtMain.Columns.Add("dateCreated" ,typeof(DateTime));
                dtMain.Columns.Add("Patient" ,typeof(Int32));
                dtMain.Columns.Add("TreatmentPlan" ,typeof(Int32));
                // tr? góp
                dtMain.Columns.Add("IsInstallment" ,typeof(Int32));
                dtMain.Columns.Add("PrepayAmount" ,typeof(decimal));
                dtMain.Columns.Add("DataInstallment" ,typeof(String));
                dtMain.Columns.Add("InstallmentDate" ,typeof(Int32));
                dtMain.Columns.Add("Installment_Term" ,typeof(Int32));
                dtMain.Columns.Add("Installment_Step" ,typeof(Int32));

                // free service
                dtMain.Columns.Add("IsFree" ,typeof(Int32));
                dtMain.Columns.Add("ReasonFree_ID" ,typeof(Int32));

                // bảo hiểm
                dtMain.Columns.Add("IsInsurance" ,typeof(Int32));
                dtMain.Columns.Add("Insurance_ID" ,typeof(Int32));

                // voucher
                dtMain.Columns.Add("Discount_VoucherID" ,typeof(Int32));
                dtMain.Columns.Add("Discount_Voucher_Series" ,typeof(Int32));
                dtMain.Columns.Add("Discount_Voucher" ,typeof(decimal));
                dtMain.Columns.Add("Discount_Voucher_Per" ,typeof(Int32));

                // discount by customer group
                dtMain.Columns.Add("Discount_CusGroup_ID" ,typeof(Int32));
                dtMain.Columns.Add("Discount_CusGroup_Percent" ,typeof(Int32));
                dtMain.Columns.Add("Discount_CusGroup_Amount" ,typeof(decimal));

                // discount by card
                dtMain.Columns.Add("Discount_Amount_Card" ,typeof(decimal));

                // discount by customer mem
                dtMain.Columns.Add("Discount_CusMem_ID" ,typeof(Int32));
                dtMain.Columns.Add("Discount_CusMem_Percent" ,typeof(Int32));
                dtMain.Columns.Add("Discount_CusMem_Amount" ,typeof(decimal));

                dtMain.Columns.Add("IsChoose" ,typeof(Int32));
                dtMain.Columns.Add("ChooseUser" ,typeof(Int32));
                dtMain.Columns.Add("ChooseDate" ,typeof(DateTime));
                dtMain.Columns.Add("Min_Payment" ,typeof(decimal));
                dtMain.Columns.Add("Price_Branch_Flag" ,typeof(String));
                dtMain.Columns.Add("Price_Branch_Exchange" ,typeof(decimal));
                dtMain.Columns.Add("PackageNumber" ,typeof(String));
                dtMain.Columns.Add("State" ,typeof(Int32));
                dtMain.Columns.Add("SourceService" ,typeof(Int32));
                dtMain.Columns.Add("PointUse" ,typeof(Int32));
                dtMain.Columns.Add("Discount_Point" ,typeof(decimal));
                dtMain.Columns.Add("TimeToTreat" ,typeof(Int32));

                dtMain.Columns.Add("Vatper" ,typeof(Int32));
                dtMain.Columns.Add("Vatamount" ,typeof(decimal));


                DataTable dtCard = new DataTable();
                dtCard.Columns.Add("Index" ,typeof(Int32));
                dtCard.Columns.Add("CardID" ,typeof(Int32));
                dtCard.Columns.Add("Amount" ,typeof(decimal));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Index"] = i + 1;
                    dr["ServiceID"] = Convert.ToInt32(DataMain.Rows[i]["ServiceID"]);
                    dr["Note"] = DataMain.Rows[i]["Note"];
                    dr["Quantity"] = Convert.ToInt32(DataMain.Rows[i]["Quantity"]);
                    dr["discountForCust"] = DataMain.Rows[i]["discountForCust"];
                    dr["DiscountForCus_Amount"] = Convert.ToDecimal(DataMain.Rows[i]["DiscountForCus_Amount"]);
                    dr["Discount_ID"] = Convert.ToInt32(DataMain.Rows[i]["Discount_ID"]);
                    dr["ReasonDiscountID"] = Convert.ToInt32(DataMain.Rows[i]["ReasonDiscountID"]);
                    dr["Price_Unit"] = Convert.ToDecimal(DataMain.Rows[i]["Price_Unit"]);
                    dr["Price_Root"] = Convert.ToDecimal(DataMain.Rows[i]["Price_Root"]);
                    dr["Discount_Amount"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_Amount"]);
                    dr["Discount_Per"] = Convert.ToInt32(DataMain.Rows[i]["Discount_Per"]);
                    dr["Discount_Amount_Doctor"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_Amount_Doctor"]);
                    dr["Price_Discounted"] = Convert.ToDecimal(DataMain.Rows[i]["Price_Discounted"]);
                    dr["current_card_used"] = Convert.ToDecimal(DataMain.Rows[i]["current_card_used"]);
                    dr["data_card_using"] = DataMain.Rows[i]["data_card_using"];
                    if (dr["data_card_using"].ToString().Trim() != "" && dr["data_card_using"].ToString().Trim() != "[]")
                    {
                        DataTable item_card = JsonConvert.DeserializeObject<DataTable>(dr["data_card_using"].ToString());
                        for (int j = 0; j < item_card.Rows.Count; j++)
                        {
                            DataRow drcard = dtCard.NewRow();
                            drcard["Index"] = i + 1;
                            drcard["CardID"] = Convert.ToInt32(item_card.Rows[j]["CardID"]);
                            drcard["Amount"] = Convert.ToDecimal(item_card.Rows[j]["Amount"]);
                            dtCard.Rows.Add(drcard);
                        }
                    }
                    //string ddd = DataMain.Rows[i]["data_card_using"].ToString();
                    dr["Telesale"] = DataMain.Columns.Contains("Telesale") ? Convert.ToInt32(DataMain.Rows[i]["Telesale"]) : 0;
                    dr["Telesale2"] = DataMain.Columns.Contains("Telesale2") ? Convert.ToInt32(DataMain.Rows[i]["Telesale2"]) : 0;
                    dr["Consult1"] = Convert.ToInt32(DataMain.Rows[i]["Consult1"]);
                    dr["Consult2"] = Convert.ToInt32(DataMain.Rows[i]["Consult2"]);
                    dr["Consult3"] = Convert.ToInt32(DataMain.Rows[i]["Consult3"]);
                    dr["Consult4"] = Convert.ToInt32(DataMain.Rows[i]["Consult4"]);
                    dr["KTV"] = Convert.ToInt32(DataMain.Rows[i]["KTV"]);
                    dr["teethChoosing"] = DataMain.Rows[i]["teethChoosing"];
                    dr["TeethType"] = Convert.ToInt32(DataMain.Rows[i]["TeethType"]);
                    dr["TeethDenture"] = Convert.ToInt32(DataMain.Rows[i]["TeethDenture"]);
                    dr["dateCreated"] = Convert.ToDateTime(Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[i]["dateCreated"].ToString()));
                    dr["Patient"] = Convert.ToInt32(DataMain.Rows[i]["Patient"]);
                    dr["TreatmentPlan"] = Convert.ToInt32(Plan);
                    dr["IsInstallment"] = Convert.ToInt32(DataMain.Rows[i]["IsInstallment"]);
                    dr["PrepayAmount"] = Convert.ToDecimal(DataMain.Rows[i]["InstallmentFirPaid"]);
                    dr["DataInstallment"] = DataMain.Rows[i]["jsonInstallment"];
                    dr["InstallmentDate"] = Convert.ToInt32(DataMain.Rows[i]["InstallmentDay"]);
                    dr["Installment_Term"] = Convert.ToInt32(DataMain.Rows[i]["Installment_Term"]);
                    dr["Installment_Step"] = 0;
                    dr["IsFree"] = Convert.ToInt32(DataMain.Rows[i]["IsFree"]);
                    dr["ReasonFree_ID"] = Convert.ToInt32(DataMain.Rows[i]["ReasonFree_ID"]);
                    dr["IsInsurance"] = Convert.ToInt32(DataMain.Rows[i]["IsInsurance"]);
                    dr["Insurance_ID"] = Convert.ToInt32(DataMain.Rows[i]["Insurance_ID"]);
                    dr["Discount_VoucherID"] = Convert.ToInt32(DataMain.Rows[i]["Discount_VoucherID"]);
                    dr["Discount_Voucher_Series"] = Convert.ToInt32(DataMain.Rows[i]["Discount_Voucher_Series"]);
                    dr["Discount_Voucher"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_Voucher"]);
                    dr["Discount_Voucher_Per"] = Convert.ToInt32(DataMain.Rows[i]["Discount_Voucher_Per"]);
                    dr["Discount_CusGroup_ID"] = Convert.ToInt32(DataMain.Rows[i]["Discount_CusGroup_ID"]);
                    dr["Discount_CusGroup_Percent"] = Convert.ToInt32(DataMain.Rows[i]["Discount_CusGroup_Percent"]);
                    dr["Discount_CusGroup_Amount"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_CusGroup_Amount"]);
                    dr["Discount_Amount_Card"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_Amount_Card"]);
                    dr["Discount_CusMem_ID"] = Convert.ToInt32(DataMain.Rows[i]["Discount_CusMem_ID"]);
                    dr["Discount_CusMem_Percent"] = Convert.ToInt32(DataMain.Rows[i]["Discount_CusMem_Percent"]);
                    dr["Discount_CusMem_Amount"] = Convert.ToDecimal(DataMain.Rows[i]["Discount_CusMem_Amount"]);

                    dr["IsChoose"] = Convert.ToInt32(DataMain.Rows[i]["IsChoose"]);
                    dr["ChooseUser"] = (Convert.ToInt32(DataMain.Rows[i]["IsChoose"]) == 1) ? user.sys_userid : 0;
                    dr["Min_Payment"] = Convert.ToDecimal(DataMain.Rows[i]["Min_Payment"]);
                    dr["Price_Branch_Flag"] = DataMain.Rows[i]["Price_Branch_Flag"];
                    dr["Price_Branch_Exchange"] = Convert.ToDecimal(DataMain.Rows[i]["Price_Branch_Exchange"]);
                    dr["PackageNumber"] = "";
                    dr["ChooseDate"] = Comon.Comon.GetDateTimeNow();
                    dr["State"] = (Convert.ToInt32(DataMain.Rows[i]["IsChoose"]) == 1) ? 1 : 0;
                    dr["SourceService"] = Convert.ToInt32(DataMain.Rows[i]["SourceService"]);
                    dr["PointUse"] = Convert.ToInt32(DataMain.Rows[i]["PointUse"]);
                    dr["Discount_Point"] = Convert.ToDecimal(DataMain.Rows[i]["DiscountPointUsed"]);
                    dr["TimeToTreat"] = Convert.ToInt32(DataMain.Rows[i]["TimeToTreat"]);
                    dr["Vatper"] = Convert.ToInt32(DataMain.Rows[i]["Vat_per"]);
                    dr["Vatamount"] = Convert.ToDecimal(DataMain.Rows[i]["Vat_amount"]);
 
                    dtMain.Rows.Add(dr);
                }

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_TabMulti_Insert" ,CommandType.StoredProcedure ,
                              "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                              "@Patient" ,SqlDbType.Int ,Patient ,
                              "@Plan" ,SqlDbType.Int ,Plan ,
                              "@IsUsingPlan" ,SqlDbType.Int ,Comon.Global.sys_Treatment_Plan ,
                              "@Plan_Note" ,SqlDbType.NVarChar ,(Plan_Note != null) ? Plan_Note.Replace("'" ,"") : "" ,
                              "@Plan_Name" ,SqlDbType.NVarChar ,(Plan_Name != null) ? Plan_Name.Replace("'" ,"") : "" ,
                              "@DateBegin" ,SqlDbType.NVarChar ,(DateBegin != null) ? DateBegin : "" ,
                              "@DateEnd" ,SqlDbType.NVarChar ,(DateEnd != null) ? DateEnd : "" ,
                              "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                              "@Branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                              "@ComboID" ,SqlDbType.Int , ComboID ,
                              "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer.ToString() ,
                              "@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0) ? dtMain : null ,
                              "@table_card" ,SqlDbType.Structured ,(dtCard.Rows.Count > 0) ? dtCard : null);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_TabMulti_Update" ,CommandType.StoredProcedure ,
                             "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                             "@Changing" ,SqlDbType.Int ,Convert.ToInt32(Changing) ,
                             "@IsChoose" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Rows[0]["IsChoose"]) ,
                             "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID) ,
                             "@ServiceID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["ServiceID"]) ,
                             "@PackageNumber" ,SqlDbType.NVarChar ,"" ,
                             "@Note" ,SqlDbType.NVarChar ,dtMain.Rows[0]["Note"] ,
                             "@Quantity" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Quantity"]) ,
                             "@discountForCust" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["discountForCust"]) ,
                             "@DiscountForCus_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["DiscountForCus_Amount"]) ,
                             "@Discount_ID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_ID"]) ,
                             "@ReasonDiscountID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["ReasonDiscountID"]) ,
                             "@Price_Root" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Price_Root"]) ,
                             "@Price_Unit" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Price_Unit"]) ,
                             "@Discount_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Amount"]) ,
                             "@Discount_Per" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Per"]) ,
                             "@Discount_Amount_Doctor" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Amount_Doctor"]) ,
                             "@Price_Discounted" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Price_Discounted"]) ,
                             "@final_CardUsing" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["current_card_used"]) ,
                             "@data_card_using" ,SqlDbType.NVarChar ,dtMain.Rows[0]["data_card_using"] ,
                             "@Telesale" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Telesale"]) ,
                             "@Telesale2" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Telesale2"]) ,
                             "@Consult1" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Consult1"]) ,
                             "@Consult2" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Consult2"]) ,
                             "@Consult3" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Consult3"]) ,
                             "@Consult4" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Consult4"]) ,
                             "@KTV" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["KTV"]) ,
                             "@teethChoosing" ,SqlDbType.NVarChar ,dtMain.Rows[0]["teethChoosing"] ,
                             "@TeethType" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["TeethType"]) ,
                             "@TeethDenture" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["TeethDenture"]) ,
                             "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                             "@Patient" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Patient"]) ,
                             "@IsFree" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["IsFree"]) ,
                             "@ReasonFree_ID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["ReasonFree_ID"]) ,
                             "@IsInstallment" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["IsInstallment"]) ,
                             "@PrepayAmount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["PrepayAmount"]) ,
                             "@DataInstallment" ,SqlDbType.NVarChar ,dtMain.Rows[0]["DataInstallment"] ,
                             "@InstallmentDate" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["InstallmentDate"]) ,
                             "@Installment_Term" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Installment_Term"]) ,
                             //"@Installment_Step", SqlDbType.Int, Convert.ToInt32(dtMain.Rows[0]["Installment_Step"]),
                             "@IsInsurance" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["IsInsurance"]) ,
                             "@Insurance_ID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Insurance_ID"]) ,
                             "@Discount_VoucherID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_VoucherID"]) ,
                             "@Discount_Voucher_Series" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_Voucher_Series"]) ,
                             "@Discount_Voucher" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Voucher"]) ,
                             "@Discount_Voucher_Per" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_Voucher_Per"]) ,
                             "@Discount_CusGroup_ID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_CusGroup_ID"]) ,
                             "@Discount_CusGroup_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Discount_CusGroup_Amount"]) ,
                             "@Discount_Amount_Card" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Amount_Card"]) ,
                             "@Discount_CusGroup_Percent" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_CusGroup_Percent"]) ,
                             "@Discount_CusMem_ID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Discount_CusMem_ID"]) ,
                             "@Discount_CusMem_Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Discount_CusMem_Amount"]) ,
                             "@Discount_CusMem_Percent" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_CusMem_Percent"]) ,
                             "@Min_Payment" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Min_Payment"]) ,
                             "@Price_Branch_Flag" ,SqlDbType.NVarChar ,dtMain.Rows[0]["Price_Branch_Flag"] ,
                             "@Price_Branch_Exchange" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Price_Branch_Exchange"]) ,
                             "@table_card" ,SqlDbType.Structured ,(dtCard.Rows.Count > 0) ? dtCard : null ,
                             "@SourceService" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["SourceService"]) ,
                             "@PointUse" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["PointUse"]) ,
                             "@TimeToTreat" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["TimeToTreat"]) ,
                             "@Vatper" ,SqlDbType.Int ,Convert.ToInt32(dtMain.Rows[0]["Vatper"]) ,
                             "@Vatamount" ,SqlDbType.Decimal ,Convert.ToDecimal(dtMain.Rows[0]["Vatamount"]) ,
                             "@Discount_Point" ,SqlDbType.Int ,Convert.ToDecimal(dtMain.Rows[0]["Discount_Point"]) ,
                             "@Dencos" ,SqlDbType.Int ,Convert.ToInt32(Global.sys_DentistCosmetic)
                         );
                    }
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostLoadDataDetail(int currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_TabService_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@Current_ID" ,SqlDbType.Int ,currentid);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExecuting_Patient_Record(string TemplateID ,string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PatientRecord_Insert]" ,CommandType.StoredProcedure ,
                        "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                        "@TemplateID" ,SqlDbType.Int ,TemplateID ,
                        "@StatusTeethID" ,SqlDbType.Int ,0 ,
                        "@branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid

                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostChecked_Patient_Record_Template(string TokentService)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Customer_Tab_Service_Check_Patient" ,CommandType.StoredProcedure ,
                         "@TokentServiceID" ,SqlDbType.NVarChar ,TokentService);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
