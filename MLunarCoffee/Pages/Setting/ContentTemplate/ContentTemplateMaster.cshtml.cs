using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ContentTemplate
{
    public class ContentTemplateMasterModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _TypeStatusID { get; set; }
        public void OnGet()
        {
            string ser = Request.Query["TypeStatusID"];
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
            if (ser != null)
            {
                _TypeStatusID = ser.ToString();
            }
            else
            {
                _TypeStatusID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ContentTemplateMaster_LoadDetail", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, CurrentID);
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

        public async Task<IActionResult> OnPostExcuteDataMaster(string data, string CurrentID)
        {
            try
            {
                TemplateMaster DataMain = JsonConvert.DeserializeObject<TemplateMaster>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Content_Master_Insert]", CommandType.StoredProcedure,
                          "@Type", SqlDbType.VarChar, DataMain.Type.Replace("'", "").Trim(),
                          "@MasterName", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Description", SqlDbType.NVarChar, DataMain.Description.Replace("'", "").Trim(),
                          "@Created_By", SqlDbType.Int, user.sys_userid,
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Content_Master_Update]", CommandType.StoredProcedure,
                          "@MasterID", SqlDbType.Int, CurrentID,
                          "@Type", SqlDbType.VarChar, DataMain.Type.Replace("'", "").Trim(),
                          "@MasterName", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Description", SqlDbType.NVarChar, DataMain.Description.Replace("'", "").Trim(),
                          "@Modified_By", SqlDbType.Int, user.sys_userid,
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

        public class TemplateMaster
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
