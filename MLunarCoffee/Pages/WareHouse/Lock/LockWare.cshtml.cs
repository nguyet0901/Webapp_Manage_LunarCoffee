using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Lock
{
    public class LockWareModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }
        public async Task<IActionResult> OnPostInitialize()
        {
            DataSet ds = new DataSet();
            //LoadWareHouse
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                dt.TableName = "Ware";
                ds.Tables.Add(dt);
            }

            return Content(Comon.DataJson.Dataset(ds));
        }

        public async Task<IActionResult> OnPostLoadLockList(string WareID, string lockid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[MLU_sp_WareHouse_Lock_Load]", CommandType.StoredProcedure,
                      "@UserID", SqlDbType.Int, user.sys_userid,
                      "@lockid", SqlDbType.Int, lockid,
                      "@WareID", SqlDbType.NVarChar, WareID,
                      "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
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
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostCheckButton(string WareID)
        {
            //MLU_sp_WareHouse_Get_Info
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Norm_CheckButton]", CommandType.StoredProcedure,
                  "@WareID", SqlDbType.NVarChar, WareID);
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
    }
}
