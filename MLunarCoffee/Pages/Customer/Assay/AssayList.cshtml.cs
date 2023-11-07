using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Assay
{

    public class AssayListModel : PageModel
    {
        public string _CustomerAssayID { get; set; }
        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _CustomerAssayID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }


        public async Task<IActionResult> OnPostLoadataAssayCusList(string CustomerID, int id, int limit, int beginID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Assay_Customer_LoadList]", CommandType.StoredProcedure
                      , "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@id", SqlDbType.Int, id
                      , "@limit", SqlDbType.Int, limit
                      , "@beginID", SqlDbType.Int, beginID
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
    }
}
