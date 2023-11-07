function AllLog_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, phone_ext) {
    var data = [];
    var allbound = [];
    var inbound = [];
    var outbound = [];
    if (phone_ext == "phone") {
        if (extention != '') {
            inbound = LogCall_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, 'callee')
            outbound = LogCall_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, 'caller')
        }
    }
    else if (phone_ext == "ext") {
        if (extention != '') {
            inbound = LogCall_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, 'callee')
            outbound = LogCall_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, 'caller')
        }
    }
    allbound = $.merge(inbound, outbound)
    for (let i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];

        element.from = item.caller;
        element.to = item.callee;
        element.direction = item.direction;
        element.duration = item.ring_duration;
        element.state = item.call_status;
        element.recordUrl = item.recording_file_url;
        element.start = ConvertDateTimeToStringYMDHHMMSS(item.start_time);
        element.status = (item.call_status == 'ANSWERED' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}
function LogCall_ETelecom(APIbaseurl, Api_Key, dateFrom, dateTo, extention, type) {
    let url = '';
    let dtresult;

    let _dF = ConvertString_DMY_To_DateTime(dateFrom);
    let _dT = ConvertString_DMY_To_DateTime(dateTo);

    let DF_TimeSpan = _dF.getTime() / 1000;
    let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime() / 1000;
    if (type == 'caller') {
        url = APIbaseurl + "?caller=" + extention + '&start_time=' + DF_TimeSpan + '&end_time=' + DT_TimeSpan;
    }
    else if (type == 'callee') {
        url = APIbaseurl + "?callee=" + extention + '&start_time=' + DF_TimeSpan + '&end_time=' + DT_TimeSpan;
    }

    $.ajax({
        url: url
        , contentType: "application/json; charset=utf-8"
        , headers: {
            "Authorization": "Bearer " + Api_Key
        }
        , type: "GET"
        , async: false
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        }
        , success: function (result) {
            let data = result.sessions;
            if (data != null && data.length > 0) {
                dtresult = data;
            }
            else {
                dtresult = [];
            }
        }
    });
    return dtresult;
}