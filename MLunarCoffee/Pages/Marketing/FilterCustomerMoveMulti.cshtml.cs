using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class FilterCustomerMoveMultiModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_TeleGroup_LoadCombo_ByUser]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadData(int groupId)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Move_LoadTele_ByGroup]" ,CommandType.StoredProcedure
                        ,"@groupID" ,SqlDbType.Int ,groupId
                        ,"@userID" ,SqlDbType.Int ,user.sys_userid);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }



        public async Task<IActionResult> OnPostExecute(string dtcust)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtMain = JsonConvert.DeserializeObject<DataTable>(dtcust);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_FilterCustomer_DivideMultiCustCare]" ,CommandType.StoredProcedure ,
                        "@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0 ? (DataTable)dtMain : null) ,
                        "@CreatedBy" ,SqlDbType.Int ,user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
