function AllLog_CMC_V2(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, extension, page) {
    var data = [];
    page = page - 1;
    var outbound = [];
    var inbound = [];
    if (extension != "") {
        outbound = LogCall_CMC_V2(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, 'outbound', extension, page);
        inbound = LogCall_CMC_V2(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, 'inbound', extension, page);
    }

    var allbound = $.merge(outbound, inbound);

    for (var i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = item.extension;
        element.to = item.phonenumber;
        element.direction = item.direction;
        element.duration = item.duration;
        element.state = item.state;
        element.recordUrl = item.recordUrl != "null" ? item.recordUrl : "";
        element.start = item.startedAt.replace("T", " ");
        element.status = (item.state == 'hangup' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}

function LogCall_CMC_V2(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, typebound, extension, page) { 
    let dtResult;
    let url = APIbaseurl + "/call-logs/" + typebound;
    let _dF = ConvertString_DMY_To_DateTime(dateFrom);
    let _dT = ConvertString_DMY_To_DateTime(dateTo);

    let DF_TimeSpan = _dF.getTime();
    let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime(); 
    let data = {};
    if (extension.toString().length < 9) {
        data = {
            'fromTime': DF_TimeSpan
            , 'queryTime': DT_TimeSpan
            , 'extension': extension
            , 'page': page
        }
    } else {
        data = {
            'fromTime': DF_TimeSpan
            , 'queryTime': DT_TimeSpan
            , 'phonenumber': extension
            , 'page': page
        }
    }   

    $.ajax({
        url: url,
        dataType: "json",
        headers: {
            "Authorization": Api_Key,
            "Domain": DomainAPI
        },
        type: "POST",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        },
        success: function (result) {
            let data = result.data.items
            if (data.length > 0) {
                dtResult = data;
            } else {
                dtResult = [];
            }
        }
    });
    return dtResult;

}