using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.WareHouse.Stock
{
    public class StockTransferDetailModel : PageModel
    {
        public string _DetailCurrentID { get; set; }
        public string _WareID { get; set; }
        public string _ViewDetail { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string view = Request.Query["ViewDetail"];
            string ware = Request.Query["WareID"];
            if (curr != null) _DetailCurrentID = curr.ToString(); else _DetailCurrentID = "0";
            if (view != null) _ViewDetail = view.ToString(); else _ViewDetail = "0";
            if (ware != null) _WareID = ware.ToString(); else _WareID = "0";
        }

        public async Task<IActionResult> OnPostInitialize(string currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
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
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Warehouse";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_Combo_Unit]", CommandType.StoredProcedure);
                    dt.TableName = "Unit";
                    ds.Tables.Add(dt);
                }
                if (currentid != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockTransferDetail_Info]", CommandType.StoredProcedure
                          , "@ID", SqlDbType.Int, Convert.ToInt32(currentid)
                          , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                          , "@UserID", SqlDbType.Int, user.sys_userid);
                        dt.TableName = "Info";
                        ds.Tables.Add(dt);
                    }
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockTransferDetail_Item]", CommandType.StoredProcedure
                           , "@ID", SqlDbType.Int, Convert.ToInt32(currentid));
                        dt.TableName = "Items";
                        ds.Tables.Add(dt);
                    }
                }
                return Content(JsonConvert.SerializeObject(ds));

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
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockSearch]", CommandType.StoredProcedure
                        , "@PackageNumber", SqlDbType.NVarChar, PackageNumber
                        , "@WareID", SqlDbType.Int, WareID);
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadStockQuantity(string masterID, string productID, string wareID, string dateExecute)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_Stock_Quantity]", CommandType.StoredProcedure
                        , "@ExportID", SqlDbType.Int, masterID
                        , "@ProductID", SqlDbType.Int, productID
                        , "@WareID", SqlDbType.Int, wareID
                        , "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateExecute));
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadStockPackNumQuantity(string masterID, string packageNumber, string wareID, string dateExecute)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Product_StockPackNum_Quantity]", CommandType.StoredProcedure
                        , "@ExportID", SqlDbType.Int, masterID
                        , "@PackageNumber", SqlDbType.Int, packageNumber
                        , "@WareID", SqlDbType.Int, wareID
                        , "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateExecute));
                }
                return Content(JsonConvert.SerializeObject(dt));
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
                    ds = await connFunc.ExecuteDataSet("YYY_sp_Product_Stock_Quantity_RealTime", CommandType.StoredProcedure,
                        "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@WareID", SqlDbType.Int, ware,
                        "@MasterID", SqlDbType.Int, CurrentID,
                        "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                    );
                }
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string ware,string wareto, string note, string DateExecute, string CurrentID)
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
                dt.Columns.Add("DiscountPer", typeof(int));
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
                    dr["Amount"] = 0;
                    dr["ExpiredDay"] = Comon.Comon.DateTimeDMY_To_YMD(dataDetail.Rows[i]["ExpiredDay"].ToString());
                    dr["IniAmount"] = 0;
                    dr["DiscountAmount"] = 0;
                    dr["DiscountPer"] = 0;
                    dr["UnitPrice"] = 0;
                    dt.Rows.Add(dr);
                }

                DataTable dtResult = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_v2_Product_Transfer_Insert", CommandType.StoredProcedure,
                            "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                            "@Created_by", SqlDbType.Int, user.sys_userid,
                            "@Ware_From", SqlDbType.Int, ware,
                            "@Ware_To", SqlDbType.Int, wareto,
                            "@note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_v2_Product_Transfer_Update", CommandType.StoredProcedure,
                            "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                            "@note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                        );
                    }

                }
                return Content(dtResult.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
