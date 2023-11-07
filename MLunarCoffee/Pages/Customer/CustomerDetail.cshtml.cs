using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Customer
{
    public class CustomerDetailModel : PageModel
    {
        public string _CustomerDetailID { get; set; }
        public string _ticketDetailID { get; set; }
        public int _checkPageAddSchedule { get; set; }
        public int _sys_ChooseDateCustomer { get; set; }
        public int _sys_BirthdayYearOnly { get; set; }
        public string _dataComboServicetype { get; set; }
        public int _RepresentPerson { get; set; }
        public int _RepresentPersonType { get; set; }
        public string _type { get; set; }
        public string _phone { get; set; }
        public string _customerName { get; set; }

        public string _dateAppointment { get; set; }
        public string _doctorid { get; set; }
        public string _servicecaretoken { get; set; }
        public string _TimeSchedule { get; set; }
        public string _NoteSchedule { get; set; }
        public int _BranchID { get; set; }
        public string _apptemid { get; set; }
        public string _preferredCountriesPhone { get; set; }
        public string CurrentPath { get; set; }
        public string _acappid { get; set; }
        public string _pageid { get; set; }
        public string _psid { get; set; }

        private readonly IHubContext<NotiHub> hubContext;

        public CustomerDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            _checkPageAddSchedule = 0;
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            _sys_ChooseDateCustomer = user.sys_ChooseDateCustomer;
            _preferredCountriesPhone = System.Uri.EscapeUriString(Global.sys_countriesPhone.ToString());
            _RepresentPerson = user.sys_Rule_OptionExtension.Before_16.Value;
            _RepresentPersonType = user.sys_Rule_OptionExtension.Before_16.TypeRule;

            _sys_BirthdayYearOnly = Global.sys_BirthdayYearOnly;

            string v = Request.Query["CustomerID"];
            if (v != null)
            {
                _CustomerDetailID = v == null ? "0" : v.ToString();
            }
            else
            {
                _CustomerDetailID = "0";
                string ticketID = Request.Query["TicketID"];
                if (ticketID != null)
                {
                    _ticketDetailID = (ticketID == null ? "0" : ticketID.ToString());
                }
                else
                {
                    _ticketDetailID = "0";
                    _checkPageAddSchedule = 1;
                }
                _ticketDetailID = (_ticketDetailID == "") ? "0" : _ticketDetailID;
            }
            string type = Request.Query["type"];
            string phone = Request.Query["phone"];
            string pageid = Request.Query["pid"];
            string psid = Request.Query["psid"];
            string customerName = Request.Query["custname"];
            string dateAppointment = Request.Query["dateapp"];
            string doctorid = Request.Query["doctorid"];
            string servicecaretoken = Request.Query["servicecaretoken"];
            string timeschedule = Request.Query["timeschedule"];
            string noteschedule = Request.Query["noteschedule"];
            string branch = Request.Query["branch"];
            string apptemid = Request.Query["apptemid"];
            string acappid = Request.Query["acappid"];

            if (type != null) _type = type;
            else _type = "";

            if (phone != null) _phone = phone;
            else _phone = "";
            if (customerName != null) _customerName = System.Uri.EscapeUriString(customerName.ToString());
            else _customerName = "";
            if (dateAppointment != null) _dateAppointment = dateAppointment;
            else _dateAppointment = "";
            if (doctorid != null) _doctorid = doctorid;
            else _doctorid = "";
            if (servicecaretoken != null) _servicecaretoken = servicecaretoken;
            else _servicecaretoken = "";
            if (timeschedule != null) _TimeSchedule = timeschedule;
            else _TimeSchedule = "";
            if (noteschedule != null) _NoteSchedule = System.Uri.EscapeUriString(noteschedule.ToString());
            else _NoteSchedule = "";
            if (branch != null) _BranchID = Convert.ToInt32(branch);
            if (apptemid != null) _apptemid = apptemid.ToString();
            else _apptemid = "0";
            if (acappid != null) _acappid = acappid.ToString();
            else _acappid = "0";

            if (pageid != null) _pageid = pageid.ToString();
            else _pageid = "0";
            if (psid != null) _psid = psid.ToString();
            else _psid = "0";



        }

        public async Task<IActionResult> OnPostLoadata(int Custumer_ID ,int Ticket_ID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Custumer_ID != 0)
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_LoadToEdit]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(Custumer_ID == 0 ? 0 : Custumer_ID));
                    }
                }
                else if (Ticket_ID != 0)
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_LoadByTicketID]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(Ticket_ID == 0 ? 0 : Ticket_ID));
                    }
                }
                else return Content("[]");
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(string CustomerID)
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
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_LoadLocation" ,CommandType.StoredProcedure
                            ,"@BranchID" ,SqlDbType.Int ,user.sys_branchID
                            );
                        dtBranch.TableName = "BranchLocation";
                        return dtBranch;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtConsult = new DataTable();
                        dtConsult = await confunc.LoadEmployeeFull(user.sys_branchID ,user.sys_AllBranchID);
                        dtConsult.TableName = "EmpFull";
                        return dtConsult;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCareer = new DataTable();
                        dtCareer = await confunc.ExecuteDataTable("[YYY_sp_Career_Load]" ,CommandType.StoredProcedure);
                        dtCareer.TableName = "Career";
                        return dtCareer;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSource = new DataTable();
                        dtSource = await confunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]" ,CommandType.StoredProcedure);
                        dtSource.TableName = "Source";
                        return dtSource;
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
                        DataTable dtLanguage = new DataTable();
                        dtLanguage = await confunc.LoadPara("Language");
                        dtLanguage.TableName = "Language";
                        return dtLanguage;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtReligion = new DataTable();
                        dtReligion = await confunc.LoadPara("Religion");
                        dtReligion.TableName = "Religion";
                        return dtReligion;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSexual = new DataTable();
                        dtSexual = await confunc.LoadPara("Sexual Orientation");
                        dtSexual.TableName = "SexualOrientation";
                        return dtSexual;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMaritalStatus = new DataTable();
                        dtMaritalStatus = await confunc.LoadPara("Marital Status");
                        dtMaritalStatus.TableName = "MaritalStatus";
                        return dtMaritalStatus;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBroker = new DataTable();
                        dtBroker = await confunc.ExecuteDataTable("[YYY_sp_LoadCombo_BrokerCus]" ,CommandType.StoredProcedure
                            ,"@CustumerID" ,SqlDbType.Int ,CustomerID);
                        dtBroker.TableName = "Broker";
                        return dtBroker;
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
                        DataTable dtAgreementOption = new DataTable();
                        dtAgreementOption = await confunc.ExecuteDataTable("[YYY_sp_Customer_Load_Agreement]" ,CommandType.StoredProcedure);
                        dtAgreementOption.TableName = "AgreementOption";
                        return dtAgreementOption;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtRelationship = new DataTable();
                        dtRelationship = await confunc.ExecuteDataTable("[YYY_sp_Relationship_LoadCombo]" ,CommandType.StoredProcedure);
                        dtRelationship.TableName = "Relationship";
                        return dtRelationship;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable ruleCust = new DataTable();
                        ruleCust = await confunc.ExecuteDataTable("[YYY_sp_Rule_Customer]" ,CommandType.StoredProcedure);
                        ruleCust.TableName = "RuleCust";
                        return ruleCust;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadEmployeeWork(string datefrom ,string dateto ,int groupID ,int branchID)
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
        public async Task<IActionResult> OnPostExcuteData(string data ,string avartar ,string CurrentID
            ,string TicketID ,string apptemid ,string acappid
            ,string dtRule
            ,string alowrule)
        {
            try
            {
                CustomerDetail DataMain = JsonConvert.DeserializeObject<CustomerDetail>(data);


                if (dtRule != null)
                {
                    var dataRule = JsonConvert.DeserializeObject<Dictionary<string ,Dictionary<string ,string>>>(dtRule);
                    foreach (KeyValuePair<string ,Dictionary<string ,string>> entry in dataRule)
                    {
                        var key = entry.Key;
                        var isRequire = entry.Value["isRequire"];
                        var isShow = entry.Value["isShow"];
                        var isUpdate = entry.Value["isUpdate"];
                        var updated = entry.Value["updated"];
                        if (isRequire == "true")
                        {
                            var item = DataMain.GetType().GetProperty(key).GetValue(DataMain ,null);
                            if (item.ToString() == "" || item.ToString() == "0" || item.ToString() == "01-01-1900")
                            {
                                return Content("-2");
                            }
                        }
                        if (isUpdate == "false" && updated == "1" && alowrule == "0")
                        {
                            return Content("-2");
                        }
                    }
                }
                string syntax_scmcn = DataMain.syntax_scmcn.Replace("'" ,"").Trim().ToLower();
                string syntax_scmcn_doc = DataMain.syntax_scmcn_doc.Replace("'" ,"").Trim().ToLower();
                string syntax_sn = DataMain.syntax_sn.Replace("'" ,"").Trim().ToLower();
                string syntax_ss = DataMain.syntax_ss.Replace("'" ,"").Trim().ToLower();
                string syntax_customercode = DataMain.syntax_customercode.Replace("'" ,"").Trim().ToLower();
                string syntax_documentcode = DataMain.syntax_documentcode.Replace("'" ,"").Trim().ToLower();
                if (syntax_customercode != "")
                {
                    syntax_customercode = syntax_customercode.Replace("[sn]" ,syntax_sn);
                    syntax_customercode = syntax_customercode.Replace("[ss]" ,syntax_ss);
                    syntax_customercode = syntax_customercode.Replace("[mcn_doc]" ,syntax_scmcn_doc);
                    syntax_customercode = syntax_customercode.Replace("[mcn]" ,syntax_scmcn);
                }
                if (syntax_documentcode != "")
                {
                    syntax_documentcode = syntax_documentcode.Replace("[sn]" ,syntax_sn);
                    syntax_documentcode = syntax_documentcode.Replace("[ss]" ,syntax_ss);
                    syntax_documentcode = syntax_documentcode.Replace("[mcn_doc]" ,syntax_scmcn_doc);
                    syntax_documentcode = syntax_documentcode.Replace("[mcn]" ,syntax_scmcn);
                }
                var user = Session.GetUserSession(HttpContext);
                string img = (avartar != "") ? avartar : Comon.Global.sys_DefaultPic;
                if (CurrentID == "0")
                {
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Insert" ,CommandType.StoredProcedure ,
                            "@CustCode" ,SqlDbType.NVarChar ,syntax_customercode ,
                            "@DocCode" ,SqlDbType.NVarChar ,syntax_documentcode ,
                            "@CharSplit" ,SqlDbType.NVarChar ,syntax_ss ,
                            "@BranchCode" ,SqlDbType.NVarChar ,syntax_scmcn ,
                            "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                            "@Gclid" , SqlDbType.NVarChar, DataMain.Gclid.Replace("'" ,"").Trim(),
                            "@Email1" ,SqlDbType.NVarChar ,DataMain.email.Replace("'" ,"").Trim() ,
                            "@Address" ,SqlDbType.NVarChar ,DataMain.address.Replace("'" ,"").Trim() ,
                            "@Phone1" ,SqlDbType.NVarChar ,DataMain.phone1.Replace("'" ,"").Trim() ,
                            "@Phone2" ,SqlDbType.NVarChar ,DataMain.phone2.Replace("'" ,"").Trim() ,
                            "@Branch_ID" ,SqlDbType.Int ,(DataMain.branch == "" ? 0 : Convert.ToInt32(DataMain.branch)) ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.custName.Replace("'" ,"").Trim() ,
                            "@NameNoSign" ,SqlDbType.NVarChar ,DataMain.NameNoSign.Replace("'" ,"").Trim() ,
                            "@Gender_ID" ,SqlDbType.Int ,DataMain.Gender_ID ,
                            "@Type_Cat_ID" ,SqlDbType.Int ,(DataMain.source == "" ? 0 : Convert.ToInt32(DataMain.source)) ,
                            "@Language_ID" ,SqlDbType.Int ,DataMain.Language_ID ,
                            "@Avatar" ,SqlDbType.NVarChar ,img ,
                            "@Birthday" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.birthday) ,
                            "@BirthYear" ,SqlDbType.Int ,DataMain.BirthYear ,
                            "@instgramurl" ,SqlDbType.NVarChar ,DataMain.instgramurl.Replace("'" ,"").Trim() ,
                            "@facebookurl" ,SqlDbType.NVarChar ,DataMain.facebookurl.Replace("'" ,"").Trim() ,
                            "@viberurl" ,SqlDbType.NVarChar ,DataMain.viberurl.Replace("'" ,"").Trim() ,
                            "@zalomurl" ,SqlDbType.NVarChar ,DataMain.zalomurl.Replace("'" ,"").Trim() ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Created" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.Created) ,
                            "@CityID" ,SqlDbType.Int ,DataMain.city ,
                            "@District" ,SqlDbType.Int ,DataMain.district ,
                            "@Commune" ,SqlDbType.Int ,DataMain.commune ,
                            "@OldCustomer" ,SqlDbType.Int ,DataMain.OldCustomer ,
                            "@TicketID" ,SqlDbType.Int ,TicketID ,
                            "@CustCodeOld" ,SqlDbType.NVarChar ,DataMain.CustCodeOld.Replace("'" ,"").Trim() ,
                            "@Career" ,SqlDbType.Int ,(DataMain.career == "" ? 0 : Convert.ToInt32(DataMain.career)) ,
                            "@phonecode" ,SqlDbType.NVarChar ,DataMain.phonecode.ToString() ,
                            "@Employee_ID" ,SqlDbType.Int ,user.sys_employeeid ,
                            "@BranchSchedule_ID" ,SqlDbType.Int ,DataMain.BranchSchedule_ID ,
                            "@Doctor_ID" ,SqlDbType.Int ,DataMain.Doctor_ID ,
                            "@Consult_ID" ,SqlDbType.Int ,DataMain.Consult_ID ,
                            "@ServiceCare_ID" ,SqlDbType.Int ,DataMain.ServiceCare_ID ,
                            "@Assistant_ID" ,SqlDbType.Int ,DataMain.Assistant_ID ,
                            "@TypeSchedule" ,SqlDbType.Int ,DataMain.TypeSchedule ,
                            "@Date_from" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from) ,
                            "@Date_to" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Date_from).AddMinutes(Convert.ToInt32(DataMain.numberDateTo)) ,
                            "@ContentSchedule" ,SqlDbType.NVarChar ,DataMain.ContentSchedule.Replace("'" ,"") ,
                            "@checkPageAddSchedule" ,SqlDbType.Int ,DataMain.checkPageAddSchedule ,
                            "@NationalID" ,SqlDbType.Int ,(DataMain.nationality == "" ? 0 : Convert.ToInt32(DataMain.nationality)) ,
                            "@CustomerGroup_ID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.custGroup) ,
                            "@existPhoneNumber" ,SqlDbType.Int ,DataMain.existPhoneNumber ,
                            "@Broker_ID" ,SqlDbType.Int ,DataMain.Broker_ID ,
                            "@checkedSchedule" ,SqlDbType.Int ,DataMain.checkedSchedule ,
                            "@Represent_Name" ,SqlDbType.NVarChar ,DataMain.Represent_Name.Replace("'" ,"").Trim() ,
                            "@Represent_Phone" ,SqlDbType.NVarChar ,DataMain.Represent_Phone.Replace("'" ,"").Trim() ,
                            "@Relationship" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Relationship) ,
                            "@IdentityCard" ,SqlDbType.NVarChar ,DataMain.IdentityCard.Replace("'" ,"").Trim() ,
                            "@isCheckIn" ,SqlDbType.Int ,DataMain.isCheckIn ,
                            "@isAgree" ,SqlDbType.Int ,DataMain.isAgree ,
                            "@apptemid" ,SqlDbType.Int ,apptemid ,
                            "@ACAppointmentID" ,SqlDbType.Int ,acappid ,
                            "@pageid" ,SqlDbType.NVarChar ,DataMain.pageid ,
                            "@psid" ,SqlDbType.NVarChar ,DataMain.psid ,
                            "@IDIssuenum" ,SqlDbType.NVarChar ,DataMain.IDIssuenum.Replace("'" ,"") ,
                            "@IDIssueplace" ,SqlDbType.NVarChar ,DataMain.IDIssueplace.Replace("'" ,"") ,
                            "@IDIssuedate" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.IDIssuedate) ,
                            "@Nation" ,SqlDbType.NVarChar ,DataMain.Nation.Replace("'" ,"") ,
                            "@HINumber" ,SqlDbType.NVarChar ,DataMain.HINumber.Replace("'" ,"") ,
                            "@HIExpireddate" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.HIExpireddate) ,
                            "@ReligionID" ,SqlDbType.Int ,DataMain.ReligionID ,
                            "@SexualOrienID" ,SqlDbType.Int ,DataMain.SexualOrienID ,
                            "@MaritalStatusID" ,SqlDbType.Int ,DataMain.MaritalStatusID ,
                            "@Membercode" ,SqlDbType.NVarChar ,DataMain.membercode != null ? DataMain.membercode.Replace("'" ,"").Trim().ToUpper() : ""
                        );
                    }
                    if (dt != null)
                    {
                        int ErrorCode = Convert.ToInt32(dt.Rows[0]["ErrorCode"].ToString());
                        int AppointID = Convert.ToInt32(dt.Rows[0]["AppointID"].ToString());
                        int CustomerID = Convert.ToInt32(dt.Rows[0]["CustomerID"].ToString());
                        if (ErrorCode == 1 && AppointID != 0)
                        {
                            DateTime DateFrom = Convert.ToDateTime(dt.Rows[0]["DateFrom"].ToString()).Date;
                            DateTime DateNow = Comon.Comon.GetDateTimeNow().Date;
                            if (DateFrom == DateNow)
                            {
                                await NotiLocal.Noti_Parti_Appointment_Branch(hubContext ,(DataMain.branch == "" ? 0 : Convert.ToInt32(DataMain.branch)) ,AppointID
                                    ,CustomerID ,"" ,DataMain.custName.Replace("'" ,"").Trim()
                                    ,0 ,DataMain.Doctor_ID ,"new");
                            }

                            if (DataMain.Doctor_ID != 0) await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,0 ,(DataMain.branch == "" ? 0 : Convert.ToInt32(DataMain.branch))
                             ,AppointID
                             ,DateFrom ,"insert" ,0);
                            if (apptemid != "0")
                            {
                                await NotiLocal.Noti_Parti_Appointment_Scheduler(hubContext ,1 ,(DataMain.branch == "" ? 0 : Convert.ToInt32(DataMain.branch))
                             ,Convert.ToInt32(apptemid)
                             ,DateFrom ,"delete" ,0);
                            }
                        }
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Update" ,CommandType.StoredProcedure ,
                        "@CustCode" ,SqlDbType.NVarChar ,syntax_customercode ,
                        "@DocCode" ,SqlDbType.NVarChar ,syntax_documentcode ,
                        "@CharSplit" ,SqlDbType.NVarChar ,syntax_ss ,
                        "@BranchCode" ,SqlDbType.NVarChar ,syntax_scmcn ,
                        "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                        "@Gclid" ,SqlDbType.NVarChar ,DataMain.Gclid.Replace("'" ,"").Trim() ,
                        "@Email1" ,SqlDbType.NVarChar ,DataMain.email.Replace("'" ,"").Trim() ,
                        "@Address" ,SqlDbType.NVarChar ,DataMain.address.Replace("'" ,"").Trim() ,
                        "@Phone1" ,SqlDbType.NVarChar ,DataMain.phone1.Replace("'" ,"").Trim() ,
                        "@Phone2" ,SqlDbType.NVarChar ,DataMain.phone2.Replace("'" ,"").Trim() ,
                        "@Name" ,SqlDbType.NVarChar ,DataMain.custName.Replace("'" ,"").Trim() ,
                        "@NameNoSign" ,SqlDbType.NVarChar ,DataMain.NameNoSign.Replace("'" ,"").Trim() ,
                        "@Gender_ID" ,SqlDbType.Int ,DataMain.Gender_ID ,
                        "@Type_Cat_ID" ,SqlDbType.Int ,(DataMain.source == "" ? 0 : Convert.ToInt32(DataMain.source)) ,
                        "@Language_ID" ,SqlDbType.Int ,DataMain.Language_ID ,
                        "@Avatar" ,SqlDbType.NVarChar ,img ,
                        "@Birthday" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.birthday) ,
                        "@BirthYear" ,SqlDbType.Int ,DataMain.BirthYear ,
                        "@instgramurl" ,SqlDbType.NVarChar ,DataMain.instgramurl.Replace("'" ,"").Trim() ,
                        "@facebookurl" ,SqlDbType.NVarChar ,DataMain.facebookurl.Replace("'" ,"").Trim() ,
                        "@viberurl" ,SqlDbType.NVarChar ,DataMain.viberurl.Replace("'" ,"").Trim() ,
                        "@zalomurl" ,SqlDbType.NVarChar ,DataMain.zalomurl.Replace("'" ,"").Trim() ,
                        "@phonecode" ,SqlDbType.NVarChar ,DataMain.phonecode != null ? DataMain.phonecode.ToString() : "vn" ,
                        "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                        "@NationalID" ,SqlDbType.Int ,(DataMain.nationality == "" ? 0 : Convert.ToInt32(DataMain.nationality)) ,
                        "@CityID" ,SqlDbType.Int ,DataMain.city ,
                        "@District" ,SqlDbType.Int ,DataMain.district ,
                        "@Commune" ,SqlDbType.Int ,DataMain.commune ,
                        "@Branch_ID" ,SqlDbType.Int ,(DataMain.branch == "" ? 0 : Convert.ToInt32(DataMain.branch)) ,
                        "@OldCustomer" ,SqlDbType.Int ,DataMain.OldCustomer ,
                        "@CustCodeOld" ,SqlDbType.NVarChar ,DataMain.CustCodeOld.Replace("'" ,"").Trim() ,
                        "@Career" ,SqlDbType.Int ,(DataMain.career == "" ? 0 : Convert.ToInt32(DataMain.career)) ,
                        "@CustomerGroup_ID" ,SqlDbType.Int ,(DataMain.custGroup == "" ? 0 : Convert.ToInt32(DataMain.custGroup)) ,
                        "@existPhoneNumber" ,SqlDbType.Int ,DataMain.existPhoneNumber ,
                        "@Broker_ID" ,SqlDbType.Int ,DataMain.Broker_ID ,
                        "@UpdatePhone" ,SqlDbType.Int ,Comon.CheckingSensative_Field.CheckSensetive_phone_customer(HttpContext) ,
                        "@Represent_Name" ,SqlDbType.NVarChar ,DataMain.Represent_Name.Replace("'" ,"").Trim() ,
                        "@Represent_Phone" ,SqlDbType.NVarChar ,DataMain.Represent_Phone.Replace("'" ,"").Trim() ,
                        "@Relationship" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Relationship) ,
                        "@IdentityCard" ,SqlDbType.NVarChar ,DataMain.IdentityCard.Replace("'" ,"").Trim() ,
                        "@isAgree" ,SqlDbType.Int ,DataMain.isAgree ,
                        "@IDIssuenum" ,SqlDbType.NVarChar ,DataMain.IDIssuenum.Replace("'" ,"") ,
                        "@IDIssueplace" ,SqlDbType.NVarChar ,DataMain.IDIssueplace.Replace("'" ,"") ,
                        "@IDIssuedate" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.IDIssuedate) ,
                        "@Nation" ,SqlDbType.NVarChar ,DataMain.Nation.Replace("'" ,"") ,
                        "@HINumber" ,SqlDbType.NVarChar ,DataMain.HINumber.Replace("'" ,"") ,
                        "@HIExpireddate" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.HIExpireddate) ,
                        "@ReligionID" ,SqlDbType.Int ,DataMain.ReligionID ,
                        "@SexualOrienID" ,SqlDbType.Int ,DataMain.SexualOrienID ,
                        "@MaritalStatusID" ,SqlDbType.Int ,DataMain.MaritalStatusID ,
                        "@Membercode" ,SqlDbType.NVarChar ,DataMain.membercode != null ? DataMain.membercode.Replace("'" ,"").Trim().ToUpper() : "");
                    }
                    if (dt != null)
                    {
                        return Content(Comon.DataJson.Datatable(dt));
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadCareerData()
        {
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[YYY_sp_Career_Load]" ,CommandType.StoredProcedure);
                dt.TableName = "Career";

                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
        }

        public async Task<IActionResult> OnPostLoadDataPrehistory()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Detail_Anamnesis_Load]" ,CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostExcutePrehistory(string Data ,string RegimenID ,string Name ,string CustomerID)
        {
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                await confunc.ExecuteDataTable("[YYY_sp_Customer_Anamnesis_Insert]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                            "@RegimenID" ,SqlDbType.Int ,RegimenID ,
                            "@Name" ,SqlDbType.NVarChar ,Name.Replace("'" ,"").Trim() ,
                            "@Note" ,SqlDbType.NVarChar ,"" ,
                            "@Data" ,SqlDbType.NVarChar ,Data.Replace("'" ,"").Trim() ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid
                );
            }
            return Content("1");
        }

        public async Task<IActionResult> OnPostCheckDupIdentityCard(string search, string customerid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_IdentityCard_Searching]" ,CommandType.StoredProcedure ,
                      "@SearchText" ,SqlDbType.NVarChar , !String.IsNullOrEmpty(search) ? search.ToString() : "",
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(customerid));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostLoadDuplicateData(string phone)
        {
            try
            {
                phone = (phone != null ? phone : "");
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_List_Duplicate]" ,CommandType.StoredProcedure ,
                      "@Phone" ,SqlDbType.NVarChar ,phone);
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
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostBrokerSearchCustomer(string search ,int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Broker_Searching_Customer]" ,CommandType.StoredProcedure
                       ,"@searchText" ,SqlDbType.NVarChar ,search.Replace("'" ,"").Trim()
                       ,"@CustomerID" ,SqlDbType.Int ,CustomerID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostCheckPhone(string Phone ,string CusID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Broker_CheckPhoneNumber]" ,CommandType.StoredProcedure
                        ,"@Phone" ,SqlDbType.NVarChar ,Phone != null ? Phone.Replace("'" ,"") : ""
                        ,"@Current" ,SqlDbType.Int, 0
                        ,"@CusID", SqlDbType.Int, Convert.ToInt32(CusID)
                        );
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExcuteDataBroker(int CustomerID ,string Name ,string Phone, string Code)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_Customer_Broker_Insert]" ,CommandType.StoredProcedure
                       ,"@CustomerID" ,SqlDbType.Int ,CustomerID
                       ,"@BrokerName" ,SqlDbType.NVarChar ,Name != null ? Name.Replace("'" ,"") : ""
                       ,"@BrokerPhone" ,SqlDbType.NVarChar ,Phone != null ? Phone.Replace("'" ,"") : ""
                       ,"@BrokerCode" , SqlDbType.NVarChar,Code != null ? Code.Replace("'" ,"").Trim().ToUpper() : ""
                       ,"@CreatedBy" ,SqlDbType.Int ,user.sys_userid); 
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class CustomerDetail
    {
        public string Note { get; set; }
        public string Gclid { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string Content { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string custName { get; set; }
        public string NameNoSign { get; set; }
        public string CustomerID { get; set; }
        public int Gender_ID { get; set; }
        public string source { get; set; }
        public int Language_ID { get; set; }
        public string birthday { get; set; }
        public string Avatar { get; set; }
        public string instgramurl { get; set; }
        public string facebookurl { get; set; }
        public string viberurl { get; set; }
        public string zalomurl { get; set; }
        public string phonecode { get; set; }
        public string nationality { get; set; }

        public string OldCustomer { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string commune { get; set; }
        public string branch { get; set; }
        public string career { get; set; }
        public string CustCodeOld { get; set; }

        public int BranchSchedule_ID { get; set; }
        public int Doctor_ID { get; set; }
        public int Consult_ID { get; set; }
        public string ServiceCare_ID { get; set; }
        public int Assistant_ID { get; set; }
        public int TypeSchedule { get; set; }
        public string Date_from { get; set; }
        public int numberDateTo { get; set; }
        public int checkPageAddSchedule { get; set; }
        public string ContentSchedule { get; set; }
        public int checkedSchedule { get; set; }
        public int existPhoneNumber { get; set; }
        public string Created { get; set; }
        public int Broker_ID { get; set; }
        public string custGroup { get; set; }
        public string Represent_Name { get; set; }
        public string Represent_Phone { get; set; }
        public int Relationship { get; set; }
        public string IdentityCard { get; set; }
        public int isCheckIn { get; set; }
        public int isAgree { get; set; }
        public string syntax_scmcn { get; set; }
        public string syntax_scmcn_doc { get; set; }
        public string syntax_sn { get; set; }
        public string syntax_ss { get; set; }
        public string pageid { get; set; }
        public string psid { get; set; }


        public string syntax_customercode { get; set; }
        public string syntax_documentcode { get; set; }
        public string IDIssuenum { get; set; }
        public string IDIssueplace { get; set; }
        public string IDIssuedate { get; set; }
        public int BirthYear { get; set; }
        public string HIExpireddate { get; set; }
        public string HINumber { get; set; }
        public string Nation { get; set; }

        public int ReligionID { get; set; }
        public int SexualOrienID { get; set; }
        public int MaritalStatusID { get; set; }

        public string membercode { get; set; }


    }
    public class CustomerProperties
    {
        public string ID { get; set; }
        public string Value { get; set; }
        public string IDPro { get; set; }

    }

}
