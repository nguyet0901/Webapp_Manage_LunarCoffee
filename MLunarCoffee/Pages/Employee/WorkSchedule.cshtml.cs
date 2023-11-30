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
using MLunarCoffee.Models.Model.WorkScheduler;

namespace MLunarCoffee.Pages.Employee
{
    public class WorkScheduleModel : PageModel
    {

        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
        }

        public async Task<IActionResult> OnPostLoadInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid
                            , "@LoadNormal", SqlDbType.Int, 1);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroup = new DataTable();
                        dtGroup = await confunc.ExecuteDataTable("[MLU_sp_Work_Scheduler_Employee_Group_LoadList]", CommandType.StoredProcedure);
                        dtGroup.TableName = "GroupEmployee";
                        return dtGroup;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroup = new DataTable();
                        dtGroup = await confunc.ExecuteDataTable("[MLU_sp_Work_Scheduler_LoadShift]", CommandType.StoredProcedure);
                        dtGroup.TableName = "Shift";
                        return dtGroup;
                    }
                }));
                 
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadEmp(int BranchID, string GroupID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt = await connFunc.ExecuteDataTable("[MLU_sp_Work_Scheduler_Employee_Load]", CommandType.StoredProcedure,
                    "@BranchID", SqlDbType.Int, BranchID,
                    "@GroupID", SqlDbType.Int, GroupID 
                );
            }
            if (dt != null && dt.Rows.Count!=0)
                return Content(Comon.DataJson.Datatable(dt));
           else return Content("[]");
        }




        public async Task<IActionResult> OnPostLoadScheduler(int employeeID, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                ds = await connFunc.ExecuteDataSet("[MLU_sp_WorK_Schedule_LoadList]", CommandType.StoredProcedure,
                    "@EmployeeID", SqlDbType.Int, employeeID,
                    "@dateFrom", SqlDbType.DateTime, dateFrom,
                    "@dateTo", SqlDbType.DateTime, dateTo
                );
            }
            return Content(Comon.DataJson.Dataset(ds));
         
        }

        public async Task<IActionResult> OnPostOffScheduler(int employeeID, string date)
        {
           try
            {
                WorkScheduler_Extension[] ws = new WorkScheduler_Extension[1];
                shifts s = new shifts();
                shifts[] ss = new shifts[1];
                ss[0] = s;

                WorkScheduler_Extension w = new WorkScheduler_Extension();
                w.off = true;
                w.shift = ss;
                ws[0] = w;

                 var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                     await connFunc.ExecuteDataTable("[MLU_sp_WorK_Schedule_OFF]", CommandType.StoredProcedure,
                        "@Employee", SqlDbType.Int, employeeID,
                         "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(ws),
                        "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(date + " 00:00")
                    );
                }
                 return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
