using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.General.Course
{
    public class CourseStatusModel : PageModel
    {
        public string _CourseID { get; set; }
        public void OnGet()
        {
            string CourseID = Request.Query["CourseID"];
            _CourseID = (CourseID != null) ? CourseID.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInit()
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
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_StatusRevi_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "StatusRevi";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_StatusCerti_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "StatusCerti";
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

        public async Task<IActionResult> OnPostLoadDetail(string courseid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_Course_StudentStatus]", CommandType.StoredProcedure,
                        "@CourseID", SqlDbType.Int, courseid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtResult = new DataTable();
                DataTable dtCourseStu = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtStuMain = new DataTable();
                dtStuMain.Columns.Add("ID", typeof(int));
                dtStuMain.Columns.Add("ReviStatus", typeof(int));
                dtStuMain.Columns.Add("LevelStatus", typeof(int));
                dtStuMain.Columns.Add("CerID", typeof(int));
                for (int i = 0; i < dtCourseStu.Rows.Count; i++)
                {
                    DataRow drStu = dtStuMain.NewRow();
                    drStu["ID"] = Convert.ToInt32(dtCourseStu.Rows[i]["ID"]);
                    drStu["ReviStatus"] = Convert.ToInt32(dtCourseStu.Rows[i]["ReviStatus"]);
                    drStu["LevelStatus"] = Convert.ToInt32(dtCourseStu.Rows[i]["LevelStatus"]);
                    drStu["CerID"] = Convert.ToInt32(dtCourseStu.Rows[i]["CertiStatus"]);
                    dtStuMain.Rows.Add(drStu);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtResult = await connFunc.ExecuteDataTable("[YYY_sp_ST_Course_StudentStatus_Update]", CommandType.StoredProcedure,
                        "@DtStudent", SqlDbType.Structured, dtStuMain.Rows.Count > 0 ? dtStuMain : null,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                }
                return Content(Comon.DataJson.GetFirstValue(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
