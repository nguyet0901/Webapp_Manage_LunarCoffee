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
    public class PermissionPageDetailModel : PageModel
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
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Permission_PageLoadDetail]", CommandType.StoredProcedure,
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

         public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataTable dt = new DataTable();
            //LoadLanguage
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.LoadPara("Per_Page");
            }
            return Content(Comon.DataJson.Datatable(dt));
        }



         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PermissionPage DataMain = JsonConvert.DeserializeObject<PermissionPage>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Page_Insert]", CommandType.StoredProcedure,
                            "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                              "@PageName ", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@GroupID", SqlDbType.Int, DataMain.GroupID,
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Permission_Page_Update]", CommandType.StoredProcedure,
                            "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                            "@PageName", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@GroupID", SqlDbType.Int, DataMain.GroupID,
                            "@CurrentID ", SqlDbType.Int, CurrentID,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                               return Content("Tên Màn Hình Đã Tồn Tại");
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
    public class PermissionPage
    {

        public string Value { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public int GroupID { get; set; }
    }
}
