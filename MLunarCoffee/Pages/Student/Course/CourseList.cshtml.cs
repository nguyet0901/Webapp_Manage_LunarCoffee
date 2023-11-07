using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Course
{
    public class CourseListModel : PageModel
    {
        public string layout { get; set; }
        public int _branchID { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null) layout = _layout;
            else layout = "";
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
        }
        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Subject_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "Subject";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_StatusRevi_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "Revision";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_StatusLevel_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "Level";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Training_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ListCourseCombo]", CommandType.StoredProcedure);
                    dt.TableName = "ListCourse";
                    ds.Tables.Add(dt);
                }



                if (dt != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadData(string BranchID, string DateFrom, string DateTo, string BeginID, string Limit, string Currentid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Settings_Course_LoadList]", CommandType.StoredProcedure,
                        "@BranchID", SqlDbType.Int, BranchID,
                        "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom),
                        "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo),
                        "@BeginID", SqlDbType.Int, BeginID,
                        "@CurrentID", SqlDbType.Int, Currentid,
                        "@Limit", SqlDbType.Int, Limit);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string Currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_Settings_Course_LoadListDetail]", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Course_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@ModifiedBy", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostDisableItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Course_Disabled]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@ModifiedBy", SqlDbType.Int, user.sys_userid
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
