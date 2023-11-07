using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Finance
{
    public class ExpenseAdsListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }

        public async Task<IActionResult> OnPostLoadDataCombo()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                //using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                //{
                //    dt =await connFunc.ExecuteDataTable("[YYY_sp_ServiceCare_LoadCombo]", CommandType.StoredProcedure);
                //    dt.TableName = "ServiceCare";
                //    ds.Tables.Add(dt);
                //}
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }

        }

         public async Task<IActionResult> OnPostLoadData(int branchId, string date)
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
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[YYY_sp_Marketing_ExpenseAds_LoadList]", CommandType.StoredProcedure,
                  "@UserID", SqlDbType.Int, user.sys_userid
                  , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                  , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                  , "@Branch_ID", SqlDbType.Int, Convert.ToInt32(branchId)
                  , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("");
            }
        }

         public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Marketing_ExpenseAds_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id
                        );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
