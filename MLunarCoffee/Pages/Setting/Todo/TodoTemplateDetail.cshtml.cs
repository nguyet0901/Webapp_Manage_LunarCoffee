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
    public class TodoTemplateDetailModel : PageModel
    {
        public string _TodoTemlateID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TodoTemlateID = curr.ToString();
            }
            else
            {
                _TodoTemlateID = "0";
            }
        }
        
         public async Task<IActionResult> OnPostLoaDataUpdate(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Todo_Temlate_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
                TodoTemplate DataMain = JsonConvert.DeserializeObject<TodoTemplate>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Todo_Template_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Color", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim(),
                              "@Descript", SqlDbType.NVarChar, DataMain.Descript.Replace("'", "").Trim()
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Todo_Template_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                             "@Modified_by", SqlDbType.Int, user.sys_userid,
                             "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@currentID", SqlDbType.Int, CurrentID,
                             "@Color", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim(),
                             "@Descript", SqlDbType.NVarChar, DataMain.Descript.Replace("'", "").Trim()
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
                    //}
                }

            }

            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class TodoTemplate
        {
            public string Name { get; set; }
            public string ColorCode { get; set; }
            public string Descript { get; set; }
        }
    }
}
