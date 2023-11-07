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

namespace MLunarCoffee.Pages.Setting
{
    public class CustomerGroupListModel : PageModel
    {

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostLoadataCustomerGroup()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("[YYY_sp_Customer_Group_LoadList]",CommandType.StoredProcedure);
                    dt.TableName="Group";
                    ds.Tables.Add(dt);
                }
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("YYY_sp_ServiceType_Load",CommandType.StoredProcedure);
                    dt.TableName="ServiceType";
                    ds.Tables.Add(dt);
                }
                if(ds!=null)
                    return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");

            }
            catch
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_Group_Delete]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,id,
                        "@userID",SqlDbType.Int,user.sys_userid
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
