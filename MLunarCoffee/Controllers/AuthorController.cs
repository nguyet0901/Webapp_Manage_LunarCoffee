using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models;
using MLunarCoffee.Models.Model.IPAddress;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private string generatedToken = null;
        public AuthorController(IConfiguration config ,ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;

        }
        #region // Login
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {
            try
            {
                AuthorResultModel authorResultModel = new AuthorResultModel();
                if (Global.sys_SignInLINK != "")
                {
                    string password = (user.PasswordEnCrypt == "" || user.PasswordEnCrypt == null) ? user.Password : Encrypt.DecryptString(user.PasswordEnCrypt ,Settings.Secret);
                    string tokenfcm = user.TokenFCM;
                    if (user.UserName != null && password != null)
                    {
                        GlobalUser globalUser = new GlobalUser();
                        string iplan = Trygetiplan();
                        AuthorResponse authorResponse = await globalUser.User_Authorize(user.UserName.Replace("'" ,"").Trim() ,password.Replace("'" ,"").Trim() ,tokenfcm ,iplan);
                        authorResultModel.ErrTime = authorResponse.ErrTime;
                        authorResultModel.BlockTime = authorResponse.BlockTime;
                        if (authorResponse.RESULT == "0")
                        {
                            authorResultModel.RESULT = "incorrect";
                            return Content(JsonConvert.SerializeObject(authorResultModel));
                        }
                        else if (authorResponse.RESULT == "-1")
                        {
                            authorResultModel.RESULT = "locked";
                            return Content(JsonConvert.SerializeObject(authorResultModel));
                        }
                        else
                        {
                            AuthorFCMResult result = authorResponse.FCMResult;
                            string user_id = result.UserID;
                            string currentfcmToken = result.TokenFCM;
                            string user_lang = result.Lang;
                            int user_minify = result.Minify;

                            string global = await globalUser.User_InitializeInfo(Convert.ToInt32(user_id));
                            if (global != null && global != "")
                            {
                                string global_user = global;
                                GlobalUser userInfo = JsonConvert.DeserializeObject<GlobalUser>(global_user);
                                //RegisterTopics(userInfo.sys_TokenTopic ,Global.fcm_server_key ,tokenfcm ,Global.APICODE);
                                RegisterTopics(userInfo.sys_TokenTopic ,Global.fcm_server_key ,tokenfcm ,"");
                                int isPassIP = userInfo.sys_DetectIP_isPassIP;
                                if (isPassIP == 0)
                                {
                                    string userip = user.IP;
                                    if (userip == "")
                                    {
                                        IPaddress iPAddress = GetIP_Executed();
                                        if (iPAddress != null) userip = iPAddress.ip_encry; ;
                                    }
                                    string resultCheck = await CheckIP(user_id ,userip);
                                    if (resultCheck == "0")
                                    {
                                        authorResultModel.RESULT = "block";
                                        return Content(JsonConvert.SerializeObject(authorResultModel));
                                    }

                                }
                                Session.SetSession(HttpContext.Session ,"CurrentUserInfo" ,global_user);
                                UserDTO validUser = new UserDTO()
                                {
                                    UserName = user.UserName ,
                                    Password = password ,
                                    Role = "webapp"
                                };
                                generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString()
                                    ,_config["Jwt:Issuer"].ToString() ,validUser);
                                if (generatedToken != null)
                                {
                                    Session.SetSession(HttpContext.Session ,"UserID" ,userInfo.sys_userid.ToString());
                                    Session.SetSession(HttpContext.Session ,"Lang" ,!String.IsNullOrEmpty(user_lang) ? user_lang.ToString() : "");
                                    Session.SetSession(HttpContext.Session ,"Token" ,!String.IsNullOrEmpty(generatedToken) ? generatedToken : "");
                                    Session.SetSession(HttpContext.Session ,"Minify" ,user_minify.ToString());
                                    Session.SetSession(HttpContext.Session ,"IPLan" ,!String.IsNullOrEmpty(iplan) ? iplan.ToString() : "");
                                    Session.SetSession(HttpContext.Session ,"LoginFrom" ,!String.IsNullOrEmpty(user.From) ? user.From : "");
                                    Session.SetSession(HttpContext.Session ,"Idletimeout" ,DateTime.Now.Ticks.ToString());
                                    Session.SetSession(HttpContext.Session ,"Idleminute" ,userInfo.sys_TimeOut.ToString());
                                    RegisterLanguage(user_lang.ToString());
                                    string passencrypt = user.PasswordEnCrypt == "" ? Encrypt.EncryptString(password ,Settings.Secret) : user.PasswordEnCrypt;
                                    authorResultModel.ID = userInfo.sys_userid;
                                    authorResultModel.Session = generatedToken;
                                    authorResultModel.UserName = user.UserName;
                                    authorResultModel.PasswordEnCrypt = passencrypt;
                                    authorResultModel.OldfcmToken = currentfcmToken;
                                    authorResultModel.NewfcmToken = tokenfcm;
                                    authorResultModel.SettingUser = userInfo.sys_SettingUser;
                                    authorResultModel.TokenTopic = JsonConvert.SerializeObject(userInfo.sys_TokenTopic);
                                    authorResultModel.Lang = user_lang.ToString();
                                    authorResultModel.Minify = user_minify;
                                    return Content(JsonConvert.SerializeObject(authorResultModel));
                                }
                                else
                                {
                                    authorResultModel.RESULT = "error";
                                    return Content(JsonConvert.SerializeObject(authorResultModel));
                                }
                            }
                            else
                            {
                                authorResultModel.RESULT = "error";
                                return Content(JsonConvert.SerializeObject(authorResultModel));
                            }
                        }
                    }
                    else
                    {
                        authorResultModel.RESULT = "error";
                        return Content(JsonConvert.SerializeObject(authorResultModel));
                    }
                }
                else
                {
                    authorResultModel.RESULT = "error";
                    return Content(JsonConvert.SerializeObject(authorResultModel));
                }
            }
            catch (Exception ex)
            {
                AuthorResultModel authorResultModel = new AuthorResultModel()
                {
                    RESULT = "error"
                };

                return Content(JsonConvert.SerializeObject(authorResultModel));
            }
        }



        #endregion

        #region // Language
        [Authorize]
        [HttpPost("ChangeLan")]
        public async Task<IActionResult> ChangeLan(UserLan userLan)
        {
            try
            {

                RegisterLanguage(userLan.lan.ToString());
                var userid = Session.GetSession(HttpContext.Session ,"UserID");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("MLU_sp_LangUserUpdate" ,CommandType.StoredProcedure
                         ,"@UserID" ,SqlDbType.Int ,userid
                         ,"@Lang" ,SqlDbType.NVarChar ,userLan.lan);
                }
                Session.SetSession(HttpContext.Session ,"Lang" ,userLan.lan);
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }

        }
        private void RegisterLanguage(string lan)
        {
            try
            {
                if (lan == "en")
                {

                    Response.Cookies.Append(
                       CookieRequestCultureProvider.DefaultCookieName ,
                       CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US")) ,
                       new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
                }
                else
                {
                    Response.Cookies.Append(
                       CookieRequestCultureProvider.DefaultCookieName ,
                       CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi-VN")) ,
                       new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
                }

                var currentLanguage = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region // Topic
        private async void RegisterTopics(DataTable dt ,string key ,string token ,string clientcode)
        {
            try
            {
                await RegisterTopic("" ,key ,token ,clientcode);
                if (dt != null && key != "" && token != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ISREGIS"].ToString() == "1")
                            await RegisterTopic(dt.Rows[i]["Topic"].ToString() ,key ,token ,clientcode);
                        else await UnRegisterTopic(dt.Rows[i]["Topic"].ToString() ,key ,token ,clientcode);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private async Task RegisterTopic(string topic ,string key ,string token ,string clientcode)
        {
            string baseaddress = "https://iid.googleapis.com";
            string uri = "/iid/v1/" + token + "/rel/topics/" + clientcode + topic;
            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(baseaddress) })
            {
                clientautho.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key" ,"=" + key);
                var result = await clientautho.PostAsync(uri ,null);
            }
        }
        private async Task UnRegisterTopic(string topic ,string key ,string token ,string clientcode)
        {
            string baseaddress = "https://iid.googleapis.com";
            string uri = "/iid/v1/" + token + "/rel/topics/" + clientcode + topic;
            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(baseaddress) })
            {
                clientautho.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key" ,"=" + key);
                var result = await clientautho.DeleteAsync(uri);
            }
        }

        #endregion

        #region // IP and Base DATA
        private static async Task<string> CheckIP(string userid ,string encryip)
        {
            try
            {
                if (encryip == "") return "0";
                string ip = Encrypt.DecryptString(encryip ,Settings.Secret);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Detect_IP]" ,CommandType.StoredProcedure ,
                        "@userID" ,SqlDbType.Int ,userid ,
                        "@ip" ,SqlDbType.NVarChar ,ip.Trim()
                    );
                }
                int resulf = Convert.ToInt32(dt.Rows[0][0].ToString());
                return resulf.ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }
        private string Trygetiplan()
        {
            try
            {
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                return ip;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        [AllowAnonymous]
        [Route("GetIP")]
        [HttpPost]
        public IActionResult GetIP()
        {
            try
            {
                IPaddress iPAddress = GetIP_Executed();
                if (iPAddress != null) return Content(JsonConvert.SerializeObject(iPAddress));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }

        }
        private IPaddress GetIP_Executed()
        {
            try
            {
                IPaddress iPAddress = new IPaddress();
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                iPAddress.ip = ip;
                iPAddress.ip_encry = Encrypt.EncryptString(ip ,Settings.Secret);
                return iPAddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Authorize]
        [Route("basedata")]
        [HttpPost]
        public IActionResult BaseData()
        {
            var token = Session.GetSession(HttpContext.Session ,"Token");
            if (token == null)
            {
                return Content("0");
            }
            if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                _config["Jwt:Issuer"].ToString() ,token))
            {
                BaseData baseData = new BaseData(HttpContext);
                return Content(JsonConvert.SerializeObject(baseData));
            }
            return Content("0");
        }

        [Authorize]
        [Route("loginfrom")]
        [HttpPost]
        public IActionResult loginfrom()
        {
            var token = Session.GetSession(HttpContext.Session ,"Token");
            if (token == null)
            {
                return Content("0");
            }
            if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString() ,
                _config["Jwt:Issuer"].ToString() ,token))
            {
                string _LoginFrom = Session.GetSession(HttpContext.Session ,"LoginFrom");
                Session.SetSession(HttpContext.Session ,"LoginFrom" ,"");
                return Content(_LoginFrom);
            }
            return Content("0");
        }

        #endregion

        [AllowAnonymous]
        [HttpPost("Clear")]
        public async Task<IActionResult> Clear(UserLogout userLogout)
        {
            try
            {
                if (userLogout.Islogout != null && userLogout.Islogout == "1")
                {

                }
                else
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        string sys_IPLan = Session.GetSession(HttpContext.Session ,"IPLan");
                        GlobalUser user = Session.GetUserSession(HttpContext);
                        await confunc.ExecuteDataTable("MLU_sp_InsertLogClear" ,CommandType.StoredProcedure
                             ,"@BranchID" ,SqlDbType.Int ,user.sys_branchID
                             ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LanIP" ,SqlDbType.Int ,sys_IPLan);
                    }
                }


                Session.SetSession(HttpContext.Session ,"Token" ,"");
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }



    }

    public class BaseData
    {
        #region // VAR
        public string PermissionTable_Menu = "";
        public string _circleAvatar { get; set; }
        public string _LayoutCustomer { get; set; }
        public string _LayoutChildCustomer { get; set; }
        public string _userNameCurrent { get; set; }
        public string _branchNameCurrent { get; set; }
        public int _branchIDCurrent { get; set; }
        public string _roleIDCurrent { get; set; }
        public string _roleGroupCurrent { get; set; }
        public string _userPopupMenu { get; set; }
        public string sys_CustomerTab { get; set; }
        public string sys_PermissionControl { get; set; }
        public string sys_PermisstionControlMustHide { get; set; }
        public string sys_HTTPImageRoot { get; set; }
        public string _versionofWebApplication { get; set; }
        public string _usingCallCenter { get; set; }
        public string _nameClient { get; set; }
        public string _calloutbound { get; set; }
        public string _calllogintimeexpired { get; set; }
        public string _callport { get; set; }
        public string _calldomain { get; set; }
        public string _callextension { get; set; }
        public string _callpassword { get; set; }
        public int _isCheckIPUser { get; set; }

        public string _areaBranch { get; set; }
        public int _isAllBranch { get; set; }
        public string PermissionTable_TabControl { get; set; }
        public int _dencos_Main { get; set; }
        public int _employeeID_Main { get; set; }
        public string _employeeName_Main { get; set; }
        public int _userID_Main { get; set; }

        public string _checkingimageNetwork { get; set; }
        public int _checkingCallCenter { get; set; }

        public string _LinkClickToCall { get; set; }
        public string _sys_DomainAPI { get; set; }
        public string _sys_CallApi_Key { get; set; }

        public string _URL_PortClient { get; set; }

        public List<string> _SMSBrandName { get; set; }
        public List<string> _MailBrand { get; set; }

        public string _SoftwareLogo { get; set; }
        public string _SoftwareSmallLogo { get; set; }
        public string _CompanyWebsite { get; set; }
        public string _CompanyName { get; set; }


        public int _isHasDashboard { get; set; }

        public string _RequireValidation { get; set; }

        public string _setting_AppointmentDoctor { get; set; }
        public string _dataFunctionAuthorise { get; set; }
        public string _dtConnectCall { get; set; }

        public string _Session_Branch { get; set; }
        public string _Session_Teeth { get; set; }
        public string _Session_Service { get; set; }
        public string _Session_ServiceCare { get; set; }
        public string _Session_Employee { get; set; }
        public string _Session_User { get; set; }
        public string _Session_City { get; set; }
        public string _Session_District { get; set; }
        public string _Session_National { get; set; }
        public string _Session_Commune { get; set; }


        public string _Session_Preload_CustomerLayout { get; set; }
        public string _Session_Data { get; set; }

        public int _sys_heartbeat { get; set; }
        #endregion

        #region // syntax
        public string syntax_scmcn { get; set; }
        public string syntax_scmcn_doc { get; set; }
        public string syntax_sn { get; set; }
        public string syntax_ss { get; set; }
        public string syntax_customercode { get; set; }
        public string syntax_documentcode { get; set; }
        public string sys_TeleLevel { get; set; }
        public string sys_TeleGroup { get; set; }

        #endregion

        #region // Third Party


        #region // Invoice
        public string sys_EinvoiceType { get; set; }
        public string sys_EinvoiceBrand { get; set; }
        #endregion
        #region // Account
        public string sys_AccountBrand { get; set; }
        public string sys_AccountType { get; set; }
        #endregion
        #region //ICD
        public string sys_ICDToken { get; set; }
        #endregion
        #endregion

        #region // Minify
        public string sys_Minify { get; set; }
        #endregion

        #region// ZNS
        public string _sys_Zuser { get; set; }
        #endregion
        #region// Cust image size
        public int _sys_Custimgblod { get; set; }
        public int _sys_Custimgblockshare { get; set; }
        public string _sys_Custimgwatermark { get; set; }

 

        #endregion

        #region// Cust image size
        public int _sys_UsingbranchAddress { get; set; }
        #endregion
        #region //Allowsearchticket
        public int _sys_Allowsearchticket { get; set; }
        #endregion

        public BaseData(HttpContext httpContext)
        {
            _URL_PortClient = Comon.Global.sys_URL_PortClient;
            sys_Minify = Session.GetSession(httpContext.Session ,"Minify");

            GlobalUser user = Session.GetUserSession(httpContext);
            _usingCallCenter = user.sys_UsingCallCenter.ToString();
            _checkingCallCenter = Comon.Global.sys_ClientIsUsingCall;
            // Client co dung call , va user duoc attach call
            _callextension = "";
            _LinkClickToCall = "";
            if (_usingCallCenter == "1" && _checkingCallCenter == 1)
            {
                _calloutbound = Comon.Global.sys_CallOutBound.ToString();
                _calllogintimeexpired = "200000";
                _callport = Comon.Global.sys_CallOutBoundPort.ToString();
                _calldomain = Comon.Global.sys_CallDomain.ToString();
                _callextension = Regex.Replace(user.sys_CalLExtension.ToString() ,@"\t|\n|\r" ,"");
                _callpassword = user.sys_CalLExtensionPassword.ToString();
            }
            else if (_usingCallCenter == "1" && _checkingCallCenter == 2)// ClickToCall Outsite
            {
                _callextension = Regex.Replace(user.sys_CalLExtension.ToString() ,@"\t|\n|\r" ,"");
                _LinkClickToCall = Comon.Global.sys_CallDomain.ToString();
                _sys_DomainAPI = Comon.Global.sys_DomainAPI.ToString();
                _sys_CallApi_Key = Comon.Global.sys_CallApi_Key.ToString();
            }
            else
            {
                _usingCallCenter = "0"; // khong co tinh nang call center
            }
            sys_HTTPImageRoot = Comon.Global.sys_HTTPImageRoot;

            _checkingimageNetwork = (Comon.Global.sys_ImageCheckNetwork != null) ? Comon.Global.sys_ImageCheckNetwork : "";
            if (user.sys_userAvatar == "")
                _circleAvatar = Comon.Global.sys_DefaultPic;
            else
                _circleAvatar = user.sys_userAvatar;
            _userNameCurrent = user.sys_username;
            _branchNameCurrent = user.sys_BranchShortName;
            _branchIDCurrent = user.sys_branchID;
            _roleGroupCurrent = user.sys_RoleName;
            _LayoutCustomer = user.sys_LayoutCustomer;
            _LayoutChildCustomer = user.sys_LayoutChildCustomer;
 
            _roleIDCurrent = user.sys_RoleID;
            sys_CustomerTab = JsonConvert.SerializeObject(Comon.Global.sys_TabCustomer);
            _areaBranch = user.sys_AreaBranch;
            _isAllBranch = user.sys_AllBranchID;
            _employeeID_Main = user.sys_employeeid;
            _dencos_Main = Comon.Global.sys_DentistCosmetic;
            _sys_Zuser = Comon.Global.sys_Zuser;
            _sys_heartbeat = Comon.Global.sys_heartbeat;
            _employeeName_Main = user.sys_employeename;
            _userID_Main = user.sys_userid;
            _userPopupMenu = JsonConvert.SerializeObject(Comon.Global.sys_UserPopup);
            _versionofWebApplication = Comon.Global.sys_DBVersion;
            _sys_Custimgblod = Comon.Global.sys_Custimgblod;
            _sys_Custimgblockshare = Comon.Global.sys_Custimgblockshare;
            _sys_Custimgwatermark = Comon.Global.sys_Custimgwatermark;
            _sys_UsingbranchAddress = Comon.Global.sys_UsingbranchAddress;
            _sys_Allowsearchticket = Comon.Global.sys_Allowsearchticket;
            _nameClient = Comon.Global.sys_NameClient;
            _isCheckIPUser = (user.sys_DetectIP_isPassIP == 1 || user.sys_DetectIP_IsCheck == 1) ? 1 : 0;
            PermissionTable_Menu = JsonConvert.SerializeObject(GetPermissionForMenu(user.sys_PermissionMenu));
            PermissionTable_TabControl = JsonConvert.SerializeObject(user.sys_PermissionTabControl);
            sys_PermissionControl = JsonConvert.SerializeObject(Comon.Comon.GetControlPermissionByGroup(user.sys_PermissionControl ,user.sys_RoleServerID) ,Formatting.Indented);
            sys_PermisstionControlMustHide = JsonConvert.SerializeObject(Comon.Global.sys_PermissionControlMustHide_Table ,Formatting.Indented);

            _SMSBrandName = user.sys_SMSBrandName.Select(l => l.sys_SMSBrandName).ToList();
            _MailBrand = user.sys_MailBrand.Select(l => l.sys_MailName).ToList();

            _SoftwareLogo = user.sys_SoftwareLogo;
            _SoftwareSmallLogo = user.sys_SoftwareSmallLogo;
            _CompanyWebsite = user.sys_CompanyWebsite;
            _CompanyName = user.sys_CompanyName;
            _isHasDashboard = user.sys_HasDashboard;
            _RequireValidation = JsonConvert.SerializeObject(Comon.Global.sys_RequireValidation);
            _dataFunctionAuthorise = JsonConvert.SerializeObject(Comon.Global.sys_AuthosizeSettingFunction);
            _dtConnectCall = Comon.Global.sys_DtDetectCallCenter;
            _Session_Branch = SessionName.Seesion_Branch;
            _Session_Teeth = SessionName.Seesion_Teeth;
            _Session_Service = SessionName.Seesion_Service;
            _Session_ServiceCare = SessionName.Seesion_ServiceCare;
            _Session_Employee = SessionName.Seesion_Employee;
            _Session_User = SessionName.Session_User;
            _Session_City = SessionName.Session_City;
            _Session_District = SessionName.Session_District;
            _Session_National = SessionName.Session_National;
            _Session_Commune = SessionName.Session_Commune;

            _Session_Preload_CustomerLayout = SessionName.Session_Preload_CustomerLayout;
            _Session_Data = SessionName.Session_Data;
            syntax_scmcn = user.syntax_scmcn;
            syntax_scmcn_doc = user.syntax_scmcn_doc;
            syntax_sn = user.syntax_sn;
            syntax_ss = user.syntax_ss;
            syntax_customercode = user.syntax_customercode;
            syntax_documentcode = user.syntax_documentcode;
            sys_TeleLevel = user.sys_TeleLevel;
            sys_TeleGroup = user.sys_TeleGroup;
            sys_EinvoiceBrand = Comon.Global.sys_HookEInvoice != null ? Comon.Global.sys_HookEInvoice.Invoice_Brand : "";
            sys_EinvoiceType = Comon.Global.sys_HookEInvoice != null ? Comon.Global.sys_HookEInvoice.Invoice_Type : "";
            sys_AccountBrand = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_Brand : "";
            sys_AccountType = Comon.Global.sys_HookAccount != null ? Comon.Global.sys_HookAccount.Account_Type : "";
            sys_ICDToken = JsonConvert.SerializeObject(user.synthird_ICDToken);
        }
        private DataTable GetPermissionForMenu(DataTable dt)
        {
            try
            {
                DataRow[] dr = dt.Select("MenuURL <> '' " ,"IndexGroup ASC,GroupID ASC,IndexChild ASC");
                return dr.CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
