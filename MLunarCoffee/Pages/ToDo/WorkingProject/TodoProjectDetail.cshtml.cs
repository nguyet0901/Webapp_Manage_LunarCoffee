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

namespace SourceMain.Pages.ToDo.WorkingProject
{
    public class TodoProjectDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
        }



        public async Task<IActionResult> OnPostLoadInitializeData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Team_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "dataTeam";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Todo_TaskStatus_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "dataStatus";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Project_LoadDetail]", CommandType.StoredProcedure,
                     "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                    dt.TableName = "dataDetail";
                    ds.Tables.Add(dt);
                }

                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                TodoProject DataMain = JsonConvert.DeserializeObject<TodoProject>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("YYY_sp_ToDo_Project_Insert", CommandType.StoredProcedure,
                        "@Name", SqlDbType.NVarChar, DataMain.name,
                        "@Note", SqlDbType.NVarChar, DataMain.description,
                         "@Date_from", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.datefrom),
                         "@Date_to", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.dateto),
                         "@Team_ID", SqlDbType.Int, DataMain.team_id,
                         "@Status_ID", SqlDbType.Int, DataMain.status_id,
                         "@Created_By", SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("YYY_sp_ToDo_Project_Update", CommandType.StoredProcedure,
                         "@Name", SqlDbType.NVarChar, DataMain.name,
                        "@Note", SqlDbType.NVarChar, DataMain.description,
                         "@Date_from", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.datefrom),
                         "@Date_to", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.dateto),
                         "@Team_ID", SqlDbType.Int, DataMain.team_id,
                         "@Status_ID", SqlDbType.Int, DataMain.status_id,
                          "@currentID", SqlDbType.Int, CurrentID,
                           "@Modified_By", SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                return Content( dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
               return Content("Error");
            }


        }
    }
    //public class TodoProject
    //{
    //    public string Name { get; set; }
    //    public string Note { get; set; }
    //    public string DateFrom { get; set; }
    //    public string DateTo { get; set; }
    //    public string Team_ID { get; set; }
    //    public string Status_ID { get; set; }
    //}
}
