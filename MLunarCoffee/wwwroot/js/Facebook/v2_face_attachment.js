
var face_link_upload_file = '/Api/Upload/UploadFace?Method=Upload&Type={type}&TimeSpan={timespan}';
var face_link_delete_file = '/Api/Upload/UploadFace?Method=Delete';
//var face_folder_file = '/UploadClass/UploadedFiles/';

function Face_AttachFile_ToSend(element) {
    if (element != null && element != undefined && element.files != null && element.files != undefined && element.files.length != 0) {
        Face_AttachFile_ShowPreview(element);
    }
}

function Face_AttachFile_ShowPreview(event) {
    if (event != undefined && event.files.length != 0) { 
        for (let iii = 0; iii < event.files.length; iii++) {
            if (event.files[iii].size > 50000000) {
                notiWarning("File size is too large");
            }
            else {
                if (!Face_Attach_Checking_File_Type_Allow(event.files[iii].type)) {
                    notiWarning("Wrong format");
                }
                else {
                    let file = event.files[iii];
                    let blob = file.slice(0, file.size, 'image/png');
                    let newFile = new File([blob], xoa_dau(file.name.replaceAll(' ', '')), { type: file.type });
                    Face_Attach_LoadFinish(newFile);
                }
            }
        }
    }
}
function Face_Attach_Checking_File_Type_Allow(type) {
    console.log(type);
    type = type.toLowerCase();
    let isAllow = false;
    for (let i = 0; i < face_attach_file_allow.length; i++) {
        if (face_attach_file_allow[i] == type) {
            i = face_attach_file_allow.length;
            isAllow = true;
        }
    }
    return isAllow;
}

function Face_ShowHideAttachment() {
    if (face_value_current_attach.length == 0) {
        $("#ParentMessengerArea_Attachment").hide();
        $('#MessengerArea_Attachment').empty()
        $('#MessengerArea_Messenger').css({ 'height': 'calc(100vh - 220px)' });
    }
    else {
        $("#ParentMessengerArea_Attachment").show();
        $('#MessengerArea_Messenger').css({ 'height': 'calc(100vh - 405px)' });
    }

}

function Face_Attach_LoadFinish(file) {
    var reader = new FileReader();
    reader.onload = function () {
        let timespan = (new Date()).getTime();
        if ($('#' + timespan).length) timespan = timespan + 1;
        var parrent = document.getElementById("MessengerArea_Attachment");
        parrent.appendChild(Face_Attach_Generate_Preview(file.type, reader.result, file.name, timespan));
        var element = {};
        element.id = timespan;
        element.type = file.type;
        if (file.type.includes('image')) element.filetype = 'image';
        else if (file.type.includes('video')) element.filetype = 'video';
        else element.filetype = 'file';
        element.value = reader.result;
        element.name = file.name;
        element.attachmentid = 0;
        element.file = file;
        face_value_current_attach.push(element);
        Face_ShowHideAttachment();
        Face_Event_Delete_Attachment();
    };
    reader.readAsDataURL(file);
    reader.onerror = function () {
    };
}
function Face_Attach_Generate_Preview(type, src, name, timespan) {
    var div = document.createElement('div');
    if (type.includes('image')) {
        div.innerHTML = '<div class="divImageAttachment" id="div_' + timespan + '"><i id="dele_' + timespan + '" class="iconDeleteAttachment vtt-icon vttech-icon-delete"></i><img class="imgAttachment" src="' + src + '"></div>';
    }
    else {
        div.innerHTML = '<div class="divFileAttachment" id="div_' + timespan + '"><i id="dele_' + timespan + '" class="iconDeleteAttachment vtt-icon vttech-icon-delete"></i>' + name + '</div></div>';
    }
    return div.firstChild;
}
function Face_Event_Delete_Attachment() {
    $(".iconDeleteAttachment").click(function () {
        let idAttachment = this.id.replace('dele_', '');
        Face_Attach_Remove_From_Array(idAttachment);
        Face_ShowHideAttachment();
    });
}
function Face_Attach_Remove_From_Array(id) {
    for (var i = 0; i < face_value_current_attach.length; i++) {
        if (face_value_current_attach[i].id == id) {
            face_value_current_attach.splice(i, 1);
            $("#div_" + id).remove();
        }
    }
}
function Face_API_Send_MESSAGE_Attachment() {
    if (face_value_current_attach != undefined && face_value_current_attach.length != 0) {
      
        for (ii = 0; ii < face_value_current_attach.length; ii++) {
            let timespan = face_value_current_attach[ii].id;
            let filetype = face_value_current_attach[ii].filetype;
         
            Face_Send_Temporary_Attach(timespan, "MessengerArea_Messenger");
            //Face_API_Attachment_To_Folder(timespan, filetype, face_value_current_attach[ii].file);

            let url = face_link_upload_file.replace('{type}', filetype);
            url = url.replace('{timespan}', timespan);
            AjaxUpload_MultiExe(url, face_value_current_attach[ii].file, face_value_current_attach[ii].file.name
                , success = function (result, e) { 
                    if (result != "0") {
                        let _result = result.split('||');
                        let _filename = _result[0];
                        let _timespan = _result[1];
                        let _type = _result[2];

                        //let _link = face_folder_file + _filename
                        Face_API_Send_MESSAGE_Attachment_Execute(_timespan, _filename, _type);
                    }
                    else {
                        // Sai dinh danh, qua dung luong
                        if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
                    }
                     
                }
                , error, before
            );
        }
    }
    face_value_current_attach = [];
    Face_ShowHideAttachment();
}
function Face_API_Attachment_To_Folder(timespan, type, file) {
    try {
        let url = face_link_upload_file.replace('{type}', type);
        url = url.replace('{timespan}', timespan);
        var formdata = new FormData();
        formdata.append(timespan, file);
        $.ajax(
            {
                url: url,
                data: formdata,
                processData: false,
                contentType: false,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log("Error"); 
                },
                success: function (result, e) { 
                    if (result != "0") {
                        let _result = result.split('||');
                        let _filename = _result[0];
                        let _timespan = _result[1];
                        let _type = _result[2];

                        //let _link = face_folder_file + _filename
                        Face_API_Send_MESSAGE_Attachment_Execute(_timespan, _filename, _type);
                    }
                    else {
                        // Sai dinh danh, qua dung luong
                        if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
                    }
                },
                beforeSend: function (xhr, e) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken")); 
                }
            }
        );
          
    }
    catch (ex) {

    }
    
    
}

function Face_API_Send_MESSAGE_Attachment_Execute(timespan,  filename, type) {
    let link = face_Link_HTTP_File + filename;
    $.ajax({
        url: facebook_link_send_message.replace('{version}', facebook_version),
        dataType: "json",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            "recipient": JSON.stringify({
                "id": face_value_current_client
            }),
            "message": JSON.stringify({
                "attachment": {
                    "type": type,
                    "payload": {
                        "url": link, 
                        "is_reusable": true
                    }
                }
            }),
            "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //(#100) Không lấy được file từ URL này
            if (XMLHttpRequest.responseJSON.error.code == 10) {
                Face_Error_API(10, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
                //CheckSendTag();
            }
            else {
                Face_Error_API(1, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
            }
            

        },
        success: function (result) {
            if (result != null && result != undefined) {
                if (result.message_id != undefined && result.recipient_id != undefined && result.message_id != null && result.recipient_id != null) {
                    let message_id = result.message_id;
                    Face_API_Get_Message_Detail("me",message_id, timespan);
                }
            }
        },
        complete: function () {
            if ($("#" + Number(timespan)).length) $("#" + Number(timespan)).remove();
            Face_Attachment_Delete_After_Send(filename);
            Face_Image_Chating_Click();
        }
    });
}

function Face_Attachment_Delete_After_Send(filename) {
    let formdata = new FormData();
    formdata.append(filename,filename);
    $.ajax(
        {
            url: face_link_delete_file,
            data: formdata,
            processData: false,
            contentType: false,
            type: "POST",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Error");
            },
            success: function (result, e) {
                console.log("Success");
            },
            beforeSend: function (xhr, e) {
                xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
            }
        }
    );
    
}

function Face_Send_Temporary_Attach(timespan, id) {
    var childNode = document.getElementById(id).children;
    var parrent = document.getElementById(id);
    if (childNode != undefined) {
        let stringContent = '';
        let tr =
            '<div id="' + timespan + '"  class="messengerDetail_me">'
            + '<img id="att_' + timespan + '" class="message_me attachment_me_temp" src="/assests/img/Face/loading_avatar.gif" height="200" width="200">'
            + '</div>';
        stringContent = tr;
        if (stringContent != "") {

            var newelement = createElementFromHTML(stringContent);
            parrent.appendChild(newelement);

        }
    }
    Face_ScrollContentTo_First_Last();
}