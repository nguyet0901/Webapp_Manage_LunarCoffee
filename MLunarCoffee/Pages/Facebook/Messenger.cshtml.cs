using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MLunarCoffee.Pages.Facebook
{
    public class MessengerModel : PageModel
    {
        //Token khi đăng nhập và set quyền user
        //private string CurToken = "EAAIbz8vGhKEBAMdhRvsrC1sXwsBjKvpZArmk4kX2jgb9tiSksTqG1P9qxJaSCFhITqhTtk6bJc7giDK44VzFM0NGNgBwKsyZAWZAi1CDx0eKyce3CZAJz07HxcSLe3uXXbc7ApeDcdZB8QJqza8KAj6SJK86OlFxHA0ftZBBVIOr6ZCR0gvipDoiByZBO4QCJBgZBfRMKatBnfQZDZD";
        //public string _fanPage_Facebook { get; set; }
        //public string _TokeFanPage { get; set; }
        //public string _messageUnread { get; set; }
        //public string _link_api_send_text { get; set; }
        //public string _link_api_send_file { get; set; }
        //public string _link_api_send_comment { get; set; }
        //public string _link_api_send_comment_exe { get; set; }
        //public string _link_api_send_blocked { get; set; }
        //public string _link_api_upload_file { get; set; }
        //public string _facebookTag { get; set; }
        //public string _CurrentStaffID { get; set; }

        //public string _AssignPID { get; set; }
        //public string _AssignUID { get; set; }

        //public string _Area_Combo { get; set; }
        //public string _ServiceCare_Combo { get; set; }
        public string _versionofWebApplication { get; set; }
        public string _Face_Link_HTTP_File { get; set; }
        public string _Face_App_ID { get; set; }
        public void OnGet()
        {
            _Face_App_ID = ""; //Comon.Global.sys_Face_AppID;
            _Face_Link_HTTP_File = ""; //""; //Comon.Global.sys_HTTPImageRoot + "Attachment/";
            _versionofWebApplication = ""; //Comon.Global.sys_DBVersion;
            //_link_api_send_text = Comon.Global.sys_Face_LinkApi_Send_Text;
            //_link_api_send_comment = Comon.Global.sys_Face_LinkApi_Send_Comment;
            //_link_api_send_comment_exe = Comon.Global.sys_Face_LinkApi_Execute_Comment;
            //_link_api_send_blocked = Comon.Global.sys_Face_LinkApi_blocked;
            //_link_api_send_file = Comon.Global.sys_Face_LinkApi_Send_Files;
            //_link_api_upload_file = Comon.Global.sys_Face_LinkApi_Upload_File;
            //GlobalUser user = (GlobalUser)HttpContext.Current.Session["CurrentUserInfo"];
            //_CurrentStaffID = user.sys_userid.ToString();
            //LoadTag();
            ////Load_Area_Combo();
            ////Load_ServiceCare_Combo();
            ////Load_Discount_Combo();
            //_AssignPID = (Request.QueryString["PID"] != null) ? Request.QueryString["PID"].ToString() : "";
            //_AssignUID = (Request.QueryString["UID"] != null) ? Request.QueryString["UID"].ToString() : "";
        }

        // public async Task<IActionResult> OnPostFace_Load_Data_Initialize()
        //{
        //    try
        //    {

        //        var user = Session.GetUserSession(HttpContext);
        //        DataSet ds = new DataSet();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[MLU_sp_Facebook_v2_Initialize]", CommandType.StoredProcedure
        //                 , "@userid", SqlDbType.NVarChar, user.sys_userid
        //                );
        //        }
        //        if (ds != null)
        //        {
        //            return Content(JsonConvert.SerializeObject(ds));
        //        }
        //        else
        //        {
        //            return Content("0");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostGet_Conversation_List(string keycodestring, string number, string date, string filter, string isalarm, string search, string keyword)
        //{
        //    try
        //    {
        //        keycodestring = (keycodestring != null ? keycodestring : "");
        //        number = (number != null ? number : "");
        //        date = (date != null ? date : "");
        //        filter = (filter != null ? filter : "");
        //        isalarm = (isalarm != null ? isalarm : "");
        //        search = (search != null ? search : "");
        //        keyword = (keyword != null ? keyword : "");

        //        string dateFrom = "";
        //        string dateTo = "";
        //        if (date.Contains(" to "))
        //        {
        //            date = date.Replace(" to ", "@");
        //            dateFrom = date.Split('@')[0];
        //            dateTo = date.Split('@')[1];
        //        }
        //        else
        //        {
        //            dateFrom = date;
        //            dateTo = date;
        //        }

        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_LoadConversation]", CommandType.StoredProcedure
        //                  , "@numberget", SqlDbType.NVarChar, number
        //                  , "@keycodestring", SqlDbType.NVarChar, keycodestring.Replace("[", ",").Replace("]", ",")
        //                  , "@filter", SqlDbType.NVarChar, filter
        //                  , "@isalarm", SqlDbType.NVarChar, isalarm
        //                  , "@search", SqlDbType.NVarChar, search
        //                  , "@keyword", SqlDbType.NVarChar, keyword
        //                  , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
        //                  , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
        //                );
        //        }
        //        if (dt != null)
        //        {
        //            return Content(JsonConvert.SerializeObject(dt));
        //        }
        //        else
        //        {
        //            return Content("0");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveArea(string page, string client, string area)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        area = (area != null ? area : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Area]", CommandType.StoredProcedure,
        //               "@area", SqlDbType.Int, area
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveServiceCare(string page, string client, string servicecare)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        servicecare = (servicecare != null ? servicecare : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Servicecare]", CommandType.StoredProcedure,
        //               "@servicecare", SqlDbType.NVarChar, servicecare
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveDiscount(string page, string client, string discount)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        discount = (discount != null ? discount : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Dicount]", CommandType.StoredProcedure,
        //               "@discount", SqlDbType.Int, discount
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveTag(string page, string client, string tag)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        tag = (tag != null ? tag : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Tag]", CommandType.StoredProcedure,
        //               "@tag", SqlDbType.Int, tag
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveStaff(string page, string client, string staff)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        staff = (staff != null ? staff : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Staff]", CommandType.StoredProcedure,
        //               "@staff", SqlDbType.Int, staff
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Load_Note(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Load_Note]", CommandType.StoredProcedure
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(JsonConvert.SerializeObject(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Load_Time(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Load_Time]", CommandType.StoredProcedure
        //               , "@userid", SqlDbType.Int, user.sys_userid
        //               , "@page", SqlDbType.NVarChar, page
        //               , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(JsonConvert.SerializeObject(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveNote(string page, string client, string note)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        note = (note != null ? note : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_Note]", CommandType.StoredProcedure,
        //               "@note", SqlDbType.NVarChar, note
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_SaveCallBackTime(string page, string client, string callbacktime)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        callbacktime = (callbacktime != null ? callbacktime : "");
        //        var user = Session.GetUserSession(HttpContext);
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Save_CallBackTime]", CommandType.StoredProcedure,
        //              "@callbacktime", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(callbacktime)
        //             , "@userid", SqlDbType.Int, user.sys_userid
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Load_TicketInfo(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Load_TicketInfo]", CommandType.StoredProcedure
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(JsonConvert.SerializeObject(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}


        // public async Task<IActionResult> OnPostFace_Checking_Mess_Is_Exist(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Load_Specific_Conver]", CommandType.StoredProcedure
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(JsonConvert.SerializeObject(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Checking_Com_Is_Exist(string post_id, string parent_id, string comment_id)
        //{
        //    try
        //    {
        //        post_id = (post_id != null ? post_id : "");
        //        parent_id = (parent_id != null ? parent_id : "");
        //        comment_id = (comment_id != null ? comment_id : "");
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Load_Specific_Comment]", CommandType.StoredProcedure
        //             , "@post_id", SqlDbType.NVarChar, post_id
        //             , "@parent_id", SqlDbType.NVarChar, parent_id
        //             , "@comment_id", SqlDbType.NVarChar, comment_id);
        //        }
        //        return Content(JsonConvert.SerializeObject(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Make_Read_To_UnRead(string page, string client, string comment, string type)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        comment = (comment != null ? comment : "");
        //        type = (type != null ? type : "");
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Make_Read_To_Unread]", CommandType.StoredProcedure
        //             , "@type", SqlDbType.NVarChar, type.ToLower()
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@comment", SqlDbType.NVarChar, comment
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Make_UnRead_To_Read(string page, string client, string client_name, string comment, string type)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        client_name = (client_name != null ? client_name : "");
        //        comment = (comment != null ? comment : "");
        //        type = (type != null ? type : "");
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Make_UnRead_To_Read]", CommandType.StoredProcedure
        //            , "@type", SqlDbType.NVarChar, type.ToLower()
        //            , "@page", SqlDbType.NVarChar, page
        //             , "@comment", SqlDbType.NVarChar, comment
        //            , "@client", SqlDbType.NVarChar, client
        //             , "@client_name", SqlDbType.NVarChar, client_name);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Make_Star(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Make_Star_NonStar]", CommandType.StoredProcedure
        //            , "@page", SqlDbType.NVarChar, page
        //            , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(dt.Rows[0][0].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Update_When_Comment_Successful(string page, string client
        //    , string comment, string message)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        comment = (comment != null ? comment : "");
        //        message = (message != null ? message : "");
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Update_When_Send_Comment]", CommandType.StoredProcedure
        //             , "@message", SqlDbType.NVarChar, message
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@comment", SqlDbType.NVarChar, comment
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Update_When_Message_Successful(string page, string client
        //    , string type, string message, string url)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        type = (type != null ? type : "");
        //        message = (message != null ? message : "");
        //        url = (url != null ? url : "");
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataTable("[MLU_sp_Facebook_v2_Update_When_Send_Message]", CommandType.StoredProcedure
        //             , "@message", SqlDbType.NVarChar, message
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@type", SqlDbType.NVarChar, type
        //             , "@url", SqlDbType.NVarChar, url
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Loading_History_Comment(string page, string client)
        //{
        //    try
        //    {
        //        page = (page != null ? page : "");
        //        client = (client != null ? client : "");
        //        DataSet ds = new DataSet();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[MLU_sp_Facebook_v2_Load_History_Comment]", CommandType.StoredProcedure
        //             , "@page", SqlDbType.NVarChar, page
        //             , "@client", SqlDbType.NVarChar, client);
        //        }
        //        return Content(JsonConvert.SerializeObject(ds));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostFace_Loading_Long_Key(string access_token)
        //{
        //    try
        //    {
        //        string long_token = "0";
        //        string url = @"https://graph.facebook.com/v5.0/oauth/access_token?grant_type=fb_exchange_token&client_id={appid}&client_secret={secret}&fb_exchange_token={token}";
        //        url = url.Replace("{appid}", Comon.Global.sys_Face_AppID).Replace("{secret}", Comon.Global.sys_Face_Secret);
        //        url = url.Replace("{token}", access_token);

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            long_token = JToken.Parse(responseString)["access_token"].ToString();
        //        }

        //        return Content(long_token);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        // public async Task<IActionResult> OnPostUpdate_LongToken_Failure(string keypage)
        //{
        //    try
        //    {
        //        keypage = (keypage != null ? keypage : "");
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            confunc.ExecuteDataSet("[MLU_sp_Facebook_v2_LongToken_Failure]", CommandType.StoredProcedure
        //                    , "@keypage", SqlDbType.NVarChar, keypage);
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}
    }
}
