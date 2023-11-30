using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.ToDo.ToDoRemind
{
    public class ToDoChangeStatusModel : PageModel
    {
        public int _TodoID { get; set; }
        public int _TicketID { get; set; }
        public int _CustomerID { get; set; }
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


        public async Task<IActionResult> OnPostExcuteChangeStatus(string CurrentID, string StatusID, string IsClose)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                  await  connFunc.ExecuteDataTable("[YYY_sp_Todo_ByUser_ChangeStatus]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                        "@StatusID", SqlDbType.Int, Convert.ToInt32(StatusID),
                        "@IsClose", SqlDbType.Int, Convert.ToInt32(IsClose),
                        "@Modified_By", SqlDbType.Int, user.sys_userid
                    );
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }


        public async Task<IActionResult> OnPostDeleteItem(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Dash_Todo_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
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
