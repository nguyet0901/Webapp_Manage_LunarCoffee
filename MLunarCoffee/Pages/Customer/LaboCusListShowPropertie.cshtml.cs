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
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MLunarCoffee.Pages.Customer
{
    public class LaboCusListShowPropertieModel : PageModel
    {
        public string LaboID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                LaboID = curr.ToString();
            }
            else
            {
                LaboID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadData(string LaboID)
        {

            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Labo_LoadProperties", CommandType.StoredProcedure
                        , "@LaboID", SqlDbType.Int, LaboID);

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
