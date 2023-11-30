
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Models;
using MLunarCoffee.Models.API.APIClient;
using MLunarCoffee.Models.GlobalPer;
using MLunarCoffee.Models.Hook;
using MLunarCoffee.Models.ThirdParty.ICD;

namespace MLunarCoffee.Comon
{
    public class SMSZNSBrandName
    {
        public string sys_SMSThirdParty = "";
        public string sys_SMSUserName = "";
        public string sys_SMSPassword = "";
        public string sys_SMSBrandName = "";
        public string sys_SMSShareKey = "";
        public string sys_SMSLogin = "";
        public string sys_SMSSendSMS = "";
        public string sys_Zuser = "";
        public string sys_Zpassword = "";
        public string sys_Zid = "";
        public string sys_Zreqid = "";
        public string sys_ZThirdParty = "";
        public string sys_ZUrl = "";
        public string sys_ZUrlLog = "";
    }

    public class MailBrand
    {
        public string sys_MailDisplayName = "";
        public string sys_MailName = "";
        public string sys_MailPassword = "";
        public string sys_MailHost = "";
        public string sys_MailPort = "";
        public string sys_MailIs2ndAuthen = "";
    }

    public class GlobalUser
    {

        #region // General
        public int sys_userid { get; set; }
        public string sys_username { get; set; }
        public string sys_Role { get; set; }
        public string sys_RoleID { get; set; }
        public string sys_RoleName { get; set; }
        public string sys_BranchShortName { get; set; }
        public string sys_RoleServerID { get; set; }
        public string sys_BranchName { get; set; }
        public int sys_branchID { get; set; }
        public int sys_floorID { get; set; }
        public int sys_roomID { get; set; }
        public int sys_AllBranchID { get; set; }
        public string sys_AreaBranch { get; set; }
        public int sys_employeeid { get; set; }
        public string sys_SignData { get; set; }
        public int sys_TimeOut { get; set; }
        public string sys_employeename { get; set; }
        public string sys_branchCode;
        public string sys_CompanyName;
        public string sys_CompanyAddress;
        public string sys_SoftwareLogo;
        public string sys_SoftwareSmallLogo;
        public string sys_CompanyWebsite;
        public string sys_hotline;
        public string sys_userAvatar;
        public int sys_isMaxDiscountService { get; set; } //max discount service
        public int sys_maxPerDiscountService { get; set; }
        public decimal sys_maxAmountDiscountService { get; set; }
        public int sys_HasDashboard { get; set; }
        public DataTable sys_TokenTopic { get; set; }
        public string sys_SettingUser { get; set; }

        public string sys_TeleLevel { get; set; }
        public string sys_TeleGroup { get; set; }
        #endregion

        #region // Permission
        public DataTable sys_PermissionMenu;
        public List<PageUser> sys_Pageuser;
        public DataTable sys_PermissionControl;
        public DataTable sys_PermissionTabControl;
        #endregion

        #region // Call Center
        public string sys_CalLExtension;
        public string sys_CalLExtensionPassword;
        public int sys_UsingCallCenter;
        #endregion

        #region // IP
        public int sys_DetectIP_isPassIP;
        public int sys_DetectIP_IsCheck;
        #endregion

        #region // Option Extension
        public OptionExtension sys_Rule_OptionExtension;
        public string sys_LayoutCustomer;
        public string sys_LayoutChildCustomer;

        #endregion

        #region // Edit Role
        public int sys_ChooseDateCustomer { get; set; }
        public int sys_EditCustomerPassingDate { get; set; }
        public int sys_DisableActionButton { get; set; }
        public int sys_UsingConfirmService { get; set; }
        #endregion

        #region // SMS & Email
        public List<SMSZNSBrandName> sys_SMSBrandName { get; set; }
        public List<MailBrand> sys_MailBrand { get; set; }

        public string sys_TokenAPI = "";
        #endregion

        #region // Syntax code
        public string syntax_scmcn { get; set; }
        public string syntax_scmcn_doc { get; set; }
        public string syntax_sn { get; set; }
        public string syntax_ss { get; set; }
        public string syntax_customercode { get; set; }
        public string syntax_documentcode { get; set; }
        #endregion

        #region // Token Thirdparty
        public string synthird_accounttoken { get; set; }
        public string synthird_minvoicetoken { get; set; }
        #endregion

        #region // Token ICD
        public ICDToken synthird_ICDToken { get; set; }
        #endregion

        public async Task<AuthorResponse> User_Authorize(string user ,string pass ,string tokenfcm ,string iplan)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataSet ds = await connFunc.ExecuteDataSet("[MLU_sp_Permission_User_LogIn]" ,CommandType.StoredProcedure ,
                         "@username" ,SqlDbType.NVarChar ,user.Replace("'" ,"").Trim() ,
                         "@password" ,SqlDbType.NVarChar ,pass.Replace("'" ,"").Trim() ,
                         "@tokenfcm" ,SqlDbType.NVarChar ,(tokenfcm != null) ? tokenfcm.ToString() : "" ,
                         "@iplan" ,SqlDbType.NVarChar ,(iplan != null) ? iplan.ToString() : "");
                    AuthorResponse authorResponse = new AuthorResponse();
                    if (ds != null)
                    {

                        authorResponse.RESULT = ds.Tables[0].Rows[0]["RESULT"].ToString();
                        authorResponse.ErrTime = ds.Tables[0].Rows[0]["ErrTime"].ToString();
                        authorResponse.BlockTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["BlockTime"].ToString());
                        if (authorResponse.RESULT == "0") return authorResponse;//"Wrong Username, Password"
                        if (authorResponse.RESULT == "-1") return authorResponse;//"Wrong Username, Password"
                        else
                        {
                            DataTable dt = ds.Tables[1];
                            string Lock = dt.Rows[0]["Lock"].ToString();//Khoa user. 1: off, 0 on
                            string IsExpired = dt.Rows[0]["IsExpired"].ToString();
                            string IsLock = dt.Rows[0]["isLock"].ToString();

                            //Su dung khoa tu dong sau khoang thoi gian khong su dung. 1: off, 0 on
                            if (Lock == "0")
                            {
                                if (IsLock == "1" && IsExpired == "1")
                                {
                                    Global_FuncUser funuser = new Global_FuncUser();
                                    await funuser.UserLock(Convert.ToInt32(dt.Rows[0]["ID"].ToString()));
                                    authorResponse.RESULT = "-1";
                                    return authorResponse;  //"User locked"
                                }
                                sys_userid = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                                sys_username = dt.Rows[0]["Username"].ToString();
                                sys_RoleID = dt.Rows[0]["Group_ID"].ToString();
                                sys_RoleServerID = dt.Rows[0]["InheritanceServer"].ToString();
                                iplan = "[" + iplan + "]";
                                int minify = (Global.TrickIP != null && Global.TrickIP.Contains(iplan)) ? 1 : 0;
                                AuthorFCMResult athorFCMResult = new AuthorFCMResult()
                                {
                                    UserID = sys_userid.ToString() ,
                                    TokenFCM = dt.Rows[0]["TokenFcm"].ToString() ,
                                    Lang = dt.Rows[0]["Lang"].ToString() ,
                                    Minify = minify
                                };
                                authorResponse.FCMResult = athorFCMResult;
                                return authorResponse;
                            }
                            else
                            {
                                authorResponse.RESULT = "-1";
                                return authorResponse;  //"User locked"
                            }
                        }
                    }
                    else
                    {
                        authorResponse.RESULT = "0";
                        return authorResponse;//"Wrong Username, Password"
                    }
                }
            }
            catch (Exception ex)
            {
                AuthorResponse authorResponse = new AuthorResponse() { RESULT = "-1" };
                return authorResponse;  //"User locked"
            }
        }

        public async Task<string> User_InitializeInfo(int id)
        {
            string result = "";
            int ChooseDateCustomer = 0;
            int EditCustomerPassingDate = 0;
            int DisableActionButton = 0;

            Global_FuncUser funuser = new Global_FuncUser();

            var task_userinfo = funuser.UserInfo(sys_userid);
            var task_usercall = funuser.UserCall(sys_userid);
            var task_webhook = funuser.Webhook();
            var task_usermenu = funuser.UserMenu(Convert.ToInt32(sys_RoleID));
            var task_usertabcontrol = funuser.UserTabControl(Convert.ToInt32(sys_RoleID));
            var task_userdash = funuser.UserDashboard(Convert.ToInt32(sys_RoleID));
            var task_usereditrole = funuser.UserPerEditRole(sys_userid);
            var task_api = funuser.GetAuthorMulti(Global.sys_APIClient.Url ,Global.sys_APIClient.UserName ,Global.sys_APIClient.Password);
            var task_email = funuser.Email();
            var task_extension = funuser.GetOptionExtension();
            await Task.WhenAll(task_userinfo ,task_usercall
                ,task_usermenu ,task_usertabcontrol
                ,task_userdash ,task_usereditrole ,task_api ,task_email ,task_webhook
                ,task_extension);

            DataSet dsUserTopic = await task_userinfo;
            if (dsUserTopic.Tables.Count != 3) return "";
            DataTable dtUser = dsUserTopic.Tables[1];
            DataTable dtTopic = dsUserTopic.Tables[0];
            DataTable dtSyntaxCode = dsUserTopic.Tables[2];

            DataTable dtUserCall = await task_usercall;
            DataTable dt = await task_usermenu;
            DataTable dtTabControl = await task_usertabcontrol;
            DataTable dtHasDashboard = await task_userdash;
            DataTable dtEditRole = await task_usereditrole;
            JToken apiToken = await task_api;
            DataTable dtEmail = await task_email;
            DataTable dtWebhook = await task_webhook;
            DataTable dtExtension = await task_extension;

            string scmcn = "", scmcn_doc = "", sn = "", ss = "", customercode = "", documentcode = "";
            if (dtSyntaxCode != null && dtSyntaxCode.Rows.Count != 0)
            {
                scmcn = dtSyntaxCode.Rows[0]["mcn"] != null ? dtSyntaxCode.Rows[0]["mcn"].ToString() : "";
                scmcn_doc = dtSyntaxCode.Rows[0]["mcn_doc"] != null ? dtSyntaxCode.Rows[0]["mcn_doc"].ToString() : "";
                sn = dtSyntaxCode.Rows[0]["sn"] != null ? dtSyntaxCode.Rows[0]["sn"].ToString() : "";
                ss = dtSyntaxCode.Rows[0]["ss"] != null ? dtSyntaxCode.Rows[0]["ss"].ToString() : "";
                customercode = dtSyntaxCode.Rows[0]["customercode"] != null ? dtSyntaxCode.Rows[0]["customercode"].ToString() : "";
                documentcode = dtSyntaxCode.Rows[0]["documentcode"] != null ? dtSyntaxCode.Rows[0]["documentcode"].ToString() : "";
            }
            if (dtEditRole != null && dtEditRole.Rows.Count != 0)
            {
                var rowChoosenDate = dtEditRole.Rows[0];
                var rowEditPassingDate = dtEditRole.Rows[1];
                var rowDiasbleButton = dtEditRole.Rows[2];
                if (rowChoosenDate != null && rowChoosenDate[0].ToString() != "0")
                {
                    ChooseDateCustomer = rowChoosenDate != null && rowChoosenDate["PermissionName"].ToString() == "ChooseDateCustomer"
                       ? Convert.ToInt32(rowChoosenDate["Value"]) : 0;

                    EditCustomerPassingDate = rowEditPassingDate != null && rowEditPassingDate["PermissionName"].ToString() == "EditCustomerPassingDate"
                       ? Convert.ToInt32(rowEditPassingDate["Value"].ToString()) : 0;

                    DisableActionButton = rowDiasbleButton != null && rowDiasbleButton["PermissionName"].ToString() == "DisableActionButton"
                       ? Convert.ToInt32(rowDiasbleButton["Value"].ToString()) : 0;
                }
            }
            string APIToken = "";
            List<SMSZNSBrandName> multisms = new List<SMSZNSBrandName>();
            if (apiToken != null && apiToken.HasValues != false)
            {
                DataTable dtsms = JsonConvert.DeserializeObject<DataTable>(apiToken.SelectToken("MultiSMS").ToString());
                if (dtsms != null && dtsms.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtsms.Rows)
                    {
                        var r = new SMSZNSBrandName();
                        r.sys_SMSThirdParty = dataRow["ThirdParty"].ToString();
                        r.sys_SMSUserName = dataRow["UserName"].ToString();
                        r.sys_SMSPassword = dataRow["Password"].ToString();
                        r.sys_SMSBrandName = dataRow["BrandName"].ToString();
                        r.sys_SMSShareKey = dataRow["ShareKey"].ToString();
                        r.sys_SMSLogin = dataRow["UrlLogin"].ToString();
                        r.sys_SMSSendSMS = dataRow["UrlSend"].ToString();
                        multisms.Add(r);
                    }

                }
                APIToken = apiToken["Token"].ToString();


            }
            List<MailBrand> multimail = new List<MailBrand>();
            if (dtEmail != null && dtEmail.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dtEmail.Rows)
                {
                    var r = new MailBrand();
                    r.sys_MailDisplayName = dataRow["DisplayName"].ToString();
                    r.sys_MailName = dataRow["Mail"].ToString();
                    r.sys_MailPassword = dataRow["Password"].ToString();
                    r.sys_MailHost = dataRow["Host"].ToString();
                    r.sys_MailPort = dataRow["Port"].ToString();
                    r.sys_MailIs2ndAuthen = dataRow["Is2ndAuthen"].ToString();
                    multimail.Add(r);
                }
            }
            string third_minvoice = "", third_account = "";
            if (dtWebhook != null && dtWebhook.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dtWebhook.Rows)
                {
                    switch (dataRow["TypeHook"].ToString().ToLower())
                    {
                        case "invoice":
                            third_minvoice = dataRow["Token"].ToString();
                            break;
                        case "account":
                            third_account = dataRow["Token"].ToString();
                            break;
                    }
                }
            }

            ICDToken tokenICD = new ICDToken()
            {
                token = Encrypt.EncryptString(DateTime.Now.ToString("yyMMdd") ,Settings.SemiSecret)
            };

            if (dtUser.Rows.Count != 0 && dt != null)
            {
                var dt_pageuser = await funuser.User_GetMenu(Global.sys_PermissionTableMenu_Table_All ,Global.sys_PermissionTableMenu_Table ,dtUser.Rows[0]["InheritanceServer"].ToString() ,dtUser.Rows[0]["GroupID"].ToString());
                var li_pageuser = (from DataRow dr in dt_pageuser.AsEnumerable()
                                   select new PageUser()
                                   {
                                       MenuURL = dr["MenuURL"].ToString() ,
                                       CodeURL = dr["CodeURL"].ToString()
                                   }).ToList();

                GlobalUser globalUser = new GlobalUser()
                {
                    sys_userid = id ,
                    sys_CalLExtension = (dtUserCall != null && dtUserCall.Rows.Count == 1) ? dtUserCall.Rows[0]["Extension"].ToString() : "" ,
                    sys_CalLExtensionPassword = (dtUserCall != null && dtUserCall.Rows.Count == 1) ? dtUserCall.Rows[0]["Password"].ToString() : "" ,
                    sys_UsingCallCenter = (dtUserCall != null && dtUserCall.Rows.Count == 1) ? 1 : 0 ,
                    sys_branchID = Convert.ToInt32(dtUser.Rows[0]["Branch_ID"].ToString()) ,
                    sys_floorID = Convert.ToInt32(dtUser.Rows[0]["MLevelID"].ToString()) ,
                    sys_roomID = Convert.ToInt32(dtUser.Rows[0]["MRoomID"].ToString()) ,
                    sys_AllBranchID = Convert.ToInt32(dtUser.Rows[0]["AllBranchID"].ToString()) ,
                    sys_AreaBranch = dtUser.Rows[0]["AreaBranch"].ToString() ,
                    sys_TimeOut = Convert.ToInt32(dtUser.Rows[0]["TimeOut"].ToString()) ,
                    sys_branchCode = dtUser.Rows[0]["BranchCode"].ToString() ,
                    sys_CompanyName = dtUser.Rows[0]["sys_CompanyName"].ToString() ,
                    sys_CompanyAddress = dtUser.Rows[0]["sys_CompanyAddress"].ToString() ,
                    sys_SoftwareLogo = dtUser.Rows[0]["sys_SoftwareLogo"].ToString() ,
                    sys_SoftwareSmallLogo = dtUser.Rows[0]["sys_SoftwareSmallLogo"].ToString() ,
                    sys_CompanyWebsite = dtUser.Rows[0]["sys_CompanyWebsite"].ToString() ,
                    sys_hotline = dtUser.Rows[0]["Tel"].ToString() ,
                    sys_employeeid = Convert.ToInt32(dtUser.Rows[0]["Employee_ID"].ToString()) ,
                    sys_employeename = dtUser.Rows[0]["EmployeeName"].ToString().Replace("'" ,"") ,
                    sys_username = dtUser.Rows[0]["Username"].ToString() ,
                    sys_BranchShortName = dtUser.Rows[0]["ShortName"].ToString() ,
                    sys_RoleName = dtUser.Rows[0]["Role"].ToString() ,
                    sys_BranchName = dtUser.Rows[0]["BranchName"].ToString() ,
                    sys_Role = dtUser.Rows[0]["Role"].ToString() ,
                    sys_RoleID = dtUser.Rows[0]["Group_ID"].ToString() ,
                    sys_TeleLevel = dtUser.Rows[0]["TeleLevel"].ToString() ,
                    sys_TeleGroup = dtUser.Rows[0]["TeleGroup"].ToString() ,

                    sys_RoleServerID = dtUser.Rows[0]["InheritanceServer"].ToString() ,
                    sys_userAvatar = dtUser.Rows[0]["userAvatar"].ToString() ,
                    sys_PermissionMenu = dt_pageuser ,
                    sys_Pageuser = li_pageuser ,
                    sys_PermissionControl = funuser.User_GetControl(Global.sys_PermissionControl_Table ,dtUser.Rows[0]["InheritanceServer"].ToString()) ,
                    sys_PermissionTabControl = funuser.User_GetControl_TabControl(Global.sys_List_TabControl_Allowed ,dtTabControl) ,
                    sys_UsingConfirmService = funuser.User_ConfirmServiceCheck(Global.sys_List_TabControl_Allowed ,dtTabControl) ,
                    sys_DetectIP_isPassIP = Convert.ToInt32(dtUser.Rows[0]["isPassIP"].ToString()) ,
                    sys_DetectIP_IsCheck = 0 ,
                    sys_ChooseDateCustomer = ChooseDateCustomer ,
                    sys_EditCustomerPassingDate = EditCustomerPassingDate ,
                    sys_DisableActionButton = DisableActionButton ,

                    sys_isMaxDiscountService = Convert.ToInt32(dtUser.Rows[0]["isMaxDiscountService"].ToString()) ,
                    sys_maxPerDiscountService = Convert.ToInt32(dtUser.Rows[0]["MaxPerDiscountService"].ToString()) ,
                    sys_maxAmountDiscountService = Convert.ToDecimal(dtUser.Rows[0]["MaxAmountDiscountService"].ToString()) ,
                    sys_HasDashboard = Convert.ToInt32(dtHasDashboard.Rows[0]["IsHasDashboard"]) ,
                    sys_SettingUser = dtUser.Rows[0]["SettingUser"].ToString() ,
                    sys_LayoutCustomer = dtUser.Rows[0]["LayoutCustomer"].ToString() ,
                    sys_LayoutChildCustomer = dtUser.Rows[0]["LayoutChildCustomer"] != null ? dtUser.Rows[0]["LayoutChildCustomer"].ToString() : "" ,
                    sys_SMSBrandName = multisms ,
                    sys_MailBrand = multimail ,
                    sys_TokenAPI = APIToken ,
                    syntax_scmcn = scmcn ,
                    syntax_scmcn_doc = scmcn_doc ,
                    syntax_sn = sn ,
                    syntax_ss = ss ,
                    syntax_customercode = customercode ,
                    syntax_documentcode = documentcode ,
                    sys_TokenTopic = dtTopic ,
                    synthird_minvoicetoken = third_minvoice ,
                    synthird_accounttoken = third_account ,
                    sys_Rule_OptionExtension = funuser.User_OptionExtension(dtExtension) ,
                    synthird_ICDToken = tokenICD
                };
                result = JsonConvert.SerializeObject(globalUser);
            }
            return result;
        }

    }
    public class Global
    {

        #region // Client
        public static string SQLIP;
        public static string SQLNAME;
        public static string SQLUSER;
        public static string SQLPASSWORD;
        public static string ROOTCODE = "mlunarcofeeeefocranulmeefocranul";
        #endregion

        #region // webhook
        public static string sys_Callafter_Username;
        public static string sys_Callafter_Password;
        public static string sys_SMSCallbackSystem;
        public static HookInvoice sys_HookEInvoice;
        public static HookAccount sys_HookAccount;
        public static HookSMSCallBack sys_HookSMSCallback;
        #endregion

        #region // Option
        public static int sys_IsOutside;
        public static string sys_Session_Client; // Client Keyword
        public static string sys_SignInLINK;
        public static string sys_DBVersion;
        public static string sys_NameClient;
        public static string sys_URL_PortClient;
        public static string sys_URL_LINKSOURCE;
        public static string sys_DBTOCATCHSQL;
        public static int sys_DB_ID;
        public static string sys_DB_IP;
        public static string sys_DB_Name;
        public static string sys_DB_User;
        public static string sys_DB_Pass;
        public static int sys_DB_AllowPermission;
        public static string sys_ServerImageFolder;
        public static string sys_ServerImageUserName;
        public static string sys_ServerImagePassword;
        public static string sys_HTTPImageRoot;
        public static string sys_HTTPImageRoot_Media_Service;
        #endregion

        #region // REALTIME NOTI
        public static int sys_ScheduleNoti = 0;
        public static int sys_RoomNoti = 0;
        public static int sys_DivideDataNoti = 0;
        public static int sys_ExecuteDataNoti = 0;
        public static int sys_PaymentNoti = 0;
        #endregion

        #region // Other

        public static string sys_DefaultPic = "/default.png";
        public static string sys_ImageCheckNetwork;
        public static string sys_CodePayment;
        public static string sys_CodePaymentReturn;
        public static string sys_PassworDefault;
        public static string sys_SMSRemind { get; set; }
        public static int sys_MaxImportTicket { get; set; }
        public static int sys_SearchingInBranch; // SETTING Searching
                                                 // public static int sys_PreventCusByBranch; // access cus,access tic
        public static DataTable sys_UserPopup; // User PopUP
        public static DataTable sys_AuthosizeSettingFunction;
        public static string sys_ALLHTTPDocument;
        public static string sys_ALLFTP_USER;
        public static string sys_ALLFTP_FTPDocument;
        public static string sys_ALLFTP_PASSWORD;
        public static string sys_ALLFTP_Blacklist;
        public static int sys_Limit_LoadList;// Số Row load
        public static int sys_isShowUserGuide; // Show Guide For End User
        public static int sys_Card_Using_First;// Using Card First
        public static int sys_limit_MLunarCoffee = 0;  //Giới hạn MLunarCoffee
        public static int sys_limit_User = 0;
        public static int sys_Permission_Tele = 0;//Phân quyền tele không xem được kh tele khác
        public static int sys_isShowLogoBranch = 0; // Avartar Branch
        public static int sys_CustomerNotViewByBranch = 0; // Nhân viên có xem được khách hàng của chi nhánh khác
        public static string sys_DefaultColor;
        public static int sys_Service_Choose_PriceMax = 0;
        public static string sys_groupreport; // Phân quyền cho phép được xem nhóm báo cáo
        public static string sys_countriesPhone = ""; // Đầu số điện thoại trong tạo KH
        public static string sys_Clicktoshow; // Ẩn hiện hình ảnh từ app
        public static string sys_roundDiscountAmount = ""; // Làm tròn số khi giảm giá
        public static int sys_Allowmultiappinday = 0; // Cho phép nhiều lịch hẹn trong 1 ngày
        public static int sys_Allowappinday_past = 0; // Cho phép lịch hẹn trong quá khứ, và khi tạo sẽ tự động checkedin
        public static int sys_Showallsteptreatment = 0; // 0: Khi thêm mới điều trị, những bước điều trị trước sẽ không thể hiện
        public static int sys_AllowUploadVideo = 0; // Cho phép upload video
        public static int sys_LimitSizeUploadInDay = 0; // Giới hạn dung lượng(byte) upload hình ảnh và file trong ngày
        public static int sys_BirthdayYearOnly = 0; // Chỉ nhập năm sinh
        public static int Sys_SMSAllowNotInBranchName = 0; // Lấy cấu hình chặn từ khoá sms branchname theo client
        public static int sys_UsingSchedulerSeft = 0; // Người tạo mới được sửa hoặc xóa lịch hẹn
        public static int sys_CustCare_NeedToCallBack = 0; // Người tạo mới được sửa hoặc xóa lịch hẹn
        public static int sys_Allowsearchticket = 0; //Cho lọc mã ticket
        public static string fcm_server_key;
        public static string TrickIP;
        public static string mobile_secretkey;
        public static int sys_Resizeimageupload = 0;
        public static int sys_Relationship1v1 = 0;
        public static int sys_TreatManualamount = 0;
        public static int sys_TreatManualIndex = 0;
        public static int sys_CardManual = 0;
        public static int sys_PKCheckingrom = 0; // Cho trùng phòng giường hay k
        public static int sys_heartbeat = 0;
        public static int sys_ArisePresAfterExport = 0;
        public static int sys_ExportConsumableAllowStage = 0;
        public static int sys_DepositNonPriority = 0;
        public static int sys_Custimgblod = 0;// Blog size image khách hàng
        public static int sys_UsingbranchAddress = 0;// Sử dụng địa chỉ chi nhánh cho khách hàng
        public static int sys_Custimgblockshare = 0;// Block share iamge
        public static string sys_Custimgwatermark = "";// Watermark hình khách hàng
        #endregion

        #region // Face
        public static string sys_face_secretkey;
        public static string sys_face_appid;
        public static string sys_face_version;

        #endregion

        #region // SMS After Payment
        public static string sys_SMSAfterPaymentContent;
        public static int sys_isSMSAfterPayment = 0;
        #endregion

        #region // Noti app
        public static int sys_isNotiAfterPayment;
        public static int sys_isNotiNewAppointment;
        public static int sys_isNotiChangeAppointment;
        public static int sys_isNotiChangeStatusAppointment;
        #endregion

        #region // API
        public static APIClient sys_APIClient;
        public static DataTable sys_SMS_NotInBrandName; // SMS Keyword

        #endregion

        #region // Treatment Setting
        public static int sys_PercentTreatment;
        public static int sys_DentistCosmetic;
        public static int sys_Treatment_Plan;
        public static int sys_Patient_Record;
        public static int sys_isDetailTeeth;
        public static int sys_Noruletreatindex = 0;
        #endregion
        #region // Tab Setting
        public static int sys_AutoselectPro;
        
        #endregion
        #region// Notification On App Mobile

        public static string sys_App_Notification_Link;
        public static string sys_App_Notification_Autho;
        #endregion

        #region // ZNS
        public static string sys_Zuser;
        public static string sys_Zpassword;
        public static string sys_Zid;
        public static string sys_Zreqid;
        public static string sys_ZUrl;
        public static string sys_ZUrlLog;
        public static string sys_ZThirdParty;
        #endregion


        #region // Permission Absoulte
        public static DataTable sys_PermissionTableMenu_Table;
        public static List<PageClass> sys_Page;
        // Du lieu menu theo tung nhom
        public static DataTable sys_PermissionTableMenu_Table_All; // chua day du thong tin cua menu, duoc cho phep tu client

        public static DataTable sys_PermissionMenuMustHide_Table;
        public static List<PageMushide> sys_Pagemusthide;

        public static DataTable sys_ListGeneral; // Du lieu list danh muc general
        public static DataTable sys_List_TabControl_Allowed; // Cac control cho phep tu phia client
        public static DataTable sys_PermissionControl_Table;
        public static DataTable sys_PermissionControlMustHide_Table;
        public static DataTable sys_TabCustomer;
        public static DataTable sys_GuideForEndUser;
        public static DataTable sys_Report;
        public static DataTable sys_PermissionAllMenu;// Permission all menu for this client
        public static DataTable sys_AllGroupMailDataBase;// All Group In Main DataBase
        public static DataTable sys_Document; // Document
        public static int sys_PrintPaymentType;  // Print Form
        public static DataTable sys_RequireValidation;   //  Require validation
        #endregion

        #region // Call Center Argument
        public static string sys_APIbaseurl = "";
        public static string sys_WSSendpoint = "";
        public static string sys_Sipendpoint = "";
        public static string sys_DomainAPI = "";
        public static string sys_CallNumber = "";
        public static string sys_CallHost = "";
        public static string sys_CallPort = "";
        public static string sys_CallApi_Key = "";
        public static string sys_CallDomain = "";
        public static string sys_CallPBXIP = "";
        public static string sys_CallPBXPort = "";
        public static string sys_CallOutBound = "";
        public static string sys_CallOutBoundPort = "";
        public static string sys_CallUserName = "";
        public static string sys_CallPassword = "";
        public static string sys_LinkDownloadAudio = "";
        public static string sys_UrlRecord = "";
        public static int sys_CallCenterType = 1;
        public static int sys_CallLimit = 100;
        public static int sys_CallLogClient = 0;
        public static int sys_ClientIsUsingCall = 0;
        public static string sys_DtDetectCallCenter = "";
        public static string sys_CallOnlyHTTP = "";
        #endregion

        #region // Facebook
        //public static string sys_Face_AppID;
        //public static string sys_Face_Secret;
        //public static string sys_Face_User = "FaceUser";
        //public static string sys_Face_Page = "FacePage";
        //public static string sys_Face_CurrentPage = "FaceCurrentPage";
        //public static string sys_Face_NumberImport = "2000";
        //public static string sys_Face_LinkApi_Root = @"https://graph.facebook.com/v4.0/";
        //public static string sys_Face_LinkApi_Permission = sys_Face_LinkApi_Root + @"me/permissions";
        //public static string sys_Face_LinkApi_Account = sys_Face_LinkApi_Root + @"me/accounts?fields=picture{url},id,access_token,name,link";
        //public static string sys_Face_LinkApi_Conversation = sys_Face_LinkApi_Root + @"me?fields=conversations.limit({number}){id,unread_count,updated_time,snippet,senders,messages.limit(1){from}},blocked{id,name}";
        //public static string sys_Face_LinkApi_GetMessage_Connect = sys_Face_LinkApi_Root + @"me?fields=id,conversations.limit({number}){id,updated_time,senders,snippet}";
        //public static string sys_Face_LinkApi_Conversation_FilterWhenReceip = sys_Face_LinkApi_Root + @"me/conversations?fields=id,unread_count,updated_time,snippet,senders&user_id={profileid}";
        //public static string sys_Face_LinkApi_Conversation_Detail = sys_Face_LinkApi_Root + @"{conversation_id}?fields=messages.limit({number}){to,from,created_time,message,id,sticker,tags,attachments,updated_time}";
        //public static string sys_Face_LinkApi_Conversation_Detail_HavingPhone = sys_Face_LinkApi_Root + @"{conversation_id}?fields=messages{to,from,created_time,message,id}";
        //public static string sys_Face_LinkApi_Send_Comment = sys_Face_LinkApi_Root + "{comment_id}/comments";
        //public static string sys_Face_LinkApi_Execute_Comment = sys_Face_LinkApi_Root + "{comment_id}";
        //public static string sys_Face_LinkApi_Send_Text = sys_Face_LinkApi_Root + "me/messages";
        //public static string sys_Face_LinkApi_Comment_Detail = sys_Face_LinkApi_Root + @"{comment_id}?fields=message,attachment,created_time,from";
        //public static string sys_Face_LinkApi_Message_Detail = sys_Face_LinkApi_Root + @"{message_id}?fields=message,created_time,tags,to,attachments,sticker,from,id";
        //public static string sys_Face_LinkApi_Conversation_Detail_Search = sys_Face_LinkApi_Root + @"{conversation_id}?fields=messages{message,created_time,from}";
        //public static string sys_Face_LinkApi_Send_Files = sys_Face_LinkApi_Root + "me/messages";
        //public static string sys_Face_LinkApi_Upload_File = sys_Face_LinkApi_Root + "me/message_attachments";
        //public static string sys_Face_LinkApi_Load_Profile = sys_Face_LinkApi_Root + @"{profile_id}?fields=first_name,gender,name,profile_pic";
        //public static string sys_Face_LinkApi_Thread = sys_Face_LinkApi_Root + @"{thread_id}?fields=updated_time,participants";
        //// public static string sys_Face_LinkApi_Post= sys_Face_LinkApi_Root + @"me/published_posts?fields=admin_creator,message,updated_time,attachments{media,target,type},comments{id,message,attachment,from,comments{from,message,attachment,id}},id&limit={number}";
        //public static string sys_Face_LinkApi_Post = sys_Face_LinkApi_Root + @"{post_id}?fields=full_picture,message,created_time,from";
        //public static string sys_Face_LinkApi_Comment = sys_Face_LinkApi_Root + @"{comment_id}?fields=message,comments{created_time,message,attachment,from,is_hidden,like_count,permalink_url},attachment,created_time,permalink_url,from,is_hidden,like_count";
        //public static string sys_Face_LinkApi_Comment_Check = sys_Face_LinkApi_Root + @"{comment_id}";
        //public static string sys_Face_LinkApi_blocked = sys_Face_LinkApi_Root + @"{page_id}/blocked";
        //public static string sys_Face_LinkApi_DeclarePage = sys_Face_LinkApi_Root + @"{page_id}/subscribed_apps";
        #endregion

        #region// Tiền tính theo điều trị hoặc tiền phát sinh
        public static int sys_Debt_By_Treatment;  // Phuong phap tinh cong no
        public static int sys_PercentTreatmentShow;  // Co show dieu tri khong
        public static int sys_disable_enter_amount_payment;
        // THÊM ĐIỀU KIỆN CÓ LOAD LOẠI THÀNH VIÊN HAY KHÔNG.
        public static int sys_MoneyToPoint;

        #endregion

        #region// Folder system
        public static string sys_FLib;  // Dùng làm quản lý file trong chi nhánh
        public static string sys_FLibUrl;
        public static string sys_FLibUrlUser;
        public static string sys_FLibUrlPass;
        public static string sys_FBakup; // Dùng trong chức năng backup (chưa triển khai)
        #endregion

        #region//Sysdefault
        public static string sys_ClientSystempass;
        public static string sys_DefaultAvatar;
        #endregion

        public static async Task Client_GetInfoFromClient()
        {
            int autho = await Client_GetDBString(sys_DBTOCATCHSQL);
            if (autho == 1)
            {
                Global_Func client = new Global_Func();
                var task_permission = client.Permission(sys_DBTOCATCHSQL);
                var task_document = client.Document(sys_DBTOCATCHSQL);
                var task_userpopup = client.UserPopup(sys_DBTOCATCHSQL);
                var task_listsetting = client.ListSetting();
                var task_realtimenoti = client.RealTimeNoti(sys_DBTOCATCHSQL);
                var task_option = client.Option(sys_DBTOCATCHSQL);
                var task_folderSystem = client.FolderSystem();
                var task_requirevalidate = client.RequireValidate();
                var task_listblockcontrol = client.ListBlockControl(sys_DB_ID);
                var task_Zns = client.GetZNS(sys_DB_ID);
                var task_Smskey = client.KeyBanSms();
                var task_initializeinfo = client.InitializeInfo();
                var task_webhook = client.Getwebhook();
                await Task.WhenAll(task_permission ,task_document
                    ,task_userpopup ,task_listsetting
                    ,task_realtimenoti ,task_option
                    ,task_folderSystem
                    ,task_Zns ,task_Smskey
                    ,task_requirevalidate ,task_listblockcontrol
                    ,task_initializeinfo ,task_webhook);

                Client_Permission(await task_permission);
                Client_Document(await task_document);
                Client_UserPopup(await task_userpopup);
                Client_ListSetting(await task_listsetting);
                Client_RealTimeNoti(await task_realtimenoti);
                Client_Option(await task_option);
                Client_FolderSystem(await task_folderSystem);
                Client_RequireValidate(await task_requirevalidate);
                Client_ListBlockControl(await task_listblockcontrol);
                Client_ZNS(await task_Zns);
                Client_InitializeInfo(await task_initializeinfo);
                Client_Webhook(await task_webhook);
                Client_KeyBanSMS(await task_Smskey);
            }

        }

        //! When client is start

        public static async Task System_Start(IConfiguration _config)
        {
            try
            {

                sys_Session_Client = _config.GetValue<string>("CLIENT:NAME").ToString();
                // MINIFY = _config.GetValue<string>("IsProductEnviroment").ToString();
                SQLNAME = Encrypt.AESDecryptString(_config.GetValue<string>("CLIENT:SQLNAME").ToString() ,ROOTCODE);
                SQLUSER = Encrypt.AESDecryptString(_config.GetValue<string>("CLIENT:SQLUSER").ToString() ,ROOTCODE);
                SQLPASSWORD = Encrypt.AESDecryptString(_config.GetValue<string>("CLIENT:SQLPASSWORD").ToString() ,ROOTCODE);

                Firebase.FirebaseInfo.FBapiKey = _config.GetValue<string>("Firebase:apiKey").ToString();
                Firebase.FirebaseInfo.FBauthDomain = _config.GetValue<string>("Firebase:authDomain").ToString();
                Firebase.FirebaseInfo.FBprojectId = _config.GetValue<string>("Firebase:projectId").ToString();
                Firebase.FirebaseInfo.FBstorageBucket = _config.GetValue<string>("Firebase:storageBucket").ToString();
                Firebase.FirebaseInfo.FBmessagingSenderId = _config.GetValue<string>("Firebase:messagingSenderId").ToString();
                Firebase.FirebaseInfo.FBappId = _config.GetValue<string>("Firebase:appId").ToString();
                Firebase.FirebaseInfo.FBdatabaseURL = _config.GetValue<string>("Firebase:databaseURL").ToString();
                Firebase.FirebaseInfo.FBmeasurementId = _config.GetValue<string>("Firebase:measurementId").ToString();
                Global_Func client = new Global_Func();
                var task_startclient = client.StartClient();

                Client_StartClient(await task_startclient);
                await Client_GetInfoFromClient();

                var task_api = client.Api(sys_DBTOCATCHSQL);
                var task_callcenter = client.CallCenter(sys_DBTOCATCHSQL);
                await Task.WhenAll(task_startclient ,task_api
                    ,task_callcenter);


                Client_Api(await task_api);
                Client_CallCenter(await task_callcenter);
            }
            catch
            {

            }

        }

        #region // Set value
        private static void Client_Permission(DataSet ds)
        {
            try
            {
                if (ds != null)
                {
                    DataTable dt1 = ds.Tables[0];
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dt1.Rows[i]["MenuURL"] = dt1.Rows[i]["MenuURL"].ToString().Trim().ToLower();
                        dt1.Rows[i]["CodeURL"] = dt1.Rows[i]["CodeURL"].ToString().Trim().ToLower();
                    }
                    sys_PermissionTableMenu_Table = dt1;

                    sys_Page = (from DataRow dr in sys_PermissionTableMenu_Table.AsEnumerable()
                                select new PageClass()
                                {
                                    MenuURL = dr["MenuURL"].ToString() ,
                                    CodeURL = dr["CodeURL"].ToString()
                                }).ToList();

                    sys_AllGroupMailDataBase = ds.Tables[1];

                    DataTable dt2 = ds.Tables[2];
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        dt2.Rows[i]["MenuURL"] = dt2.Rows[i]["MenuURL"].ToString().Trim().ToLower();
                        dt2.Rows[i]["CodeURL"] = dt2.Rows[i]["CodeURL"].ToString().Trim().ToLower();
                    }
                    sys_PermissionTableMenu_Table_All = dt2;


                    sys_PermissionAllMenu = ds.Tables[3];
                    sys_PermissionControl_Table = ds.Tables[4];
                    sys_PermissionControlMustHide_Table = ds.Tables[5];
                    sys_TabCustomer = ds.Tables[6];
                    sys_GuideForEndUser = ds.Tables[7];
                    sys_Report = ds.Tables[8];

                    DataTable dt3 = ds.Tables[9];
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        dt3.Rows[i]["LinkUrlWhenClick"] = dt3.Rows[i]["LinkUrlWhenClick"].ToString().Trim().ToLower();
                    }
                    sys_PermissionMenuMustHide_Table = dt3;


                    sys_Pagemusthide = (from DataRow dr in sys_PermissionMenuMustHide_Table.AsEnumerable()
                                        select new PageMushide()
                                        {
                                            LinkUrlWhenClick = dr["LinkUrlWhenClick"].ToString() ,
                                            IPAllow = dr["IPAllow"].ToString()
                                        }).ToList();

                }
            }
            catch
            {

            }

        }
        private static void Client_Api(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];
                    sys_APIClient = new APIClient
                    {
                        UserName = dr["Name"].ToString() ,
                        Password = dr["Password"].ToString() ,
                        Url = dr["APIUrl"].ToString() ,
                        IsPortal = Convert.ToInt32(dr["IsPortal"].ToString()) ,
                        IsMobileApp = Convert.ToInt32(dr["IsMobileApp"].ToString())
                    };
                }
                else
                {
                    sys_APIClient = new APIClient
                    {
                        UserName = "" ,
                        Password = "" ,
                        Url = "" ,
                        IsPortal = 0 ,
                        IsMobileApp = 0
                    };
                }
            }
            catch
            {

            }
        }

        private static void Client_CallCenter(DataTable dt)
        {
            if (dt != null && dt.Rows.Count != 0)
            {
                sys_APIbaseurl = dt.Rows[0]["APIbaseurl"].ToString();
                sys_WSSendpoint = dt.Rows[0]["WSSendpoint"].ToString();
                sys_DomainAPI = dt.Rows[0]["DomainAPI"].ToString();
                sys_Sipendpoint = dt.Rows[0]["Sipendpoint"].ToString();
                sys_CallNumber = dt.Rows[0]["Number"].ToString();
                sys_CallHost = dt.Rows[0]["Host"].ToString();
                sys_CallPort = dt.Rows[0]["Port"].ToString();
                sys_CallApi_Key = dt.Rows[0]["Api_Key"].ToString();
                sys_CallDomain = dt.Rows[0]["Domain"].ToString();
                sys_CallPBXIP = dt.Rows[0]["PBXIP"].ToString();
                sys_CallPBXPort = dt.Rows[0]["PBXPort"].ToString();
                sys_CallOutBound = dt.Rows[0]["Outbound"].ToString();
                sys_CallOutBoundPort = dt.Rows[0]["OutBound_Port"].ToString();
                sys_CallCenterType = Convert.ToInt32(dt.Rows[0]["Type"].ToString());
                sys_CallLimit = Convert.ToInt32(dt.Rows[0]["Limit"].ToString());
                sys_CallUserName = dt.Rows[0]["UserName"].ToString();
                sys_CallPassword = dt.Rows[0]["Password"].ToString();
                sys_CallOnlyHTTP = dt.Rows[0]["OnlyHTTP"].ToString();
                sys_LinkDownloadAudio = dt.Rows[0]["LinkDownloadAudio"].ToString();
                sys_UrlRecord = dt.Rows[0]["URLRecord"].ToString();
                sys_CallLogClient = Convert.ToInt32(dt.Rows[0]["LogClient"].ToString());
                sys_ClientIsUsingCall = Convert.ToInt32(dt.Rows[0]["TypeCall"].ToString());
                sys_DtDetectCallCenter = JsonConvert.SerializeObject(dt);
            }
            else
            {
                sys_APIbaseurl = "";
                sys_WSSendpoint = "";
                sys_Sipendpoint = "";
                sys_DomainAPI = "";
                sys_CallNumber = "";
                sys_CallHost = "";
                sys_CallPort = "";
                sys_CallApi_Key = "";
                sys_CallDomain = "";
                sys_CallPBXIP = "";
                sys_CallPBXPort = "";
                sys_CallOutBound = "";
                sys_CallOutBoundPort = "";
                sys_CallUserName = "";
                sys_CallPassword = "";
                sys_LinkDownloadAudio = "";
                sys_UrlRecord = "";
                sys_CallCenterType = 1;
                sys_CallLimit = 100;
                sys_CallLogClient = 0;
                sys_ClientIsUsingCall = 0;
                sys_DtDetectCallCenter = "[]";
            }
        }
        private static void Client_Document(DataTable dt)
        {
            if (dt != null && dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["DocUrlHTTP"] = sys_ALLHTTPDocument + dt.Rows[i]["DocUrl"].ToString();
                }
                sys_Document = dt;
            }
        }
        private static void Client_UserPopup(DataSet ds)
        {
            DataTable dt1 = ds.Tables["UserPopup"];
            if (dt1 != null)
            {
                sys_UserPopup = dt1;
            }
            DataTable dt2 = ds.Tables["Authorize"];
            if (dt2 != null)
            {
                sys_AuthosizeSettingFunction = dt2;
            }
        }
        private static void Client_ListSetting(DataTable dt)
        {
            if (dt != null)
            {
                sys_ListGeneral = dt;
            }
            else
            {
                sys_ListGeneral = null;
            }
        }
        //private static void Client_Email(DataTable dt)
        //{
        //    if (dt != null && dt.Rows.Count != 0)
        //    {
        //        sys_Mail_Username = dt.Rows[0]["Username"].ToString();
        //        sys_Mail_Password = dt.Rows[0]["Password"].ToString();
        //        sys_Mail_MailFrom = dt.Rows[0]["MailFrom"].ToString();
        //        sys_Mail_Server = dt.Rows[0]["Server"].ToString();
        //        sys_Mail_Port = Convert.ToInt32(dt.Rows[0]["Port"].ToString());
        //    }
        //}
        private static void Client_InitializeInfo(DataTable dt)
        {
            sys_SMSRemind = "";
            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    switch (dRow["Type"].ToString())
                    {
                        case "sys_ImageCheckNetwork":
                            sys_ImageCheckNetwork = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_MaxImportTicket":
                            sys_MaxImportTicket = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_CodePayment":
                            sys_CodePayment = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_CodePaymentReturn":
                            sys_CodePaymentReturn = dRow["Value"].ToString().Trim();
                            break;
                        case "PasswordDefault":
                            sys_PassworDefault = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_PercentTreatment":
                            sys_PercentTreatment = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Noruletreatindex":
                            sys_Noruletreatindex = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_AutoselectPro":
                            sys_AutoselectPro = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Limit_LoadList":
                            sys_Limit_LoadList = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Permission_Tele":
                            sys_Permission_Tele = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Debt_By_Treatment":
                            sys_Debt_By_Treatment = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_PercentTreatmentShow":
                            sys_PercentTreatmentShow = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                        case "sys_MoneyToPoint":
                            sys_MoneyToPoint = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_ClientSystempass":
                            sys_ClientSystempass = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_Card_Using_First":
                            sys_Card_Using_First = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_disable_enter_amount_payment":
                            sys_disable_enter_amount_payment = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_CustomerNotViewByBranch":
                            sys_CustomerNotViewByBranch = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_DefaultColor":
                            sys_DefaultColor = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_Service_Choose_PriceMax":
                            sys_Service_Choose_PriceMax = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_groupreport":
                            sys_groupreport = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_countriesPhone":
                            sys_countriesPhone = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_Clicktoshow":
                            sys_Clicktoshow = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_roundDiscountAmount":
                            sys_roundDiscountAmount = dRow["Value"].ToString().Trim();
                            break;
                        case "sys_Allowmultiappinday":
                            sys_Allowmultiappinday = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Allowappinday_past":
                            sys_Allowappinday_past = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Showallsteptreatment":
                            sys_Showallsteptreatment = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_AllowUploadVideo":
                            sys_AllowUploadVideo = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_LimitSizeUploadInDay":
                            sys_LimitSizeUploadInDay = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Resizeimageupload":
                            sys_Resizeimageupload = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Relationship1v1":
                            sys_Relationship1v1 = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_TreatManualamount":
                            sys_TreatManualamount = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_TreatManualIndex":
                            sys_TreatManualIndex = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_CardManual":
                            sys_CardManual = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_heartbeat":
                            sys_heartbeat = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                        case "sys_ArisePresAfterExport":
                            sys_ArisePresAfterExport = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                        case "sys_ExportConsumableAllowStage":
                            sys_ExportConsumableAllowStage = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                        case "sys_DepositNonPriority":
                            sys_DepositNonPriority = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Custimgblod":
                            sys_Custimgblod = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Custimgblockshare":
                            sys_Custimgblockshare = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Custimgwatermark":
                            sys_Custimgwatermark = dRow["Value"].ToString().Trim();
                            break;

                        case "sys_UsingbranchAddress":
                            sys_UsingbranchAddress = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                        case "sys_PKCheckingrom":
                            sys_PKCheckingrom = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_BirthdayYearOnly":
                            sys_BirthdayYearOnly = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "Sys_SMSAllowNotInBranchName":
                            Sys_SMSAllowNotInBranchName = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_UsingSchedulerSeft":
                            sys_UsingSchedulerSeft = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_CustCare_NeedToCallBack":
                            sys_CustCare_NeedToCallBack = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;
                        case "sys_Allowsearchticket":
                            sys_Allowsearchticket = Convert.ToInt32(dRow["Value"].ToString().Trim());
                            break;

                    }
                }
            }
        }
        private static void Client_Webhook(DataTable dt)
        {

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    switch (dRow["TypeHook"].ToString().ToLower())
                    {
                        case "aftercall":
                            sys_Callafter_Username = dRow["UserName"].ToString().Trim();
                            sys_Callafter_Password = dRow["Password"].ToString().Trim();
                            break;
                        case "smscallbacksystem":
                            {
                                sys_SMSCallbackSystem = dRow["UrlLink"].ToString().Trim();
                            }
                            break;
                        case "invoice":
                            {
                                sys_HookEInvoice = new HookInvoice()
                                {
                                    Invoice_Brand = dRow["Brand"].ToString() ,
                                    Invoice_Username = dRow["Username"].ToString() ,
                                    Invoice_Password = dRow["Password"].ToString() ,
                                    Invoice_Type = dRow["Type"].ToString() ,
                                    Invoice_Url = dRow["UrlLink"].ToString() ,
                                    Invoice_Code = dRow["Code"].ToString()
                                };
                            }
                            break;
                        case "account":
                            {
                                sys_HookAccount = new HookAccount()
                                {
                                    Account_Brand = dRow["Brand"].ToString() ,
                                    Account_UserName = dRow["Username"].ToString() ,
                                    Account_Password = dRow["Password"].ToString() ,
                                    Account_Type = dRow["Type"].ToString() ,
                                    Account_Url = dRow["UrlLink"].ToString() ,
                                    Account_Api = dRow["appId"].ToString() ,
                                    Account_Accesscode = dRow["accessCode"].ToString()
                                };
                            }
                            break;
                        case "smscallback":
                            {
                                sys_HookSMSCallback = new HookSMSCallBack()
                                {
                                    SMS_Callback = dRow["UrlLink"].ToString()
                                };
                            }
                            break;
                    }
                }
            }
        }
        private static void Client_FolderSystem(DataTable dt)
        {
            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    if (dRow["IsManagement"].ToString() == "1")
                    {
                        sys_FLib = dRow["Name"].ToString().Trim();
                        sys_FLibUrl = dRow["Url"].ToString().Trim();
                        sys_FLibUrlUser = dRow["UrlUser"].ToString().Trim();
                        sys_FLibUrlPass = dRow["UrlPassword"].ToString().Trim();
                    }
                }
            }
        }
        private static void Client_KeyBanSMS(DataSet ds)
        {
            if (ds != null)
            {
                sys_SMS_NotInBrandName = Sys_SMSAllowNotInBranchName == 1
                    ? ds.Tables["Keyword"].Copy()
                    : ds.Tables["KeywordRoot"].Copy();
            }
            else
            {
                sys_SMS_NotInBrandName = null;
            }
        }
        private static void Client_RealTimeNoti(DataTable dt)
        {
            if (dt != null && dt.Rows.Count != 0)
            {
                sys_ScheduleNoti = Convert.ToInt32(dt.Rows[0]["ScheduleNoti"].ToString());
                sys_RoomNoti = Convert.ToInt32(dt.Rows[0]["RoomNoti"].ToString());
                sys_DivideDataNoti = Convert.ToInt32(dt.Rows[0]["DivideDataNoti"].ToString());
                sys_ExecuteDataNoti = Convert.ToInt32(dt.Rows[0]["ExecuteDataNoti"].ToString());
                sys_PaymentNoti = Convert.ToInt32(dt.Rows[0]["PaymentNoti"].ToString());
            }
        }
        private static void Client_Option(DataSet ds)
        {
            DataTable dt = ds.Tables["DB-Option"];
            if (dt != null && dt.Rows.Count != 0)
            {
                sys_DentistCosmetic = Convert.ToInt32(dt.Rows[0]["Type"].ToString());

                sys_Treatment_Plan = Convert.ToInt32(dt.Rows[0]["Is_Treatment_Plan"].ToString());
                sys_Patient_Record = Convert.ToInt32(dt.Rows[0]["Is_Patient_Record"].ToString());
                sys_isShowUserGuide = Convert.ToInt32(dt.Rows[0]["isShowUserGuide"].ToString());
                sys_ServerImageFolder = dt.Rows[0]["ImageFolder"].ToString();
                sys_ServerImageUserName = dt.Rows[0]["ImageUserName"].ToString();
                sys_ServerImagePassword = dt.Rows[0]["ImagePassword"].ToString();
                sys_SearchingInBranch = Convert.ToInt32(dt.Rows[0]["SearchCusInBranch"].ToString());
                // sys_PreventCusByBranch = Convert.ToInt32(dt.Rows[0]["PreventCusByBranch"].ToString());
                sys_isSMSAfterPayment = Convert.ToInt32(dt.Rows[0]["isSMSAfterPayment"].ToString());
                sys_isNotiAfterPayment = Convert.ToInt32(dt.Rows[0]["isNotiAfterPayment"].ToString());
                sys_isNotiNewAppointment = Convert.ToInt32(dt.Rows[0]["isNotiNewAppointment"].ToString());
                sys_isNotiChangeAppointment = Convert.ToInt32(dt.Rows[0]["isNotiChangeAppointment"].ToString());
                sys_isNotiChangeStatusAppointment = Convert.ToInt32(dt.Rows[0]["isNotiChangeStatusAppointment"].ToString());
                sys_isShowLogoBranch = Convert.ToInt32(dt.Rows[0]["isShowLogoBranch"].ToString());
                sys_PrintPaymentType = Convert.ToInt32(dt.Rows[0]["PaymentForm"].ToString());
                sys_HTTPImageRoot = dt.Rows[0]["ImageURL"].ToString();
                sys_HTTPImageRoot_Media_Service = dt.Rows[0]["MediaFolder"].ToString() + "/";
                //sys_Face_AppID = dt.Rows[0]["Face_appid"].ToString();
                //sys_Face_Secret = dt.Rows[0]["Face_secret"].ToString();
                sys_limit_MLunarCoffee = Convert.ToInt32(dt.Rows[0]["limit_Fanpage"].ToString());
                sys_limit_User = Convert.ToInt32(dt.Rows[0]["limit_User"].ToString());
                sys_isDetailTeeth = Convert.ToInt32(dt.Rows[0]["isDetailTeeth"].ToString());

            }

            DataTable dt2 = ds.Tables["Option"];
            if (dt2 != null && dt2.Rows.Count != 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    switch (dt2.Rows[i]["Name"].ToString())
                    {
                        case "HTTPDocument":
                            sys_ALLHTTPDocument = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "FTPUser":
                            sys_ALLFTP_USER = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "FTPDocument":
                            sys_ALLFTP_FTPDocument = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "FTPPass":
                            sys_ALLFTP_PASSWORD = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "FTPBlacklist":
                            sys_ALLFTP_Blacklist = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "SMSAfterPaymentContent":
                            sys_SMSAfterPaymentContent = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "fcm_server_key":
                            fcm_server_key = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "TrickIP":
                            TrickIP = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "mobile_secretkey":
                            mobile_secretkey = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "face_secretkey":
                            sys_face_secretkey = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "face_appid":
                            sys_face_appid = dt2.Rows[i]["Value"].ToString();
                            break;
                        case "face_version":
                            sys_face_version = dt2.Rows[i]["Value"].ToString();
                            break;
                        default: break;
                    }
                }
            }
        }

        private static void Client_RequireValidate(DataTable dt)
        {
            if (dt != null && dt.Rows.Count != 0)
            {
                sys_RequireValidation = dt;
            }
            else
            {
                sys_RequireValidation = null;
            }
        }
        private static void Client_ListBlockControl(DataTable dt)
        {
            if (dt != null)
            {
                sys_List_TabControl_Allowed = dt;
            }
            else
            {
                sys_List_TabControl_Allowed = null;
            }
        }
        private static void Client_ZNS(DataTable dt)
        {
            if (dt != null && dt.Rows.Count == 1)
            {
                sys_Zuser = dt.Rows[0]["Zuser"].ToString();
                sys_Zpassword = dt.Rows[0]["Zpassword"].ToString();
                sys_Zid = dt.Rows[0]["Zid"].ToString();
                sys_Zreqid = dt.Rows[0]["Zreqid"].ToString();
                sys_ZThirdParty = dt.Rows[0]["ZThirdParty"].ToString();
                sys_ZUrl = dt.Rows[0]["ZUrl"].ToString();
                sys_ZUrlLog = dt.Rows[0]["ZUrlLog"].ToString();


            }
            else
            {
                sys_Zuser = "";
                sys_Zpassword = "";
                sys_Zid = "";
                sys_Zreqid = "";
                sys_ZThirdParty = "";
                sys_ZUrl = "";
                sys_ZUrlLog = "";
            }
        }


        private static void Client_StartClient(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count == 1)
                {
                    sys_SignInLINK = dt.Rows[0]["LinkSignIn"].ToString();
                    sys_DBTOCATCHSQL = dt.Rows[0]["DBName"].ToString();
                    sys_DB_ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    sys_DBVersion = dt.Rows[0]["Version"].ToString();
                    sys_NameClient = Comon.ConvertToUnsign(dt.Rows[0]["ClientName"].ToString());
                    sys_URL_PortClient = dt.Rows[0]["UrlPort"].ToString();
                    sys_URL_LINKSOURCE = dt.Rows[0]["UrlSource"].ToString();
                    sys_DB_AllowPermission = Convert.ToInt32(dt.Rows[0]["AllowPermission"].ToString());
                    sys_IsOutside = Convert.ToInt32(dt.Rows[0]["IsOutside"].ToString());
                }
                else
                {
                    sys_SignInLINK = "";
                    sys_DBTOCATCHSQL = "";
                    sys_DBVersion = "";
                    sys_NameClient = "";
                    sys_URL_PortClient = "";
                    sys_URL_LINKSOURCE = "";
                    sys_DB_ID = 0;
                    sys_DB_AllowPermission = 0;
                    sys_IsOutside = 0;

                }
            }
            catch
            {
                sys_SignInLINK = "";
                sys_DBTOCATCHSQL = "";
                sys_DB_ID = 0;
                sys_DBVersion = "";
                sys_NameClient = "";
                sys_URL_PortClient = "";
                sys_URL_LINKSOURCE = "";
                sys_DB_AllowPermission = 0;
            }
        }
        private static async Task<int> Client_GetDBString(string db)
        {
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("MLU_sp_DB_DetectNameString" ,CommandType.StoredProcedure ,"@db" ,SqlDbType.NVarChar ,db);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        sys_DB_IP = dt.Rows[0]["IP"].ToString();
                        sys_DB_Name = dt.Rows[0]["DBName"].ToString();
                        sys_DB_User = dt.Rows[0]["DBUser"].ToString();
                        sys_DB_Pass = dt.Rows[0]["DBPass"].ToString();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch
            {
                return 0;
            }
        }

        #endregion

    }

}
