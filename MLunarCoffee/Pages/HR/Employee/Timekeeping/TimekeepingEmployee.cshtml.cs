using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.HR.Employee.Timekeeping
{
    public class TimekeepingEmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtBranch = new DataTable();
                    dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dtBranch.TableName = "Branch";
                    ds.Tables.Add(dtBranch);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtEmployee = new DataTable();
                    dtEmployee = await confunc.ExecuteDataTable("[YYY_sp_Timekeeping_Employee]",
                        CommandType.StoredProcedure);
                    dtEmployee.TableName = "Employee";
                    ds.Tables.Add(dtEmployee);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtGroupEmployee = new DataTable();
                    dtGroupEmployee = await confunc.ExecuteDataTable("[YYY_sp_Work_Scheduler_Employee_Group_LoadList]",
                        CommandType.StoredProcedure);
                    dtGroupEmployee.TableName = "GroupEmployee";
                    ds.Tables.Add(dtGroupEmployee);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Timekeeping_Load]", CommandType.StoredProcedure
                        ,"@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
