using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Myinfo
{
    public class UserLogModel : PageModel
    {
        public string _UserID { get; set; }
        public void OnGet()
        {
            string UserID = Request.Query["UserID"];
            _UserID = UserID != null ? UserID.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_UserLog_LoadType", CommandType.StoredProcedure);
                    dt.TableName = "Type";
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
                    dt = await confunc.ExecuteDataTable("[MLU_sp_User_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "User";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDataMaster(string data)
        {
            try
            {

                DataTable dt = new DataTable();
                UserLogList DataMain = JsonConvert.DeserializeObject<UserLogList>(data);
                var user = Session.GetUserSession(HttpContext);
                string minify = Session.GetSession(HttpContext.Session, "Minify");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_User_Log_LoadList]" , CommandType.StoredProcedure
                      , "@TextSearch", SqlDbType.NVarChar, DataMain.TextSearch.ToLower().Replace("'", "").Trim()
                      , "@UserID", SqlDbType.Int,DataMain.IsMine == 1 ? user.sys_userid : DataMain.UserID
                      , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom)
                      , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo)
                      , "@BranchID", SqlDbType.Int, DataMain.BranchID
                      , "@BranchToken", SqlDbType.NVarChar, user.sys_AreaBranch
                      , "@IsAllBranch", SqlDbType.Int, user.sys_AllBranchID
                      , "@Limit", SqlDbType.Int, DataMain.Limit
                      , "@BeginTime", SqlDbType.BigInt, DataMain.BeginTime
                      , "@IsMinify", SqlDbType.Int, minify
                      , "@TypeAction", SqlDbType.NVarChar, DataMain.TypeAction
                      , "@Type", SqlDbType.NVarChar, DataMain.Type.ToLower().Replace("'", "").Trim());
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
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDataDetail(string data, string currentID)
        {
            try
            {
                DataTable dt = new DataTable();
                UserLogDetail DataMain = JsonConvert.DeserializeObject<UserLogDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                string minify = Session.GetSession(HttpContext.Session, "Minify");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_User_Log_Loaddetail_V2]", CommandType.StoredProcedure
                      , "@CustID", SqlDbType.Int, DataMain.CustID
                      , "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim()
                      , "@IsMine", SqlDbType.Int, DataMain.IsMine
                      , "@UserID", SqlDbType.Int, DataMain.UserID
                      , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom)
                      , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo)
                      , "@Type", SqlDbType.NVarChar, DataMain.Type.ToLower().Replace("'", "").Trim()
                      , "@CurrentID", SqlDbType.BigInt, currentID);
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
                return Content("0");
            }
        }
    }


    public class UserLogList
    {
        public string TextSearch { get; set; }
        public int IsMine { get; set; }
        public int UserID { get; set; }
        public int BranchID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Type { get; set; }
        public int Limit { get; set; }
        public long BeginTime { get; set; }
        public string TypeAction { get; set; }
    }

    public class UserLogDetail
    {
        public int CustID { get; set; }
        public string Value { get; set; }
        public int IsMine { get; set; }
        public int UserID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int branchID { get; set; }
        public string Type { get; set; }
    }

}
