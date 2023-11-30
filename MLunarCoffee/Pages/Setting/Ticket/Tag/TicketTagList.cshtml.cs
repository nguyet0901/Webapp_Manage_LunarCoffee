using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.Ticket.Tag
{
    public class TicketTagListModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Setting_TicketTag_LoadList", CommandType.StoredProcedure);
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
            catch(Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostDelete(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Setting_TicketTag_Delete]", CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
