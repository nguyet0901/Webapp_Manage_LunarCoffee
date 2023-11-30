using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Ticket
{
    public class TicketActionTable : PageModel
    {
        public int _CustomerID { get; set; }
        public int _TicketID { get; set; }
        public int _AppID { get; set; }
        public int _MasterID { get; set; }
        public int _Type_Care { get; set; }
        public int _TreatmentID { get; set; }
        public string _TreatmentDate { get; set; }
        public string _HeaderTicketAction { get; set; }
        public string _TypeCallUsing { get; set; }
        public int _UserID { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                _UserID = user.sys_userid;
                _TypeCallUsing = Comon.Global.sys_ClientIsUsingCall.ToString();
                string _cus = Request.Query["CustomerID"];
                string _tic = Request.Query["TicketID"];
                string _typ = Request.Query["Type"];
                string _app = Request.Query["AppID"];
                string _mas = Request.Query["MasterID"];
                string _treatID = Request.Query["TreatmentID"];
                string _treatDate = Request.Query["TreatmentDate"];

                _TicketID = Convert.ToInt32((_tic == null || _tic == "") ? "0" : _tic.ToString());
                _CustomerID = Convert.ToInt32((_cus == null || _cus == "") ? "0" : _cus.ToString());

                _Type_Care = Convert.ToInt32((_typ == null || _typ == "") ? "0" : _typ.ToString());
                if (_Type_Care == 0) _Type_Care = 7; // trong truong hop vao tu tat ca truong hop ngoai tru, customer care
                _AppID = Convert.ToInt32((_app == null || _app == "") ? "0" : _app.ToString());
                _MasterID = Convert.ToInt32((_mas == null || _mas == "") ? "0" : _mas.ToString());
                _TreatmentID = Convert.ToInt32((_treatID == null || _treatID == "") ? "0" : _treatID.ToString());
                _TreatmentDate = ((_treatDate == null || _treatDate == "") ? "" : _treatDate.ToString());
                _HeaderTicketAction = DetectHeader(Convert.ToInt32(_Type_Care));
                if (_HeaderTicketAction == "") Response.Redirect("/assests/Error/index.html");
                //await Loadatabranch(_TicketID, _CustomerID);
                CurrentPath = HttpContext.Request.Path;
            }
            catch (Exception ex)
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

        private string DetectHeader(int type)
        {
            string resulf = "";
            switch (type)
            {
                case 1:
                    resulf = "Nhắc Lịch Khách Hàng";
                    break;
                case 2:
                    resulf = "Chăm Sóc Khách Tư Vấn Thất Bại";
                    break;
                case 3:
                    resulf = "Chăm Sóc Khách Sinh Nhật";
                    break;
                case 4:
                    resulf = "Chăm Sóc Khách Đặt Lịch Không Đến";
                    break;
                case 5:
                    resulf = "Chăm Sóc Khách Sau Điều Trị";
                    break;
                case 6:
                    resulf = "Comlaint Khách Hàng";
                    break;
                case 7:
                    resulf = "Ticket Info";
                    break;
                case 9:
                    resulf = "Chăm Sóc";
                    break;
                case 10:
                    resulf = "Chăm Sóc Hủy Lịch Hẹn";
                    break;
                default: break;
            }
            return resulf;
        }

        public async Task<IActionResult> OnPostLoadata(int TicketID, int CustomerID, int UserTeleLevel, int UserTeleGroup)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_GetInfoCheck]", CommandType.StoredProcedure,
                    "@Ticket", SqlDbType.Int, Convert.ToInt32(TicketID)
                    , "@Customer_ID", SqlDbType.Int, Convert.ToInt32(CustomerID));
            }
            if (dt != null && dt.Rows.Count == 1)
            {
                int BranchID = Convert.ToInt32(dt.Rows[0]["BranchID"]); // Chi nhanh của khách hàng
                int TeleID = Convert.ToInt32(dt.Rows[0]["TeleID"]); // Tele đang giữ ticket
                int TeleGroupID = Convert.ToInt32(dt.Rows[0]["TeleGroupID"]); // Nhóm của tele này
                int isAllowTele = 1;
                int isAllowBranch = 1;
                if (Global.sys_Permission_Tele == 1)
                {
                    if (UserTeleLevel == 2 && UserTeleGroup != TeleGroupID) isAllowTele = 0;
                    if (UserTeleLevel == 1 && user.sys_userid != TeleID) isAllowTele = 0;
                }
                if (Global.sys_CustomerNotViewByBranch == 1 && BranchID != 0)
                {
                    if (user.sys_AllBranchID == 0 && !("," + user.sys_AreaBranch + ",").Contains("," + BranchID.ToString() + ","))
                        isAllowBranch = 0;
                }
                if (isAllowTele == 1 && isAllowBranch == 1)
                {
                    return Content("1");
                }
                else return Content("0");
            }
            else
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadata_History(string TicketID, string CustomerID, string BeginCC, string BeginValue, string BeginHist)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_v2_sp_Ticket_Customer_LoadHistoryCare]", CommandType.StoredProcedure,
                     "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                      , "@Ticket", SqlDbType.Int, Convert.ToInt32(TicketID)
                      , "@BeginCC", SqlDbType.Int, Convert.ToInt32(BeginCC)
                      , "@BeginValue", SqlDbType.Int, Convert.ToInt32(BeginValue)
                      , "@BeginHist", SqlDbType.Int, Convert.ToInt32(BeginHist)
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
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadata_SMS(string TicketID, string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_v2_Ticket_GetInfo_LoadSMS]", CommandType.StoredProcedure,
                     "@Ticket", SqlDbType.Int, Convert.ToInt32(TicketID),
                      "@Customer_ID", SqlDbType.Int, Convert.ToInt32(CustomerID)
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
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadata_Info(string TicketID, string CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Ticket_GetInfo]", CommandType.StoredProcedure,
                        "@Ticket", SqlDbType.Int, Convert.ToInt32(TicketID),
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID));
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
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadataCustomerCareList(string CustomerID, string TicketID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Customer_LoadHistoryCare]", CommandType.StoredProcedure,
                  "@Customer_ID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                  , "@Ticket", SqlDbType.Int, Convert.ToInt32(TicketID));
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

        public async Task<IActionResult> OnPostCheckCustomerExist(string TicketID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Customer_CheckExist]", CommandType.StoredProcedure,
                  "@TicketID", SqlDbType.Int, Convert.ToInt32(Convert.ToInt32(TicketID)));
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            else
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadata_Todo(string TicketID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Get_Todo_Ticket_Customer]", CommandType.StoredProcedure,
                        "@TicketID", SqlDbType.Int, Convert.ToInt32(TicketID)
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

        public async Task<IActionResult> OnPostLoadata_Tag(int TicketID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TagByTicket]", CommandType.StoredProcedure,
                        "@TicketID", SqlDbType.Int, Convert.ToInt32(TicketID));
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

        public async Task<IActionResult> OnPostHistoryTeleDelete(int TicketID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Ticket_HistoryTele_Delete", CommandType.StoredProcedure,
                        "@TicketID", SqlDbType.Int, Convert.ToInt32(TicketID)
                        , "@UserID", SqlDbType.Int, user.sys_userid
                    );
                    
                }
                return Content(dt.Rows[0]["result"].ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
