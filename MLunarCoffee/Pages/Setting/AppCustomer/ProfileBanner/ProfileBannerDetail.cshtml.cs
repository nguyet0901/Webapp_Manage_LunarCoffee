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
    public class ProfileBannerDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            _CurrentID = Request.Query["CurrentID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_ProfileBanner_LoadDetail]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string CurrentID ,string ImgFea, string Img, string Title, string content)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_ProfileBanner_Insert]" ,CommandType.StoredProcedure
                            ,"@ImgFea", SqlDbType.NVarChar,ImgFea
                            ,"@Img", SqlDbType.NVarChar,Img
                            ,"@Title", SqlDbType.NVarChar,Title
                            ,"@content", SqlDbType.NVarChar,content
                            ,"@UserID", SqlDbType.Int, user.sys_userid
                            );
                    }
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_ProfileBanner_Update]" ,CommandType.StoredProcedure
                            ,"@CurrentID", SqlDbType.Int,CurrentID
                            ,"@ImgFea" ,SqlDbType.NVarChar,ImgFea
                            ,"@Img" ,SqlDbType.NVarChar ,Img
                            ,"@Title" ,SqlDbType.NVarChar ,Title
                            ,"@content" ,SqlDbType.NVarChar ,content
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
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
