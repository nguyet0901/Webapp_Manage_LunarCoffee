using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Card.Status
{
    public class CardList_EditModel : PageModel
    {
        public string _CardCustomerID { get; set; }
        public string _CardCurrentID { get; set; }
        public string _DataFrom { get; set; }
        public string _DateTo { get; set; }

        public void OnGet()
        {
            string CardCustomerID = Request.Query["CustomerID"];
            string CardCurrentID = Request.Query["CurrentID"];
            string DataFrom = Request.Query["DateFrom"];
            string DateTo = Request.Query["DateTo"];
            _CardCustomerID = CardCustomerID != null ? CardCustomerID.ToString() : "";
            _CardCurrentID = CardCurrentID != null ? CardCurrentID.ToString() : "";
            _DataFrom = DataFrom != null ? DataFrom.ToString() : "";
            _DateTo = DateTo != null ? DateTo.ToString() : "";
        }
        public async Task<IActionResult> OnPostLoadInit(string customerid)
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
                        DataTable dtCashBackReason = new DataTable();
                        dtCashBackReason = await connFunc.ExecuteDataTable("[YYY_LoadCombo_Card_CashbackReason]", CommandType.StoredProcedure);
                        dtCashBackReason.TableName = "ReasonCashback";
                        return dtCashBackReason;
                    }
                }));
                 tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_Card_LoadTempSMS]", CommandType.StoredProcedure
                             ,"@BranchID", SqlDbType.Int, Convert.ToInt32(user.sys_branchID)
                             ,"@CustomerID", SqlDbType.Int, Convert.ToInt32(customerid));
                        dt.TableName = "SMS";
                        return dt;
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

        public async Task<IActionResult> OnPostLoadData(int CurrentID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_CustomerCard_StatusEdit_Load]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID)
                        , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data, string CustomerID, string CurrentID)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    ds = await conFunc.ExecuteDataSet("[YYY_sp_Customer_Card_InStatus_Update]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        , "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                        , "@Modified_By", SqlDbType.Int, user.sys_userid
                        , "@EndLess", SqlDbType.Int, DataMain.Rows[0]["EndLess"]
                        , "@NumExpired", SqlDbType.Int, DataMain.Rows[0]["NumExpired"]
                        , "@IsTimesUsed", SqlDbType.Int, DataMain.Rows[0]["IsTimesUsed"]
                        , "@TimesUsed", SqlDbType.Int, DataMain.Rows[0]["TimesUsed"]
                        , "@PriceUse", SqlDbType.Decimal, DataMain.Rows[0]["PriceUse"]
                        , "@Note", SqlDbType.NVarChar, DataMain.Rows[0]["Note"]
                        , "@IsEditPriceUse", SqlDbType.Int, DataMain.Rows[0]["IsEditPriceUse"]
                        , "@PriceUpdate", SqlDbType.Int, DataMain.Rows[0]["PriceUpdate"]
                        , "@Reason", SqlDbType.Int, DataMain.Rows[0]["Reason"]
                        , "@IsPlus", SqlDbType.Int, DataMain.Rows[0]["IsPlus"]
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
