using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Permission
{
    public class PermissionPageListModel : PageModel
    {
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostLoadData()
        {

            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[YYY_sp_Permission_PageLoad]", CommandType.StoredProcedure);
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

         public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataTable dt = new DataTable();
            //LoadLanguage
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.LoadPara("Per_Page");
            }
            return Content(Comon.DataJson.Datatable(dt));
        }



         public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Permission_Page_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@Modified_By", SqlDbType.Int, user.sys_userid
                    );
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                           return Content("0");
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                    else
                    {
                        return Content("1");
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
