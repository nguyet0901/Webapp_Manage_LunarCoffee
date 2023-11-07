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


namespace MLunarCoffee.Pages.HR.Employee.Punish
{
    public class PunishEmployeeModel : PageModel
    {
        public int sys_AllBranchID { get; set; }
        public string sys_AreaBranch { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            sys_AllBranchID = user.sys_AllBranchID;
            sys_AreaBranch = user.sys_AreaBranch;
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
                DataTable dt = new DataTable();
                //LoadBranch
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadataPunishEmployee(string date)
        {
            try
            {
                DataTable dt = new DataTable();
                string dateFrom;
                string dateTo;
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
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_HR_Employee_Punish_LoadList]", CommandType.StoredProcedure,
                     "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                     , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }
        }
    }
}
