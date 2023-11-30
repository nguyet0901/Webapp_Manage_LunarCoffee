using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Settings.Status
{
    public class StatusRevisionDetailModel : PageModel
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
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_StatusRevision_LoadDetail]", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                StatusRevision DataMain = JsonConvert.DeserializeObject<StatusRevision>(data);
                DataTable dt = new DataTable();
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_StatusRevision_Insert]", CommandType.StoredProcedure,
                                "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                                "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                                "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                                "@Created_By", SqlDbType.Int, user.sys_userid
                            );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_StatusRevision_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
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
    public class StatusRevision
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

    }
}
