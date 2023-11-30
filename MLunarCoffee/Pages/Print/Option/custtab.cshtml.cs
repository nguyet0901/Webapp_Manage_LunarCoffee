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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Print.Option
{
 
    public class custtab : PageModel
    {
        string treatplanid { get; set; }
        public void OnGet()
        {
            string _treatplanid = Request.Query["treatment_plan"];
            
            if (_treatplanid != null)
            {
                treatplanid = _treatplanid.ToString();
            }
            else
            {
                _treatplanid = "0";
            }
        }
       

         public async Task<IActionResult> OnPostCheckDeleStage(string idStage)
        {
            try
            {
                if (idStage == "0") return Content("0");
                DataTable dt1 = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt1 =await connFunc.ExecuteDataTable("[MLU_sp_Service_CheckDeleteStage]", CommandType.StoredProcedure,
                    "@ID", SqlDbType.Int, Convert.ToInt32(idStage));
                }
                if (dt1 != null && dt1.Rows.Count != 0 && Convert.ToInt32(dt1.Rows[0][0]) != 0)
                    return Content("0");
                else return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
