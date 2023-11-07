using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{

    public class TicketList : PageModel
    {

        public string layout { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadCombo(string userID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dts = new DataTable();
                DataTable dt = new DataTable();
                DataTable dtsd = new DataTable();
                DataTable dtTele = new DataTable();
                DataTable dtPerTele = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Name");
                DataRow dr = dt.NewRow(); dr[0] = 0; dr[1] = "Chọn Tình Trạng";
                DataRow dr1 = dt.NewRow(); dr1[0] = 1; dr1[1] = "Cần Xử Lý";
                DataRow dr2 = dt.NewRow(); dr2[0] = 2; dr2[1] = "Đã Xử Lý";
                dt.Rows.Add(dr); dt.Rows.Add(dr1); dt.Rows.Add(dr2);
                dt.TableName = "Status";
                ds.Tables.Add(dt);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dts = await confunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]" ,CommandType.StoredProcedure);
                    dts.TableName = "Source";
                    ds.Tables.Add(dts);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dtsd = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_StatusData]" ,CommandType.StoredProcedure);
                    dtsd.TableName = "StatusData";
                    ds.Tables.Add(dtsd);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dtTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadTele]" ,CommandType.StoredProcedure ,"@UserID" ,SqlDbType.Int ,userID);
                    dtTele.TableName = "Tele";
                    ds.Tables.Add(dtTele);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dtPerTele = await confunc.ExecuteDataTable("[YYY_sp_Check_Tele_Permission]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,userID);
                    dtPerTele.TableName = "PerTele";
                    ds.Tables.Add(dtPerTele);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dtPerTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_GroupLoadCombo]" ,CommandType.StoredProcedure);
                    dtPerTele.TableName = "GroupTele";
                    ds.Tables.Add(dtPerTele);
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }


        }
        // IF isTypeLoad =0 : Load theo user hien hanh
        public async Task<IActionResult> OnPostLoadList(string data ,int TicketID ,string idbegin ,string limit ,string LevelUser ,string GroupUser)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                TicketListData dataMain = JsonConvert.DeserializeObject<TicketListData>(data);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_v2_Ticket_TicketList_Load]" ,CommandType.StoredProcedure ,
                          "@StaffID" ,SqlDbType.Int ,dataMain.UserID
                          ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                          ,"@TicketID" ,SqlDbType.Int ,TicketID
                          ,"@Type" ,SqlDbType.Int ,dataMain.Type
                          ,"@TypeDate" ,SqlDbType.Int ,dataMain.TypeDate
                          ,"@Status" ,SqlDbType.Int ,dataMain.Status
                          ,"@Source" ,SqlDbType.Int ,dataMain.Source
                          ,"@IsCust" ,SqlDbType.Int ,dataMain.IsCust
                          ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateFrom)
                          ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateTo)
                          ,"@limit" ,SqlDbType.Int ,Convert.ToInt32(limit)
                          ,"@idbegin" ,SqlDbType.Int ,Convert.ToInt32(idbegin)
                          ,"@Level" ,SqlDbType.Int ,Convert.ToInt32(LevelUser)
                          ,"@GroupTele" ,SqlDbType.Int ,Convert.ToInt32(GroupUser)
                          ,"@GroupID" ,SqlDbType.Int ,dataMain.GroupID
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
    }

    public class TicketListData
    {
        public int UserID { get; set; }
        public int Type { get; set; }
        public int TypeDate { get; set; }
        public int Status { get; set; }
        public int Source { get; set; }
        public int IsCust { get; set; } = 0;
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public int GroupID { get; set; }
    }
}
