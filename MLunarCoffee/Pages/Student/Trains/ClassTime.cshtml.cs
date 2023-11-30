using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;
namespace MLunarCoffee.Pages.Student.Trains
{
    public class ClassTime : PageModel
    {
        public string layout { get; set; }
        public int _branchID { get; set; }
        public int _employeeID_Main { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            _employeeID_Main = user.sys_employeeid;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_AttendLoadComBo", CommandType.StoredProcedure);
                    dt.TableName = "Attend";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_RatingLoadComBo", CommandType.StoredProcedure);
                    dt.TableName = "Rating";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_ManuClass_LoadComBo", CommandType.StoredProcedure);
                    dt.TableName = "Class";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_Subject_LoadComBo", CommandType.StoredProcedure);
                    dt.TableName = "Subject";
                    ds.Tables.Add(dt);
                }


                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("MLU_sp_ST_TeacherAliasComBo", CommandType.StoredProcedure);
                    dt.TableName = "Teacher";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadTime(string teacherID, string classid, string subjectid, string branchID, string date_from, string date_to)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_ClassTimeLoad]", CommandType.StoredProcedure,
                        "@TeacherID", SqlDbType.Int, teacherID
                         , "@branchid", SqlDbType.Int, branchID
                         , "@subjectid", SqlDbType.Int, subjectid
                         , "@classid", SqlDbType.Int, classid
                         , "@date_from", SqlDbType.DateTime, date_from
                         , "@date_to", SqlDbType.DateTime, date_to
                         , "@ClassTimeID", SqlDbType.Int, 0);
                }
                if (dt != null)
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else
                    {
                        return Content("[]");
                    }
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
        public async Task<IActionResult> OnPostLoadSpecific(string classtimeid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_ClassTimeSpecific]", CommandType.StoredProcedure,
                        "@classtimeid", SqlDbType.Int, classtimeid);
                }
                if (dt != null)
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                    else
                    {
                        return Content("[]");
                    }
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
    }
}
