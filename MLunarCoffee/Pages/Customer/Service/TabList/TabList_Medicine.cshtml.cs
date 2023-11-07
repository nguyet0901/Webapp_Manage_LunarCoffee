using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_MedicineModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadataPrescriptionMedicine(int CustomerID, int id, int limit, int beginID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Prescription_Medicine_LoadList]", CommandType.StoredProcedure,
                        "@CustomerID", SqlDbType.Int, CustomerID
                        , "@id", SqlDbType.Int, id
                        , "@limit", SqlDbType.Int, limit
                        , "@beginID", SqlDbType.Int, beginID
                    );
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItemPrescriptionMedicine(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Prescription_Medicine_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
