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
    public class TicketMoveDetailModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadTele]", CommandType.StoredProcedure,
                    "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "Tele";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadPara("MoveTicketType");
                    dt.TableName = "MoveType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadPara("MoveTicketExecuteType");
                    dt.TableName = "ExecuteType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_TicketImport_LoadToTransfer]", CommandType.StoredProcedure);
                    dt.TableName = "ImportFile";
                    ds.Tables.Add(dt);
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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string userTo, string userFrom, string transferData, string type, string importFile, string dateFrom, string dateTo)
        {

            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Move_Insert]", CommandType.StoredProcedure,
                         "@userTo", SqlDbType.NVarChar, Convert.ToInt32(userTo),
                         "@userFrom", SqlDbType.NVarChar, Convert.ToInt32(userFrom),
                         "@transferData", SqlDbType.NVarChar, Convert.ToInt32(transferData),
                         "@type", SqlDbType.NVarChar, Convert.ToInt32(type),
                         "@importFile", SqlDbType.NVarChar, Convert.ToInt32(importFile),
                         "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                         "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                         "@Created_By", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
                //return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
