using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Student.Settings.Certificate
{
    public class CertificateListModel : PageModel
    {
        public async Task<IActionResult> OnPostLoadData(string IdDetail)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_Certificate_Load", CommandType.StoredProcedure,
                      "@IdDetail", SqlDbType.Int, Convert.ToInt32(IdDetail));
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDelete(string IdDetail, string State)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_Certificate_Delete", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(IdDetail)
                      );
                }

                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
