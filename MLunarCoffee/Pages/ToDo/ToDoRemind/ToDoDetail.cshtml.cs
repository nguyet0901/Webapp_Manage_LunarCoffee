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
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MLunarCoffee.Pages.ToDo.ToDoRemind
{
    public class ToDoDetailModel : PageModel
    {
        public int _TodoID { get; set; }
        public int _TicketID { get; set; }
        public int _CustomerID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public ToDoDetailModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string todoid = Request.Query["CurrentID"];
            string custid = Request.Query["CustomerID"];
            string ticketid = Request.Query["TicketID"];
            _TodoID = Convert.ToInt32((todoid != null ? todoid : "0"));
            _TicketID = Convert.ToInt32((ticketid != null ? ticketid : "0"));
            _CustomerID = Convert.ToInt32((custid != null ? custid : "0"));
        }


        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await confunc.ExecuteDataTable("YYY_sp_Todo_TaskStatus_LoadCombo", CommandType.StoredProcedure);
                    dt.TableName = "Status";
                    ds.Tables.Add(dt);

                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_ToDo_Quick_Template_LoadCombo", CommandType.StoredProcedure);
                    dt.TableName = "QuickTemplate";
                    ds.Tables.Add(dt);

                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_User_LoadCombo", CommandType.StoredProcedure);
                    dt.TableName = "User";
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


        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Dash_Todo_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID
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

        public async Task<IActionResult> OnPostExcuteData (string CurrentID, string Data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                TodoDetail DataMain = JsonConvert.DeserializeObject<TodoDetail>(Data);
                var CurID = CurrentID;
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                       dt=await connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_Update]", CommandType.StoredProcedure,
                            "@TicketID", SqlDbType.Int, DataMain.TicketID,
                            "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                            "@StatusID", SqlDbType.Int, DataMain.StatusID,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@UserTo", SqlDbType.Int, DataMain.UserTo,
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                   
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt=await connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_Insert]", CommandType.StoredProcedure,
                            "@TicketID", SqlDbType.Int, DataMain.TicketID,
                            "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                            "@StatusID", SqlDbType.Int, DataMain.StatusID,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@UserTo", SqlDbType.Int, DataMain.UserTo,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                        );
                    }
                     
                }
 
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public class TodoDetail
        {
            public int StatusID { get; set; }
            public int CustomerID { get; set; }
            public int TicketID { get; set; }
            public int UserTo { get; set; }
            public string Content { get; set; }
        }
    }
}
