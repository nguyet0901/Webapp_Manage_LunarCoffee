using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.Ticket.Tag
{
    public class TicketTagDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _CurrentID = curr != null ? curr.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Setting_TicketTag_Detail", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, id);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }

            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                TagDetail DataMain = JsonConvert.DeserializeObject<TagDetail>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_TicketTag_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.VarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@MaxExcute", SqlDbType.Int, DataMain.MaxExcute,
                          "@Color", SqlDbType.VarChar, DataMain.Color.Replace("'", "").Trim(),
                          "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                          "@UserID", SqlDbType.Int, user.sys_userid);
                    }

                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_TicketTag_Update]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Name", SqlDbType.VarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@MaxExcute", SqlDbType.Int, DataMain.MaxExcute,
                            "@Color", SqlDbType.VarChar, DataMain.Color.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid);
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public class TagDetail
        {
            public string Name { get; set; }
            public int MaxExcute { get; set; }
            public string Color { get; set; }
            public string Note { get; set; }
        }
    }
}
