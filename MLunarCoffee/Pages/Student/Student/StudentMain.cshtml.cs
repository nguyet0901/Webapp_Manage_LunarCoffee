using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Student
{
    public class StudentMainModel : PageModel
    {
        public string _studentid { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string studentid = Request.Query["studentid"];
            string _layout = Request.Query["layout"];

            if (studentid != null) _studentid = studentid.ToString();
            else _studentid = "0";

            if (_layout != null) layout = _layout;
            else layout = "";
        }
        public async Task<IActionResult> OnPostLoadInfo(int Studentid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_InfoLoad]", CommandType.StoredProcedure
                        , "@Studentid", SqlDbType.Int, Studentid);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadCourse(int Studentid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_InfoCourse]", CommandType.StoredProcedure
                        , "@Studentid", SqlDbType.Int, Studentid);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadCourseDetail(int Studentid, int Courseid, int StuCourseid)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_ST_InfoCourseDetail]", CommandType.StoredProcedure
                        , "@Studentid", SqlDbType.Int, Studentid
                         , "@Courseid", SqlDbType.Int, Courseid
                         , "@StuCourseid", SqlDbType.Int, StuCourseid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadClassTimeDetail(int ClassStudentID, int StudentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_ST_ClassScheduleDetail]", CommandType.StoredProcedure
                        , "@ClassStudentID", SqlDbType.Int, ClassStudentID
                        , "@StudentID", SqlDbType.Int, StudentID);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostLoadPaid(int Studentid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_InfoPaymentLoad]", CommandType.StoredProcedure
                        , "@StudentID", SqlDbType.Int, Studentid);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadPaidDetai(int Paymentid)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_ST_InfoPaymentDetail]", CommandType.StoredProcedure
                        , "@PaymentID", SqlDbType.Int, Paymentid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostDeleteItem(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Payment_Delete", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                         "@UserID", SqlDbType.Int, user.sys_userid,
                         "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("");
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
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Student_Certi_Update_Sign]", CommandType.StoredProcedure,
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