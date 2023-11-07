using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Account.Fund
{
    public class ClosingEntryViewModel : PageModel
    {
        public string _ClosingEntryID { get; set; }
        public void OnGet()
        {
            string current = Request.Query["CurrentID"];
            _ClosingEntryID = current != null ? current.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInitialize(string currentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Closing_Entry_View_Load]", CommandType.StoredProcedure,
                       "@ID", SqlDbType.Int, currentID);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string date, int branchid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_v2_Account_Funds_Load]", CommandType.StoredProcedure,
                                "@date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(date.ToString())
                                , "@branchID", SqlDbType.Int, branchid);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }
    }
}
