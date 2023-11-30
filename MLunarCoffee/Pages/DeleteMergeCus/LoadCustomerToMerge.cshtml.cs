using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.DeleteMergeCus
{
    public class LoadCustomerToMergeModel : PageModel
    {
        public string _customerFromMerge { get; set; }
        public void OnGet()
        {
            string cus = Request.Query["CustomerID"];
            if (cus != null)
            {
                _customerFromMerge = cus.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Customer_DeleteMerge_LoadDetail", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


         public async Task<IActionResult> OnPostDetectDetination(string CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Customer_DeleteMerge_LoadDetail", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, CustomerID);
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

         public async Task<IActionResult> OnPostSearchingCustomerToMerge(string keyword, string customerFrom)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Merge_Searching_Detination]", CommandType.StoredProcedure,
                 "@searchText", SqlDbType.NVarChar, keyword,
                 "@customerFrom", SqlDbType.Int, customerFrom,
                  "@user", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
