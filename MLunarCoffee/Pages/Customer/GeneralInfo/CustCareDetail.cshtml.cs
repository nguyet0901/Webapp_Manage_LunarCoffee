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

namespace MLunarCoffee.Pages.Customer.GeneralInfo
{
    public class CustCareDetail : PageModel
    {


        public string _Type { get; set; }
        public void OnGet()
        {
            string v = Request.Query["type"];
            if (v != null)
            {
                _Type = v == null ? "-1" : v.ToString();
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string CustomerID, string type)
        {
            try
            {
                DataTable dt= new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CustomerID != null)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_LoadEmployee", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                            "@type", SqlDbType.Int, type
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        
        public async Task<IActionResult> OnPostExcuteData(string CustomerID, string EmpUserID, string type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CustomerID != null)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_Update", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                             "@EmpUserID", SqlDbType.Int, EmpUserID,
                              "@type", SqlDbType.Int, type
                        );
                    }
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
