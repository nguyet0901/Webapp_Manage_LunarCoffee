using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Treatment.TreatmentWork
{
    public class TreatWorkDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string _CurrentID { get; set; }
        public string _Type { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            var type = Request.Query["Type"];
            if (type.ToString() != String.Empty)
            {
                _Type = type.ToString();
            }
            else
            {
                _Type = "";
            }
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _CurrentID = curr.ToString();
                }
                else
                {
                    _CurrentID = "0";
                }
            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Work_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadComboMain()
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
                        dt = await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                        dt.TableName = "Employee";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployee(17, user.sys_branchID, user.sys_AllBranchID);
                        dt.TableName = "Assist";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                        dt.TableName = "Employee";

                        DataTable dt1 = new DataTable();
                        dt1 = await confunc.LoadEmployee(17, user.sys_branchID, user.sys_AllBranchID);
                        dt1.TableName = "Assist";

                        DataTable dtAll = new DataTable();
                        dtAll = dt.Copy();
                        dtAll.Merge(dt1);

                        dtAll.TableName = "Tech";
                        return dtAll;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Treatment_Task_Load]", CommandType.StoredProcedure);
                        dt.TableName = "Task";
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
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Work_Insert]", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, DataMain.Rows[0]["CustomerID"],
                            "@EmployeeID", SqlDbType.Int, DataMain.Rows[0]["Employee"],
                            "@Content", SqlDbType.NVarChar, DataMain.Rows[0]["Content"].ToString().Replace("'", "").Trim(),
                            "@Task", SqlDbType.NVarChar, DataMain.Rows[0]["Task"].ToString().Replace("'", "").Trim(),
                            "@CreatedBy", SqlDbType.Int, user.sys_userid,
                            "@BranchID", SqlDbType.Int, user.sys_branchID,
                            "@ChooseDateCustomer", SqlDbType.Int, user.sys_ChooseDateCustomer.ToString(),
                            "@dateCreated", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[0]["dateCreated"].ToString())
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Work_Update]", CommandType.StoredProcedure,
                            "@EmployeeID", SqlDbType.Int, DataMain.Rows[0]["Employee"],
                            "@Content", SqlDbType.NVarChar, DataMain.Rows[0]["Content"].ToString().Replace("'", "").Trim(),
                            "@Task", SqlDbType.NVarChar, DataMain.Rows[0]["Task"].ToString().Replace("'", "").Trim(),
                            "@ModifiedBy", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID);
                    }
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
