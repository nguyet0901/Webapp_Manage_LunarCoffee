var Second_alram_count = 20;
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie (cname) {
    try {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";

    }
    catch (ex) {

    }

}

function checkCookie(name) {
    var Cookie = getCookie(name);
    if (Cookie != "" && Cookie != null) {
        return true;
    } else {
        return false;
    }
}

function SettingAlarm(empid) {

    if (checkCookie('SettingAlarm')) {
        let Cookie = JSON.parse(getCookie('SettingAlarm'));
        let AlarmCookie = Cookie.data;
        let empID = Cookie.empID;
        if (empID != empid) {
            DeleteAlarmCookie();
            GetAlarmSchedule(1, Second_alram_count, empid);
        } else {
            EstablishAlarmSchedule(JSON.parse(getCookie('SettingAlarm')).data, Second_alram_count);
        }
    } else {
        DeleteAlarmCookie();
        GetAlarmSchedule(1, Second_alram_count, empid);
    }
    setInterval(function () {
        EstablishAlarmSchedule(JSON.parse(getCookie('SettingAlarm')).data, Second_alram_count);
    }, Second_alram_count * 1000);
}

function SetAlarmSchedule(data, exdays, second_alram, empid) {
    let d = { 'empID': empid, 'data': data }
    setCookie('SettingAlarm', JSON.stringify(d), exdays);
    EstablishAlarmSchedule(data, second_alram);
}

function GetAlarmSchedule(exdays, second_alram, empid) {
    AjaxLoad(url = "/Todo/AlarmSchedule/MyAlarmScheduler_List/?handler=LoadAlarmScheduleToday"
        , data = {}, async = false, error = null
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result);
                if (data.length > 0)
                    SetAlarmSchedule(data, exdays, second_alram, empid);
            }
        });
}

function EstablishAlarmSchedule(data, second_alram) {
    let datenow = new Date();
    let Hours = datenow.getHours();
    let Minutes = datenow.getMinutes();
    for (i = 0; i < data.length; i++) {
        let item = data[i];
        let time = item.TimeAlarm.split(':');
        let hr = Number(time[0]) - Number(Hours);
        let mt = Number(time[1]) - Number(Minutes);
        let numSecond = (hr * 3600) + (mt * 60);
        if (numSecond < 0) {
            DeleteItemAlarmCookie(item.ID, 1);
        } else {
            if (numSecond < second_alram) {
                notiMess10(  item.Content);
                DeleteItemAlarmCookie(item.ID, 1);
            }
        }
    }
}

function UpdateAlarmCookie(data, exdays) {
    if (checkCookie('SettingAlarm')) {
        let isUpdate = 0;
        let dataTime = JSON.parse(data.data)[0];
        let now = new Date();
        switch (dataTime.type.toString()) {
            case '1':
                let dateUpdate = new Date(dataTime.date);
                let datenow = new Date(now.getFullYear() + '-' + Number(now.getMonth() + 1) + '-' + now.getDate());
                if (dateUpdate.toDateString() == datenow.toDateString()) {
                    isUpdate = 1;
                }
                break;
            case '2':
                isUpdate = 1;
                break;
            case '3':
                var nowWeek = now.getDay();
                let dateweek = dataTime.dayofweek;
                if (nowWeek == dateweek) {
                    isUpdate = 1;
                }
                break;
            case '4':
                if (now.getDate() == dataTime.day) {
                    isUpdate = 1;
                }
                break;
            default:
                break;
        }
        if (isUpdate == 1 && data.Status != 0) {
            let Cookie = JSON.parse(getCookie('SettingAlarm'));
            let AlarmCookie = Cookie.data;
            let empID = Cookie.empID;
            let isNew = 1;
            for (i = 0; i < AlarmCookie.length; i++) {
                if (AlarmCookie[i].ID == data.ID) {
                    AlarmCookie[i].Title = data.Title;
                    AlarmCookie[i].Content = data.Content;
                    AlarmCookie[i].TimeAlarm = dataTime.hour;
                    isNew = 0;
                }
            }
            if (isNew != 0) {
                let dataUpdate = [{ ID: data.ID.toString(), Title: data.Title, Content: data.Content, TimeAlarm: dataTime.hour }];
                AlarmCookie.push(dataUpdate[0]);
            }
            let d = { 'empID': empID, 'data': AlarmCookie }
            setCookie('SettingAlarm', JSON.stringify(d), exdays);
        } else {
            DeleteItemAlarmCookie(data.ID, 1)
        }
    } else {
        RefeshAlarmCookie(exdays)
    }
}
function RefeshAlarmCookie(exdays) {
    setCookie("SettingAlarm", '', -1);
    GetAlarmSchedule(exdays);
}
function DeleteItemAlarmCookie(ID, exdays) {
    let Cookie = JSON.parse(getCookie('SettingAlarm'));
    let AlarmCookie = Cookie.data;
    let empID = Cookie.empID;
    let index = -1;
    if (AlarmCookie.length > 0 && AlarmCookie != null) {
        for (j = 0; j < AlarmCookie.length; j++) {
            if (AlarmCookie[j].ID == ID) {
                index = j;
                break;
            }
        }
        if (index != -1) {
            AlarmCookie.splice(index, 1);
            let d = { 'empID': empID, 'data': AlarmCookie }
            setCookie('SettingAlarm', JSON.stringify(d), exdays);
        }
    }
}
function DeleteAlarmCookie() {
    setCookie("SettingAlarm", '', -1);
}