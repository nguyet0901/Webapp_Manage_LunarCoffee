using System;
using System.Collections.Generic;
using System.Data;
using System.Linq; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Settings.Course
{
    public class CourseDetailModel : PageModel
    {

        public string _CourseCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _CourseCurrentID = curr.ToString();
            else _CourseCurrentID = "0";
        }


        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Subject_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "Subjects";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ManuClass_LoadComBo]", CommandType.StoredProcedure);
                    dt.TableName = "ManuClass";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_TeacherAliasComBo]", CommandType.StoredProcedure);
                    dt.TableName = "Teachers";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ListCourseCombo]", CommandType.StoredProcedure);
                    dt.TableName = "ListCourse";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_Settings_Course_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string dataInfo, string dataClass, string dataClassTime)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtResult = new DataTable();
                CourseInfo dtInfo = JsonConvert.DeserializeObject<CourseInfo>(dataInfo);
                DataTable dtClass = JsonConvert.DeserializeObject<DataTable>(dataClass);
                DataTable dtClassTime = JsonConvert.DeserializeObject<DataTable>(dataClassTime);

                DataTable dtClassMain = new DataTable();
                dtClassMain.Columns.Add("ID", typeof(Int64));
                dtClassMain.Columns.Add("ItemPrice", typeof(decimal));
                dtClassMain.Columns.Add("TimeTeach", typeof(int));
                dtClassMain.Columns.Add("SubjectID", typeof(int));
                dtClassMain.Columns.Add("TimeBegin", typeof(int));
                dtClassMain.Columns.Add("TimeEnd", typeof(int));
                dtClassMain.Columns.Add("Price", typeof(decimal));
                dtClassMain.Columns.Add("MainTeacherID", typeof(int));
                dtClassMain.Columns.Add("StudentLimit", typeof(int));
                for (int i = 0; i < dtClass.Rows.Count; i++)
                {
                    DataRow drClass = dtClassMain.NewRow();
                    drClass["ID"] = Convert.ToInt64(dtClass.Rows[i]["ID"]);
                    drClass["ItemPrice"] = Convert.ToDecimal(dtClass.Rows[i]["ItemPrice"]);
                    drClass["Price"] = Convert.ToDecimal(dtClass.Rows[i]["Price"]);
                    drClass["TimeTeach"] = Convert.ToInt32(dtClass.Rows[i]["TimeTeach"]);
                    drClass["SubjectID"] = Convert.ToInt32(dtClass.Rows[i]["SubjectID"]);
                    drClass["TimeBegin"] = Convert.ToInt32(dtInfo.TimeBegin);
                    drClass["TimeEnd"] = Convert.ToInt32(dtInfo.TimeEnd);
                    drClass["MainTeacherID"] = Convert.ToInt32(dtClass.Rows[i]["MainTeacherID"]);
                    drClass["StudentLimit"] = Convert.ToInt32(dtClass.Rows[i]["StudentLimit"]);
                    dtClassMain.Rows.Add(drClass);
                }

                DataTable dtClassTimeMain = new DataTable();
                dtClassTimeMain.Columns.Add("ID", typeof(Int64));
                dtClassTimeMain.Columns.Add("ClassID", typeof(Int64));
                dtClassTimeMain.Columns.Add("ManuClassID", typeof(int));
                dtClassTimeMain.Columns.Add("DOW", typeof(int));
                dtClassTimeMain.Columns.Add("Dayint", typeof(int));
                dtClassTimeMain.Columns.Add("TimeStart", typeof(string));
                dtClassTimeMain.Columns.Add("TimeEnd", typeof(string));
                dtClassTimeMain.Columns.Add("TeacherID", typeof(int));



                for (int j = 0; j < dtClassTime.Rows.Count; j++)
                {
                    DataRow drClassTime = dtClassTimeMain.NewRow();
                    drClassTime["ID"] = Convert.ToInt64(dtClassTime.Rows[j]["ID"]);
                    drClassTime["ClassID"] = Convert.ToInt64(dtClassTime.Rows[j]["ClassID"]);
                    drClassTime["ManuClassID"] = Convert.ToInt32(dtClassTime.Rows[j]["Manuclass"]);
                    drClassTime["DOW"] = Convert.ToInt32(dtClassTime.Rows[j]["DOW"]);
                    drClassTime["Dayint"] = Convert.ToInt32(dtClassTime.Rows[j]["Dayint"]);
                    drClassTime["TimeStart"] = dtClassTime.Rows[j]["TimeStart"];
                    drClassTime["TimeEnd"] = dtClassTime.Rows[j]["TimeEnd"];
                    drClassTime["TeacherID"] = Convert.ToInt32(dtClassTime.Rows[j]["Teacher"]);
                    dtClassTimeMain.Rows.Add(drClassTime);
                }

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Course_Insert]", CommandType.StoredProcedure,
                            "@ListCourseID", SqlDbType.Int, dtInfo.ListCourseID,
                            "@Code", SqlDbType.NVarChar, dtInfo.Code,
                            "@BranchID", SqlDbType.NVarChar, dtInfo.BranchID,
                            "@Price", SqlDbType.Decimal, dtInfo.Price,
                            "@TimeBegin", SqlDbType.Int, dtInfo.TimeBegin,
                            "@TimeEnd", SqlDbType.Int, dtInfo.TimeEnd,
                            "@Note", SqlDbType.NVarChar, dtInfo.Note,
                            "@DTClass", SqlDbType.Structured, dtClassMain.Rows.Count > 0 ? dtClassMain : null,
                            "@DTClassTime", SqlDbType.Structured, dtClassTimeMain.Rows.Count > 0 ? dtClassTimeMain : null,
                            "@CreatedBy", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Course_Update]", CommandType.StoredProcedure,
                             "@ListCourseID", SqlDbType.Int, dtInfo.ListCourseID,
                            "@CourseID", SqlDbType.Int, CurrentID,
                            "@Code", SqlDbType.NVarChar, dtInfo.Code,
                            "@BranchID", SqlDbType.NVarChar, dtInfo.BranchID,
                            "@Price", SqlDbType.Decimal, dtInfo.Price,
                            "@TimeBegin", SqlDbType.Int, dtInfo.TimeBegin,
                            "@TimeEnd", SqlDbType.Int, dtInfo.TimeEnd,
                            "@Note", SqlDbType.NVarChar, dtInfo.Note,
                            "@DTClass", SqlDbType.Structured, dtClassMain.Rows.Count > 0 ? dtClassMain : null,
                             "@DTClassTime", SqlDbType.Structured, dtClassTimeMain.Rows.Count > 0 ? dtClassTimeMain : null,
                            "@ModifiedBy", SqlDbType.Int, user.sys_userid);
                    }
                }

                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }

    public class CourseInfo
    {
        public int ListCourseID { get; set; }
        public string Code { get; set; }
        public int BranchID { get; set; }
        public decimal Price { get; set; }
        public int TimeBegin { get; set; }
        public int TimeEnd { get; set; }
        public string Note { get; set; }
    }

}
