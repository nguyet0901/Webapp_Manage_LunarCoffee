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
    public class PermissionGroupControlDetailModel : PageModel
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
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Permission_GroupControl_LoadDetail]", CommandType.StoredProcedure,
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
                dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Combo_Control]", CommandType.StoredProcedure);
                dt.TableName = "_DataControl";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Combo_Group]", CommandType.StoredProcedure);
                dt.TableName = "_DataGroupUser";
                ds.Tables.Add(dt);
            }

            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Combo_Page]", CommandType.StoredProcedure);
                dt.TableName = "_DataPage";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await connFunc.LoadPara("Per_Page");
                dt.TableName = "_DataGroupPage";
                ds.Tables.Add(dt);
            }
            return Content(Comon.DataJson.Dataset(ds));
        }



         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PermissionGroupControl DataMain = JsonConvert.DeserializeObject<PermissionGroupControl>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Group_Control_Insert]", CommandType.StoredProcedure,
                            "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                            "@Control_ID", SqlDbType.Int, DataMain.Control_ID,
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Group_Control_Update]", CommandType.StoredProcedure,
                            "@CurrentID ", SqlDbType.Int, CurrentID,
                            "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                            "@Control_ID", SqlDbType.Int, DataMain.Control_ID,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                               return Content("Phân Quyền Nhóm Theo Control Đã Tồn Tại. Vui Lòng Kiểm Tra Lại");
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
    public class PermissionGroupControl
    {
        public string Note { get; set; }
        public string Control_ID { get; set; }
        public string Group_ID { get; set; }
    }
}

