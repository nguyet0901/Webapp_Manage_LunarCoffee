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
    public class StockInDetailModel : PageModel
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
                        dtProduct = await connFunc.ExecuteDataTable("[YYY_sp_Product_Get]", CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitProduct = new DataTable();
                        dtUnitProduct = await connFunc.ExecuteDataTable("[YYY_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                        dtUnitProduct.TableName = "UnitProduct";
                        return dtUnitProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtWarehouse = new DataTable();
                        dtWarehouse = await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
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
                        dtUnit = await connFunc.ExecuteDataTable("[YYY_sp_Product_Combo_Unit]", CommandType.StoredProcedure);
                        dtUnit.TableName = "Unit";
                        return dtUnit;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSupplier = new DataTable();
                        dtSupplier = await connFunc.ExecuteDataTable("[YYY_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                        dtSupplier.TableName = "Supplier";
                        return dtSupplier;
                    }
                }));
                if (currentid != "0")
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtInfo = new DataTable();
                            dtInfo = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockInDetail_Info]", CommandType.StoredProcedure
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
                            dtItems = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockInDetail_Item]", CommandType.StoredProcedure
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
                    ds = await connFunc.ExecuteDataSet("YYY_sp_Product_StockIn_Quantity_RealTime", CommandType.StoredProcedure,
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
        public async Task<IActionResult> OnPostLoadProduct()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_Get]", CommandType.StoredProcedure);
                    dt.TableName = "Product";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                    dt.TableName = "UnitProduct";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostGetExpiredDate(string productid, string packagenumber)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_WareHouse_GetExpired]", CommandType.StoredProcedure,
                          "@productid", SqlDbType.Int, productid,
                           "@packagenumber", SqlDbType.NVarChar, packagenumber
                        );

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadSupplier()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string ware, string supplierID, string note, string numberContract, string numberInvoice, string DateExecute, string CurrentID, string IsPackageNumber)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                DataTable dtResult = new DataTable();
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
                dt.Columns.Add("Amount", typeof(decimal));
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
                    dr["SupplierID"] = dataDetail.Rows[i]["SupplierID"];
                    dr["UnitCountID"] = dataDetail.Rows[i]["UnitCountID"];
                    dr["Number"] = dataDetail.Rows[i]["Number"];
                    dr["Amount"] = Convert.ToDecimal(dataDetail.Rows[i]["Amount"]);
                    dr["Note"] = dataDetail.Rows[i]["Note"];
                    dr["ExpiredDay"] = Comon.Comon.DateTimeDMY_To_YMD(dataDetail.Rows[i]["ExpiredDay"].ToString());
                    dr["IniAmount"] = dataDetail.Rows[i]["IniAmount"];
                    dr["DiscountAmount"] = dataDetail.Rows[i]["DiscountAmount"];
                    dr["DiscountPer"] = dataDetail.Rows[i]["DiscountPer"];
                    dr["UnitPrice"] = dataDetail.Rows[i]["UnitPrice"];
                    dt.Rows.Add(dr);
                }

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_v2_Product_Receipt_Insert", CommandType.StoredProcedure,
                            "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Ware_ID", SqlDbType.Int, ware,
                            "@State", SqlDbType.Int, 1,
                            "@TransferID", SqlDbType.Int, 0,
                            "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                            "@Note", SqlDbType.NVarChar, note != null ? note.Replace("'", "").Trim() : "",
                            "@NumberContract", SqlDbType.NVarChar, numberContract != null ? numberContract.Replace("'", "").Trim() : "",
                            "@NumberInvoice", SqlDbType.NVarChar, numberInvoice != null ? numberInvoice.Replace("'", "").Trim() : "",
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null,
                            "@SupplierID", SqlDbType.Int, supplierID
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_v2_Product_Receipt_Update", CommandType.StoredProcedure,
                            "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(DateExecute),
                            "@Note", SqlDbType.NVarChar, note != null ? note.Replace("'", "").Trim() : "",
                            "@NumberContract", SqlDbType.NVarChar, numberContract != null ? numberContract.Replace("'", "").Trim() : "",
                            "@NumberInvoice", SqlDbType.NVarChar, numberInvoice != null ? numberInvoice.Replace("'", "").Trim() : "",
                            "@Ware_ID", SqlDbType.Int, ware,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                            "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null,
                            "@SupplierID", SqlDbType.Int, supplierID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id, string typeid, string ware)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_Stock_Delete]", CommandType.StoredProcedure,
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
