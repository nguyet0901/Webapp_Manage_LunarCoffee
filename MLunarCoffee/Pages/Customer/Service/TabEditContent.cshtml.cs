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

namespace MLunarCoffee.Pages.Customer.Service
{

    public class TabEditContentModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _CustomerID { get; set; }

        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            var cus = Request.Query["CustomerID"];
            if (curr.ToString() != String.Empty)
            {
                _CurrentID = curr.ToString();
                _CustomerID = cus.ToString();
            }
            else
            {
                _CurrentID = "0";
                _CustomerID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Tab_Content_LoadDetail", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string content, string CurrentID, string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Tab_Content_Update]", CommandType.StoredProcedure,
                       "@Content", SqlDbType.NVarChar, content.Replace("'", "").Trim()
                       , "@CurrentID", SqlDbType.Int, CurrentID
                       , "@Modified_by", SqlDbType.Int, user.sys_userid);
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
