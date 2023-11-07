using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_Medicine_DetailModel : PageModel
    {
        public string _Prescription_CustomerID { get; set; }
        public string _Prescription_CurrentID { get; set; }
        public string _Type { get; set; }
        public string _PatientRecordID { get; set; }
        public int _FirstPrescriptionMedicine { get; set; }
        public int sysArisePresAfterExport { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            var type = Request.Query["Type"];
            var PatientRecordID = Request.Query["PatientRecordID"];
            _Type = type.ToString() != String.Empty ? type.ToString() : "";
            _PatientRecordID = PatientRecordID.ToString() != String.Empty ? PatientRecordID : "0";
            sysArisePresAfterExport = Comon.Global.sys_ArisePresAfterExport != null ? Comon.Global.sys_ArisePresAfterExport : 0;
            if (cus.ToString() != String.Empty && curr.ToString() != String.Empty)
            {
                _Prescription_CustomerID = cus.ToString();
                _Prescription_CurrentID = curr.ToString();
                _FirstPrescriptionMedicine = 0;
            }
            else if (cus.ToString() != String.Empty)
            {
                _Prescription_CustomerID = cus.ToString();
                _Prescription_CurrentID = "0";
                _FirstPrescriptionMedicine = 1;
            }
        }

        public async Task<IActionResult> OnPostLoadComboMain(string ICDArea)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Prescription_Medicine_Combo]" ,CommandType.StoredProcedure);
                    dt.TableName = "PrescriptionMedicine";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtSourceService = new DataTable();
                    dtSourceService = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                    dtSourceService.TableName = "SourceService";
                    ds.Tables.Add(dtSourceService);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtWare = new DataTable();
                    dtWare = await confunc.ExecuteDataTable("[YYY_sp_Medicine_CheckWareExport]" ,CommandType.StoredProcedure ,
                        "@BranchID" ,SqlDbType.Int ,user.sys_branchID);
                    dtWare.TableName = "Ware";
                    ds.Tables.Add(dtWare);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dticd = new DataTable();
                    dticd = await confunc.ExecuteDataTable("[YYY_ICDLoadCombo]" ,CommandType.StoredProcedure ,
                        "@Area" ,SqlDbType.Int ,ICDArea);
                    dticd.TableName = "ICD";
                    ds.Tables.Add(dticd);
                    
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataSet dsMedicine = new DataSet();
                    dsMedicine = await confunc.ExecuteDataSet("[YYY_sp_Medicine_Combo_Load]" ,CommandType.StoredProcedure);

                    DataTable dt1 = dsMedicine.Tables[0].Copy();
                    dt1.TableName = "Medicine";

                    DataTable dt2 = dsMedicine.Tables[1].Copy();
                    dt2.TableName = "MedicineUnit";

                    ds.Tables.Add(dt1);
                    ds.Tables.Add(dt2);

                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data ,string ware)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt.Columns.Add("PackageNumber" ,typeof(string));
                dt.Columns.Add("ProductID" ,typeof(int));
                dt.Columns.Add("Number" ,typeof(decimal));
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PackageNumber"] = "";
                    dr["ProductID"] = dataDetail.Rows[i]["idProduct"];
                    dr["Number"] = dataDetail.Rows[i]["number"];
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("YYY_sp_Product_Stock_Quantity_RealTime" ,CommandType.StoredProcedure ,
                        "@DateExecute" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@WareID" ,SqlDbType.Int ,ware ,
                        "@MasterID" ,SqlDbType.Int ,0 ,
                        "@dt" ,SqlDbType.Structured ,dt.Rows.Count > 0 ? dt : null
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadPrescription(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Prescription_Medicine_ComboDetail]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadUpdate(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Prescription_Medicine_LoadDetail]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID)
                        ,"@UserId" ,SqlDbType.Int ,user.sys_userid
                        ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataDetail ,string CustomerID ,string CurrentID ,string PatientRecordID ,string IsExportMedicine)
        {
            try
            {
                PreMedicineDetail DataMain = JsonConvert.DeserializeObject<PreMedicineDetail>(data);
                DataTable DataDetail = JsonConvert.DeserializeObject<DataTable>(dataDetail);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();

                DataTable dtDetail = new DataTable();
                dtDetail.Columns.Add("IdDetail" ,typeof(Int32));
                dtDetail.Columns.Add("MedicineID" ,typeof(Int32));
                dtDetail.Columns.Add("Unit_ID" ,typeof(Int32));
                dtDetail.Columns.Add("Price" ,typeof(decimal));
                dtDetail.Columns.Add("Quantity" ,typeof(Int32));
                dtDetail.Columns.Add("Dosage1" ,typeof(string));
                dtDetail.Columns.Add("Dosage2" ,typeof(string));
                dtDetail.Columns.Add("Dosage3" ,typeof(string));
                dtDetail.Columns.Add("MedicineNote" ,typeof(string));
                dtDetail.Columns.Add("PackageNumber" ,typeof(string));
                for (int i = 0; i < DataDetail.Rows.Count; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    dr["IdDetail"] = Convert.ToInt32(DataDetail.Rows[i]["IdDetail"].ToString());
                    dr["MedicineID"] = Convert.ToInt32(DataDetail.Rows[i]["MedicineID"].ToString());
                    dr["Unit_ID"] = Convert.ToInt32(DataDetail.Rows[i]["Unit_ID"].ToString());
                    dr["Price"] = Convert.ToDecimal(DataDetail.Rows[i]["Price"].ToString());
                    dr["Quantity"] = Convert.ToInt32(DataDetail.Rows[i]["Quantity"].ToString());
                    dr["Dosage1"] = Convert.ToString(DataDetail.Rows[i]["Dosage1"].ToString());
                    dr["Dosage2"] = Convert.ToString(DataDetail.Rows[i]["Dosage2"].ToString());
                    dr["Dosage3"] = Convert.ToString(DataDetail.Rows[i]["Dosage3"].ToString());
                    dr["MedicineNote"] = Convert.ToString(DataDetail.Rows[i]["MedicineNote"].ToString());
                    dr["PackageNumber"] = "";
                    dtDetail.Rows.Add(dr);
                }

                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Prescription_Medicine_Update]" ,CommandType.StoredProcedure ,
                             "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                             "@Name" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Name) ? DataMain.Name : "" ,
                             "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID) ,
                             "@PrescriptionMedicineID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PreMedicineID) ,
                             "@PriceDiscounted" ,SqlDbType.Decimal ,DataMain.PriceDiscounted ,
                             "@PriceRoot" ,SqlDbType.Decimal ,DataMain.PriceRoot ,
                             "@Quantity" ,SqlDbType.Int ,DataMain.Quantity ,
                             "@PerDiscount" ,SqlDbType.Decimal ,DataMain.PerDiscount ,
                             "@AmountDiscount" ,SqlDbType.Decimal ,DataMain.AmountDiscount ,
                             "@Note" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Note) ? DataMain.Note : "" ,
                             "@SourceService" ,SqlDbType.Int ,Convert.ToInt32(DataMain.SourceService) ,
                             "@Diagnosis" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Diagnosis) ? DataMain.Diagnosis : "" ,
                             "@Subclinical" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Subclinical) ? DataMain.Subclinical : "" ,
                             "@DocCreatedID" ,SqlDbType.Int ,DataMain.DocCreatedID ,
                             "@SellerID" ,SqlDbType.Int ,DataMain.SellerID ,
                             "@CardAmountUsing" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.CardAmountUsing) ? DataMain.CardAmountUsing : "" ,
                             "@BranchID" ,SqlDbType.Int ,user.sys_branchID ,
                             "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                             "@IsExportMedicine" ,SqlDbType.Int ,IsExportMedicine ,
                             "@IsArisePresAfterExport" ,SqlDbType.Int ,Comon.Global.sys_ArisePresAfterExport != null ? Comon.Global.sys_ArisePresAfterExport : 0 ,
                             "@StatusICD" ,SqlDbType.NVarChar ,DataMain.StatusICD ,
                             "@table_data" ,SqlDbType.Structured ,dtDetail.Rows.Count > 0 ? dtDetail : null
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Prescription_Medicine_Insert]" ,CommandType.StoredProcedure ,
                             "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID) ,
                             "@Name" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Name) ? DataMain.Name : "" ,
                             "@PrescriptionMedicineID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PreMedicineID) ,
                             "@PriceDiscounted" ,SqlDbType.Decimal ,DataMain.PriceDiscounted ,
                             "@PriceRoot" ,SqlDbType.Decimal ,DataMain.PriceRoot ,
                             "@Quantity" ,SqlDbType.Int ,DataMain.Quantity ,
                             "@PerDiscount" ,SqlDbType.Int ,DataMain.PerDiscount ,
                             "@AmountDiscount" ,SqlDbType.Decimal ,DataMain.AmountDiscount ,
                             "@Note" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Note) ? DataMain.Note : "" ,
                             "@SourceService" ,SqlDbType.Int ,Convert.ToInt32(DataMain.SourceService) ,
                             "@Diagnosis" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Diagnosis) ? DataMain.Diagnosis : "" ,
                             "@Subclinical" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.Subclinical) ? DataMain.Subclinical : "" ,
                             "@CardAmountUsing" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.CardAmountUsing) ? DataMain.CardAmountUsing : "" ,
                             "@BranchID" ,SqlDbType.Int ,user.sys_branchID ,
                             "@DocCreatedID" ,SqlDbType.Int ,DataMain.DocCreatedID ,
                             "@SellerID" ,SqlDbType.Int ,DataMain.SellerID ,
                             "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                             "@Created" ,SqlDbType.NVarChar ,DataMain.Created ,
                             "@table_data" ,SqlDbType.Structured ,dtDetail.Rows.Count > 0 ? dtDetail : null ,
                             "@IsExportMedicine" ,SqlDbType.Int ,IsExportMedicine ,
                             "@IsArisePresAfterExport" ,SqlDbType.Int ,Comon.Global.sys_ArisePresAfterExport != null ? Comon.Global.sys_ArisePresAfterExport : 0 ,
                             "@StatusICD" ,SqlDbType.NVarChar ,DataMain.StatusICD ,
                             "@PatientRecordID" ,SqlDbType.Int ,Convert.ToInt32(PatientRecordID)
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



        public class PreMedicineDetail
        {
            public string Name { get; set; }
            public int PreMedicineID { get; set; }
            public int DocCreatedID { get; set; }
            public int SellerID { get; set; }
            public decimal PriceDiscounted { get; set; }
            public decimal PriceRoot { get; set; }
            public int Quantity { get; set; }
            public int PerDiscount { get; set; }
            public decimal AmountDiscount { get; set; }
            public string Note { get; set; }
            public string Diagnosis { get; set; }
            public string Subclinical { get; set; }
            public string CardAmountUsing { get; set; }
            public int SourceService { get; set; }
            public string Created { get; set; }
            public string StatusICD { get; set; }

        }
    }
}
