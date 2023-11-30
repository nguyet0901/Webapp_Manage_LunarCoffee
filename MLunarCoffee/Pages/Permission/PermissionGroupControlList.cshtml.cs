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

namespace MLunarCoffee.Pages.Permission
{
    public class PermissionGroupControlListModel : PageModel
    {
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.LoadPara("Per_Page");
                dt.TableName = "_dataGroupPage";
                ds.Tables.Add(dt);
            }

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("MLU_sp_Permission_LoadAllPage", CommandType.StoredProcedure);
                dt.TableName = "_dataPage";
                ds.Tables.Add(dt);
            }

            //LoadLanguage
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("MLU_sp_User_Group_LoadList", CommandType.StoredProcedure);
                dt.TableName = "_dataGroup";
                ds.Tables.Add(dt);
            }

            return Content(Comon.DataJson.Dataset(ds));

        }

       
         public async Task<IActionResult> OnPostLoadDataGroupControlList()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[MLU_sp_Permission_GroupControl]", CommandType.StoredProcedure);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
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
                    DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Group_Control_delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@Modified_By", SqlDbType.Int, user.sys_userid
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
