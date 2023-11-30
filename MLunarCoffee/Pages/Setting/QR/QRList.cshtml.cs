using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.QR
{
    public class QRListModel : PageModel
    {
        public string _Minify { get; set; }
        public void OnGet()
        {
            _Minify = Session.GetSession(HttpContext.Session ,"Minify");
        }
        public async Task<IActionResult> OnPostLoadData(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_LoadList]" ,CommandType.StoredProcedure ,
                        @"ID" ,SqlDbType.Int ,Convert.ToInt32(id));

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostUpdate(int currentID ,string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                QRTemplate DataMain = JsonConvert.DeserializeObject<QRTemplate>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_Update]" ,CommandType.StoredProcedure ,
                        "@ID" ,SqlDbType.Int ,currentID ,
                        "@Name" ,SqlDbType.NVarChar ,DataMain.Name ,
                        "@Value" ,SqlDbType.NVarChar ,DataMain.Value ,
                        "@Note" ,SqlDbType.NVarChar ,DataMain.Note ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostImport(string data)
        {
            try
            {
                QRTemplate DataMain = JsonConvert.DeserializeObject<QRTemplate>(data);
                string minify = Session.GetSession(HttpContext.Session ,"Minify");
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_Import]" ,CommandType.StoredProcedure ,
                        "@IsMinify" ,SqlDbType.Int ,Convert.ToInt16(minify) ,
                        "@Code" ,SqlDbType.NVarChar ,DataMain.Code ,
                        "@Type" ,SqlDbType.NVarChar ,DataMain.Type ,
                        "@Name" ,SqlDbType.NVarChar ,DataMain.Name ,
                        "@Value" ,SqlDbType.NVarChar ,DataMain.Value ,
                        "@Note" ,SqlDbType.NVarChar ,DataMain.Note ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDelete(string code ,string id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_Delete]" ,CommandType.StoredProcedure ,
                        "@Code" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(code) ? code : "" ,
                        "@id" ,SqlDbType.Int ,Convert.ToInt32(id) ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostClone(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_Clone]" ,CommandType.StoredProcedure ,
                        "@ID" ,SqlDbType.Int ,Convert.ToInt32(id) ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
    public class QRTemplate
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
    }
}
