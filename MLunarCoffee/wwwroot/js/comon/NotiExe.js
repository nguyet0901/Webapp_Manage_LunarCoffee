//start notiApp
//var IncommingaudioApp = new Audio('/assests/img/Call/incommingCall.mp3');
function InccomingApp(data) {
    try {

        //IncommingaudioApp.loop = true;
        //IncommingaudioApp.play();
        $('#AppNotiDiv').show();
        $('#AppNotiDiv').attr('data-id', data.ID);
        $('#AppPageName').html(data.TypeName);
        $('#AppRecipientName').html('UserName App: ' + data.UserName);
        $('#AppageNoti').html(data.Message);
        $('#AppPageBranch').html(data.BranchName);

    }
    catch (err) {
        console.log("Sound Error");
    }
}
function AcceptAppNoti() {
    //IncommingaudioApp.pause();
    //IncommingaudioApp.currentTime = 0;
    $('#AppNotiDiv').hide();
    //show
    window.location.href = '/Notification/NotificationList?Value=' + $('#AppNotiDiv').attr('data-id');
    return false;
}
function DeclineApp() {
    //IncommingaudioApp.pause();
    //IncommingaudioApp.currentTime = 0;
    $('#AppNotiDiv').hide();
    return false;
}
function LoadNotiCountUnRead() {
    //AjaxLoad(url = "/Notification/NotificationList/?handler=LoadNotiReadUnCount"
    //    , data = {}, async = true, error = null
    //    , success = function (result) {
    //        let data = JSON.parse(result)[0];
    //        if (data.CountUnRead > 0) {
    //            $('#icon-noti-app1').css('display', 'block');
    //            $('#icon-noti-app2').css('display', 'block');
    //            $('#icon-noti-app1').html(data.CountUnRead);
    //            $('#icon-noti-app2').html(data.CountUnRead);

    //        } else {
    //            $('#icon-noti-app1').css('display', 'none');
    //            $('#icon-noti-app2').css('display', 'none');
    //        }
    //        SetAlarmSchedule(data, exdays, second_alram, empid);
            
    //    });
}
//end notiapp

//start notiPage
function InccomingPage(pageName, RetName, MessageNoti) {
    try {
        $('#MessPageDiv').show();
        Messagegaudio.loop = true;
        Messagegaudio.play();
        $('#MessPageName').html(pageName);
        $('#MessRecipientName').html('( ' + RetName + ' )');
        $('#MessageNoti').html(MessageNoti);

    }
    catch (err) {
        console.log("Sound Error");
    }
}






async function NotiExe_ToUser(json) {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                let data = JSON.parse(json);
                if (data != undefined) {
                    let empid = data.empid;
                    let title = data.title;
                    let message = data.message;
                    if (Number(empid) == Number(sys_employeeID_Main)) {
                        notiMess10( message);
                    }
                }
                resolve()
            }
        );
    });

}
async function NotiExe_ToStringUser(json) {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                let data = JSON.parse(json);
                if (data != undefined) {
                    let userid = data.userid;
                    let userfrom = data.userfrom;
                    let title = data.title;
                    let message = data.message;
                    userid = ',' + userid + ','; 
                    let _myuserid = ',' + sys_userID_Main + ','; 
                    if (Number(sys_userID_Main) != Number(userfrom) && userid.includes(_myuserid)) {
                        notiMess10( message);
               
                    }                   
                }
                resolve()
            }
        );
    });

}

async function Noti_And_LogoutUser(json) {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                let data = JSON.parse(json);
                if (data != undefined) {
                    let userId = data.userid;
                    let title = data.title;
                    let message = data.message;
                    if (Number(userId) == Number(sys_userID_Main)) {
                        notiMessage(title, '', message);
                        let timeoutId = setTimeout(() => {
                            MS_HandleLogout()
                            clearTimeout(timeoutId);
                        },5000)
                    }
                }
                resolve()
            }
        );
    });

}