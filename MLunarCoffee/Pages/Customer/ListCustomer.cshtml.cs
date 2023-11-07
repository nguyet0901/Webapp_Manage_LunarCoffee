using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer
{
    public class ListCustomerModel : PageModel
    {
        public int _branchID { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _branchID = user.sys_branchID;
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
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_MemberLoad]" ,CommandType.StoredProcedure);
                        dt.TableName = "Membership";
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


        public async Task<IActionResult> OnPostLoadData(string date ,string branchID ,int maxdate, int SortBy)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to " ,"@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                var user = Session.GetUserSession(HttpContext);
                if (totalDate > maxdate)
                {
                    return Content("0");
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_LoadList_V2]" ,CommandType.StoredProcedure ,
                      "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                      ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                      ,"@branchID" ,SqlDbType.Int ,branchID
                      ,"@SortBy", SqlDbType.Int,SortBy
                      );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string date ,string branchID ,int maxdate ,int currentID)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to " ,"@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > maxdate)
                {
                    return Content("0");
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_LoadDetail_V2]" ,CommandType.StoredProcedure
                      ,"@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                      ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                      ,"@branchID" ,SqlDbType.Int ,branchID
                      ,"@CurrentID" ,SqlDbType.Int ,currentID
                      );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
