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
    public class TicketDeleteModel : PageModel
    {
        public string _TicketDeleteID { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["TicketID"];
            if (curr.ToString() != null)
            {
                _TicketDeleteID = curr.ToString();
            }
            else
            {
                _TicketDeleteID = "0";

            }

        }

         public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_ReasonDelete_LoadCombo]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }


         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                TicketDetailDelete DataMain = JsonConvert.DeserializeObject<TicketDetailDelete>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Delete]", CommandType.StoredProcedure,
                        "@Current_ID", SqlDbType.Int, CurrentID,
                         "@ReasonDelete_ID", SqlDbType.Int, DataMain.StatusCall_ID,
                         "@Modified_by", SqlDbType.Int, user.sys_userid,
                         "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                     );
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "0")
                        {
                            return Content("0");//Chỉ Người Phụ Trách Mới Được Xóa. Vui Lòng Kiểm Tra Lại
                        }
                        else if (dt.Rows[0][0].ToString() == "-1")
                        {
                            return Content("-1");//Ticket Là Khách Hàng , Phát Sinh Dữ Liệu Không Thể Xóa
                        }
                        else return Content("1");
                    }
                    else
                    {
                        return Content("0");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
    public class TicketDetailDelete
    {
        public string StatusCall_ID { get; set; }
        public string Note { get; set; }

    }
}
