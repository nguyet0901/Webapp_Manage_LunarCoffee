async function AllLog_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, page, phone_ext) {
    var data = []; 
    if (page == 1) {
        var allbound = [];
        let promises = [];
        if (phone_ext == "phone") {
            promises.push(LogCall_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, 'search', 'outbound'));
            promises.push(LogCall_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, 'search', 'inbound'));
            allbound = await Promise.all(promises);
            allbound = $.merge(allbound[0], allbound[1]);
        } else {
            if (extension != "") {
                promises.push(LogCall_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, 'source', 'outbound'));
                promises.push(LogCall_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, 'destination', 'inbound'));
                allbound = await Promise.all(promises);
                allbound = $.merge(allbound[0], allbound[1]);
            }
        }
        for (var i = 0; i < allbound.length; i++) {
            let element = {};
            let item = allbound[i];
            element.from = item.src;
            element.to = item.dst;
            element.direction = item.type;
            element.duration = item.billsec;
            element.state = item.status;
            element.recordUrl = item.recording != "" ? item.recording : "null";
            element.start = item.calldate;
            element.status = (item.status == 'ANSWERED' ? 'SUCCESS' : '')
            data.push(element);
        }
    }
    return data;
}

async function LogCall_Voip24h(APIbaseurl, UserName, Password, dateFrom, dateTo, extension, type0, type1) {
    return new Promise(resolve => {
        let dtResult;

        let _dF = ConvertString_DMY_To_DateTime(dateFrom);
        let _dT = ConvertString_DMY_To_DateTime(dateTo);

        let DF_TimeSpan = _dF.getTime() / 1000;
        let DT_TimeSpan = _dT.addDays(1).addMinutes(-1).getTime() / 1000;

        let url = APIbaseurl + "/dial/search?voip=" + UserName + "&secret=" + Password + "&date_start=" + DF_TimeSpan + "&date_end=" + DT_TimeSpan + "&length=10000&" + type0 + "=" + extension + "&type=" + type1;

        $.ajax({
            url: url,
            ContentType: "application /json",
            type: "GET",
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                notiError_SW();
            },
            success: function (result) {
                let data = result.result.data;
                if (data != null && data.length > 0) {
                    dtResult = data;
                } else {
                    dtResult = [];
                }
                resolve(dtResult);
            }
        });
    })
}