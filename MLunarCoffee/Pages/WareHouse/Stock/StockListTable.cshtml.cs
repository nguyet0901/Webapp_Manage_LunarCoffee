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
    public class StockListTableModel : PageModel
    {
        public string _ViewOnly { get; set; }
        public void OnGet() {
            string ViewOnly = Request.Query["ViewOnly"];
            if (ViewOnly != null)
            {
                _ViewOnly = ViewOnly;
            }
            else
            {
                _ViewOnly = "0";
            }
        }
        public  async Task<IActionResult> OnPostLoadData(string WareHouseID, string DetailID, string BeginID, string Limit, string date, string TypeID)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Product_Stock_LoadList]", CommandType.StoredProcedure,
                      "@UserID", SqlDbType.Int, user.sys_userid,
                      "@WareHouseID", SqlDbType.NVarChar, WareHouseID,
                      "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                      "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                      "@BeginID", SqlDbType.BigInt, Convert.ToInt64(BeginID),
                      "@Limit", SqlDbType.Int, Limit,
                      "@DetailID", SqlDbType.Int, Convert.ToInt64(DetailID),
                      "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate,
                      "@Type", SqlDbType.Int, Convert.ToInt32(TypeID));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
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

        public  async Task<IActionResult> OnPostDeleteItem(int id, string typeid, string warefrom, string wareto)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Product_Stock_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Type", SqlDbType.Int, Convert.ToInt32(typeid),
                        "@WareFromID", SqlDbType.Int, Convert.ToInt32(warefrom),
                        "@WareToID", SqlDbType.Int, Convert.ToInt32(wareto)
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public  async Task<IActionResult> OnPostUpdateSign(int id, string sign, string typeid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Product_Stock_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(id),
                        "@userID", SqlDbType.Int, user.sys_userid,
                         "@Sign", SqlDbType.NVarChar, sign,
                         "@Type", SqlDbType.Int, Convert.ToInt32(typeid)
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
