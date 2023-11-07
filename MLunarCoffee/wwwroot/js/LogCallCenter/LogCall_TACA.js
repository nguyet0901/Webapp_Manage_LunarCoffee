function AllLog_TACA(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, extension, page, URLRecord) {
    var data = [];
    var offset = 0
    if (page > 1) {
        offset = (page-1) * 100;
    } 
    var outbound = [];
    var inbound = [];
    if (extension != "") {
        outbound = LogCall_TACA(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, extension, offset, "caller_id_number");
        inbound = LogCall_TACA(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, extension, offset, "destination_number");
    }

    var allbound = $.merge(outbound, inbound);

    for (var i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = item.caller_id_number;
        element.to = item.destination_number;
        element.direction = item.direction;
        element.duration = item.billsec;
        element.state = item.call_status;
        element.recordUrl = item.record_path != null ? URLRecord + item.record_path : "null";
        element.start = item.start_stamp;
        element.status = (item.call_status == 'ANSWER' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}
function LogCall_TACA(APIbaseurl, Api_Key, DomainAPI, dateFrom, dateTo, extension, offset, typeBound) { 
    let dtResult;
    
    let _dF = DateFormat(dateFrom) + " 00:00:00";
    let _dT = DateFormat(dateTo) + " 23:59:59";

    let url = APIbaseurl + "/api/v2/cdr?domain_name=" + DomainAPI + "&from=" + _dF + "&to=" + _dT + "&limit=100&offset=" + offset + "&" + typeBound + "=" + extension + "&api_key=" + Api_Key;
  
    $.ajax({
        url: url, 
        headers: {
            "api-key": Api_Key
        },
        type: "GET", 
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        },
        success: function (result) { 
            let data = result.data
            if (data != null && data.length > 0) {
                dtResult = data;
            } else {
                dtResult = [];
            }
        }
    });
    return dtResult;

}