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
    public class TicketListRecoverModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                //    //DataRow dr3 = dt1.NewRow();
                //    //dr3[0] = 0; dr3[1] = "All";
                //    //dt1.Rows.InsertAt(dr3, 0);
                //    dt.TableName = "Source";
                //    ds.Tables.Add(dt);
                //}
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadGroupTele]", CommandType.StoredProcedure,
                         "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "GroupTele";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }



        public async Task<IActionResult> OnPostLoadTicketList(string dateFrom, string dateTo, int groupTeleID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TicketRecoverList_Load]", CommandType.StoredProcedure,
                  "@UserID", SqlDbType.Int, user.sys_userid
                  , "@GroupTeleID", SqlDbType.Int, groupTeleID
                  , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                  , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostRecover(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);  
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_ticket_Recover]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

    }
}
