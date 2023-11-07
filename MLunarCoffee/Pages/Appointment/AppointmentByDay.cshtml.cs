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
    public class AppointmentByDayModel : PageModel
    {

        public int _branchID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
  
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
                        dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_LoadType" ,CommandType.StoredProcedure);
                        dt.TableName = "Type";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_EmployeeFull]" ,CommandType.StoredProcedure);
                        dt.TableName = "Employee";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Schedule_CbbStatus]" ,CommandType.StoredProcedure);
                        dt.TableName = "Status";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployee(14 ,user.sys_branchID ,user.sys_AllBranchID);
                        dt.TableName = "Doctor";

                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_MemberLoad]" ,CommandType.StoredProcedure);
                        dt.TableName = "Membership";

                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Scheduler_Type_LoadList" ,CommandType.StoredProcedure);
                        dt.TableName = "SchedulerType";
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
                //using (var jsonConvert = new DataToJson(ref ds))
                //{
                //    return Content(jsonConvert.DataSetToJSON());
                //}

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadTotal(string dataTotal)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataDetail = JsonConvert.DeserializeObject<DataTable>(dataTotal);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Appointment_LoadByDayv2All]" ,CommandType.StoredProcedure
                      ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataDetail.Rows[0]["dateFrom"].ToString())
                      ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataDetail.Rows[0]["dateTo"].ToString())
                      ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["branchID"])
                      ,"@areaBranch" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                      ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                      ,"@type" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["type"])
                      ,"@status" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["status"])
                      ,"@doctor" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["doctor"])
                      ,"@inttimefrom" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["inttimefrom"])
                      ,"@inttimeto" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["inttimeto"])
                      );
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadataAppointmentList(string dataList ,string RowNumBegin ,string limit ,string currentid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataDetail = JsonConvert.DeserializeObject<DataTable>(dataList);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Appointment_LoadByDayv2]" ,CommandType.StoredProcedure
                      ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataDetail.Rows[0]["dateFrom"].ToString())
                      ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataDetail.Rows[0]["dateTo"].ToString())
                      ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["branchID"])
                      ,"@areaBranch" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                      ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                      ,"@type" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["type"])
                      ,"@status" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["status"])
                      ,"@doctor" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["doctor"])
                      ,"@inttimefrom" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["inttimefrom"])
                      ,"@inttimeto" ,SqlDbType.Int ,Convert.ToInt32(DataDetail.Rows[0]["inttimeto"])
                      ,"@RowNumBegin" ,SqlDbType.Int ,Convert.ToInt32(RowNumBegin)
                      ,"@limit" ,SqlDbType.Int ,Convert.ToInt32(limit)
                      ,"@currentid" ,SqlDbType.Int ,Convert.ToInt32(currentid));
                    return Content(Comon.DataJson.Datatable(dt));
                    //using (var jsonConvert = new DataToJson(ref dt))
                    //{
                    //    return Content(jsonConvert.DataTableToJSON());
                    //}
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
