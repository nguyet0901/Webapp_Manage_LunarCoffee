function AllLog_CloudFone(APIbaseurl, DomainAPI, UserName, Password, dateFrom, dateTo, extension, page) {
    var data = [];
    var outbound = [];
    var inbound = [];

    if (extension != "") {
        outbound = LogCall_CloudFone(APIbaseurl, DomainAPI, UserName, Password, dateFrom, dateTo, '1', extension, page);
        inbound = LogCall_CloudFone(APIbaseurl, DomainAPI, UserName, Password, dateFrom, dateTo, '2', extension, page);
    }

    var allbound = $.merge(outbound, inbound);

    for (var i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = formatHTML(item.soGoiDen);
        element.to = item.soNhan;
        element.direction = item.typecall != null ? item.typecall : "";
        element.duration = item.thoiGianThucGoi;
        element.state = item.trangThai;
        element.recordUrl = item.linkFile != "" ? item.linkFile : "null";
        element.start = item.ngayGoi.replace("T", " ");
        element.status = (item.trangThai == 'ANSWERED' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}

function LogCall_CloudFone(APIbaseurl, DomainAPI, UserName, Password, dateFrom, dateTo, typeBound, extension, page) { 
    let dtResult;
    let url = APIbaseurl + "/api/CloudFone/GetCallHistory";
    let _dF = DateFormat(dateFrom) + " 00:00:00";
    let _dT = DateFormat(dateTo) + " 23:59:59";

    let CallNum = (typeBound == "1" ? extension : ""); //gọi vào 
    let ReceiveNum = (typeBound == "2" ? extension : ""); //gọi ra 
     
    $.ajax({
        url: url, 
        type: "POST",
        data: JSON.stringify({
            'ServiceName': DomainAPI
            , 'AuthUser': UserName
            , 'AuthKey': Password
            , 'TypeGet': 0
            , 'DateStart': _dF
            , 'DateEnd': _dT
            , 'CallNum': CallNum
            , 'ReceiveNum': ReceiveNum
            , 'Key': ''
            , 'PageIndex': page
            , 'PageSize': 200
           
        }),
        contentType: 'application/json',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
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