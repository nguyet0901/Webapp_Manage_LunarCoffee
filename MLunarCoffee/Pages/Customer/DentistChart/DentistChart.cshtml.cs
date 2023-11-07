using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.DentistChart
{

    public class DentistChartModel : PageModel
    {
        public string _imageFeature_Disease { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            _imageFeature_Disease = Comon.Global.sys_HTTPImageRoot + "Sys_Disease_Feature/";
        }

        public async Task<IActionResult> OnPostLoad_Chart_Teeth_List(string customerid, int id, int limit, int beginID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                // load rang nho, rang lon o day
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Chart_Teeth_LoadList]", CommandType.StoredProcedure
                    , "@CustomerID", SqlDbType.Int, Convert.ToInt32(customerid)
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
                return Content("");
            }
        }


        public async Task<IActionResult> OnPostDeleteItem(int id)
        {

            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_Dentist_Chart_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                         "@userID", SqlDbType.Int, user.sys_userid
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
