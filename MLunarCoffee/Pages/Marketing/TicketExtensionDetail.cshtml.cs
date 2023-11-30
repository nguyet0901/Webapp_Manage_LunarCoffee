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
    public class TicketExtensionDetailModel : PageModel
    {
        public string _TeleExtensionDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TeleExtensionDetailID = curr.ToString();
            }
            else
            {
                _TeleExtensionDetailID = "0";
            }
        }

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Ticket_Extension_LoadDetails]", CommandType.StoredProcedure,
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
                TicketExtension DataMain = JsonConvert.DeserializeObject<TicketExtension>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Ticket_Extension_Insert]", CommandType.StoredProcedure,
                          "@Extension", SqlDbType.NVarChar, DataMain.Extension.Replace("'", "").Trim(),
                          "@Password", SqlDbType.NVarChar, DataMain.Password.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");//Extension Đã Tồn Tại Vui Lòng Kiểm Tra Lại
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Ticket_Extension_Update]", CommandType.StoredProcedure,
                            "@Extension", SqlDbType.NVarChar, DataMain.Extension.Replace("'", "").Trim(),
                            "@Password", SqlDbType.NVarChar, DataMain.Password.Replace("'", "").Trim(),
                            "@CurrentID", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");//Extension Đã Tồn Tại Vui Lòng Kiểm Tra Lại
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
    public class TicketExtension
    {
        public string Extension { get; set; }
        public string Password { get; set; }
    }
}
