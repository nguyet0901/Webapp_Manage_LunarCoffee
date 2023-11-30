
//#region //Load logs from db VTT
let dtAudio = [];
function AllLog_FPT_Oncall(urlVerify, urlLoadRecord, urlRecord, username, password, dateFrom, dateTo, extension, limit, phone_ext)
{

    var data = [];
    var allbound = LogCall_FPT_Oncall(dateFrom, dateTo, extension);

    for (var i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = item.caller.toString();
        element.to = item.callee;
        element.direction = item.direction;
        element.duration = item.duration;
        element.state = item.call_status;
        element.recordUrl = "";
        element.start = ConvertTimeSpanToFullDateTime(item.start);
        element.status = (item.call_status == 'ANSWERED' ? 'SUCCESS' : '')
        data.push(element);
    }
    return data;
}

function LogCall_FPT_Oncall(dateFrom, dateTo, extension)
{
    let dtResult;
    let _dF = ConvertString_DMY_To_DateTime(dateFrom);
    let _dT = ConvertString_DMY_To_DateTime(dateTo);

    let DF_TimeSpan = _dF.getTime() / 1000;
    let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime() / 1000;
    AjaxLoad(
        url = "/Marketing/Call/HistoryCall/?handler=LoadLogs"
        , data = {
            'dateFrom': DF_TimeSpan
            , 'dateTo': DT_TimeSpan
            , 'extension': extension
        }
        , async = false
        , error = function () { notiError_SW(); }
        , success = function (result)
        {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data.length > 0) {
                    dtResult = data;
                } else {
                    dtResult = [];
                }
            } else {
                dtResult = [];
            }
        }
    );

    return dtResult;

}
// #endregion

//#region // Load audio from API Oncall
function LogCall_FTP_Author(urlVerify, username, password)
{
    let strToken = "";
    try {
        let objToken = author_get('logcalltoken');
        if (objToken != undefined && objToken !== "" && objToken !== "0") {
            let data = JSON.parse(objToken);
            if (data != undefined && Object.keys(data).length > 0) {
                let expiredDate = data?.timeout ?? "";
                let timeout = new Date(expiredDate);
                let currDate = new Date(Date.now() - 1000 * 60);
                if (data.token != "" && currDate < timeout) {
                    return "";
                }
            }
        }
        $.ajax({
            url: urlVerify,
            dataType: "json",
            type: "POST",
            data: JSON.stringify({ "name": username, "password": password }),
            contentType: 'application/json; charset=utf-8',
            async: false,
            error: function (XMLHttpRequest, textStatus, errorThrown)
            {

            },
            success: function (result)
            {
                let value = {};
                if (result != "" && result != "null" && result != "0") {
                    let obj = JSON.parse(JSON.stringify(result));
                    if (obj != undefined && Object.keys.length > 0) {
                        debugger
                        let expiresDate = ConvertDateTimeToTimeSpan(new Date(Date.now() + 1000 * obj?.expires));
                        strToken = obj.access_token ?? ""
                        value = {
                            token: strToken,
                            timeout: expiresDate
                        }
                        strToken = obj.access_token;
                    }
                }
                else {
                    //notiWarning('Callcenter không kết nối!');
                }
                author_set('logcalltoken', JSON.stringify(value));
                return strToken;
            }
        });
    } catch (ex) {
        author_set('logcalltoken', '0');
        return strToken;
    }
}
function LogCall_FTP_LoadAudio(urlVerify, username, password, urlLoadRecord, ext, dateFrom, dateTo, limit)
{

    let data_audio = [];
    try {
        let objToken = author_get('logcalltoken');
        let strToken = "";
        if (objToken != undefined && objToken !== "" && objToken !== "0") {
            let data = JSON.parse(objToken);
            if (data != undefined && Object.keys(data).length > 0) {
                let expiredDate = data?.timeout ?? "";
                let timeout = new Date(expiredDate);
                let currDate = new Date(Date.now() - 1000 * 60);
                strToken = data?.token;
                if (data.token == "" || currDate > timeout) {
                    strToken = LogCall_FTP_Author(urlVerify, username, password);
                }
            }
        }
        if (strToken == "") {
            strToken = LogCall_FTP_Author(urlVerify, username, password);
        }
        dateFrom = ConvertStringDMY_YMD(dateFrom);
        dateTo = ConvertStringDMY_YMD(dateTo);
        $.ajax({
            url: urlLoadRecord,
            dataType: "json",
            type: "GET",
            data: {
                pagination: 1,
                pagesize: limit,
                start_time: formatDate(dateFrom) + " 00:00:00",
                end_time: formatDate(dateTo) + " 23:59:59",
                extension: ext,
            },
            contentType: 'application/json; charset=utf-8',
            headers: { "token": strToken },
            async: false,
            error: function (XMLHttpRequest, textStatus, errorThrown)
            {
            },
            success: function (result)
            {
                if (result != "" && result != "null" && result != "0") {
                    let data = JSON.parse(JSON.stringify(result));
                    if (data["records"]) {
                        data_audio = data["records"];
                    }
                }
                else {
                    //notiWarning("Không thể tải audio")
                }
            }
        });
        return data_audio;
    } catch (ex) {
        return data_audio;
    }
}
function GetURLRecord(time)
{
    let result = dtAudio.filter(word => word["start_time"] == time);
    if (result.length > 0) {
        return result[0].filepath;
    }
    return "";
}
// #endregion