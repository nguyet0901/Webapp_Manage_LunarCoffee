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

namespace MLunarCoffee.Pages.Service.PrescriptionMedicine
{
    public class PrescriptionMedicineModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await  confunc.ExecuteDataSet("[YYY_sp_PrescriptionMedicine_LoadList]", CommandType.StoredProcedure);
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
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_PrescriptionMedicine_Delete]", CommandType.StoredProcedure,
                         "@Modified_By", SqlDbType.Int, user.sys_userid,
                         "@CurrentID", SqlDbType.Int, id);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }

            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
