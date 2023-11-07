using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.DrugStore
{
    public class DrugStoreDetailModel : PageModel
    {
        public string _MedID { get; set; }
        public string _ExportID { get; set; }
        public int sysArisePresAfterExport { get; set; }
        public void OnGet()
        {
            var MedID = Request.Query["MedID"];
            var ExportID = Request.Query["ExportID"];

            _MedID = MedID.ToString() != String.Empty ? MedID.ToString() : "";
            _ExportID = ExportID.ToString() != String.Empty ? ExportID.ToString() : "";
            sysArisePresAfterExport = Comon.Global.sys_ArisePresAfterExport != null ? Comon.Global.sys_ArisePresAfterExport : 0;
        }
        public async Task<IActionResult> OnPostGetWareHouse(int MedID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_WareHouse_DrugStore_Info" ,CommandType.StoredProcedure
                    ,"@PrescriptionID" ,SqlDbType.Int ,MedID);
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
            catch (Exception ex)
            {
                return Content("0");
            }

        }



        public async Task<IActionResult> OnPostLoadData(int DetailID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_WareHouse_Export_Prepare]" ,CommandType.StoredProcedure
                         ,"@DetailID" ,SqlDbType.Int ,DetailID);
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
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostUnExport(int DetailID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Drug_Export_UnExportBranch" ,CommandType.StoredProcedure
                         ,"@MasterID" ,SqlDbType.Int ,DetailID
                         ,"@IsArisePresAfterExport" ,SqlDbType.Int ,Comon.Global.sys_ArisePresAfterExport);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostLoadPackNum(string MasterID, string ProductID, string WareID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_DrugStore_PackageNumer]" ,CommandType.StoredProcedure
                        ,"@MasterSTID" ,SqlDbType.Int ,MasterID
                        ,"@WareID" ,SqlDbType.Int ,WareID
                        ,"@ProductID" ,SqlDbType.Int ,ProductID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string ware ,string CurrentID)
        {
            try
            {

                DataTable DataExport = new DataTable();
                DataExport.Columns.Add("productID" ,typeof(Int32));
                DataExport.Columns.Add("unitID" ,typeof(Int32));
                DataExport.Columns.Add("number" ,typeof(decimal));
                DataExport.Columns.Add("package" ,typeof(String));
                DataExport.Columns.Add("current" ,typeof(Int32));

                DataTable _DataExport = new DataTable();
                _DataExport = JsonConvert.DeserializeObject<DataTable>(data);

                for (int i = 0; i < _DataExport.Rows.Count; i++)
                {
                    DataRow dr = DataExport.NewRow();
                    dr["productID"] = Convert.ToInt32(_DataExport.Rows[i]["productID"]);
                    dr["unitID"] = Convert.ToInt32(_DataExport.Rows[i]["unitID"]);
                    dr["number"] = Convert.ToDecimal(_DataExport.Rows[i]["number"]);
                    dr["package"] = _DataExport.Rows[i]["package"].ToString();
                    dr["current"] = Convert.ToInt32(_DataExport.Rows[i]["current"]);
                    DataExport.Rows.Add(dr);
                }
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_WareHouse_Export_DrugExecute" ,CommandType.StoredProcedure ,
                        "@DateExecute" ,SqlDbType.DateTime ,DateTime.Now ,
                        "@WareID" ,SqlDbType.Int ,ware ,
                        "@MasterID" ,SqlDbType.Int ,CurrentID ,
                        "@CreatedBy" ,SqlDbType.Int ,user.sys_userid ,
                        "@IsPackageNumber" ,SqlDbType.Int ,1 ,
                        "@IsArisePresAfterExport" ,SqlDbType.Int ,Comon.Global.sys_ArisePresAfterExport != null ? Comon.Global.sys_ArisePresAfterExport : 0 ,
                        "@dt" ,SqlDbType.Structured ,DataExport.Rows.Count > 0 ? DataExport : null
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data ,string ware ,string CurrentID ,string IsPackageNumber)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                dt.Columns.Add("PackageNumber" ,typeof(string));
                dt.Columns.Add("ProductID" ,typeof(int));
                dt.Columns.Add("Number" ,typeof(decimal));
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PackageNumber"] = dataDetail.Rows[i]["package"];
                    dr["ProductID"] = dataDetail.Rows[i]["productID"];
                    dr["Number"] = Convert.ToDecimal(dataDetail.Rows[i]["numberStandard"]);
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("YYY_sp_Product_Stock_Quantity_STRealTime" ,CommandType.StoredProcedure ,
                        "@DateExecute" ,SqlDbType.DateTime ,DateTime.Now ,
                        "@IsPackageNumber" ,SqlDbType.Int ,IsPackageNumber ,
                        "@WareID" ,SqlDbType.Int ,ware ,
                        "@MasterSTID" ,SqlDbType.Int ,CurrentID ,
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

    }
}
