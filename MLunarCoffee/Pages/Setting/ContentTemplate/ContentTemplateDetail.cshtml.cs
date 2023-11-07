using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ContentTemplate
{
    public class ContentTemplateDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _MasterID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _CurrentID = curr != null ? curr.ToString() : "0";
            string masterID = Request.Query["MasterID"];
            _MasterID = masterID != null ? masterID.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_ContentTemplateMaster_LoadCombo]", CommandType.StoredProcedure);

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

        public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Content_Detail_Load", CommandType.StoredProcedure,
                      "@DetailID", SqlDbType.Int, CurrentID);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch
            {
                return Content("");
            }


        }


        //XỬ LÍ THÊM SỬA DETAIL

        public async Task<IActionResult> OnPostExcuteDataDetail(string data, string CurrentID, string TypeStatusID)
        {
            try
            {
                Detail DataMain = JsonConvert.DeserializeObject<Detail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Content_Detail_Insert]", CommandType.StoredProcedure,
                          "@MasterID", SqlDbType.Int, DataMain.MasterID,
                          "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Content_Detail_Update]", CommandType.StoredProcedure,
                          "@DetailID", SqlDbType.Int, CurrentID,
                          "@MasterID", SqlDbType.Int, DataMain.MasterID,
                          "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
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
    }
    public class Detail
    {
        public int MasterID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
