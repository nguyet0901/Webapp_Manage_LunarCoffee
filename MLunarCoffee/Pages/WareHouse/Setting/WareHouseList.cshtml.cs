using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class WareHouseList : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostWareHouseLoadList()
        {
            try {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("MLU_sp_v2_WareHouse_LoadList", CommandType.StoredProcedure,
                    "@UserID", SqlDbType.Int, user.sys_userid);
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[MLU_sp_LoadCombo_Branch_WareHouse]", CommandType.StoredProcedure);
                    ds.Tables.Add(dt);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("0");
                }

            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                   dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_WareHouse_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
