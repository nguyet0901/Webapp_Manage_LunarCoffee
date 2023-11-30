using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.WareHouse.Require
{
    public class RequireDetailModel : PageModel
    {
        public string _DetailCurrentID { get; set; }
        public string _WareID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public RequireDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string ware = Request.Query["WareID"];
            if (curr != null) _DetailCurrentID = curr.ToString(); else _DetailCurrentID = "0";
            if (ware != null) _WareID = ware.ToString(); else _WareID = "0";
        }

        public async Task<IActionResult> OnPostInitialize(string currentid, string ware)
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
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "User";
                    ds.Tables.Add(dt);
                }

                if (currentid != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_OrderDetail_Info]", CommandType.StoredProcedure
                          , "@ID", SqlDbType.Int, Convert.ToInt32(currentid)
                          , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                          , "@UserID", SqlDbType.Int, user.sys_userid);
                        dt.TableName = "Info";
                        ds.Tables.Add(dt);
                    }
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_OrderDetail_Item]", CommandType.StoredProcedure
                           , "@ID", SqlDbType.Int, Convert.ToInt32(currentid));
                        dt.TableName = "Items";
                        ds.Tables.Add(dt);
                    }
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
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string ware, string userto, string note, string CurrentID)
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
                dt.Columns.Add("Amount", typeof(decimal));
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
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Order_Insert", CommandType.StoredProcedure,
                            "@UserFrom", SqlDbType.Int, user.sys_userid,
                            "@UserTo", SqlDbType.Int, userto,
                            "@WareID", SqlDbType.Int, ware,
                            "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("MLU_sp_v2_Product_Order_Update", CommandType.StoredProcedure,
                            "@Note", SqlDbType.NVarChar, note.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@UserTo", SqlDbType.Int, userto,
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                            "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
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
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Order_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
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
