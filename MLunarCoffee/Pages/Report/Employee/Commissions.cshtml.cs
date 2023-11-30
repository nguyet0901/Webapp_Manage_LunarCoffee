using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Employee
{
    public class CommissionsModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

        // public async Task<IActionResult> OnPostLoadEmployee(int branchID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.LoadEmployeeFull(branchID, 1);

        //        }
        //        return Content(JsonConvert.SerializeObject(dt, Formatting.Indented));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("");
        //    }
        //}

         public async Task<IActionResult> OnPostLoadata(string date, int maxdate, int employeeid)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }

                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > maxdate)
                {
                    return Content("0");
                }
                else
                {

                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_v2_ReportVer1_Revenue_Individual]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@dencos", SqlDbType.Int, Comon.Global.sys_DentistCosmetic,
                            "@empID", SqlDbType.Int, employeeid);
                    }
                    return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

         public async Task<IActionResult> OnPostLoadata_Prepare(int branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[sp_v2_ReportVer1_Revenue_Prepare]", CommandType.StoredProcedure
                        , "@Type", SqlDbType.Int, 0);

                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Employee_LoadCombo_Group]", CommandType.StoredProcedure);
                    dt.TableName = "dtEmployeeGroup";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);

                    DataRow dr = dt.NewRow();
                    dr[0] = 0; dr[1] = "All";
                    dt.Rows.InsertAt(dr, 0);
                    dt.TableName = "dtBranch";
                    ds.Tables.Add(dt);
                }
                return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
         public async Task<IActionResult> OnPostLoadata_Employee(int groupID, int branchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = dt =await  confunc.LoadEmployee(groupID, branchID, user.sys_AllBranchID);
                    dt.TableName = "dtEmployee";
                }
                return Content(JsonConvert.SerializeObject(dt, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
