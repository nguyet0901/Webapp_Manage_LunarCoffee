var facebook_link_getlistpage = "https://graph.facebook.com/{version}/me/accounts?fields=picture{url},id,access_token,name,link";
var facebook_link_subscribed_app = "https://graph.facebook.com/{version}/{pageid}/subscribed_apps";
var face_data_page_of_userfacebook = [];
var face_data_load_initialize_page = [];

var face_value_selected_page_string = "";
var face_value_total_page_string = "";

function Face_Init() {
    FB.init({
        appId: face_app_appid,
        cookie: true,
        xfbml: true,
        version: facebook_version
    });
    FB.getLoginStatus(function (response) {
        Face_Login_Facebook_Status(response);
    });
}

function Face_Login_Facebook_Status(response) {

    if (response.authResponse != undefined && response.status == "connected") {
        face_user_token = response.authResponse.accessToken;
        face_user_id = response.authResponse.userID;
        let pic = '//graph.facebook.com/' + face_user_id + '/picture?access_token=' + face_user_token;
        $('#myfacebookavatar').attr("src", pic);
        $('#Face_loginin').hide();
        $('#Face_loginout').show();
        Face_Load_Data_Initialize();
        $('#Face_loginout').on("click", function () {
            Face_Logout();
        });
    }
    else {
        $('#Face_loginin').show();
        $('#Face_loginout').hide();
    }
}
function Face_SuccessLogin(e) {
    location.reload();
}

function Face_Load_Data_Initialize() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Load_Data_Initialize"
        , data = {  }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                let dataPage = data.Table;
                face_data_load_initialize_user = data.Table1;
                face_data_load_initialize_tag = data.Table2;
                face_data_load_initialize_area = data.Table3;
                face_data_load_initialize_servicecare = data.Table4;
                face_data_load_initialize_discount = data.Table5;
                face_data_load_initialize_template = data.Table6;
                for (i = 0; i < dataPage.length; i++) {
                    let e = {};
                    e.name = dataPage[i].Name;
                    e.access_token = "";
                    e.long_access_token = "";
                    face_data_load_initialize_page[dataPage[i].KeyPage] = e;
                    face_value_total_page_string = face_value_total_page_string + "[" + dataPage[i].KeyPage + "]";
                }

                face_value_selected_page_string = Face_Storage_PageChossing_Get();
                Face_Render_Combo_Page(face_data_load_initialize_page, "CurrentFace_Chossen");
                Face_Event_ChoosingPage_Click();
                Face_Render_Tag_Detail(face_data_load_initialize_tag, "facebooktag_ticket");
                Face_Render_Tag_Filter(face_data_load_initialize_tag, "listTagFiter");
                FaceFilter_Tag_Event();
                FaceFilter_Date_Event();

                Face_Render_Template(face_data_load_initialize_template, "MessengerTemplateBody");
                Face_Search_Teamplate_Event();
                Load_Combo(face_data_load_initialize_area, "cbbArea", false);
                Load_Combo(face_data_load_initialize_user, "cbbDectectStaffOnConver", true);
                Load_Combo(face_data_load_initialize_user, "cbbTokenTagStaff", true);
                Load_Combo(face_data_load_initialize_servicecare, "cbbServiceCareFace", false);
                Load_Combo(face_data_load_initialize_discount, "cbbDiscountFace", false);
                FB_Getlistpage();

                // Face_DeclarePageFacebook();
                Face_Changing_Page_Start();
                $('#MessengerArea').show();
            }
        }
        , sender = null
    );

}
function Face_Render_Combo_Page(data, id) {

    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        for ([key, value] of Object.entries(data)) {
            let tr =
                '<div id="divpageFanpage_' + key + '" class="div_face_page_total">'
                + '<a  class="ui left right corner label aTagChoosenpage"><i id="pageFanpageChosen_' + key + '" class="minus icon iconActivePageFace"></i></a>'
                + '<img id="pageFanpage_' + key + '" class="ui mini circular image pageAvatar pageAvatarpage" title="'
                + value.name
                // + '" src="' + face_link_image_fanpage.replace('{keycode}', key) + '" alt="label-image" />'
                + '" src="/assests/img/Face/loading_avatar.gif" alt="label-image" />'
                + '</div>'
            stringContent = stringContent + tr;

        }
        document.getElementById(id).innerHTML = stringContent;

    }
}
function FB_Getlistpage() {

    $.ajax({
        url: facebook_link_getlistpage.replace('{version}', facebook_version) + "&access_token=" + face_user_token,
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError();
        },
        success: function (result) {

            let data = result.data;
            face_data_page_of_userfacebook = [];
            if (data != undefined && data.length != 0) {
                for (i = 0; i < data.length; i++) {
                    let e = {};
                    e.id = data[i].id;
                    e.access_token = data[i].access_token;
                    face_data_page_of_userfacebook.push(e);
                }
                Face_Initialize_Detect_And_Suprise();
            }
        },
        beforeSend: function () {
            $('#Face_Loading_Div').show();
            $('#CurrentFace_Chossen').hide();
        },
        complete: function () {
            $('#Face_Loading_Div').hide();
            $('#CurrentFace_Chossen').show();
        }

    })
}
function Face_Initialize_Detect_And_Suprise() {
    let promises = [];
    for ([key, value] of Object.entries(face_data_load_initialize_page)) {
        promises.push(Face_Initialize_Detect_And_Suprise_Each(key));
    }
    Promise.all(promises)
        .then((results) => {
            //console.log("All done", results);
            Face_Load_Conversation_List(numload_conver_begin, 0,0,0,"");
        })
        .catch((ex) => {
            // Handle errors here
        });
}
async function Face_Initialize_Detect_And_Suprise_Each(KeyPage) {
    $('#pageFanpage_' + KeyPage).attr('src', '/assests/img/Face/loading_avatar.gif')
    await Face_Initialize_Detect_And_Suprise_Each_ActionAsync(KeyPage);
    $('#pageFanpage_' + KeyPage).attr('src', face_link_image_fanpage.replace('{keycode}', KeyPage))

}
function Face_Initialize_Detect_And_Suprise_Each_ActionAsync(KeyPage) {
    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
                //let len = Object.keys(face_data_load_initialize_page).length;
                let active = 0;
                let data = face_data_page_of_userfacebook.filter(word => {
                    return (word["id"] == KeyPage);
                });

                if (data != undefined && data.length == 1) {
                    Face_Subscribed_App(KeyPage, data[0].access_token);
                    // if (Face_Subscribed_App(KeyPage, data[0].access_token) == 1)
                    active = 1;
                }
                if (active == 1) {
                    Face_Active_Sucess(KeyPage, data[0].access_token)
                }
                else {
                    Face_Active_Failure(KeyPage);
                    Sort_Page_Actice();
                }

                resolve()
            }
        )
    })
}
function Face_Active_Failure(KeyPage) {
    let idimg = "#pageFanpage_" + KeyPage;
    if ($(idimg).hasClass("pageAvatarpage_Have_Permission")) {
        $(idimg).removeClass("pageAvatarpage_Have_Permission");
    }
    if (!$(idimg).hasClass("pageAvatarpage_Not_Permission")) {
        $(idimg).addClass("pageAvatarpage_Not_Permission");
    }
    $('#pageFanpageChosen_' + KeyPage).remove();
    if (face_value_selected_page_string.includes("[" + KeyPage + "]")) {
        face_value_selected_page_string.replace("[" + KeyPage + "]", "")
        Face_Storage_PageChossing_Set(face_value_selected_page_string);
    }
    if (face_value_total_page_string.includes("[" + KeyPage + "]")) {
        face_value_total_page_string.replace("[" + KeyPage + "]", "")
    }
}
function Face_Active_Sucess(KeyPage, access_token) {
    let idimg = "#pageFanpage_" + KeyPage;
    if ($(idimg).hasClass("pageAvatarpage_Not_Permission")) {
        $(idimg).removeClass("pageAvatarpage_Not_Permission");
    }
    if (!$(idimg).hasClass("pageAvatarpage_Have_Permission")) {
        $(idimg).addClass("pageAvatarpage_Have_Permission");
    }
    face_data_load_initialize_page[KeyPage].access_token = access_token;
    Face_Initialize_Get_Long_Key(KeyPage, access_token);
    if (face_value_selected_page_string.includes("[" + KeyPage + "]")) {
        let _idicon = "pageFanpageChosen_" + KeyPage;
        $('#' + _idicon).attr('style', 'display:block !important');
    }
}
function Face_Initialize_Get_Long_Key(keypage, acesstoken) {

    if (face_data_load_initialize_page[keypage] != undefined) {
        let long_token = face_data_load_initialize_page[keypage].long_access_token;
        if (long_token == undefined || long_token == "") {
            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Loading_Long_Key"
                , data = { 'access_token': acesstoken }
                , async = true
                , error = function () { notiError_SW(); }
                , success = function (result) {
                    if (result != "0") {
                        face_data_load_initialize_page[keypage].long_access_token = result;
                    }
                }
                , sender = null
            );

        }
    }
}
function Face_Logout() {
    FB.logout(function (response) {
        Face_Storage_PageChossing_Clear();

        location.reload();
    });
}
function Face_Event_ChoosingPage_Click() {
    $(".div_face_page_total").click(function (event) {
        let _id = this.id.replace('divpageFanpage_', '');
        let isActive = 0;
        if (face_data_load_initialize_page[_id] != undefined)
            if (face_data_load_initialize_page[_id].long_access_token != undefined
                && face_data_load_initialize_page[_id].long_access_token != "")
                isActive = 1;

        if (isActive == 1) {



            let _idicon = "pageFanpageChosen_" + _id;
            if ($('#' + _idicon).is(":visible")) {
                $('#' + _idicon).attr('style', 'display:none !important');

                if (face_value_selected_page_string.includes("[" + _id + "]")) {
                    face_value_selected_page_string = face_value_selected_page_string.replace("[" + _id + "]", "")
                    Face_Storage_PageChossing_Set(face_value_selected_page_string);
                }

            }
            else {
                $('#' + _idicon).attr('style', 'display:block !important');
                if (!face_value_selected_page_string.includes("[" + _id + "]")) {
                    face_value_selected_page_string = face_value_selected_page_string + "[" + _id + "]";
                    Face_Storage_PageChossing_Set(face_value_selected_page_string);
                }
            }
            if ($('#divpageFanpage_' + _id).hasClass('pageAvatarpage_NewMessage')) {
                $('#divpageFanpage_' + _id).removeClass('pageAvatarpage_NewMessage')
            }

            Face_Load_Conversation_List(numload_conver_begin, 0,0,0,"");
        }
    });
}
function Face_Subscribed_App(pageid, token) {
    $.ajax({
        url: facebook_link_subscribed_app.replace('{version}', facebook_version).replace('{pageid}', pageid),
        dataType: "json",
        type: "POST",
        data: JSON.stringify({
            "subscribed_fields": "messages,messaging_postbacks,feed"
            , "access_token": token
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Face_Active_Failure(pageid);
        },
        success: function (result) {

            if (result != undefined && result.success != undefined && result.success == true) {
                Face_Active_Sucess(pageid, token)
            } else Face_Active_Failure(pageid);
        },
        complete: function (result) {

        }
    });
}
function Face_Get_Token_Access_From_PageID(keypage) {

    //return "EAANKQZBAuokMBAAosQcmsktMCtdQn4qdAG0Eof93hZBRH5hGYZB4SjkEteDTJ924hxVoKuioTVVtAxBUdZCGwCqJqJClmlEpHdsyqUh5ijmNVEueb7ln9ZBXtMPmWkgxI29muRVitG3zhHrxWY7CQNZC3kTETGSyD9uQhLhvZBmwQZDZD"
    return face_data_load_initialize_page[keypage].long_access_token;
}
//function Face_Get_Short_Token_Access_From_PageID(keypage) {
//    return face_data_load_initialize_page[keypage].access_token;
//}
function Face_Storage_PageChossing_Clear() {
    localStorage.setItem("facebookpage", "");
}
function Face_Storage_PageChossing_Get() {
    try {
        return (localStorage.getItem("facebookpage") == null) ? "" : localStorage.getItem("facebookpage");
    }
    catch (ex) {
        return "";
    }
}
function Face_Storage_PageChossing_Set(data) {
    localStorage.setItem("facebookpage", data);
}

