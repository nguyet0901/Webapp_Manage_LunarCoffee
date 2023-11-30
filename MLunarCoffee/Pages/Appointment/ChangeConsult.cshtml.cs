using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class ChangeConsultModel : PageModel
    {
        public string _DataComboConsult { get; set; }
        public string _AppDetailID { get; set; }
        public string _ConsultID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string consult = Request.Query["ConsultID"];
            if (curr != null && consult != null)
            {
                _AppDetailID = curr.ToString();
                _ConsultID = consult.ToString();
            }
            else
            {
                _AppDetailID = "0";
                _ConsultID = "0";
            }

        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                }

                if (dt != null && dt.Rows.Count != 0)
                    return Content(Comon.DataJson.Datatable(dt));
                else
                    return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string Consult_ID)
        {
            try
            {
                if (CurrentID == "0") return Content("0");
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Appoinment_ChangeConsult]", CommandType.StoredProcedure,
                       "@CurrentID", SqlDbType.Int, CurrentID,
                       "@Consult_ID", SqlDbType.Int, Consult_ID);
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
