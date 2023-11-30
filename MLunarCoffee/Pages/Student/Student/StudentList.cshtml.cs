using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Student
{
    public class StudentListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null) layout = _layout;
            else layout = "";
        }

        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_Source_LoadCombo]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostLoadData(string textsearch, string BeginID, string CurrentID, string limit, string sourceId, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_LoadList]", CommandType.StoredProcedure
                        , "@textsearch", SqlDbType.NVarChar, textsearch != null ? textsearch : ""
                        , "@BeginID", SqlDbType.Int, Convert.ToInt32(BeginID)
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        , "@limit", SqlDbType.Int, Convert.ToInt32(limit)
                        , "@SourceID", SqlDbType.Int, Convert.ToInt32(sourceId)
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDelete(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_Delete]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.NVarChar, CurrentID
                        );
                }
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDisableItem(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Student_Disable]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt16(CurrentID),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
