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
using SourceMain.Models.ToDoModel;
using SourceMain.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace SourceMain.Pages.ToDo.WorkingProject
{
    public class TodoTaskDetailModel : PageModel
    {

        public string _taskDetailID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public TodoTaskDetailModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _taskDetailID = curr.ToString();
            }
            else
            {
                _taskDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(int projectID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await confunc.ExecuteDataTable("[YYY_sp_ToDo_Staff_LoadCombo]", CommandType.StoredProcedure,
                    "@projectID", SqlDbType.Int, Convert.ToInt32(projectID));
                dt.TableName = "dtStaff";
                ds.Tables.Add(dt);
            }

            return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
        }



        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                TodoTask DataMain = JsonConvert.DeserializeObject<TodoTask>(data);

                TodoTask[] task = new TodoTask[1];

                TodoTask t = new TodoTask();
                t.id = DataMain.id;
                t.name = DataMain.name;
                t.description = DataMain.description;
                t.staffid = DataMain.staffid;
                t.created = DateTime.Now;
                t.state = 1;
                t.checklists = new TodoCheckList[] { };
                task[0] = t;

                return Content(JsonConvert.SerializeObject(task));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostLoadCommentDetail(string projectid, string taskid)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Project_Load_Detail_Comment]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid
                        , "@projectid", SqlDbType.NVarChar, projectid
                        , "@taskid", SqlDbType.NVarChar, taskid
                    );
                }
                return Content(JsonConvert.SerializeObject(dt));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostInsertCommentDetail (string projectid, string taskid, string empwork, string title, string content)
        {
            try
            {

                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Project_Insert_Comment]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid
                        , "@EmpID", SqlDbType.Int, user.sys_employeeid
                        , "@projectid", SqlDbType.NVarChar, projectid
                        , "@taskid", SqlDbType.NVarChar, taskid
                        , "@Content", SqlDbType.NVarChar, content
                    );
                }
                // Người duoc chon chinh la nguoi dang thao tac,
                if (empwork == user.sys_employeeid.ToString())
                {
                }
                // Người thêm là người giao
                else
                {
                    await NotiLocal.Noti_Parti_Emp(hubContext,Convert.ToInt32(empwork), title, content);
                }

                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
