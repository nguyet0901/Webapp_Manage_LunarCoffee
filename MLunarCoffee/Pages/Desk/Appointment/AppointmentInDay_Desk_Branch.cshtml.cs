using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Desk.Appointment
{
    public class AppointmentInDay_Desk_BranchModel : PageModel
    {
        public string _Status { get; set; }

        public int _branchID { get; set; }
        public void OnGet()
        {
            string Status = Request.Query["Type"];
            if (Status != null)
            {
                _Status = Status.ToString();
            }
            else
            {
                _Status = "-1";
            }
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;

        }

        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_LoadType", CommandType.StoredProcedure);
                        dt.TableName = "Type";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Scheduler_Type_LoadList", CommandType.StoredProcedure);
                        dt.TableName = "SchedulerType";
                        //DataRow dr = dt.NewRow();
                        //dr[0] = 0;
                        //dr[1] = "<span>Tất Cả Loại</span>";
                        //dt.Rows.InsertAt(dr, 0);
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployee(14, user.sys_branchID, user.sys_AllBranchID);
                        dt.TableName = "Doctor";
                        //DataRow dr = dt.NewRow();
                        //dr[0] = 0;
                        //dr[1] = "<span>Tất Cả Bác Sĩ</span>";
                        //dr[2] = "All";
                        //dt.Rows.InsertAt(dr, 0);
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Schedule_CbbStatus]", CommandType.StoredProcedure);
                        dt.TableName = "Status";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_MemberLoad]", CommandType.StoredProcedure);
                        dt.TableName = "Membership";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_EmployeeFull]", CommandType.StoredProcedure);
                        dt.TableName = "Employee";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                        dt.TableName = "Source";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ServiceCare_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "ServiceCare";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Scheduler_ReasonCancel_LoadData]" ,CommandType.StoredProcedure);
                        dt.TableName = "ReasonCancel";
                        return dt;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataAppointmentList(int BranchID, int AppID, string DateFrom, int StatusID, int DoctorID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Appointment_Indayv2]", CommandType.StoredProcedure
                        , "@AppID", SqlDbType.Int, AppID
                        , "@DoctorID", SqlDbType.Int, DoctorID
                        , "@StatusID", SqlDbType.Int, StatusID
                        , "@BranchID", SqlDbType.Int, BranchID
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom));
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
