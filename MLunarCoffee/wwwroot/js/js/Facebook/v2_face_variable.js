var face_user_token = "";
var face_user_id = "";
var facebook_version = "v5.0";
var face_appsecret_proof;
var face_appsecret_token = '504527056866840|DW6P-OXgFvIaRlqCPUGQGNk0PT0';
var face_link_graph = "https://graph.facebook.com/";
var face_link_image_fanpage = 'https://graph.facebook.com/{keycode}/picture?type=square';
var face_data_conversation_list = {};
var face_data_load_initialize_tag = [];
var face_data_load_initialize_user = [];
var face_data_load_initialize_discount = [];
var face_data_load_initialize_area = [];
var face_data_load_initialize_servicecare = [];
var face_data_load_initialize_template = [];
var face_value_current_page;
var face_value_current_client_name;
var face_value_current_client;
var face_value_current_post;
var face_value_current_post_link;
var face_value_current_comment;
var face_is_send_mess_from_comment = 0;
var face_value_current_mess_post;
var face_value_current_conver_id;
var facebook_link_inbox_mail_teamplate = 'https://www.facebook.com{link}'
var facebook_link_inbox_mail;
var face_value_current_comment_edit;
var facebook_begin_save_ticket = 0;
var face_attach_file_allow = ['image/png', 'video/mp4', 'video/wav', 'image/jpeg'
    , 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    , 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
    , 'image/jpg', 'application/pdf', 'application/vnd.ms-excel']

var face_value_current_attach = [];
var face_value_clicked_page;
var face_value_clicked_client;
var face_value_clicked_comment;
var face_value_clicked_conver_id;
var face_value_clicked_mess_post;
var face_tag_send = 'ACCOUNT_UPDATE';
var numload_conver_maximum = 500;
var numload_conver_begin = 30;
var numload_conver_step = 50;
var numload_conver_current = 0;
var numload_mess_begin = 20;
var numload_mess_step = 100;
var numload_mess_current;




function Face_Render_Tag_Detail(data, id) {
    var myNode = document.getElementById(id);
    let stringContent = '';
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data != undefined && data != "") {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr =
                    '<div class="ui checkbox facebooktag_ticket">'
                    + '<input  id="tickettag' + item.ID + '" type="checkbox" value=' + item.ID + ' class="facebooktag">'
                    + '<label  for="tickettag' + item.ID + '" class="tagface_text">' + item.Name + '</label>'
                    + '<i class="circle mini red icon avt circle_tag" style="font-size: 12px;color:' + item.Color + ' !important"></i>'
                    + '</div>';

                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    Face_SaveTag();
}
function Face_Render_Tag_Filter(data, id) {
    var myNode = document.getElementById(id);
    let stringContent = '';
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data != undefined && data != "") {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr =
                    '<div class="ui checkbox facebooktag_ticket">'
                    + '<input  id="facebooktagfilter' + item.ID + '" type="checkbox" value=' + item.ID + ' class="facebooktagfilter">'
                    + '<label  for="facebooktagfilter' + item.ID + '" class="tagface_text">' + item.Name + '</label>'
                    + '<i class="circle mini red icon avt circle_tag" style="font-size: 12px;color:' + item.Color + ' !important"></i>'
                    + '</div>';

                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Render_Template(data, id) {
    
    var myNode = document.getElementById(id);
    let stringContent = '';
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data != undefined && data != "") {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let backgroud = (i % 2 != 0) ? 'white' : '#f1f9ff;';
                let tr =
                    '<div style="background:' + backgroud+'" class="face_template_div" id="face_template_' + item.ID+'">'
                    + '<label id="face_template_title_' + item.ID+'" class="face_template_title seachRow">' + item.Title + '</label>'
                    + '<label id="face_template_description_' + item.ID+'" class="face_template_description">' + item.Description + '</label>'
                    + '</div>';

                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Load_Conversation_List_More() {

    numload_conver_current = numload_conver_current + numload_conver_step;
    if (numload_conver_current > numload_conver_maximum) numload_conver_current = numload_conver_maximum;

    if (face_fitler_date == "")
        Face_Load_Conversation_List(numload_conver_current, 0, 0, 0, "");
    else {
        Face_Load_Conversation_List(numload_conver_current, 1, 1, 0, "");
    }
}
var face_ajax_request;
function Face_Load_Conversation_List(number, loadmore, filter, search, keyword) {
    let isalarm = ($('#filter_date_time_alarm').is(":checked")) ? 1 : 0;
    face_data_conversation_list = {};
    if (face_ajax_request != null) {
        face_ajax_request.abort();
        face_ajax_request = null;
    }
    face_ajax_request = AjaxLoad(url = "/Facebook/Messenger/?handler=Get_Conversation_List"
        , data = {
            'keycodestring': face_value_selected_page_string, 'number': number
            , 'date': face_fitler_date, 'filter': filter, 'isalarm': isalarm
            , 'search': search, 'keyword': keyword
        }, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data != undefined) {
                    for (i = 0; i < data.length; i++) {
                        let e = {};
                        e.page = data[i].page;
                        e.client = data[i].client;
                        e.client_name = data[i].client_name;
                        e.type = data[i].type;
                        e.message = data[i].message;
                        e.created = data[i].created;
                        e.updated = data[i].updated;
                        e.readtime = data[i].readtime;
                        e.client_to_server = data[i].client_to_server;
                        e.staff = data[i].staff;
                        e.tag = data[i].tag;
                        e.url = data[i].url;
                        e.servicecare = data[i].servicecare;
                        e.discount = data[i].discount;
                        e.area = data[i].area;
                        e.callbacktime = data[i].callbacktime;
                        e.ticket = data[i].ticket;
                        e.star = data[i].star;
                        e.typeconver = data[i].typeconver;
                        e.staffname = data[i].staffname;
                        e.discountname = data[i].discountname;
                        e.verb = data[i].verb;
                        e.post_id = data[i].post_id;
                        e.comment_id = data[i].comment_id;
                        e.lastexecute = data[i].lastexecute;

                        let id = (data[i].typeconver == 'mess')
                            ? (data[i].page + '-' + data[i].client)
                            : (data[i].page + '-' + data[i].post_id + '-' + data[i].comment_id);
                        face_data_conversation_list[id] = e;
                    }
                }
                Face_Render_Conversation_List(face_data_conversation_list, "MessengerArea_List", loadmore);
            }
        },
        sender = null,  
        before = function () {
            Face_SH_Begin_LoadMoreConver();
        },
        complete = function (e) {
            Face_SH_Complete_LoadMoreConver();
        }
    );
     
}
function Face_Render_Conversation_List(data, id, loadmore) {

    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        for ([key, value] of Object.entries(data)) {
            stringContent = stringContent + Face_Render_Conversation_List_Each(key, value);
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    Face_Event_Conversation_List_Click();
    Face_Scroll_Conversation_ToLast(loadmore);
    Face_Filter_Executing();
    Face_Get_Name_Client();
    let xxx = document.getElementsByClassName('pageconver');
  
    for (i = 0; i < xxx.length; i++) {
        Face_Load_Image_Conver_Asyn(xxx[i].id, xxx[i].src);
    }
}
function Face_Get_Name_Client() {
    let _x = document.querySelectorAll('.messegerItem,.messegerCommentItem')
    if (_x != undefined) {
        for (_p = 0; _p < _x.length; _p++) {
            if ($('#clientname_' + _x[_p].id).length && $('#clientname_' + _x[_p].id).html() == "")
                Face_API_Get_ClientInfo(_x[_p].id, _x[_p].attributes.data_client.value, _x[_p].attributes.data_page.value);
        }
    }
}
function Face_API_Get_ClientInfo(id, client, pageid) {
    let url = facebook_link_client_info.replace('{version}', facebook_version).replace('{client_id}', client);
    url = url + "?access_token=" + Face_Get_Token_Access_From_PageID(pageid);
    $.ajax({
        url: url,
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        success: function (result) {
            if (result != null && result != undefined) {
                $('#clientname_' + id).html(result.name);
            }
        }
    });
}
function Face_Render_Conversation_List_Each(key, item) {
    let stringContent, snippet, pic, piccurrentpage, piccurrentclient, clientname, iscallback, tagname, isread;
    let dateNow_onlydate = GetDateTime_Now_Only_Date();
    let url = "";
    pic = '//graph.facebook.com/' + item.client + '/picture?access_token=' + Face_Get_Token_Access_From_PageID(item.page);

    stringContent = "";
    piccurrentclient = '/assests/img/Face/client_send.png';
    piccurrentpage = face_link_image_fanpage.replace('{keycode}', item.page);
    clientname = item.client_name;
    if (item.updated > item.readtime) isread = 1;
    else isread = 0;
    let toppixcel;
    if (item.typeconver == "mess") {
        //mess
        switch (item.type) {
            case "text":
                snippet = (item.message.length > 30 ? item.message.substring(0, 30) : item.message);
                url = '/assests/img/Face/more_16.png';
                break;
            case "image":
                snippet = "";
                url = item.url;
                break;
            case "audio":
                snippet = "Sending a audio";
                url = "/assests/img/Face/audio_16.png";
                break;
            case "video":
                snippet = "Sending a video";
                url = "/assests/img/Face/video_16.png";
                break;
            default:
                snippet = "Sending a file";
                url = "/assests/img/Face/file_16.png";
                break;
        }
        if (dateNow_onlydate > item.callbacktime || item.updated > item.callbacktime) iscallback = 0;
        else iscallback = 1;
        tagname = Face_Render_Tag(item.tag);
        toppixcel = '18px';

    }
    else {
        //comment
        snippet = (item.message.length > 30 ? item.message.substring(0, 30) : item.message);
        snippet = (snippet == "") ? "New comment" : snippet;
        iscallback = 0;
        tagname = ""; toppixcel = '9px';
    }
    let tr =
        '<img id="image_avatar_conver_' + key+'" style="float: left !important;top: ' + toppixcel + ';" class="ui mini circular image pageconver pageAvatar" src="' + pic + '" alt="label-image" />'
        + '<div class="divItemName">'
        + '<span id="clientname_' + key + '" class="snipTime_snippest face_client_name">' + clientname + '</span>'
        + '<span class="snipTime_snippest snip_text">'
        + '<img id="smallavatar_client_' + key + '" style="float: left !important; display:' + ((item.client_to_server == 0) ? 'none' : 'block') + ';" class="ui mini circular image pageAvatarsmall" src="' + piccurrentclient + '" alt="label-image" />'
        + '<img id="smallavatar_page_' + key + '" style="float: left !important; display:' + ((item.client_to_server == 1) ? 'none' : 'block') + ';" class="ui mini circular image pageAvatarsmall" src="' + piccurrentpage + '" alt="label-image" />'
        + '<span style="margin-right: 10px;" id="_text_snippet_' + key + '">' + snippet + '</span>'
        + ((url != "") ? '<img id="_img_snippet_' + key + '" class="imgsnipset" src="' + url + '" alt="label-image" />' : '')
        + '</span>'
        + '</div>'

        + '<div class="divRightCommentMess">'
        + '<span class="snipTime_time" id="_snipTime_' + key + '" data-time="' + item.updated + '">' + ConvertDateTimeUTC_NoYear(item.updated) + '</span>'
        + ((item.typeconver == "mess")
            ? '<img class="imgMessCom" title="Inbox" src="/assests/img/Face/mess_.png" alt="label-image" />'
            : '<img class="imgMessCom" title="Comment" src="/assests/img/Face/comment_.png" alt="label-image" />')
        + ((item.typeconver == "mess" && item.ticket != 0)
            ? '<img id="_img_profile_' + key + '" class="imgMessCom" title="Có Hồ Sơ"  src="/assests/img/Face/profile_.png" alt="label-image" />'
            : '<img id="_img_profile_' + key + '" class="imgMessCom" style="display:none;" title="Có Hồ Sơ"  src="/assests/img/Face/profile_.png" alt="label-image" />')

        + ((item.typeconver == "mess" && iscallback == 1) ? ('<img class="imgMessCom" title="' + ConvertDateTimeUTC_NoYear(item.callbacktime) + '" src="/assests/img/Face/' + ((iscallback != 0) ? ('time_done') : ('time_not')) + '.png" alt="label-image">') : (''))
        + ((item.typeconver == "mess" && item.star == "1")
            ? '<img class="imgMessCom" title="Star" id="star_' + key + '" src="/assests/img/Face/star_.png" alt="label-image" />'
            : '<img class="imgMessCom" title="Star" id="star_' + key + '" style="display:none;" src="/assests/img/Face/star_.png" alt="label-image" />')
        + '</div>'
        + '<span class="tagMessParent" id="tag_' + key + '" >' + tagname
        + '</span>'
        + '<img id="exe_' + key + '" class="imgExecute" data_type="' + item.typeconver
        + '" data_page="' + item.page
        + '" data_client="' + item.client
        + '" data_comment="' + item.comment_id
        + '" src="/assests/img/Face/dot.png" alt="label-image">'
        + '<span id="staffname_' + key + '" class="staffInConversation">' + item.staffname + '</span>'

    let classitem = (item.typeconver == "mess") ? "messegerItem" : "messegerCommentItem";
    let classnew = (isread == 1) ? "newMessage" : "";
    stringContent = stringContent + '<a data_staff="' + item.staff
        + '" data_typeconver="' + item.typeconver
        + '" data_verb="' + item.verb
        + '" data_post="' + item.post_id
        + '" data_comment="' + item.comment_id
        + '" data_ticket="' + item.ticket
        + '" data_servicecare="' + item.servicecare
        + '" data_tag="' + item.tag
        + '" data_page="' + item.page
        + '" data_type="' + item.type
        + '" data_client="' + item.client
        + '" data_discount="' + item.discount
        + '" data_area="' + item.area
        + '" data_star="' + item.star
        + '" data_avatar="' + pic
        + '" data_discountname="' + item.discountname
        + '" data_client_name="' + clientname
        + '" class="' + classitem + ' ' + classnew
        + '" href="#" id=' + key + '>' + tr + '</a>';

    return stringContent;

}
function Face_SH_Complete_LoadMoreConver() {
    $("#MessengerArea_List_loaderList").hide();
    $("#loadmoreCon").show();
}
function Face_SH_Begin_LoadMoreConver() {
    $("#MessengerArea_List_loaderList").show();
    $("#loadmoreCon").hide();
}
function Face_Render_Tag(tag) {
    let resulf = "";
    let tagelement = "";
    if (tag != undefined && tag != "") {
        let x = tag.split(',');
        let indexCount = 0;
        for (i = 0; i < x.length; i++) {
            if (x[i] != "") {
                if (indexCount < 3) {
                    let id = Number(x[i]);
                    let color = "white";
                    let y = face_data_load_initialize_tag.filter(word => word["ID"] == id);
                    if (y[0] != undefined) {
                        color = y[0].Color;
                        let name = y[0].Name;
                        tagelement = '<span class="tagMessList" title="' + name + '" style="background-color:' + color + '"></span>';
                        resulf = resulf + tagelement;
                        indexCount = indexCount + 1;
                    }

                }
                else {
                    let numleft = x.length - indexCount - 1;
                    resulf = resulf + '<span class="tagMessMore">' + (numleft + ' more +') + '</span>';
                    i = x.length;
                }
            }
        }
    }
    if (resulf == "") resulf = '<span class="tagMessList"></span>';
    return resulf;
}
function Face_Event_Conversation_List_Click() {
    $(".imgExecute").click(function (event) {
        $("#popupExecute").css({ left: event.pageX });
        $("#popupExecute").css({ top: event.pageY - 70 });
        $("#popupExecute").show();
        let _id = this.id;
        face_value_clicked_conver_id = _id.replace('exe_', '');
        face_value_clicked_mess_post = $(this).attr("data_type");
        face_value_clicked_page = $(this).attr("data_page");
        face_value_clicked_client = $(this).attr("data_client");
        face_value_clicked_comment = $(this).attr("data_comment");
        if (face_value_clicked_mess_post == "mess") {
            $('#face_mailstar').show();
        }
        else $('#face_mailstar').hide();
        $('#MessengerArea_Reply_Comment').hide();
        face_is_send_mess_from_comment = 0;
        event.stopPropagation();
    });
    $(".messegerCommentItem").click(function (event) {
        face_value_current_conver_id = this.id;
        face_value_current_mess_post = "COM";
        face_value_current_page = $(this).attr("data_page");
        face_value_current_client = $(this).attr("data_client");
        face_value_current_client_name = ($(this).attr("data_client_name") != "") ? $(this).attr("data_client_name") : $('#clientname_' + face_value_current_conver_id).html();

        face_value_current_post = $(this).attr("data_post");
        face_value_current_comment = $(this).attr("data_comment");
       

        let avatar = $(this).attr("data_avatar");
        Face_Fill_Data_When_Click_Comment(avatar);
        Face_Fill_Data_When_Click_Page(face_value_current_page);
        Face_SH_Click_Conversation_Left();
        Face_API_Get_Comment_Post_And_Content();
        Face_Css_When_Click(face_value_current_conver_id);
        Face_Update_UnRead_To_Read();
        $('#MessengerArea_Reply_Comment').hide();
        face_is_send_mess_from_comment = 0;
        
    });
    $(".messegerItem").click(function (event) {
        face_value_current_conver_id = this.id;
        face_value_current_mess_post = "MESS";
        face_value_current_page = $(this).attr("data_page");
        face_value_current_client = $(this).attr("data_client");
        face_value_current_client_name = ($(this).attr("data_client_name") != "") ? $(this).attr("data_client_name") : $('#clientname_' + face_value_current_conver_id).html();

        face_value_current_attach = []; // Reset attacment file
        let tag = $(this).attr("data_tag");
        let servicecare = $(this).attr("data_servicecare");
        let discount = $(this).attr("data_discount");
        let area = $(this).attr("data_area");
        let discountname = $(this).attr("data_discountname");
        let star = $(this).attr("data_star");
        let staff = $(this).attr("data_staff");
        let avatar = $(this).attr("data_avatar");
        Face_Fill_Data_When_Click_Message(tag, servicecare, discount, area, discountname, star, staff, avatar);
        Face_Fill_Data_When_Click_Page(face_value_current_page);
        numload_mess_current = numload_mess_begin;
        Face_API_Get_Conversation(numload_mess_current, 0);
        Face_SH_Click_Conversation_Left();
        Face_API_Checking_Block(face_value_current_page, face_value_current_client);
        Face_Css_When_Click(face_value_current_conver_id);
        Face_Update_UnRead_To_Read();
        $('#MessengerArea_Reply_Comment').hide();
        face_is_send_mess_from_comment = 0;
    });
}

function Face_Get_appsecret_proof() {
    var hash = CryptoJS.HmacSHA256("EAAHK3VwBDhgBAHxtpjPtSqcUycZALZCxx64UZCtbJNRAp3LchaWP1X2O3E1hb69fwW3Nh7D6cY7Nl4p6mVZBNZBmz1Ocp0ZCyVxXIIhGFPhsOAvQMx8AyQcsV3fS1x3iShjIQyyGLFxQgZBRFe6wR4I8OmaZCUqXlAVyXNKNAja0BvZC7gsaZAZBWwirhAtrt1eU8S604AVKDb5qQZDZD", face_app_secret);
    var base64 = hash.toString(CryptoJS.enc.Base64);
    face_appsecret_proof = base64;
}
//Update design

function Face_Load_Image_Asyn(id, link) {

    $("#" + id).attr('src', '/assests/img/Face/loading_avatar.gif');
    var img = $("<img />").attr('src', link)
        .on('load', function () {
            if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) { 
            } else {
                $("#" + id).attr('src', img[0].src);
                img = null;
            }
        });
}
function Face_Load_Image_Conver_Asyn(id, link) {
 
    $("#" + id).attr('src', '/assests/img/Face/loading_avatar.gif');
    var img = $("<img />").attr('src', link)
        .on('load', function () {
            if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
           
            }
            else {
                $("#" + id).attr('src', img[0].src);
                img = null;
            }
        });
}

function Face_Changing_Page_Start() {
    $('#MessengerArea_Info').hide();
    $('#MessengerArea_Chating').hide();
    $("#ParentMessengerArea_Attachment").hide();
    $('#MessengerArea_Messenger_Area').hide();
}
function Face_SH_Click_Conversation_Left() {
    if (face_value_current_mess_post == "COM") {
        $('#btnMessengerTemplate').hide();
        $('#attachFileImg').hide();
        $('#gotoProfileMess').show();
        $('#btnPostFace_Link').show();
    }
    else {
        $('#btnMessengerTemplate').show();
        $('#attachFileImg').show();
        $('#gotoProfileMess').hide();
        $('#btnPostFace_Link').hide();

    }
    $('#MessengerArea_Info').show();
    $("#ParentMessengerArea_Attachment").hide();
    $('#MessengerArea_Attachment').empty()
    $('#MessengerArea_Messenger').css({ 'height': 'calc(100vh - 256px)' });
    $("#MessengerArea_Chating").show();
    $('#MessengerArea_Messenger_Area').show();
    $('#MessengerArea_Chating_Hide').hide();
    $('#MessengerArea_Messenger').empty()

}
function Face_SH_Click_Error() {
    $("#ParentMessengerArea_Attachment").hide();
    $('#MessengerArea_Attachment').empty()
    $('#MessengerArea_Info').hide();
    $('#MessengerArea_Chating').hide();
    $("#ParentMessengerArea_Attachment").hide();
    $('#MessengerArea_Messenger_loaderList').hide();
    $('#MessengerArea_Messenger').css({ 'height': 'calc(100vh - 256px)' });
    $('#MessengerArea_Chating_Hide').show();

}
function Face_SH_Blocked_Mess() {
    $('#face_tic_blocked').show();
    $('#face_tic_un_block').hide();
    $('#MessengerArea_Chating_Hide').show();
    $('#MessengerArea_Chating').hide();
}
function Face_SH_No_Block_Mess() {
    $('#MessengerArea_Chating_Hide').hide();
    $('#MessengerArea_Chating').show();
    $('#face_tic_blocked').hide();
    $('#face_tic_un_block').show();
}