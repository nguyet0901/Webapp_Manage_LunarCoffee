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

namespace MLunarCoffee.Pages.Account.PaymentSupplier
{
    public class PaymentList : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

        public async Task<IActionResult> OnPostLoadIni()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "Warehouse";
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
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostLoadData(string supID, string wareID, string wareToken, int limit, int beginID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {


                    dt = await connFunc.ExecuteDataTable("MLU_sp_Account_SupplierList", CommandType.StoredProcedure
                        , "@supID", SqlDbType.Int, Convert.ToInt32(supID)
                        , "@wareid", SqlDbType.Int, Convert.ToInt32(wareID)
                        , "@wareToken", SqlDbType.NVarChar, wareToken != null ? wareToken : ""
                        , "@limit", SqlDbType.Int, limit
                        , "@beginID", SqlDbType.Int, beginID);
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
            catch
            {
                return Content("0");
            }

        }
    }
}
