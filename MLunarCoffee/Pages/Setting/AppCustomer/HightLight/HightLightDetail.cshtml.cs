using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.AppCustomer.HightLight
{
    public class HightLightDetailModel : PageModel
    {
        public string _HightLightDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _HightLightDetailID = curr != null ? curr.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_AC_HightLight_LoadDetail]", CommandType.StoredProcedure,
                    "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID));
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
                HightLight DataMain = JsonConvert.DeserializeObject<HightLight>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_AC_HightLight_Update]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@Url", SqlDbType.NVarChar, DataMain.Url.Replace("'", "").Trim(),
                              "@Image", SqlDbType.NVarChar, DataMain.Image,
                              "@Position", SqlDbType.NVarChar, DataMain.Position.Replace("'", "").Trim(),
                              "@IsClose", SqlDbType.Int, DataMain.IsClose,
                              "@Description", SqlDbType.NVarChar, DataMain.Description.Replace("'", "").Trim(),
                              "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom),
                              "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo),
                              "@UserID", SqlDbType.Int, user.sys_userid,
                              "@currentID ", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tiêu Đề Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class HightLight
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string Position { get; set; }
            public int IsClose { get; set; }
            public string Description { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }

        }
    }
}
