function heartBeat() {
   //let userid = sys_userID_Main;
   //let username = sys_userName_Main;
    AjaxJWT(url = "/api/Home/KeepAlive"
        , data = JSON.stringify({})
        , async = true
        , success = function (result) {
 
        }
    );
    Master_Checking_CallCenter();
   
}

//#region
//function Get_User_Online_List() {
//    $.get("/KeepAlive.ashx?useronline=1", function (data) {
//        if (data != "") {
//            let DataUserOnline = JSON.parse(data);
//            RenderListUserOnline(DataUserOnline, "ListUserOnline");
//        }
//    });
//}
//#endregion

$(function () {
    setInterval("heartBeat()", 1000 * 500 );
}); 