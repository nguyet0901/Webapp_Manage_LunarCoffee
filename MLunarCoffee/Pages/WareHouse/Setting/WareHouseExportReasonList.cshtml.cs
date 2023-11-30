using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class WareHouseExportReasonListModel : PageModel
    {
        public void OnGet()
        { 
        }

        public async Task<IActionResult> OnPostLoadData() { 
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await confunc.ExecuteDataTable("[MLU_sp_WareHouse_PrExportReason_Load]", CommandType.StoredProcedure,
                  "@UserID", SqlDbType.Int, user.sys_userid);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }
        }
         
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                   await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_PrExportReason_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
