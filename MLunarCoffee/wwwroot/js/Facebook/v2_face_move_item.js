
// Tạo mới từ obj, Đưa lên top, cập nhật time thành thời gian hiện tại
function Face_Exchange_Create_Mess_To_Top(id, e) {

    let item = Face_Render_Conversation_List_Each(id, e);
    $("#MessengerArea_List").prepend(item);
    Face_Get_Name_Client();
}

// Đưa lên top, cập nhật time thành thời gian hiện tại
function Face_Exchange_Move_Mess_To_Top(id) {
    if ($("#" + id).length) {
        $("#" + id).prependTo("#MessengerArea_List");
        $("#_snipTime_" + id).html(ConvertDateTimeUTC_NoYear(new Date()));
    }
}
// Cập nhật snipset, người gửi, giờ gửi
function Face_Exchange_Update_Item_Update_Person_Do(id, fromid, type, message, urlitem) {
    if (face_value_selected_page_string.includes(fromid)) {
        // Trang gửi
        $("#smallavatar_client_" + id).hide();
        $("#smallavatar_page_" + id).show();
    }
    else {
        // Khách gửi
        $("#smallavatar_client_" + id).show();
        $("#smallavatar_page_" + id).hide();
    }
    let snippet;
    let url = "";
    switch (type) {
        case "text":
            snippet = (message.length > 30 ? message.substring(0, 30) : message);
            url = '/assests/img/Face/more_16.png';
            break;
        case "image":
            snippet = "";
            url = urlitem;
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
    $("#_text_snippet_" + id).html(snippet);
    $("#_img_snippet_" + id).attr("src", url);

}

function Face_Exchange_Update_Item_Update_Person_Do_Comment(id, fromid, message) {
    if (face_value_selected_page_string.includes(fromid)) {
        // Trang gửi
        $("#smallavatar_client_" + id).hide();
        $("#smallavatar_page_" + id).show();
    }
    else {
        // Khách gửi
        $("#smallavatar_client_" + id).show();
        $("#smallavatar_page_" + id).hide();
    }
    snippet = (message != undefined) ? ((message.length > 30 ? message.substring(0, 30) : message)) :"New comment";
    $("#_text_snippet_" + id).html(snippet);

}

// Đưa lên tạm thời, khi mở cuộc trò chuyện
function Face_Exchange_Move_Mess_To_Top_Temporary(id, page, client) {
    if ($("#" + id).length) {
        $("#" + id).prependTo("#MessengerArea_List");
        $("#_snipTime_" + id).html(ConvertDateTimeUTC_NoYear(new Date()));
    }
}


function Face_Trigger_Click(id) {
    if ($("#" + id).length) {
        $("#" + id).trigger("click");
        Face_Css_When_Click(id);
    }
}
function Face_Css_When_Click(id) {
    let _inbox = document.getElementsByClassName('messegerItem');
    if (_inbox != undefined) {
        for (i = 0; i < _inbox.length; i++) {
            if (_inbox[i].className.includes("activeitemface")) _inbox[i].className = _inbox[i].className.replace("activeitemface", "")
        }
    }
    let _comment = document.getElementsByClassName('messegerCommentItem');
    if (_comment != undefined) {
        for (i = 0; i < _comment.length; i++) {
            if (_comment[i].className.includes("activeitemface")) _comment[i].className = _comment[i].className.replace("activeitemface", "")
        }
    }
    $("#" + id).addClass("activeitemface");
}
function Face_Exchange_Update_Item_Unread_To_Read(id) {
    if ($("#" + id).hasClass('newMessage')) {
        $("#" + id).removeClass('newMessage');
    }
}
function Face_Exchange_Update_Item_Read_To_Unread(id) {
    if (!$("#" + id).hasClass('newMessage')) {
        $("#" + id).addClass('newMessage');
    }
}
function Face_Update_Read_To_Unread() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Make_Read_To_UnRead"
        , data = {
            'page': face_value_clicked_page
            , 'client': face_value_clicked_client
            , 'comment': (face_value_clicked_comment != undefined) ? face_value_clicked_comment : ""
            , 'type': face_value_clicked_mess_post
        }, async = true
        , error = function () { notiError(); }
        , success = function (result) {
            if (result != "0") {
                Face_Exchange_Update_Item_Read_To_Unread(face_value_clicked_conver_id);
            }
        }
    );
     
}
function Face_Update_UnRead_To_Read() {
    let client_name = $('#clientname_' + face_value_current_conver_id).html();

    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Make_UnRead_To_Read"
        , data = {
            'page': face_value_current_page
            , 'client': face_value_current_client
            , 'client_name': client_name
            , 'comment': (face_value_current_comment != undefined) ? face_value_current_comment : ""
            , 'type': face_value_current_mess_post
        }, async = true
        , error = function () { notiError(); }
        , success = function (result) {
            if (result != "0") {
                Face_Exchange_Update_Item_Unread_To_Read(face_value_current_conver_id);
            }
        }
    );
     
}
function Face_Update_Star() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Make_Star"
        , data = {
            'page': face_value_clicked_page, 'client': face_value_clicked_client
        }, async = true
        , error = function () { notiError(); }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data == "1") {
                    $("#star_" + face_value_clicked_conver_id).show();
                }
                else {
                    $("#star_" + face_value_clicked_conver_id).hide();
                }
            }
        }
    );
     
}

function Face_Create_New_Obj_FromData(obj) {
    let result = {};
    let e = {};
    e.page = obj.page;
    e.client = obj.client;
    e.client_name = obj.client_name;
    e.type = obj.type;
    e.message = obj.message;
    e.created = obj.created;
    e.updated = obj.updated;
    e.readtime = obj.readtime;
    e.client_to_server = obj.client_to_server;
    e.staff = obj.staff;
    e.tag = obj.tag;
    e.servicecare = obj.servicecare;
    e.discount = obj.discount;
    e.area = obj.area;
    e.url = obj.url;
    e.callbacktime = obj.callbacktime;
    e.ticket = obj.ticket;
    e.star = obj.star;
    e.typeconver = obj.typeconver;
    e.staffname = obj.staffname;
    e.discountname = obj.discountname;
    e.verb = obj.verb;
    e.post_id = obj.post_id;
    e.comment_id = obj.comment_id;
    e.lastexecute = obj.lastexecute;
    let id = (obj.typeconver == 'mess')
        ? (obj.page + '-' + obj.client)
        : (obj.page + '-' + obj.post_id + '-' + obj.comment_id);
    face_data_conversation_list[id] = e;
    result.id = id;
    result.e = e;
    return result;
}
function Face_Create_New_Obj_Mess_Empty_From_Comment() {
    let dateNow = new Date();
    let dateMin = new Date();
    dateMin.setDate(dateMin.getDate() - 1);
    let e = {};
    let id = face_value_current_page + '-' + face_value_current_client;
    e.page = face_value_current_page;
    e.client = face_value_current_client;
    e.client_name = "";
    e.type = "text";
    e.message = "";
    e.created = dateNow;
    e.updated = dateNow;
    e.readtime = dateNow;
    e.client_to_server = 0;
    e.staff = 0;
    e.tag = '';
    e.servicecare = '';
    e.discount = 0;
    e.area = 0;
    e.callbacktime = dateMin;
    e.ticket = 0;
    e.star = 0;
    e.typeconver = 'mess';
    e.staffname = '';
    e.discountname = '';
    e.verb = '';
    e.url = '';
    e.post_id = '';
    e.comment_id = '';
    e.lastexecute = '';
    face_data_conversation_list[id] = e;
    Face_Exchange_Create_Mess_To_Top(id, e);
    Face_Event_Conversation_List_Click();
    Face_Trigger_Click(id);

}
function Face_Scroll_Conversation_ToLast(loadmore) {
    if (loadmore == undefined || loadmore == 0) {
        var objDiv = document.getElementById("MessengerArea_List");
        objDiv.scrollTop = 0;
    }
    else {
        setTimeout(
            () => {
                var objDiv = document.getElementById("MessengerArea_List");
                objDiv.scrollTop = objDiv.scrollHeight;
            }
        )
    }

}

function Face_Update_When_Comment_Successful(message) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Update_When_Comment_Successful"
        , data = {
            'page': face_value_current_page
            , 'client': face_value_current_client
            , 'comment': (face_value_current_comment != undefined) ? face_value_current_comment : ""
            , 'message': message
        }, async = true
        , error = null
        , success = function (result) {
            
        }
    );
    
}
function Face_Update_When_Message_Successful(message,type,url) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Update_When_Message_Successful"
        , data = {
            'page': face_value_current_page
            , 'client': face_value_current_client
            , 'type': type
            , 'url': url
            , 'message': message
        }, async = true
        , error = null
        , success = function (result) {

        }
    );
    
}

function Face_Send_Comment_Sucessful(id,fromid,message) {
    Face_Exchange_Move_Mess_To_Top(id);
    Face_Exchange_Update_Item_Update_Person_Do_Comment(id, fromid, message);
    Face_Update_When_Comment_Successful(message);
}

function Face_Send_Message_Sucessful(id, fromid, type, message, urlitem) {
    Face_Exchange_Move_Mess_To_Top(id);
    Face_Exchange_Update_Item_Update_Person_Do(id, fromid, type, message, urlitem);
    Face_Update_When_Message_Successful(message, type, urlitem);
}






jQuery.fn.outerHTML = function () {
    return jQuery('<div />').append(this.eq(0).clone()).html();
};
