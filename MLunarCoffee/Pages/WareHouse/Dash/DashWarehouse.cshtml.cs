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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.WareHouse.Dash
{
 
    public class DashWarehouse : PageModel {
        public string layout { get; set; }
        public void OnGet ( ) {
            string _layout = Request.Query["layout"];
            if(_layout!=null)
            {
                layout=_layout;
            }
            else layout="";
        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]",CommandType.StoredProcedure
                    ,"@UserID",SqlDbType.Int,user.sys_userid
                    ,"@LoadNormal",SqlDbType.Int,1);
                    dt.TableName="Warehouse";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
