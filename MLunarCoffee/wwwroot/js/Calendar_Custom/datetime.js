
function Caculator_Date (datefrom, dateto) {
    let Difference_In_Time = dateto.getTime() - datefrom.getTime();
    let Distant_In_Days = Difference_In_Time / (1000 * 3600 * 24) + 1;
    return Distant_In_Days;
}


function ConvertDateTimeUTC_Time_OnlyHour_Int (date) {
    let dd = new Date(date);
    return dd.getHours() * 60 + dd.getMinutes();
}
function HHMM_Int_To_Hour (_int) {
    return Math.floor(_int / 60)
}
function HHMM_Int_To_Minute (_int) {
    return _int % 60
}
function ConvertDT_To_StringYMD_Int (x) {
    try {
        var d = new Date(x);
        let _month = d.getMonth() + 1;
        let month = (_month < 10) ? ("0" + _month) : _month;
        let date = (d.getDate() < 10) ? ("0" + d.getDate()) : d.getDate();
        let _res = "" + d.getFullYear() + month + date;
        return Number(_res);
    }
    catch (err) {
        return 19000101;
    }
}

function Convert_DateInt_To_StringYMD (str) {
    str = str.toString();
    return (str.substring(0, 4) + "-" + str.substring(4, 6) + "-" + str.substring(6, 8));
}
function Convert_DateTime_To_YMD_Int (date) {
    let dd = new Date(date);
    return dd.getFullYear()
        + ("0" + (dd.getMonth() + 1)).slice(-2)
        + ("0" + (dd.getDate())).slice(-2)
}
function YMD_Str_To_DateTime (str) {
    return new Date(str.substring(0, 4), Number(str.substring(4, 6)) - 1, str.substring(6, 8));

}
function Convert_Hour_Time_To_Int (hour) {
    let result = 0;
    let _hour = hour.toString().split(":");
    result += Number(_hour[0]) * 60;
    result += Number(_hour[1]);
    return result;
}
function Convert_Hour_Time_To_Data_Time (hour) {
    let result = 0;
    let _hour = hour.toString().split(":");
    result += Number(_hour[0]) * 100;
    result += Number(_hour[1]);
    return result;
}
function Convert_Int_To_Hour_Time (num) {
    let time_num = num % 1440;

    let _hour = (parseInt(time_num / 60) >= 10) ? (parseInt(time_num / 60)) : ("0" + parseInt(time_num / 60));
    let _minute = ((parseInt(time_num % 60) >= 10) ? parseInt(time_num % 60) : ("0" + parseInt(time_num % 60)))
    let _unit = '';
    if (Number(_minute) == 0) {

        if (Number(_hour) >= 12) _unit = '<span class="cal-timeline-item_unit" >PM</span>';
        else _unit = '<span class="cal-timeline-item_unit">AM</span>';
    }

    return '<span class="cal-timeline-item_hour">' + _hour + ':' + _minute + '</span> ' + _unit;
}
function Convert_Hour_Time_Int_To_Time (hour) {
    let time_num = hour;
    let _hour = (parseInt(time_num / 60) >= 10) ? (parseInt(time_num / 60)) : ("0" + parseInt(time_num / 60));
    let _minute = ((parseInt(time_num % 60) >= 10) ? parseInt(time_num % 60) : ("0" + parseInt(time_num % 60)))
    return _hour + ":" + _minute + ":00";
}
//#endregion



