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

namespace MLunarCoffee.Pages.Dash.Revenue.ServiceCat
{
    //[ResponseCache(Duration = 86400)]
    public class ServiceCatChartModel : PageModel
    {
        public string _branch { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branch = user.sys_branchID.ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Dash_Revenue_Service_Cat]", CommandType.StoredProcedure,
                        "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        , "@branchID", SqlDbType.DateTime, Convert.ToInt32(branchID));
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }
    }
}

