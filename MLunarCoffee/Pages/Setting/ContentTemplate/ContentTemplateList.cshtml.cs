using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ContentTemplate
{
    public class ContentTemplateListModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadData(int MasterID)
        {

            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Content_Template_V2_Load", CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostDeleteMaster(int MasterID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Content_Master_Delete]", CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@MasterID", SqlDbType.Int, MasterID,
                        "@Modified", SqlDbType.Int, DateTime.Now
                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteDetail(int DetailID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Content_Detail_Delete]", CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@DetailID", SqlDbType.Int, DetailID,
                        "@Modified", SqlDbType.Int, DateTime.Now
                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
