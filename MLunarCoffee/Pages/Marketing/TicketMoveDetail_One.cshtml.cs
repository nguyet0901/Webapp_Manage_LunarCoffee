using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketMoveDetail_OneModel : PageModel
    {
        public string _TicketDetailID { get; set; }
        public void OnGet()
        {
            _TicketDetailID = Request.Query["TicketID"].ToString();
        }

        public async Task<IActionResult> OnPostInitializeCombo(int ticketid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadTeleMove]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "Tele";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_GetStaffID]", CommandType.StoredProcedure
                         , "@ID", SqlDbType.Int, ticketid);
                    dt.TableName = "Staff";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Check_Tele_Permission]", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "PerTele";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string userTo, string reason, string ticketid)
        {

            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_Move_Insert_One]", CommandType.StoredProcedure
                        ,"@ticketid", SqlDbType.NVarChar, Convert.ToInt32(ticketid)
                        ,"@userTo", SqlDbType.NVarChar, Convert.ToInt32(userTo)
                        ,"@reason", SqlDbType.Int, Convert.ToInt32(reason)
                        ,"@Created_By", SqlDbType.Int, user.sys_userid);
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
