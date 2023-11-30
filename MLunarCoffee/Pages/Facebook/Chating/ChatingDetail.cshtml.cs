using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon;

namespace MLunarCoffee.Pages.Facebook.Chating
{
    public class ChatingDetailModel : PageModel
    {
        public void OnGet()
        {
        }



        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_FB_Mess_Template_LoadList]",
                        CommandType.StoredProcedure );
                    dt.TableName = "MessTemplate";
                    ds.Tables.Add(dt);
                }
                return Content(DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
