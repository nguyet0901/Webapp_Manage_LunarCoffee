using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.CustomerPage.Setting
{
    public class CustomerPageDetailModel : PageModel
    {
        public string _CustomerPageDetailID { get; set; }
        public string _CustomerPageType { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string type = Request.Query["Type"];
            _CustomerPageDetailID = curr != null ? curr.ToString() : "0";
            _CustomerPageType = type != null ? type.ToString() : "";
        }


        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_CustomerWebPage_Load]", CommandType.StoredProcedure,
                    "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID));
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



        public async Task<IActionResult> OnPostExcuteData(string Value, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustomerWebPage_Update]", CommandType.StoredProcedure,
                              "@Value", SqlDbType.NVarChar, Value.ToString(),
                              "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                              "@UserID", SqlDbType.Int, user.sys_userid
                        );
                        return Content("1");
                    }
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
