using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Treatment.TreatmentWork
{

    public class TreatWorkListModel : PageModel
    {
        public string _TreatCustomerID { get; set; }
        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _TreatCustomerID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadata(string CustomerID, int id, int limit, int beginID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Treatment_Work_LoadList]", CommandType.StoredProcedure,
                        "@CustomerID", SqlDbType.Int, CustomerID
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@id", SqlDbType.Int, id
                      , "@limit", SqlDbType.Int, limit
                      , "@beginID", SqlDbType.Int, beginID);
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Work_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
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
