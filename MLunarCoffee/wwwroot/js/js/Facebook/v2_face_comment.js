﻿function Face_API_Get_Comment_Post_And_Content() {
    document.getElementById('MessengerArea_Messenger').innerHTML =
        '<div id="MessengerArea_Messenger_Post_Area"></div>'+'<div id="MessengerArea_Messenger_Content_Area"></div>';
    Face_API_Get_Comment_Post();
    Face_API_Get_Comment();
}
function Face_API_Get_Comment_Post() {
    if (face_value_current_comment != "") {

        if (xhrRequestFacebook_Post && xhrRequestFacebook_Post.readyState != 4) {
            xhrRequestFacebook_Post.abort();
        }
        face_data_post = [];
        let url = facebook_link_post.replace("{post_id}", face_value_current_post).replace('{version}', facebook_version);
        url = url + "&access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page) ;
        xhrRequestFacebook_Post = $.ajax({
            url: url,
            dataType: "json",
            type: "GET",
            data: JSON.stringify({}),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
                    xhrRequestFacebook.abort();
                }
            },
            success: function (result) {
                if (result.hasOwnProperty('id')) {
                    let e = {};
                    if (result.hasOwnProperty("full_picture")) {
                        e.full_picture = result.full_picture;
                    }
                    else {
                        e.full_picture = "";
                    }
                    e.permalink_url = result.permalink_url;
                    e.created_time = result.created_time;
                    e.from_id = result.from.id;
                    e.from_name = result.from.name;
                    e.message = result.message;
                    face_data_post.push(e);
                    Face_Render_API_Post(face_data_post, "MessengerArea_Messenger_Post_Area");
                    Face_SH_Complete_LoadDetail_Comment();
                }
                else {
                    Face_Error_API(1, "Something wrongs in get object post");
                }
            },
            beforeSend: function () {
                Face_SH_Begin_LoadDetail();
            }
        })
    }
}

function Face_API_Get_Comment() {
    if (face_value_current_comment != "") {
        if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
            xhrRequestFacebook.abort();
        }
        face_data_comment = [];
        let urlcomment = facebook_link_comment_detail.replace("{comment_id}", face_value_current_comment).replace('{version}', facebook_version);
        urlcomment = urlcomment + "&access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page);
      
        xhrRequestFacebook = $.ajax({
            url: urlcomment,
            dataType: "json",
            type: "GET",
            data: JSON.stringify({}),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
                    xhrRequestFacebook.abort();
                }
                else {
                    // Comment is deleted
                    Face_Error_API(100, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                    Face_SH_Click_Error();
                }
            },
            success: function (result) {
                let e = {};
                e.permalink_url = result.permalink_url;
                e.created_time = result.created_time;
                if (result.from != undefined) {
                    e.from_id = result.from.id;
                    e.from_name = result.from.name;
                }
                else {
                    e.from_id = "";
                    e.from_name = "";
                }
                e.message = result.message;
                e.is_hidden = result.is_hidden;
                e.like_count = result.like_count;

                if (result.hasOwnProperty("attachment")) {
                    e.type = result.attachment.type;
                    switch (result.attachment.type) {
                        case "animated_image_share":
                        case "photo":
                        case "sticker":
                            e.url = result.attachment.media.image.src;
                            break;
                        default:
                            e.url = "";
                            break;
                    }


                }
                else {
                    e.type = "";
                    e.url = "";
                }
                e.id = result.id;
                face_data_comment.push(e);
                if (result.hasOwnProperty("comments")) {
                    let obj_data = result.comments.data;
                    if (obj_data != undefined && obj_data.length != 0)
                        for (k = 0; k < obj_data.length; k++) {
                            let e_sub = {};
                            e_sub.permalink_url = obj_data[k].permalink_url;
                            e_sub.created_time = obj_data[k].created_time;
                            e_sub.from_id = obj_data[k].from.id;
                            e_sub.from_name = obj_data[k].from.name;
                            e_sub.message = obj_data[k].message;
                            e_sub.is_hidden = obj_data[k].is_hidden;
                            e_sub.like_count = obj_data[k].like_count;
                            if (obj_data[k].hasOwnProperty("attachment")) {
                                e_sub.type = obj_data[k].attachment.type;
                                switch (obj_data[k].attachment.type) {
                                    case "animated_image_share":
                                    case "photo":
                                    case "sticker":
                                        e_sub.url = obj_data[k].attachment.media.image.src;
                                        break;
                                    default:
                                        e_sub.url = "";
                                        break;
                                }
                            }
                            else {
                                e_sub.type = "";
                                e_sub.url = "";
                            }

                            e_sub.id = obj_data[k].id;
                            face_data_comment.push(e_sub);
                        }
                }
                Face_Render_API_Comment(face_data_comment, "MessengerArea_Messenger_Content_Area");
            }
        })
    }
}
function Face_Render_API_Post(datapost, id) {
    var myNode = document.getElementById(id);
    face_value_current_post_link = '#';
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (datapost && datapost.length > 0) {
            let created_time = ConvertDateTimeUTC_Time(datapost[0].created_time);
            face_value_current_post_link = datapost[0].permalink_url;
            let pic = '//graph.facebook.com/' + datapost[0].from_id + '/picture?access_token=' + Face_Get_Token_Access_From_PageID(face_value_current_page);
            stringContent =
                '<div class="postDetail_me">'
                + '<div class="divPostInMessage">'
                + ((datapost[0].full_picture == "") ? '' : ('<img class="image_me" src="' + datapost[0].full_picture + '" alt="label-image" />'))
                + ((datapost[0].message == "") ? '' : ('<span class="postmessage_me" title="' + created_time + '">' + datapost[0].message + '</span>'))
                + '<div class="postmessage_me postmessage_me_name">'
            + '<img style="margin-bottom: 4px;display: inline;" class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
                + datapost[0].from_name
                + '<br>'
            + '<a href="' + datapost[0].permalink_url + '" target="_blank"><img style="height: 16px;margin-right: 10px;margin-left: -1px;margin-bottom: -4px;" title="Link comment" src="/assests/img/Face/face_icon.png"></a>'
                + created_time
                + '</div>'
                + '</div>'
                + '</div>';
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Render_API_Comment(datacomment, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (datacomment && datacomment.length > 0) {
            for (i = 0; i < datacomment.length; i++) {
                let item = datacomment[i];
                let tr = Face_Render_API_Comments_Each(item, ((i == 0) ? 1 : 0));
                let dateline = Face_Render_API_Comments_Time(item.from_id, item.created_time);
                stringContent = stringContent + tr + dateline;
            }
        }

        document.getElementById(id).innerHTML = stringContent;
    }



    Face_Comment_Click_Hidden();
    Face_Comment_Click_Delete();
    Face_Comment_Click_Edit();
    Face_ScrollContentTo_First_Last();
    Face_Image_Chating_Click();
    Face_Event_Show_Title();


}
function Face_Render_API_Comments_Time(from_id, created_time) {
    let result = '';
    let pic = '';
    let created_time_onlyhour = ConvertDateTimeUTC_Time_DOWFULLDAY(created_time);
    if (face_value_current_page != from_id) {
    
        if (from_id == '') {
            pic = '/assests/img/Face/loading_avatar.gif';
        }
        else {
            pic = '//graph.facebook.com/' + from_id + '/picture?access_token=' + Face_Get_Token_Access_From_PageID(face_value_current_page);
        }
    }
    else {
        pic = face_link_image_fanpage.replace('{keycode}', from_id);
    }
    if (face_value_current_page != from_id) {
        result = '<div style="display: flow-root;padding-bottom: 3px; padding-left: 17px;">'
            + '<img style="display: inline;" class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
            + '<span style="font-size: 12px;">' + created_time_onlyhour + '</span>'
            + '</div></br></br>'
    } else {
        result = '<div style="display: flow-root;padding-bottom: 3px;padding-right: 11px;float: right;">'
            + '<span style="font-size: 12px;padding-right: 10px;">' + created_time_onlyhour + '</span>'
            + '<img style="margin-bottom: 5px;float: right;" class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
            + '</div></br></br>'
    }
    return result;
}
function Face_Render_API_Comments_Each(item, first) {
    let timespan = ConvertDateTimeToTimeSpan(item.created_time);
    let isHidden = item.is_hidden;
    let message = item.message;
    let comment_id = item.id;
    let type = item.type;
    let url = item.url;
     let permalink_url = item.permalink_url;
    let created_time_onlyhour = ConvertDateTimeUTC_Time_DOWFULLDAY(item.created_time);
    let resulf = "";
    let classname = "";
    if (item.from_id == face_value_current_page) {
        classname = (isHidden) ? "message_me me_hidden __comment_me_face_" : "message_me __comment_me_face_";
        resulf =
            '<div id="' + timespan + '" class="messengerDetail_me" >'
            + '<div id="com_div_' + comment_id + '" class="' + classname + '" title="' + created_time_onlyhour + '">'
            + '<span id="com_' + comment_id + '">' + message + '</span>'
            + ((url == "") ? '' : ('<img class="sticker_me_comment"  src="' + url + '">'))
            + '<span class="comment_button_div">'
            + '<a class="commentedit_a" href="#"  id="edit_' + comment_id + '"><img id="imgEdit_' + comment_id + '" class="commentdelete commentdelete_img" src="/assests/img/Face/edit.png">' + '</a>'
            + '<a class="commentdelete_a" href="#" data_first="' + first + '"  id="del_' + comment_id + '"><img id="imgDelete_' + comment_id + '" class="commentdelete commentdelete_img" src="/assests/img/Face/nodelete_w.png">' + '</a>'
            + ((permalink_url != "") ? ('<a href="' + permalink_url + '" target="_blank"><img class="commentdelete commentdelete_img"  title="Link comment" src="/assests/img/Face/face_w.png"></a>') : '')
            + '</span>'

            + '</div>'
            + '</div>'
    }
    else {
        classname = (isHidden) ? "message_client client_hidden __comment_client_face_" : "message_client __comment_client_face_";
        resulf =
            '<div id="' + timespan + '" class="messengerDetail_client">'
            + '<div id="com_div_' + comment_id + '" class="' + classname + '" title="' + created_time_onlyhour + '">'
            + '<span id="com_' + comment_id + '">' + message + '</span>'
            + ((url == "") ? '' : ('<img class="sticker_client_comment"  src="' + url + '">'))
            + '<span class="comment_button_div">'
            + '<a class="commentdelete_a" href="#" data_first="' + first + '"  id="del_' + comment_id + '"><img id="imgDelete_' + comment_id + '" class="commentdelete commentdelete_img" src="/assests/img/Face/nodelete_b.png">' + '</a>'
            + ((permalink_url != "") ? ('<a href="' + permalink_url + '" target="_blank"><img class="commentdelete commentdelete_img"  title="Link comment" src="/assests/img/Face/face_b.png"></a>') : '')
            + '<a class="commenthidden" title="' + ((isHidden) ? 'Unhide' : 'Hide') + '" id="hidcom_' + comment_id + '" href="#"><img id="imghidcom_' + comment_id + '" class="commentdelete commentdelete_img" src="' + ((isHidden) ? '/assests/img/Face/nohide.png' : '/assests/img/Face/hide.png') + '">' + '</a>'

            + '</span>'
            + '</div>'
            + '</div>'
    }
    return resulf;
}
function Face_Comment_Click_Hidden() {
    $(".commenthidden").click(function () {
        let status;
        let idHidden = this.id;
        let id = idHidden.replace("hidcom_", '');
        let idimg = "imghidcom_" + id;
        let idcom = "com_div_" + id;
        if (this.title == "Unhide") {
            status = false;
        }
        else {
            status = true;
        }

        let url = facebook_link_executecomment.replace('{version}', facebook_version);
        url = url.replace('{comment_id}', id);
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                "is_hidden": status,
                "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
            }),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                Face_Error_API(1, "Cannot hide or unhide this comment");
            },
            success: function (result) {
                if (result != null && result != undefined && result.success) {
                    if (result.success) {
                        if (status == false) {
                            $('#' + idcom).removeClass('client_hidden');
                            $('#' + idHidden)[0].title = "Hide";
                            $('#' + idimg).attr("src", "/assests/img/Face/hide.png");

                        }
                        else {
                            $('#' + idcom).addClass('client_hidden');
                            $('#' + idHidden)[0].title = "Unhide";
                            $('#' + idimg).attr("src", "/assests/img/Face/nohide.png");
                        }
                    }
                }
            },
            beforeSend: function () {
                $('#' + idimg).attr("src", "/assests/img/Face/waiting_exe.gif");
            }
        });
    });
}
function Face_Comment_Click_Delete() {
    $(".commentdelete_a").click(function () {
        const promise = notiConfirm();
        let idDelete = this.id;
        let first = $(this).attr("data_first");
        promise.then(function () { Face_Comment_Click_Delete_Confirm(idDelete, first); }, function () { });
    });
}
function Face_Comment_Click_Delete_Confirm(idDelete, first) {
    let id = idDelete.replace("del_", '');
    let idimg = "imgDelete_" + id;
    let url = facebook_link_executecomment.replace('{version}', facebook_version);
    url = url.replace('{comment_id}', id);
    $.ajax({
        url: url,
        dataType: "json",
        type: "DELETE",
        contentType: "application/json",
        data: JSON.stringify({
            "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Face_Error_API(1, "Cannot delete this comment");
        },
        success: function (result) {
            if (result != null && result != undefined && result.success) {
                if (result.success) {
                    $('#' + idimg).attr("src", "/assests/img/Face/deleted.png");
                    $('#' + idimg).closest(".message_me").css('background-color', '#07283e');
                    $('#' + idimg).closest(".commentdelete_a").unbind("click");
                    if (first == "1") {
                        //Face_DeleteComment_ToDB(id);
                        //$("#MessengerArea_Chating").hide();
                        //$("#MessengerArea_Chating_Hide").show();
                    }
                }
            }
        },
        beforeSend: function () {
            $('#' + idimg).attr("src", "/assests/img/Face/waiting_exe.gif");
        }
    });
}
function Face_DeleteComment_ToDB(id) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Delete_First_Comment"
        , data = { 'comment': face_selected_commentid, 'post': face_selected_postid  }, async = true, error = null
        , success = function (result) {
            $('#deletehind_' + id).show();
        }
    ); 
}
function Face_Comment_Click_Edit() {
    $(".commentedit_a").click(function () {
        let idEdit = this.id;
        let id = idEdit.replace('edit_', '');
        face_value_current_comment_edit = id;
        let idCom = "com_" + id;

        let mes = (document.getElementById(idCom).childNodes[0] != undefined) ?
            document.getElementById(idCom).childNodes[0].textContent : '';
        if ($('#MessengerArea_Chating_Hide').css('display') == 'none') {
            if ($('#MessengerArea_Chating_Edit').css('display') == 'none') {
                $("#MessengerArea_Chating_Edit").show();
                $("#MessengerArea_Chating").hide();
                $("#txtContentMessage_Chatting_Edit").val(mes);
            }
            else {
                $("#MessengerArea_Chating_Edit").hide();
                $("#MessengerArea_Chating").show();
                $("#txtContentMessage_Chatting_Edit").val('');
            }
        }
        $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/assests/img/Face/waiting_exe.gif");
    });
}
function Face_Comment_Click_Edit_OK() {
    let message = $("#txtContentMessage_Chatting_Edit").val();
    let url = facebook_link_executecomment.replace('{version}', facebook_version);
    url = url.replace('{comment_id}', face_value_current_comment_edit);
    let idCom = "com_" + face_value_current_comment_edit;
    $.ajax({
        url: url,
        dataType: "json",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            "message": message,
            "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError("Cannot edit this comment. Try again later");
        },
        success: function (result) {
            if (result != null && result != undefined && result.success) {
                if (result.success) {
                    $("#MessengerArea_Chating_Edit").hide();
                    $("#MessengerArea_Chating").show();
                    $("#txtContentMessage_Chatting_Edit").val('');
                    $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/assests/img/Face/edit.png");
                    face_value_current_comment_edit = "";
                    document.getElementById(idCom).childNodes[0].textContent = message;

                    //document.getElementById('snippet_' + face_selected_commentid).innerHTML = message.substring(0, 30);
                    //let pic = '//graph.facebook.com/' + facebook_pageid + '/picture?access_token=' + facebook_accessToken;
                    //$('#smallavatar_' + face_selected_commentid).attr("src", pic);
                }
            }
        }
    });
}
function Face_Comment_Click_Edit_Cancel() {
    $("#MessengerArea_Chating_Edit").hide();
    $("#MessengerArea_Chating").show();
    $("#txtContentMessage_Chatting_Edit").val('');
    $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/assests/img/Face/edit.png");
    face_value_current_comment_edit = "";

}
function Face_API_Send_COMMENT_Text() {
    let message = $("#txtContentMessage_Chatting").val().trim();
    $("#txtContentMessage_Chatting").val('');
    if (message != "" && face_value_current_comment != "") {
        let timespan = (new Date()).getTime();
        Face_Send_Temporary_Comment_Text(message, timespan, "MessengerArea_Messenger");
        let url = facebook_link_send_comment;
        url = url.replace('{comment_id}', face_value_current_comment).replace('{version}', facebook_version)
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                "message": message,
                "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
            }),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // co loi 200 (#200) The permission(s) pages_manage_posts are no… deprecated or need to be approved by App Review.
                if (XMLHttpRequest.responseJSON.error.code == 10) {
                    Face_Error_API(10, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                    //CheckSendTag();
                }
                else {
                    Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                }

                //if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();

            },
            success: function (result) {
                if (result != null && result != undefined) {
                    if (result.id != undefined && result.id != null) {
                        let id = result.id;
                        Face_API_Get_Comment_Detail(id, timespan);
                        Face_Send_Comment_Sucessful(face_value_current_conver_id, face_value_current_page, message);
                    }
                    else {
                        notiError("Error Comment");
                    }
                }
            }
        });
    }
}
function Face_Send_Temporary_Comment_Text(message, timespan, id) {
    var childNode = document.getElementById(id).children;
    var parrent = document.getElementById(id);
    if (childNode != undefined) {
        let stringContent = '';
        if (message != undefined && message) {
            let tr = "";
            tr =
                '<div id="' + timespan + '"  class="messengerDetail_me">'
                + ((message == "") ? '' : ('<span class="message_me message_me_temp" >'
                    + '<img class="waitingfacebook_exe" src="/assests/img/Face/waiting_exe.gif" alt="label-image" />'
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
            var newelement = createElementFromHTML(stringContent);
            if (indexfind == childNode.length - 1) parrent.appendChild(newelement)
            else parrent.insertBefore(newelement, parrent[indexfind - 1]);

        }
    }
    Face_ScrollContentTo_First_Last();
}
function Face_API_Get_Comment_Detail(comment_id, timespan) {
    $.ajax({
        url: facebook_link_comment_detail.replace("{comment_id}", comment_id).replace('{version}', facebook_version)
            + "&access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page),
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        success: function (result) {
            let e_sub = {};
            e_sub.permalink_url = result.permalink_url;
            e_sub.created_time = result.created_time;
            e_sub.from_id = result.from.id;
            e_sub.from_name = result.from.name;
            e_sub.message = result.message;
            e_sub.is_hidden = result.is_hidden;
            e_sub.like_count = result.like_count;
            e_sub.type = "";
            e_sub.url = "";
            e_sub.id = result.id;
            face_data_comment.push(e_sub);

            let timeline = Face_Render_API_Comments_Time(result.from.id, result.created_time);
            let stringContent = document.getElementById('MessengerArea_Messenger').innerHTML;
            stringContent = stringContent + Face_Render_API_Comments_Each(e_sub) + timeline;
            document.getElementById('MessengerArea_Messenger').innerHTML = stringContent;

           
        },
        complete: function () {
            if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
            Face_Comment_Click_Hidden();
            Face_Comment_Click_Delete();
            Face_Comment_Click_Edit();
            Face_ScrollContentTo_First_Last();
            Face_Image_Chating_Click();
            Face_Event_Show_Title();
        }
    });
}
function Face_Goto_FacePost_Link() {
    if (face_value_current_mess_post == "COM") {
        window.open(face_value_current_post_link);
    }
}
function Face_Goto_MESSAGE_From_Post() {
    let id = face_value_current_page + '-' + face_value_current_client;
    if ($("#" + id).length) {
        Face_Exchange_Move_Mess_To_Top(id, face_value_current_page, face_value_current_client);
        Face_Event_Conversation_List_Click();
        Face_Trigger_Click(id);
    }
    else {
        AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Checking_Mess_Is_Exist"
            , data = { 'page': face_value_current_page, 'client': face_value_current_client }, async = true
            , error = function (XMLHttpRequest, textStatus, errorThrown) {
                Face_Create_New_Obj_Mess_Empty_From_Comment();
            }
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length == 0) {
                        Face_Create_New_Obj_Mess_Empty_From_Comment()
                    }
                    else {
                        let r = Face_Create_New_Obj_FromData(data[0]);
                        Face_Exchange_Create_Mess_To_Top(r.id, r.e);
                        Face_Event_Conversation_List_Click();
                        Face_Trigger_Click(r.id);
                    }
                } else {
                    Face_Create_New_Obj_Mess_Empty_From_Comment();
                }
            }
        );
         
    }
    $('#MessengerArea_Reply_Comment').show();
    face_is_send_mess_from_comment = 1;
}

