using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.General.Course
{
    public class CourseListModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadData(string branchID, string dateFrom, string dateTo, string beginID, string beginClassID, string limit)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_ST_Course_LoadList]", CommandType.StoredProcedure,
                        "@BranchID", SqlDbType.Int, branchID,
                        "@Limit", SqlDbType.Int, limit,
                        "@BeginID", SqlDbType.Int, beginID,
                        "@BeginClassID", SqlDbType.Int, beginClassID,
                        "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                        "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostLoadCourseClass(string courseID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_CourseClass_LoadDetail]", CommandType.StoredProcedure,
                        "@CourseID", SqlDbType.Int, courseID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }


    }
}
