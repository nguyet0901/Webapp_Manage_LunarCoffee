using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketMoveModel : PageModel
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

        public async Task<IActionResult> OnPostLoadTicketMove(string dateFrom)
        {
            try
            {
                string From = "";
                string To = "";
                if (dateFrom.Contains(" to "))
                {
                    dateFrom = dateFrom.Replace(" to ", "@");
                    From = dateFrom.Split('@')[0];
                    To = dateFrom.Split('@')[1];
                }
                else
                {
                    From = dateFrom;
                    To = dateFrom;
                }
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_TicketMove_Load]", CommandType.StoredProcedure
                      , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(From)
                      , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(To));
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
    }
}
