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

namespace MLunarCoffee.Pages.Customer.Service.TreatmentPlan
{

    public class TreatmentPlanDetailModel : PageModel
    {

        public string _CurrentID { get; set; }

        public void OnGet()
        {
            var v = Request.Query["CurrentID"];
            _CurrentID = v.ToString();
        }
        public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Tab_Treatment_Plan_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, CurrentID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string name, string note)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Tab_Treatment_Plan_Update]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@Name", SqlDbType.NVarChar, name,
                        "@Note", SqlDbType.NVarChar, note,
                        "@Created_By", SqlDbType.Int, user.sys_userid);
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
