﻿function Face_Receipt_Comment(jsonString) {
    let promise = new Promise((resolve, reject) => {
        if (jsonString.includes('"verb":"add"'))
            Face_Receipt_Comment_Action(jsonString)
    });


}
function Face_Receipt_Comment_Action(jsonString) {

    if (typeof face_value_total_page_string !== 'undefined' && typeof face_value_total_page_string !== 'undefined') {
        let e = {};
        let changes_object = JSON.parse(jsonString).changes;
        let value_obj = changes_object[0].value;
        let post_obj = value_obj.post;
        e.permalink_url = post_obj.permalink_url;
        e.created_time = value_obj.created_time;
        e.from_id = value_obj.from.id;
        e.from_name = value_obj.from.name;
        e.message = value_obj.message;
        e.is_hidden = 0;
        e.like_count = 0;
        e.id = value_obj.comment_id; //comment_id
        let post_id = value_obj.post_id;
        let page = post_id.split('_')[0];
        let parent_id = value_obj.parent_id;

        if (face_value_total_page_string.includes(page)) {
            Face_Delivery_Comment(e, post_id, parent_id, page);
        }

    }
}
function Face_Delivery_Comment(e, post_id, parent_id, page) {
    let idwithmess = page + '-' + post_id + '-' + e.id;
    let idwithparent = page + '-' + post_id + '-' + parent_id;
    let id = '';
    if ($('#' + idwithmess).length) id = idwithmess;
    if ($('#' + idwithparent).length) id = idwithparent;
    if (face_value_selected_page_string.includes(page)) {
        if (id != '') {
            // Có trong danh sách page hiện tại, dang ton tai trong danh sach luoi
            let idnoti = Face_Notification_New_Message(e.from_id, page, e.message, "", "text", "COM");
            Face_Notification_New_Remove_FromQueure(idnoti);
            Face_Notification_New_Event_Close();
            Face_Exchange_Move_Mess_To_Top(id);
            Face_Exchange_Update_Item_Update_Person_Do_Comment(id, e.from_id, e.message);
            Face_Exchange_Update_Item_Read_To_Unread(id);
        }
        else {
            let idnoti = Face_Notification_New_Message(e.from_id, page, e.message, "", "text", "COM");
            Face_Notification_New_Remove_FromQueure(idnoti);
            Face_Get_Comment_In_Notification(post_id, parent_id, e.id);
            Face_Notification_New_Event_Close();
        }
    }
    else {
        // Có trong string page, nhưng không phải những trang hiện tại
        Face_Notification_OtherPage(page);
    }
}