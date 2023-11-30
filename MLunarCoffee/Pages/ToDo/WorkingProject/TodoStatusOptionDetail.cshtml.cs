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

namespace MLunarCoffee.Pages.ToDo.WorkingProject
{
    public class TodoStatusOptionDetailModel : PageModel
    {
        public string _TodoStatusID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TodoStatusID = curr.ToString();
            }
            else
            {
                _TodoStatusID = "0";
            }
        }
        
         public async Task<IActionResult> OnPostLoaDataUpdate(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Todo_TaskStatus_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                TodoStatus DataMain = JsonConvert.DeserializeObject<TodoStatus>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_TaskStatus_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Color ", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_TaskStatus_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                             "@Modified_by", SqlDbType.Int, user.sys_userid,
                             "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@currentID", SqlDbType.Int, CurrentID,
                             "@Color ", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim()
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                    //}
                }

            }

            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class TodoStatus
        {
            public string Name { get; set; }
            public string ColorCode { get; set; }
        }
    }
}
