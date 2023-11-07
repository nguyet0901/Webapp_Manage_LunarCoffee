using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Trains
{
    public class ClassAdjustmentModel : PageModel
    {
        public string _ClassTimeID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _ClassTimeID = curr.ToString();
            else _ClassTimeID = "0";
        }
        public async Task<IActionResult> OnPostLoadData(string ClassTimeID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_Info]", CommandType.StoredProcedure,
                        "@ClassTimeID", SqlDbType.Int, ClassTimeID);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                ClassTimeItem DataMain = JsonConvert.DeserializeObject<ClassTimeItem>(data);
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_ST_ClassTime_Update", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, CurrentID,
                      "@DayInt", SqlDbType.Int, DataMain.DayInt,
                      "@TimeStart", SqlDbType.NVarChar, DataMain.TimeStart,
                      "@TimeEnd", SqlDbType.NVarChar, DataMain.TimeEnd,
                      "@TeacherID", SqlDbType.Int, DataMain.TeacherID,
                      "@ManuClassID", SqlDbType.Int, DataMain.ManuClassID,
                      "@IsDisabled", SqlDbType.Int, DataMain.IsDisabled
                    );
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
    public class ClassTimeItem
    {
        public int DayInt { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int TeacherID { get; set; }
        public int ManuClassID { get; set; }
        public int IsDisabled { get; set; }
    }
}
