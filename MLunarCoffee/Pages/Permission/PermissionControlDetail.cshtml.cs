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


namespace MLunarCoffee.Pages.Permission
{
    public class PermissionControlDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
           string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
        }
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Permission_ControlLoadDetail]", CommandType.StoredProcedure,
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
         public async Task<IActionResult> OnPostInitializeComboPermissionPage()
        {
            DataSet ds = new DataSet();

            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Combo_Page]", CommandType.StoredProcedure);
                dt.TableName = "_DataComboPermissionPage";
                ds.Tables.Add(dt);
            }
            //LoadType
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("MLU_sp_Permission_LoadTypeControl", CommandType.StoredProcedure);
                dt.TableName = "_dataType";
                ds.Tables.Add(dt);
            }
            //LoadPageType
            //LoadLanguage
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.LoadPara("Per_Page");
                dt.TableName = "_dataPageType";
                ds.Tables.Add(dt);
            }
            return Content(Comon.DataJson.Dataset(ds));
        }



         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PermissionControl DataMain = JsonConvert.DeserializeObject<PermissionControl>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Control_Insert]", CommandType.StoredProcedure,
                            "@Page_ID", SqlDbType.Int, DataMain.Page_ID,
                            "@ControlValue", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                            "@ControlName", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@ColumnName ", SqlDbType.NVarChar, DataMain.ColumnName.Replace("'", "").Trim(),
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Control_Update]", CommandType.StoredProcedure,
                            "@Page_ID", SqlDbType.Int, DataMain.Page_ID,
                            "@ControlValue", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                            "@ControlName", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@ColumnName ", SqlDbType.NVarChar, DataMain.ColumnName.Replace("'", "").Trim(),
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@CurrentID ", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Control Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class PermissionControl
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Page_ID { get; set; }
        public string ColumnName { get; set; }
        public string Type { get; set; }
    }
}
