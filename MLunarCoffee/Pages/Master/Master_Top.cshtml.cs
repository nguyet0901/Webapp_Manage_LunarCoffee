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
using Microsoft.AspNetCore.Http.Extensions;
using System.Globalization;

namespace MLunarCoffee.Pages.Master
{
    public class Master_TopModel : PageModel
    {

        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath=HttpContext.Request.Path;

        }


        public async Task<IActionResult> OnPostCheckCustCode(string CustCode)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Customer_Check_CustCode]"
                       ,CommandType.StoredProcedure,"@CustCode",SqlDbType.NVarChar,CustCode);
                }
                if(dt!=null&dt.Rows.Count>0)
                    return Content(dt.Rows[0]["CustID"].ToString());
                return Content("0");
            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostPasswordCheck()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_System_Check_Reset_Password]"
                       ,CommandType.StoredProcedure,"@UserID",SqlDbType.Int,user.sys_userid);

                }
                return Content(Comon.DataJson.Datatable(dt));

            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostSearchQuick(string textsign,string textsearch, string isInt ,string IsSign)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Searching_List_Quick_By_Option]",CommandType.StoredProcedure
                        ,"@textsign" ,SqlDbType.NVarChar ,textsign
                        , "@textsearch", SqlDbType.NVarChar, textsearch
                        , "@BranchToken", SqlDbType.Int, user.sys_AreaBranch
                        , "@IsAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@IsCustNotViewByBranch", SqlDbType.Int, Global.sys_CustomerNotViewByBranch
                        , "@IsInt", SqlDbType.NVarChar, isInt
                         ,"@IsSign" ,SqlDbType.NVarChar ,IsSign
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        );
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        #region // Notification

        public async Task<IActionResult> OnPostNotiItemCount()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_System_GetNoti]"
                       ,CommandType.StoredProcedure,"@UserID",SqlDbType.Int,user.sys_userid
                       ,"@UserGroupID",SqlDbType.Int,user.sys_RoleID
                       ,"@BranchID",SqlDbType.Int,user.sys_branchID
                        ,"@GetDetail",SqlDbType.Int,0);

                }

                return Content(Comon.DataJson.Datatable(dt));

            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostNotiItemLoad()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_System_GetNoti]"
                       ,CommandType.StoredProcedure,"@UserID",SqlDbType.Int,user.sys_userid
                        ,"@UserGroupID",SqlDbType.Int,user.sys_RoleID
                       ,"@BranchID",SqlDbType.Int,user.sys_branchID
                        ,"@GetDetail",SqlDbType.Int,1);
                }
                if(dt!=null)
                    return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostNotiExecute(int IsReadAll,int NotiID,int todo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_System_UpdateNoti_Read]",CommandType.StoredProcedure
                        ,"@UserID",SqlDbType.Int,user.sys_userid
                        ,"@UserGroupID",SqlDbType.Int,user.sys_RoleID
                        ,"@BranchID",SqlDbType.Int,user.sys_branchID
                        ,"@IsReadAll",SqlDbType.Int,IsReadAll
                        ,"@CurrentID",SqlDbType.Int,NotiID
                        ,"@todo",SqlDbType.Int,todo);
                }
                if(dt!=null&dt.Rows.Count>0)
                    return Content("1");
                else return Content("0");
            }

            catch(Exception ex)
            {
                return Content("0");
            }

        }


        #endregion



    }
}
