
//#region //success,info,danger,warning,other
async function notif_initialize (_type, _mes, _avatar, _title, _delay) {
    let _id = (new Date()).getTime() + RandomNumber();
    let _pa = 'MS_Notification';
    let _rel = await notif_createel(_id, _pa, _type, _mes, _avatar, _title);
    if (_rel == 'resolved') {
        notif_closeauto(_id, _delay);
        notif_progressrun(_id, _delay);
        notif_closeevent(_id)
        //notif_timmersend(_id);
    }

}
function notif_closeauto (_id, _delay) {
    return new Promise(resolve => {
        setTimeout(() => {
            if ($('#nt' + _id).length) $('#nt' + _id).remove();
        }, _delay);
    });
}
function notif_progressrun (_id, _delay) {
    let seco = _delay / 200;
    return new Promise(resolve => {
        let elem = document.getElementById('pb' + _id);
        if (typeof (elem) != 'undefined' && elem != null) {
            let width = 1;
            let id = setInterval(frame, seco);
            function frame () {
                if (width >= 100) {
                    clearInterval(id);
                } else {
                    width++;
                    elem.style.width = width + "%";
                }
            }
        }
    });
}
function notif_createel (_id, _pa, _type, _mes, _avatar, _title) {
    let type = '', mess = '', per = '', title = '', time = '';
    per = '<div class="progress bg-transparent">'
        + '<div id="pb' + _id + '" class="progress-bar bg-gradient-light" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>'
        + '</div>'
    let icon = '<i class="ni ni-bell-55 text-white me-2"></i>';
    time = '<small id="tm' + _id + '" class="text-white">' + Outlang["Bay_gio"] + '</small>'
    switch (_type) {
        case 'success':
            type = 'bg-gradient-success'; title = Outlang["Thanh_cong"];
            break;
        case 'info':
            type = 'bg-gradient-info'; title = Outlang["Chu_y"];
            break;
        case 'danger':
            type = 'bg-gradient-danger'; title = Outlang["Xu_ly_that_bai"];
            break;
        case 'warning':
            type = 'bg-gradient-warning'; title = Outlang["Canh_bao"];
            break;
        case 'message':
            type = 'bg-gradient-info';
            title = _title != "" ? _title : Outlang["Tin_nhan"];
            icon = _avatar != "" ? ('<img class="avatar avatar-xs border-radius-md shadow-sm me-2" src="' + _avatar + '" alt="label-image">') : icon;
            break;
        default:
            type = 'bg-white'; title = Outlang["Chu_y"];
            break;
    }
    mess = (_mes != '' && _mes != undefined) ? ('<div class="toast-body text-white"> ' + _mes + '</div>') : '';
    return new Promise(resolve => {
        let content = '<div class="toast fade show p-2 mt-2 ' + type + '" role="alert" id="nt' + _id + '" >'
            + '<div class="toast-header bg-transparent border-0">'
            + icon
            + '<span class="me-auto text-white font-weight-bold">' + title + '</span>'
            + time
            + '<i id="cl' + _id + '"class="fas fa-times text-md text-white ms-3 cursor-pointer" data-bs-dismiss="toast" aria-label="Close"></i>'
            + '</div>'
            + '<hr class="horizontal light m-0">'
            + mess
            + per
            + '</div>'
        let el = createElementFromHTML(content)
        $("#" + _pa).append(el);
        resolve('resolved');
    });
}
function notif_closeevent (_id) {
    $("#cl" + _id).unbind('click').click(function (event) {
        if ($('#nt' + _id).length) $('#nt' + _id).remove();

    });
}
function notif_timmersend (_id) {
    let seco = 10000;
    return new Promise(resolve => {
        let _timenow = DateNowHHMM();
        let id = setInterval(function () {frame(_timenow)}, seco);
        function frame (_timenow) {
            let elem = document.getElementById('tm' + _id);
            if (typeof (elem) != 'undefined' && elem != null) {
                let timecurrent = DateNowHHMM();
                let _timeago = HHMM_Distance_HHMM(_timenow, timecurrent);
                _timeago = _timeago < 0 ? 0 : _timeago;
                let _timeago_text = ChangeMinute_To_Hour_Minute(_timeago, 'ago');
                if ($('#tm' + _id).length) $('#tm' + _id).html((_timeago_text != '') ? _timeago_text : Outlang["May_giay_truoc"]);

            }
            else clearInterval(id);
        }

    });
}
//#endregion

function notiError_SW () {
    notif_initialize('danger', 'Something Wrongs', avatar = '', title = '', 5000);
}
function notiSuccess () {
    notif_initialize('success', '', avatar = '', title = '', 2000);
}
function notiSuccessMess (mes) {
    notif_initialize('success', mes, avatar = '', title = '', 2000);
}
function notiError (mes) {
    notif_initialize('danger', mes, avatar = '', title = '', 5000);
}
function notiWarning (mes) {
    notif_initialize('warning', mes, avatar = '', title = '', 5000);
}
function notiMess10 (mes) {
    notif_initialize('info', mes, avatar = '', title = '', 5000);
}
function notiMessage (title, avatar, mes) {
    notif_initialize('message', mes, avatar, title, 5000);
}
function notiAlertinfo (title, mes, time) {
    notif_initialize('message', mes, avatar = '', title = title, time);
}
//#region //Confirmation
function confirmf_initialize (title, text, isconfirm, iscancel, icon = 'warning', textconfirm = '<i class="fas fa-check pe-2"></i>Yes', textcancel = '<i class="fas fa-times text-danger pe-2"></i>Cancel') {
    return new Promise(resolve => {
        let swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn bg-gradient-primary btn-outline-success mx-2',
                cancelButton: 'btn bg-gradient-light mx-2'
            },
            buttonsStyling: false
        })
        swalWithBootstrapButtons.fire({
            title: title,
            html: text,
            icon: icon,
            showConfirmButton: isconfirm,
            showCancelButton: iscancel,
            confirmButtonText: textconfirm,
            cancelButtonText: textcancel,
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                resolve(true);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                resolve(false);
            } else {
                resolve(false);
            }
        })
    });


}
//#endregion

async function notiConfirm (content) {

    let re = await confirmf_initialize(Outlang["Xac_nhan"]
        , content != undefined ? content : (Outlang["Ban_co_muon_tiep_tuc"] + '?')
        , true, true
    );

    if (!re) return new Promise();
    else return null;


}
async function notiConfirm_Type(type) {
    let content = Outlang[type];
    let re = await confirmf_initialize(Outlang["Xac_nhan"]
        , content !== undefined && content !== "" ? content : (Outlang["Ban_co_muon_tiep_tuc"] + ' ? ')
        , true, true
    );
    if (!re) return new Promise();
    else return null;
}
async function notiPopup_Type(content = '') {
    let re = await confirmf_initialize(Outlang["Thong_bao"]
        , content != undefined ? content : ''
        , false, true
    );
    if (!re) return new Promise();
    else return null;
}
async function notiPopup_Type_Service (type, data) {
    let content = "";
    for (_notiindex = 0; _notiindex < language_global_noti_text.length; _notiindex++) {
        if (language_global_noti_text[_notiindex].title == type) {
            content = language_global_noti_text[_notiindex].value[(Global_Language_Chossing ? Global_Language_Chossing : "vn")];
            _notiindex = language_global_noti_text.length;
        }
    }
    for (let i = 0; i < data.length; i++) {
        content = content.replace("{" + i + "}", data[i].toString());
    }
    let re = await confirmf_initialize(Outlang["Thong_bao"]
        , content != undefined ? content : (Outlang["Ban_co_muon_tiep_tuc"] + ' ? ')
        , false, true
    );
    if (!re) return new Promise();
    else return null;
}



