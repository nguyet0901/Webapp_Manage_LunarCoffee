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
using MLunarCoffee.Models.Model.AlrmScheduler;

namespace MLunarCoffee.Pages.ToDo.AlarmSchedule
{
    public class AlarmScheduleDetailModel : PageModel
    {
        public string CurrentID { get; set; }
        public string CurrentType { get; set; }
        public string data_TypeTime { get; set; }
        public string data_DayWeek { get; set; }
        public string data_Employee { get; set; }
        public string Current_EmployeeID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            Current_EmployeeID = user.sys_employeeid.ToString();
            string Current = Request.Query["CurrentID"];
            string currentType = Request.Query["CurrentType"];
            if (Current != null)
            {
                CurrentID = Current;
            }
            else
            {
                CurrentID = "0";
            }
            if (currentType != null)
            {
                CurrentType = currentType;
            }
            else
            {
                CurrentType = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadInitialize()
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            Current_EmployeeID = user.sys_employeeid.ToString();
            DataTable dt = new DataTable();
            DataTable data_TypeTime = InitializeTypeTimeCombo();
            data_TypeTime.TableName = "dataTypeTime";
            ds.Tables.Add(data_TypeTime);
            DataTable data_DayWeek = InitializeDayWeekCombo();
            data_DayWeek.TableName = "dataDayWeek";
            ds.Tables.Add(data_DayWeek);
            DataTable data_Employee =await LoadEmployeeCombo(user);
            data_Employee.TableName = "dataEmployee";
            ds.Tables.Add(data_Employee);
            return Content(Comon.DataJson.Dataset(ds));
        }

        private DataTable InitializeTypeTimeCombo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(String));
            dt.Columns.Add("Name", typeof(String));

            DataRow dr = dt.NewRow();
            dr["ID"] = 1;
            dr["Name"] = "Fixed Date";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 2;
            dr["Name"] = "Daily";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 3;
            dr["Name"] = "Weekly";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 4;
            dr["Name"] = "Monthly";
            dt.Rows.Add(dr);
            return dt;
        }
        private DataTable InitializeDayWeekCombo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(String));
            dt.Columns.Add("Name", typeof(String));

            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["Name"] = "Sunday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 1;
            dr["Name"] = "Monday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 2;
            dr["Name"] = "Tuesday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 3;
            dr["Name"] = "Wednesday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 4;
            dr["Name"] = "Thursday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 5;
            dr["Name"] = "Friday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = 6;
            dr["Name"] = "Saturday";
            dt.Rows.Add(dr);

            return dt;
        }

        public async Task<IActionResult> OnPostLoadata(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Detail]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID
                          );
                }
                if (dt != null)
                {
                    AlrmScheduler[] timedata = Comon.Comon.GetArrayTimeLine_AlarmScheduler(dt);
                    return Content( JsonConvert.SerializeObject(timedata));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }

        public static async Task<DataTable>  LoadEmployeeCombo(GlobalUser user)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_LoadCombo_EmployeeFull]", CommandType.StoredProcedure,
                            "@Branch_ID", SqlDbType.Int, user.sys_branchID,
                            "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                          );
                }
                if (dt != null)
                {

                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IActionResult> OnPostExecute(string CurrentID, string Title, string content, int Employee, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (CurrentID == "0")
                    {
                        dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Insert]", CommandType.StoredProcedure,
                            "@Employee", SqlDbType.Int, Employee,
                             "@CreatedBy", SqlDbType.Int, user.sys_userid,
                            "@Title", SqlDbType.NVarChar, Title,
                            "@Content", SqlDbType.NVarChar, content,
                            "@Data", SqlDbType.NVarChar, data
                          );
                    }
                    else
                    {
                        dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Update]", CommandType.StoredProcedure,
                           "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Employee", SqlDbType.Int, Employee,
                            "@CreatedBy", SqlDbType.Int, user.sys_userid,
                           "@Title", SqlDbType.NVarChar, Title,
                           "@Content", SqlDbType.NVarChar, content,
                           "@Data", SqlDbType.NVarChar, data
                         );
                    }
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
