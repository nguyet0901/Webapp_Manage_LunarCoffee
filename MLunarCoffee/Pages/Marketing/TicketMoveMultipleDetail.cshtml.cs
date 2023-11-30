using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketMoveMultipleDetailModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadTeleMove]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "Tele";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_Tag]", CommandType.StoredProcedure);
                    dt.TableName = "Tag";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Move_LoadReason]", CommandType.StoredProcedure);
                    dt.TableName = "Reason";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExecute(string userTo, string reason, string isChangeTag, string TagID, string dtticket)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtMain = JsonConvert.DeserializeObject<DataTable>(dtticket);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_Move_Insert_v2]", CommandType.StoredProcedure,
                        "@table_data", SqlDbType.Structured, (dtMain.Rows.Count > 0 ? (DataTable)dtMain : null),
                        "@userTo", SqlDbType.NVarChar, Convert.ToInt32(userTo),
                        "@reason", SqlDbType.NVarChar, Convert.ToInt32(reason),
                        "@isChangeTag", SqlDbType.Int, Convert.ToInt32(isChangeTag),
                        "@tagID", SqlDbType.Int, Convert.ToInt32(TagID),
                        "@Created_By", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
