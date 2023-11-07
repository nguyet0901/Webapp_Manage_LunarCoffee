using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer
{
    public class LaboNoteHistoryModel : PageModel
    {
        public string LabID { get; set; }
        public void OnGet()
        {
            string LaboID = Request.Query["CurrentID"];
            LabID = LaboID != "" ? LaboID : "0";
        }
        public async Task<IActionResult> OnPostLoadData(string LabID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_HistoryNote_Load]" ,CommandType.StoredProcedure
                        , "@LaboID", SqlDbType.Int, Convert.ToInt32(LabID)
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
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExecuted(string LabID, string content)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_HistoryNote_Insert]" ,CommandType.StoredProcedure
                        ,"@LaboID" ,SqlDbType.Int ,Convert.ToInt32(LabID)
                        ,"@Content", SqlDbType.NVarChar, content
                        ,"@CreateBy", SqlDbType.Int, user.sys_userid
                        );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
