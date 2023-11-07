"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notiHub").build();

// To User
//connection.on("SendMessageClient_ToUser", function (jsonString) {
//    HubExecute_ToUser(jsonString);
//});
// To String User

// From App
//connection.on("SendMessageClient_From_App", function (jsonString) {
//    HubExecute_FromAapp(jsonString);
//});
// From Face
//connection.on("ReceiptMessageFromFace", function (jsonString) {
//    if (typeof Face_Receipt_Message == 'function' && $.isFunction(Face_Receipt_Message)) {
//        Face_Receipt_Message(jsonString);
//    }
//});
//connection.on("ReceiptCommentFromFace", function (jsonString) {
//    if (typeof Face_Receipt_Comment == 'function' && $.isFunction(Face_Receipt_Comment)) {
//        Face_Receipt_Comment(jsonString);
//    }
//});
// Notier
//connection.on("Send_Notier_Client", function (jsonString) {
//    Notier.notify(0, JSON.parse(jsonString));
//});
connection.on("Send_Message_To_Branch", function (jsonString) {
     
    if (typeof Desk_AppHub == 'function') {
        Desk_AppHub(jsonString);
    }
});
connection.on("Send_Message_To_Room", function (jsonString) {
 
    if (typeof Desk_RoomHub == 'function') {
         
        Desk_RoomHub(jsonString);
    }
    if (typeof DeskDoc_Hub == 'function') {
        DeskDoc_Hub(jsonString);
    }
    if (typeof MonitorRoom_Hub == 'function') {
        MonitorRoom_Hub(jsonString);
    }
});
connection.on("Send_MonitorRoom", function (jsonString) {
    if (typeof MonitorRoom_Hub == 'function') {
        MonitorRoom_Hub(jsonString);
    }
});

connection.on("Send_Message_To_Scheduler", function (jsonString) {

    if (typeof cal_vtt_Manage_Executing == 'function' && $.isFunction(cal_vtt_Manage_Executing)) {
        cal_vtt_Manage_Executing(jsonString);
    }
});


//connection.on("SendTodo_ToUser", function (jsonString) {
//    if (typeof message_load_execute == 'function' && $.isFunction(message_load_execute)) {
//        message_load_execute(jsonString);
//    }
//    if (typeof TodoList_Json_Execute == 'function' && $.isFunction(TodoList_Json_Execute)) {
//        TodoList_Json_Execute(jsonString);
//    }
//});
//connection.on("SendLabo_ToUser", function (jsonString) {
//    if (typeof message_load_execute == 'function' && $.isFunction(message_load_execute)) {
//        message_load_execute(jsonString);
//    }
//    if (typeof LaboList_Json_Execute == 'function' && $.isFunction(LaboList_Json_Execute)) {
//        LaboList_Json_Execute(jsonString);
//    }
//});


///////////////
connection.on("Send_Noti_And_LogOut", function (jsonString) {
    if (typeof Noti_And_LogoutUser === 'function') {
        Noti_And_LogoutUser(jsonString);
    }
});
///////////////


/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
// #region // To User
function HubExecute_ToUser (pram) {
    NotiExe_ToUser(pram);
}
function HubExecute_ToStringUser (pram) {

    NotiExe_ToStringUser(pram);
}

// #endregion

/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
// #region // From App
function HubExecute_FromAapp (pram) {

}
function NoticeEntirePageApp (data) {
    //If the account is authorized MLunarCoffee
    InccomingApp(data);
    // if settings for notifications = true
    setTimeout(function () {
        DeclineApp();
    }, 10000);
}
// #endregion
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////



connection.start().then(function () { }).catch(function (err) {
    return console.error(err.toString());
});

//connection.invoke("SendMessage", user, message).catch(function (err) {
//    return console.error(err.toString());
//});
