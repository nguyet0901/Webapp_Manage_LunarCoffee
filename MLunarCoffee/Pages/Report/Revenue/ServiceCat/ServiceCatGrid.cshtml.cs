using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Revenue.ServiceCat
{
    public class ServiceCatGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_ServiceCatv2]", CommandType.StoredProcedure
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                        , "@branchID", SqlDbType.Int, Convert.ToInt32(branchID)
                        , "@Type", SqlDbType.Int, 0 //Type = 0: Load All (Service & Card & Medicine & Deposit)
                        , "@Revenue", SqlDbType.Int, 2 //Revenue = 2: Load all (Target & Revenue)
                        , "@ServiceCatID", SqlDbType.Int, 0 //Service = 0: Load all service
                        );
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataDetail(int branchID, string dateFrom, string dateTo, string TypeID, string ServiceTypeID, string IsRevenue)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_ServiceCatv2]", CommandType.StoredProcedure
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                        , "@branchID", SqlDbType.Int, Convert.ToInt32(branchID)
                        , "@Type", SqlDbType.Int, Convert.ToInt32(TypeID)
                        , "@Revenue", SqlDbType.Int, Convert.ToInt32(IsRevenue)
                        , "@ServiceCatID", SqlDbType.Int, Convert.ToInt32(ServiceTypeID)
                        );
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
