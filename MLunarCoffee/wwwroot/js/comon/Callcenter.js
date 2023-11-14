
// #region  General Function
var CC_TotalSeconds = 0;
function CCF_CallFail (phonenumber) {
 
    if (Incommingaudio != undefined && Incommingaudio != null) {
        Incommingaudio.pause();
        Incommingaudio.currentTime = 0;
    }
    $('#CallCenter_Area').addClass('d-none');
    CCF_ResetFiled();
    flagCalling = 0;
}
function CCF_BeginTime () {
    CC_TotalSeconds = 0;

}
function CCF_setTime () {
    ++CC_TotalSeconds;
    $('#CallCenter_ProgressingTime').html(CCF_pad(parseInt(CC_TotalSeconds / 60)) + ':' + CCF_pad(CC_TotalSeconds % 60));
}
function CCF_pad (val) {
    let valString = val + "";
    if (valString.length < 2) {
        return "0" + valString;
    } else {
        return valString;
    }
}
function CCF_Event () {
    $("#CallCenter_ProgressingMute").on("click", function () {
        $("#CallCenter_ProgressingUnMute").removeClass('d-none');
        $(this).addClass('d-none');
        CallingMute();
    });
    $("#CallCenter_ProgressingUnMute").on("click", function () {
        $("#CallCenter_ProgressingMute").removeClass('d-none');
        $(this).addClass('d-none');
        CallingUnMute();
    });
    $("#CallCenter_ProgressingEndCall").on("click", function () {
        sip_hangup();
        $('#CallCenter_Area').addClass('d-none');
        CCF_ResetFiled();
    });
}
function CCF_ResetFiled () {
    CC_TotalSeconds = 0;
    clearInterval(interval);
    interval = null;
    $('#CallCenter_HangDiv').removeClass('d-none');
    $('#CallCenter_ProgressingArea').addClass('d-none');
    $('#CallCenter_ProgressingMute').removeClass('d-none');
    $('#CallCenter_ProgressingUnMute').addClass('d-none');
    $('#CallCenter_Name').html(Outlang["Khong_co_ho_so"]);
    $('#CallCenter_Phone').html('');
    $('#CallCenter_ProgressingTime').html('');
    $('#CallCenter_Avatar').attr('src', '/assests/img/gray.png');
    $('#CallCenter_MultiPhone').html('');
}


//#endregion

// Incoming call
function sip_incomingcall (header, headername) {
    if (flagCalling == 0) {
        try {
            CCF_ResetFiled();
            $('#CallCenter_Area').removeClass('d-none');
            Incommingaudio.loop = true;
            Incommingaudio.play();
            CCF_Incom_DetectPhone(header, headername);
        }
        catch (err) {
            CCF_Incom_DetectPhone(header, headername);
        }
        
    }
}
function CCF_Incom_DetectPhone (header, headername) {
    let phonenumber = '';
    let name = '';
    let _defaultImage = "/assests/img/gray.png";
    $('#CallCenter_Avatar').attr('src', _defaultImage);
    $('#CallCenter_Header').html(Outlang["Cuoc_goi_den"] || "Cuộc gọi đến");
    if (headername != "0") {
        if(headername.indexOf(":") > 0){
            headername = headername.split(":")[0] ?? headername;
        }
        phonenumber = headername;
    }
    else {
        let resdouble = header.split(":");
        let res = resdouble[1].split("@");
        phonenumber = res[0];

    }
    AjaxJWT(url = "/api/Call/InCommingCall"
        , data = JSON.stringify({phone: phonenumber})
        , async = true
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result)
                name = data[0].Name;
                let custid = data[0].CustID;
                let custcode = data[0].Code;
                if (custid != '') {
                    $("#CallCenter_Name").html('<a target="_blank" class="text-white" href="/Customer/MainCustomer?CustomerID=' + custid + '">' + custcode + ' - ' + name + '</a>');
                }
                else {
                    $("#CallCenter_Name").html(name);
                }
                $("#CallCenter_Phone").html(phonenumber);
                $('#CallCenter_Avatar').attr('src', (data[0].Image ? data[0].Image : _defaultImage));
            } else {
                $("#CallCenter_Name").html(Outlang["Khong_co_ho_so"]);
                $("#CallCenter_Phone").html(phonenumber);
                $('#CallCenter_Avatar').attr('src', _defaultImage);
            }
        }
    );
}
function CCF_Incom_Accept () {
    try {
        Incommingaudio.pause();
        Incommingaudio.currentTime = 0;
        $('#CallCenter_HangDiv').addClass('d-none');
        $('#CallCenter_ProgressingArea').removeClass('d-none');
        CCF_Event();
        CallingAnswer();
        flagCalling = 1;
        return false;
    }
    catch (err) {
        flagCalling = 0;
        return false;
    }
}
function CCF_Incom_Decline () {

    if (Incommingaudio != undefined && Incommingaudio != null) {
        Incommingaudio.pause();
        Incommingaudio.currentTime = 0;
    }
    sip_hangup();
    $('#CallCenter_Area').addClass('d-none');
    CCF_ResetFiled();
    flagCalling = 0;
    return false;
}

//#endregion

// Outcoming call
function CCF_OutcomCall (phone, customer, ticket, line) {
    CCF_ResetFiled();
    if (CCOutsite_Line != "" && CCOutsite_Link != "") {
        AjaxJWT(url = "/api/Call/DetectPhone"
            , data = JSON.stringify({
                cus: customer
                , tic: 0
                , line: 0
                , linefromcall: CCOutsite_Line
                , linkfromcall: CCOutsite_Link
            })
            , async = true
            , success = function (result) {
                if (result === '-1') { // error
                    notiWarning(Outlang["Khong_ton_tai"]);
                }
                else {
                    if (result === '0') { // call backend
                    }
                    else {
                        CCF_Call_Outsize(linkClickToCall, callextension, callApi_Key, calldomainApi, result, line);
                    }
                }
            }
        );
    }
    else {
        $('#CallCenter_HangDiv').addClass('d-none');
        $('#CallCenter_Header').html(Outlang["Cuoc_goi_di"]);
        Incommingaudio.pause();
        Incommingaudio.currentTime = 0;

        AjaxJWT(url = "/api/Call/OutCommingCall"
            , data = JSON.stringify({
                cus: customer
                , tic: ticket
                , line: line
            })
            , async = true
            , success = function (result) {
                let data = JSON.parse(result);
                let dataoutcomming = data.Table;
                let dataPhone = data.Table1;
                let phone1 = dataPhone[0].Phone1;
                let phone2 = dataPhone[0].Phone2;
                if (dataoutcomming != null && dataoutcomming.length != 0) {
                    $('#CallCenter_Name').html((dataoutcomming[0].ID != "0")
                        ? '<a target="_blank" class="text-white" href="/Customer/MainCustomer?CustomerID=' + dataoutcomming[0].ID + '">' + dataoutcomming[0].Code + ' - ' + dataoutcomming[0].Name + '</a>'
                        : dataoutcomming[0].Name
                    );
                    $('#CallCenter_Avatar').attr('src', (dataoutcomming[0].Image != "")
                        ? dataoutcomming[0].Image
                        : Master_Default_Pic
                    );
                }
                else {
                    $('#CallCenter_Name').html(Outlang["Khong_co_ho_so"]);
                    $('#CallCenter_Avatar').attr('src', Master_Default_Pic);
                }
                if (phone1 != '' && phone2 != '') {
                    CCF_Outcom2Phone(phone1, phone2);
                } else {
                    if (line != 0) {
                        sip_callexe(line, function (val) {
                            syslog_call('i', val)
                        });
                        $('#CallCenter_Phone').html(line);
                    }
                    else {

                        sip_callexe(phone1, function (val) {
                            syslog_call('i', val)
                        });
                        //$('#CallCenter_Phone').html(phone1.slice(0, -3) + '***');
                        $('#CallCenter_Phone').html(phone1);
                    }
                    $('#CallCenter_ProgressingArea').removeClass('d-none');
                }
                Checking_TabControl_Permission();
            }
        );


        $('#CallCenter_Area').removeClass('d-none');
        CCF_Event();
        return false;
    }

}
async function CCF_Call_Outsize (_linkClickToCall, _callextension, _callApi_Key, _calldomainApi, _phone, _line) {
    new Promise((resolove) => {
        let _url = `${_linkClickToCall}/from_number/${_callextension}/to_number/${_phone}/key/${_callApi_Key}/domain/${_calldomainApi}`;
        $.get(_url, function (data) {
            if (data != "Success") notiWarning("Error");
        });
    })
}
function CCF_OutcomCallPhone (phonenumber) {
    let _defaultImage = "/assests/img/gray.png";
    if (phonenumber.length >= 3) {
        $('#CallCenter_HangDiv').addClass('d-none');
        $('#CallCenter_Header').html(Outlang["Cuoc_goi_di"]);
        Incommingaudio.pause();
        Incommingaudio.currentTime = 0;
        AjaxJWT(url = "/api/Call/InCommingCall"
            , data = JSON.stringify({phone: phonenumber})
            , async = true
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result)
                    let name = data[0].Name;
                    let custid = data[0].CustID;
                    let custcode = data[0].Code;
                    if (custid != '') {
                        $("#CallCenter_Name").html('<a target="_blank" class="text-white" href="/Customer/MainCustomer?CustomerID=' + custid + '">' + custcode + ' - ' + name + '</a>');
                    }
                    else {
                        $("#CallCenter_Name").html(name);
                    }
                    $('#CallCenter_Avatar').attr('src', (data[0].Image != "" ? data[0].Image : Master_Default_Pic));
                } else {
                    $("#CallCenter_Name").html(Outlang["Khong_co_ho_so"]);
                    $('#CallCenter_Avatar').attr('src', Master_Default_Pic);
                }
                sip_callexe(phonenumber, function (val) {
                    syslog_call('i', val)
                });
                $('#CallCenter_ProgressingArea').removeClass('d-none');
                $('#CallCenter_Phone').html(phonenumber);
                Checking_TabControl_Permission();
            }
        );
        $('#CallCenter_Area').removeClass('d-none');
        CCF_Event();
        return false;
    }
}

function CCF_Outcom2Phone (phone1, phone2) {
    let Multi_phone1 = (phone1.length > 0 ? encrypt_phone(phone1.replace(" ", "")) : "");
    let Multi_phone2 = (phone2.length > 0 ? encrypt_phone(phone2.replace(" ", "")) : "");
    let str =
        `
        <span>${Outlang["Chon_sdt"]}</span>
        <div class="d-flex mt-2">
            <span data-info="${Multi_phone1}" data-tab="phone_customer" class="_tab_control_ call w-50 text-dark text-sm me-3 badge bg-gradient-light cursor-pointer my-1">
                ${phone1}
            </span>
            <span data-info="${Multi_phone2}" data-tab="phone_customer" class="_tab_control_ call w-50 text-dark text-sm badge bg-gradient-light cursor-pointer my-1">
                ${phone2}
            </span>
        </div>`

    $('#CallCenter_MultiPhone').html(str);
    $("#CallCenter_MultiPhone .call").on("click", function () {
        let phoneExe = $(this).attr("data-info");
        $("#CallCenter_Phone").html($(this).html());
        $("#CallCenter_MultiPhone").html('');
        $('#CallCenter_ProgressingArea').removeClass('d-none');
        sip_callexe(decrypt_phone(phoneExe), function (val) {
            syslog_call('i', val)
        });
        return false;
    });
}
//#endregion
