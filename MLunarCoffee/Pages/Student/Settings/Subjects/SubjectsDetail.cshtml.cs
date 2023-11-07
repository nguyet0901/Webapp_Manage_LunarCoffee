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

namespace MLunarCoffee.Pages.Student.Settings.Subjects.SubjectsDetail
{
    public class SubjectsDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public string _CourseID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string cour = Request.Query["CourseID"];
            if (curr != null && cour != null)
            {
                _DataDetailID = curr.ToString();
                _CourseID = cour.ToString();
            }
            else
            {
                _DataDetailID = "0";
                _CourseID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Settings_ListCourse_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "Course";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Settings_Subjects_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id)
                      );
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                Subjects DataMain = JsonConvert.DeserializeObject<Subjects>(data);
                DataTable dt = new DataTable();
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Subjects_Insert]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                            , "@Color", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim()
                            , "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                            , "@Course", SqlDbType.Int, Convert.ToInt32(DataMain.Course)
                            , "@ItemPrice", SqlDbType.Decimal, DataMain.ItemPrice
                            , "@TimeTeach", SqlDbType.Int, Convert.ToInt32(DataMain.TimeTeach)
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                            );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Settings_Subjects_Update]", CommandType.StoredProcedure
                            , "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                            , "@Color", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim()
                            , "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                            , "@Course", SqlDbType.Int, Convert.ToInt32(DataMain.Course)
                            , "@CourseOld", SqlDbType.Int, Convert.ToInt32(DataMain.CourseOld)
                            , "@ItemPrice", SqlDbType.Decimal, DataMain.ItemPrice
                            , "@TimeTeach", SqlDbType.Int, Convert.ToInt32(DataMain.TimeTeach)
                            , "@Modified_By", SqlDbType.Int, user.sys_userid
                            , "@currentID ", SqlDbType.Int, CurrentID
                        );
                    }
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class Subjects
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public string Course { get; set; }
        public string CourseOld { get; set; }
        public decimal ItemPrice { get; set; }
        public string TimeTeach { get; set; }

    }
}
