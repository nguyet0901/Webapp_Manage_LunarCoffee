using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Marketing
{
    public class SMSSentListModel : PageModel
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

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_SMS_Option_Combo_Load", CommandType.StoredProcedure);
                    dt.TableName = "TypeSMS";
                    DataRow dr = dt.NewRow();
                    DataRow otherrow = dt.NewRow();
                    otherrow[0] = -1;
                    otherrow[1] = "<span>Khác</span>";
                    otherrow[2] = "Other";
                    dt.Rows.InsertAt(otherrow, 0);
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_SmsTemp_LoadCombo" ,CommandType.StoredProcedure);
                    dt.TableName = "CodeSms";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadTotal(string data, string SearText, int IsPhone)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                SMSSent DataMain = JsonConvert.DeserializeObject<SMSSent>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_Sent_LoadTotal]" ,CommandType.StoredProcedure
                       ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.dateFrom != null ? DataMain.dateFrom : "01-01-1900")
                       ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.dateTo != null ? DataMain.dateTo : "01-01-1900")
                       ,"@BranchID" ,SqlDbType.Int ,DataMain.branchid
                       ,"@branchToken" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                       ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       ,"@Type" ,SqlDbType.NVarChar ,DataMain.type != null ? DataMain.type : ""
                       ,"@typeSuccess" ,SqlDbType.Int ,Convert.ToInt32(DataMain.typeSuccess)
                       ,"@Search" ,SqlDbType.NVarChar ,SearText != null ? SearText : ""
                       ,"@IsPhone", SqlDbType.Int,IsPhone
                       ,"@isZNS" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isZNS)
                       ,"@issystem" ,SqlDbType.Int ,Convert.ToInt32(DataMain.issystem)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string data,  int Limit, string BeginID, string SearchText, int IsPhone)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                SMSSent DataMain = JsonConvert.DeserializeObject<SMSSent>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_Sent_LoadList]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.dateFrom != null ? DataMain.dateFrom : "01-01-1900")
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.dateTo != null ? DataMain.dateTo : "01-01-1900")
                       , "@BranchID", SqlDbType.Int,DataMain.branchid
                       , "@branchToken", SqlDbType.NVarChar,user.sys_AreaBranch
                       , "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                       , "@Type", SqlDbType.NVarChar,DataMain.type != null ? DataMain.type : ""
                       , "@Limit", SqlDbType.Int, Limit
                       , "@typeSuccess", SqlDbType.Int, Convert.ToInt32(DataMain.typeSuccess)
                       , "@BeginID", SqlDbType.BigInt, Convert.ToInt64(BeginID)
                       , "@Search", SqlDbType.NVarChar, SearchText != null ? SearchText : ""
                       , "@IsPhone", SqlDbType.Int,IsPhone
                       , "@isZNS", SqlDbType.Int, Convert.ToInt32(DataMain.isZNS)
                       , "@issystem" , SqlDbType.Int, Convert.ToInt32(DataMain.issystem)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class SMSSent
        {
            public int branchid { get; set; }
            public string type { get; set; }
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
            public int typeSuccess { get; set; }
            public int issystem { get; set; }
            public int isZNS { get; set; }
        }
    }
}
