using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class EmailSettingCheckModel : PageModel
    {
        public string _SerCurrentID { get; set; }
        public void OnGet()
        {
            _SerCurrentID = Request.Query["CurrentID"].ToString();
        }

        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("YYY_Email_Setting_Load_Detail", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
