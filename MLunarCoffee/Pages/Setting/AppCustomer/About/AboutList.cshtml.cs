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

namespace MLunarCoffee.Pages.Setting.AppCustomer.About
{
    public class AboutListModel : PageModel
    {
        public async Task<IActionResult> OnPostLoadData(int ID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Setting_AC_About_LoadList]", CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteAboutUs(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                AboutUs DataMain = JsonConvert.DeserializeObject<AboutUs>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_About_Update]", CommandType.StoredProcedure,
                    "@Content", SqlDbType.NVarChar, DataMain.Content,
                    "@UserID", SqlDbType.Int, user.sys_userid,
                    "@CurrentID ", SqlDbType.Int, DataMain.ID);
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_AboutUsPerson_Delete]", CommandType.StoredProcedure,
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
    public class AboutUs
    {
        public string ID { get; set; }
        public string Content { get; set; }
    }
}

