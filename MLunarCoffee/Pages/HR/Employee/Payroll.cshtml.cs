using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.HR.Employee
{
    public class PayrollModel : PageModel
    {
        public string _dataBranch { get; set; }
        public string _dataEmployeeGroup { get; set; }
        public string layout { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }

        public async Task<IActionResult> OnPostLoadComboMain()
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
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                          ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                          ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "dataBranch";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Salary_Type_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "dataAllowanceType";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Work_Scheduler_Employee_Group_LoadList]" ,CommandType.StoredProcedure);
                        dt.TableName = "dataEmployeeGroup";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Revenue_Payroll]" ,CommandType.StoredProcedure);
                        dt.TableName = "dataRevenuePayroll";
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

        public async Task<IActionResult> OnPostLoadDataPayroll(string dateFrom ,string dateTo ,int branch ,int group)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Payroll_Load]" ,CommandType.StoredProcedure ,
                      "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                      ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                      ,"@branch" ,SqlDbType.Int ,branch
                      ,"@group" ,SqlDbType.Int ,group
                    );
                    //if (ds != null)
                    //{
                    //    DataTable dt = ds.Tables[0];
                    //    DataTable dtPunish = ds.Tables[1];
                    //    {
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //        {
                    //            string resultPunish = DectectPunishDetail(dtPunish, Convert.ToInt32(dt.Rows[i]["ID"]));
                    //            dt.Rows[i]["PunishList"] = resultPunish;
                    //        }
                    //    }

                    //    return Content(Comon.DataJson.Datatable(dt));
                    //}
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        private static string DectectPunishDetail(DataTable dt ,int EmpID)
        {
            try
            {
                string result = "";
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<int>("EmpID") == EmpID
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    result = result + "<p class='whitespace'>" + dtResult.Rows[i]["PunishName"].ToString() + ": " + dtResult.Rows[i]["PunishAmount"].ToString() + " |</p>";
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
