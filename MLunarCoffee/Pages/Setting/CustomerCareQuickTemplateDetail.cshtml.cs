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

namespace MLunarCoffee.Pages.Setting
{
    public class CustomerCareQuickTemplateDetailModel : PageModel
    {
        public string _QuickTemplateDetailID { get; set; }
        public string sys_StatusTypeID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string _type = Request.Query["TypeID"];
            sys_StatusTypeID = (_type == null) ? "0" : _type.ToString();
            _QuickTemplateDetailID = (curr == null) ? "0" : curr.ToString();

        }

        public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_CustomerCare_QuickTemplate_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string TypeID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                CustCareQuickTemplate DataMain = JsonConvert.DeserializeObject<CustCareQuickTemplate>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustomerCare_QuickTemplate_Insert]", CommandType.StoredProcedure,
                            "@Feature", SqlDbType.NVarChar, DataMain.Feature.Replace("'", "").Trim(),
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@TypeID", SqlDbType.Int, TypeID,
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustomerCare_QuickTemplate_Update]", CommandType.StoredProcedure,
                            "@Feature", SqlDbType.NVarChar, DataMain.Feature.Replace("'", "").Trim(),
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
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
    public class CustCareQuickTemplate
    {
        public string Feature { get; set; }
        public string Content { get; set; }
    }
}