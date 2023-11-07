function AllLog_Omicall(APIbaseurl, Api_Key, dateFrom, dateTo, extention, phone_ext, page) {
    var data = [];
    var allbound = [];
    var inbound = [];
    var outbound = [];
    if (phone_ext == "phone") {
        if (extention != '') { 
            outbound = LogCall_Omicall(APIbaseurl, Api_Key, dateFrom, dateTo, extention, page, 'keyword')
        }
    }
    else if (phone_ext == "ext") {
        if (extention != '') {
            //inbound = LogCall_Omicall(APIbaseurl, Api_Key, dateFrom, dateTo, extention, page, 'keyword')
            outbound = LogCall_Omicall(APIbaseurl, Api_Key, dateFrom, dateTo, extention, page, 'sip_user')
        }
    }
    allbound = $.merge(inbound, outbound)
    for (let i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];

        element.from = item.sip_user;
        element.to = item.destination_number;
        element.direction = item.direction;
        element.duration = item.bill_sec;
        element.state = item.disposition;
        element.recordUrl = (item.recording_file != null ? item.recording_file : '');
        element.start = ConvertTimeSpanToDateTime(item.time_start_to_answer);
        element.status = (item.disposition == 'answered' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}
function LogCall_Omicall(APIbaseurl, Api_Key, dateFrom, dateTo, extention, page, type) {
    let url = '';
    let dtresult;

    let _dF = ConvertString_DMY_To_DateTime(dateFrom);
    let _dT = ConvertString_DMY_To_DateTime(dateTo);

    let DF_TimeSpan = _dF.getTime();
    let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime();
    if (type == 'sip_user') {
        url = APIbaseurl + "/call_transaction/list?sip_user=" + extention + '&from_date=' + DF_TimeSpan + '&to_date=' + DT_TimeSpan + '&page=' + page;
    }
    else if (type == 'keyword') {
        url = APIbaseurl + "/call_transaction/list?keyword=" + extention + '&from_date=' + _dT.addDays(-29).getTime() + '&to_date=' + DT_TimeSpan + '&page=' + page;
    }

    let auth = Auth_Omicall(APIbaseurl, Api_Key);

    $.ajax({
        url: url
        , contentType: "application/json; charset=utf-8"
        , headers: {
            "Authorization": "Bearer " + auth
        }
        , type: "GET"
        , async: false
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        }
        , success: function (result) {
            let data = result.payload;
            if (data != null && Object.keys(data).length > 0) {
                dtresult = data.items;
            }
            else {
                dtresult = [];
            }
        }
    });
    return dtresult;
}
function Auth_Omicall(APIbaseurl, apikey) {
    let token = '';
    let url = APIbaseurl + '/auth?apikey=' + apikey
    $.ajax({
        url: url
        , contentType: "application/json"
        , headers: {
            "Content-Type": "application/json"
        }
        , type: "GET"
        , async: false
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        }
        , success: function (result) {
            let data = result.payload;
            if (data != null && Object.keys(data).length > 0) {
                token = data.access_token;
            }
            else {
                token = '';
            }
        }
    }); 

    return token;
}