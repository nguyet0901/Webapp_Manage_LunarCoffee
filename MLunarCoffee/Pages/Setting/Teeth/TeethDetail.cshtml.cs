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

namespace MLunarCoffee.Pages.Setting.Teeth
{
    public class TeethDetailModel : PageModel
    {
        public string _TeethDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TeethDetailID = curr.ToString();
            }
            else
            {
                _TeethDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_Teeth_LoadDetail]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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


        public async Task<IActionResult> OnPostExcuteData(string AdultTeeth, string BabyTeeth, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Teeth_Update]", CommandType.StoredProcedure,
                        "@AdultTeeth", SqlDbType.NVarChar, AdultTeeth.Replace("'", "").Trim(),
                        "@BabyTeeth", SqlDbType.NVarChar, BabyTeeth.Replace("'", "").Trim(),
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
