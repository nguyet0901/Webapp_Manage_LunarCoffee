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
using SourceMain.Models.Model.AlrmScheduler;

namespace SourceMain.Pages.ToDo.WorkingProject
{
    public class ProjectOptionModel : PageModel
    {
        public string _projectID { get; set; }
        //public string _Todo_Project_Detail { get; set; }
        //public string _Todo_Alram_Detail { get; set; }
        //public string _Todo_Staffs { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _projectID = curr.ToString();
            }
            else
            {
                _projectID = "0";
            }

        }

        public async Task<IActionResult> OnPostLoadDataInitialize(string id)
        {
            try
            {
                DataSet dsResult = new DataSet();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Todo_Project_LoadUpdate]", CommandType.StoredProcedure,
                      "@ProjectID", SqlDbType.Int, Convert.ToInt32(id));
                    dt = ds.Tables[1];
                    dt.TableName = "Alram";
                    dsResult.Tables.Add(dt.Copy());
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ToDo_Staff_LoadCombo]", CommandType.StoredProcedure,
                        "@projectID", SqlDbType.Int, Convert.ToInt32(id));
                    dt.TableName = "Staffs";
                    dsResult.Tables.Add(dt);
                }

                return Content(JsonConvert.SerializeObject(dsResult));

            }
            catch(Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadProjectDetail(string id)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Todo_Project_LoadUpdate]", CommandType.StoredProcedure,
                  "@ProjectID", SqlDbType.Int, Convert.ToInt32(id));
            }
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                TodoList[] ProjectDetail;
                if (dt.Rows[0][0].ToString() == "") // chua set lan nao
                {
                    ProjectDetail = GetArrayProject_Detail_Empty();

                    return Content( JsonConvert.SerializeObject(ProjectDetail));
                }
                else // da set
                {
                    return Content(dt.Rows[0][0].ToString());
                }
            }
            else
            {
                return Content("");
            }



        }

        public async Task<IActionResult> OnPostLoadAlramDetail(string id)
        { // load ds staff

            try
            {
                DataSet ds = new DataSet();
                DataSet dspost = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Todo_Project_LoadUpdate]", CommandType.StoredProcedure,
                      "@ProjectID", SqlDbType.Int, Convert.ToInt32(id));
                }
                if (ds != null)
                {
                    DataTable dtFromAlram = ds.Tables[1];
                   return Content( JsonConvert.SerializeObject(dtFromAlram));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadStaffs(string id)
        { // load ds staff

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ToDo_Staff_LoadCombo]", CommandType.StoredProcedure,
                        "@projectID", SqlDbType.Int, Convert.ToInt32(id));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string idTask, string idList, string isDelete, string projectID)
        {
            try
            {
                TodoList[] DataMain = JsonConvert.DeserializeObject<TodoList[]>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("YYY_sp_ToDo_Project_UpdateTask", CommandType.StoredProcedure,
                        "@ProjectID", SqlDbType.Int, projectID,
                        "@Created_By", SqlDbType.Int, user.sys_userid,
                        "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(DataMain)
                    );
                    if (dt != null && dt.Rows.Count == 1 && dt.Rows[0]["RESULFT"].ToString() == "1" && idTask != "-1" && idList != "-1")
                    {
                        if (isDelete == "1")// Xoa
                        {
                            dt = await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Project_Delete]", CommandType.StoredProcedure,
                                                "@TaskID", SqlDbType.NVarChar, idTask,
                                                "@CreatedBy", SqlDbType.Int, user.sys_userid,
                                                  "@ProjectID", SqlDbType.Int, projectID
                                              );
                        }
                        else
                        {
                            for (int i = 0; i < DataMain.Length - 1; i++)
                            {
                                if (DataMain[i].id == Convert.ToInt64(idList))
                                {
                                    string descriptionPro = DataMain[i].description;
                                    TodoTask task = null;
                                    if (idTask == "0") // them moi
                                    {
                                        task = DataMain[i].tasks[DataMain[i].tasks.Length - 1];

                                    }
                                    else // update
                                    {
                                        for (int j = 0; j < DataMain[i].tasks.Length; j++)
                                        {
                                            if (DataMain[i].tasks[j].id == Convert.ToInt64(idTask))
                                            {
                                                task = DataMain[i].tasks[j];
                                            }
                                        }
                                    }
                                    string title = task.name;
                                    string content = task.description;
                                    string emp = task.staffid.ToString();
                                    TodoCheckList[] cls = task.checklists;

                                    AlrmScheduler_Item[] als = new AlrmScheduler_Item[] { new AlrmScheduler_Item() {
                                        dayofweek = 0,
                                        type = 1,
                                        hour = Comon.Comon.GetDateTimeNow().ToString("HH:mm"),
                                        date =  Comon.Comon.GetDateTimeNow().ToString("yyyy-MM-dd"),
                                        day = 0
                                    }
                                };
                                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Project_Insert_Update]", CommandType.StoredProcedure,
                                                 "@Employee", SqlDbType.Int, emp,
                                                 "@ProjectID", SqlDbType.Int, projectID,
                                                 "@TaskID", SqlDbType.NVarChar, task.id,
                                                 "@CreatedBy", SqlDbType.Int, user.sys_userid,
                                                 "@Title", SqlDbType.NVarChar, title,
                                                 "@Content", SqlDbType.NVarChar, content,
                                                 "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(als),
                                                  "@DataCheckList", SqlDbType.NVarChar, JsonConvert.SerializeObject(cls)
                                               );
                                }
                            }
                        }
                    }
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public static TodoList[] GetArrayProject_Detail_Empty()
        {
            try
            {
                TodoList[] ls = new TodoList[1];
                for (int k = 0; k < 1; k++)
                {
                    TodoList l = new TodoList();
                    l.id = 1;
                    l.name = "Work Group 1";
                    l.description = "Content ";
                    l.tasks = new TodoTask[] { };
                    l.state = 1;
                    ls[k] = l;
                }
                return ls;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
