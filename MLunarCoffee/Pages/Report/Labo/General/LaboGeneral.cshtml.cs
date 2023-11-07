using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Labo.General
{
    public class LaboGeneralModel : PageModel
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

        public async Task<IActionResult> OnPostLoadInitialize()
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
                        dt = await confunc.LoadEmployee(14 ,user.sys_branchID ,user.sys_AllBranchID);
                        dt.TableName = "Doctor";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_Combo_Supplier]" ,CommandType.StoredProcedure);
                        dt.TableName = "Supplier";
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
            catch (Exception e)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadData(string branchID ,string dateFrom ,string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_Labo_General]" ,CommandType.StoredProcedure
                       ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@branchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch);

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDataDetail(string branchID ,string dateFrom ,string dateTo ,string docID ,string suppID ,string sourceID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_Labo_General_Detail]" ,CommandType.StoredProcedure
                       ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@branchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                       ,"@docID" ,SqlDbType.Int ,Convert.ToInt32(docID)
                       ,"@suppID" ,SqlDbType.Int ,Convert.ToInt32(suppID)
                       ,"@sourceID" ,SqlDbType.Int ,Convert.ToInt32(sourceID)
                       //,"@userTo" ,SqlDbType.Int ,Convert.ToInt32(userTo)
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch);

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
