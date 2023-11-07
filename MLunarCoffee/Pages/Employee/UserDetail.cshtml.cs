using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Employee
{
    public class UserDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _CurrentGroupID { get; set; }
        public string sys_PassworDefault { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string curr = Request.Query["CurrentID"];
            string group = Request.Query["GroupID"];
            sys_PassworDefault = Comon.Global.sys_PassworDefault;
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
            if (group != null)
            {
                _CurrentGroupID = group.ToString();
            }
            else
            {
                _CurrentGroupID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_User_Detail]" ,CommandType.StoredProcedure ,
                            "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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
        public async Task<IActionResult> OnPostLoadComboMain(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);

                //Group
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_User_LoadCombo_GroupUser" ,CommandType.StoredProcedure);
                    dt.TableName = "GroupUser";
                    ds.Tables.Add(dt);
                }

                //Branch
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,0);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }

                //LoadEmployee
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_User_LoadCombo_Employee" ,CommandType.StoredProcedure
                         ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                            );
                    dt.TableName = "Employee";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                UserDetail DataMain = JsonConvert.DeserializeObject<UserDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        var dt = await connFunc.ExecuteDataTable("[YYY_sp_User_Insert]" ,CommandType.StoredProcedure ,
                         "@Employee_ID" ,SqlDbType.Int ,DataMain.Emp_ID ,
                         "@Group_ID" ,SqlDbType.Int ,DataMain.Group ,
                         "@Branch_ID" ,SqlDbType.Int ,DataMain.Branch_ID ,
                         "@AreaBranch" ,SqlDbType.NVarChar ,DataMain.AreaBranch ,
                         "@Username" ,SqlDbType.NVarChar ,DataMain.UserName ,
                         "@Password" ,SqlDbType.NVarChar ,Comon.Global.sys_PassworDefault ,
                         "@isPassIP" ,SqlDbType.Int ,DataMain.isPassIP ,
                         "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                         "@isAllBranch" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isAllBranch) ,
                         "@isDiscountMax" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isDiscountMax) ,
                         "@AmountMaxDiscount" ,SqlDbType.Decimal ,DataMain.AmountMaxDiscount ,
                         "@PerMaxDiscount" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PerMaxDiscount) ,
                         "@isLockUser" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isLockUser) ,
                         "@MaxDayLock" ,SqlDbType.Int ,Convert.ToInt32(DataMain.MaxDayLock) ,
                         "@TimeResetPassword" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TimeResetPassword) ,
                         "@sys_limit_user" ,SqlDbType.Int ,Comon.Global.sys_limit_User);

                        if (dt != null)
                        {
                            return Content(DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        var dt = await connFunc.ExecuteDataTable("YYY_sp_User_Update" ,CommandType.StoredProcedure ,
                        "@Group_ID" ,SqlDbType.Int ,DataMain.Group ,
                        "@Branch_ID" ,SqlDbType.Int ,DataMain.Branch_ID ,
                        "@AreaBranch" ,SqlDbType.NVarChar ,DataMain.AreaBranch ,
                        "@isPassIP" ,SqlDbType.Int ,DataMain.isPassIP ,
                        "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@Modified" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@isAllBranch" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isAllBranch) ,
                        "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                        "@Username" ,SqlDbType.NVarChar ,DataMain.UserName ,
                        "@isDiscountMax" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isDiscountMax) ,
                        "@AmountMaxDiscount" ,SqlDbType.Decimal ,DataMain.AmountMaxDiscount ,
                        "@PerMaxDiscount" ,SqlDbType.Int ,Convert.ToInt32(DataMain.PerMaxDiscount) ,
                        "@isLockUser" ,SqlDbType.Int ,Convert.ToInt32(DataMain.isLockUser) ,
                        "@MaxDayLock" ,SqlDbType.Int ,Convert.ToInt32(DataMain.MaxDayLock) ,
                        "@TimeResetPassword" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TimeResetPassword));

                        if (dt != null)
                        {
                            return Content(DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class UserDetail
    {
        public int Branch_ID { get; set; }
        public string AreaBranch { get; set; }
        public int Emp_ID { get; set; }
        public int isPassIP { get; set; }
        public int State { get; set; }
        public int Group { get; set; }
        public string UserName { get; set; }
        public int isAllBranch { get; set; }
        public int isDiscountMax { get; set; }
        public int PerMaxDiscount { get; set; }
        public double AmountMaxDiscount { get; set; }
        public int isLockUser { get; set; }
        public int MaxDayLock { get; set; }
        public int TimeResetPassword { get; set; }
    }
}
