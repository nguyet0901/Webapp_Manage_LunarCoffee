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

using Microsoft.AspNetCore.Http.Extensions;
namespace MLunarCoffee.Pages.Student.Trains
{
    public class ClassDetail : PageModel
    {
        public string _ClassID { get; set; }
        public string _ClassTimeID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _ClassTimeID = curr.ToString();
            else _ClassTimeID = "0";

            string classid = Request.Query["ClassID"];
            if (classid != null) _ClassID = classid.ToString();
            else _ClassID = "0";
        }

        public async Task<IActionResult> OnPostLoadStudent(string ClassID,string ClassTimeID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_Student]", CommandType.StoredProcedure,
                        "@ClassID", SqlDbType.Int, ClassID,
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
        public async Task<IActionResult> OnPostLoadInfo(string ClassTimeID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_ClassDetail_Info]", CommandType.StoredProcedure,
                        "@ClassTimeID", SqlDbType.Int, ClassTimeID);
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
        public async Task<IActionResult> OnPostCheckInout(string ClassTimeID,string Status)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                int TeacherID = 1;
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_CheckInout]", CommandType.StoredProcedure,
                        "@ClassTimeID", SqlDbType.Int, ClassTimeID
                        , "@CurrentStatus", SqlDbType.Int, Status
                       , "@TeacherID", SqlDbType.Int, TeacherID
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
        public async Task<IActionResult> OnPostRevertCheckInout(string ClassTimeID, string Status)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                int TeacherID = 1;
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_ReCheckInout]", CommandType.StoredProcedure,
                        "@ClassTimeID", SqlDbType.Int, ClassTimeID
                        , "@CurrentStatus", SqlDbType.Int, Status
                        , "@TeacherID", SqlDbType.Int, TeacherID
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
        public async Task<IActionResult> OnPostRaattend(string ClassTimeID, string statusid, string type, string obj)
        {
            try
            {
                DataTable DataRaattend = new DataTable();
                DataRaattend = JsonConvert.DeserializeObject<DataTable>(obj);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if(type== "attend")
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_AttendInsert]", CommandType.StoredProcedure,
                            "@ClassTimeID", SqlDbType.Int, ClassTimeID
                            , "@StatusID", SqlDbType.Int, statusid
                            , "@data", SqlDbType.Structured, DataRaattend.Rows.Count > 0 ? DataRaattend : null
                            , "@Createdby", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_RatingInsert]", CommandType.StoredProcedure,
                            "@ClassTimeID", SqlDbType.Int, ClassTimeID
                            , "@StatusID", SqlDbType.Int, statusid
                            , "@data", SqlDbType.Structured, DataRaattend.Rows.Count > 0 ? DataRaattend : null
                            , "@Createdby", SqlDbType.Int, user.sys_userid);
                    }
                }

                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        
        
        public async Task<IActionResult> OnPostAttenNote(string ClassTimeID, string StudentID, string Note)
        {
            try
            {
                DataTable DataRaattend = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("[YYY_sp_ST_ClassDetail_NoteInsert]", CommandType.StoredProcedure
                        ,"@ClassTimeID", SqlDbType.Int, ClassTimeID
                        ,"@StudentID", SqlDbType.Int, StudentID
                        ,"@Note", SqlDbType.NVarChar, Note != null ? Note.Replace("'","").ToString() : null
                        ,"@Createdby", SqlDbType.Int, user.sys_userid);
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
