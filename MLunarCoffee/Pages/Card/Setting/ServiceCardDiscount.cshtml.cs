using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Card.Setting
{
    public class ServiceCardDiscountModel : PageModel
    {
        public string _CardID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _CardID = curr.ToString();
            else _CardID = "0";
        }

        public async Task<IActionResult> OnPostLoadInitialize(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Discount_Service_Type_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "ServiceType";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Discount_Service_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "Service";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Setting_Service_Card_LoadRule", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                    dt.TableName = "Detail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExecute(int CurrentID, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Service_Card_UpdateRule]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@Rule", SqlDbType.NVarChar, data,
                        "@Type", SqlDbType.NVarChar, "DISCOUNT"
                    );
                }
                if(dt != null)
                    return Content(Comon.DataJson.GetFirstValue(dt));
                return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
