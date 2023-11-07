using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Course
{
    public class CourseTabListModel : PageModel
    {
        public string _CourseCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _CourseCurrentID = curr.ToString();
            else _CourseCurrentID = "0";
        }

        public async Task<IActionResult> OnPostLoadData(string Currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_Settings_Course_LoadTabListDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Currentid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int courseid, int studentid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Course_DeteleStudent]", CommandType.StoredProcedure,
                        "@CourseID", SqlDbType.Int, courseid,
                        "@StudentID", SqlDbType.Int, studentid,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null) return Content(Comon.DataJson.GetFirstValue(dt));
                return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostUpdateSign(int id, string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Student_Certi_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                         "@Sign", SqlDbType.NVarChar, sign
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
