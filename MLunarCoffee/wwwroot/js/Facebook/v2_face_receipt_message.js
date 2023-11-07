function Face_Receipt_Message(jsonString) {
    let promise = new Promise((resolve, reject) => {
        Face_Receipt_Message_Action(jsonString)
    });
}

function Face_Receipt_Message_Action(jsonString) {
    if (typeof face_value_total_page_string !== 'undefined' && typeof face_value_total_page_string !== 'undefined') {
        let e = {};
        let dateNow = new Date();
        let dateMin = new Date();
        dateMin.setDate(dateMin.getDate() - 1);
        let type, url, message;

        let data = JSON.parse(jsonString).entry[0];
        let mess_obj = data.messaging[0].message;
        let mess_id = mess_obj.mid;
        let page = data.messaging[0].recipient.id;
        if (face_value_total_page_string.includes(page)) {
            let client = data.messaging[0].sender.id;
            if (mess_obj.hasOwnProperty("attachments")) {
                let attach_obj = mess_obj.attachments[0];
                type = attach_obj.type;
                url = attach_obj.payload.url;
                message = "Sending image";
            }
            if (mess_obj.hasOwnProperty("text")) {
                message = mess_obj.text;
                url = '';
                type = "text";
            }
            e.page = page;
            e.client = client;
            e.type = type;
            e.message = message;
            e.url = url;
            e.created = dateNow;
            e.updated = dateNow;
            e.readtime = dateMin;
            e.client_to_server = "1";
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
            e.post_id = '';
            e.comment_id = '';
            let id = page + '-' + client;
            Face_Delivery_Message(e, id, mess_id);
        }
    }
}
function Face_Delivery_Message(e, id, mess_id) {
    if (face_value_selected_page_string.includes(e.page)) {
        if ($('#' + id).length) {
            // Có trong danh sách page hiện tại, và đang chatting
            if (e.client == face_value_current_client && e.page == face_value_current_page && face_value_current_mess_post == "MESS") {
                // Đang trong chatting, thực hiện cập nhật chatting,đưa conver lên top, snipset
                Face_Exchange_Move_Mess_To_Top(id);
                Face_Exchange_Update_Item_Update_Person_Do(id, e.client, e.type, e.message, e.url);
                Face_Update_Chatting(mess_id, e.created);
                Face_Update_Conversation_Having_Phone(e.message, e.created);
            }
            else {
                // Có trong danh sách page hiện tại, nhưng không chatting.
                // Show popup, đưa lên top, chuyển trạng thái đọc
                // Add vào danh sách hiện tại
                let idnoti = Face_Notification_New_Message(e.client, e.page, e.message, e.url, e.type, "MESS");
                Face_Notification_New_Remove_FromQueure(idnoti);

                Face_Notification_New_Event_Close();
                Face_Exchange_Move_Mess_To_Top(id);
                Face_Exchange_Update_Item_Update_Person_Do(id, e.client, e.type, e.message, e.url);
                Face_Exchange_Update_Item_Read_To_Unread(id);


            }
        }
        else {

            // Có trong danh sách page đang chọn, nhưng không tồn tại trong danh sách, cần lấy 
            let idnoti = Face_Notification_New_Message(e.client, e.page, e.message, e.url, e.type, "MESS");
            Face_Notification_New_Remove_FromQueure(idnoti);
            Face_Notification_New_Event_Close();
            Face_Get_Mess_In_Notification(e.page, e.client);


            // Mang lên trên
        }
    }
    else {
        // Có trong string page, nhưng không phải những trang hiện tại
        Face_Notification_OtherPage(e.page);
    }
}

