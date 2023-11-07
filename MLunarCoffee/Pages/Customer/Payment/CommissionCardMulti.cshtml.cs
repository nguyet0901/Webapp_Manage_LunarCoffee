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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.Payment
{

    public class CommissionCardMultiModel : PageModel
    {
        public string _CustCommissionCard { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            var cus = Request.Query["CustomerID"];
            if (cus.ToString() != String.Empty)  _CustCommissionCard = cus.ToString();
            else _CustCommissionCard = "0";
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                    dt.TableName = "Employee";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadDataCommissionCard(int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_CardCommission_LoadComBo_Card_v2]", CommandType.StoredProcedure,
                        "@CustomerID", SqlDbType.Int, CustomerID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string customerID, string note, int CardID, string created, int branchID)
        {
            try
            {
                DataTable dtdata = JsonConvert.DeserializeObject<DataTable>(data);

                DataTable DataMain = new DataTable();
                DataMain.Columns.Add("EmployeeID1", typeof(int));
                DataMain.Columns.Add("AmountConsult1", typeof(decimal));
                DataMain.Columns.Add("EmployeeID2", typeof(int));
                DataMain.Columns.Add("AmountConsult2", typeof(decimal));
                DataMain.Columns.Add("EmployeeID3", typeof(int));
                DataMain.Columns.Add("AmountConsult3", typeof(decimal));

                DataRow dr = DataMain.NewRow();
                int s = 1;
                for (int i = 0; i < dtdata.Rows.Count; i++)
                {
                    dr["EmployeeID" + s.ToString()] = dtdata.Rows[i]["Consult"];
                    dr["AmountConsult" + s.ToString()] = dtdata.Rows[i]["ConsultAmount"];
                    if (s == 3)
                    {
                        DataMain.Rows.Add(dr);
                        s = 1;
                        dr = DataMain.NewRow();
                    }
                    else if (i == (dtdata.Rows.Count - 1))
                    {
                        DataMain.Rows.Add(dr);
                    }
                    else
                    {
                        s++;
                    }
                }
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_CardCommission_Multi_Insert]", CommandType.StoredProcedure,
                        "@CardID", SqlDbType.Int, CardID,
                        "@CustomerID", SqlDbType.Int, customerID,
                        "@Note", SqlDbType.NVarChar, note != null ? note.Replace("'", "").Trim() : "",
                        "@IsChooseDate", SqlDbType.Int, user.sys_ChooseDateCustomer,
                        "@CreatedBy", SqlDbType.Int, user.sys_userid,
                        "@Created", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(created),
                        "@BranchID", SqlDbType.Int, branchID != 0 ? branchID : user.sys_branchID,
                        "@table_data", SqlDbType.Structured, DataMain.Rows.Count > 0 ? DataMain : null
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
