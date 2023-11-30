using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Settings.Certificate
{
    public class CertificateDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string idDetail = Request.Query["CurrentID"];
            if (idDetail != null)
            {
                _CurrentID = idDetail;
            }
            else
            {
                _CurrentID = "0";
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string Name, string Avatar, string Note, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Certificate_Insert", CommandType.StoredProcedure,
                          "@Name", SqlDbType.NVarChar, Name,
                          "@Avatar", SqlDbType.NVarChar, Avatar,
                          "@Note", SqlDbType.NVarChar, Note,
                          "@UserID", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Certificate_Update", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                         "@Name", SqlDbType.NVarChar, Name,
                         "@Avatar", SqlDbType.NVarChar, Avatar,
                         "@Note", SqlDbType.NVarChar, Note,
                         "@UserID", SqlDbType.Int, user.sys_userid
                       );
                    }
                }

                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string CurrentId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Certificate_LoadDetail", CommandType.StoredProcedure,
                            "@CurrentId", SqlDbType.Int, Convert.ToInt32(CurrentId)
                            );
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
