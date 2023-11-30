var sys_SMSTEMPLATE = {
    "PAYMENT": 'SMSContentAfterPayment',
    "CARD": 'SMSContentAfterCashback',
    "APPOINTMENT": 'SMSContentAfterAppointment'
}

function SMSSYS_Pending (_custid, _phone, _content, _amount, _brandname, _typecare, _ticketid, _appid, _studentid, _znssms
    , _actionsend) {
    try {
        _content = xoa_dau(_content);
        AjaxJWT(url = "/api/SMS/Pending"
            , data = JSON.stringify({
                'content': _content,
                'phone': _phone,
                'amount': _amount,
                'customerID': _custid,
                'ticketID': _ticketid,
                'appID': _appid,
                'studentID': _studentid,
                'typecare': _typecare,
                'znssms': _znssms
            })
            , async = true
            , success = function (result) {
                if (result !== "0") {
                    let _idsms = Number(result);
                    if (typeof _actionsend === 'function') _actionsend(_idsms);
                }
            }
        );
    }
    catch (ex) { }
}
function SMSSYS_ActionSend (_phone, _content, _amount, _brandname,_znssms
    , _idsms
    , _successfunc, _errorfunc, _bannedfunc) {
    try {
        AjaxJWT(url = "/api/SMS/Send"
            , data = JSON.stringify({
                'phone': _phone,
                'content': _content,
                'brandname': _brandname,
                'znssms': _znssms,
                'CallbackID': _idsms,
                'CallbackUrl': ''
            })
            , async = true
            , success = function (result) {
                if (result === "1") {
                    if (_brandname !== "") syslog_sms('i', _phone, _brandname + ': ' + _content);
                    else syslog_sms('i', _phone, _content);
                    if (typeof _successfunc === 'function') _successfunc();
                }
                else {
                    if (result === "-1") {
                        if (typeof _bannedfunc === 'function') _bannedfunc();
                    }
                    else {
                        if (typeof _errorfunc === 'function') _errorfunc();
                    }
                }
            }
        );
    }
    catch (ex) { }
}
//function SMSSYS_UpdateState (_idsms, _status) {
//    try {
//        AjaxJWT(url = "/api/SMS/SMSUpdateState"
//            , data = JSON.stringify({
//                'idsms': _idsms
//                , 'status': _status
//            })
//            , async = true
//            , success = function (result) { }
//        );
//    }
//    catch (ex) { }
//}
function SMSSYS_MultiAction (_datamess, _brandname, _successfunc, _errorfunc) {
    try {
        AjaxJWT(url = "/api/SMS/SendMulti"
            , data = JSON.stringify({
                'SmsItem': _datamess,
                'BrandName': _brandname,
                'CallbackMultiUrl': ''
            })
            , async = true
            , success = function (result) {
                if (result === "1") {
                    if (typeof _successfunc === 'function') _successfunc();
                }
                else {
                    if (result === "-1") {
                        if (typeof _bannedfunc === 'function') _bannedfunc();
                    }
                    else {
                        if (typeof _errorfunc === 'function') _errorfunc();
                    }
                }
            }
        );
    }
    catch (ex) { }
}
function SMSSYS_Getlog (_limit, _page, _from_time, _template_id, _znssms
    , _successfunc, _errorfunc) {
    try {
        AjaxJWT(url = "/api/SMS/SMSGetLog"
            , data = JSON.stringify({
                'znssms': _znssms,
                'template_id': _template_id,
                'from_time': _from_time,
                'page': _page,
                'limit': _limit
            })
            , async = true
            , success = function (result) {
                if (result != "0") {
                    if (typeof _successfunc === 'function') _successfunc(result);
                }
                else {
                    if (typeof _errorfunc === 'function') _errorfunc();
                }
            }
        );
    }
    catch (ex) { }
}

//#region

//#region // Send SMS Complete 
function MainCust_AfterChangeCard(_customerid, _data) {
    if (_data != undefined && _data.length != 0) {
        for (let ii = 0; ii < _data.length; ii++) {
            if (_data[ii].data_card_using != '') {
                let cardused = JSON.parse(_data[ii].data_card_using);
                if (cardused != undefined && cardused.length > 0)
                    SMSSYS_CardAfterChange(_customerid, (0 - cardused[0].Amount), cardused[0].CardID);
            }
        }
    }
}


async function SMSSYS_CardAfterChange(_customerid, _amount, _cardid) {
    return new Promise((resolve, reject) => {
        AjaxJWT(url = "/api/SMS/afterChange"
            , data = JSON.stringify({
                'customerid': _customerid
                , 'amount': formatNumber(_amount)
                , 'cardid': _cardid
                , 'templatetype': sys_SMSTEMPLATE.CARD
            })
            , async = true
            , success = function (result) {

            }
        );
    })
}

async function SMSSYS_AfterPaid(_customerid, _objCurrentID) {
    return new Promise((resolve, reject) => {
        let {
            PaymentID = 0,
            PaymentCardID = 0,
            PaymentMedID = 0,
            CardID = 0,
            DepositID = 0,
            MedicineID = 0,
            TabID = 0
        } = _objCurrentID;
        AjaxJWT(url = "/api/SMS/afterPaid"
            , data = JSON.stringify({
                'customerid': _customerid,
                'paymentid': PaymentID,
                'paymentcardid': PaymentCardID,
                'paymentmedid': PaymentMedID,
                'depositid': DepositID,
                'tabid': TabID,
                'cardid': CardID,
                'medicineid': MedicineID,
                'stupaymentid': 0,
                'templatetype': sys_SMSTEMPLATE.PAYMENT
            })
            , async = true
            , success = function (result) {

            }
        );
    })
}

async function SMSSYS_AfterApp(_customerid, _objCurrentID) {
    return new Promise((resolve, reject) => {
        
        AjaxJWT(url = "/api/SMS/afterAppointment"
            , data = JSON.stringify({
                'customerid': _customerid,
                'appid': _objCurrentID,
                'templatetype': sys_SMSTEMPLATE.APPOINTMENT
            })
            , async = true
            , success = function (result) {
                resolve();
            }
        );
    })
}

//#endregion