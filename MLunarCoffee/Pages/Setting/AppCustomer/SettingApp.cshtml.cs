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


namespace MLunarCoffee.Pages.Setting.AppCustomer
{
    public class SettingAppModel : PageModel
    {
        private string key = "Mobile";
        public string _dtCurrentList { get; set; }
        public string layout { get; set; }

        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

        public async Task<IActionResult> OnPostLoadList()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();


                string _dtPermissionCurrentPage = HttpContext.Request.Path;
                dt = Comon.Permission.Get_Control_IsShow_ByCurrentpage(
                    Comon.Permission.SelectDataFromKey(Comon.Global.sys_ListGeneral, key)
                    , Comon.Global.sys_PermissionControlMustHide_Table
                    , user.sys_PermissionControl
                    , _dtPermissionCurrentPage);
                dt.TableName = "Menu";
                ds.Tables.Add(dt);

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Setting_AC_TypeAccept", CommandType.StoredProcedure);
                    dt.TableName = "TypeAccept";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Setting_AC_IniSetting", CommandType.StoredProcedure);
                    dt.TableName = "VersionSettingApp";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));

            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostApplyVer(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                AppSetting DataMain = JsonConvert.DeserializeObject<AppSetting>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_AC_IniSetting_Apply]", CommandType.StoredProcedure
                        , "@CurrenID", SqlDbType.Int, Convert.ToInt32(DataMain.ID)
                        , "@Value", SqlDbType.NVarChar, DataMain.Value
                        , "@Modified_By", SqlDbType.Int, user.sys_userid
                        );
                }
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }
        public class AppSetting
        {
            public string ID { get; set; }
            public string Value { get; set; }
        }
    }
}
