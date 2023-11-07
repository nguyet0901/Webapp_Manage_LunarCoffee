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
    public class TicketTeleGroupDetailModel : PageModel
    {
        public string _TeleGroupDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TeleGroupDetailID = curr.ToString();
            }
            else
            {
                _TeleGroupDetailID = "0";
            }
        }

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Ticket_TeleGroup_LoadDetails]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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

         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                TicketTeleGroup DataMain = JsonConvert.DeserializeObject<TicketTeleGroup>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_TeleGroup_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");//Danh Mục Nhóm Tele Đã Tồn Tại Vui Lòng Kiểm Tra Lại
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_TeleGroup_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@CurrentID", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");//Danh Mục Nhóm Tele Đã Tồn Tại Vui Lòng Kiểm Tra Lại
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class TicketTeleGroup
    {
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
