using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.ContentTemplate
{
    public class TreatmentContentTemplateStatusDetailModel : PageModel
    {
        public string _TypeStatusID { get; set; }
        public string _CurrentID { get; set; }
        public string _TempGroupID { get; set; }
        public void OnGet()
        {
            string ser = Request.Query["TypeStatusID"];
            string curr = Request.Query["CurrentID"];
            string groupid = Request.Query["TempGroupID"];
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
            if (groupid != null)
            {
                _TempGroupID = groupid.ToString();
            }
            else
            {
                _TempGroupID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(string TypeStatusID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Content_Template_Load]", CommandType.StoredProcedure
                         , "@MasterID", SqlDbType.Int, 0);

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

        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Content_Detail_Load", CommandType.StoredProcedure,
                      "@DetailID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
        public async Task<IActionResult> OnPostExcuteDataDetail(string data, string CurrentID, string TypeStatusID)
        {
            try
            {
                TreatmentDetail DataMain = JsonConvert.DeserializeObject<TreatmentDetail>(data);
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
    public class TreatmentDetail
    {
        public int MasterID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
