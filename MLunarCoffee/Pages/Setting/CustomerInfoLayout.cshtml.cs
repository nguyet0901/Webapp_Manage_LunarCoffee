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
    public class CustomerInfoLayoutModel : PageModel
    {
        public string CurrentPath { get; set; }
        public string sys_CustomerInfo_ScreenSetup { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            CurrentPath = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_Setting_CustomerInfoLayout_Load]", CommandType.StoredProcedure);
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
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerInfoLayout_Update]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.NVarChar, CurrentID,
                        "@Modified_By", SqlDbType.Int, user.sys_userid
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content("1");
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteDataChild(string CurrentID,string value)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerInfoLayoutChild_Update]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.NVarChar ,CurrentID ,
                        "@value" ,SqlDbType.NVarChar ,value ,
                        "@Modified_By" ,SqlDbType.Int ,user.sys_userid
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content("1");
                    }
                    else
                    {
                        return Content("0");
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
