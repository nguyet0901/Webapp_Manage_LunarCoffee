using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.ICD
{
    public class ICDAreaDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string Curr = Request.Query["CurrentID"];
            if (Curr != null)
            {
                _CurrentID = Curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
        }
        public async Task<IActionResult> OnPostAreaLoadDetail(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_ICDArea_LoadDetail]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string CurrentID ,string Area)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                { 
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_Setting_ICDArea_Update" ,CommandType.StoredProcedure
                            ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                            ,"@Area" ,SqlDbType.NVarChar ,Area
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        );
                    }
                    return Content("1");
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
