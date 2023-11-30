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

namespace MLunarCoffee.Pages.AppCustomer
{
    public class AppCustSettingArticleDetailModel : PageModel
    {
        public string _PushNotiDetailID { get; set; }
        public void OnGet()
        {

            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _PushNotiDetailID = curr.ToString();

            }
            else
            {
                _PushNotiDetailID = "0";

            }
        }
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_AppCustSetting_Article_LoadDetail]", CommandType.StoredProcedure,
                        "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
        
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string imgCurrent)
        {
            try
            {
                PushNoti dataPush = JsonConvert.DeserializeObject<PushNoti>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_AppCustSetting_Article_Insert]",
                            CommandType.StoredProcedure,
                            "@Image", SqlDbType.NVarChar, dataPush.Image,
                            "@Name", SqlDbType.NVarChar, dataPush.Title,
                            "@Description", SqlDbType.NVarChar, dataPush.Description,
                            "@Content", SqlDbType.NVarChar, dataPush.Content,
                            "@CreatedBy", SqlDbType.Int, user.sys_userid,
                            "@IsFeature", SqlDbType.Int, dataPush.IsFeature

                        );
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_AppCustSetting_Article_Update]",
                            CommandType.StoredProcedure,
                            "@Image", SqlDbType.NVarChar, dataPush.Image,
                            "@Name", SqlDbType.NVarChar, dataPush.Title,
                            "@Description", SqlDbType.NVarChar, dataPush.Description,
                            "@Content", SqlDbType.NVarChar, dataPush.Content,
                            "@ModifiedBy", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@IsFeature", SqlDbType.Int, dataPush.IsFeature
                        );
                        if (imgCurrent != "" && imgCurrent != dataPush.Image)
                        {
                            string des = Comon.Global.sys_ServerImageFolder + "App/" + imgCurrent.ToString();
                            Comon.Comon.DeleteFile(des);
                        }
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class PushNoti
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public string Image { get; set; }
            public int Type { get; set; }
            public int IsFeature { get; set; }
        }
    }
}
