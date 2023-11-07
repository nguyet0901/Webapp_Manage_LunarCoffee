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
    public class CustomerCareModel : PageModel
    {
        public int sys_CareTypeID { get; set; }
        public int sys_UserID { get; set; }
        public int sys_Permission_Tele { get; set; }
        public string layout { get; set; }
        public string _CurrentPath { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            sys_UserID = user.sys_userid;
            sys_Permission_Tele = Comon.Global.sys_Permission_Tele;
            string _layout = Request.Query["layout"];
            string _type = Request.Query["Type"];
            layout = _layout != null ? _layout : "";
            sys_CareTypeID = _type != null ? Convert.ToInt32(_type.ToString()) : 0;
            _CurrentPath = HttpContext.Request.Path != null ? HttpContext.Request.Path : "";
        }

        public async Task<IActionResult> OnPostInitialization(int Type)
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
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_LoadStatus_ByType]", CommandType.StoredProcedure,
                           "@TypeID", SqlDbType.Int, Type);
                        dt.TableName = "TypeCare";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Appointment_LoadType", CommandType.StoredProcedure);
                        dt.TableName = "TypeSchedule";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadPara("TypeIntput");
                        dt.TableName = "StatusData";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_ListTele" ,CommandType.StoredProcedure);
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


        public async Task<IActionResult> OnPostLoadData(string data, string DateFrom, string DateTo, string CurrentID, string BeginID, string Limit, int Type)
        {
            try
            {

                DataTable dt = new DataTable();
                CustomerCare dtMain = JsonConvert.DeserializeObject<CustomerCare>(data);
                var user = Session.GetUserSession(HttpContext);
                switch (Type)
                {
                    case 1:
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToRemind_Appointment]" , CommandType.StoredProcedure
                             , "@BranchID", SqlDbType.Int, dtMain.BranchID
                             , "@BranchToken", SqlDbType.NVarChar, dtMain.BranchToken
                             , "@StaffID", SqlDbType.Int, dtMain.StaffID
                             , "@StaffString", SqlDbType.NVarChar,dtMain.StaffString
                             , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                             , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                             , "@appID", SqlDbType.Int, CurrentID
                             , "@BeginID", SqlDbType.Int, BeginID
                             , "@statusCallID", SqlDbType.Int, Convert.ToInt32(dtMain.statusCallID)
                             , "@statusID", SqlDbType.Int, Convert.ToInt32(dtMain.statusID)
                             , "@TypeScheduleID", SqlDbType.Int, dtMain.TypeScheduleID
                             , "@StatusCalled", SqlDbType.Int, dtMain.StatusCalled
                             , "@CCStaffID", SqlDbType.Int, dtMain.CCStaffID
                             , "@UserID", SqlDbType.Int, user.sys_userid
                             , "@TypeData", SqlDbType.Int, dtMain.TypeData
                             , "@Limit", SqlDbType.Int, Limit
                             , "@UseRuleTele", SqlDbType.Int, dtMain.UseRuleTele
                             , "@SearchText", SqlDbType.NVarChar, dtMain.SearchText
                             );
                        }
                        break;
                    case 3:
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToTakeCare_Holiday]" ,CommandType.StoredProcedure
                             ,"@BranchID" ,SqlDbType.Int ,dtMain.BranchID
                             ,"@BranchToken" ,SqlDbType.NVarChar ,dtMain.BranchToken
                             ,"@StaffID" ,SqlDbType.Int ,dtMain.StaffID
                             ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                             ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                             ,"@CustID" ,SqlDbType.Int ,CurrentID
                             ,"@BeginID" ,SqlDbType.Int ,BeginID
                             ,"@StatusCalled" ,SqlDbType.Int ,dtMain.StatusCalled
                             ,"@CCStaffID" ,SqlDbType.Int ,dtMain.CCStaffID
                             ,"@Limit" ,SqlDbType.Int ,Limit
                             ,"@TypeData" ,SqlDbType.Int ,dtMain.TypeData
                             ,"@SearchText" ,SqlDbType.NVarChar ,dtMain.SearchText
                             ,"@statusCallID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.statusCallID)
                             ,"@statusID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.statusID)
                             );
                        }
                        break;
                    case 4:
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToFollow_NotCheckIn]", CommandType.StoredProcedure
                             , "@BranchID", SqlDbType.Int, dtMain.BranchID
                             , "@BranchToken", SqlDbType.NVarChar, dtMain.BranchToken
                             , "@StaffID", SqlDbType.Int, dtMain.StaffID
                             , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                             , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                             , "@appID", SqlDbType.Int, CurrentID
                             , "@BeginID", SqlDbType.Int, BeginID
                             , "@CallID", SqlDbType.Int, dtMain.StatusCalled
                             , "@CCStaffID" ,SqlDbType.Int ,dtMain.CCStaffID
                             , "@TypeScheduleID" ,SqlDbType.Int ,dtMain.TypeScheduleID
                             , "@Limit", SqlDbType.Int, Limit
                             , "@TypeData" ,SqlDbType.Int ,dtMain.TypeData
                             , "@SearchText" ,SqlDbType.NVarChar ,dtMain.SearchText
                             , "@statusCallID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.statusCallID)
                             , "@statusID" ,SqlDbType.Int ,Convert.ToInt32(dtMain.statusID)
                             );
                        }
                        break;
                    case 10:
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_ToTakeCare_AppointmentCancel]", CommandType.StoredProcedure
                             , "@BranchID", SqlDbType.Int, dtMain.BranchID
                             , "@BranchToken", SqlDbType.NVarChar, dtMain.BranchToken
                             , "@StaffID", SqlDbType.Int, dtMain.StaffID
                             , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                             , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                             , "@appID", SqlDbType.Int, CurrentID
                             , "@BeginID", SqlDbType.Int, BeginID
                             , "@StatusCalled" ,SqlDbType.Int ,dtMain.StatusCalled
                             , "@CCStaffID" ,SqlDbType.Int ,dtMain.CCStaffID
                             , "@Limit", SqlDbType.Int, Limit
                             , "@TypeData", SqlDbType.Int, dtMain.TypeData
                             ,"@SearchText" ,SqlDbType.NVarChar ,dtMain.SearchText
                             ,"@statusCallID", SqlDbType.Int, dtMain.statusCallID
                             ,"@statusID" ,SqlDbType.Int ,dtMain.statusID
                             );
                        }
                        break;

                    default:
                        break;
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public class CustomerCare
        {
            public int BranchID { get; set; }
            public string BranchToken { get; set; }
            public int StaffID { get; set; }
            public int TypeScheduleID { get; set; }
            public int StatusCalled { get; set; }
            public int CCStaffID { get; set; }
            public string StaffString { get; set; }
            public string TypeData { get; set; }
            public int UseRuleTele { get; set; }
            public string SearchText { get; set; }
            public int statusCallID { get; set; }
            public int statusID { get; set; }
        }
    }
}
