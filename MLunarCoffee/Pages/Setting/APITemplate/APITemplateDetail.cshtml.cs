using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
namespace MLunarCoffee.Pages.Setting.APITemplate
{
    public class APITemplateDetailModel : PageModel
    {
        public string _currentID { get; set; }
        public string _parrentID { get; set; }
        public void OnGet()
        {
            string currid = Request.Query["CurrentID"];
            string parid = Request.Query["ParrentID"];

            _currentID = currid != null ? currid.ToString() : "0";
            _parrentID = parid != null ? parid.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadData(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting_Detail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id));
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


        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                MappingSetting DataMain = JsonConvert.DeserializeObject<MappingSetting>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting_Insert]" ,CommandType.StoredProcedure ,
                            "@APIType" ,SqlDbType.NVarChar ,DataMain.ActionType.Replace("'" ,"").Trim() ,
                            "@ActionType" ,SqlDbType.NVarChar ,DataMain.ActionType.Replace("'" ,"").Trim() ,
                            "@ParaName" ,SqlDbType.NVarChar ,DataMain.ParaName.Replace("'" ,"").Trim() ,
                            "@MapField" ,SqlDbType.NVarChar ,DataMain.MapField.Replace("'" ,"").Trim() ,
                            "@IsMaster" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsMaster) ,
                            "@UseDataFirst" ,SqlDbType.Int ,Convert.ToInt32(DataMain.UseDataFirst) ,
                            "@ParrentID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ParrentID) ,
                            "@LevelPath" ,SqlDbType.Int ,Convert.ToInt32(DataMain.LevelPath) ,
                            "@TypeData" ,SqlDbType.NVarChar ,DataMain.TypeData ,
                            "@TypeItem" ,SqlDbType.NVarChar ,DataMain.TypeItem ,
                            "@Path" ,SqlDbType.NVarChar ,DataMain.Path ,
                            "@Conditions" ,SqlDbType.NVarChar ,DataMain.Conditions ,
                            "@DefaultValue" ,SqlDbType.NVarChar ,DataMain.DefaultValue ,
                            "@CountSelf" ,SqlDbType.Int ,Convert.ToInt32(DataMain.CountSelf) ,
                            "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                            "@UserID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_userid)
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {

                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting_Update]" ,CommandType.StoredProcedure ,
                            "@ParaName" ,SqlDbType.NVarChar ,DataMain.ParaName.Replace("'" ,"").Trim() ,
                            "@MapField" ,SqlDbType.NVarChar ,DataMain.MapField.Replace("'" ,"").Trim() ,
                            "@IsMaster" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsMaster) ,
                            "@UseDataFirst" ,SqlDbType.Int ,Convert.ToInt32(DataMain.UseDataFirst) ,
                            "@TypeData" ,SqlDbType.NVarChar ,DataMain.TypeData ,
                            "@TypeItem" ,SqlDbType.NVarChar ,DataMain.TypeItem ,
                            "@Conditions" ,SqlDbType.NVarChar ,DataMain.Conditions ,
                            "@DefaultValue" ,SqlDbType.NVarChar ,DataMain.DefaultValue ,
                            "@CountSelf" ,SqlDbType.Int ,Convert.ToInt32(DataMain.CountSelf) ,
                            "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                            "@UserID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_userid) ,
                            "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDelete(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_API_Accounting_LoadSetting_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
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
    }
    public class MappingSetting
    {
        public string APIType { get; set; }
        public string ActionType { get; set; }
        public string ParaName { get; set; }
        public string MapField { get; set; }
        public int IsMaster { get; set; }
        public int UseDataFirst { get; set; }
        public int ParrentID { get; set; }
        public int LevelPath { get; set; }
        public string TypeData { get; set; }
        public string TypeItem { get; set; }
        public string Path { get; set; }
        public string Conditions { get; set; }
        public string DefaultValue { get; set; }
        public int CountSelf { get; set; }
        public string Note { get; set; }
    }

}
