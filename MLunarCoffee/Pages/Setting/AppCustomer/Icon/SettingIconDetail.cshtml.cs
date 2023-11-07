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

namespace MLunarCoffee.Pages.Setting.AppCustomer.Icon
{
    public class SettingIconDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string Curr = Request.Query["CurrentID"];
            _CurrentID = Curr != null ? Curr.ToString() : "0";
        }
        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_AC_Color_LoadDetail]" ,CommandType.StoredProcedure ,
                    "@ID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID));
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
        public async Task<IActionResult> OnPostExecuted(string CurrentID ,string ImageLink, string ImageLinkR)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_Color_UpdateImg]" ,CommandType.StoredProcedure
                        ,"@CurrentID", SqlDbType.Int,CurrentID
                        ,"@ImageLink" ,SqlDbType.NVarChar ,ImageLink
                        ,"@ImageLinkR" ,SqlDbType.NVarChar ,ImageLinkR
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
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
