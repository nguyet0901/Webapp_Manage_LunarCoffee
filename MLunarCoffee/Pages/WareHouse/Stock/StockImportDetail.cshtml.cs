using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.WareHouse.Stock
{
    public class StockImportDetailModel : PageModel
    {
        public string _WareID { get; set; }
        public void OnGet()
        {
            string ware = Request.Query["WareID"];
            if (ware != null) _WareID = ware.ToString(); else _WareID = "0";
        }

        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Get]", CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitProduct = new DataTable();
                        dtUnitProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                        dtUnitProduct.TableName = "UnitProduct";
                        return dtUnitProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtWarehouse = new DataTable();
                        dtWarehouse = await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                                        , "@UserID", SqlDbType.Int, user.sys_userid
                                        , "@LoadNormal", SqlDbType.Int, 1);
                        dtWarehouse.TableName = "Warehouse";
                        return dtWarehouse;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnit = new DataTable();
                        dtUnit = await connFunc.ExecuteDataTable("[MLU_sp_Product_Combo_Unit]", CommandType.StoredProcedure);
                        dtUnit.TableName = "Unit";
                        return dtUnit;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSupplier = new DataTable();
                        dtSupplier = await connFunc.ExecuteDataTable("[MLU_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                        dtSupplier.TableName = "Supplier";
                        return dtSupplier;
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
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string dataDetail, string wareid)
        {
            try
            {
                StockImport stockImport = JsonConvert.DeserializeObject<StockImport>(data);
                DataTable dtResult = new DataTable();
                DataTable dtDetail = JsonConvert.DeserializeObject<DataTable>(dataDetail);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                dt.Columns.Add("PackageNumber", typeof(string));
                dt.Columns.Add("ExpiredDay", typeof(DateTime));
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("Quantity", typeof(decimal));
                dt.Columns.Add("UnitID", typeof(int));
                dt.Columns.Add("Discounted", typeof(decimal));
                dt.Columns.Add("Amount", typeof(decimal));
                dt.Columns.Add("Note", typeof(string));

                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PackageNumber"] = dtDetail.Rows[i]["PackageNumber"].ToString();
                    dr["ExpiredDay"] = Convert.ToDateTime(dtDetail.Rows[i]["DateExpiration"].ToString());
                    dr["Code"] = dtDetail.Rows[i]["Code"].ToString().Trim();
                    dr["Price"] = Convert.ToDecimal(dtDetail.Rows[i]["CostPrice"].ToString());
                    dr["Quantity"] = Convert.ToDecimal(dtDetail.Rows[i]["Quantity"].ToString());
                    dr["UnitID"] = Convert.ToInt32(dtDetail.Rows[i]["UnitID"].ToString());
                    dr["Discounted"] = Convert.ToDecimal(dtDetail.Rows[i]["Discounted"].ToString());
                    dr["Amount"] = Convert.ToDecimal(dtDetail.Rows[i]["Amount"].ToString());
                    dr["Note"] = dtDetail.Rows[i]["Note"].ToString();
                    dt.Rows.Add(dr);
                }

              
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_ReceiptImport_Insert", CommandType.StoredProcedure,
                        "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(stockImport.DateExecute),
                        "@CreatedBy", SqlDbType.Int, user.sys_userid,
                        "@WareID", SqlDbType.Int, Convert.ToInt32(wareid),
                        "@IsPackageNumber", SqlDbType.Int, stockImport.IsPackageNumber,
                        "@Note", SqlDbType.NVarChar, !String.IsNullOrEmpty(stockImport.Note) ? stockImport.Note : "" , 
                        "@NumberContract", SqlDbType.NVarChar, !String.IsNullOrEmpty(stockImport.NumberContract) ? stockImport.NumberContract : "",
                        "@NumberInvoice", SqlDbType.NVarChar, !String.IsNullOrEmpty(stockImport.NumberInvoice) ? stockImport.NumberInvoice : "",
                        "@DataTable", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null,
                        "@SupplierID", SqlDbType.Int, stockImport.SupplierID
                    );
                }
                
                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class StockImport
    {
        public int IsPackageNumber { get; set; }
        public int SupplierID { get; set; }
        public string DateExecute { get;set; }
        public string NumberContract { get;set; }
        public string NumberInvoice { get;set; }
        public string Note { get;set; }
    }
}
