using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_Card_MoveModel : PageModel
    {
        public string _CardCustomerID { get; set; }
        public string _CardCurrentID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            if (cus.ToString() != String.Empty && curr.ToString() != String.Empty)
            {
                _CardCustomerID = cus.ToString();
                _CardCurrentID = curr.ToString();
            }
            else if (cus.ToString() != String.Empty)
            {
                _CardCustomerID = cus.ToString();
                _CardCurrentID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Card_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostSearchCustomer(string TextSearch, string CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Card_SearchCustomer]", CommandType.StoredProcedure,
                        "@TextSearch", SqlDbType.NVarChar, TextSearch,
                        "@CustomerID", SqlDbType.Int, CustomerID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string CustomerID, string CurrentID, string Note)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Card_Move]", CommandType.StoredProcedure,
                         "@CustomerID", SqlDbType.Int, CustomerID,
                         "@CardCustID", SqlDbType.Int, CurrentID,
                         "@Note", SqlDbType.NVarChar, Note != null ? Note : "",
                         "@BranchID", SqlDbType.Int, user.sys_branchID,
                         "@Created_By", SqlDbType.Int, user.sys_userid);
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
