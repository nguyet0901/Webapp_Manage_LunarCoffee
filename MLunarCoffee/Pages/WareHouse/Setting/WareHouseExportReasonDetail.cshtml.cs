using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class WareHouseExportReasonDetailModel : PageModel
    {
        public string _ExportReasonID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _ExportReasonID = curr.ToString();
            else _ExportReasonID = "0";
        }

        public async Task<IActionResult> OnPostLoadUpdate(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await confunc.ExecuteDataTable("[MLU_sp_WareHouse_PrExportReason_LoadDetails]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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

        public async Task<IActionResult> OnPostExcuteData(string Name, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_PrExportReason_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_PrExportReason_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID);
                    }
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