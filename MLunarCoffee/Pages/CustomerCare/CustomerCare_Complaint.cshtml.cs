using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.CustomerCare
{
    public class CustomerCare_ComplaintModel : PageModel
    {
        public int sys_CareTypeID { get; set; }
        public int sys_UserID { get; set; }
        public int sys_Permission_Tele { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            sys_UserID = user.sys_userid;
            sys_Permission_Tele = Comon.Global.sys_Permission_Tele;
            string _layout = Request.Query["layout"];
            string _type = Request.Query["Type"];
            layout = _layout != null ? _layout : "";
            sys_CareTypeID = _type != null ? Convert.ToInt32(_type.ToString()) : 0;
        }
        public async Task<IActionResult> OnPostInitialization()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadTele]" ,CommandType.StoredProcedure ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid);
                        dtTele.TableName = "Tele";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_StatusData]" ,CommandType.StoredProcedure);
                        dt.TableName = "StatusCall";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_LoadStatus_ByType]" ,CommandType.StoredProcedure ,
                           "@TypeID" ,SqlDbType.Int ,6);
                        dt.TableName = "StatusCare";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ListTele]" ,CommandType.StoredProcedure);
                        dt.TableName = "TeleCare";
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
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string data ,string DateFrom ,string DateTo ,string CurrentID ,string BeginID ,string Limit ,int Type ,string statusCallID)
        {
            try
            {
                CCComplaint dtMain = JsonConvert.DeserializeObject<CCComplaint>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToTakeCare_Complaint]" ,CommandType.StoredProcedure
                     ,"@BranchID" ,SqlDbType.Int ,dtMain.BranchID
                     ,"@BranchToken" ,SqlDbType.NVarChar ,dtMain.BranchToken
                     ,"@StaffID" ,SqlDbType.Int ,dtMain.StaffID
                     ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                     ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                     ,"@MasterID" ,SqlDbType.Int ,CurrentID
                     ,"@BeginID" ,SqlDbType.Int ,BeginID
                     ,"@Limit" ,SqlDbType.Int ,Limit
                     ,"@CCStaffID", SqlDbType.Int, dtMain.CCStaffID
                     ,"@CallID" ,SqlDbType.Int ,dtMain.CallID
                     ,"@TypeCare",SqlDbType.Int,dtMain.TypeCare
                     ,"@SearchText", SqlDbType.NVarChar, dtMain.SearchText
                     ,"@statusCallID", SqlDbType.Int, dtMain.statusCallID
                     ,"@statusID" ,SqlDbType.Int ,dtMain.statusID
                     );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string currentID)
        {
            try
            {

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Complaint_LoadDetail]" ,CommandType.StoredProcedure
                      ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(currentID)
                      ,"@CustID" ,SqlDbType.Int ,0
                      );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
    public class CCComplaint
    {
        public int BranchID { get; set; }
        public string BranchToken { get; set; }
        public int StaffID { get; set; }
        public int CallID { get; set; }
        public int CCStaffID { get; set; }
        public int TypeCare { get; set; }
        public string SearchText { get; set; }
        public int statusCallID { get; set; }
        public int statusID { get; set; }
    }
}
