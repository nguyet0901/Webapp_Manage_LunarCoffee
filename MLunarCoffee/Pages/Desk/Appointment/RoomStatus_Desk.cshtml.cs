using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Desk.Appointment
{
    public class RoomStatus_DeskModel : PageModel
    {
        public string _dataBranch { get; set; }
        public int _BranchID { get; set; }
        public string _Type { get; set; }
        public string _CurrentFloor { get; set; }
        public string _CurrentRoom { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            _CurrentFloor = (user.sys_floorID).ToString();
            _CurrentRoom = (user.sys_roomID).ToString();
            string Type = Request.Query["Type"];
            if (Type != null)
            {
                _Type = Type.ToString();
            }
            else _Type = "0";
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1
                    );
                }
                if (dt != null && dt.Rows.Count != 0)
                    return Content(Comon.DataJson.Datatable(dt));
                else
                    return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadRoom(int BranchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Room_Load" ,CommandType.StoredProcedure
                        ,"@Branchid" ,SqlDbType.Int ,BranchID);
                    dt.TableName = "Room";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Floor_Load]" ,CommandType.StoredProcedure
                     ,"@Branchid" ,SqlDbType.Int ,BranchID);
                    dt.TableName = "Floor";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ChairBed_Load]" ,CommandType.StoredProcedure
                     ,"@Branchid" ,SqlDbType.Int ,BranchID);
                    dt.TableName = "ChairBed";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));


            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadataRoom(int Branchid ,int floorID ,int roomID ,int Appid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_RoomStatus_Load]" ,CommandType.StoredProcedure
                        ,"@Branchid" ,SqlDbType.Int ,Branchid
                        ,"@floorID" ,SqlDbType.Int ,floorID
                        ,"@roomID" ,SqlDbType.Int ,roomID
                        ,"@appid" ,SqlDbType.Int ,Appid);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadDetailRoom(int RoomID ,int ChairID ,int BranchID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_RoomScheduleDetailStatus_Load]" ,CommandType.StoredProcedure
                        ,"@RoomID" ,SqlDbType.Int ,RoomID
                        ,"@ChairID" ,SqlDbType.Int ,ChairID
                        ,"@BranchID" ,SqlDbType.Int ,BranchID
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }



        public async Task<IActionResult> OnPostLoadEmpWork(int BranchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
               
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_v2_Work_Schedule_Appointment_ByBranch]" ,CommandType.StoredProcedure ,
                          "@DateFrom" ,SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                          "@DateTo" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow(),
                          "@BranchID" ,SqlDbType.NVarChar, BranchID
                    );
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadEmployeeFull(user.sys_branchID, 0);
                    dt.TableName = "Employee";
                    ds.Tables.Add(dt);
                }
             
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
    }
}
