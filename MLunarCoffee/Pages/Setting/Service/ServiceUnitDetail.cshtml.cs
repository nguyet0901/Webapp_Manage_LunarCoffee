using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
namespace MLunarCoffee.Pages.Setting.Service
{
    public class ServiceUnitDetailModel : PageModel
    {
        public string sys_DentistCosmetic { get; set; }
        public string _CurrentUnitID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            if (curr != null && curr != "0")
            {
                _CurrentUnitID = curr.ToString();
            }
            else
            {
                _CurrentUnitID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_ServiceUnit_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                ServiceUnit DataMain = JsonConvert.DeserializeObject<ServiceUnit>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_ServiceUnit_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@Type", SqlDbType.NVarChar, DataMain.Type.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_ServiceUnit_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@Type", SqlDbType.NVarChar, DataMain.Type.Replace("'", "").Trim(),
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Modified", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
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
}
public class ServiceUnit
{
    public string Name { get; set; }
    public string Note { get; set; }
    public string Type { get; set; }
}