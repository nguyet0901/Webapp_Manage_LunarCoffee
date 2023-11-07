function Face_Notification_New_Event_Close() {
    $(".imgDeletenoti").click(function (event) {
        let _id = this.id.replace('_dele_noti_', '');
        if ($('#' + _id).length) {
            $('#' + _id).remove();
        }
    });
}
function Face_Notification_New_Remove_FromQueure(id) {
    setTimeout(function () {
        if ($('#' + id).length) {
            $('#' + id).remove();
        }
    }, 10000)
}
// Nhận tin nhắn và update chatting
function Face_Update_Chatting(mess_id, created) {
    let timespan = ConvertDateTimeToTimeSpan(created);
    Face_API_Get_Message_Detail("client", mess_id, timespan);

}
function Face_Get_Mess_In_Notification(page, client) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Checking_Mess_Is_Exist"
        , data = {
            'page': page, 'client': client
        }, async = true
        , error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data.length != 0) {
                    let r = Face_Create_New_Obj_FromData(data[0]);
                    Face_Exchange_Create_Mess_To_Top(r.id, r.e);
                    Face_Event_Conversation_List_Click();
                    Face_Exchange_Update_Item_Read_To_Unread(r.id);
                }
            }
        }
    );
    
}
function Face_Get_Comment_In_Notification( post_id,  parent_id,  comment_id) {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Checking_Com_Is_Exist"
        , data = {
            'post_id': post_id, 'parent_id': parent_id, 'comment_id': comment_id
        }, async = true
        , error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data.length != 0) {
                    let r = Face_Create_New_Obj_FromData(data[0]);
                    Face_Exchange_Create_Mess_To_Top(r.id, r.e);
                    Face_Event_Conversation_List_Click();
                    Face_Exchange_Update_Item_Read_To_Unread(r.id);
                }
            }
        }
    );
    
}

function Face_Notification_OtherPage(pageid) {

    if ($('#divpageFanpage_' + pageid).length) {
        if (!$('#divpageFanpage_' + pageid).hasClass('pageAvatarpage_NewMessage'))
        {
            $('#divpageFanpage_' + pageid).addClass('pageAvatarpage_NewMessage')
        }
    }
}

function Face_Notification_New_Message(client,page, message, url, type, typeconver) {
    try {
        let timespan = (new Date()).getTime();
        let text;
        let pic = '//graph.facebook.com/' + client + '/picture?access_token=' + Face_Get_Token_Access_From_PageID(page);
        let classname = "";
        switch (type) {
            case "image":
                text = 'Sending image';
                break;
            case "video":
                text = 'Sending video';
                break;
            case "text":
                if (typeconver == "MESS") text = (message == undefined || message == "") ? '* New message' : message
                else text = (message == undefined || message == "") ?'* New comment' : message;
                break;
            case "audio":
                text = 'Sending audio';
                break;
            default:
                text = 'Sending file';
                break;
        }
        if (typeconver == "MESS") classname = "noti_new_message";
        else classname = "noti_new_comment";

        let id = 'NewMessageFacebook';
        var parrent = document.getElementById(id);
        if (parrent.childNodes.length == 0) classname = classname + " first_noti_from_face";
        let obj = '<div id="' + timespan + '" class="' + classname + '">'
            + '<img  class="ui mini circular image notiMessage_avatar" src="' + pic + '" alt="label-image" />'
            + '<div class="notiMessage notiMessage_mess">' + text + '</div>'
            + ((url != undefined && url != "")
                ? '<img class="notiMessage_image" src="' + url + '" alt="label-image" />'
                : ''
            )
            + '<img id="_dele_noti_' + timespan + '" class="imgDeletenoti" src="/assests/img/Face/remove_noti.png" alt="label-image">'
            + '</div>';

        var newelement = createElementFromHTML(obj);
        if (parrent.childNodes.length > 2) parrent.removeChild(parrent.childNodes[0])
        parrent.appendChild(newelement);
        return timespan;
    }
    catch (err) {

    }
}