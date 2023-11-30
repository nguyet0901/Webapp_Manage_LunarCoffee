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
using MLunarCoffee.Pages.Setting.ContentTemplate;

namespace MLunarCoffee.Pages.Student.Settings.Subjects
{
    public class ListCourseDetailModel : PageModel
    {
        public string CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                CurrentID = curr.ToString();
            }
            else
            {
                CurrentID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadData(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_ListCourse_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                    );
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
        public async Task<IActionResult> OnPostExecuted(string DataCourse, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt_main = JsonConvert.DeserializeObject<DataTable>(DataCourse);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (CurrentID.Trim() == "0")
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_ListCourse_Insert]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, dt_main.Rows[0]["Name"].ToString().Replace("'", "").Trim()
                            , "@Color", SqlDbType.NVarChar, dt_main.Rows[0]["Color"].ToString().Replace("'", "").Trim()
                            , "@Note", SqlDbType.NVarChar, dt_main.Rows[0]["Note"].ToString().Replace("'", "").Trim()
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                    else
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_ListCourse_Update]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, dt_main.Rows[0]["Name"].ToString().Replace("'", "").Trim()
                            , "@Color", SqlDbType.NVarChar, dt_main.Rows[0]["Color"].ToString().Replace("'", "").Trim()
                            , "@Note", SqlDbType.NVarChar, dt_main.Rows[0]["Note"].ToString().Replace("'", "").Trim()
                            , "@Modified_By", SqlDbType.Int, user.sys_userid
                            , "@currentID ", SqlDbType.Int, CurrentID
                        );
                    }
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}