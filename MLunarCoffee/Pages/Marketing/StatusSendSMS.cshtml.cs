using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class StatusSendSMSModel : PageModel
    {
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

         public async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
          var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt =await connFunc.ExecuteDataTable("[MLU_sp_SMS_MultiMaster_Load]", CommandType.StoredProcedure,
                     "@userid", SqlDbType.Int, user.sys_userid
                    );
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

         public async Task<IActionResult> OnPostLoadDataDetail(string CurrentID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt =await connFunc.ExecuteDataTable("[MLU_sp_SMS_MultiDetail_Load]", CommandType.StoredProcedure,
                    "@masterID", SqlDbType.Int, CurrentID);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

    }
}
