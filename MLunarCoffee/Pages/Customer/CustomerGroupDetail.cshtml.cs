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

namespace MLunarCoffee.Pages.Customer
{
    public class CustomerGroupDetailModel : PageModel
    {

        public string _TicketID { get; set; }
        public string _CustomerID { get; set; }
        public string _GroupID { get; set; }
        public string _dataCustomerGroup { get; set; }

        public void OnGet()
        {
            var tic = Request.Query["TicketID"];
            var cus = Request.Query["CustomerID"];
            var GroupID = Request.Query["GroupID"];
            if (cus.ToString() != "")
            {
                _CustomerID = cus.ToString();
            }
            else
            {
                _CustomerID = "0";
                Response.Redirect("/assests/Error/index.html");
            }
            if (tic.ToString() == "")
            {
                _TicketID = "0";
            }
            else
            {
                _TicketID = tic.ToString();
            }

            if (GroupID.ToString() == "")
            {
                _GroupID = "0";
            }
            else
            {
                _GroupID = GroupID.ToString();
            }

        }

         public async Task<IActionResult> OnPostLoadata_Group()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Group_ComboLoad]", CommandType.StoredProcedure
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
                return Content("0");
            }

        }



         public async Task<IActionResult> OnPostExcuteData(string CustomerID, string GroupID, string TicketID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CustomerID != null)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Customer_Group_Customer_Update", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                             "@TicketID", SqlDbType.Int, TicketID,
                            "@GroupID", SqlDbType.Int, GroupID
                        );
                    }
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
