using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Store
{
    public class TicketWebsiteModel : PageModel
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

         public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt1 =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadTele]", CommandType.StoredProcedure,
                      "@UserID", SqlDbType.Int, user.sys_userid);
                    dt1.TableName = "dataTele";
                    ds.Tables.Add(dt1);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt1 =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_Website_LoadList]", CommandType.StoredProcedure);
                    dt1.TableName = "dataList";
                    ds.Tables.Add(dt1);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


         public async Task<IActionResult> OnPostExecute(string data)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                dt.Columns.Add("TicketWebsiteID", typeof(String));
                dt.Columns.Add("TeleID", typeof(String));
                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["TicketWebsiteID"] = DataMain.Rows[i]["ID"].ToString();
                    dr["TeleID"] = DataMain.Rows[i]["Tele"].ToString();
                    dt.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("[MLU_sp_Ticket_Website_To_Ticket]", CommandType.StoredProcedure,
                          "@Created_By", SqlDbType.Int, user.sys_userid,
                          "@Table", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                      );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

         public async Task<IActionResult> OnPostDeleteTicket(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Ticket_Website_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@UserID", SqlDbType.Int, user.sys_userid
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
