using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Service
{
    public class DataServiceDetail
    {

        public string ServiceProduct { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceType { get; set; }
        public string Amount { get; set; }
        public string Amount_Max { get; set; }
        public string PerConsulAmount { get; set; }
        public string PerConsulPercent { get; set; }
        public string PerTelesaleAmount { get; set; }
        public string PerTelesalePercent { get; set; }
        public string PerRevenue { get; set; }
        public string PerTreatAmount { get; set; }
        public string PerTreatPercent { get; set; }
        public string PerTreatAmountDoc { get; set; }
        public string PerTreatPercentDoc { get; set; }
        public string PerPaidAmountDoc { get; set; }
        public string PerPaidPercentDoc { get; set; }
        public string PerTreatAmountSpecific { get; set; }
        public string PerTreatPercentSpecific { get; set; }
        public string Content { get; set; }
        public string TimeToTreatment { get; set; }
        public string UnitCount { get; set; }
        public string MediaFolder { get; set; }
        public string link_guid { get; set; }
        public string link_warranty { get; set; }
        public string IdentificationCode { get; set; }
        public string IdentificationName { get; set; }
        public int IsInstallment { get; set; }
        public int TeethType { get; set; }
        public int IsLabo { get; set; }
        public int WarrantyMonth { get; set; }
        public int UseExportStage { get; set; }

    }
    public class ServiceDetailModel : PageModel
    {
        public string _ServiceDetailCurrentID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string sys_ExportConsumableAllowStage { get; set; }


        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;

            string curr = Request.Query["CurrentID"];
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            sys_ExportConsumableAllowStage = Comon.Global.sys_ExportConsumableAllowStage.ToString();
            if (curr != null)
            {
                _ServiceDetailCurrentID = curr.ToString();
            }
            else
            {
                _ServiceDetailCurrentID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoad_Data_Initialize(int currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtType = new DataTable();
                        dtType = await connFunc.ExecuteDataTable("YYY_sp_v2_ServiceType_LoadCombo" ,CommandType.StoredProcedure);
                        dtType.TableName = "Type";
                        return dtType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnit = new DataTable();
                        dtUnit = await connFunc.ExecuteDataTable("YYY_sp_Service_UnitCount" ,CommandType.StoredProcedure);
                        dtUnit.TableName = "ServiceUnit";
                        return dtUnit;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProUnit = new DataTable();
                        dtProUnit = await connFunc.ExecuteDataTable("YYY_sp_Product_Combo_Unit" ,CommandType.StoredProcedure);
                        dtProUnit.TableName = "ProductUnit";
                        return dtProUnit;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitFull = new DataTable();
                        dtUnitFull = await connFunc.ExecuteDataTable("YYY_sp_Product_Combo_UnitFull" ,CommandType.StoredProcedure);
                        dtUnitFull.TableName = "UnitFull";
                        return dtUnitFull;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMedia = new DataTable();
                        dtMedia = await connFunc.ExecuteDataTable("[YYY_sp_Media_LoadCombo]" ,CommandType.StoredProcedure);
                        dtMedia.TableName = "Media";
                        return dtMedia;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCurrency = new DataTable();
                        dtCurrency = await connFunc.ExecuteDataTable("[YYY_sp_Price_Service_Currency]" ,CommandType.StoredProcedure);
                        dtCurrency.TableName = "Currency";
                        return dtCurrency;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await connFunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,0
                            ,"@LoadNormal" ,SqlDbType.Int ,0);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await connFunc.ExecuteDataTable("[YYY_sp_Product_Get]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitPro = new DataTable();
                        dtUnitPro = await connFunc.ExecuteDataTable("[YYY_sp_Product_Unit_Product]" ,CommandType.StoredProcedure);
                        dtUnitPro.TableName = "UnitProduct";
                        return dtUnitPro;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSyntaxCode = new DataTable();
                        dtSyntaxCode = await connFunc.ExecuteDataTable("[YYY_sp_v2_Service_Get_Syntax_Code]" ,CommandType.StoredProcedure);
                        dtSyntaxCode.TableName = "SyntaxCode";
                        return dtSyntaxCode;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTreatStage = new DataTable();
                        dtTreatStage = await connFunc.ExecuteDataTable("[YYY_sp_Service_TreatStage_Combo]" ,CommandType.StoredProcedure);
                        dtTreatStage.TableName = "TreatStage";
                        return dtTreatStage;
                    }
                }));


                if (currentid != 0)
                {
                    DataSet dsdetail = new DataSet();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dsdetail = await confunc.ExecuteDataSet("[YYY_sp_v2_ServiceDetail_Load]" ,CommandType.StoredProcedure ,
                          "@ID" ,SqlDbType.Int ,Convert.ToInt32(currentid == 0 ? 0 : currentid));
                    }
                    if (dsdetail != null)
                    {
                        DataTable m = dsdetail.Tables[0].Copy();
                        m.TableName = "Update_Main";
                        ds.Tables.Add(m);

                        DataTable v = dsdetail.Tables[1].Copy();
                        v.TableName = "Update_VTTH";
                        ds.Tables.Add(v);

                        DataTable note = dsdetail.Tables[2].Copy();
                        note.TableName = "Update_Note";
                        ds.Tables.Add(note);

                        DataTable price = dsdetail.Tables[3].Copy();
                        price.TableName = "Update_Price";
                        ds.Tables.Add(price);

                        DataTable stage = dsdetail.Tables[4].Copy();
                        stage.TableName = "Update_Stage";
                        ds.Tables.Add(stage);

                    }
                }
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

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataVTTH
           ,string dataNote ,string dataStageService ,string priceService ,string service_syntax_code ,string CurrentID)
        {

            try
            {
                DataServiceDetail DataMain = JsonConvert.DeserializeObject<DataServiceDetail>(data);
                DataTable DataServiceStage = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                // VTTH
                DataTable DataVTTH = new DataTable();
                DataVTTH.Columns.Add("productID" ,typeof(String));
                DataVTTH.Columns.Add("unitID" ,typeof(String));
                DataVTTH.Columns.Add("minNumber" ,typeof(decimal));
                DataVTTH.Columns.Add("maxNumber" ,typeof(decimal));
                DataVTTH.Columns.Add("note" ,typeof(String));

                DataTable _DataVTTH = new DataTable();
                _DataVTTH = JsonConvert.DeserializeObject<DataTable>(dataVTTH);
                for (int i = 0; i < _DataVTTH.Rows.Count; i++)
                {
                    DataRow dr = DataVTTH.NewRow();
                    dr["productID"] = _DataVTTH.Rows[i]["productID"];
                    dr["unitID"] = _DataVTTH.Rows[i]["unitID"];
                    dr["minNumber"] = Convert.ToDecimal(_DataVTTH.Rows[i]["minNumber"]);
                    dr["maxNumber"] = Convert.ToDecimal(_DataVTTH.Rows[i]["maxNumber"]);
                    dr["note"] = _DataVTTH.Rows[i]["note"];
                    DataVTTH.Rows.Add(dr);
                }

                // note sevice
                DataTable DataNote = new DataTable();
                DataNote.Columns.Add("ID" ,typeof(String));
                DataNote.Columns.Add("Tag" ,typeof(String));
                DataNote.Columns.Add("Explain" ,typeof(String));
                DataTable _DataNote = new DataTable();
                _DataNote = JsonConvert.DeserializeObject<DataTable>(dataNote);
                for (int i = 0; i < _DataNote.Rows.Count; i++)
                {
                    DataRow dr = DataNote.NewRow();
                    dr["ID"] = _DataNote.Rows[i]["id"];
                    dr["Tag"] = _DataNote.Rows[i]["tag"];
                    dr["Explain"] = _DataNote.Rows[i]["explain"];
                    DataNote.Rows.Add(dr);
                }

                // stage sevice
                DataTable DataStageService = new DataTable();
                DataStageService.Columns.Add("ID" ,typeof(Int32));
                DataStageService.Columns.Add("TreatStageID" ,typeof(Int32));
                DataStageService.Columns.Add("Percent" ,typeof(Int32));
                DataStageService.Columns.Add("PercentComplete" ,typeof(Int32));
                DataTable _DataStageService = new DataTable();
                _DataStageService = JsonConvert.DeserializeObject<DataTable>(dataStageService);
                for (int i = 0; i < _DataStageService.Rows.Count; i++)
                {
                    DataRow dr = DataStageService.NewRow();
                    dr["ID"] = Convert.ToInt32(_DataStageService.Rows[i]["id"]);
                    dr["TreatStageID"] = _DataStageService.Rows[i]["treatStageID"];
                    dr["Percent"] = _DataStageService.Rows[i]["percent"];
                    dr["PercentComplete"] = _DataStageService.Rows[i]["percentcomplete"];
                    DataStageService.Rows.Add(dr);
                }

                // price sevice
                DataTable DataPrice = new DataTable();
                DataPrice.Columns.Add("id" ,typeof(String));
                DataPrice.Columns.Add("branchid" ,typeof(String));
                DataPrice.Columns.Add("exchangeid" ,typeof(String));
                DataPrice.Columns.Add("pricemin" ,typeof(decimal));
                DataPrice.Columns.Add("pricemax" ,typeof(decimal));
                DataTable _DataPrice = new DataTable();
                _DataPrice = JsonConvert.DeserializeObject<DataTable>(priceService);
                for (int i = 0; i < _DataPrice.Rows.Count; i++)
                {
                    DataRow dr = DataPrice.NewRow();
                    dr["id"] = _DataPrice.Rows[i]["id"];
                    dr["branchid"] = _DataPrice.Rows[i]["branchid"];
                    dr["exchangeid"] = _DataPrice.Rows[i]["exchangeid"];
                    dr["pricemin"] = Convert.ToDecimal(_DataPrice.Rows[i]["pricemin"]);
                    dr["pricemax"] = Convert.ToDecimal(_DataPrice.Rows[i]["pricemax"]);
                    DataPrice.Rows.Add(dr);
                }

                // isproduct
                int isProduct = (DataMain.ServiceProduct == "1") ? 0 : 1;
                int isDentistCos = Comon.Global.sys_DentistCosmetic;
                // san pham, hoac nha khoa thi timetotreat =1
                string timetotreat = (isProduct == 1 || isDentistCos == 1) ? "1" : DataMain.TimeToTreatment;
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_v2_ServiceDetail_Insert" ,CommandType.StoredProcedure ,

                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@Avatar" ,SqlDbType.NVarChar ,DataMain.Avatar.Replace("'" ,"").Trim() ,
                            "@NameEn" ,SqlDbType.NVarChar ,DataMain.NameEn.Replace("'" ,"").Trim() ,
                            "@ServiceCode" ,SqlDbType.NVarChar ,DataMain.ServiceCode.Replace("'" ,"").Trim() ,
                            "@IdentificationCode" ,SqlDbType.NVarChar ,DataMain.IdentificationCode ,
                            "@IdentificationName" ,SqlDbType.NVarChar ,DataMain.IdentificationName.Replace("'" ,"").Trim() ,
                            "@NameNoSign" ,SqlDbType.NVarChar ,Comon.Comon.ConvertToUnsign(DataMain.Name.Replace("'" ,"").Trim()) ,
                            "@Amount" ,SqlDbType.Decimal ,DataMain.Amount ,
                            "@Amount_Max" ,SqlDbType.Decimal ,DataMain.Amount_Max ,
                            "@ServiceType" ,SqlDbType.Int ,DataMain.ServiceType ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Content" ,SqlDbType.Int ,DataMain.Content.Replace("'" ,"").Trim() ,
                            "@PerConsulAmount" ,SqlDbType.Decimal ,DataMain.PerConsulAmount ,
                            "@PerConsulPercent" ,SqlDbType.Decimal ,DataMain.PerConsulPercent ,
                            "@PerTelesaleAmount" ,SqlDbType.Decimal ,DataMain.PerTelesaleAmount ,
                            "@PerTelesalePercent" ,SqlDbType.Decimal ,DataMain.PerTelesalePercent ,
                            "@PerRevenue" ,SqlDbType.Decimal ,DataMain.PerRevenue ,
                            "@PerTreatAmount" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmount ,
                            "@PerTreatPercent" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercent ,
                            "@PerTreatAmountDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmountDoc ,
                            "@PerTreatPercentDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercentDoc ,
                            "@PerPaidAmountDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerPaidAmountDoc ,
                            "@PerPaidPercentDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerPaidPercentDoc ,
                            "@PerTreatAmountSpecific" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmountSpecific ,
                            "@PerTreatPercentSpecific" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercentSpecific ,
                            "@TimeToTreatment" ,SqlDbType.Int ,timetotreat ,
                            "@UnitCount" ,SqlDbType.Int ,DataMain.UnitCount ,
                            "@IsProduct" ,SqlDbType.Int ,isProduct ,
                            "@MediaFolder" ,SqlDbType.Int ,DataMain.MediaFolder ,
                            "@Link_guid" ,SqlDbType.NVarChar ,DataMain.link_guid ,
                            "@Link_warranty" ,SqlDbType.NVarChar ,DataMain.link_warranty ,
                            "@IsInstallment" ,SqlDbType.Int ,DataMain.IsInstallment ,
                            "@IsLabo" ,SqlDbType.Int ,DataMain.IsLabo ,
                            "@TeethType" ,SqlDbType.Int ,DataMain.TeethType ,
                            "@DataVTTH" ,SqlDbType.Structured ,DataVTTH.Rows.Count > 0 ? DataVTTH : null ,
                            "@DataNote" ,SqlDbType.Structured ,DataNote.Rows.Count > 0 ? DataNote : null ,
                            "@DataStage" ,SqlDbType.Structured ,DataStageService.Rows.Count > 0 ? DataStageService : null ,
                            "@DataPrice" ,SqlDbType.Structured ,DataPrice.Rows.Count > 0 ? DataPrice : null ,
                            "@service_syntax_code" ,SqlDbType.NVarChar ,service_syntax_code != null ? service_syntax_code : "" ,
                            "@WarrantyMonth" ,SqlDbType.Int ,DataMain.WarrantyMonth ,
                            "@UseExportStage" ,SqlDbType.Int ,DataMain.UseExportStage
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_v2_ServiceDetail_Update" ,CommandType.StoredProcedure ,

                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@Avatar" ,SqlDbType.NVarChar ,DataMain.Avatar.Replace("'" ,"").Trim() ,
                            "@NameEn" ,SqlDbType.NVarChar ,DataMain.NameEn.Replace("'" ,"").Trim() ,
                            "@ServiceCode" ,SqlDbType.NVarChar ,DataMain.ServiceCode.Replace("'" ,"").Trim() ,
                            "@IdentificationCode" ,SqlDbType.NVarChar ,DataMain.IdentificationCode.Replace("'" ,"").Trim() ,
                            "@IdentificationName" ,SqlDbType.NVarChar ,DataMain.IdentificationName.Replace("'" ,"").Trim() ,
                            "@NameNoSign" ,SqlDbType.NVarChar ,Comon.Comon.ConvertToUnsign(DataMain.Name.Replace("'" ,"").Trim()) ,
                            "@Amount" ,SqlDbType.Decimal ,DataMain.Amount ,
                            "@Amount_Max" ,SqlDbType.Decimal ,DataMain.Amount_Max ,
                            "@ServiceType" ,SqlDbType.Int ,DataMain.ServiceType ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"").Trim() ,
                            "@PerConsulAmount" ,SqlDbType.Decimal ,DataMain.PerConsulAmount ,
                            "@PerConsulPercent" ,SqlDbType.Decimal ,DataMain.PerConsulPercent ,
                            "@PerTelesaleAmount" ,SqlDbType.Decimal ,DataMain.PerTelesaleAmount ,
                            "@PerTelesalePercent" ,SqlDbType.Decimal ,DataMain.PerTelesalePercent ,
                            "@PerRevenue" ,SqlDbType.Decimal ,DataMain.PerRevenue ,
                            "@PerTreatAmount" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmount ,
                            "@PerTreatPercent" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercent ,
                            "@PerTreatAmountDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmountDoc ,
                            "@PerTreatPercentDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercentDoc ,
                            "@PerPaidAmountDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerPaidAmountDoc ,
                            "@PerPaidPercentDoc" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerPaidPercentDoc ,
                            "@PerTreatAmountSpecific" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatAmountSpecific ,
                            "@PerTreatPercentSpecific" ,SqlDbType.Decimal ,(isProduct == 1) ? "0" : DataMain.PerTreatPercentSpecific ,
                            "@TimeToTreatment" ,SqlDbType.Int ,timetotreat ,
                            "@UnitCount" ,SqlDbType.Int ,DataMain.UnitCount ,
                            "@IsProduct" ,SqlDbType.Int ,isProduct ,
                            "@MediaFolder" ,SqlDbType.Int ,DataMain.MediaFolder ,
                            "@Link_guid" ,SqlDbType.NVarChar ,DataMain.link_guid ,
                            "@Link_warranty" ,SqlDbType.NVarChar ,DataMain.link_warranty ,
                            "@IsInstallment" ,SqlDbType.Int ,DataMain.IsInstallment ,
                            "@IsLabo" ,SqlDbType.Int ,DataMain.IsLabo ,
                            "@TeethType" ,SqlDbType.Int ,DataMain.TeethType ,
                            "@DataVTTH" ,SqlDbType.Structured ,DataVTTH.Rows.Count > 0 ? DataVTTH : null ,
                            "@DataNote" ,SqlDbType.Structured ,DataNote.Rows.Count > 0 ? DataNote : null ,
                            "@DataStage" ,SqlDbType.Structured ,DataStageService.Rows.Count > 0 ? DataStageService : null ,
                            "@DataPrice" ,SqlDbType.Structured ,DataPrice.Rows.Count > 0 ? DataPrice : null ,
                            "@service_syntax_code" ,SqlDbType.NVarChar ,service_syntax_code != null ? service_syntax_code : "" ,
                            "@currentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                            "@WarrantyMonth" ,SqlDbType.Int ,DataMain.WarrantyMonth ,
                            "@UseExportStage" ,SqlDbType.Int ,DataMain.UseExportStage
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

        public async Task<IActionResult> OnPostCheckDeleStage(string idStage)
        {
            try
            {
                if (idStage == "0") return Content("0");
                DataTable dt1 = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt1 = await connFunc.ExecuteDataTable("[YYY_sp_Service_CheckDeleteStage]" ,CommandType.StoredProcedure ,
                    "@ID" ,SqlDbType.Int ,Convert.ToInt32(idStage));
                }
                if (dt1 != null && dt1.Rows.Count != 0 && Convert.ToInt32(dt1.Rows[0][0]) != 0)
                    return Content("0");
                else return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Service_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
