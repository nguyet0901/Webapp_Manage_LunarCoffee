
var facebook_link_subscribed_app = "https://graph.facebook.com/{version}/{pageid}/subscribed_apps";
var facebook_link_permission = "https://graph.facebook.com/{version}/me/permissions";
var facebook_link_debug_token = "https://graph.facebook.com/{version}/debug_token?fields=is_valid&input_token={input_token}";
var face_data_page_of_userfacebook = [];
var face_data_load_initialize_page = [];

var face_value_selected_page_string = "";
var face_value_total_page_string = "";
var face_value_selected_page_number = 0;

var face_page_merger_limit = 3;//số fanpage merger tối da.



function Face_Load_Data_Initialize() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Load_Data_Initialize"
        , data = {}, async = true
        , error = function () { notiError(); }
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
                    e.long_access_token = dataPage[i].LongToken_Key;
                    e.long_token_status = dataPage[i].LongToken_Status;
                    face_data_load_initialize_page[dataPage[i].KeyPage] = e;
                    face_value_total_page_string = face_value_total_page_string + "[" + dataPage[i].KeyPage + "]";
                }

                face_value_selected_page_string = Face_Storage_PageChossing_Get();
                let j = 0;
                var result = face_value_selected_page_string.split("[").reduce(function (acc, curr) {
                    return j++;
                }, {});
                face_value_selected_page_number = result;
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
                Load_Combo(face_data_load_initialize_servicecare, "cbbServiceCareFace", true);
                Load_Combo(face_data_load_initialize_discount, "cbbDiscountFace", false);
                //FB_Getlistpage();
                Face_Initialize_Detect_And_Suprise();
                Face_Changing_Page_Start();
                $('#MessengerArea').show();
            }
        }, 
        sender = null,
        before = function () {
            $('#Face_Loading_Div').show();
            $('#CurrentFace_Chossen').hide();
        },
        complete = function (e) {
            $('#Face_Loading_Div').hide();
            $('#CurrentFace_Chossen').show();
        }
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
                + '" src="/assests/img/Face/loading_avatar.gif" alt="label-image" />'
                + '<div class="titlepage" >' + value.name +'</div>'
                + '</div>'
            stringContent = stringContent + tr;

        }
        document.getElementById(id).innerHTML = stringContent;

    }
}

function Face_Initialize_Detect_And_Suprise() {
    let promises = [];
    for ([key, value] of Object.entries(face_data_load_initialize_page)) {
        promises.push(Face_Initialize_Detect_And_Suprise_Each(key));
        Face_Initialize_Page_Debug_Token(key, value);
    } 
    Promise.all(promises)
        .then((results) => {
            let findstring = Face_Storage_FindItem_Get();
            if (findstring != "") {
                let keyword = findstring.split('-')[0]; 
                let page = findstring.split('-')[1];
                $("#searchingFacebook").val(keyword);
                let _idicon = "pageFanpageChosen_" + page;
                if (!$('#' + _idicon).is(":visible")) {

                    $('#' + _idicon).attr('style', 'display:block !important');
                    if (!face_value_selected_page_string.includes("[" + page + "]")) {
                        face_value_selected_page_string = face_value_selected_page_string + "[" + page + "]";
                        Face_Storage_PageChossing_Set(face_value_selected_page_string);
                    }

                } 
                Face_Load_Conversation_List(numload_conver_begin, 0, 0, 1, keyword);
            }
            else {
                Face_Load_Conversation_List(numload_conver_begin, 0, 0, 0, "");
            }
            Face_Storage_FindItem_Clear();
        })
        .catch((e) => {
        });
}
async function Face_Initialize_Detect_And_Suprise_Each(KeyPage) {
    $('#pageFanpage_' + KeyPage).attr('src', face_link_image_fanpage.replace('{keycode}', KeyPage))
    if (face_value_selected_page_string.includes("[" + KeyPage + "]")) {
        let _idicon = "pageFanpageChosen_" + KeyPage;
        $('#' + _idicon).attr('style', 'display:block !important');
    }

}
function Face_Initialize_Detect_And_Suprise_Each_ActionAsync(KeyPage) {
    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
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
                    face_value_selected_page_number--;
                }

            }
            else if (face_value_selected_page_number < face_page_merger_limit) {
                $('#' + _idicon).attr('style', 'display:block !important');
                if (!face_value_selected_page_string.includes("[" + _id + "]")) {
                    face_value_selected_page_string = face_value_selected_page_string + "[" + _id + "]";
                    Face_Storage_PageChossing_Set(face_value_selected_page_string);
                    face_value_selected_page_number++;

                }
            } else {
                return false;
            }
            if ($('#divpageFanpage_' + _id).hasClass('pageAvatarpage_NewMessage')) {
                $('#divpageFanpage_' + _id).removeClass('pageAvatarpage_NewMessage')
            }
            Face_Load_Conversation_List(numload_conver_begin, 0, 0, 0, "");
        } else {
            notiPopup_Type("re_register_fanpage_software");
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
    return face_data_load_initialize_page[keypage].long_access_token;
}

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


function Face_Storage_FindItem_Clear() {
    localStorage.setItem("facebookpage_goto", "");
}
function Face_Storage_FindItem_Get() {
    try {
        return (localStorage.getItem("facebookpage_goto") == null) ? "" : localStorage.getItem("facebookpage_goto");
    }
    catch (ex) {
        return "";
    }
}
//Face_loginout
 


//debug token page 
function Face_Initialize_Page_Debug_Token(key_page, data_page) {
    if (data_page.long_access_token && data_page.long_access_token != '' && data_page.long_token_status == "1") {
        $.ajax({
            url: facebook_link_debug_token.replace('{version}', facebook_version).replace('{input_token}', data_page.long_access_token) + "&access_token=" + face_appsecret_token,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                Face_Active_Failure(key_page);
                if (data_page.long_token_status == "1")
                    Update_Face_Fanpage_Failure_Token(key_page);
            },
            success: function (result) {
                let data = result.data;
                if (data != undefined) {
                    if (data.is_valid == false) {
                        Face_Active_Failure(key_page);
                        if (data_page.long_token_status == "1")
                            Update_Face_Fanpage_Failure_Token(key_page);
                    }
                }
            }
        })
    } else {
        Face_Active_Failure(key_page);
        if (data_page.long_token_status == "1")
            Update_Face_Fanpage_Failure_Token(key_page);
    }
}
function Face_Active_Failure(KeyPage) {
    face_data_load_initialize_page[KeyPage].long_access_token = '';
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
function Update_Face_Fanpage_Failure_Token(key_page) {
    if (key_page != '') {
        AjaxLoad(url = "/Facebook/Messenger/?handler=Update_LongToken_Failure"
            , data = { "keypage": key_page }, async = true, error = null
            , success = function (result) {

            }
        );
        
    }
}