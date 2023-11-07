
var facebook_link_detailconversation_byuser = "https://graph.facebook.com/{version}/{pageid}/conversations?fields=id&user_id={userid}";
//var facebook_link_detailconversation = "https://graph.facebook.com/{version}/{conversation_id}?fields=messages.limit({number}){to,from,created_time,message,id,sticker,tags,attachments}";
var facebook_link_detailconversation = "https://graph.facebook.com/{version}/{pageid}/conversations?fields=link,messages.limit({number}){created_time,to,from,message,id,attachments,sticker}&user_id={userid}";
var facebook_link_message = "https://graph.facebook.com/{version}/{message_id}?fields=message,created_time,to,attachments,sticker,from,id";
var facebook_link_comment_detail = "https://graph.facebook.com/{version}/{comment_id}?fields=message,comments{created_time,message,attachment,from,is_hidden,like_count,permalink_url},attachment,created_time,permalink_url,from,is_hidden,like_count";
var facebook_link_post = "https://graph.facebook.com/{version}/{post_id}?fields=full_picture,message,created_time,from,permalink_url";
var facebook_link_send_message = "https://graph.facebook.com/{version}/me/messages";
var facebook_link_get_info_client = "https://graph.facebook.com/{version}/{client}";
var facebook_link_upload_attach = "https://graph.facebook.com/{version}/me/message_attachments";
var facebook_link_executecomment = "https://graph.facebook.com/{version}/{comment_id}";
var facebook_link_send_comment = 'https://graph.facebook.com/{version}/{comment_id}/comments'
var facebook_link_blocked = "https://graph.facebook.com/{version}/{page_id}/blocked";
var facebook_link_client_info = "https://graph.facebook.com/{version}/{client_id}";


var face_data_chatting = [];
var face_data_comment = [];
var face_data_post = [];
var face_data_chatting_having_phone = [];


function Face_API_Get_Conversation_More() {
    numload_mess_current = numload_mess_current + numload_mess_step;
    let loadmore = 1;

    Face_API_Get_Conversation(numload_mess_current, loadmore);
}
function Face_API_Get_Conversation(number, loadmore) {
    if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
        xhrRequestFacebook.abort();
    }
    Face_SH_Begin_LoadDetail();
    face_data_chatting = [];
    face_data_chatting_having_phone = [];

    let url = facebook_link_detailconversation.replace("{pageid}", face_value_current_page).replace('{version}', facebook_version);
    url = url.replace("{number}", number);
    url = url.replace("{userid}", face_value_current_client);
    url = url + "&access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page);
 
    xhrRequestFacebook = $.ajax({
        url: url,
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (face_is_send_mess_from_comment != 0) {
                Face_SH_Complete_LoadDetail();
                $('#attachFileImg').hide();
            } else {
                // code :1 Cuộc trò chuyện không tồn tại, đóng tất cả phần chat...
                Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
                    xhrRequestFacebook.abort();
                }
                Face_SH_Click_Error();
            }
        },
        success: function (result) {
            if (result != undefined && result.data != undefined && result.data.length == 1) {
              
                let message = result.data[0].messages;
                facebook_link_inbox_mail = result.data[0].link;
                if (message != undefined && message.length != 0) {
                    let data = message.data;
                    for (i = 0; i < data.length; i++) {
                       
                        let e = {};
                        e.created_time = data[i].created_time;
                        e.from = data[i].from;
                        e.to = data[i].to.data[0];
                        e.message = data[i].message;
                        if (data[i].hasOwnProperty("attachments")) {
                            e.attachments = data[i].attachments.data;
                        }
                        else {
                            e.attachments = [];
                        }
                        if (data[i].hasOwnProperty("sticker")) {
                            e.sticker = data[i].sticker;
                        }
                        else {
                            e.sticker = "";
                        }

                        let phoneis = Face_Checking_String_Is_Phone(data[i].message);
                        if (phoneis != "") {
                            let p = {};
                            p.phone = phoneis;
                            p.created_time = data[i].created_time;
                            face_data_chatting_having_phone.push(p);
                        }
                        face_data_chatting.push(e);

                    }
                }
                Face_Render_Message_Having_Phone(face_data_chatting_having_phone, "facebook_getphone");
                Face_Event_Click_Create_Ticket();
                Face_Render_API_Conversation(face_data_chatting, "MessengerArea_Messenger", loadmore);
                Face_SH_Complete_LoadDetail();
            }
            else {
                if (face_is_send_mess_from_comment != 0) {
                    Face_SH_Complete_LoadDetail();
                } else {
                    // code :1 Cuộc trò chuyện không tồn tại, đóng tất cả phần chat...
                    Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                    if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
                        xhrRequestFacebook.abort();
                    }
                    Face_SH_Click_Error();
                }
            }
            face_is_send_mess_from_comment = 0;
            $('#MessengerArea_Reply_Comment').hide();
        }
    })

}
function Face_Error_API(code, content) {
    switch (Number(code)) {
        case 10:
            // (#10) Tin nhắn này được gửi ngoài khoảng thời gian cho phép
            break;
        case 1:
            // (#1) Mất kết nối
            break;
        default: break;
    }

}
function Face_SH_Complete_LoadDetail_Comment() {
    $("#MessengerArea_Messenger_loaderList").hide();
}
function Face_SH_Complete_LoadDetail() {
    $('#loadmoreDetail').show();
    $("#MessengerArea_Messenger_loaderList").hide();
}
function Face_SH_Begin_LoadDetail() {
    $('#loadmoreDetail').hide();
    $("#MessengerArea_Messenger_loaderList").show();
    $("#ParentMessengerArea_Attachment").hide();

}
function Face_Checking_String_Is_Phone(mes) {
    var twentyDigits = /\d{3}/;
    let re = "";
    if (twentyDigits.test(mes)) {
        mes = mes.split(" ").join("");
        mes = mes.split("+").join("");
        mes = mes.split(")").join("");
        mes = mes.split("(").join("");
        mes = mes.split(".").join("");
        mes = mes.split("-").join("");
        mes = mes.replace(/[^0-9]/g, "");
        if (!isNaN(mes) && mes.length > 8 && mes.length < 13)
            re = mes;
    }
    return re;
}
function Face_Render_API_Conversation(data, id, loadmore) {
    var myNode = document.getElementById(id);
    if (myNode != null) {

        let stringContent = '';
        if (data && data.length > 0) {
            let dateBegin = new Date();
            for (var i = data.length - 1; i >= 0; i--) {
                let tr = "";
                let item = data[i];
                tr = Face_Render_API_Conversation_Each(item)
                let dateLine = Face_Render_API_Conversation_Date(item.created_time, dateBegin);
                dateBegin = new Date(item.created_time);
                stringContent = stringContent
                    + dateLine
                    + tr
            }
        }

        document.getElementById(id).innerHTML = stringContent;
    }

    Face_ScrollContentTo_First_Last(loadmore);
    Face_Image_Chating_Click();
    Face_Image_Chating_Load();
    Face_Event_Show_Title();

}
function Face_Event_Show_Title() {
    $('.message_me,.message_client,.sticker_client,.sticker_me,.image_me,.video_me,.file_me,.image_client,.video_client,.file_client').qtip({ // Grab some elements to apply the tooltip to
        content: {
            text: function (event, api) {
                return $(this).attr('oldtitle');
            }
        }
    })
}
function Face_Render_API_Conversation_Each(item) {
    let sticker = item.sticker;
    let message = item.message;
    let attachments = item.attachments;
    let created_time = ConvertDateTimeUTC_Time(item.created_time);
    let created_time_onlyhour = ConvertDateTimeUTC_Time_OnlyHour(item.created_time);
    let timespan = ConvertDateTimeToTimeSpan(item.created_time);
    let resulf = "";
    let type = (face_value_current_page == item.from.id) ? "me" : "client";
    if (type == "me") {
        resulf =
            '<div id="' + timespan + '" class="messengerDetail_me" data_times="' + item.created_time + '">'
            + ((message == "") ? '' : ('<span class="message_me" title="' + created_time_onlyhour + '">' + message + '</span>'))
            + ((sticker == "") ? '' : ('<img class="sticker_me" title="' + created_time_onlyhour + '" src="' + sticker + '">'))
            + Face_Render_API_Conversation_Attachment(attachments, type, created_time, created_time_onlyhour)
            + '</div>'
    }
    else {
        resulf = '<div id="' + timespan + '" class="messengerDetail_client" data_times="' + item.created_time + '">'
            + ((message == "") ? '' : ('<span class="message_client" title="' + created_time_onlyhour + '">' + message + '</span>'))
            + ((sticker == "") ? '' : ('<img class="sticker_client" title="' + created_time_onlyhour + '" src="' + sticker + '">'))
            + Face_Render_API_Conversation_Attachment(attachments, type, created_time, created_time_onlyhour)
            + '</div>'
    }
    return resulf;
}
function Face_Render_API_Time_Send(type, created_time, timespan) {
    let ti_line = "";
    let created_time_onlyhour = ConvertDateTimeUTC_Time_OnlyHour(created_time);
    if (type == "me") {
        ti_line = '<div class="messengerDetail_me">'
            + '<span class="message_me message_me_time_send" data_time="' + created_time + '" title="Time" id="send_' + timespan + '">'
            + created_time_onlyhour + '</span>'
            + '</div>';
    }
    else {
        ti_line = '<div class="messengerDetail_client">'
            + '<span class="message_client message_client_time_send" data_time="' + created_time + '" title="Time" id="send_' + timespan + '">'
            + created_time_onlyhour + '</span>'
            + '</div>';
    }
    return ti_line;
}
function Face_Render_API_Conversation_Date(created_time, dateBegin) {
    var d = new Date(created_time);
    if (d.getDate() == dateBegin.getDate() && d.getYear() == dateBegin.getYear() && d.getMonth() == dateBegin.getMonth()) {
        var diff = Math.abs(d - dateBegin);
        if (diff > 900000) {
            return '<div class="linedate">' 
                + Face_Convert_Date_To_DOW_HM(d)
                + '</div>';
        }
        else return ''
    }
    else {
        return '<div class="linedate">' + Face_Convert_Date_To_DOW_DMY(d)+ '</div>';
    }
   

}
function Face_Render_API_Conversation_Attachment(attachments, type, created_time, created_time_onlyhour) {
    if (attachments == [] || attachments.length == 0) return "";
    else {
        let resultMain = '';
        let resultMain_Image = '';
        let classgroup = '';
        //Count size image
        let numberSizeImage = 200;
        let numberImage = 0;
        for (i = 0; i < attachments.length; i++) {
            if (attachments[i] != null) {
                let mime_type = attachments[i].mime_type;
                if (mime_type.includes('image')) numberImage = numberImage + 1;
            }
        }
        if (numberImage < 2) numberSizeImage = 200;
        else if (numberImage >= 2 && numberImage < 5) numberSizeImage = 100;
        else if (numberImage >= 5 && numberImage < 10) numberSizeImage = 70;
        else numberSizeImage = 50;


        for (i = 0; i < attachments.length; i++) {
            if (attachments[i] != null) {
                let mime_type = attachments[i].mime_type;
                let url = "";
                let preload_url = "/assests/img/Face/loading_image_face.gif";
                if (attachments[i].hasOwnProperty("file_url")) {
                    url = attachments[i].file_url;
                }
                if (mime_type.includes('image')) {
                    url = attachments[i].image_data.url;
                }
                if (mime_type.includes('video')) {
                    url = attachments[i].video_data.url;
                    preview_url = attachments[i].video_data.preview_url;
                }
                let name = decodeURI(attachments[i].name);
                let result = "";
                let result_image = "";

                if (type == "me") {
                    classgroup = "groupAttachment_me";
                    if (mime_type.includes('image'))
                        result_image = '<img class="image_me" style="width:' + numberSizeImage + 'px;height:' + numberSizeImage + 'px" data_render=0 data_link="' + url + '" oldtitle="' + created_time_onlyhour + '" src="' + preload_url + '">';
                    else if (mime_type.includes('video')) result = '<video class="video_me" oldtitle="' + created_time_onlyhour + '" controls>'
                        + '<source  data_render=0 class="video_me_source" type="' + mime_type + '" data_link="' + url + '"></video>';
                    else if (mime_type.includes('audio')) result = '<audio class="audio_me" oldtitle="' + created_time_onlyhour + '" controls>'
                        + '<source type="' + mime_type + '" src="' + url + '"></audio>';
                    else result = '<a class="file_me" oldtitle="' + created_time + '" href="' + url + '">' + '<i class="arrow down icon" style="display: contents;"></i>&nbsp;&nbsp;' + name + '</a>';
                }
                else {
                    classgroup = "groupAttachment_client";
                    if (mime_type.includes('image'))
                        result_image = '<img class="image_client" style="width:' + numberSizeImage + 'px;height:' + numberSizeImage + 'px" data_render=0 data_link="' + url + '" oldtitle="' + created_time_onlyhour + '" src="' + preload_url + '">';
                    else if (mime_type.includes('video')) result = '<video  class="video_client" oldtitle="' + created_time_onlyhour + '" controls>'
                        + '<source data_render=0 class="video_client_source" type="' + mime_type + '" data_link="' + url + '"></video>';
                    else if (mime_type.includes('audio')) result = '<audio class="audio_client" oldtitle="' + created_time_onlyhour + '" controls>'
                        + '<source type="' + mime_type + '" src="' + url + '"></audio>';
                    else result = '<a class="file_client" oldtitle="' + created_time + '" href="' + url + '">' + '<i class="arrow down icon" style="display: contents;"></i>&nbsp;&nbsp;' + name + '</a>';
                }

                resultMain = resultMain + result;
                resultMain_Image = resultMain_Image + result_image;
            }
        }
        return '<div class=" groupAttachment_top ' + classgroup + '"><div class="' + classgroup + '">' + resultMain_Image + '</div> </div>'
            + '<div class="' + classgroup + '">' + resultMain + '</div>';
    }
}
function Face_ScrollContentTo_First_Last(loadmore) {
    if (loadmore != undefined && loadmore == 1) {
        var objDiv = document.getElementById("MessengerArea_Messenger");
        objDiv.scrollTop = 0;
    }
    else {
        setTimeout(
            () => {
                var objDiv = document.getElementById("MessengerArea_Messenger");
                objDiv.scrollTop = objDiv.scrollHeight;
            }
        )
    }
}
function Face_Image_Chating_Click() {
    $(".image_me").unbind().click(function () {
        window.open(this.src);
        return;
    });
    $(".image_client").unbind().click(function () {
        window.open(this.src);
        return;
    });

}
function Face_Image_Chating_Load() {
    let _ob = document.getElementsByClassName('image_client');
    if (_ob != undefined) {
        for (ii_ = 0; ii_ < _ob.length; ii_++) {
            if (Number(_ob[ii_].attributes["data_render"].value) == 0) {

                _ob[ii_].src = _ob[ii_].attributes["data_link"].value;
                _ob[ii_].attributes["data_render"].value = 1;
            }

        }
    }

    _ob = document.getElementsByClassName('image_me');
    if (_ob != undefined) {
        for (ii_ = 0; ii_ < _ob.length; ii_++) {
            if (Number(_ob[ii_].attributes["data_render"].value) == 0) {

                _ob[ii_].src = _ob[ii_].attributes["data_link"].value;
                _ob[ii_].attributes["data_render"].value = 1;
            }

        }
    }

    _ob = document.getElementsByClassName('video_client_source');
    if (_ob != undefined) {
        for (ii_ = 0; ii_ < _ob.length; ii_++) {
            if (Number(_ob[ii_].attributes["data_render"].value) == 0) {

                _ob[ii_].src = _ob[ii_].attributes["data_link"].value;
                _ob[ii_].attributes["data_render"].value = 1;
            }

        }
    }
    _ob = document.getElementsByClassName('video_me_source');
    if (_ob != undefined) {
        for (ii_ = 0; ii_ < _ob.length; ii_++) {
            if (Number(_ob[ii_].attributes["data_render"].value) == 0) {

                _ob[ii_].src = _ob[ii_].attributes["data_link"].value;
                _ob[ii_].attributes["data_render"].value = 1;
            }

        }
    }

}
function Face_API_Send_MESSAGE() {
    if (face_value_current_mess_post == "COM") {
        Face_API_Send_COMMENT_Text();
    }
    if (face_value_current_mess_post == "MESS") {
        Face_API_Send_MESSAGE_Text();
        Face_API_Send_MESSAGE_Attachment();

        // Face_UploadAttachment_GetImageLink();
    }
}
function Face_API_Send_MESSAGE_Text() {
    let message = $("#txtContentMessage_Chatting").val().trim();
    $("#txtContentMessage_Chatting").val('');
    if (message != "" && face_value_current_client != "") {
        let recipient = {}
        if (face_is_send_mess_from_comment != 1) {
            recipient = {
                "id": face_value_current_client
            }
        } else {
            recipient = {
                "comment_id": face_value_current_comment
            }
        }
        let timespan = (new Date()).getTime();
        Face_Send_Temporary_Text(message, timespan, "MessengerArea_Messenger");
        $.ajax({
            url: facebook_link_send_message.replace('{version}', facebook_version),
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                "messaging_type": "RESPONSE",
                "recipient": JSON.stringify(recipient),
                "message": JSON.stringify({
                    "text": message
                }),
                "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
            }),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //(#551) Người này hiện không có mặt. ( chan )
                if (XMLHttpRequest.responseJSON.error.code == 10) {
                    Face_Error_API(10, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                    //CheckSendTag();
                    Face_API_Send_MESSAGE_Text_With_TagSend(message, timespan);
                }
                else {
                    Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                    if ($("#" + Number(timespan)).length) {
                        if (!$("#spantext_temp_" + Number(timespan)).hasClass('temporary_error')) {
                            $("#spantext_temp_" + Number(timespan)).addClass('temporary_error');
                            $("#spanimg_temp_" + Number(timespan)).attr("src", "/assests/img/Face/problem.png");

                        }
                    }
                }
            },
            success: function (result) {
                if (result != null && result != undefined) {
                    if (result.message_id != undefined && result.recipient_id != undefined && result.message_id != null && result.recipient_id != null) {
                        if (face_is_send_mess_from_comment != 1) {
                            let message_id = result.message_id;
                            Face_API_Get_Message_Detail("me", message_id, timespan);
                        } else {
                            Face_Trigger_Click(face_value_current_page + '-' + result.recipient_id);
                        }
                    }
                }
            },
            complete: function () {
                face_is_send_mess_from_comment = 0;
            }
        });
    }
}
function Face_API_Send_MESSAGE_Text_With_TagSend(message, timespan) {
    $.ajax({
        url: facebook_link_send_message.replace('{version}', facebook_version),
        dataType: "json",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            "messaging_type": "MESSAGE_TAG",
            "recipient": JSON.stringify({
                "id": face_value_current_client
            }),
            "message": JSON.stringify({
                "text": message
            }),
            "tag": face_tag_send,
            "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if ($("#" + Number(timespan)).length) {
                if (!$("#spantext_temp_" + Number(timespan)).hasClass('temporary_error')) {
                    $("#spantext_temp_" + Number(timespan)).addClass('temporary_error');
                    $("#spanimg_temp_" + Number(timespan)).attr("src", "/assests/img/Face/problem.png");

                }
            }
        },
        success: function (result) {
            if (result != null && result != undefined) {
                if (result.message_id != undefined && result.recipient_id != undefined && result.message_id != null && result.recipient_id != null) {
                    let message_id = result.message_id;
                    Face_API_Get_Message_Detail("me", message_id, timespan);
                }
            }
        }
    });
}
function Face_Send_Temporary_Text(message, timespan, id) {
    var childNode = document.getElementById(id).children;
    var parrent = document.getElementById(id);
    if (childNode != undefined) {
        let stringContent = '';
        if (message != undefined && message) {
            let tr = "";
            tr =
                '<div id="' + timespan + '"  class="messengerDetail_me">'
                + ((message == "") ? '' : ('<span id="spantext_temp_' + timespan + '" class="message_me message_me_temp" >'
                    + '<img title="Something wrongs" id="spanimg_temp_' + timespan + '" class="waitingfacebook_exe" src="/assests/img/Face/waiting_exe.gif" alt="label-image" />'
                    + message
                    + '</span>'))

                + '</div>';
            stringContent = tr;
        }
        if (stringContent != "") {
            let timespanNumber = (timespan != "") ? Number(timespan) : 0;
            let indexfind = 0;
            for (i = childNode.length - 1; i >= 0; i--) {
                let id = childNode[i].id;
                if (timespanNumber > id) {
                    indexfind = i;
                    i = 0;
                }
            }
            //if (childNode[indexfind] != undefined && childNode[indexfind] != null && $("#send_" + childNode[indexfind].id).length) {
            //    $("#send_" + childNode[indexfind].id).remove();
            //}
            var newelement = createElementFromHTML(stringContent);
            if (indexfind == childNode.length - 1) parrent.appendChild(newelement);
            else parrent.insertBefore(newelement, parrent[indexfind - 1]);

        }
    }
    Face_ScrollContentTo_First_Last();
}
function Face_Time_Checking_Distince(created_time, type) {
    try {
        let classname_timesend = '';
        if (type == "me") classname_timesend = 'message_me_time_send';
        else classname_timesend = 'message_client_time_send';
        let _y = document.getElementsByClassName(classname_timesend);
        if (_y != undefined && _y.length != 0) {
            let da_time = _y[_y.length - 1].attributes.data_time.value;
            var diff = Math.abs(new Date(created_time) - new Date(da_time));
            if (diff < 400000) {
                $("#" + _y[_y.length - 1].id).remove();
            }
        }
    } catch (exception) {

    }

}
function Face_API_Get_Message_Detail(type, message_id, timespan) {
    $.ajax({
        url: facebook_link_message.replace("{message_id}", message_id).replace('{version}', facebook_version) + "&access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page),
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
        success: function (result) {
            let e = {};
            let type = "text";
            let url = "";
            e.created_time = result.created_time;
            e.from = result.from;
            e.to = result.to.data[0];
            e.message = result.message;
            if (result.hasOwnProperty("attachments")) {
                e.attachments = result.attachments.data;
                if (result.attachments.data[0].hasOwnProperty("mime_type")) {
                    let mime_type = result.attachments.data[0].mime_type;
                    if (mime_type.includes('image')) {
                        type = "image";
                        url = result.attachments.data[0].image_data.url;
                    }
                    else if (mime_type.includes('excel') || mime_type.includes('pdf') || mime_type.includes('word')) {
                        type = "file";
                        url = "";
                    }
                }
            }
            else {
                e.attachments = [];
            }
            if (result.hasOwnProperty("sticker")) {
                e.sticker = result.sticker;
            }
            else {
                e.sticker = "";
            }
            face_data_chatting.push(e);
            Face_Update_Chatting_On_Design(type, e, result.created_time, timespan);
            Face_Send_Message_Sucessful(face_value_current_conver_id, result.from, type, result.message, url);

        },
        complete: function () {
            if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
            Face_ScrollContentTo_First_Last();
            Face_Image_Chating_Click();
            Face_Event_Show_Title();
        }
    })
}
function Face_Update_Chatting_On_Design(type, e, created_time, id_timespan) {
    Face_Time_Checking_Distince(created_time, type);
    var parrent = document.getElementById('MessengerArea_Messenger');
    var childNode = document.getElementById('MessengerArea_Messenger').children;

    var newelement = createElementFromHTML(Face_Render_API_Conversation_Each(e));
    //var timeline = createElementFromHTML(Face_Render_API_Time_Send(type, created_time, id_timespan));
    // Find index to insert

    let indexfind = 0;
    for (i = childNode.length - 1; i >= 0; i--) {
        let _created_time = (childNode[i].attributes.data_times != undefined) ? childNode[i].attributes.data_times.value : '';
        if (_created_time <= created_time && _created_time != '') {
            indexfind = i;
            i = 0;
        }
    }

    if (indexfind == childNode.length - 1) {
        parrent.appendChild(newelement);
        //parrent.appendChild(timeline);
    }
    else {
        parrent.insertBefore(newelement, childNode[indexfind + 1]);

    }
    Face_Image_Chating_Load();
}
function Face_Update_Conversation_Having_Phone(message, created) {
    let phoneis = Face_Checking_String_Is_Phone(message);
    if (phoneis != "") {
        let p = {};
        p.phone = phoneis;
        p.created_time = created;
        face_data_chatting_having_phone.push(p);
        let created_time = ConvertDateTimeUTC_Time_DOWFULLDAY(p.created_time);
        let obj = Face_Render_Message_Having_Phone_Each(created_time, phoneis);
        var parrent = document.getElementById("facebook_getphone");
        var newelement = createElementFromHTML(obj);
        if (parrent.children.length != 0) {
            parrent.insertBefore(newelement, parrent[0]);
        } else parrent.appendChild(newelement);
        Face_Event_Click_Create_Ticket();
    }
}

