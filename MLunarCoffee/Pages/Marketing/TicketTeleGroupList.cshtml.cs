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
    public class TicketTeleGroupListModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TeleGroup_Load]", CommandType.StoredProcedure);
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
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_TeleGroup_Delete]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, id
                    );
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "1")
                        {
                            return Content("-1");//Danh Mục Này Đang Sử Dụng Ở Nhóm Tele. Vui Lòng Kiểm Tra Lại
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
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
