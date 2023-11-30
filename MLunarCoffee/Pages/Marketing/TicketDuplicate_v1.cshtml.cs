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
    public class TicketDuplicate_v1Model : PageModel
    {
        public string _TicketID { get; set; }
        public string _Phone { get; set; }
        public string _TicketDuplicate { get; set; }
        public void OnGet()
        {
            string TicketID = Request.Query["TicketID"];
            string Phone = Request.Query["Phone"];

            _TicketID = "0";
            _Phone = "";

            if (TicketID != null && Phone != null)
            {
                _TicketID = TicketID;
                _Phone = Phone;

            }
        }


         public async Task<IActionResult> OnPostLoadata(string phone)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_List_Duplicate_V1]", CommandType.StoredProcedure,
                        "@Phone", SqlDbType.NVarChar, phone
                    );
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
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
