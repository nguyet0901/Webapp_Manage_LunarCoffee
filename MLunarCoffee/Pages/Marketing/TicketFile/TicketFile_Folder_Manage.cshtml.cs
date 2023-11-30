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
    public class TicketFile_Folder_ManageModel : PageModel
    {
        public int _UserTeleID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _UserTeleID = Convert.ToInt32(user.sys_userid);
        }


         public async Task<IActionResult> OnPostLoadData(string user)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Folder_LoadData]", CommandType.StoredProcedure
                        , "@User", SqlDbType.Int, user);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

         public async Task<IActionResult> OnPostUpdate_SeenFile(int FileID, int IsSeen)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Folder_UpdateSeen]", CommandType.StoredProcedure
                      , "@FileID", SqlDbType.Int, FileID
                      , "@IsSeen", SqlDbType.Int, IsSeen);
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
                return Content("0");
            }
        }

         public async Task<IActionResult> OnPostUpdate_Name(int FileID, string FileName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Folder_UpdateName]", CommandType.StoredProcedure
                      , "@FileID", SqlDbType.Int, FileID
                      , "@FileName", SqlDbType.NVarChar, FileName);
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
