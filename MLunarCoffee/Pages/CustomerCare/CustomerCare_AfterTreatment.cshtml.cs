using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.CustomerCare
{
    public class CustomerCare_AfterTreatmentModel : PageModel
    {
        public int sys_UserID { get; set; }
        public int sys_Permission_Tele { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
            var user = Session.GetUserSession(HttpContext);
            sys_UserID = user.sys_userid;
            sys_Permission_Tele = Comon.Global.sys_Permission_Tele;
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
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid
                            , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadTele]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                        dtTele.TableName = "Tele";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceType = new DataTable();
                        dtServiceType = await confunc.ExecuteDataTable("[YYY_sp_v2_Service_Type]", CommandType.StoredProcedure);
                        dtServiceType.TableName = "SerType";
                        return dtServiceType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_StatusCall_LoadCombo]" ,CommandType.StoredProcedure);
                        dtTele.TableName = "ParaCall";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_ListTele]" ,CommandType.StoredProcedure);
                        dtTele.TableName = "TeleCare";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_LoadStatus_ByType]" ,CommandType.StoredProcedure
                            , "@TypeID" ,SqlDbType.Int ,5);
                        dtTele.TableName = "TypeCare";
                        return dtTele;
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
        public async Task<IActionResult> OnPostLoadData(string data, string DateFrom, string DateTo, string currentID, string BeginID, string Limit)
        {
            try
            {
                CCTreatment dtMain = JsonConvert.DeserializeObject<CCTreatment>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToTakeCare_AfterTreatment]", CommandType.StoredProcedure
                     , "@BranchID", SqlDbType.Int, dtMain.BranchID
                     , "@BranchToken", SqlDbType.NVarChar, dtMain.BranchToken
                     , "@StaffID", SqlDbType.Int, dtMain.StaffID
                     , "@SerTypeID", SqlDbType.Int, dtMain.SerTypeID
                     , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                     , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                     , "@CurrentID", SqlDbType.VarChar, currentID
                     , "@BeginID", SqlDbType.Int, BeginID
                     , "@Limit", SqlDbType.Int, Limit
                     , "@CallID", SqlDbType.Int, dtMain.CallID
                     , "@CCStaffID", SqlDbType.Int, dtMain.CCStaffID
                     , "@TypeDate", SqlDbType.Int, dtMain.TypeDate
                     , "@SearchText", SqlDbType.NVarChar, dtMain.SearchText
                     , "@statusCallID", SqlDbType.Int, dtMain.statusCallID
                     , "@statusID" ,SqlDbType.Int ,dtMain.statusID
                     );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(string cus, string numcreated)
        {
            try
            {

                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_CustCare_AfterDetail]", CommandType.StoredProcedure
                     , "@cus", SqlDbType.Int, cus);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
    public class CCTreatment
    {
        public int BranchID { get; set; }
        public string BranchToken { get; set; }
        public int StaffID { get; set; }
        public int SerTypeID { get; set; }
        public int CallID { get; set; }
        public int CCStaffID { get; set; }
        public int TypeDate { get; set; }
        public string SearchText { get; set; }
        public int statusCallID { get; set; }
        public int statusID { get; set; }
    }
}
