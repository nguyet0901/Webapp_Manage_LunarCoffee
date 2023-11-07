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

namespace MLunarCoffee.Pages.Report
{
    public class TagDetail : PageModel
    {
        public string _ReportTagCurrentID { get; set; }
        public string _dataReportTag { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ReportTagCurrentID = curr.ToString();
            }
            else
            {
                _ReportTagCurrentID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_ReportTag_Detail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Tag DataMain = JsonConvert.DeserializeObject<Tag>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_ReportTag_Insert]", CommandType.StoredProcedure,
                              "@TagName", SqlDbType.NVarChar, DataMain.TagName.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@TagShort", SqlDbType.VarChar, DataMain.TagShort
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Tag Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                        DataTable dt = await connFunc.ExecuteDataTable("YYY_sp_ReportTag_Update", CommandType.StoredProcedure,
                            "@TagName", SqlDbType.NVarChar, DataMain.TagName,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                             "@currentID ", SqlDbType.Int, CurrentID,
                              "@TagShort ", SqlDbType.VarChar, DataMain.TagShort
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Tag Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
        public class Tag
        {
            public string TagName { get; set; }
            public string TagShort { get; set; }
        }
    }
}
