using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Myinfo
{
    public class MyApproveform : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public MyApproveform(IHubContext<NotiHub> hubContext)
        {
            this.hubContext=hubContext;
        }
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostGetForm(string begin,string limit,string cur)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_System_GetApproveForm]",CommandType.StoredProcedure
                        ,"@UserID",SqlDbType.Int,user.sys_userid
                       , "@cur",SqlDbType.Int,cur
                        ,"@begin",SqlDbType.Int,begin
                        ,"@Limit",SqlDbType.Int,limit);

                }
                return Content(Comon.DataJson.Datatable(dt));

            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostUpdateSign(int id,string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Product_FormOrderSign]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid,
                         "@Sign",SqlDbType.NVarChar,sign 
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostApprove(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Product_FormOrderApprove]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostReject(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Product_FormOrderReject]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
