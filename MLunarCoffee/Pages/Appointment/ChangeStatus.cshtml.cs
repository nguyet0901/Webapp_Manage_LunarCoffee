using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using System.Linq;
namespace MLunarCoffee.Pages.Appointment
{
    public class ChangeStatusModel : PageModel
    {
        public string _DataComboNewStatus { get; set; }
        public string _DataComboLevel { get; set; }
        public string _DataComboRoom { get; set; }
        public string _AppDetailID { get; set; }
        public int _BranchID { get; set; }
        public int sys_PKCheckingrom { get; set; }

        private readonly IHubContext<NotiHub> hubContext;
        public ChangeStatusModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            string curr = Request.Query["CurrentID"];
            sys_PKCheckingrom = Comon.Global.sys_PKCheckingrom;
            if (curr != null)
            {
                _AppDetailID = curr.ToString();
            }
            else
            {
                _AppDetailID = "0";
            }

        }

        public async Task<IActionResult> OnPostInitialize(int app_id)
        {
            try
            {
                var tasks = new List<Task<DataTable>>();
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_LoadNextStatus" ,CommandType.StoredProcedure
                            ,"@AppID" ,SqlDbType.Int ,app_id);
                        dt.TableName = "StatusNext";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Appointment_LoadStatusDetail]" ,CommandType.StoredProcedure
                            ,"@Current" ,SqlDbType.Int ,Convert.ToInt32(app_id == 0 ? 0 : app_id));
                        dt.TableName = "Detail";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadEmployee(string datefrom ,string dateto ,int groupID ,int branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Employee_ByWorkSchedule]" ,CommandType.StoredProcedure
                          ,"@GroupID" ,SqlDbType.Int ,groupID
                          ,"@Branch_ID" ,SqlDbType.Int ,branchID
                          ,"@isAllBranch" ,SqlDbType.Int ,0
                          ,"@DateFrom" ,SqlDbType.DateTime ,datefrom
                          ,"@DateTo" ,SqlDbType.DateTime ,dateto);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        //public async Task<IActionResult> OnPostGetRoom()
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[YYY_sp_RoomStatus_ListRoom]" ,CommandType.StoredProcedure
        //                ,"@Branchid" ,SqlDbType.Int ,user.sys_branchID);
        //        }
        //        return Content(Comon.DataJson.Dataset(ds));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}
        public async Task<IActionResult> OnPostInitializeData_Room()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_RoomStatus_Change_Room]" ,CommandType.StoredProcedure
                        ,"@Branchid" ,SqlDbType.Int ,user.sys_branchID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadWorkTimeDoctor(string tokenDocID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_WorK_Schedule_Doctor_By_Date]" ,CommandType.StoredProcedure ,
                          "@Date" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow()
                          ,"@TokenDocID" ,SqlDbType.NVarChar ,tokenDocID
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
