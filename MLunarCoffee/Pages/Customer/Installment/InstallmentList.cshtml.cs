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

namespace MLunarCoffee.Pages.Customer.Installment
{

    public class InstallmentList : PageModel
    {
        public string _CustomerInstallID { get; set; }
        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _CustomerInstallID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadService(int CustomerID, string TabID, string Limit, string BeginID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_InstallmentList]", CommandType.StoredProcedure,
                  "@Customer_ID", SqlDbType.Int, CustomerID
                  , "@Limit", SqlDbType.Int, Limit
                  , "@BeginID", SqlDbType.Int, BeginID
                  , "@TabID", SqlDbType.Int, TabID);
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
        public async Task<IActionResult> OnPostLoadDetail( int TabID)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_InstallmentDetail]", CommandType.StoredProcedure
                  , "@TabID", SqlDbType.Int, TabID);
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
    }
}
