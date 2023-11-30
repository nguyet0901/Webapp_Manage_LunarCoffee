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
using Microsoft.AspNetCore.Http.Extensions;
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;
namespace MLunarCoffee.Pages.Customer
{

    public class ScheduleList_ScheduleModel : PageModel
    {
        public string _SchedulerCustomerID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public int _SchedulerSeft { get; set; }
        public ScheduleList_ScheduleModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                _SchedulerCustomerID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
            _SchedulerSeft = Comon.Global.sys_UsingSchedulerSeft;
        }


        public async Task<IActionResult> OnPostLoadata(string CustomerID, int Limit, string BeginID,string IsDelete)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Schedule_LoadList]", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@BranchID", SqlDbType.Int, Convert.ToInt32(user.sys_branchID)
                      , "@Limit", SqlDbType.Int, Limit
                      , "@BeginID", SqlDbType.BigInt, Convert.ToInt64(BeginID)
                      , "@IsDelete" , SqlDbType.BigInt, Convert.ToInt64(IsDelete)
                      );
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

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Schedule_Delete]", CommandType.StoredProcedure,
                    "@CurrentID", SqlDbType.Int, id,
                    "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                    "@userID", SqlDbType.Int, user.sys_userid,
                    "@BranchID", SqlDbType.Int, user.sys_branchID
                );
                DateTime DateFrom_Old = Convert.ToDateTime(dt.Rows[0]["OldDate"].ToString()).Date;
                DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                string oldbranchid = dt.Rows[0]["Branch"].ToString();
                string doctor = dt.Rows[0]["Doctor"].ToString();
                if (doctor != "0")
                {
                    await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext, 0, Convert.ToInt32(oldbranchid), Convert.ToInt32(id), DateTime.Now, "delete", 0);
                }
                if (DateFrom_Old == DateNow)
                {


                    string CustName = dt.Rows[0]["CustName"].ToString();
                    string CustID = dt.Rows[0]["CustID"].ToString();
                    string CustCode = dt.Rows[0]["CustCode"].ToString();
                    string Room = dt.Rows[0]["Room"].ToString();

                    await NotiLocal.Noti_Parti_Appointment_Branch(hubContext, Convert.ToInt32(oldbranchid), Convert.ToInt32(id)
                        , Convert.ToInt32(CustID), CustCode, CustName
                        , 0, Convert.ToInt32(doctor), "delete");
                    if (Convert.ToInt32(Room) != 0)
                    {
                        //Comon.PushNoti.Noti_Parti_Appointment_Room(Convert.ToInt32(oldbranchid), Convert.ToInt32(id)
                        //, Convert.ToInt32(CustID), CustCode, CustName
                        //, Convert.ToInt32(Room), "cancelbook");
                    }
                }
            }
            return Content(Comon.DataJson.GetValueRowProperty(dt, 0, "AppCode"));
        }

        public async Task<IActionResult> OnPostLoadSheduleInDay(int CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Schedule_Load_ScheduleInDay]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CustomerID
                    );
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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
    }
}
