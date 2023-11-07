using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.DiseaseStatus.DiseaseTemplate
{
    public class DiseaseTemplateModel : PageModel
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
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);

                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_Diagnose_Template]" ,CommandType.StoredProcedure ,
                            "@ID" ,SqlDbType.Int ,Convert.ToInt32(id));
                        dt.TableName = "List";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetForm]" ,CommandType.StoredProcedure
                            ,"@TYPE" ,SqlDbType.NVarChar ,"dia_form"
                            );
                        dt.TableName = "Form";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostUpdate(int currentID ,string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DinoTemplate DataMain = JsonConvert.DeserializeObject<DinoTemplate>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Setting_Diagnose_Template_Update]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,currentID ,
                        "@FormID" ,SqlDbType.Int ,DataMain.FormID ,
                        "@ShowPortal" ,SqlDbType.Int ,DataMain.ShowPortal ,
                        "@UsingICD" ,SqlDbType.Int ,DataMain.UsingICD ,
                        "@IsDisabled" ,SqlDbType.Int ,DataMain.IsDisabled ,
                        "@Name" ,SqlDbType.NVarChar ,DataMain.Name ,
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
                DinoTemplate DataMain = JsonConvert.DeserializeObject<DinoTemplate>(data);
                string minify = Session.GetSession(HttpContext.Session ,"Minify");
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Diagnose_Template_ImportV2]" ,CommandType.StoredProcedure ,
                        "@IsMinify" ,SqlDbType.Int ,Convert.ToInt16(minify) ,
                        "@Code" ,SqlDbType.NVarChar ,DataMain.Code ,
                        "@CanvasType" ,SqlDbType.NVarChar ,DataMain.CanvasType ,
                        "@Name" ,SqlDbType.NVarChar ,DataMain.Name ,
                        "@Type" ,SqlDbType.NVarChar ,DataMain.Type ,
                        "@SVG" ,SqlDbType.NVarChar ,DataMain.SVG ,
                        "@Width" ,SqlDbType.Int ,DataMain.Width ,
                        "@Height" ,SqlDbType.Int ,DataMain.Height ,
                        "@Image" ,SqlDbType.NVarChar ,DataMain.Image ,
                        "@WidthImg" ,SqlDbType.Int ,DataMain.WidthImg ,
                        "@HeightImg" ,SqlDbType.Int ,DataMain.HeightImg ,
                        "@ColorDraw" ,SqlDbType.NVarChar ,DataMain.ColorDraw
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDelete(string code)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Diagnose_Template_Delete]" ,CommandType.StoredProcedure ,
                        "@Code" ,SqlDbType.NVarChar ,code
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
    public class DinoTemplate
    {
        public int ShowPortal { get; set; }
        public int UsingICD { get; set; }
        public int FormID { get; set; }
        public int IsDisabled { get; set; }
        public string Code { get; set; }
        public string CanvasType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SVG { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Image { get; set; }
        public string WidthImg { get; set; }
        public string HeightImg { get; set; }
        public string ColorDraw { get; set; }
    }
}
