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

namespace MLunarCoffee.Pages.Setting.Validation
{
    public class RequireValidationDetailModel : PageModel
    {
        public string _RvaliDID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _RvaliDID = curr.ToString();
            }
            else
            {
                _RvaliDID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_RequireValidation_LoadDetail]", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string DataDetail)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                MSType DataMain = JsonConvert.DeserializeObject<MSType>(DataDetail);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_RequireValidation_Insert]", CommandType.StoredProcedure,
                            "@FieldName", SqlDbType.NVarChar, DataMain.FieldName,
                            "@ControlName", SqlDbType.NVarChar, DataMain.ControlName,
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Content", SqlDbType.NVarChar, DataMain.Content,
                            "@PagePath", SqlDbType.NVarChar, DataMain.PagePath

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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_RequireValidation_Update]", CommandType.StoredProcedure,
                            "@FieldName", SqlDbType.NVarChar, DataMain.FieldName,
                            "@ControlName", SqlDbType.NVarChar, DataMain.ControlName,
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Content", SqlDbType.NVarChar, DataMain.Content,
                            "@PagePath", SqlDbType.NVarChar, DataMain.PagePath,
                            "@currentID ", SqlDbType.Int, CurrentID

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
        public class MSType
        {
            public string FieldName { get; set; }
            public string ControlName { get; set; }
            public string Content { get; set; }
            public int Type { get; set; }
            public string PagePath { get; set; }
        }
    }
}
