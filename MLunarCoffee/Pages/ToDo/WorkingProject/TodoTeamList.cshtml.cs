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

namespace MLunarCoffee.Pages.ToDo.WorkingProject
{
    public class TodoTeamListModel : PageModel
    {
        public string _dtPermissionControl { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            CurrentPath = HttpContext.Request.Path;
            _dtPermissionControl = JsonConvert.SerializeObject(
                Comon.Comon.GetControlPermissionByGroup(user.sys_PermissionControl,
                    user.sys_RoleServerID), Formatting.Indented);
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await confunc.ExecuteDataTable("[YYY_sp_ToDo_Team_LoadList]", CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                  await  connFunc.ExecuteDataTable("[YYY_sp_ToDo_Team_Delete]", CommandType.StoredProcedure,
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
