using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_MasterModel : PageModel
    {
        public string CurrentPath { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
            CurrentPath = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostLoadTotalSeenFile(int UserID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Master_TotalSeen]", CommandType.StoredProcedure
                      , "@UserID", SqlDbType.Int, UserID);
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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
    }
}
