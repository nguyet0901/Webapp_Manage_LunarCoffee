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
    public class SettingAppointmentDotorModel : PageModel
    {
        public string sys_Setting_Appointment_Doctor { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostLoadData(string data, string CurrentID)
        {
            try
            {
                return Content("0");//Content(Global.sys_Setting_Appointment_Doctor);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentTypeOption)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Option_Update_By_Type]", CommandType.StoredProcedure,
                     "@Modified_By", SqlDbType.Int, user.sys_userid,
                      "@Type", SqlDbType.NVarChar, CurrentTypeOption,
                      "@Value ", SqlDbType.NVarChar, data
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
