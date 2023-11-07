using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.AppCustomer.Desk
{
    public class GeneralModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }


        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Branch_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
