using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_Card_UpdatePriceModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CardID { get; set; }
        public string _Type { get; set; }

        public void OnGet()
        {
            _CustomerID = Request.Query["CustomerID"].ToString();
            _CardID = Request.Query["CurrentID"].ToString();
            _Type = Request.Query["Type"].ToString();
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_LoadCombo_Card_CashbackReason]", CommandType.StoredProcedure);
                        dt.TableName = "ReasonCashback";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach(var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data, string CustomerID, string CardID)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Card_UpdatePriceUse]", CommandType.StoredProcedure
                        , "@AmountEdit", SqlDbType.Int, DataMain.Rows[0]["AmountEdit"]
                        , "@Reason", SqlDbType.Int, DataMain.Rows[0]["Reason"]
                        , "@IsPlus", SqlDbType.Int, DataMain.Rows[0]["IsPlus"]
                        , "@AmountCard", SqlDbType.Int, DataMain.Rows[0]["AmountCard"]
                        , "@CustomerID", SqlDbType.Int, CustomerID
                        , "@CardID", SqlDbType.Int, Convert.ToInt32(CardID)
                        , "@Modified_By", SqlDbType.Int, user.sys_userid);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
    }
}
