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

namespace MLunarCoffee.Pages.Setting.DiseaseStatus
{
    public class DiseaseSignDetailModel : PageModel
    {
        public string _DetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DetailID = curr.ToString();
            }
            else
            {
                _DetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_DiseaseSign_LoadDetailContent]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string content, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);                
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_DiseaseSign_UpdateContent]", CommandType.StoredProcedure,
                        "@Content", SqlDbType.NVarChar, content,
                        "@UserID", SqlDbType.Int, user.sys_userid,                            
                        "@currentID ", SqlDbType.Int, CurrentID
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
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
