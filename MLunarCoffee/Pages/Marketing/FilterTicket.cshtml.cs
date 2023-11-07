using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class FilterTicketModel : PageModel
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
        public async Task<IActionResult> OnPostLoadataTicket(string DateFrom ,string DateTo ,int SearchDay ,string ExecutedFrom
             ,string ExecutedTo ,int ExecutedDay ,string LastCheckinFrom ,string LastCheckinTo ,int LastCheckinDay ,int FollowFrom ,int FollowTo ,int FollowTime ,int Gender ,int ServiceCare
             ,string TKCusGroup ,string TKStatus ,string TKSource ,string TKTele ,string TKTeleGroup ,string EmployeMar ,string TypeData ,string ScheduleCus ,int Limit
             ,string City ,string District ,string AgeFrom ,string AgeTo ,string SearchAge
             ,string LastTreatFrom ,string LastTreatTo ,int LastTreatDay ,string LastCareFrom ,string LastCareTo ,int LastCareDay
             ,string DivideFrom ,string DivideTo,int DivideDay
            )
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Ticket_Filter]" ,CommandType.StoredProcedure ,
                        "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                        ,"@SearchDay" ,SqlDbType.Int ,SearchDay
                        ,"@ExecutedFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(ExecutedFrom)
                        ,"@ExecutedTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(ExecutedTo)
                        ,"@ExecutedDay" ,SqlDbType.Int ,ExecutedDay
                        ,"@LastCheckinFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(LastCheckinFrom)
                        ,"@LastCheckinTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(LastCheckinTo)
                        ,"@LastCheckinDay" ,SqlDbType.Int ,LastCheckinDay
                        ,"@FollowFrom" ,SqlDbType.Int ,FollowFrom
                        ,"@FollowTo" ,SqlDbType.Int ,FollowTo
                        ,"@FollowTime" ,SqlDbType.Int ,FollowTime
                        ,"@Gender" ,SqlDbType.Int ,Gender
                        ,"@ServiceCare" ,SqlDbType.Int ,ServiceCare
                        ,"@TKCusGroup" ,SqlDbType.NVarChar ,(TKCusGroup != null ? TKCusGroup.ToString() : "")
                        ,"@TKStatus" ,SqlDbType.NVarChar ,(TKStatus != null ? TKStatus.ToString() : "")
                        ,"@TKSource" ,SqlDbType.NVarChar ,(TKSource != null ? TKSource.ToString() : "")
                        ,"@TKTele" ,SqlDbType.NVarChar ,(TKTele != null ? TKTele.ToString() : "")
                        ,"@TKTeleGroup" ,SqlDbType.NVarChar ,(TKTeleGroup != null ? TKTeleGroup.ToString() : "")
                        ,"@EmployeMar" ,SqlDbType.Int ,EmployeMar
                        ,"@TypeData" ,SqlDbType.Int ,Convert.ToInt32(TypeData)
                        ,"@ScheduleCus" ,SqlDbType.Int ,Convert.ToInt32(ScheduleCus)
                        ,"@City" ,SqlDbType.Int ,Convert.ToInt32(City)
                        ,"@District" ,SqlDbType.Int ,Convert.ToInt32(District)
                        ,"@Limit" ,SqlDbType.Int ,Limit
                        ,"@AgeFrom" ,SqlDbType.Int ,Convert.ToInt32(AgeFrom)
                        ,"@AgeTo" ,SqlDbType.Int ,Convert.ToInt32(AgeTo)
                        ,"@SearchAge" ,SqlDbType.Int ,Convert.ToInt32(SearchAge)
                        ,"@LastTreatFrom" ,SqlDbType.DateTime ,LastTreatFrom
                        ,"@LastTreatTo" ,SqlDbType.DateTime ,LastTreatTo
                        ,"@LastTreatDay" ,SqlDbType.Int ,LastTreatDay
                        ,"@LastCareFrom" ,SqlDbType.DateTime ,LastCareFrom
                        ,"@LastCareTo" ,SqlDbType.DateTime ,LastCareTo
                        ,"@LastCareDay" ,SqlDbType.Int ,LastCareDay
                        ,"@DivideFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DivideFrom)
                        ,"@DivideTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DivideTo)
                        ,"@DivideDay" ,SqlDbType.Int ,DivideDay


                        );
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

        public async Task<IActionResult> OnPostLoadTeleSaleGroup(string GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Filter_GroupTeleSale_Load]" ,CommandType.StoredProcedure
                        ,"@GroupID" ,SqlDbType.Int ,Convert.ToInt32(GroupID));
                    if (dt != null && dt.Rows.Count != 0)
                        return Content(Comon.DataJson.Datatable(dt));
                    else return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSource = new DataTable();
                        dtSource = await confunc.ExecuteDataTable("YYY_sp_Customer_Type_LoadList" ,CommandType.StoredProcedure);
                        dtSource.TableName = "Source";
                        return dtSource;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtEmployee = new DataTable();
                        dtEmployee = await confunc.LoadEmployeeFull(user.sys_branchID ,user.sys_AllBranchID);
                        dtEmployee.TableName = "Employee";
                        return dtEmployee;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroupTele = new DataTable();
                        dtGroupTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_TeleGroup_Load]" ,CommandType.StoredProcedure);
                        dtGroupTele.TableName = "GroupTele";
                        return dtGroupTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCustomerGroup = new DataTable();
                        dtCustomerGroup = await confunc.ExecuteDataTable("[YYY_sp_Customer_Group_ComboLoad]" ,CommandType.StoredProcedure);
                        dtCustomerGroup.TableName = "CustomerGroup";
                        return dtCustomerGroup;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGender = new DataTable();
                        dtGender = await confunc.LoadPara("Gender");
                        dtGender.TableName = "Gender";
                        return dtGender;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                          ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                          ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtStatus = new DataTable();
                        dtStatus = await confunc.ExecuteDataTable("[YYY_sp_LoadCombo_Para_All]" ,CommandType.StoredProcedure
                         ,"@paraTypeName" ,SqlDbType.Int ,"TypeIntput");
                        dtStatus.TableName = "Status";
                        return dtStatus;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCare = new DataTable();
                        dtServiceCare = await confunc.ExecuteDataTable("[YYY_sp_ServiceCare_LoadCombo]" ,CommandType.StoredProcedure);
                        dtServiceCare.TableName = "ServiceCare";
                        return dtServiceCare;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadTele_All]" ,CommandType.StoredProcedure);
                        dtTele.TableName = "Tele";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMarketing = new DataTable();
                        dtMarketing = await confunc.ExecuteDataTable("[YYY_sp_MarketingEmployee_LoadCombo]" ,CommandType.StoredProcedure);
                        dtMarketing.TableName = "Marketing";
                        return dtMarketing;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtStatusFollow = new DataTable();
                            dtStatusFollow = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_StatusFollowTele]" ,CommandType.StoredProcedure);
                            dtStatusFollow.TableName = "StatusFollow";
                            return dtStatusFollow;
                        }
                    }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCity = new DataTable();
                        dtCity = await connFunc.ExecuteDataTable("[YYY_sp_LoadCombo_LocationCity]" ,CommandType.StoredProcedure);
                        dtCity.TableName = "City";
                        return dtCity;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDistrict = new DataTable();
                        dtDistrict = await connFunc.ExecuteDataTable("[YYY_sp_LoadCombo_LocationDistrict]" ,CommandType.StoredProcedure);
                        dtDistrict.TableName = "District";
                        return dtDistrict;
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

        public async Task<IActionResult> OnPostExcuteMoveTicket(string dataTicket ,string dataTele)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtTicket = new DataTable();
                DataTable dtTele = new DataTable();
                dtTele = JsonConvert.DeserializeObject<DataTable>(dataTele);
                dtTicket = JsonConvert.DeserializeObject<DataTable>(dataTicket);

                DataTable dtData = new DataTable();
                dtData.Columns.Add("TicketID" ,typeof(String));
                dtData.Columns.Add("TeleID" ,typeof(String));
                int NumTicket = 0;
                int TotalTicket = 0;
                for (int i = 0; i < dtTele.Rows.Count; i++)
                {
                    NumTicket = Convert.ToInt32(dtTele.Rows[i]["NumTicket"]);
                    for (int j = TotalTicket; j < (TotalTicket + NumTicket); j++)
                    {
                        DataRow dr = dtData.NewRow();
                        dr["TicketID"] = dtTicket.Rows[j]["TicketID"];
                        dr["TeleID"] = dtTele.Rows[i]["UserID"];
                        dtData.Rows.Add(dr);

                    }
                    TotalTicket += NumTicket;
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    await confunc.ExecuteDataTable("[YYY_sp_Ticket_Fiter_MoveTicket]" ,CommandType.StoredProcedure
                        ,"@Table" ,SqlDbType.Structured ,dtData.Rows.Count > 0 ? dtData : null
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
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
