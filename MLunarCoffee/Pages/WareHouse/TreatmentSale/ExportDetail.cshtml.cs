﻿using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.TreatmentSale
{
    public class ExportDetail : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostGetWareHouse(int DetailID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_WareHouse_ExportByBranch_Info" ,CommandType.StoredProcedure
                    ,"@DetailID" ,SqlDbType.Int ,DetailID);
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
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostLoadPackNum(string ProductID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_PackageNumer]" ,CommandType.StoredProcedure
                        ,"@ProductID" ,SqlDbType.Int ,ProductID);
                }
                return Content(Comon.DataJson.Datatable(dt));
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
                    dt = await confunc.ExecuteDataTable("[MLU_sp_WareHouse_Export_Prepare]" ,CommandType.StoredProcedure
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
                    dt = await confunc.ExecuteDataTable("[MLU_sp_WareHouse_Export_UnExportBranch]" ,CommandType.StoredProcedure
                         ,"@MasterID" ,SqlDbType.Int ,DetailID);
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

        public async Task<IActionResult> OnPostDelete(int DetailID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_WareHouse_Export_DeleteExportBranch]" ,CommandType.StoredProcedure
                         ,"@MasterID" ,SqlDbType.Int ,DetailID);
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


        public async Task<IActionResult> OnPostExcuteData(string data ,string ware ,string DateExecute ,string IsPackageNumber ,string CurrentID)
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
                    dt = await connFunc.ExecuteDataTable("MLU_sp_WareHouse_Export_PrepareExecute" ,CommandType.StoredProcedure ,
                        "@DateExecute" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute) ,
                        "@WareID" ,SqlDbType.Int ,ware ,
                        "@MasterID" ,SqlDbType.Int ,CurrentID ,
                        "@IsPackageNumber" ,SqlDbType.Int ,IsPackageNumber ,
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

        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data ,string ware ,string DateExecute ,string CurrentID ,string IsPackageNumber)
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
                    ds = await connFunc.ExecuteDataSet("MLU_sp_Product_Stock_Quantity_STRealTime" ,CommandType.StoredProcedure ,
                        "@DateExecute" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute) ,
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
