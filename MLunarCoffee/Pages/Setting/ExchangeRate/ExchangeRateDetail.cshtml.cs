using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ExchangeRate
{
    public class ExchangeRateDetailModel : PageModel
    {
        public string _exchangeRateDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _exchangeRateDetailID = curr.ToString();
            }
            else
            {
                _exchangeRateDetailID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ExchangeRate_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }


        }


        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                ExchangeRate DataMain = JsonConvert.DeserializeObject<ExchangeRate>(data);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_ExchangeRate_Update]", CommandType.StoredProcedure,
                            "@ExchangeRate", SqlDbType.NVarChar, DataMain.Exchange_Rate,
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
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
        public class ExchangeRate
        {
            public decimal Exchange_Rate { get; set; }
        }
    }
}
