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

namespace MLunarCoffee.Pages.Setting.Todo
{
    public class TodoQuickTemplateDetailModel : PageModel
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
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("YYY_sp_Todo_Quick_Template_Load", CommandType.StoredProcedure,
                  "@QuickID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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


        //XỬ LÍ THÊM SỬA DETAIL

        public async Task<IActionResult> OnPostExcuteDataDetail(string data, string CurrentID)
        {
            try
            {
                TodoQuickTemplate DataMain = JsonConvert.DeserializeObject<TodoQuickTemplate>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Todo_Quick_Template_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.VarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                          "@Created_By", SqlDbType.Int, user.sys_userid,
                          "@Created", SqlDbType.Int, DateTime.Now
                   );

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");
                            }
                            else
                            {
                                return Content("1");
                            }
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Todo_Quick_Template_Update]", CommandType.StoredProcedure,
                          "@QuickID", SqlDbType.Int, CurrentID,
                          "@Name", SqlDbType.VarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                          "@Modified_By", SqlDbType.Int, user.sys_userid,
                          "@Modified", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");
                            }
                            else
                            {
                                return Content("1");
                            }
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
    }
    public class TodoQuickTemplate
    {
        public int QuickID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}

