using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Teacher.TeacherDetail
{
    public class TeacherDetailModel : PageModel
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
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ST_Teacher_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id)
                      );
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string Avatar)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                Teacher DataMain = JsonConvert.DeserializeObject<Teacher>(data);
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Teacher_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Avatar", SqlDbType.NVarChar, Avatar,
                            "@Alias", SqlDbType.NVarChar, DataMain.Alias.Replace("'", "").Trim(),
                            "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                            "@Address", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }

                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_Teacher_Update]", CommandType.StoredProcedure,
                           "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                           "@Avatar", SqlDbType.NVarChar, Avatar,
                           "@Alias", SqlDbType.NVarChar, DataMain.Alias.Replace("'", "").Trim(),
                           "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                           "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                           "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                           "@Address", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                           "@Modified_By", SqlDbType.Int, user.sys_userid,
                           "@currentID ", SqlDbType.Int, CurrentID
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
    public class Teacher
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
