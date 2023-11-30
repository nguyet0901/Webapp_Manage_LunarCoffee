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

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class InstallmentDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CurrentTabID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentTabID"];

            if (cus.ToString() != String.Empty && curr.ToString() != String.Empty)
            {
                _CurrentTabID = curr.ToString();
                _CustomerID = cus.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadDataUpdate(int CurrentID)
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Installment_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID);

                    if (dt != null && dt.Rows.Count != 0)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
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
        public async Task<IActionResult> OnPostExcuteData(string data, string currentTab, string totalStep)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_TabInstallment_Update]", CommandType.StoredProcedure,
                        "@totalStep", SqlDbType.Int, totalStep,
                        "@currentTab", SqlDbType.Int, currentTab,
                        "@data", SqlDbType.NVarChar, data

                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        return Content("Không Thể Thêm, Vui Lòng Kiểm Tra Lại");
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
