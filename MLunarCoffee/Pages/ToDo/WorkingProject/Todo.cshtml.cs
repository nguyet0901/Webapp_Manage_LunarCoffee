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
using MLunarCoffee.Models.ToDoModel;

namespace MLunarCoffee.Pages.ToDo.WorkingProject
{
    public class TodoModel : PageModel
    {
        public string _dtTodoStatus { get; set; }
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
        public async Task<IActionResult> OnPostLoadDataProject(string date)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (date.Contains(" to "))
                {
                    date = date.Replace(" to ", "@");
                    dateFrom = date.Split('@')[0];
                    dateTo = date.Split('@')[1];
                }
                else
                {
                    dateFrom = date;
                    dateTo = date;
                }
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[YYY_sp_ToDo_Project_LoadList]", CommandType.StoredProcedure,
                        "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                    );
                }

                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception EX)
            {
                throw;
            }

        }


        public async Task<IActionResult> OnPostLoadDataInitialize()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Todo_TaskStatus_LoadCombo]", CommandType.StoredProcedure);
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
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


         public async Task<IActionResult> OnPostDeleteProject(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataTable("[YYY_sp_ToDo_Project_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostUpdateProjectStatus(string statusid, string CurrentID)
        {
            try
            {
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Project_UpdateStatus]", CommandType.StoredProcedure,
                              "@currentID", SqlDbType.Int, CurrentID,
                              "@Status_ID", SqlDbType.Int, statusid
                          );
                    }
                    return Content("1");
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
    }
}
