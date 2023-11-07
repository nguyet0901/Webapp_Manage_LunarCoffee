
var imageAddr = "";
var downloadSize = 4995374; //bytes
var is_chrome = navigator.userAgent.indexOf('Chrome') > -1;
var is_safari = navigator.userAgent.indexOf("Safari") > -1;

function ShowProgressMessage(msg, speedMbps) {
    let oProgress = document.getElementById("StatusOfNetwork");
    if (oProgress) {
        oProgress.innerHTML = msg;
        if (Number(speedMbps) > 200) {
            oProgress.style.color = "#00d933";
            oProgress.classList.add("text-success");
            oProgress.classList.remove("text-danger");
        }
        else {
            oProgress.style.color = "#c68d8d";
            oProgress.classList.remove("text-success");
            oProgress.classList.add("text-danger");
        }
    }
}
function InitiateSpeedDetection() {
    ShowProgressMessage(Outlang["Dang_kiem_tra_ket_noi_vui_long_cho"] + "...");
    window.setTimeout(MeasureConnectionSpeed, 1);
};
function CheckConnection(image) {
    if (image != "") {
        imageAddr = image;
        InitiateSpeedDetection();
    }
    else {
        let oProgress = document.getElementById("StatusOfNetwork");
        oProgress.innerHTML = Outlang["Kiem_tra_that_bai"];
    }
    return false;
}
function MeasureConnectionSpeed() {

    var startTime, endTime;
    var download = new Image();
    download.onload = function () {
        endTime = (new Date()).getTime();
        showResults();
    }
    download.onerror = function (err, msg) {
        ShowProgressMessage(Outlang["Khong_hop_le_hoac_kiem_tra_that_bai"]);
    }
    startTime = (new Date()).getTime();
    var cacheBuster = "?nnn=" + startTime;
    download.src = imageAddr + cacheBuster;
    function showResults() {
        var duration = (endTime - startTime) / 1000;
        var bitsLoaded = downloadSize * 8;
        var speedBps = (bitsLoaded / duration).toFixed(2);
        var speedKbps = (speedBps / 1024).toFixed(2);
        var speedMbps = (speedKbps / 1024).toFixed(2);
        ShowProgressMessage((Outlang["Toc_do_truy_cap"] + "&nbsp;" + speedMbps + " vts"), speedMbps);
    }
}

function CheckingvoiceDevice() {
    let oProgress = document.getElementById("StatusOfVoiceDevice");
    navigator.getUserMedia = navigator.getUserMedia ||
        navigator.webkitGetUserMedia ||
        navigator.mozGetUserMedia || navigator.msGetUserMedia;

    if (navigator.getUserMedia) {
        navigator.getUserMedia({ audio: true, video: false },
            function (stream) {
                oProgress.innerHTML = Outlang["Da_ket_noi_microphone"];
                oProgress.classList.add("text-success");
                oProgress.classList.remove("text-danger");
            },
            function (err) {
                oProgress.innerHTML = Outlang["Khong_co_ket_noi_den_microphone"];
                oProgress.classList.add("text-danger");
                oProgress.classList.remove("text-success");
            }
        );

    } else {
        oProgress.innerHTML = Outlang["Khong_co_ket_noi_den_microphone"];
        oProgress.classList.add("text-danger");
        oProgress.classList.remove("text-success");
    }
}
function GetMedia_Mobile_Browser_Ios() {
    let oProgress = document.getElementById("StatusOfVoiceDevice");
    try {
        var constraints = {
            audio: true,
            video: false
        }

        navigator.mediaDevices.getUserMedia(constraints).then(function success(stream) {
            oProgress.innerHTML = Outlang["Quyen_truy_cap_bi_tu_choi"];
            oProgress.classList.add("text-success");
            oProgress.classList.remove("text-danger");
        });
    }
    catch(ex)  {
        oProgress.innerHTML = Outlang["Da_truy_cap"];
        oProgress.classList.add("text-danger");
        oProgress.classList.remove("text-success");
    }
}
function Callcenter_CheckUsing(usingCallCenter, checkingCallCenter, callextension, linkClickToCall
    , calloutbound, calllogintimeexpired, callport, calldomain, callpassword
) {
    let oProgress = document.getElementById("StatusOfLineCenter");

    if (usingCallCenter == 1 && checkingCallCenter == 1) {
        try {
            Call_loginExtend(calloutbound, callport, calldomain, callextension, callpassword, calllogintimeexpired);
            Call_newRTCSession();
        } catch (ex){
        }
    }
    else if (usingCallCenter == 1 && checkingCallCenter == 2) {
        if (callextension != "" && linkClickToCall != "") {
            oProgress.innerHTML = Outlang["Duong_truyen"] + "&nbsp;" + callextension.toString();
            oProgress.style.color = "rgb(0, 217, 51)";
            CCOutsite_Line = callextension;
            CCOutsite_Link = linkClickToCall;

        } else {
            oProgress.innerHTML = Outlang["Khong_su_dung_trung_tam_cuoc_goi"];
            if (oProgress.style.color == "#767676") oProgress.style.color = "#f2711c";
        }
    } else {
        oProgress.innerHTML = Outlang["Khong_su_dung_trung_tam_cuoc_goi"];
        if (oProgress.style.color == "#767676") oProgress.style.color = "#f2711c";
    }
}



function isSafariBrowser() {
    if (is_safari) {
        if (is_chrome)
            return false;
        else
            return true;
    }
    return false;
}