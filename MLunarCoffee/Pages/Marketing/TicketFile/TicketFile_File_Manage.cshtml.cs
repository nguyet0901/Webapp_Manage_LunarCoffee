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
    public class TicketFile_File_ManageModel : PageModel
    {
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostLoadTicketImportList(string date, string sourceid, string beginID, string beginFileID, string limit)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Ticket_File_Files_LoadData", CommandType.StoredProcedure
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@sourceid", SqlDbType.Int, sourceid
                      , "@BeginID", SqlDbType.Int, beginID
                      , "@BeginFileID", SqlDbType.Int, beginFileID
                      , "@Limit", SqlDbType.Int, limit
                      , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                      , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
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

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                    dt.TableName = "Source";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }


        }

        public async Task<IActionResult> OnPostDeleteItem(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Files_Delete]", CommandType.StoredProcedure
                      , "@Current", SqlDbType.Int, Convert.ToInt32(id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
