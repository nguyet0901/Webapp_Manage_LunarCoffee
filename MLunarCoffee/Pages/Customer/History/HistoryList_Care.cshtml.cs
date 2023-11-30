using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.History
{

    public class HistoryList_CareModel : PageModel
    {
        public string _CustomerHistoryID { get; set; }
        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _CustomerHistoryID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }


        public async Task<IActionResult> OnPostLoadataHistory(string CustomerID ,string Limit ,string BeginID ,string Type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Ticket_Customer_LoadHistoryCare" ,CommandType.StoredProcedure ,
                      "@Customer_ID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID) ,
                      "@Ticket" ,SqlDbType.Int ,0 ,
                      "@BeginID" ,SqlDbType.Int ,BeginID ,
                      "@Limit" ,SqlDbType.Int ,Limit ,
                      "@Type" ,SqlDbType.Int ,Type ,
                      "@UserId" ,SqlDbType.Int ,user.sys_userid ,
                      "@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {

            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_History_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                         "@userID" ,SqlDbType.Int ,user.sys_userid
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
