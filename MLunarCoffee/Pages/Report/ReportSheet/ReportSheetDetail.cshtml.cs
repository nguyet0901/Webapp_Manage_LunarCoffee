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

namespace MLunarCoffee.Pages.Report.ReportSheet
{
    public class ReportSheetDetailModel : PageModel
    {
        public string _ReportSheetCurrentID { get; set; }
        public string _dataReportSheet { get; set; }
        public string _dataComboTag { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ReportSheetCurrentID = curr.ToString();
            }
            else
            {
                _ReportSheetCurrentID = "0";
            }
        }

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[MLU_sp_ReportSheet_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (ds != null)
            {
               // DataTable dtSheet = ds.Tables[0];
                //DataTable dt = ds.Tables[1];
                return Content(JsonConvert.SerializeObject(ds));
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
                Sheet DataMain = JsonConvert.DeserializeObject<Sheet>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_ReportSheet_Update]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.NVarChar, DataMain.NameShet.Trim(),
                              "@Tag", SqlDbType.VarChar, DataMain.Tag.Trim(),
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
        public class Sheet
        {
            public string NameShet { get; set; }
            public string Tag { get; set; }
        }
    }
}
