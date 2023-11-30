
//#region //Load logs from db VTT

function AllLog_Voicecloud(dateFrom, dateTo, extension) {

    var data = [];
    var allbound = LogCall_Voicecloud(dateFrom, dateTo, extension);

    for (let i = 0; i < allbound.length; i++) {
        let element = {};
        let item = allbound[i];
        element.from = item?.caller.toString();
        element.to = item?.callee;
        element.direction = item?.direction;
        element.duration = item?.duration;
        element.state = item?.call_status;
        element.recordUrl = item?.audio_file;
        element.start = ConvertTimeSpanToFullDateTime(item?.start);
        element.status = (item?.call_status == 'ANSWERED' || item?.call_status == 'ANSWER' ? 'SUCCESS' : '');
        data.push(element);
    }
    return data;
}

function LogCall_Voicecloud(dateFrom, dateTo, extension) {
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
        , success = function (result) {
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
