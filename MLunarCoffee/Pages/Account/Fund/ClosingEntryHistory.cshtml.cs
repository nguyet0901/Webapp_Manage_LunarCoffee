using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Account.Fund
{
    public class ClosingEntryHistory : PageModel
    {
        public string _IsShowDetail { get; set; }
        public int _branchID { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            string isShowDetail = Request.Query["isdetail"];
            if (isShowDetail != "" && isShowDetail != null)
                _IsShowDetail = isShowDetail;
            else
                _IsShowDetail = "0";
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                }

                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadHistory(string date, int branchid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Closing_Entry_History_LoadList]", CommandType.StoredProcedure,
                       "@date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMM(date.ToString())
                      , "@branchID", SqlDbType.Int, branchid
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string date, int branchid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_v2_Account_Funds_Load]", CommandType.StoredProcedure,
                       "@date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(date.ToString())
                      , "@branchID", SqlDbType.Int, branchid);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadTotal(string date, int branchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_v2_Account_Funds_LoadTotal]", CommandType.StoredProcedure,
                       "@date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(date.ToString())
                      , "@branchID", SqlDbType.Int, branchid);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch (Exception e)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Closing_Entry_History_Delete", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        );
                    return Content(Comon.DataJson.GetValueRowProperty(dt, 0, "RESULT"));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
