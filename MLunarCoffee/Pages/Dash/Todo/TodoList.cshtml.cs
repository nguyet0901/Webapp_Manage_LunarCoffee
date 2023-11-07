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
namespace MLunarCoffee.Pages.Dash.Todo
{
    //[ResponseCache(Duration = 86400)]
    public class TodoListModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadata(string CurrentID, string Createdbegin, string limit)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                { 
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Dash_Todo_LoadList_ByUser]", CommandType.StoredProcedure
                        , "@Createdbegin", SqlDbType.BigInt, Createdbegin
                        , "@limit", SqlDbType.DateTime, Convert.ToInt32(limit)
                        , "@CurrentID", SqlDbType.DateTime, Convert.ToInt32(CurrentID)
                        , "@User_ID", SqlDbType.DateTime, Convert.ToInt32(user.sys_userid));

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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string data, string TicketID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                TodoDetail DataMain = JsonConvert.DeserializeObject<TodoDetail>(data);

                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_Update]", CommandType.StoredProcedure,
                            "@TicketID", SqlDbType.Int, TicketID,
                            "@TemplateID", SqlDbType.Int, DataMain.TemplateID,
                            "@EmployeeID", SqlDbType.Int, DataMain.EmployeeID,
                            "@Created", SqlDbType.DateTime, DataMain.DateCreated,
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_Insert]", CommandType.StoredProcedure,
                            "@TicketID", SqlDbType.Int, TicketID,
                            "@TemplateID", SqlDbType.Int, DataMain.TemplateID,
                            "@EmployeeID", SqlDbType.Int, DataMain.EmployeeID,
                            "@Created", SqlDbType.DateTime, DataMain.DateCreated,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim()
                        );
                    }
                }


                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostExcuteChangeTemplate(string CurrentID, string TemplateID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_ChangeTemplate]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@TemplateID", SqlDbType.Int, TemplateID,
                        "@Modified_By", SqlDbType.Int, user.sys_userid
                    );
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostDeleteItem(int CurrentID)
        {
            try
            { 
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Dash_Todo_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Dash_Todo_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
    public class TodoDetail
    {
        public int TemplateID { get; set; }
        public int EmployeeID { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
