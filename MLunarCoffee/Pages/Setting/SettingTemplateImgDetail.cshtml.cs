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

namespace MLunarCoffee.Pages.Setting
{
    public class SettingTemplateImgDetailModel : PageModel
    {
        public string _ImgDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _ImgDetailID = (curr != null) ? curr.ToString() : "0";
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Image_Template_LoadDetails]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostExcuteData(string Name, string DataTemplate, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Image_Template_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, Name.ToString().Replace("'", "").Trim(),
                            "@DataTemplate", SqlDbType.NVarChar, DataTemplate.ToString().Trim(),
                            "@CreatedBy", SqlDbType.Int, user.sys_userid
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                            return Content("0");
                    }

                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Image_Template_Update]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID.ToString().Trim(),
                            "@Name", SqlDbType.NVarChar, Name.ToString().Replace("'", "").Trim(),
                            "@DataTemplate", SqlDbType.NVarChar, DataTemplate.ToString().Trim(),
                            "@CreatedBy", SqlDbType.Int, user.sys_userid

                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                            return Content("0");
                    }
                }
            }

            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }

}
