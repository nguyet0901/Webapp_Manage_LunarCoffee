using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Myinfo
{
    public class MySetting : PageModel
    {
 
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostGetData()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds=await connFunc.ExecuteDataSet("[YYY_sp_UserSetting_GetDataIni]",CommandType.StoredProcedure
                        ,"@UserID",SqlDbType.Int,user.sys_userid);

                }
                return Content(Comon.DataJson.Dataset(ds));

            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateSetting(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_UserSetting_Update]",CommandType.StoredProcedure,
                        "@userID",SqlDbType.Int,user.sys_userid,
                         "@UsetSetting",SqlDbType.NVarChar,data
                    );
                }
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
