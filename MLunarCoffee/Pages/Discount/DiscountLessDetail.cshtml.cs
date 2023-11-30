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


namespace MLunarCoffee.Pages.Discount
{
    public class DiscountLessDetailModel : PageModel
    {
        public string _CurrentLessID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];

            if (curr != null)
            {
                _CurrentLessID = curr.ToString();
            }
            else
            {
                _CurrentLessID = "0";
            }
        }
         public async Task<IActionResult> OnPostLoadDataDetail(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Discount_LoadDetail]", CommandType.StoredProcedure,
                      "@Current", SqlDbType.Int, Convert.ToInt32(id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

         public async Task<IActionResult> OnPostExcuteData(string CurrentID, string DateToLess)
        {
            try
            {
                if (CurrentID == "0")
                {
                    return Content("0");
                }
                else
                {
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[MLU_sp_Discount_Update_Less]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@Date_To", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateToLess),
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                      );
                    }
                    return Content("1");
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }
}
