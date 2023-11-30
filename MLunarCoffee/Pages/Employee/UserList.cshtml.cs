using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Employee
{
    public class UserListModel : PageModel
    {
        public string _limitUser { get; set; }
        public string layout { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public UserListModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            _limitUser = Global.sys_limit_User.ToString();
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }
        public async Task<IActionResult> OnPostLoadataUserType()
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_User_Group_LoadList_All" ,CommandType.StoredProcedure);
                dt.TableName = "GroupList";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_User_LoadTopicToken" ,CommandType.StoredProcedure);
                dt.TableName = "Topic";
                ds.Tables.Add(dt);
            }

            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
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
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
                await NotiLocal.Noti_And_LogOut(hubContext ,id.ToString() ,"Logout" ,"Your account has been deleted");

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostResetPassword(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_ResetPassword]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                await NotiLocal.Noti_And_LogOut(hubContext ,id.ToString() ,"Logout" ,"Your account has been reset password");
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


        public async Task<IActionResult> OnPostUnLockItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_UnLock]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
        public async Task<IActionResult> OnPostLockItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_Lock]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Datenow" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                await NotiLocal.Noti_And_LogOut(hubContext ,id.ToString() ,"Logout" ,"Your account has been locked");
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

        public async Task<IActionResult> OnPostLoaddataUser(int groupID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_User_LoadList]" ,CommandType.StoredProcedure ,
                                "@GroupID" ,SqlDbType.Int ,groupID
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
    }
}
