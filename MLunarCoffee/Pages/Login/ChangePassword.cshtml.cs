using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Login
{
    public class ChangePasswordModel : PageModel
    {
        public string _userNameOfUser { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _userNameOfUser = user.sys_username;
        }

        public async Task<IActionResult> OnPostExcuteData(string OldPassword, string NewPassword, string NewPasswordConfirm)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_ChangePassword]", CommandType.StoredProcedure,
                        "@OldPassword", SqlDbType.NVarChar, OldPassword,
                        "@NewPassword", SqlDbType.NVarChar, NewPassword,
                        "@Created_By", SqlDbType.Int, user.sys_userid,
                        "@UserID", SqlDbType.Int, user.sys_userid
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