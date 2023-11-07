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

namespace SourceMain.Pages.Setting
{
    public class TypeHistoryListModel : PageModel
    {
        public string _dtPermissionControl { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _dtPermissionControl = JsonConvert.SerializeObject(
                Comon.Comon.GetControlPermissionByGroup(user.sys_PermissionControl,
                   user.sys_RoleServerID), Formatting.Indented);
            CurrentPath = HttpContext.Request.Path;
        }

        public  async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_TypeHistory_LoadList]", CommandType.StoredProcedure,
                  "@UserID", SqlDbType.Int, user.sys_userid);
            }
            if (dt != null)
            {
                return Content(JsonConvert.SerializeObject(dt));
            }
            else
            {
                return Content("");
            }
        }

        public  async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    //connFunc.ExecuteDataTable("[YYY_sp_Product_Unit_Delete]", CommandType.StoredProcedure,
                    //    "@CurrentID", SqlDbType.Int, id,
                    //    "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                    //    "@userID", SqlDbType.Int, user.sys_userid
                    //);
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