
//#region //


function AllLog_Callio(APIbaseurl, Api_Key, dateFrom, dateTo, extention, phone_ext, page) {

    var data = []; 
    var userext = LogCall_Callio_GetUser(APIbaseurl, Api_Key, extention);
    var allbound = LogCall_Callio(APIbaseurl, Api_Key, dateFrom, dateTo, userext, extention, page);

    for (let i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = item?.fromExt.toString();
        element.to = item?.toNumber;
        element.direction = item?.direction;
        element.duration = item?.billDuration;
        element.state = (item?.hangupCause == 'NORMAL_CLEARING' ? 'SUCCESS' : '');
        element.recordUrl = '';
        element.start = ConvertTimeSpanToFullDateTime(item?.startTime / 1000);
        element.status = (item?.hangupCause == 'NORMAL_CLEARING' ? 'SUCCESS' : '');
        data.push(element);
    }
    return data;
}

function LogCall_Callio(APIbaseurl, Api_Key, dateFrom, dateTo, user, extention, page) {
    let dtResult;
    let _dF = ConvertString_DMY_To_DateTime(dateFrom);
    let _dT = ConvertString_DMY_To_DateTime(dateTo);

    let DF_TimeSpan = _dF.getTime() /// 1000;
    let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime() // / 1000;
    let urlLogCall = APIbaseurl + '/call?page=' + page + '&pageSize=10000&from=' + DF_TimeSpan + '&to=' + DT_TimeSpan + "&user=" + user;
    $.ajax({
        url: urlLogCall
        , contentType: "application/json; charset=utf-8"
        , headers: {
            "token": Api_Key
        }
        , type: "GET"
        , async: false
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        }
        , success: function (result) {
            debugger
            let data = result?.docs;
            let dataByExt = data.filter((item) => { return item.fromExt == extention });
            if (dataByExt != null && dataByExt.length > 0) {
                dtResult = data;
            }
            else {
                dtResult = [];
            }
        }
    });

    return dtResult;

}


function LogCall_Callio_GetUser(APIbaseurl, Api_Key, extention) {

    let urlGetUser = APIbaseurl + '/user?page=1&pageSize=500';
    let userid = '';
    $.ajax({
        url: urlGetUser
        , contentType: "application/json; charset=utf-8"
        , headers: {
            "token": Api_Key
        }
        , type: "GET"
        , async: false
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError_SW();
        }
        , success: function (result) {
            let data = result?.docs;
            let dataByExt = data.find((item) => { return item.ext == extention });
            if (dataByExt && Object.entries(dataByExt).length != 0) {
                userid = dataByExt?.id
            }
            else userid = "";
        }
    });

    return (userid != undefined ? userid : "");
}

// #endregion
