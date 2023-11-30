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
    public class StockDetailModel : PageModel
    {
        public string _DetailCurrentID { get; set; }
        public string _Type { get; set; }
        public string _ViewDetail { get; set; }
        public void OnGet()
        {
             string curr = Request.Query["CurrentID"];
            string typ = Request.Query["Type"];
            string view = Request.Query["ViewDetail"];

            _Type = typ.ToString();
            if (curr != null)
            {
                _DetailCurrentID = curr.ToString();

            }
            else
            {
                _DetailCurrentID = "0";

            }
            if (view != null)
            {
                _ViewDetail = view.ToString();

            }
            else
            {
                _ViewDetail = "0";

            }
        }

        public async Task<IActionResult> OnPostInitialize(string currentid, string type)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Get]", CommandType.StoredProcedure);
                    dt.TableName = "Product";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                    dt.TableName = "UnitProduct";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Warehouse";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Combo_Unit]", CommandType.StoredProcedure);
                    dt.TableName = "Unit";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Export_Reason]", CommandType.StoredProcedure);
                    dt.TableName = "Reason";
                    ds.Tables.Add(dt);
                }
                if (currentid != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockDetail_Info]", CommandType.StoredProcedure
                          , "@ID", SqlDbType.Int, Convert.ToInt32(currentid)
                          , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                          , "@UserID", SqlDbType.Int, user.sys_userid
                          , "@Type", SqlDbType.Int, Convert.ToInt32(type));
                        dt.TableName = "Info";
                        ds.Tables.Add(dt);
                    }
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_StockDetail_Item]", CommandType.StoredProcedure
                           , "@ID", SqlDbType.Int, Convert.ToInt32(currentid)
                           , "@Type", SqlDbType.Int, Convert.ToInt32(type));
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

        public async Task<IActionResult> OnPostLoadProduct()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Get]", CommandType.StoredProcedure);
                    dt.TableName = "Product";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                    dt.TableName = "UnitProduct";
                    ds.Tables.Add(dt);
                }
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        
        public async Task<IActionResult> OnPostLoadSupplier()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string ware, string wareto, string reason
            , string type
            , string note, string DateExecute, string CurrentID)
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
                DataTable dtResult = new DataTable();
                if (CurrentID == "0")
                {
                    if (type == "1")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Receipt_Insert", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@Ware_ID", SqlDbType.Int, ware,
                                "@TransferID", SqlDbType.Int, 0,
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }
                    }
                    if (type == "2")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Export_Insert", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@Ware_ID", SqlDbType.Int, ware,
                                "@TransferID", SqlDbType.Int, 0,
                                "@reason", SqlDbType.Int, reason,
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }
                    }
                    if (type == "3")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Transfer_Insert", CommandType.StoredProcedure,
                                "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@Ware_From", SqlDbType.Int, ware,
                                "@Ware_To", SqlDbType.Int, wareto,
                                "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                                "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                            );
                        }
                    }

                }
                else
                {
                    if (type == "1")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Receipt_Update", CommandType.StoredProcedure,
                               "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                               "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                               "@Ware_ID", SqlDbType.Int, ware,
                               "@Modified_By", SqlDbType.Int, user.sys_userid,
                               "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                               "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                           );
                        }
                    }
                    if (type == "2")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Export_Update", CommandType.StoredProcedure,
                               "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                               "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                               "@Ware_ID", SqlDbType.Int, ware,
                                "@reason", SqlDbType.Int, reason,
                               "@Modified_By", SqlDbType.Int, user.sys_userid,
                               "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                               "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                           );
                        }
                    }
                    if (type == "3")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dtResult =await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Transfer_Update", CommandType.StoredProcedure,
                               "@date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                               "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                               "@Modified_By", SqlDbType.Int, user.sys_userid,
                               "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                               "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                           );
                        }
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
