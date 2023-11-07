using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using SourceMain.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace SourceMain.Pages.ToDo.Personal
{
    public class TodoDetailModel : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public TodoDetailModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds =await confunc.ExecuteDataSet("[YYY_sp_Todo_LoadDetail_Info]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        );

                }
                if (ds != null) return Content(JsonConvert.SerializeObject(ds));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadData_Message(int CurrentID, int limit, int begin, int mesid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_LoadDetail_Message]", CommandType.StoredProcedure
                        , "@TodoID", SqlDbType.Int, CurrentID
                        , "@idbegin", SqlDbType.Int, begin
                        , "@limit", SqlDbType.Int, limit
                         , "@mesid", SqlDbType.Int, mesid
                        );

                }
                if (dt != null) return Content(JsonConvert.SerializeObject(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostReaction_Todo(int CurrentID, string userstring)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_Reaction_Info]", CommandType.StoredProcedure
                        , "@TodoID", SqlDbType.Int, CurrentID
                        , "@userstring", SqlDbType.Int, userstring
                        );

                }
                if (dt != null) return Content(JsonConvert.SerializeObject(dt));
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostReaction_Message(int messid, string userstring)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_Reaction_Message]", CommandType.StoredProcedure
                        , "@MessageID", SqlDbType.Int, messid
                        , "@userstring", SqlDbType.Int, userstring
                        );

                }
                if (dt != null) return Content(JsonConvert.SerializeObject(dt));
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostSending_Mess (int TodoID, string content, int currentid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_InsertMessage]", CommandType.StoredProcedure
                        , "@TodoID", SqlDbType.Int, TodoID
                         , "@currentid", SqlDbType.Int, currentid
                         , "@CreatedBy", SqlDbType.Int, user.sys_userid
                         , "@content", SqlDbType.Int, content
                        );

                }
                if (dt != null)
                {
                    int type = (currentid == 0) ? 0 : 1;
                    int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
                    if (result != 0)
                    {
                        int userto = Convert.ToInt32(dt.Rows[0]["USER_TO"]);
                        int userfrom = Convert.ToInt32(dt.Rows[0]["USER_FROM"]);
                        if (userto != 0 && userto != user.sys_userid) await NotiLocal.Noti_Todo_User(hubContext,userto, TodoID, result, type, "");
                        if (userfrom != 0 && userfrom != user.sys_userid) await NotiLocal.Noti_Todo_User(hubContext,userfrom, TodoID, result, type, "");

                    }


                   return Content(result.ToString());
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostUpdate_Read_Todo(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_Update_Read]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                         , "@UserID", SqlDbType.Int, user.sys_userid
                        );

                }
                if (dt != null) return Content(dt.Rows[0][0].ToString());
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostDelete_Mess (int messid, int todoID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_DeleteMessage]", CommandType.StoredProcedure
                         , "@TodoID", SqlDbType.Int, todoID
                         , "@messid", SqlDbType.Int, messid
                         , "@CreatedBy", SqlDbType.Int, user.sys_userid
                        );

                }
                if (dt != null)
                {
                    int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
                    if (result == 1)
                    {
                        int userto = Convert.ToInt32(dt.Rows[0]["USER_TO"]);
                        int userfrom = Convert.ToInt32(dt.Rows[0]["USER_FROM"]);
                        if (userto != 0 && userto != user.sys_userid) await NotiLocal.Noti_Todo_User(hubContext,userto, todoID, messid, 2, "");
                        if (userfrom != 0 && userfrom != user.sys_userid) await NotiLocal.Noti_Todo_User(hubContext,userfrom, todoID, messid, 2, "");
                    }
                   return Content(result.ToString());
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
