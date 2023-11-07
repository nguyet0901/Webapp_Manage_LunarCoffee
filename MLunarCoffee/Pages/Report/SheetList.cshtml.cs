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
    public class SheetList : PageModel
    {
        public string _ReportSheetCurrentID { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ReportSheetCurrentID = curr;
            }
            else
            {
                _ReportSheetCurrentID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadata()
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_ReportSheet_LoadList]", CommandType.StoredProcedure);
            }
            if (ds != null)
            {

                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Sheet DataMain = JsonConvert.DeserializeObject<Sheet>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_ReportSheet_Update]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.NVarChar, DataMain.Name.Trim(),
                              "@Tag", SqlDbType.VarChar, DataMain.Tag.Trim(),
                              "@Type", SqlDbType.VarChar, DataMain.Type.Trim(),
                              "@Des", SqlDbType.VarChar, DataMain.Des.Trim(),
                               "@DesCode", SqlDbType.VarChar, DataMain.DesCode.Trim(),
                              "@ModifiedBy", SqlDbType.Int, user.sys_userid,
                              "@CurrentID", SqlDbType.Int, CurrentID
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Sheet Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                    return Content("Chọn Sheet Cần Chỉnh Sửa");
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
    public class Sheet
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        public string Des { get; set; }
        public string DesCode { get; set; }
    }
}
 