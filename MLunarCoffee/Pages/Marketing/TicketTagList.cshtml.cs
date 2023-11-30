using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketTagListModel : PageModel
    {
        public string layout { get; set; }
        public int _UserTeleID { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";
            var user = Session.GetUserSession(HttpContext);
            _UserTeleID = Convert.ToInt32(user.sys_userid);
            CurrentPath = HttpContext.Request.Path;
        }


        public async Task<IActionResult> OnPostLoadComboMain(int userID)
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                        dt.TableName = "Source";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_StatusData]", CommandType.StoredProcedure);
                        dt.TableName = "Status";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {

                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_TicketTag_LoadTele]", CommandType.StoredProcedure,
                            "@UserID", SqlDbType.Int, userID);
                        dt.TableName = "Tele";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_GroupLoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "GroupTele";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_ServiceCare_LoadCombo", CommandType.StoredProcedure);
                        dt.TableName = "ServiceCare";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_StatusFollowTele]", CommandType.StoredProcedure);
                        dt.TableName = "StatusFollow";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_Tag]", CommandType.StoredProcedure);
                        dt.TableName = "Tag";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_CreateByUser]", CommandType.StoredProcedure);
                        dt.TableName = "UserTele";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Check_Tele_Permission]", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, userID);
                        dt.TableName = "PerTele";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Combo_CustomerGroup]" , CommandType.StoredProcedure);
                        dt.TableName = "CustGroup";
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


        public async Task<IActionResult> OnPostLoadTotal(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                TagList dtMain = JsonConvert.DeserializeObject<TagList>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TicketTagList_Total]" ,CommandType.StoredProcedure
                        ,"@GroupID" ,SqlDbType.Int ,dtMain.GroupID
                        ,"@StaffID" ,SqlDbType.Int ,dtMain.StaffID
                        ,"@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dtMain.DateFrom)
                        ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dtMain.DateTo)
                        ,"@statusID" ,SqlDbType.Int ,dtMain.StatusID
                        ,"@statusCall" ,SqlDbType.Int ,dtMain.StatusCallID
                        ,"@statusCallDetail" ,SqlDbType.Int ,dtMain.StatusCallDetail
                        ,"@sourceID" ,SqlDbType.Int ,dtMain.SourceID
                        ,"@custGroupID" ,SqlDbType.Int ,dtMain.CustGroupID
                        ,"@serviceCare" ,SqlDbType.Int ,dtMain.ServiceCare
                        ,"@createBy" ,SqlDbType.Int ,dtMain.CreateBy
                        ,"@IsCustomer" ,SqlDbType.Int ,dtMain.IsCustomer
                        ,"@Level" ,SqlDbType.Int ,dtMain.Level
                        ,"@GroupUser" ,SqlDbType.Int ,dtMain.GroupUser
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@TypeDate" ,SqlDbType.Int ,dtMain.TypeDate
                        ,"@TimeExecFrom" ,SqlDbType.Int ,dtMain.TimeExecFrom
                        ,"@TimeExecTo" ,SqlDbType.Int ,dtMain.TimeExecTo);
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

        public async Task<IActionResult> OnPostLoadData(string data, int BeginID, int Limit, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                TagList dtMain = JsonConvert.DeserializeObject<TagList>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_TicketTagList_Load]" , CommandType.StoredProcedure,
                          "@TagID", SqlDbType.Int, dtMain.TagID
                          , "@GroupID", SqlDbType.Int, dtMain.GroupID
                          , "@StaffID", SqlDbType.Int, dtMain.StaffID
                          , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dtMain.DateFrom)
                          , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dtMain.DateTo)
                          , "@limit", SqlDbType.Int, Limit
                          , "@idbegin", SqlDbType.Int, BeginID
                          , "@statusID", SqlDbType.Int, dtMain.StatusID
                          , "@statusCall", SqlDbType.Int, dtMain.StatusCallID
                          , "@statusCallDetail", SqlDbType.Int, dtMain.StatusCallDetail
                          , "@sourceID", SqlDbType.Int, dtMain.SourceID
                          , "@custGroupID" ,SqlDbType.Int ,dtMain.CustGroupID
                          , "@serviceCare", SqlDbType.Int, dtMain.ServiceCare
                          , "@createBy", SqlDbType.Int, dtMain.CreateBy
                          , "@IsCustomer", SqlDbType.Int, dtMain.IsCustomer
                          , "@Level", SqlDbType.Int, dtMain.Level
                          , "@GroupUser", SqlDbType.Int, dtMain.GroupUser
                          , "@UserID", SqlDbType.Int, user.sys_userid
                          , "@TypeDate", SqlDbType.Int, dtMain.TypeDate
                          , "@TimeExecFrom", SqlDbType.Int, dtMain.TimeExecFrom
                          , "@TimeExecTo", SqlDbType.Int, dtMain.TimeExecTo
                          , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
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

        public class TagList
        {
            public int GroupID { get; set; }
            public int StaffID { get; set; }
            public int TagID { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public int StatusID { get; set; }
            public int StatusCallID { get; set; }
            public int StatusCallDetail { get; set; }
            public int SourceID { get; set; }
            public int CustGroupID { get; set; }
            public int ServiceCare { get; set; }
            public int CreateBy { get; set; }
            public int IsCustomer { get; set; }
            public int Level { get; set; }
            public int GroupUser { get; set; }
            public int TypeDate { get; set; }
            public int TimeExecFrom { get; set; }
            public int TimeExecTo { get; set; }
        }
    }
}
