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
    public class TicketFile_Folder_DetailModel : PageModel
    {
        public string _TicketFileID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != "" && curr != null)
            {
                _TicketFileID = curr.ToString();
            }
            else
            {
                _TicketFileID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int FileID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Ticket_File_Folder_LoadDetail]", CommandType.StoredProcedure,
                      "@TicketFileID", SqlDbType.Int, Convert.ToInt32(FileID == 0 ? 0 : FileID));
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
                return Content("[]");
            }
        }
    }
}
