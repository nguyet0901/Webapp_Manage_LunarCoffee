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

namespace MLunarCoffee.Pages.Card.Status
{
    public class CardList : PageModel
    {
        public int _branchID { get; set; }
        public string layout { get; set; }
        public string _EditCustomer_PassingDate { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
            string _layout = Request.Query["layout"];
            layout = (_layout != null) ? _layout : "";

            _EditCustomer_PassingDate = user.sys_EditCustomerPassingDate.ToString();
        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadDataTotal( string DateFrom, string DateTo, int SearchDay, int BranchID, string TextSearch, string IsSearchAll)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Customer_CardUsedTotal", CommandType.StoredProcedure
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                        , "@SearchDay", SqlDbType.Int, SearchDay
                        , "@BranchID", SqlDbType.Int, BranchID
                        , "@TextSearch", SqlDbType.NVarChar, (TextSearch != null) ? TextSearch.ToString() : ""
                        , "@IsSearchAll", SqlDbType.Int, Convert.ToInt32(IsSearchAll)
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadData(string limit, string beginID,string DateFrom,string DateTo, int SearchDay, int BranchID, string TextSearch, string IsSearchAll, string CurrenID)
        {
            try
            {

                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_Customer_CardUsed_V3", CommandType.StoredProcedure
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                        , "@SearchDay", SqlDbType.Int, SearchDay
                        , "@BranchID", SqlDbType.Int, BranchID
                        , "@TextSearch", SqlDbType.NVarChar, (TextSearch != null) ? TextSearch.ToString() : ""
                        , "@IsSearchAll", SqlDbType.Int, Convert.ToInt32(IsSearchAll)
                        , "@limit", SqlDbType.Int, Convert.ToInt32(limit)
                        , "@beginID", SqlDbType.Int, Convert.ToInt32(beginID)
                        , "@CurrenID",SqlDbType.Int, Convert.ToInt32(CurrenID)
                        );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLockAndUnlockCard(int id, int type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_ServiceCard_ChangeLock]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type", SqlDbType.Int, type,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
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
