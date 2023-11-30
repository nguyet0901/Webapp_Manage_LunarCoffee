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

namespace MLunarCoffee.Pages.Setting
{
    public class EmployeeLevelDetailModel : PageModel
    {
        public string _EmployeeLevelID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _EmployeeLevelID = curr.ToString();
            }
            else
            {
                _EmployeeLevelID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Employee_Level_LoadDetail]", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostExcuteData(string NameLevel, string Note, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Employee_Level_Insert]", CommandType.StoredProcedure,
                            "@LevelName", SqlDbType.Int, NameLevel,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Note", SqlDbType.NVarChar, Note
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
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
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Employee_Level_Update]", CommandType.StoredProcedure,
                            "@LevelName", SqlDbType.NVarChar, NameLevel,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Note", SqlDbType.NVarChar, Note

                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
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
}
