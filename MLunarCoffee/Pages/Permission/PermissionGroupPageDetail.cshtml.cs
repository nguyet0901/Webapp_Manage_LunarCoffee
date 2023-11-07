using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Permission
{
    public class PermissionGroupPageDetailModel : PageModel
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
                    dt =await  confunc.ExecuteDataTable("[dbo].[YYY_sp_Permission_GroupPage_LoadDetail]", CommandType.StoredProcedure,
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
                dt =await connFunc.ExecuteDataTable("[YYY_sp_Permission_Combo_Page]", CommandType.StoredProcedure);
                dt.TableName = "_DataComboPermissionPage";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await connFunc.ExecuteDataTable("[YYY_sp_Permission_Combo_Group]", CommandType.StoredProcedure);
                dt.TableName = "_DataComboPermisstionGroup";
                ds.Tables.Add(dt);
            }

            return Content(Comon.DataJson.Dataset(ds));

        }

       

         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PermissionGroupPage DataMain = JsonConvert.DeserializeObject<PermissionGroupPage>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Permission_GroupPage_Insert]", CommandType.StoredProcedure,
                            "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                            "@Page_ID", SqlDbType.Int, DataMain.Page_ID,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                               return Content("Phân Quyền Nhóm Theo Màn Hình Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Permission_GroupPage_Update]", CommandType.StoredProcedure,
                            "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                            "@Page_ID", SqlDbType.Int, DataMain.Page_ID,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@CurrentID ", SqlDbType.Int, CurrentID

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                               return Content("Phân Quyền Nhóm Theo Màn Hình Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
    public class PermissionGroupPage
    {
        public string Note { get; set; }
        public string Page_ID { get; set; }
        public string Group_ID { get; set; }
    }
}
