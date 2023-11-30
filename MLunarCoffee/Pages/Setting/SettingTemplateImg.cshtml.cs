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
    public class SettingTemplateImgModel : PageModel
    {
        //public string sys_Setting_Appointment_Doctor { get; set; }
        public void OnGet()
        {
            //sys_Setting_Appointment_Doctor = Comon.Global.sys_Setting_Appointment_Doctor;
        }


        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Image_Template_Load", CommandType.StoredProcedure);
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
            catch
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
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_sp_Image_Template_Delete]", CommandType.StoredProcedure,
                        "@id", SqlDbType.Int, id
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        // 
        // public async Task<IActionResult> OnPostExcuteData(string Name,string DataTemplate,string Note)
        //{
        //    try
        //    {
        //        GlobalUser user = (GlobalUser)HttpContext.Current.Session["CurrentUserInfo"];

        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Image_Template_Insert]", CommandType.StoredProcedure,
        //                      "@Name", SqlDbType.NVarChar, Name.ToString().Replace("'", "").Trim(),
        //                      "@DataTemplate",SqlDbType.NVarChar,DataTemplate.ToString().Trim(),
        //                       "@note", SqlDbType.NVarChar, Note.ToString().Trim()
        //                );
        //                if (dt.Rows.Count > 0)
        //                {

        //                        return "1";

        //                }
        //                else
        //                {
        //                    return "1";
        //                }
        //            }

        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }
        //}

    }
}