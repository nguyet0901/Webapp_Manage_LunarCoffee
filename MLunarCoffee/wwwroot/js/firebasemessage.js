const fcmfaceicon = 'ttps://LunarCoffee.com/faceicon.svg';
const fcmicon = (typeof _SoftwareSmallLogo !== 'undefined' && _SoftwareSmallLogo != '')
    ? _SoftwareSmallLogo
    : '/default.png';
const fcmkeycode = (typeof VDCODE !== 'undefined' && VDCODE != '')
    ? VDCODE
    : '';
const fcmtype_signout = 'signout';
const fcmtype_mess = 'message';
const fcmtype_topic = 'topic';
const fcmtopic_payment = 'payment';
const fcmtopic_app = 'appnotification';
const fcmtopic_local = 'localnotification';
const fcmtopic_customergate = 'customergate';
const tokenobj = author_get('fcm');
const istokenvalid = 0;
var fcmpagelist = {};



if (tokenobj != '') {
    let _page = author_get("fb_pagelist");
    if (_page != undefined && _page != "") fcmpagelist = JSON.parse(_page);
    let obj = JSON.parse(tokenobj);
    var token = null;

    var config = {
        apiKey: obj.apiKey,
        authDomain: obj.authDomain,
        projectId: obj.projectId,
        storageBucket: obj.storageBucket,
        messagingSenderId: obj.messagingSenderId,
        appId: obj.appId,
        measurementId: "G-PZYWWSHF3X"//obj.measurementId
    };
    if (firebase.apps.length === 0) {
        firebase.initializeApp(config);
    }
    const messaging = firebase.messaging.isSupported() ? firebase.messaging() : null;
    if (messaging != null) {
        messaging.requestPermission()
        .then(function () {
            messaging.onMessage(function (payload) {
                fcm_executemessage(payload);
            });
        });
    }
    
    

}

//#region  // Execute Message
async function fcm_executemessage (payload) {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                try {
                     
                    let Messagetitle = '', Messageavatar = '', Messagecontent = '', MessaeContentNoti = '';
                    let MFBtitle = '', MFBPageID = '', MFBIcon = '', MFBBody = '',MFBFromID = '' ;
                    let isrecipt = 0;
                    let isnotifcm = 0;
                    if (payload.data.data != undefined) {
                        let data = payload.data.data;
                        if (data != undefined && data != "") {
                            let objdata = JSON.parse(data);
                            let _type = objdata.type;
                            if (_type != undefined) {
                                let _userto = objdata.userto;
                                let _converid = objdata.converid;

                                Messagetitle = payload.data.title;
                                Messageavatar = payload.data.icon;
                                Messagecontent = payload.data.body;
                                if (_converid != undefined && _converid != '') {

                                    Messagecontent = Messagecontent != '' ? Messagecontent : Outlang['Dang_gui_tep'];
                                    switch (_type) {
                                        case 'txt':
                                            MessaeContentNoti = payload.data.body;
                                            break;
                                        case 'img':
                                            MessaeContentNoti = '<img src="' + objdata.linkfeature + '" class="img-fluid mb-2 border-radius-lg" style="max-height: 50px;min-height: 50px;">';
                                            break;
                                        case 'fle':
                                            MessaeContentNoti = '<span class="d-block">' + objdata.filename + '<span/>';
                                            break;
                                    }
                                    if (typeof Message_Receipt !== 'undefined' && typeof Message_Receipt === 'function') {
                                        let e = {};
                                        let objdata = JSON.parse(data);
                                        e.custid = objdata.custid;
                                        e.userid = objdata.userid;
                                        e.time = objdata.time;
                                        e.link = objdata.link;
                                        e.converid = objdata.converid;
                                        e.type = objdata.type;
                                        e.linkfeature = objdata.linkfeature;
                                        e.filename = objdata.filename;
                                        e.sendername = payload.data.title;
                                        e.senderavatar = payload.data.icon;
                                        e.body = payload.data.body;
                                        Message_Receipt(e);
                                    }
                                    else if (typeof Conver_Receipt !== 'undefined' && typeof Conver_Receipt === 'function') {
                                        let objdata = JSON.parse(data);
                                        Conver_Receipt(objdata.converid);
                                    }
                                    else {
                                        if (fcm_detecttopic(fcmtopic_customergate) == 1) {
                                            isrecipt = 1;
                                            isnotifcm = 1;
                                        }
                                    }
                                }
                                else {
                                    switch (_type) {
                                        case fcmtype_signout: { // logout
                                            if (typeof sys_userID_Main !== 'undefined' && sys_userID_Main != '' && _userto == sys_userID_Main) {
                                                isrecipt = 1; isnotifcm = 0;
                                                if ($('#logoutdiv').length) {
                                                    $("#logoutdiv").trigger("click");
                                                }

                                            }
                                            break;
                                        }
                                        case fcmtype_mess: { // tin nhắn cá nhân 1 vs 1.
                                            if (typeof sys_userID_Main !== 'undefined' && sys_userID_Main != '' && _userto == sys_userID_Main && Messagetitle != 'portcheckonl') {
                                                if (fcm_detecttopic('personal') == 1) {
                                                    isrecipt = 1; isnotifcm = 1;
                                                }
                                                fcm_updateCount();
                                            }
                                            if (Messagetitle == 'portcheckonl') {
                                                let objData = JSON.parse(Messagecontent);
                                                if (typeof SSO_ExcuteStatusDevice === 'function') SSO_ExcuteStatusDevice(objData.key, objData.time);
                                            }
                                            break;
                                        }
                                        case fcmtype_topic: { // Theo topic
                                            let _branch = objdata.branch;
                                            let _topic = objdata.topic;
                                            switch (_topic) {
                                                case fcmtopic_payment:
                                                    {
                                                        if (typeof Master_branchID !== 'undefined' && Master_branchID != '' && Master_branchID == _branch) {
                                                            if (fcm_detecttopic(_topic) == 1) {isrecipt = 1; isnotifcm = 1;}
                                                        }
                                                    }
                                                    break;
                                                case fcmtopic_local:
                                                    {
                                                        if (typeof Appnote_Load === 'function') Appnote_Load(); // Profile Customer - Ghi chú lịch hẹn trong ngày
                                                    }
                                                    break;
                                                default: break;
                                            }
                                            break;
                                        }
                                        case "rating":
                                        case "app": {
                                            if (objdata.from == "fromapp") {
                                                let _topic = objdata.topic;
                                                switch (_topic) {
                                                    case fcmtopic_app:
                                                        {
                                                            if (fcm_detecttopic(_topic) == 1) {
                                                                isrecipt = 1; isnotifcm = 0;
                                                                if (objdata.ratingid != undefined) {
                                                                    const notiStar = (name, num) => {
                                                                        let MaxStar = 5, content = '';
                                                                        name = `<div>${name}</div>`
                                                                        for (let i = 0; i < MaxStar; i++) {
                                                                            if (num > i) content += `<i class="fas fa-star text-warning"></i>`;
                                                                            else content += `<i class="far fa-star text-secondary"></i>`;
                                                                        }
                                                                        return `<div >${name}<div>${content}</div></div>`;
                                                                    }
                                                                    Messagetitle = notiStar(objdata.custname, objdata.star);
                                                                }
                                                                if (objdata.ratingid != undefined && typeof DeskGeneral_LoadRating === 'function') DeskGeneral_LoadRating(objdata.ratingid);
                                                                if (objdata.appid != undefined && typeof AC_DAL_LoadShedule === 'function') AC_DAL_LoadShedule(objdata.appid);
                                                            }
                                                        }
                                                        break;
                                                    default: break;
                                                }
                                            }
                                            break;
                                        }
                                        default: break;
                                    }
                                    MessaeContentNoti = Messagecontent;
                                }
                                if (isrecipt == 1) {

                                    fcm_executemessage_popup().then(function (result) {
                                        if (result) notiMessage(title = Messagetitle, avatar = Messageavatar, mes = MessaeContentNoti);

                                    });
                                    fcm_dingdong();
                                }
                                if (isnotifcm == 1 && _type != fcmtype_signout) {
                                    fcm_executemessage_popup().then(function (result) {
                                        if (result) {
                                            notificationTitle = Messagetitle;
                                            notificationOptions = {
                                                body: Messagecontent,
                                                icon: Messageavatar
                                            };
                                            var notification = new Notification(notificationTitle, notificationOptions);
                                        }
                                    });

                                }
                            }
          
                        }
                    }
                    else {
                        MFBtitle = "Facebook";
                        MFBIcon = payload.data.icon;
                        MFBBody = payload.data.body;
                        MFBPageID = payload.data.pageid;
                        MFBFromID = payload.data.fromid;
                        if (fcm_detectpagefb(MFBPageID) == 1) {
                            if (typeof FBR_ReceiptMess !== 'undefined' && typeof FBR_ReceiptMess === 'function') {
                                FBR_ReceiptMess(payload.data);
                                fcm_tengteng();
                            }
                            fcm_executemessage_popup().then(function (result) {
                                if (result) {
                                    notificationTitle = MFBtitle;
                                    notificationOptions = {
                                        body: MFBBody,
                                        icon: fcmfaceicon
                                    };
                                    var notification = new Notification(notificationTitle, notificationOptions);
                                }
                            });
                        }
                    }
                } catch (e) { }
                resolve()
            }
        )
    });
}
async function fcm_dingdong () {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                try {
                    if (!document.hidden && !fcm_checkmutesound()) {
                        let beat = new Audio('/ding.mp3');
                        beat.play();
                    }


                } catch (e) { }
                resolve()
            }
        )
    });

}
async function fcm_tengteng () {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                try {
                    if (!document.hidden && !fcm_checkmutesound()) {
                        
                        let beat = new Audio('/teng.mp3');
                        beat.play();
                    }
                } catch (e) { }
                resolve()
            }
        )
    });

}
function fcm_checkmutesound () {
    try {
        let _fcm_set = author_get('setting');
        let _fcmmute = 0;
        if (_fcm_set != undefined && _fcm_set != "") {
            let _fcm_setobj = JSON.parse(_fcm_set);
            if (_fcm_setobj != undefined && _fcm_setobj.mutesound != undefined) _fcmmute = _fcm_setobj.mutesound;
        }
        return (_fcmmute == 1);
    } catch (e) {return false;}
}
async function fcm_executemessage_popup () {
    try {
        let _fcm_set = author_get('setting');
        let _fcmpersonal = 0;
        if (_fcm_set != undefined && _fcm_set != "") {
            let _fcm_setobj = JSON.parse(_fcm_set);
            if (_fcm_setobj != undefined && _fcm_setobj.personal != undefined) _fcmpersonal = _fcm_setobj.personal;
        }
        return (_fcmpersonal == 1);
    } catch (e) {return (false)}
}
async function fcm_updateCount () {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                try {
                    if ($("#MS_NotiTotal").length) {
                        let num = $("#MS_NotiTotal").html();
                        let number = 0;
                        if (isNaN(num)) number = 1
                        else if (Number(num) >= 20) number = 20;
                        else number = Number(num) + 1;
                        $("#MS_NotiTotal").html(number);
                        if ($("#MS_NotiTotal").hasClass('d-none')) $("#MS_NotiTotal").removeClass('d-none');
                    }

                } catch (e) { }
                resolve()
            }
        )
    });

}
function fcm_detecttopic (topiccheck) {
    try {
        let isReceipt = 0;
        let TokenTopic = author_get('TokenTopic');
        let Setting = author_get('setting');
        if (topiccheck == 'personal') {
            let objSetting = JSON.parse(Setting);
            if (objSetting[topiccheck] == 0) isReceipt = 0;
            else isReceipt = 1;
        }
        else {
            if (TokenTopic != undefined && TokenTopic != "") {
                let topic = JSON.parse(TokenTopic);
                for (let ii = 0; ii < topic.length; ii++) {
                    if (Number(topic[ii].ISREGIS) == 1 && topic[ii].Topic == topiccheck) {
                        if (Setting == undefined || Setting == "") {
                            isReceipt = 1;
                        }
                        else {
                            let objSetting = JSON.parse(Setting);
                            if (objSetting[topiccheck] == 0) isReceipt = 0;
                            else isReceipt = 1;
                        }
                    }
                }
            }
        }
        return isReceipt;
    }
    catch (ex) {
        return 0;
    }

}
function fcm_detectpagefb (pageid) {
    try {

        if (fcmpagelist[pageid] != undefined && fcmpagelist[pageid].IsNoti==1)
            return 1;
        else return 0;
    }
    catch (ex) {
        return 0;
    }

}
//#endregion

//#region // Function Send FCM
function fcm_sendtopic (topicto, avatarfrom, namefrom, content) {
 
    if (fcmkeycode != '' && topicto != undefined && topicto != '') {
        let realtopic = '/topics/' + fcmkeycode + topicto;
        namefrom = namefrom != "" ? namefrom : Outlang['He_thong'];
        avatarfrom = avatarfrom != "" ? avatarfrom : _SoftwareSmallLogo;
        let e = {};
        e.type = fcmtype_topic;
        e.branch = (typeof Master_branchID !== 'undefined' && Master_branchID != '') ? Master_branchID : 0;
        e.topic = topicto;

        fcm_sendtoken(
            title = namefrom
            , body = content
            , data = JSON.stringify(e)
            , icon = avatarfrom
            , token = realtopic);



    }
}
function fcm_sendmultiuser (userstringto, avatarfrom, namefrom, content) {
    AjaxJWT(url = "/api/Home/TokenMultiUser"
        , data = JSON.stringify({
            stringuserid: userstringto
        })
        , async = true
        , success = function (result) {
            if (result != "0") {
                namefrom = namefrom != "" ? namefrom : Outlang['He_thong'];
                avatarfrom = avatarfrom != "" ? avatarfrom : _SoftwareSmallLogo;
                let tokento = '';
                let _data = JSON.parse(result);
                if (_data != undefined && _data.length != 0) {
                    fcm_excutesendmulti(_data, avatarfrom, namefrom, content);
                }
            }
        }
    );
}

function fcm_excutesendmulti(_data, avatarfrom, namefrom, content) {
    namefrom = namefrom != "" ? namefrom : Outlang['He_thong'];
    avatarfrom = avatarfrom != "" ? avatarfrom : _SoftwareSmallLogo;
    let tokento = '';
    if (_data != undefined && _data.length != 0) {
        for (let jj = 0; jj < _data.length; jj++) {
            if (_data[jj].TokenFcm != "" && _data[jj].ID != 0) {
                tokento = _data[jj].TokenFcm;
                if (tokento != "") {
                    let e = {};
                    e.userto = _data[jj].ID;
                    e.type = fcmtype_mess;

                    fcm_sendtoken(
                        title = namefrom
                        , body = content
                        , JSON.stringify(e)
                        , icon = avatarfrom
                        , token = tokento);

                }
            }
        }
    }
}


function fcm_senduser (userto, avatarfrom, namefrom, content) {

    AjaxJWT(url = "/api/Home/TokenUser"
        , data = JSON.stringify({
            UserID: userto
        })
        , async = true
        , success = function (result) {
            if (result != "0") {
                let tokento = result;
                namefrom = namefrom != "" ? namefrom : Outlang['He_thong'];
                avatarfrom = avatarfrom != "" ? avatarfrom : _SoftwareSmallLogo;
                if (tokento != "") {
                    let e = {};
                    e.userto = userto;
                    e.type = fcmtype_mess;
                    fcm_sendtoken(
                        title = namefrom
                        , body = content
                        , JSON.stringify(e)
                        , icon = avatarfrom
                        , token = tokento);
                }

            }
        }
    );
}
//function fcm_pushlogout (userto, tokento) {
//    let e = {};
//    e.userto = userto;
//    e.type = fcmtype_signout;
//    let title = (typeof Master_nameClient !== 'undefined' && Master_nameClient != '') ? Master_nameClient : 'System';
//    fcm_sendtoken(
//        title = title
//        , body = Outlang["Nguoi_dung_da_dang_nhap_o_nhung_noi_khac"]
//        , data = JSON.stringify(e)
//        , icon = (typeof _SoftwareSmallLogo !== 'undefined' && _SoftwareSmallLogo != '') ? _SoftwareSmallLogo : '/alert.png'
//        , token = tokento);
//}
function fcm_chatcustgate (tokento, converid, custid, time, type, message, link, linkfeature, filename) {

    let e = {};
    e.converid = converid;
    e.custid = custid;
    e.userid = 0;
    e.time = time;
    e.type = type;
    e.link = link;
    e.linkfeature = linkfeature;
    e.filename = filename;

    fcm_sendtoken(
        title = _CompanyName
        , body = message
        , data = JSON.stringify(e)
        , icon = _SoftwareLogo
        , token = tokento);
}
function fcm_sendtoken (title, body, data, icon, token) {
    AjaxJWT(url = "/api/FCM/Send"
        , data = JSON.stringify({
            to: token
            , title: title
            , body: decodeHtml(body)
            , icon: icon
            , data: data
        })
        , async = true
        , success = function (result) {

        }
    );
}
//#endregion

//#region // Function send mobile app
function fcmmobile_sendcust (customerid, header, content, articleid, imageurl, successcall) {
    if (customerid != 0) {
        AjaxJWT(url = "/api/Home/MobileCust"
            , data = JSON.stringify({
                CustomerID: customerid
            })
            , async = true
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data != undefined && data.length == 1) {
                        let token = data[0].Token;
                        let platform = data[0].Platform.toLowerCase();
                        if (token != "") {
                            fcmmobile_sendexe(
                                token = token
                                , header = header
                                , content = content
                                , articleid = articleid
                                , imageurl = imageurl
                                , platform = (platform == 'ios' ? 'ios' : 'and')
                                , successcall
                            )

                        }
                    }
                }

            }
        );
    }
}
function fcmmobile_sendexe (token, header, content, articleid, imageurl, platform, successcall) {
     
    AjaxJWT(url = "/api/FCM/MobileNoti"
        , data = JSON.stringify({
            token_id: token
            , Header: header
            , Content: content
            , ID: articleid
            , Image: imageurl
            , Platform: platform
        })
        , async = true
        , success = function (result) {
            if (result == "1") {
                if (typeof successcall === 'function') {
                    successcall(result);
                }

            }
        }
    );
}

//TZ0106ZZZ3GF5DGK4563522HFE5452D
//ios,and,all
function fcmmobile_sendtopicexe (topic, header, content, articleid, imageurl, platform, successcall) {

    AjaxJWT(url = "/api/FCM/MobileTopic"
        , data = JSON.stringify({
            topic: topic
            , Header: header
            , Content: content
            , ID: articleid
            , Image: imageurl
            , Platform: platform
        })
        , async = true
        , success = function (result) {
            if (result == "1") {
                if (typeof successcall === 'function') {
                    successcall(result);
                }

            }
        }
    );
}
//#endregion

