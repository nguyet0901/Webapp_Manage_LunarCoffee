using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Account
{
    public class LiabilitySuppViewFormModel : PageModel
    {
        public string code { get; set; }
        public void OnGet()
        {
            code = (Request.Query["code"].ToString() != null) ? Request.Query["code"].ToString() : "";
        }
        public async Task<IActionResult> OnPostViewForm(string code)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Report_Accountant_Liability_Supplier_ViewForm]", CommandType.StoredProcedure
                       , "@code", SqlDbType.NVarChar, code
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
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
