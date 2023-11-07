using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.PatientRecord
{
    public class PatientRecordImageDetailModel : PageModel
    {
        public string _PatientRecordDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _PatientRecordDetailID = curr != null ? curr.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetTempleteByType]", CommandType.StoredProcedure,
                        "@TYPE", SqlDbType.NVarChar, "patient_form");
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

        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_PatientRecord_LoadDetail]", CommandType.StoredProcedure,
                    "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID));
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



        public async Task<IActionResult> OnPostExcuteData(string Image,string Name, string CurrentID, string FormID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_PatientRecord_UpdateImage]", CommandType.StoredProcedure,
                              "@Image", SqlDbType.NVarChar, Image.ToString(),
                              "@Name", SqlDbType.NVarChar, Name.ToString() ,
                              "@CurrentID" , SqlDbType.Int, Convert.ToInt32(CurrentID),
                              "@FormID", SqlDbType.Int, Convert.ToInt32(FormID),
                              "@UserID", SqlDbType.Int, user.sys_userid
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
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
