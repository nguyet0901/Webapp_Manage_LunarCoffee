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

namespace SourceMain.Pages.ToDo.WorkingProject
{
    public class TodoTeamDetailModel : PageModel
    {
        public string _TodoTeamID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TodoTeamID = curr.ToString();
            }
            else
            {
                _TodoTeamID = "0";
            }
        }



        public async Task<IActionResult> OnPostLoadInitializeData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                    dt.TableName = "cbbUser";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await confunc.ExecuteDataTable("[YYY_sp_ToDo_Team_LoadDetail]", CommandType.StoredProcedure,
                     "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
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
                ToDoTeam DataMain = JsonConvert.DeserializeObject<ToDoTeam>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Team_Insert]", CommandType.StoredProcedure,
                            "@UserToken", SqlDbType.NVarChar, DataMain.TokenUser,
                              "@TeamName", SqlDbType.NVarChar, DataMain.TeamName,
                              "@Created_By", SqlDbType.Int, user.sys_userid
                            );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("Tên Nhóm Đã Tồn Tại, Vui Lòng Thử Lại");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_ToDo_Team_Update]", CommandType.StoredProcedure,
                            "@UserToken", SqlDbType.NVarChar, DataMain.TokenUser,
                            "@TeamName", SqlDbType.NVarChar, DataMain.TeamName,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("Tên Nhóm Đã Tồn Tại, Vui Lòng Thử Lại");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
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
    public class ToDoTeam
    {
        public string TokenUser { get; set; }
        public string TeamName { get; set; }
    }
}
