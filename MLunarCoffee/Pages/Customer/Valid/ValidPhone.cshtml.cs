using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Valid
{
    public class ValidPhone : PageModel
    {
        public string _vlp_phone { get; set; }
        public void OnGet()
        {
            var v = Request.Query["phone"];
            _vlp_phone = v.ToString();
        }
        public async Task<IActionResult> OnPostValid(string CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_ValidProfile" ,CommandType.StoredProcedure
                       ,"@CurrentID" ,SqlDbType.Int ,CustomerID
                       ,"@CreatedBy" ,SqlDbType.Int ,user.sys_userid
                       );
                }

                return Content("1");
            }

            catch (Exception ex)
            {
                return Content("");
            }

        }

    }
}
