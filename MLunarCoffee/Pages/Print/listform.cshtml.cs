using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Print
{
    public class listform : PageModel
    {

        public string layout { get; set; }
        public string _Minify { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
            _Minify = Session.GetSession(HttpContext.Session ,"Minify");
        }
        public async Task<IActionResult> OnPostGetType()
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetListType]" ,CommandType.StoredProcedure);
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostGetTemplate(int id)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetTemplate]" ,CommandType.StoredProcedure
                        ,"@id" ,SqlDbType.Int ,Convert.ToInt32(id));
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostGetDetail(int id)
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetDetail]" ,CommandType.StoredProcedure
                        ,"@templateid" ,SqlDbType.Int ,id
                   );
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostChangeState(int id ,int status)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_ChangeState]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@Status" ,SqlDbType.Int ,status
                        ,"@templateid" ,SqlDbType.Int ,id
                   );
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_Delete]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@templateid" ,SqlDbType.Int ,id
                   );
                }
                return Content(DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostClone(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_Clone]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@templateid" ,SqlDbType.Int ,id
                   );
                }
                return Content(DataJson.GetFirstValue(dt));

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
                PrintForm DataMain = JsonConvert.DeserializeObject<PrintForm>(data);
                string minify = Session.GetSession(HttpContext.Session ,"Minify");
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_ImportForm]" ,CommandType.StoredProcedure ,
                         "@form" ,SqlDbType.NVarChar ,DataMain.form ,
                         "@code" ,SqlDbType.NVarChar ,DataMain.code ,
                         "@name" ,SqlDbType.NVarChar ,DataMain.name ,
                         "@width" ,SqlDbType.Int ,Convert.ToInt32(DataMain.width) ,
                         "@height" ,SqlDbType.Int ,Convert.ToInt32(DataMain.height) ,
                         "@note" ,SqlDbType.NVarChar ,DataMain.note != null ? DataMain.note.Replace("'" ,"") : "" ,
                         "@watermark" ,SqlDbType.NVarChar ,DataMain.watermark != null ? DataMain.watermark.Replace("'" ,"") : "" ,
                         "@commandID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.commandID) ,
                         "@command" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.command) ? DataMain.command : "" ,
                         "@commandExecuted" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.commandExecuted) ? DataMain.commandExecuted : "" ,
                         "@commandDesc" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.commandDesc) ? DataMain.commandDesc : "" ,
                         "@stogesign" ,SqlDbType.Int ,Convert.ToInt32(DataMain.stogesign) ,
                         "@type" ,SqlDbType.Int ,Convert.ToInt32(DataMain.type) ,
                         "@typeName" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.typeName) ? DataMain.typeName : "" ,
                         "@typeDesc" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(DataMain.typeDesc) ? DataMain.typeDesc : "" ,
                         "@typeNumForm" ,SqlDbType.NVarChar ,Convert.ToInt32(DataMain.typeNumForm) ,
                         "@IssetDimension" ,SqlDbType.Int ,Convert.ToInt32(DataMain.dimesion) ,
                         "@IsPaging" ,SqlDbType.Int ,Convert.ToInt32(DataMain.paging) ,
                         "@IsEnterExchangeRate" ,SqlDbType.Int ,Convert.ToInt32(DataMain.exchangeRate) ,
                         "@IsMinify" ,SqlDbType.Int ,Convert.ToInt16(minify) ,
                         "@UserID" ,SqlDbType.Int ,user.sys_userid
                   );
                }
                return Content(DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class PrintForm
    {
        public string form { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string typeName { get; set; }
        public string typeDesc { get; set; }
        public int typeNumForm { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string note { get; set; }
        public string watermark { get; set; }
        public int commandID { get; set; }
        public string command { get; set; }
        public string commandExecuted { get; set; }
        public string commandDesc { get; set; }
        public int dimesion { get; set; }
        public int paging { get; set; }
        public int stogesign { get; set; }
        public int exchangeRate { get; set; }
    }
}
