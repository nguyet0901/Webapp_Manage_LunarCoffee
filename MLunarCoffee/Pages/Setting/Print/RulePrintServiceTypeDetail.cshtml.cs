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

namespace MLunarCoffee.Pages.Setting.Print
{
    public class RulePrintServiceTypeDetailModel : PageModel
    {
        public string _SerTypeID { get; set; }
        public void OnGet()
        {
            string Curr = Request.Query["CurrentID"];
            if (Curr != null)
            {
                _SerTypeID = Curr.ToString();
            }
            else {
                _SerTypeID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadInit()
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
                        DataTable SerType = new DataTable();
                        SerType = await confunc.ExecuteDataTable("YYY_sp_v2_ServiceType_LoadCombo", CommandType.StoredProcedure);
                        SerType.TableName = "ServiceType";
                        return SerType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable StogeRule = new DataTable();
                        StogeRule = await confunc.ExecuteDataTable("YYY_sp_StogeRule_LoadCombo", CommandType.StoredProcedure);
                        StogeRule.TableName = "StogeRule";
                        return StogeRule;
                    }
                }));


                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(int ID)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_RulePrintServiceType_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(ID == 0 ? 0 : ID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostExecuted(string SerTypeID, string StoreRule)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (SerTypeID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_RulePrintServiceType_Update", CommandType.StoredProcedure
                            , "@SerTypeID", SqlDbType.Int, Convert.ToInt32(SerTypeID)
                            , "@StoreRule", SqlDbType.NVarChar, StoreRule
                        );
                    }
                    return Content("1");
                }
                else {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
