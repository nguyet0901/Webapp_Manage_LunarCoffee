using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Stock
{
    public class StockOutDetailModel : PageModel
    {
        public string _DetailCurrentID { get; set; }
        public string _WareID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string ware = Request.Query["WareID"];
            if (curr != null) _DetailCurrentID = curr.ToString(); else _DetailCurrentID = "0";
            if (ware != null) _WareID = ware.ToString(); else _WareID = "0";
        }

        public async Task<IActionResult> OnPostInitialize(string currentid)
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
                        DataTable dtWarehouseTransfer = new DataTable();
                        dtWarehouseTransfer = await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                                , "@UserID", SqlDbType.Int, 0
                                , "@LoadNormal", SqlDbType.Int, 0);
                        dtWarehouseTransfer.TableName = "WarehouseTransfer";
                        return dtWarehouseTransfer;
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
                        DataTable dtReason = new DataTable();
                        dtReason = await connFunc.ExecuteDataTable("[MLU_sp_Product_Export_Reason]", CommandType.StoredProcedure);
                        dtReason.TableName = "Reason";
                        return dtReason;
                    }
                }));
                if (currentid != "0")
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtInfo = new DataTable();
                            dtInfo = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockOutDetail_Info]", CommandType.StoredProcedure
                                      , "@ID", SqlDbType.Int, Convert.ToInt32(currentid)
                                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                                      , "@UserID", SqlDbType.Int, user.sys_userid);
                            dtInfo.TableName = "Info";
                            return dtInfo;
                        }
                    }));
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtItems = new DataTable();
                            dtItems = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockOutDetail_Item]", CommandType.StoredProcedure
                                , "@ID", SqlDbType.Int, Convert.ToInt32(currentid));
                            dtItems.TableName = "Items";
                            return dtItems;
                        }
                    }));
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

        public async Task<IActionResult> OnPostSearchPackNum(string PackageNumber, string WareID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockSearch]", CommandType.StoredProcedure
                        , "@PackageNumber", SqlDbType.NVarChar, PackageNumber
                        , "@WareID", SqlDbType.Int, WareID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadStockQuantity(string masterID ,string productID,string wareID,string dateExecute)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Stock_Quantity]", CommandType.StoredProcedure
                        , "@ExportID", SqlDbType.Int, masterID
                        , "@ProductID", SqlDbType.Int, productID
                        , "@WareID", SqlDbType.Int, wareID
                        , "@DateExecute",SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateExecute));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadStockPackNumQuantity(string masterID, string packageNumber,string wareID,string dateExecute, string packageID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockPackNum_Quantity]", CommandType.StoredProcedure
                        , "@ExportID", SqlDbType.Int, masterID
                        , "@PackageNumber", SqlDbType.Int, packageNumber
                        , "@PackageID" , SqlDbType.Int,packageID
                        , "@WareID", SqlDbType.Int, wareID
                        , "@DateExecute",SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateExecute));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data, string ware, string DateExecute, string CurrentID)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt.Columns.Add("PackageNumber", typeof(string));
                dt.Columns.Add("ProductID", typeof(int));
                dt.Columns.Add("Number", typeof(decimal));
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PackageNumber"] = dataDetail.Rows[i]["packageNumber"];
                    dr["ProductID"] = dataDetail.Rows[i]["idProduct"];
                    dr["Number"] = dataDetail.Rows[i]["number"];
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("MLU_sp_Product_Stock_Quantity_RealTime", CommandType.StoredProcedure,
                        "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@WareID", SqlDbType.Int, ware,
                        "@MasterID", SqlDbType.Int, CurrentID,
                        "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string ware, string wareto, string reason, string note, string DateExecute, string CurrentID,string IsPackageNumber)
        {
            try
            {
                note = (note != null ? note : "");
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();

                dt.Columns.Add("packageNumber", typeof(string));
                dt.Columns.Add("idDetail", typeof(int));
                dt.Columns.Add("state", typeof(int));
                dt.Columns.Add("idProduct", typeof(int));
                dt.Columns.Add("SupplierID", typeof(int));
                dt.Columns.Add("UnitCountID", typeof(int));
                dt.Columns.Add("Number", typeof(decimal));
                dt.Columns.Add("Amount", typeof(Decimal));
                dt.Columns.Add("Note", typeof(string));
                dt.Columns.Add("ExpiredDay", typeof(DateTime));

                dt.Columns.Add("IniAmount", typeof(decimal));
                dt.Columns.Add("DiscountAmount", typeof(decimal));
                dt.Columns.Add("DiscountPer", typeof(decimal));
                dt.Columns.Add("UnitPrice", typeof(decimal));

                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["packageNumber"] = dataDetail.Rows[i]["packageNumber"];
                    dr["idDetail"] = dataDetail.Rows[i]["idDetail"];
                    dr["state"] = dataDetail.Rows[i]["state"];
                    dr["idProduct"] = dataDetail.Rows[i]["idProduct"];
                    dr["UnitCountID"] = dataDetail.Rows[i]["UnitCountID"];
                    dr["Number"] = dataDetail.Rows[i]["Number"];
                    dr["Note"] = dataDetail.Rows[i]["Note"];
                    dr["SupplierID"] = 0;
                    dr["Amount"] = Convert.ToDecimal(dataDetail.Rows[i]["Amount"]);
                    dr["ExpiredDay"] = Comon.Comon.DateTimeDMY_To_YMD(dataDetail.Rows[i]["ExpiredDay"].ToString());
                    dr["IniAmount"] = dataDetail.Rows[i]["IniAmount"];
                    dr["DiscountAmount"] = dataDetail.Rows[i]["DiscountAmount"];
                    dr["DiscountPer"] = dataDetail.Rows[i]["DiscountPer"];
                    dr["UnitPrice"] = dataDetail.Rows[i]["UnitPrice"];
                    dt.Rows.Add(dr);
                }

                DataTable dtResult = new DataTable();
                if (wareto == "0")
                {
                    if (CurrentID == "0")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Export_Insert", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@Ware_ID", SqlDbType.Int, ware,
                                "@TransferID", SqlDbType.Int, 0,
                                "@reason", SqlDbType.Int, reason,
                                "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }
                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Export_Update", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@Ware_ID", SqlDbType.Int, ware,
                                "@reason", SqlDbType.Int, reason,
                                "@Modified_By", SqlDbType.Int, user.sys_userid,
                                "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                                "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }

                    }
                }
                else
                {
                    if (CurrentID == "0")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Transfer_Insert", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                                "@Created_by", SqlDbType.Int, user.sys_userid,
                                "@reason", SqlDbType.Int, reason,
                                "@Ware_From", SqlDbType.Int, ware,
                                "@Ware_To", SqlDbType.Int, wareto,
                                "@note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }
                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Export_Update", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@Ware_ID", SqlDbType.Int, ware,
                                "@reason", SqlDbType.Int, reason,
                                "@Modified_By", SqlDbType.Int, user.sys_userid,
                                "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                                "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }

                    }
                }
                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id,string typeid, string ware)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Stock_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Type", SqlDbType.Int, Convert.ToInt32(typeid),
                        "@WareID", SqlDbType.Int, Convert.ToInt32(ware)
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
