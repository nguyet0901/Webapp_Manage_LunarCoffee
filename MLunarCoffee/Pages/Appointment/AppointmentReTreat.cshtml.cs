using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class AppointmentReTreatModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _EmployeeID { get; set; }

        public void OnGet()
        {
            string Cus = Request.Query["CustomerID"];
            if (Cus != null)
            {
                var user = Session.GetUserSession(HttpContext);
                _EmployeeID = user.sys_employeeid.ToString();
                _CustomerID = Cus.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostCheckAppReTreat(int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_Check_AppRetreat_Exist" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,CustomerID
                      ,"@DateFrom" ,SqlDbType.DateTime ,DateTime.Now
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataReTreatInfo(int CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Appointment_ReTreat_Info_Load" ,CommandType.StoredProcedure ,
                          "@CustomerID" ,SqlDbType.Int ,CustomerID
                        );
                        dt.TableName = "ReTreatContent";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.LoadEmployee(14 ,user.sys_branchID ,user.sys_AllBranchID);
                        dt.TableName = "Doctor";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Schedule_Type_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "ScheduleType";
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
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string CustomerID ,string data ,string _Content ,string DoctorID ,string ScheduleTypeID)
        {
            try
            {
                _Content = (_Content != null ? _Content : "");
                DoctorID = (DoctorID != null ? DoctorID : "0");
                DataTable dtMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_ReTreat_Insert" ,CommandType.StoredProcedure ,
                         "@CustomerID" ,SqlDbType.Int ,CustomerID
                       ,"@Content" ,SqlDbType.NVarChar ,_Content
                       ,"@Created_By" ,SqlDbType.Int ,user.sys_userid
                       ,"@BranchID" ,SqlDbType.Int ,user.sys_branchID
                       ,"@DoctorID" ,SqlDbType.Int ,Convert.ToInt32(DoctorID)
                       ,"@ScheduleTypeID" ,SqlDbType.Int ,Convert.ToInt32(ScheduleTypeID)
                       ,"@data_table" ,SqlDbType.Structured ,((DataTable)dtMain).Rows.Count > 0 ? (DataTable)dtMain : null
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
