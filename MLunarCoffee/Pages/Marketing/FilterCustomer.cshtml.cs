using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Marketing
{
    public class FilterCustomerModel : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public FilterCustomerModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
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

        public async Task<IActionResult> OnPostInitializeComboType()
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
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
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
                        DataTable dtCustomerGroup = new DataTable();
                        dtCustomerGroup = await confunc.ExecuteDataTable("[MLU_sp_Customer_Group_ComboLoad]" ,CommandType.StoredProcedure);
                        dtCustomerGroup.TableName = "CustomerGroup";
                        return dtCustomerGroup;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCustomerSource = new DataTable();
                        dtCustomerSource = await connFunc.ExecuteDataTable("MLU_sp_Customer_Type_ComboLoad" ,CommandType.StoredProcedure);
                        dtCustomerSource.TableName = "CustomerSource";
                        return dtCustomerSource;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGender = new DataTable();
                        dtGender = await connFunc.LoadPara("Gender");
                        dtGender.TableName = "Gender";
                        return dtGender;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCareer = new DataTable();
                        dtCareer = await connFunc.ExecuteDataTable("MLU_sp_Career_Load" ,CommandType.StoredProcedure);
                        dtCareer.TableName = "Career";
                        return dtCareer;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceType = new DataTable();
                        dtServiceType = await connFunc.ExecuteDataTable("MLU_sp_v2_Service_Type" ,CommandType.StoredProcedure);
                        dtServiceType.TableName = "ServiceType";
                        return dtServiceType;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtInsurance = new DataTable();
                        dtInsurance = await connFunc.ExecuteDataTable("[MLU_sp_Insurance_LoadComBo]" ,CommandType.StoredProcedure);
                        dtInsurance.TableName = "Insurance";
                        return dtInsurance;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCity = new DataTable();
                        dtCity = await connFunc.ExecuteDataTable("[MLU_sp_LoadCombo_LocationCity]" ,CommandType.StoredProcedure);
                        dtCity.TableName = "City";
                        return dtCity;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDistrict = new DataTable();
                        dtDistrict = await connFunc.ExecuteDataTable("[MLU_sp_LoadCombo_LocationDistrict]" ,CommandType.StoredProcedure);
                        dtDistrict.TableName = "District";
                        return dtDistrict;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDistrict = new DataTable();
                        dtDistrict = await connFunc.ExecuteDataTable("[MLU_sp_CustomerMembership_LoadCombo]" ,CommandType.StoredProcedure);
                        dtDistrict.TableName = "MemberShip";
                        return dtDistrict;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtService = new DataTable();
                        dtService = await connFunc.ExecuteDataTable("[MLU_sp_Service_LoadCombo]" ,CommandType.StoredProcedure);
                        dtService.TableName = "Service";
                        return dtService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtService = new DataTable();
                        dtService = await connFunc.ExecuteDataTable("[MLU_sp_Service_LoadList]" ,CommandType.StoredProcedure);
                        dtService.TableName = "AllService";
                        return dtService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBroker = new DataTable();
                        dtBroker = await connFunc.ExecuteDataTable("[MLU_sp_LoadCombo_Broker]" ,CommandType.StoredProcedure);
                        dtBroker.TableName = "Broker";
                        return dtBroker;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTele = new DataTable();
                        dtTele = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadTele_All]" ,CommandType.StoredProcedure);
                        dtTele.TableName = "Tele";
                        return dtTele;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMarketing = new DataTable();
                        dtMarketing = await confunc.ExecuteDataTable("[MLU_sp_MarketingEmployee_LoadCombo]" ,CommandType.StoredProcedure);
                        dtMarketing.TableName = "Marketing";
                        return dtMarketing;
                    }
                }));
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("ID" ,typeof(int));
                dt1.Columns.Add("Name" ,typeof(String));

                DataRow dr = dt1.NewRow();
                dr["ID"] = 1;
                dr["Name"] = "Đang Điều Trị";
                dt1.Rows.Add(dr);

                DataRow dr1 = dt1.NewRow();
                dr1["ID"] = 2;
                dr1["Name"] = "Hoàn Thành Điều Trị";
                dt1.Rows.Add(dr1);

                dt1.TableName = "TreatStatus";
                ds.Tables.Add(dt1);


                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadInitialize_Devide_Data()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_GroupTele_LoadCombo]" ,CommandType.StoredProcedure ,
                    "@UserID" ,SqlDbType.Int ,user.sys_userid);
                    dt.TableName = "dataGroup";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TeleLevel_LoadCombo]" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid);
                    dt.TableName = "dataLevel";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadDataUser(string GroupID ,string LevelID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TicketDevide_LoadUser]" ,CommandType.StoredProcedure
                    ,"@GroupID" ,SqlDbType.Int ,Convert.ToInt32(GroupID)
                    ,"@LevelID" ,SqlDbType.Int ,Convert.ToInt32(LevelID)
                    ,"@UserID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_userid)
                                        );
                    dt.TableName = "dataUser";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadataCustomer(string DateFrom ,string DateTo ,int SearchDay
            ,string CusGroup ,int CusGender
            ,string AmountFrom ,string AmountTo ,int SearchAmount
            ,int CusAgeFrom ,int CusAgeTo ,int SearchAge
            ,string CusBranch ,string CusSource ,string CusBroker ,int CusCity ,int CusDistrict
            ,string UsedServiceFrom ,string UsedServiceTo ,int SearchDayService
            ,string CusTreatFrom ,string CusTreatTo ,int SearchCusLastTreat ,string BirthDayFrom ,string BirthDayTo ,string SearchDayBirth
            ,string ServiceType ,int Insurance ,int TreatmentStatus ,string Career ,string limit ,string isdentic ,string service ,string SeriveTreat
            ,string ServiceTreatFrom ,string ServiceTreatTo ,string SearchServiceTreat ,string ServiceTreatType
            ,string SerDebtFrom ,string SerDebtTo ,string PayFrom ,string PayTo
            ,string TeleID ,string CustCareID
            )
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Marketing_Filter_Customer_v2]" ,CommandType.StoredProcedure
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                        ,"@SearchDay" ,SqlDbType.Int ,SearchDay

                        ,"@CusGroup" ,SqlDbType.NVarChar ,(CusGroup != null ? CusGroup.ToString() : "")
                        ,"@CusGender" ,SqlDbType.Int ,CusGender

                        ,"@CusAmountFrom" ,SqlDbType.Decimal ,Convert.ToDecimal(AmountFrom)
                        ,"@CusAmountTo" ,SqlDbType.Decimal ,Convert.ToDecimal(AmountTo)
                        ,"@SearchAmount" ,SqlDbType.Int ,SearchAmount


                        ,"@CusAgeFrom" ,SqlDbType.Int ,CusAgeFrom
                        ,"@CusAgeTo" ,SqlDbType.Int ,CusAgeTo
                        ,"@SearchAge" ,SqlDbType.Int ,SearchAge

                        ,"@CusBranch" ,SqlDbType.NVarChar ,(CusBranch != null ? CusBranch.ToString() : "")
                        ,"@CusSource" ,SqlDbType.NVarChar ,(CusSource != null ? CusSource.ToString() : "")
                        ,"@CusBroker" ,SqlDbType.Int ,Convert.ToInt32(CusBroker)
                        ,"@CusCity" ,SqlDbType.Int ,CusCity
                        ,"@CusDistrict" ,SqlDbType.Int ,CusDistrict
                        ,"@UsedServiceFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(UsedServiceFrom)
                        ,"@UsedServiceTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(UsedServiceTo)
                        ,"@SearchDayService" ,SqlDbType.Int ,SearchDayService

                        ,"@CusTreatFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(CusTreatFrom)
                        ,"@CusTreatTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(CusTreatTo)
                        ,"@SearchCusLastTreat" ,SqlDbType.Int ,SearchCusLastTreat
                        ,"@BirthDayFrom" ,SqlDbType.VarChar ,BirthDayFrom
                        ,"@BirthDayTo" ,SqlDbType.VarChar ,BirthDayTo
                        ,"@SearchDayBirth" ,SqlDbType.Int ,SearchDayBirth

                        ,"@ServiceType" ,SqlDbType.NVarChar ,(ServiceType != null ? ServiceType.ToString() : "")
                        ,"@Service" ,SqlDbType.NVarChar ,(service != null ? service.ToString() : "")
                        ,"@Insurance" ,SqlDbType.Int ,Insurance
                        ,"@TreatmentStatus" ,SqlDbType.Int ,TreatmentStatus
                        ,"@Career" ,SqlDbType.NVarChar ,(Career != null ? Career.ToString() : "")
                        ,"@Limit" ,SqlDbType.Int ,Convert.ToInt32(limit)
                        ,"@Dentist" ,SqlDbType.Int ,Convert.ToInt32(isdentic)
                        ,"@ServiceTreat" ,SqlDbType.Int ,Convert.ToInt32(SeriveTreat != null ? SeriveTreat.ToString() : 0)
                        ,"@ServiceTreatFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(ServiceTreatFrom)
                        ,"@ServiceTreatTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(ServiceTreatTo)
                        ,"@SearchServiceTreat" ,SqlDbType.Int ,Convert.ToInt32(SearchServiceTreat)
                        ,"@ServiceTreatType" ,SqlDbType.NVarChar ,(ServiceTreatType != null ? ServiceTreatType.ToString() : "")
                        ,"@ServiceDebtFrom" ,SqlDbType.Int ,Convert.ToInt32(SerDebtFrom)
                        ,"@ServiceDebtTo" ,SqlDbType.Int ,Convert.ToInt32(SerDebtTo)
                        ,"@PayFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(PayFrom)
                        ,"@PayTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(PayTo)
                        ,"@TeleID" ,SqlDbType.Int ,TeleID
                        ,"@CustCareID" ,SqlDbType.Int ,CustCareID
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

        public async Task<IActionResult> OnPostExcuteDevideCustomer(string data ,string note ,string stringStaff)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtCustomer = new DataTable();
                dtCustomer = JsonConvert.DeserializeObject<DataTable>(data);
                string stringuser = "";
                note = note != null ? note : "";
                for (int i = 0; i < dtCustomer.Rows.Count; i++)
                {
                    stringuser = stringuser + dtCustomer.Rows[i]["StaffID"].ToString() + ",";
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    await confunc.ExecuteDataTable("[MLU_sp_Customer_Fiter_DevideData]" ,CommandType.StoredProcedure
                         ,"@Table" ,SqlDbType.Structured ,dtCustomer.Rows.Count > 0 ? dtCustomer : null
                         ,"@Note" ,SqlDbType.Int ,note.Replace("'" ,"").Trim()
                         ,"@stringStaff" ,SqlDbType.NVarChar ,stringStaff
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


        #region // Send SMS

        public IActionResult OnPostCheckContent(string content)
        {
            try
            {
                content = content != null ? content : "";
                if (content == "" || content.Length < 5) return Content("0");
                return Content(Comon.Comon.CheckContentSMS(content));
            }
            catch (Exception ex)
            {
                return Content("-1");
            }
        }

        public async Task<IActionResult> OnPostSendSMSMulti(string data ,string content)
        {
            try
            {
                if (content == "" || content.Length < 5) return Content("0");
                if (Comon.Comon.CheckContentSMS(content) != "1") return Content("-2"); //Content khong chap nhan
                var user = Session.GetUserSession(HttpContext);
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt.Columns.Add("CusID");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Status");
                string resulf = "0";
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    string phone = dataDetail.Rows[i]["Phone"].ToString();
                    string CusID = dataDetail.Rows[i]["CusID"].ToString();
                    resulf = "0";// Comon.Comon.SendSMS(content, phone);
                    DataRow dr = dt.NewRow();
                    dr["CusID"] = CusID;
                    dr["Phone"] = phone;
                    if (Convert.ToInt32(resulf) < 3 && Convert.ToInt32(resulf) > 0)
                    {
                        dr["Status"] = 1;
                    }
                    else
                    {
                        dr["Status"] = 0;
                    }
                    dt.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("MLU_sp_Customer_SMS_Multi" ,CommandType.StoredProcedure ,
                           "@Content" ,SqlDbType.NVarChar ,content ,
                           "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                           "@table_data" ,SqlDbType.Structured ,dt.Rows.Count > 0 ? dt : null
                       );
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        #endregion
    }
}
