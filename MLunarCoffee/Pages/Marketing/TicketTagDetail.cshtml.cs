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
    public class TicketTagDetailModel : PageModel
    {
        public string _TicketID { get; set; }
        public void OnGet()
        {
            string tic = Request.Query["TicketID"];
            _TicketID = tic != null ? tic.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadInit(int TicketID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_Tag]", CommandType.StoredProcedure);
                    dt.TableName = "ComboTag";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TagByTicket]", CommandType.StoredProcedure,
                           "@TicketID", SqlDbType.Int, TicketID);
                    dt.TableName = "Detail";
                    ds.Tables.Add(dt);
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
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string TicketID, string TagID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (TicketID != null)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Ticket_TagUpdate", CommandType.StoredProcedure,
                            "@TicketID", SqlDbType.Int, TicketID,
                            "@TagID", SqlDbType.Int, TagID,
                            "@UserID", SqlDbType.Int, user.sys_userid
                        );
                    }
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
