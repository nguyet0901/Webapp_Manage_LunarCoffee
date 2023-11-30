using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ImportTemplate
{
    public class ImportTemplateListModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData(string type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_Import_Template_LoadList]", CommandType.StoredProcedure
                        , "@Type", SqlDbType.NVarChar, type);
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

        public async Task<IActionResult> OnPostDisableItem(int id, int isRequired)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_Import_Template_Disable]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, id
                        , "@userID", SqlDbType.Int, user.sys_userid
                        , "@IsRequired", SqlDbType.Int, isRequired
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("Name" ,typeof(String));
                dtMain.Columns.Add("Description" ,typeof(String));
                dtMain.Columns.Add("IsRequired" ,typeof(Int32));
                dtMain.Columns.Add("ValidationName" ,typeof(String));
                dtMain.Columns.Add("ValidationValue" ,typeof(String));
                dtMain.Columns.Add("isReplace" ,typeof(Int32));
                dtMain.Columns.Add("ReplaceCol" ,typeof(String));
                dtMain.Columns.Add("ReplaceValue" ,typeof(String));
                dtMain.Columns.Add("TypeData" ,typeof(String));
                dtMain.Columns.Add("ImportTo" ,typeof(String));
                dtMain.Columns.Add("ExampleValue" ,typeof(String));
                dtMain.Columns.Add("Type" ,typeof(String));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Name"] = DataMain.Rows[i]["Name"];
                    dr["Description"] = DataMain.Rows[i]["Description"];
                    dr["IsRequired"] = DataMain.Rows[i]["IsRequired"];
                    dr["ValidationName"] = DataMain.Rows[i]["ValidationName"];
                    dr["ValidationValue"] = DataMain.Rows[i]["ValidationValue"];
                    dr["isReplace"] = DataMain.Rows[i]["isReplace"];

                    dr["ReplaceCol"] = DataMain.Rows[i]["ReplaceCol"];
                    dr["ReplaceValue"] = DataMain.Rows[i]["ReplaceValue"];
                    dr["TypeData"] = DataMain.Rows[i]["TypeData"];
                    dr["ImportTo"] = DataMain.Rows[i]["ImportTo"];
                    dr["ExampleValue"] = DataMain.Rows[i]["ExampleValue"];
                    dr["Type"] = DataMain.Rows[i]["Type"];
                    dtMain.Rows.Add(dr);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_Template_Update]" ,CommandType.StoredProcedure 
                        ,"@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0) ? dtMain : null
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
