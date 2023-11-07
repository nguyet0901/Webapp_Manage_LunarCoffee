using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.ICD
{
    public class GroupListModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostDataAreaLoad(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_ICDArea_LoadData]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.NVarChar , CurrentID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }        
        public async Task<IActionResult> OnPostDataCodeLoad()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_ICDChapter_GetCode]" ,CommandType.StoredProcedure
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string chaptercode ,string data)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("MabenhKD" ,typeof(String));
                dtMain.Columns.Add("TenbenhEn" ,typeof(String));
                dtMain.Columns.Add("TenbenhVn" ,typeof(String));
                dtMain.Columns.Add("TenbenhEn_Nosign" ,typeof(String));
                dtMain.Columns.Add("TenbenhVn_Nosign" ,typeof(String));
                dtMain.Columns.Add("Maloai" ,typeof(String));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["MabenhKD"] = DataMain.Rows[i]["MabenhKD"];
                    dr["TenbenhEn"] = DataMain.Rows[i]["TenbenhEn"];
                    dr["TenbenhVn"] = DataMain.Rows[i]["TenbenhVn"];
                    dr["TenbenhEn_Nosign"] = DataMain.Rows[i]["TenbenhEn_Nosign"];
                    dr["TenbenhVn_Nosign"] = DataMain.Rows[i]["TenbenhVn_Nosign"];
                    dr["Maloai"] = DataMain.Rows[i]["Maloai"];
                    dtMain.Rows.Add(dr);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_ICDChapter_Update]" ,CommandType.StoredProcedure ,
                        "@chaptercode" ,SqlDbType.NVarChar ,chaptercode
                        ,"@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0) ? dtMain : null
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostRemoveData(string chaptercode)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_ICDChapter_Remove]" ,CommandType.StoredProcedure ,
                        "@chaptercode" ,SqlDbType.NVarChar ,chaptercode
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class ICD
    {
        public string MabenhKD { get; set; }
        public string TenbenhEn { get; set; }
        public string TenbenhVn { get; set; }
        public string TenbenhEn_Nosign { get; set; }
        public string TenbenhVn_Nosign { get; set; }
        public string MaNhom { get; set; }
        public string Maloai { get; set; }
    }
}
