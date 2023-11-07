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

namespace MLunarCoffee.Pages.HR.Setting.DataCombo
{
    public class HRComboTypeModel : PageModel
    {
        public string sys_HRTypeID { get; set; }
        public void OnGet()
        {
            string _type = Request.Query["TypeID"];
            sys_HRTypeID = (_type == null) ? "0" : _type.ToString();
        }

        public async Task<IActionResult> OnPostLoadataHRType(string TypeStatusID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("YYY_sp_Setting_HR_ComboType_LoadList", CommandType.StoredProcedure,
                  "@TypeID", SqlDbType.Int, TypeStatusID);
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteDataHRTypeParent(string StatusName, string CurrentID, string TypeStatusID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_HR_ComboType_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.NVarChar, StatusName.Replace("'", "").Trim(),
                          "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Created", SqlDbType.Int, DateTime.Now
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Trạng Thái Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_HR_ComboType_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, StatusName.Replace("'", "").Trim(),
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Modified", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("");
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

        public async Task<IActionResult> OnPostDeleteItem(string StatusID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Setting_HR_ComboType_Delete]", CommandType.StoredProcedure,
                        "@User_ID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, StatusID,
                        "@Modified", SqlDbType.Int, DateTime.Now
                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
