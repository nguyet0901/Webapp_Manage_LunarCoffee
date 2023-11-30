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

namespace MLunarCoffee.Pages.Setting.AppCustomer.ProfileBanner
{
    public class ProfileBannerListModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadList(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[MLU_sp_AC_ProfileBanner_Load]" ,CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostHide(int ID, int IsDelete, int IsHide)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[MLU_sp_AC_ProfileBanner_ExecutedHide]" ,CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, ID
                        , "@IsDelete", SqlDbType.Int, IsDelete
                        ,"@IsHide", SqlDbType.Int,IsHide
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
