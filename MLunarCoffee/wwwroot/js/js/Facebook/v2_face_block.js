﻿function Face_API_Checking_Block(page, client) {
    let url = facebook_link_blocked.replace("{page_id}", page).replace('{version}', facebook_version);
    let converid;
    $.ajax({
        url: url + "?access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page),
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        success: function (result) {
            if (JSON.stringify(result).includes(client)) {
                Face_SH_Blocked_Mess();
            }
            else {
                Face_SH_No_Block_Mess();
            }
        }
    });
    return converid;
}

function Face_Un_Block_User() {
    $('#executing_ticket_info').show();
    let url = facebook_link_blocked.replace('{version}', facebook_version);
    url = url.replace('{page_id}', face_value_current_page);
    $.ajax({
        url: url,
        dataType: "json",
        type: "DELETE",
        contentType: "application/json",
        data: JSON.stringify({
            "psid": face_value_current_client
            , "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (XMLHttpRequest.responseJSON.error.code == 100) {
                Face_Error_API(100, "Không thể cấm người này.Người này giúp quản lý Trang này và bạn phải xóa người này khỏi vai trò của mình thì mới có thể cấm họ");
                //CheckSendTag();
            }
        },
        success: function (result) {
            if (JSON.stringify(result).includes('true')) {
                Face_SH_No_Block_Mess();
            }
        },
        complete: function () {
            $('#executing_ticket_info').hide();
        }
    });
}
function Face_Block_User() {
    $('#executing_ticket_info').show();
    let url = facebook_link_blocked.replace('{version}', facebook_version);
    url = url.replace('{page_id}', face_value_current_page);
    $.ajax({
        url: url,
        dataType: "json",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            "psid": face_value_current_client
            , "access_token": Face_Get_Token_Access_From_PageID(face_value_current_page)
        }),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (XMLHttpRequest.responseJSON.error.code == 100) {
                Face_Error_API(100, "Không thể cấm người này.Người này giúp quản lý Trang này và bạn phải xóa người này khỏi vai trò của mình thì mới có thể cấm họ");
                //CheckSendTag();
            }
        },
        success: function (result) {
            if (JSON.stringify(result).includes('true')) {
                Face_SH_Blocked_Mess();
            }
        },
        complete: function () {
            $('#executing_ticket_info').hide();
        }
 
    });
}
