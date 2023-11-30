using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_File_DetailModel : PageModel
    {
        public string _ImportID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ImportID = curr.ToString();
            }
            else
            {
                _ImportID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int ImportID)
        {

            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Ticket_File_Files_LoadDetail]", CommandType.StoredProcedure,
                      "@ImportID", SqlDbType.Int, Convert.ToInt32(ImportID == 0 ? 0 : ImportID));
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
            catch(Exception ex)
            {
                return Content("[]");
            }
            

        }
    }
}
