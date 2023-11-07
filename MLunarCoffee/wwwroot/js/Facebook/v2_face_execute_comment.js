

function Face_API_Get_Comment() {
    if (face_value_current_comment != "") {
        if (xhrRequestFacebook && xhrRequestFacebook.readyState != 4) {
            xhrRequestFacebook.abort();
        }
        face_data_post = [];
        face_data_comment = [];
        let url = facebook_link_post.replace("{post_id}", face_value_current_post).replace('{version}', facebook_version);
        url = url + "&access_token=" + face_long_key;
        let urlcomment = facebook_link_comment_detail.replace("{comment_id}", face_value_current_comment).replace('{version}', facebook_version);
        urlcomment = urlcomment + "&access_token=" + face_long_key;

        xhrRequestFacebook = $.ajax({
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
                    Face_Render_API_Post(face_data_post, "MessengerArea_Messenger");
                    Face_SH_Complete_LoadDetail();
                    $.ajax({
                        url: urlcomment,
                        dataType: "json",
                        type: "GET",
                        data: JSON.stringify({}),
                        contentType: 'application/json; charset=utf-8',
                        async: true,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            // Comment is deleted
                            Face_Error_API(100, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                        },
                        success: function (result) {
                            let e = {};
                            e.permalink_url = result.permalink_url;
                            e.created_time = result.created_time;
                            e.from_id = result.from.id;
                            e.from_name = result.from.name;
                            e.message = result.message;
                            e.is_hidden = result.is_hidden;
                            e.like_count = result.like_count;
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
                                        e_sub.id = obj_data[k].id;
                                        face_data_comment.push(e_sub);
                                    }
                            }
                            Face_Render_API_Comment(face_data_comment, "MessengerArea_Messenger");
                        }
                    })
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
function Face_Render_API_Post(datapost, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (datapost && datapost.length > 0) {
            let created_time = ConvertDateTimeUTC_Time(datapost[0].created_time);
            let pic = '//graph.facebook.com/' + datapost[0].from_id + '/picture?access_token=' + face_long_key;
            stringContent =
                '<div class="postDetail_me">'
                + '<div class="divPostInMessage">'
                + ((datapost[0].full_picture == "") ? '' : ('<img class="image_me" src="' + datapost[0].full_picture + '" alt="label-image" />'))
                + ((datapost[0].message == "") ? '' : ('<span class="postmessage_me" title="' + created_time + '">' + datapost[0].message + '</span>'))
                + '<div class="postmessage_me postmessage_me_name">'
                + '<img style="margin-bottom: 4px;" class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
                + datapost[0].from_name
                + '<br>'
                + '<a href="' + datapost[0].permalink_url + '" target="_blank"><img style="height: 16px;margin-right: 10px;margin-left: -1px;margin-bottom: -4px;" title="Link comment" src="/img/Face/face_b.png"></a>'
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
        let stringContent = '';
        if (datacomment && datacomment.length > 0) {
            for (i = 0; i < datacomment.length; i++) {
                let item = datacomment[i];


                let tr = Face_Render_API_Comments_Each(item, ((i == 0) ? 1 : 0));
                let dateline = Face_Render_API_Comments_Time(item.from_id, item.created_time);
                stringContent = stringContent + tr + dateline;
            }
        }

        document.getElementById(id).innerHTML = document.getElementById(id).innerHTML + stringContent;
    }



    Face_Comment_Click_Hidden();
    Face_Comment_Click_Delete();
    Face_Comment_Click_Edit();
    Face_ScrollContentToLast();
    Face_Image_Chating_Click();
    Face_Event_Show_Title();


}
function Face_Render_API_Comments_Time(from_id, created_time) {
    let result = '';
    let pic = '';
    let created_time_onlyhour = ConvertDateTimeUTC_Time_DOWFULLDAY(created_time);
    if (face_value_current_page != from_id) {
        pic = '//graph.facebook.com/' + from_id + '/picture?access_token=' + face_long_key;
    }
    else {
        pic = face_link_image_fanpage.replace('{keycode}', from_id);
    }
    if (face_value_current_page != from_id) {
        result = '<div style="display: flow-root;padding-bottom: 3px; padding-left: 17px;">'
            + '<img  class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
            + '<span style="font-size: 12px;">' + created_time_onlyhour + '</span>'
            + '</div></br></br>'
    } else {
        result = '<div style="display: flow-root;padding-bottom: 3px;padding-right: 11px;float: right;">'
            + '<span style="font-size: 12px;padding-right: 10px;">' + created_time_onlyhour + '</span>'
            + '<img style="margin-bottom: 5px;" class="ui mini circular image pageAvatarsmall" src="' + pic + '" alt="label-image" />'
            + '</div></br></br>'
    }
    return result;
}
function Face_Render_API_Comments_Each(item, first) {
    let timespan = ConvertDateTimeToTimeSpan(item.created_time);
    let isHidden = item.is_hidden;
    let message = item.message;
    let comment_id = item.id;
    let permalink_url = item.permalink_url;
    let created_time_onlyhour = ConvertDateTimeUTC_Time_DOWFULLDAY(item.created_time);
    let resulf = "";
    if (item.from_id == face_value_current_page) {
        resulf =
            '<div id="' + timespan + '" class="messengerDetail_me">'
            + ((message == "") ? '' : ('<span id="com_' + comment_id + '" class="'
                + ((isHidden) ? 'message_me me_hidden' : 'message_me')
                + '" title="' + created_time_onlyhour + '">'
                + message
                + '<span style="display: flex;">'
                + '<a class="commentedit_a" href="#"  id="edit_' + comment_id + '"><img id="imgEdit_' + comment_id + '" class="commentdelete commentdelete_img" src="/img/Face/edit.png">' + '</a>'
                + '<a class="commentdelete_a" href="#" data_first="' + first + '"  id="del_' + comment_id + '"><img id="imgDelete_' + comment_id + '" class="commentdelete commentdelete_img" src="/img/Face/nodelete_w.png">' + '</a>'
                + ((permalink_url != "") ? ('<a href="' + permalink_url + '" target="_blank"><img class="commentdelete commentdelete_img"  title="Link comment" src="/img/Face/face_w.png"></a>') : '')
                + '</span>'))
            + '</span>'
            + '</div>'
    }
    else {
        resulf =
            '<div id="' + timespan + '" class="messengerDetail_client">'
            + ((message == "") ? '' : ('<span id="com_' + comment_id + '" class="'
                + ((isHidden) ? 'message_client client_hidden' : 'message_client')
                + '" title="' + created_time_onlyhour + '">'
                + message
                + '<span style="display: flex;">'
                + '<a class="commentdelete_a" href="#" data_first="' + first + '"  id="del_' + comment_id + '"><img id="imgDelete_' + comment_id + '" class="commentdelete commentdelete_img" src="/img/Face/nodelete_b.png">' + '</a>'
                + ((permalink_url != "") ? ('<a href="' + permalink_url + '" target="_blank"><img class="commentdelete commentdelete_img"  title="Link comment" src="/img/Face/face_b.png"></a>') : '')
                + '<a class="commenthidden" title="' + ((isHidden) ? 'Unhide' : 'Hide') + '" id="hidcom_' + comment_id + '" href="#"><img id="imghidcom_' + comment_id + '" class="commentdelete commentdelete_img" src="' + ((isHidden) ? '/img/Face/nohide.png' : '/img/Face/hide.png') + '">' + '</a>'
                + '</span>'))
            + '</span>'
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
        let idcom = "com_" + id;
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
                "access_token": face_long_key
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
                            $('#' + idimg).attr("src", "/img/Face/hide.png");

                        }
                        else {
                            $('#' + idcom).addClass('client_hidden');
                            $('#' + idHidden)[0].title = "Unhide";
                            $('#' + idimg).attr("src", "/img/Face/nohide.png");
                        }
                    }
                }
            },
            beforeSend: function () {
                $('#' + idimg).attr("src", "/img/Face/waiting_exe.gif");
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
            "access_token": face_long_key
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Face_Error_API(1, "Cannot delete this comment");
        },
        success: function (result) {
            if (result != null && result != undefined && result.success) {
                if (result.success) {
                    $('#' + idimg).attr("src", "/img/Face/deleted.png");
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
            $('#' + idimg).attr("src", "/img/Face/waiting_exe.gif");
        }
    });
}
function Face_DeleteComment_ToDB(id) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Delete_First_Comment"
        , data = { 'comment': face_selected_commentid, 'post': face_selected_postid }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            $('#deletehind_' + id).show();
        }
        , sender = null
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
        $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/img/Face/waiting_exe.gif");
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
            "access_token": face_long_key
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
                    $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/img/Face/edit.png");
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
    $('#imgEdit_' + face_value_current_comment_edit).attr("src", "/img/Face/edit.png");
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
                "access_token": face_long_key
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
                        debugger
                        let id = result.id;
                        /ace_API_Get_Comment_Detail(id, timespan);
                        // Putting_ConversationToTopWhenClickAndReceive(face_selected_conversation, message);
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
                    + '<img class="waitingfacebook_exe" src="/img/Face/waiting_exe.gif" alt="label-image" />'
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
    Face_ScrollContentToLast();
}
function Face_API_Get_Comment_Detail(comment_id, timespan) {
    $.ajax({
        url: facebook_link_comment_detail.replace("{comment_id}", comment_id).replace('{version}', facebook_version)
            + "&access_token=" + face_long_key,
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
            e_sub.id = result.id;
            face_data_comment.push(e_sub);

            let timeline = Face_Render_API_Comments_Time(result.from.id, result.created_time);
            let stringContent = document.getElementById('MessengerArea_Messenger').innerHTML;
            stringContent = stringContent + Face_Render_API_Comments_Each(e_sub) + timeline;
            document.getElementById('MessengerArea_Messenger').innerHTML = stringContent;



            //Face_EventCLickEditComment();
            //Face_EventCLickDeleteComment();
            //Face_EventCLickHiddenComment();
            //$('#smallavatar_' + face_selected_commentid).attr("src", pic);
            //Face_ScrollContentToLast(0);
            //Face_EventLickImage();
            //Face_UpdatetimeComment(face_selected_commentid, face_selected_postid, comment.created_time, facebook_pageid, comment.message);
        },
        complete: function () {
            if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
            Face_Comment_Click_Hidden();
            Face_Comment_Click_Delete();
            Face_Comment_Click_Edit();
            Face_ScrollContentToLast();
            Face_Image_Chating_Click();
            Face_Event_Show_Title();
        }
    });
}