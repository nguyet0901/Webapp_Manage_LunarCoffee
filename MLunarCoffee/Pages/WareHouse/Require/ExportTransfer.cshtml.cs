using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.WareHouse.Require
{
    public class ExportTransferModel : PageModel
    {
        public string _Detail { get; set; }
        public void OnGet()
        {
            string Detail = Request.Query["Detail"];
            _Detail = (Detail == null) ? "0" : Detail.ToString();
        }

        public async Task<IActionResult> OnPostGetInfo(int DetailID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_WareHouse_ExportTransfer_Info", CommandType.StoredProcedure
                    , "@OrderWareID", SqlDbType.Int, DetailID);
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

        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data, string ware, string DateExecute)
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
                    dr["PackageNumber"] = dataDetail.Rows[i]["package"];
                    dr["ProductID"] = dataDetail.Rows[i]["productID"];
                    dr["Number"] = dataDetail.Rows[i]["number"];
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("MLU_sp_Product_Stock_Quantity_RealTime", CommandType.StoredProcedure,
                        "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@WareID", SqlDbType.Int, Convert.ToInt32(ware),
                        "@MasterID", SqlDbType.Int, 0,
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

        public async Task<IActionResult> OnPostExcuteData(string data, string DateExecute, string IsPackageNumber, string CurrentID, string IntAmount)
        {
            try
            {

                DataTable DataExport = new DataTable();
                DataExport.Columns.Add("productID", typeof(Int32));
                DataExport.Columns.Add("number", typeof(Decimal));
                DataExport.Columns.Add("unitID", typeof(Int32));
                DataExport.Columns.Add("package", typeof(string));
                DataExport.Columns.Add("expiredDay", typeof(DateTime));
                DataExport.Columns.Add("price", typeof(Decimal));
                DataExport.Columns.Add("amount", typeof(Decimal));

                DataTable _DataExport = JsonConvert.DeserializeObject<DataTable>(data);

                for (int i = 0; i < _DataExport.Rows.Count; i++)
                {
                    DataRow dr = DataExport.NewRow();
                    dr["productID"] = Convert.ToInt32(_DataExport.Rows[i]["productID"]);
                    dr["number"] = Convert.ToDecimal(_DataExport.Rows[i]["number"]);
                    dr["unitID"] = Convert.ToInt32(_DataExport.Rows[i]["unitID"]);
                    dr["package"] = _DataExport.Rows[i]["package"];
                    dr["expiredDay"] = Comon.Comon.DateTimeDMY_To_YMD(_DataExport.Rows[i]["expiredDay"].ToString());
                    dr["price"] = Convert.ToDecimal(_DataExport.Rows[i]["price"]);
                    dr["amount"] = Convert.ToDecimal(_DataExport.Rows[i]["amount"]);
                    DataExport.Rows.Add(dr);
                }
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_WareHouse_ExportTransfer_ImportWare", CommandType.StoredProcedure
                        , "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute)
                        , "@OrderWareID", SqlDbType.Int, CurrentID
                        , "@IsPackageNumber", SqlDbType.Int, IsPackageNumber
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@dt", SqlDbType.Structured, DataExport.Rows.Count > 0 ? DataExport : null
                        , "@IntAmount", SqlDbType.Decimal, Convert.ToDecimal(IntAmount)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostUpdateSign(int id, string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_SignExportOrder]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(id),
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Sign", SqlDbType.NVarChar, sign
                    );
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
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_ExportWare_Delete]", CommandType.StoredProcedure,
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
