using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.AppCustomer.Service
{
    public class ServiceDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _CurrentID = (curr != null) ? curr.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_AC_Service_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                AppServiceDetail DataMain = JsonConvert.DeserializeObject<AppServiceDetail>(data);
                DataTable dt = new DataTable();

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                         dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_Service_Insert]", CommandType.StoredProcedure,
                            "@Header", SqlDbType.NVarChar, DataMain.Header.Replace("'", "").Trim(),
                            "@Sub", SqlDbType.NVarChar, DataMain.Sub.Replace("'", "").Trim(),
                            "@Price", SqlDbType.NVarChar, DataMain.Price.Replace("'", "").Trim(),
                            "@Image", SqlDbType.NVarChar, DataMain.Image,
                            "@FeatureImage", SqlDbType.NVarChar, DataMain.FeatureImage,
                            "@Source", SqlDbType.NVarChar, DataMain.Source,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@CreatedBy", SqlDbType.NVarChar, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_Service_Update]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Header", SqlDbType.NVarChar, DataMain.Header.Replace("'", "").Trim(),
                            "@Sub", SqlDbType.NVarChar, DataMain.Sub.Replace("'", "").Trim(),
                            "@Price", SqlDbType.NVarChar, DataMain.Price.Replace("'", "").Trim(),
                            "@Image", SqlDbType.NVarChar, DataMain.Image,
                            "@FeatureImage", SqlDbType.NVarChar, DataMain.FeatureImage,
                            "@Source", SqlDbType.NVarChar, DataMain.Source,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@ModifiedBy", SqlDbType.Int, user.sys_userid);
                    }
                }
                if(dt != null)
                {
                    return Content("1");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }


    public class AppServiceDetail
    {
        public string Header { get; set; }
        public string Sub { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public string FeatureImage { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
    }
}
