using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class StatusOptionModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _StatusID { get; set; }
        public string _AppID { get; set; }
        public void OnGet()
        {
            string custid = Request.Query["CustomerID"];
            string statusid = Request.Query["StatusID"];
            string appid = Request.Query["AppID"];
            _CustomerID = Convert.ToString(custid) != "" ? Convert.ToString(custid) : "0";
            _StatusID = Convert.ToString(statusid) != "" ? Convert.ToString(statusid) : "0";
            _AppID = Convert.ToString(appid) != "" ? Convert.ToString(appid) : "0";
        }
        public async Task<IActionResult> OnPostIni(int branchid)
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCashBackReason = new DataTable();
                        dtCashBackReason = await connFunc.ExecuteDataTable("[MLU_sp_Employee_LoadCombo_Group]" ,CommandType.StoredProcedure);
                        dtCashBackReason.TableName = "EmpGroup";
                        return dtCashBackReason;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("MLU_sp_LoadCombo_EmployeeFull" ,CommandType.StoredProcedure
                             ,"@Branch_ID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_branchID)
                             ,"@isAllBranch" ,SqlDbType.Int ,0);
                        dt.TableName = "Emp";
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
        public async Task<IActionResult> OnPostLoadData(int statusid ,int custID ,int appID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Schedule_StatusOption_LoadInfo]" ,CommandType.StoredProcedure
                        ,"@StatusID" ,SqlDbType.Int ,statusid
                        ,"@AppID" ,SqlDbType.Int ,appID
                        ,"@CustID" ,SqlDbType.Int ,custID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("[]");
            }
        }
    }
}
