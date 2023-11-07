using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.General.Course
{
    public class CourseExaminationModel : PageModel
    {
        public string _CourseID { get; set; }
        public string _AxamID { get; set; }
        public void OnGet()
        {
            string CourseID = Request.Query["CourseID"];
            string AxamID = Request.Query["AxamID"];
            _CourseID = (CourseID != null) ? CourseID.ToString() : "0";
            _AxamID = (AxamID != null) ? AxamID.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInit(string courseid, string axamid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_StatusLevel_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "StatusLevel";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Settings_Examination_LoadCombo]", CommandType.StoredProcedure,
                                    "@CourseID", SqlDbType.Int, courseid,
                                    "@AxamID", SqlDbType.Int, axamid);
                    dt.TableName = "Examination";
                    ds.Tables.Add(dt);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string courseid, string examid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_CourseExam_Detail]", CommandType.StoredProcedure,
                        "@CourseID", SqlDbType.Int, courseid,
                        "@ExamID", SqlDbType.Int, examid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data, string examid, string courseid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtResult = new DataTable();
                DataTable dtCourseStu = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtStuMain = new DataTable();
                dtStuMain.Columns.Add("StudentID", typeof(int));
                dtStuMain.Columns.Add("LevelStatus", typeof(int));
                for (int i = 0; i < dtCourseStu.Rows.Count; i++)
                {
                    DataRow drStu = dtStuMain.NewRow();
                    drStu["StudentID"] = Convert.ToInt32(dtCourseStu.Rows[i]["StudentID"]);
                    drStu["LevelStatus"] = Convert.ToInt32(dtCourseStu.Rows[i]["LevelStatus"]);
                    dtStuMain.Rows.Add(drStu);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtResult = await connFunc.ExecuteDataTable("[YYY_sp_ST_CourseAxam_StudentStatus_Update]", CommandType.StoredProcedure
                        ,"@DtStudent", SqlDbType.Structured, dtStuMain.Rows.Count > 0 ? dtStuMain : null
                        ,"@ExamID", SqlDbType.Int, examid
                        ,"@CourseID", SqlDbType.Int, courseid
                        ,"@UserID", SqlDbType.Int, user.sys_userid);
                }
                return Content(Comon.DataJson.GetFirstValue(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostDeleteItem(string examid, string courseid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_CourseAxam_Delete]", CommandType.StoredProcedure
                        , "@ExamID", SqlDbType.Int, examid
                        , "@CourseID", SqlDbType.Int, courseid
                        , "@ModifiedBy", SqlDbType.Int, user.sys_userid
                    );
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
