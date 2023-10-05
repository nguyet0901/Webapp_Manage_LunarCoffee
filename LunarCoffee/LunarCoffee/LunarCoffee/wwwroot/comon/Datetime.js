Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}
Date.prototype.addHours = function (h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
}
Date.prototype.addMinutes = function (m) {
    this.setTime(this.getTime() + (m  * 60 * 1000));
    return this;
}

// Server to show client m-d-y
function formatDateClient(date) {
    if (date != undefined && date != "") {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [day, month, year].join('-');
    }
    else {
        return '01-01-1900';
    }
}
//  m-d-y to date
function formatDMY_To_Date(date) {
    if (date != undefined && date != "") {
        dates = date.split('-');
        return new Date(dates[2], Number(dates[1]) - 1, dates[0], 0, 0, 0, 0);
    }
    else {
        return new Date();
    }
}
//  m-d-y to date
function formatdDateTime_To_Date(date) {
    if (date != undefined && date != "") {
        var d = new Date(date),
            month = '' + d.getMonth(),
            day = '' + d.getDate(),
            year = d.getFullYear();

        return new Date(year, month, day, 0, 0, 0, 0);
    }
    else {
        return new Date();
    }
}
//  m-d-y hh-mm to date
function formatDMYHHMM_To_Date(date) {
    if (date != undefined && date != "") {
        dates = date.split(' ');
        dates1 = dates[0].split('-');
        dates2 = dates[1].split(':');
        return new Date(dates1[2], dates1[1] - 1, dates1[0], dates2[0], dates2[1], 0, 0);
    }
    else {
        return new Date();
    }
}
// Server to show client dd-mm
function yyyyMMdd_ddMM(date) {
    var x = date.split("-");
    return x[2] + "-" + x[1];
}

function DateFormat(date) {
    if (date !== undefined && date !== '' && date !== null) {
        date = date.split("-").reverse().join("-");
    }
    return date;
}
// Server to show client y-m-d
// date( d-m-y)
function formatDate(date) {
    if (date != undefined && date != "") {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        return [year, month, day].join('-');
    }
    else {
        return '1900-01-01';
    }
}



function formatDateToDMYHM(date) {
    if (date != undefined && date != "") {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
        hour = d.getHours();
        minute = d.getMinutes();
        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        if (hour < 10) hour = '0' + hour;
        if (minute < 10) minute = '0' + minute;
        return [day, month, year].join('-') + ' ' + [hour, minute].join(':');
    }
    else {
        return '01-01-1990';
    }
}

// Formate date for datetotpe
function formatDateInput(date) {
    if (date != undefined && date != "") {
        if (date.includes("-")) {
            var x = date.split("-");
            return x[0] + "-" + x[1] + "-" + x[2];
        }
        else if (date.includes("/")) {
            var x = date.split("/");
            return x[0] + "-" + x[1] + "-" + x[2];
        }
        else return '01-01-1900';
    }
    else {
        return '01-01-1900';
    }
}
// second to hms
function secondsToHms(totalSeconds) {
    let hours = Math.floor(totalSeconds / 3600);
    totalSeconds %= 3600;
    let minutes = Math.floor(totalSeconds / 60);
    let seconds = totalSeconds % 60;
    minutes = String(minutes).padStart(2, "0");
    hours = String(hours).padStart(2, "0");
    seconds = String(seconds).padStart(2, "0");
    return (hours + ":" + minutes + ":" + seconds);
}

// Server to show client dd-mm
function yyyyMMddHHMMM_ddMMyyyy(date) {
    var x = date.split(" ");
    var y = x[0].split("-");
    return y[2] + "-" + y[1] + "-" + y[0];
}

// Server to show client dd-mm
function yyyyMMddHHMMM_HHMM(date) {
    var x = date.split(" ");
    return x[1];
}
//Formate Birthday
function formatBirthday(date) {
    try {
        let now = new Date();
        if (date != undefined && date != "") {
            if (date.includes("-")) {
                var x = date.split("-");
                if ((Number(x[2]) < 1900 || Number(x[2]) > now.getFullYear()) || !Number(x[0]) || !Number(x[1]) || !Number(x[2]))
                    return "";
                return x[0] + "-" + x[1] + "-" + x[2];
            }
            else if (date.includes("/")) {
                var x = date.split("/");
                if ((Number(x[2]) < 1900 || Number(x[2]) > now.getFullYear()) || Number(x[0]) || Number(x[1]) || Number(x[2]))
                    return "";
                return x[0] + "-" + x[1] + "-" + x[2];
            }
            else return '01-01-1900';
        }
        else {
            return '01-01-1900';
        }
    } catch (ex) {
        return "";
    }
}

//Function Distance 2 time
function GetHHMM_FromDateTime(x) {
    let date = new Date(x);
    let hour = date.getHours() < 10 ? '0' + date.getHours() : date.getHours();
    let minute = date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
    return hour + ":" + minute;
}
function DateNowHHMM() {
    let now = new Date();
    return now.getHours() + ":" + now.getMinutes();
}
function ChangeMinute_To_Hour_Minute(__minute, text = "") {
    __minute = Number(__minute);
    if (__minute == 0) return "";
    let _result = "";
    switch (__minute) {
        case 0:
            _result = " second " + text;
            break;
        case 1:
            _result = __minute.toString() + " minute " + text;
            break;
        default:
            {
                if (__minute <= 60) {
                    _result = __minute.toString() + " minutes " + text;
                }

                else if (__minute > 61 && __minute <= 120) {
                    _result = Math.floor(__minute / 60) + " hour " + __minute % 60 + " minute " + text;
                }
                else {
                    _result = Math.floor(__minute / 60) + " hours " + text;
                }
            }
            break;
    }

    return _result;
}
function HHMM_Distance_HHMM(_from, _to) {
    try {
        if (_from == "" || _to == "") return 0;

        let __from = Number(_from.split(':')[0]) * 60 + Number(_from.split(':')[1]);
        let __to = Number(_to.split(':')[0]) * 60 + Number(_to.split(':')[1]);
        let __minute = __to - __from;
        return __minute;

    }
    catch (ex)
    {
        return 0;
    }
}


//(date) 2020-03-16T00:00:00   -> 2020-03-16 (string)
function ConvertDT_To_StringYMD(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return d.getFullYear() + '-' + month + '-' + date;

    }
    catch (err) {
        return "";
    }
}
function ConvertDT_To_DT1(x) {
    try {
        var d = new Date(x);
        return new Date(d.getFullYear(), d.getMonth(), 1);


    }
    catch (err) {
        return new Date();
    }
}

function GETDATE_NOW_DMYHM() {
    try {
        var d = new Date();
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return date + '-' + month + '-' + d.getFullYear();

    }
    catch (err) {
        return "";
    }
}
function INTYMDHMToDate (intymd) {
    try {
        intymd = intymd.toString();
        return new Date(intymd.substring(0, 4), Number(intymd.substring(4, 6))-1, intymd.substring(6, 8)
            , intymd.substring(8, 10)
            , intymd.substring(10, 12)
            ,0 );

    }
    catch (err) {

        return new Date();
    }
}
function INTYMDToDate (intymd) {
    try {
        intymd = intymd.toString();
        return new Date(intymd.substring(0, 4), Number(intymd.substring(4, 6)) - 1, intymd.substring(6, 8)
            , 0
            , 0
            , 0);

    }
    catch (err) {

        return new Date();
    }
}

function GetDateTime_Now_Only_Date() {
    try {
        var datenow = new Date();
        return new Date(datenow.getFullYear(), datenow.getMonth(), datenow.getDate());
    }
    catch (err) {
        return "";
    }
}

function GetDateTime_Now_To_String() {
    try {
        var datenow = new Date();
        let hour = (datenow.getHours() < 10) ? ("0" + datenow.getHours()) : datenow.getHours();
        let minute = (datenow.getMinutes() < 10) ? ("0" + datenow.getMinutes()) : datenow.getMinutes();
        let second = (datenow.getSeconds() < 10) ? ("0" + datenow.getSeconds()) : datenow.getSeconds();
        return hour + ":" + minute + ":" + second;
    }
    catch (err) {
        return "";
    }
}
function GetDateTime_Now_HHMM() {
    try {
        var datenow = new Date();
        let hour = (datenow.getHours() < 10) ? ("0" + datenow.getHours()) : datenow.getHours();
        let minute = (datenow.getMinutes() < 10) ? ("0" + datenow.getMinutes()) : datenow.getMinutes();
        return hour + ":" + minute ;
    }
    catch (err) {
        return "";
    }
}
function GetMonthName_Eng(x) {
    let month_array = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Agu', 'Sep', 'Oct', 'Nov', 'Dec'];
    let d = new Date(x);
    let _month = d.getMonth();
    return month_array[_month];
}
function GetDateTime_String_HHMM(x) {
    try {
        var datenow = new Date(x);
        let hour = (datenow.getHours() < 10) ? ("0" + datenow.getHours()) : datenow.getHours();
        let minute = (datenow.getMinutes() < 10) ? ("0" + datenow.getMinutes()) : datenow.getMinutes();
        return hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}
function GetDateTime_String_DMY(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return date + '-' + month + '-' + d.getFullYear();
    }
    catch (err) {
        return "";
    }
}
function GetDateTime_String_DMYHM(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        return date + '-' + month + '-' + d.getFullYear() + ' ' + hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}
function GetDateTime_String_DMYHMSS (x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let second = (d.getSeconds() < 10) ? ("0" + d.getSeconds()) : d.getSeconds();
        return date + '-' + month + '-' + d.getFullYear() + ' ' + hour + ":" + minute + ":" + second;
    }
    catch (err) {
        return "";
    }
}
function GetDateTime_String_DMOnly (x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;

        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let month = (_month < 10) ? ("0" + _month.toString()) : _month;
        var dayofweek = ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"];
        let dow = dayofweek[d.getDay()] ;
        return dow + ', ' +date + '-' + month;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToHHMMSS(DateTime) {
    try {
        var datenow = new Date(DateTime);
        let hour = (datenow.getHours() < 10) ? ("0" + datenow.getHours()) : datenow.getHours();
        let minute = (datenow.getMinutes() < 10) ? ("0" + datenow.getMinutes()) : datenow.getMinutes();
        let second = (datenow.getSeconds() < 10) ? ("0" + datenow.getSeconds()) : datenow.getSeconds();
        return hour + ":" + minute + ":" + second;
    }
    catch (err) {
        return "";
    }
}
// (Date) 1900-01-01 -> ''
function ConvertToDateRemove1900(x) {

    try {
        var d = new Date(x);
        if (Number(d.getFullYear()) == 1900) return "";
        else return x
    }
    catch (err) {
        return x;
    }
}


// convert datetime to string
function ConvertStringYMD_DMY(x) {
    try {
        let j = x.split('-');
        return (j[2] + "-" + j[1] + "-" + j[0]);
    }
    catch (err) {
        return "";
    }
}
function ConvertStringDMY_YMD(x) {
    try {
        let j = x.split('-');
        return (j[2] + "-" + j[1] + "-" + j[0]);
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToString(x) {
    try {
        let j = x.split('-');
        return ('ngày ' + j[0] + ' tháng ' + j[1] + ' năm ' + j[2]);
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToStringRemove1900(x) {
    try {
        let j = x.split('-');
        if (j[2] == "1900") return "";
        else return (j[0] + " - " + j[1] + " - " + j[2]);
    }
    catch (err) {
        return "";
    }
}
// DateTimeUTC => 19-08-2021
function ConvertDateTimeUTC_DMY(x) {
    try {

        if (x != undefined) {
            if (x == "") return "";
            var d = new Date(x);
            let _month = d.getMonth() + 1;
            let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
            let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
            let month = (_month < 10) ? ("0" + _month) : _month;
            let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
            return date + "-" + month + "-" + d.getFullYear();
        }
        else {
            return "";
        }
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeTo_StringT(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let second = (d.getSeconds() < 10) ? ("0" + d.getSeconds()) : d.getSeconds();
        return d.getFullYear() + "-" + month + "-" + date + 'T' + hour + ":" + minute + ":" + second;
    }
    catch (err) {
        return "";
    }
}

function ConvertDateTimeUTC_DMY_Remove1900(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        if (d.getFullYear() == "1900") return '';
        else return d.getFullYear() + "-" + month + "-" + date;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTC_YMD(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return d.getFullYear() + "-" + month + "-" + date;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTCSS(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        var datenow = new Date();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let second = (d.getSeconds() < 10) ? ("0" + d.getSeconds()) : d.getSeconds();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
            return hour + ":" + minute + ":" + second;
        }
        else {
            return date + "-" + month + "-" + d.getFullYear();
        }
        return d;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTC(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        var datenow = new Date();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
            return hour + ":" + minute;
        }
        else {
            return date + "-" + month + "-" + d.getFullYear();
        }
        return d;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTC_NoYear(x) {


    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        var datenow = new Date();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
            return hour + ":" + minute;
        }
        else {
            return date + "-" + month + "-" + d.getFullYear();
        }
        //return d;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTC_Time(x) {
    try {

        if (x != undefined) {
            if (x == "") return "";
            var d = new Date(x);
            var datenow = new Date();
            let _month = d.getMonth() + 1;
            let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
            let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
            let month = (_month < 10) ? ("0" + _month) : _month;
            let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

            if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
                return hour + ":" + minute;
            }
            else {
                return hour + ":" + minute + "  " + date + "-" + month + "-" + d.getFullYear();
            }
            return d;
        }
        else {
            var d = new Date();
            let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
            let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
            return hour + ":" + minute;
        }
    }
    catch (err) {
        return "";
    }
}
//DateTimeUTC => "15:09  19-08-2021"
function ConvertDateTimeUTC_DMYHM(x) {
    try {

        if (x != undefined) {
            if (x == "") return "";
            var d = new Date(x);
            let _month = d.getMonth() + 1;
            let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
            let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
            let month = (_month < 10) ? ("0" + _month) : _month;
            let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
            return hour + ":" + minute + " " + date + "-" + month + "-" + d.getFullYear();
        }
        else {
            return "";
        }
    }
    catch (err) {
        return "";
    }
}

function ConvertDateTimeUTC_Time_OnlyHour(x) {
    try {

        var d = isNaN(x) ? new Date(x) : new Date(Number(x));
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        return hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}
function ConvertTimeSpanUTC_Time(x) {
    try {

        var d = isNaN(x) ? new Date(x) : new Date(Number(x));
        var datenow = new Date();
        let _month = d.getMonth() + 1;
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
            return hour + ":" + minute;
        }
        else {
            return hour + ":" + minute + "  " + date + "-" + month + "-" + d.getFullYear();
        }
        return d;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToTimeSpan(x) { 
    try {
        if (x != undefined) { 
            return new Date(x).getTime();
        } else { 
            return new Date().getTime();
        } 
    }
    catch (err) {
        return "";
    }
}
// 1656231712 => '2022-06-26 15:21'
function ConvertTimeSpanToDateTime(x) {
    try {
        var d = new Date(Number(x) * 1000);

        let _month = d.getMonth() + 1;
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        return d.getFullYear() + "-" + month + "-" + date + " " + hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}
// 1656231712 => '2022-06-26 15:21:52'
function ConvertTimeSpanToFullDateTime(x) {
    try {
        var d = new Date(Number(x) * 1000);

        let _month = d.getMonth() + 1;
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let seconds = (d.getSeconds() < 10) ? ("0" + d.getSeconds()) : d.getSeconds();  
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        return d.getFullYear() + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + seconds;
    }
    catch (err) {
        return "";
    }
}

// Convert datetime to number
function ConvertDateByNumbers(startDate) {

    let chars = [' ', '<br>'];
    try {
        for (i = 0; i < chars.length; i++) {
            let char = chars[i];
            if (startDate.includes(char)) {
                startDate = startDate.split(char)[0];
            }
            if (startDate != "" || startDate != undefined) {
                var parts = startDate.split('-');
                startDate = parts[1] + '-' + parts[0] + '-' + parts[2];
                var date = new Date(startDate);
                return Number(date.getTime());
            }
            else return 0;
        }

    }
    catch (err) {
        return 0;
    }

}

function StringYMDTODate(x) {
    try {
        if (x.includes('T')) {
            x = x.split('T')[0];
        }
        let j = x.split('-');
        return new Date(Number(j[0]), Number(j[1]) - 1, Number(j[2])); // 0-11 for month
    }
    catch (err) {
        return new Date();
    }
}

function StringYMD_SPACE_HMTODate(x) {
    try {
        if (x.includes(' ')) {
            x1 = x.split(' ')[0];
            x2 = x.split(' ')[1];
            let j1 = x1.split('-');
            let j2 = x2.split(':');
            return new Date(Number(j1[0]), Number(j1[1]) - 1, Number(j1[2]), Number(j2[0]), Number(j2[1])); // 0-11 for month

        }

    }
    catch (err) {
        return new Date();
    }
}
function ConvertDateTimeToStringDMY_HM(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        var datenow = new Date();
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        if (d.getDate() == datenow.getDate() && d.getYear() == datenow.getYear() && d.getMonth() == datenow.getMonth()) {
            return hour + ":" + minute;
        }
        else {
            return hour + ":" + minute + "  " + date + "-" + month + "-" + d.getFullYear();
        }
        return d;
    }
    catch (err) {
        return "";
    }
}
// Server to show client dd-mm-yyyy HH:MM
function formatDate_to_ddmmyyyyHHMM(date) {
    if (date != undefined && date != "") {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear(),
            hh = ' ' + d.getHours(),
            mm = '' + d.getMinutes();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        var day = [day, month, year].join('-');
        var hour = [hh, mm].join(':');
        return day + hour;
    }
    else {
        return '01-01-1900 09:00';
    }
}
// new Date => "2021-08-19 15:13:00"
function ConvertDateTimeToStringYMDHHMMSS(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;

        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        return d.getFullYear() + "-" + month + "-" + date + " " + hour + ":" + minute+":00";
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToString_D_M(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let month = (_month < 10) ? ("0" + _month) : _month;
        return date +"/"+ month;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeToString_DOW(x) {
    try {
        var dayofweek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        var d = new Date(x);
        return dayofweek[Number(d.getDay())];
    }
    catch (err) {
        return "";
    }
}

function ConvertDateTimeToString_D_(x) {
    try {
        var d = new Date(x);
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return date ;
    }
    catch (err) {
        return "";
    }
}
function ConvertDateTimeUTC_Time_DOWFULLDAY(x) {
    try {

        var d = isNaN(x) ? new Date(x) : new Date(Number(x));
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let _month= d.getMonth() + 1
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        return ConvertDateTimeToString_DOW(x)
            + " , " + date + "/" + month + "/"+d.getFullYear()
            + " " + hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}
function Face_Convert_Date_To_DOW_DMY(x) {
    try {

        var d = isNaN(x) ? new Date(x) : new Date(Number(x));
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();
        let _month = d.getMonth() + 1
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();

        return ConvertDateTimeToString_DOW(x)
            + " , " + date + "/" + month + "/" + d.getFullYear()
    }
    catch (err) {
        return "";
    }
}

// 2021-09-20 15:08 => 15:08
function Face_Convert_Date_To_DOW_HM(x) {
    try {
        var d = isNaN(x) ? new Date(x) : new Date(Number(x));
        let hour = (d.getHours() < 10) ? ("0" + d.getHours()) : d.getHours();
        let minute = (d.getMinutes() < 10) ? ("0" + d.getMinutes()) : d.getMinutes();

        return hour + ":" + minute;
    }
    catch (err) {
        return "";
    }
}

// 2021-09-20 15:08 => 20/09/2021
function ConvertDateTimeToString_D_M_Y(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let month = (_month < 10) ? ("0" + _month) : _month;
        let year = d.getFullYear();
        return date + "/" + month + "/" + year;
    }
    catch (err) {
        return "";
    }
}

function ConvertDateTime_To_Timespan_TimeZone(_dd) {
    try {
        return (_dd.getTime() + (_dd.getTimezoneOffset() * 60 * 1000 * -1));
    } catch (ex){
        return 0;
    }
}

function ConverDateTime_Only_Date_From_DateTime(x) {
    try {
        return new Date(x.getFullYear(), x.getMonth(), x.getDate());
    }
    catch (err) {
        return "";
    }
}
// => "2021-08-19"
function ConvertDT_To_StringYMD(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return d.getFullYear() + '-' + month + '-' + date;

    }
    catch (err) {
        return "";
    }
}

function GetTimeAgo_FromCurrent(x) {
    try {
        let resulf = '';
        let date = new Date(x);
        let hour = (date.getHours() < 10) ? ("0" + date.getHours()) : date.getHours();
        let minute = (date.getMinutes() < 10) ? ("0" + date.getMinutes()) : date.getMinutes();


        let dateNow = new Date();
        if (date.toDateString() == dateNow.toDateString()) {
            resulf = hour + ":" + minute;
        }
        else {
            let diffTime = Math.abs(date - dateNow);
            let diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            if (diffDays == 1) {
                resulf = diffDays + ' day ago ' + hour + ":" + minute;
            }
            else if (diffDays < 6) {
                resulf = (diffDays-1) + ' days ago ' + hour + ":" + minute;
            }
            else {
                resulf = ConvertDateTimeToString_D_M_Y(x)+'  ' + hour + ":" + minute;
            }
        }

        return resulf;
    }
    catch (err) {
        return "";
    }
}


function ConvertString_DMY_To_StringYMD(x) {
    try {
        let _d = x.split(' ')[0];
        return _d.split('-')[2] + '-' + _d.split('-')[1] + '-' + _d.split('-')[0];

    }
    catch (err) {
        return "";
    }
}

function ConvertString_YMD_To_DateTime(x) {
    try {
        let _d = x.split(' ')[0];
        return new Date(_d.split('-')[0], Number(_d.split('-')[1])-1, _d.split('-')[2]);
    }
    catch (err) {
        return "";
    }
}
function ConvertString_DMY_To_DateTime(x) {
    try {
        let _d = x.split(' ')[0];
        return new Date(_d.split('-')[2], Number(_d.split('-')[1]) - 1, _d.split('-')[0]);
    }
    catch (err) {
        return "";
    }
}
function ConvertOnly_DMY_To_DateTime(x) {
    try {
        return new Date(x.split('-')[2], Number(x.split('-')[1]) - 1, x.split('-')[0]);
    }
    catch (err) {
        return "";
    }
}
function ConvertString_DMYHM_To_DateTime(x) {
    try {
        let _d = x.split(' ')[0];
        let _h = x.split(' ')[1];
        return new Date(_d.split('-')[2], Number(_d.split('-')[1]) - 1, _d.split('-')[0], _h.split(':')[0], _h.split(':')[1]);
    }
    catch (err) {
        return "";
    }
}

function ConvertDT_To_ThousandNumber(x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let _year = d.getFullYear();
        return _year * 12 + _month;
    }
    catch (err) {
        return 0;
    }
}
function ConvertThousandNumber_To_MY(t) {
    try {
        let _m = "";
        let _y = "";

        if (t % 12 == 0) {
            _m = "12";
            _y = t / 12 - 1;
        }
        else {
            _m = t % 12;
            _y = (t - _m) / 12;


        }
        return ((_m < 10) ? ('0'+_m.toString()) : _m.toString()) + '/' + _y.toString().substring(2, 4);
    }
    catch (ex){
        return "";
    }
}
function Distance_Year_2Date(d1, d2) {
    try {
        return d2.getFullYear() - d1.getFullYear();
    }
    catch (ex) {
        return 0;
    }
}
function BigIntToTime (time) {
    try {
        time = time.toString();
        if (time == "" || time== "0") return "";
        let result = '';
        let year = time.substring(0, 4);
        let month = time.substring(4, 6);
        let day = time.substring(6, 8);
        let hour = time.substring(8, 10);
        let minute = time.substring(10, 12);
        let dateNow = new Date();
        if (new Date(dateNow.getFullYear(), dateNow.getMonth() + 1, dateNow.getDate()).getTime() == (new Date(year, month, day)).getTime()) {
            result = hour + ":" + minute
        }
        else {
            result = day + '-' + month + '-' + year;
            result = hour + ":" + minute + " " + result;
        }
        return result;
    }
    catch (ex) {
        return "";
    }
}
function BigIntToAgo (time) {
    try {

        let result = '';
        time = time.toString();
        if (time == "" || time== "0") return "";
        let year = time.substring(0, 4);
        let month = Number(time.substring(4, 6)) - 1;
        let day = time.substring(6, 8);
        let hour = time.substring(8, 10);
        let minute = time.substring(10, 12);
        let dateNow = new Date();
        let currentdate = new Date(year, month, day, hour, minute)

        let difYears = Math.floor(Math.abs(dateNow - currentdate) / (1000 * 60 * 60 * 24 * 30 * 365));
        let diffMonths = Math.floor(Math.abs(dateNow - currentdate) / (1000 * 60 * 60 * 24 * 30));
        let diffDays = Math.floor(Math.abs(dateNow - currentdate) / (1000 * 60 * 60 * 24));
        let diffHours = Math.floor(Math.abs(dateNow - currentdate) / (1000 * 60 * 60));
        let diffMinutes = Math.floor(Math.abs(dateNow - currentdate) / (1000 * 60));
        if (difYears != 0) resulf = difYears + ' năm';
        else if (diffMonths != 0) resulf = diffMonths + ' tháng';
        else if (diffDays != 0) resulf = diffDays + ' ngày';
        else if (diffHours != 0) resulf = diffHours + ' giờ';
        else if (diffMinutes != 0) resulf = diffMinutes + ' phút';
        else resulf = 'vài giây';

        return resulf;
    }
    catch (err) {
        return "";
    }
}
function BigIntToDate (time) {
    try {
        time = time.toString();
        if (time == "" || time== "0") return "";
        let result = '';
        let year = time.substring(0, 4);
        let month = time.substring(4, 6);
        let day = time.substring(6, 8);
        let dateTime = new Date(year, month - 1, day, 0, 0, 0, 0);
        var dayofweek = ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"];
        result = dayofweek[dateTime.getDay()] + ', ';
        result += day + '-' + month + '-' + year;
        return result;
    }
    catch (ex) {
        return "";
    }
}
function BigIntToHHMM (time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let result = '';
        let hour = time.substring(0, 2);
        let minute = time.substring(2, 4);

        result = hour + ':' + minute;
        return result;
    }
    catch (ex) {
        return "";
    }
}
function BigIntYMDHMToHHMM (time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let result = '';
        let hour = time.substring(8, 10);
        let minute = time.substring(10, 12);

        result = hour + ':' + minute;
        return result;
    }
    catch (ex) {
        return "";
    }
}
function ConvertYMDInt_ToDate(time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let result = '';
        let year = time.substring(0, 4);
        let month = time.substring(4, 6);
        let day = time.substring(6, 8);
        result = day + '-' + month + '-' + year;
        return result;
    }
    catch (ex) {
        return "";
    }
}

function ConvertBetweenYMDInt_ToDate(datefrom, dateto) {
    try {
        datefrom = datefrom.toString();
        dateto = dateto.toString();
        if (datefrom == "" || datefrom == "0" || dateto == "" || dateto == "0") return "";
        let result = '';
        let yearf = datefrom.substring(0, 4);
        let monthf = datefrom.substring(4, 6);
        let dayf = datefrom.substring(6, 8);
        let yeart = dateto.substring(0, 4);
        let montht = dateto.substring(4, 6);
        let dayt = dateto.substring(6, 8);
        if (yearf == yeart)
            result = dayf + '/' + monthf + ' - ' + dayt + '/' + monthf + '/' + yearf;
        else result = dayf + '/' + monthf + '/' + yearf + ' - ' + dayt + '/' + montht + '/' + yeart;
        return result;
    }
    catch (ex) {
        return "";
    }
}


function ConvertYMDInt_ToDOW(time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let year = Number(time.substring(0, 4));
        let month = Number(time.substring(4, 6));
        let day = Number(time.substring(6, 8));
        let newDate = new Date(year, month - 1, day, 0, 0, 0, 0);
        return newDate.getDay();
    }
    catch (ex) {
        return 0;
    }
}
function ConvertYMDInt_ToDateTime(time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let year = Number(time.substring(0, 4));
        let month = Number(time.substring(4, 6));
        let day = Number(time.substring(6, 8));
        return new Date(year, month - 1, day, 0, 0, 0, 0);
    }
    catch (ex) {
        return new Date();
    }
}
function ConvertYMDInt_ToDM(time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let month = time.substring(4, 6);
        let day = time.substring(6, 8);
        return day+'/'+month;
    }
    catch (ex) {
        return new Date();
    }
}
function ConvertDateTime_To_YMDInt(datetime) {
    try {
        let d = new Date(datetime);
        let nummonth = d.getMonth()+1;
        let month = (nummonth < 10) ? ("0" + nummonth) : nummonth;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        return Number(d.getFullYear() + '' + month + '' + date);
    }
    catch (ex) {
        return 0;
    }
}

function ConvertDMY_ToInt(date) {
    try {
        date = date.split('-');
        return Number(date[2] + date[1] + date[0]);
    }
    catch (ex) {
        return 0;
    }
}

function ConvertTime_ToString(time) {
    try {
        time = time.split(':');
        return time[0] +''+ time[1];
    }
    catch (ex) {
        return 0;
    }
}

function ConvertTimeInt_ToTime(time) {
    try {
        time = time.toString();
        if (time == "" || time == "0") return "";
        let result = '';
        let hours = time.substring(0, 2);
        let minutes = time.substring(2, 4);
        result = hours + ':' + minutes;
        return result;
    }
    catch (ex) {
        return 0;
    }
}

function BigIntToLive (time) {
    try {
        time = time.toString();
        let result = [], status = '', timeAgo = '';
        let year = time.substring(0, 4);
        let month = time.substring(4, 6);
        let day = time.substring(6, 8);
        let hour = time.substring(8, 10);
        let minute = time.substring(10, 12);
        let date = new Date(year, month - 1, day, hour, minute, 0, 0);

        var seconds = Math.floor((new Date() - date) / 1000);


        let interval = 0;
        interval = seconds / 2592000;
        if (interval > 1) {
            result.push('bg-secondary');
            result.push(Math.floor(interval) + " months ago");
            return result;
        }
        else if (seconds / 86400 > 1) {
            result.push('bg-secondary');
            result.push(Math.floor(seconds / 86400) + " days ago");
            return result;
        }
        if (seconds / 3600 > 1) {
            result.push('bg-warning')
            result.push(Math.floor(seconds / 3600) + " hours ago");
            return result;
        }
        else if (seconds / 60 > 5) {
            result.push('bg-warning')
            result.push(Math.floor(seconds / 60) + " minutes ago");
            return result;
        }
        else if (seconds / 60 < 5) {
            result.push('bg-success');
            result.push('active now');
            return result;
        }
    }
    catch (ex) {
        return ['', ''];
    }
}
function BigIntYmdhms () {
    var date = new Date();
    return date.getFullYear() + ("0" + (date.getMonth() + 1)).slice(-2) + ("0" + date.getDate()).slice(-2) + ("0" + date.getHours()).slice(-2) + ("0" + date.getMinutes()).slice(-2) + ("0" + date.getSeconds()).slice(-2)

}
//date_validate
(function ($) {
    $.fn.date_validate = function (options) {
        var base = this;
        var is_error = true;
        var defaults = {
            format: /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/,
            class: 'error-birthday',
            elm_error: '.field'
        };
        var settings = $.extend({}, defaults, options);
        base.init = function () {
            is_error = validate_date(base.val());
        };
        base.init();
        function validate_date(value) {
            if (value && value != '') {
                if (value.match(settings.format)) {
                    var opera1 = value.split('/');
                    var opera2 = value.split('-');
                    lopera1 = opera1.length;
                    lopera2 = opera2.length;
                    if (lopera1 > 1) {
                        var pdate = value.split('/');
                    }
                    else if (lopera2 > 1) {
                        var pdate = value.split('-');
                    }
                    var dd = parseInt(pdate[0]);
                    var mm = parseInt(pdate[1]);
                    var yy = parseInt(pdate[2]);
                    var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    if (mm == 1 || mm > 2) {
                        if (dd > ListofDays[mm - 1]) {
                            return false;
                        }
                    }
                    if (mm == 2) {
                        var lyear = false;
                        if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                            lyear = true;
                        }
                        if ((lyear == false) && (dd >= 29)) {
                            return false;
                        }
                        if ((lyear == true) && (dd > 29)) {
                            return false;
                        }
                    }
                } else {
                    return false;
                }
                return true;
            } else {
                return true;
            }
        }
        base.keypress(function (_e) {
            var v = base.val();
            if (v.match(/^\d{2}$/) !== null) {
                base.val(v + '-');
            } else if (v.match(/^\d{2}\-\d{2}$/) !== null) {
                base.val(v + '-');
            }

        })
        base.change(function (_e) {
            if (validate_date(base.val()) == false) {
                base.closest(settings.elm_error).addClass(settings.class);
                is_error = false;
            } else {
                base.closest(settings.elm_error).removeClass(settings.class);
                is_error = true;
            }
        })
        var date_validate = {
            get_validate: function (_d) {
                return is_error;
            }

        };
        return date_validate;
    };

}(jQuery));