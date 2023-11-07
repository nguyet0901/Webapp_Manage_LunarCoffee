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

namespace MLunarCoffee.Pages.Discount
{
    public class DiscountListModel : PageModel
    {
        public int _branchID { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if(_layout!=null)
            {
                layout=_layout;
            }
            else layout="";
            var user = Session.GetUserSession(HttpContext);
            _branchID=user.sys_branchID;
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("YYY_sp_Branch_Load",CommandType.StoredProcedure
                        ,"@UserID",SqlDbType.Int,user.sys_userid
                        ,"@LoadNormal",SqlDbType.Int,1);
                    dt.TableName="Branch";
                    ds.Tables.Add(dt);
                }
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("YYY_sp_ServiceType_Load",CommandType.StoredProcedure);
                    dt.TableName="ServiceType";
                    ds.Tables.Add(dt);
                }
                if(ds!=null)
                    return Content(Comon.DataJson.Dataset(ds));
                else return Content("0");
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadDataDiscount(string date,string branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                string dateFrom = "";
                string dateTo = "";
                if(date.Contains(" to "))
                {
                    date=date.Replace(" to ","@");
                    dateFrom=date.Split('@')[0];
                    dateTo=date.Split('@')[1];
                }
                else
                {
                    dateFrom=date;
                    dateTo=date;
                }
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_sp_Discount_LoadList",CommandType.StoredProcedure,
                    "@UserID",SqlDbType.Int,user.sys_userid,
                    "@branchID",SqlDbType.Int,branchID,
                    "@datefrom",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                    "@dateto",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if(dt!=null)

                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                    return Content("0");

            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        private static string DectectDetail(DataTable dt,string Rule)
        {
            try
            {
                string result = "";
                string[] rule = Rule.Split('-');
                foreach(var r in rule)
                {
                    if(r!="")
                    {
                        int id = Convert.ToInt32(r);
                        var resultRow = from myRow in dt.AsEnumerable()
                                        where myRow.Field<int>("ID")==id
                                        select myRow;
                        DataTable dtResult = resultRow.CopyToDataTable();
                        for(int i = 0;i<dtResult.Rows.Count;i++)
                        {
                            result=(result=="") ? result+dtResult.Rows[i]["Detail"].ToString() : result+dtResult.Rows[i]["Detail"].ToString();
                        }
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                return "";
            }

        }

        private static string DectectBranchToken(DataTable dt,string Token)
        {
            try
            {
                string result = "";
                string[] token = Token.Split(',');
                foreach(var r in token)
                {
                    if(r!="")
                    {
                        int id = Convert.ToInt32(r);
                        var resultRow = from myRow in dt.AsEnumerable()
                                        where myRow.Field<int>("ID")==id
                                        select myRow;
                        DataTable dtResult = resultRow.CopyToDataTable();
                        for(int i = 0;i<dtResult.Rows.Count;i++)
                        {
                            result=(result=="") ? result+dtResult.Rows[i]["Name"].ToString() : result+dtResult.Rows[i]["Name"].ToString();
                        }
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                return "";
            }

        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Discount_Delete]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,id,
                        "@Datenow",SqlDbType.DateTime,Comon.Comon.GetDateTimeNow(),
                        "@userID",SqlDbType.Int,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }

    }
}
