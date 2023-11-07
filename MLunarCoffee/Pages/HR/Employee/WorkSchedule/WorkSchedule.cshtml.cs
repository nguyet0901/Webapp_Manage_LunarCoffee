using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.HR.Employee.WorkSchedule
{
    public class WorkScheduleModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadEmp(int BranchID ,string GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Work_Scheduler_Employee_Load]" ,CommandType.StoredProcedure ,
                        "@BranchID" ,SqlDbType.Int ,BranchID ,
                        "@GroupID" ,SqlDbType.Int ,GroupID
                    );
                }
                if (dt != null && dt.Rows.Count != 0)
                    return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string dateFrom ,string dateTo ,string tokenEmpID)
        {
            try
            {
                tokenEmpID = (tokenEmpID != null ? tokenEmpID : "");
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_v2_Work_Schedule_Appointment_Doctor]" ,CommandType.StoredProcedure ,
                          "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                          ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                          ,"@TokenDocID" ,SqlDbType.NVarChar ,tokenEmpID
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        //public async Task<IActionResult> OnPostLoadData(int employeeID ,int groupID ,string dateFrom ,string dateTo)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await connFunc.ExecuteDataSet("[YYY_sp_rp_HR_Work_Schedule_LoadList]" ,CommandType.StoredProcedure ,
        //                "@EmployeeID" ,SqlDbType.Int ,employeeID ,
        //                "@groupID" ,SqlDbType.Int ,groupID ,
        //                "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
        //                "@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
        //            );
        //        }
        //        return Content(Comon.DataJson.Dataset(ds));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}
    }
}
