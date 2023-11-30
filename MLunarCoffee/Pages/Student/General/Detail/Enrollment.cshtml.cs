using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.General.Detail
{
    public class EnrollmentModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Training_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
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

        public async Task<IActionResult> OnPostLoadComboCourse(string DateFrom, string DateTo, string BranchID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Course_LoadCombo]", CommandType.StoredProcedure,
                        "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom),
                        "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo),
                        "@BranchID", SqlDbType.Int, BranchID);
                    dt.TableName = "Course";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Discount_LoadCombo]", CommandType.StoredProcedure,
                        "@BranchID", SqlDbType.Int, BranchID);
                    dt.TableName = "Discount";
                    ds.Tables.Add(dt);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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

        public async Task<IActionResult> OnPostLoadData(string CourseID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Class_LoadByCourse]", CommandType.StoredProcedure
                        , "@courseID", SqlDbType.Int, CourseID);
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


        public async Task<IActionResult> OnPostExcutedData(string obj, string courseID, string totalPrice)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtStudent = JsonConvert.DeserializeObject<DataTable>(obj);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("StudentID", typeof(int));
                dtMain.Columns.Add("ClassID", typeof(int));
                dtMain.Columns.Add("DiscountPercent", typeof(int));
                dtMain.Columns.Add("DiscountAmount", typeof(decimal));
                dtMain.Columns.Add("PriceInit", typeof(decimal));
                dtMain.Columns.Add("Price", typeof(decimal));
                dtMain.Columns.Add("Note", typeof(string));
                for (int i = 0; i < dtStudent.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["StudentID"] = Convert.ToInt32(dtStudent.Rows[i]["StudentID"]);
                    dr["ClassID"] = Convert.ToDecimal(dtStudent.Rows[i]["ClassID"]);
                    dr["DiscountPercent"] = Convert.ToInt32(dtStudent.Rows[i]["DiscountPer"]);
                    dr["DiscountAmount"] = Convert.ToDecimal(dtStudent.Rows[i]["DiscountAmount"]);
                    dr["PriceInit"] = Convert.ToDecimal(dtStudent.Rows[i]["PriceInit"]);
                    dr["Price"] = Convert.ToDecimal(dtStudent.Rows[i]["Price"]);
                    dr["Note"] = dtStudent.Rows[i]["Note"];
                    dtMain.Rows.Add(dr);
                }
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Student_Enrollment]", CommandType.StoredProcedure
                        , "@table", SqlDbType.Structured, dtMain.Rows.Count > 0 ? dtMain : null
                        , "@courseID", SqlDbType.Int, Convert.ToInt32(courseID)
                        , "@totalPrice", SqlDbType.Decimal, Convert.ToDecimal(totalPrice)
                        , "@UserID", SqlDbType.Int, user.sys_userid);
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
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
