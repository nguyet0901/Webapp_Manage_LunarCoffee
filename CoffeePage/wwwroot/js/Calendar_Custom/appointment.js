function update_cal_MLU_app (_id, _branch, date_from, type, istemp, isupdate) {
    if (_branch == data_Branch_ID && date_from >= ConvertDT_To_StringYMD_Int(data_Calendar_DateFrom)
        && date_from <= ConvertDT_To_StringYMD_Int(data_Calendar_DateTo)
    ) {
        AjaxLoad(url = "/Appointment/Appointment/?handler=LoadScheduler"
            , data = {
                'datefrom': ConvertDateTimeToStringYMDHHMMSS(new Date())
                , 'dateto': ConvertDateTimeToStringYMDHHMMSS(new Date())
                , 'branch': _branch
                , 'DoctorID': 0
                , 'AppID': _id
                , 'type': type
                , 'IsTemp': istemp
            }, async = true
            , error = function () {
                notiError_SW();
            }
            , success = function (result) {
                if (result != "") {
                    let _data = JSON.parse(result);
                    let data_result = callMLU_appget_each(_data);
                    if (isupdate == 1) cal_MLU_delete_app(_data[0].ID, _branch);
                    cal_MLU_insert_app(data_result);
                }
            }
        );
    }
}
function cal_MLU_Manage_Executing (json) {

    let data = JSON.parse(json);
    if (data != undefined && data.length != 0) {
        let userexcept = data.userexcept;
        if (userexcept != sys_userID_Main) {
            let type = data.type;
            let istemp = data.istemp;
            let branch_id = data.branch_id;
            let date_from = ConvertDT_To_StringYMD_Int(data.date_from);
            let appointment_id = Number(data.appointment_id);
            if (istemp == 1) appointment_id = (appointment_id < 0) ? (appointment_id) : (0 - appointment_id);
            switch (type) {
                case "cancel":
                    {
                        if (branch_id == data_Branch_ID) {
                            update_cal_MLU_app(appointment_id, branch_id, date_from, 0, istemp, 1);
                        }
                        break;
                    }
                case "delete":
                    {
                        if (branch_id == data_Branch_ID) cal_MLU_delete_app(appointment_id, branch_id);
                        break;
                    }
                case "insert":
                    {
                        update_cal_MLU_app(appointment_id, branch_id, date_from, 0, istemp, 0);
                        break;
                    }
                case "update":
                    {
                        update_cal_MLU_app(appointment_id, branch_id, date_from, 0, istemp, 1);
                        break;
                    }

                case "change_status":
                    {
                        if (branch_id == data_Branch_ID) {
                            cal_MLU_delete_app(appointment_id, branch_id);
                            update_cal_MLU_app(appointment_id, branch_id, date_from, 0, 0, 0);
                        }
                        break;
                    }
                default: break;
            }
        }
    }

}
function callMLU_appget_each (_data) {
    let _value = {};
    let _doctorid = 0, _e;
    let _m;
    let _c;
    if (_data.length > 0) {
        for (let iii = 0; iii < _data.length; iii++) {
            _e = {}
            _doctorid = _data[iii].DocID;

            let _datefrom_int = ConvertDT_To_StringYMD_Int(_data[iii].DateFrom);
            _e.ID = _data[iii].ID + ((_data[iii].DoctorMain == 0) ? '_temp' : '');
            _e.BranchName = callMLU_branch[_data[iii].BranchID];
            _e.Color = 'white';
            _e.SchedulerType = _data[iii].SchedulerType;
            _e.SchedulerTypeDetail = _data[iii].SchedulerTypeDetail;
            _e.TagID = _data[iii].TagID;
            _e.ServiceID = 0;
            _e.DocID = _data[iii].DocID;
            _e.DocIDTemp = _data[iii].DocIDTemp;
            _e.DoctorMain = _data[iii].DoctorMain;
            _e.CustCode = _data[iii].CustCode;
            _e.CustCodeOld = _data[iii].CustCodeOld;
            _e.DocumentCode = _data[iii].DocumentCode;
            _e.CustName = _data[iii].CustName;
            _e.CustPhone = _data[iii].CustPhone;
            _e.DateFrom = _data[iii].DateFrom;
            _e.DateTo = _data[iii].DateTo;
            _e.IsPriority = _data[iii].IsPriority;
            _e.Service = ((Number(_data[iii].TypeService) == 1)
                ? (Fun_GetString_ByToken(DataServiceCare, _data[iii].ServiceCare))
                : (Fun_GetString_ByToken(DataService, _data[iii].ServiceTreat)));
            _e.Status = _data[iii].Status;
            _e.Cancel = _data[iii].IsCancel;
            _e.Done = _data[iii].IsDone;
            _e.Type = _data[iii].Type;
            _e.ServiceCare = _data[iii].ServiceCare;
            _e.Content = _data[iii].Content;
            _e.DateFrom_Int = _datefrom_int;
            _e.StatusName = _data[iii].StatusName;
            _e.StatusColor = _data[iii].StatusColor;
            _e.EmpCreated = _data[iii].EmpCreated;
            if (_value[_doctorid] == undefined) {
                _m = {};
                _c = {};
                _c[_e.ID] = _e;
                _m[_datefrom_int] = _c;
                _value[_doctorid] = _m;
            }
            else {
                _m = _value[_doctorid];
                if (_m[_datefrom_int] == undefined) {
                    _c = {};
                    _c[_e.ID] = _e;
                    _m[_datefrom_int] = _c;
                    _value[_doctorid] = _m;
                }
                else {
                    _c = _m[_datefrom_int];
                    _c[_e.ID] = _e;
                    _m[_datefrom_int] = _c;
                    _value[_doctorid] = _m;
                }
            }
        }
    }

    return _value;
}

//#region // Setting
function callMLU_settings () {
    let obj;
    if (localstorage_check('MLU-app')) obj = JSON.parse(JSON.parse(localstorage_get('MLU-app')).data);
    else obj=callMLU_settingsdefault();
    callMLU_settingset(obj);
}
function callMLU_settingsdefault () {
    let e = {};
    e.timerange = 15;
    e.height = 25;
    e.show_app_cancel = true;
    e.show_app_temp = true;
    e.show_app_consul = true;
    e.showdoctornowork = true;
    e.show_app_treat = true;
    e.doctorarr = 'asc';
    e.typeday = 7;
    localstorage_set('MLU-app', JSON.stringify(e));
    return e;
}
function callMLU_settingsreset () {
    let obj;
    obj = callMLU_settingsdefault();
    callMLU_settingset(obj);
    callMLU_executed();
}
function callMLU_settingsaccept () {
    try {
        let e = {};
        e.timerange = Number($('#settingMLULists input[name=minutecell]:checked').val());
        e.height = Number($('#settingMLULists input[name=heightcell]:checked').val());
        e.show_app_cancel = $("#settingMLULists input[name=showcancel]").is(":checked");
        e.show_app_temp = $("#settingMLULists input[name=showtemp]").is(":checked");
        e.show_app_consul = $("#settingMLULists input[name=showconsul]").is(":checked");
        e.show_app_treat = $("#settingMLULists input[name=showtreat]").is(":checked");
        e.showdoctornowork = $("#settingMLULists input[name=showdoctornowork]").is(":checked");
        e.doctorarr = $("#settingMLULists input[name=sortdoctor]:checked").val();
        e.typeday = $("#settingMLULists input[name=typeday]:checked").val();
        localstorage_set('MLU-app', JSON.stringify(e));
        block_height = Number(e.height);
        block_time_range = Number(e.timerange);
        callMLU_isshowcancel = Number(e.show_app_cancel);
        callMLU_isshowtemp = Number(e.show_app_temp);
        callMLU_isshowconsul = Number(e.show_app_consul);
        callMLU_showdoctornowork = e.showdoctornowork;
        callMLU_isshowtreat = Number(e.show_app_treat);
        callMLU_sortdoctor = e.doctorarr;
        callMLU_typeday = e.typeday;
        callMLU_viewbydate();
        callMLU_executed();
    }
    catch (e){
    }
}
function callMLU_settingset (obj) {
    $("#settingMLULists input[name=heightcell][value=" + obj.height + "]").prop('checked', true);
    $("#settingMLULists input[name=minutecell][value=" + obj.timerange + "]").prop('checked', true);
    $('#settingMLULists input[name=showcancel]').prop('checked', obj.show_app_cancel);
    $('#settingMLULists input[name=showtemp]').prop('checked', obj.show_app_temp);
    $('#settingMLULists input[name=showconsul]').prop('checked', obj.show_app_consul);
    $('#settingMLULists input[name=showdoctornowork]').prop('checked', obj.showdoctornowork);
    $('#settingMLULists input[name=showtreat]').prop('checked', obj.show_app_treat);
    $("#settingMLULists input[name=sortdoctor][value=" + obj.doctorarr + "]").prop('checked', true);
    $("#settingMLULists input[name=typeday][value=" + obj.typeday + "]").prop('checked', true);
    block_height = Number(obj.height);
    block_time_range = Number(obj.timerange);
    callMLU_isshowcancel = Number(obj.show_app_cancel);
    callMLU_isshowtemp = Number(obj.show_app_temp);
    callMLU_isshowconsul = Number(obj.show_app_consul);
    callMLU_showdoctornowork = obj.showdoctornowork;
    callMLU_isshowtreat = Number(obj.show_app_treat);
    callMLU_sortdoctor = obj.doctorarr;
    callMLU_typeday = obj.typeday;

    if (callMLU_typeday != 1) {
        $("#settingMLULists #rowShowDoctorWork").fadeOut();
        callMLU_showdoctornowork = true;
    }
}
//#endregion

//#region // Work Scheduler

function callMLU_getdatawork(data_begin, data_end, branch, doctor) {
    return new Promise((resolve) => {
        AjaxLoad(url = "/Appointment/Appointment/?handler=LoadWorkTimeDoctor"
            , data = {
                'datefrom': ConvertDateTimeToStringYMDHHMMSS(data_begin)
                , 'dateto': ConvertDateTimeToStringYMDHHMMSS(data_end)
                , 'tokenDocID': doctor
            }
            , async = true
            , error = function () {
                resolve({
                    work: [],
                    workSchedule: []
                })
            }
            , success = function (result) {
                if (result != "0") {

                    let _data = JSON.parse(result);
                    let _work = Work_Scheduler_Get_From_TokenDoc(branch, data_begin, data_end, _data.Table, _data.Table1, _data.Table2, _data.Table3);

                    resolve({
                        work: _work,
                        workSchedule: _data.Table
                    })
                }
                else {
                    resolve({
                        work: [],
                        workSchedule: []
                    })
                }
            }
        );
    })
}

async function callMLU_setworkview (_work, _dataWS) {

    var proallcell = callMLU_worksetall(_work, _dataWS);
    Promise.allSettled([proallcell]).then(() => {
        if (_work != undefined && _work.length != 0) {
            for (let ii = 0; ii < _work.length; ii++) {
                var ele = _work[ii];
                callMLU_workseteach(ele);
            }
        }
    });
}


async function callMLU_worksetall (_work, _dataWS) {

    return new Promise(resolve => {
        let _x = document.getElementsByClassName("cal-time");
        if ((_work != undefined && _work.length != 0) || (_dataWS != undefined && _dataWS.length != 0)) {
            for (let i = 0; i < _x.length; i++) {
                _x[i].style.backgroundColor = callMLU_cellnowork;
                _x[i].dataset.aval = 0;
            }
        }
        resolve(true);
    });
}

function callMLU_workseteach (e) {
    return new Promise(resolve => {
        let _df = e.Date_From;
        let _dt = e.Date_To;
        if (_df < _dt) {
            _dfh = ConvertDateTimeUTC_Time_OnlyHour_Int(_df);
            _dth = ConvertDateTimeUTC_Time_OnlyHour_Int(_dt);
            while (_dfh <= _dth) {
                __index = Math.floor((_dfh - begin_range_int) / block_time_range);
                if (__index >= 0) {
                    __id = 'cal_cell_' + e.EmpID + '_' + e.Date + '_' + __index;
                    $("#" + __id).attr("data-aval", 1);
                    $("#" + __id).css("background-color", callMLU_cellwork);
                }
                _dfh = _dfh + block_time_range
            }
        }
        resolve(true);

    });

}

//#endregion

//#region // Temporary Delete
//#region //Doctor Arrange
//function PopupArrange_Doctor () {
//    $("#docLists").removeClass("show");
//}
//function Render_List_Doctor_Arrange (data, id) {
//    let data_resouce = {};
//    let _source_sort = [];
//    for (i = 0; i < data.length; i++) {
//        data_resouce[data[i].ID] = data[i].NickName;
//    }
//    let indexdoctor = Get_Value_Doctor_Arrange_Cookie();
//    let data_doctor_json = indexdoctor;

//    if (indexdoctor != undefined && indexdoctor != null && indexdoctor != "") {
//        let __x = indexdoctor.split(',');
//        for (jj = 0; jj < __x.length; jj++) {
//            if (!isNaN(__x[jj]) && __x[jj].trim() != '') {
//                __key = Number(__x[jj]);
//                if (data_resouce[__key] != undefined) {
//                    let _e = {};
//                    _e.ID = __key;
//                    _e.Name = data_resouce[__key];
//                    _source_sort.push(_e);
//                }
//            }
//        }
//        indexdoctor = ',' + indexdoctor + ',';
//        for ([key, value] of Object.entries(data_resouce)) {
//            __key = ',' + key.toString() + ',';
//            if (!indexdoctor.includes(__key)) {
//                let _e = {};
//                _e.ID = __key;
//                _e.Name = data_resouce[key];
//                _source_sort.push(_e);
//                data_doctor_json += key.toString() + ',';
//            }
//        }
//        Set_Value_Doctor_Arrange_Cookie(data_doctor_json, data_Branch_ID)
//    }
//    else {
//        _source_sort = data;
//    }
//    var myNode = document.getElementById(id);
//    if (myNode != null) {
//        myNode.innerHTML = '';
//        let stringContent = '';
//        if (_source_sort && _source_sort.length > 0) {
//            for (let i = 0; i < _source_sort.length; i++) {
//                let item = _source_sort[i];

//                let tr = '<li class="d-flex item py-2 px-3"  data-id="' + item.ID + '">'
//                    + '<p class="text-sm mb-0" >' + item.Name + '</p >'
//                    + '</li >'
//                stringContent = stringContent + tr;
//            }
//        }
//        document.getElementById(id).innerHTML = stringContent;
//    }

//    Event_Sortable_Doctor_Arrange();
//}
//function Event_Sortable_Doctor_Arrange () {
//    $("#div_doctor_arrange_list").sortable({
//        items: "li.item",
//        start: function (event, ui) {
//            $(ui.helper[0]).addClass("cur_doctor");
//        },
//        stop: function (event, ui) {
//            ui.item.removeClass("cur_doctor");
//        }
//    });
//}
//function Get_Value_Doctor_Arrange () {
//    let Token_Doctor = "";
//    $("#div_doctor_arrange_list li.item").each(function () {
//        let id = $(this).attr("data-id");
//        Token_Doctor += id + ",";
//    })
//    Set_Value_Doctor_Arrange_Cookie(Token_Doctor, data_Branch_ID);
//    callMLU_branchonchange()
//    PopupArrange_Doctor();
//}
//function Reset_Value_Doctor_Arrange () {
//    try {
//        let doctor_MLUapp_arrange = window.localStorage.getItem('doctor-MLUapp-doc');
//        if (doctor_MLUapp_arrange != undefined) {
//            window.localStorage.removeItem("doctor-MLUapp-doc");
//            callMLU_branchonchange()
//            PopupArrange_Doctor();
//        }
//    }
//    catch {

//    }
//}
//function Get_Value_Doctor_Arrange_Cookie () {
//    try {
//        let obj_doc;
//        let doctor_MLUapp_arrange = window.localStorage.getItem('doctor-MLUapp-doc');
//        if (doctor_MLUapp_arrange != undefined && doctor_MLUapp_arrange != '') {
//            obj_doc = JSON.parse(doctor_MLUapp_arrange);
//            return (obj_doc[data_Branch_ID] ? obj_doc[data_Branch_ID] : null);
//        } else {
//            return null;
//        }
//    }
//    catch {
//        return null;
//    }

//}
//function Set_Value_Doctor_Arrange_Cookie (_value, _branch) {
//    try {
//        let obj_doc = {};
//        let doctor_MLUapp_arrange = window.localStorage.getItem('doctor-MLUapp-doc');
//        if (doctor_MLUapp_arrange != undefined && doctor_MLUapp_arrange != '') {
//            obj_doc = JSON.parse(doctor_MLUapp_arrange);
//            obj_doc[_branch] = _value;
//        }
//        else {
//            obj_doc[_branch] = _value;
//        }
//        window.localStorage.setItem('doctor-MLUapp-doc', JSON.stringify(obj_doc));
//    }
//    catch {
//        localStorage.removeItem('doctor-MLUapp-doc');
//    }

//}
//#endregion

//#region // Color Board Service Care
//function LoadDataColor_ServiceCare() {
//    AjaxLoad(url = "/Appointment/Appointment/?handler=LoadDataColorServiceCare"
//        , data = { }, async = true
//        , error = function () {
//            notiError_SW();
//        }
//        , success = function (result) {
//            //let data_Service_Care = JSON.parse(result).Table;
//            //let data_Service_Type = JSON.parse(result).Table1;
//            //Render_List_Color(data_Service_Care, "div_Color_Service_Care", "care");
//            //Render_List_Color(data_Service_Type, "div_Color_Service_Type", "type");
//            //Event_Checked_Choose_Filter_Service_Care();
//        }
//    );

//}
//function Render_List_Color(data, id, type) {
//    var myNode = document.getElementById(id);
//    if (myNode != null) {
//        myNode.innerHTML = '';
//        let stringContent = '';
//        if (data && data.length > 0) {
//            for (let i = 0; i < data.length; i++) {
//                let item = data[i];
//                let tr = '<a class="item" data-id="' + item.ID + '">'
//                    + '<label for="choose_item_service_' + type + '_' + item.ID + '" class="ellipsis_one_line" style="max-width:120px;line-height: 18px;" title="' + item.Name + '">'
//                    + item.Name
//                    + '</label>'
//                    + '<div class="color">'
//                    + '<div class="ui checkbox">'
//                    + '<input type="checkbox" checked="checked" name="item_service" class="choose_all_service_type" id="choose_item_service_' + type + '_' + item.ID + '" >'
//                    + '<label ><div class="checkbox_color" style="border-color:' + item.Color + ';background:' + item.Color + '"></div></label>'
//                    + '</div>'
//                    + '</div >'
//                    + '</a >'
//                stringContent = stringContent + tr;
//            }
//        }
//        document.getElementById(id).innerHTML = stringContent;
//    }
//}

//function Choose_All_Service_Care() {
//    if ($("#choose_all_service_care").is(":checked")) {
//        $('#div_Color_Service_Care input[name="item_service"]').prop("checked", true);
//    }
//    else {
//        $('#div_Color_Service_Care input[name="item_service"]').prop("checked", false);
//    }
//}
//function Choose_All_Service_Type() {
//    if ($("#choose_all_service_type").is(":checked")) {
//        $('#div_Color_Service_Type input[name="item_service"]').prop("checked", true);
//    }
//    else {
//        $('#div_Color_Service_Type input[name="item_service"]').prop("checked", false);
//    }
//}

//function Event_Checked_Choose_Filter_Service_Care() {
//    $('#div_Color_Service_Care input[name="item_service"]').change(function () {
//        let isChecked = true;
//        $('#div_Color_Service_Care input[name="item_service"]').each(function () {
//            if ($(this).is(":checked") == false) {
//                isChecked = false;
//            }
//        });
//        (isChecked)
//            ? $("#choose_all_service_care").prop("checked", true)
//            : $("#choose_all_service_care").prop("checked", false);
//    })
//    $('#div_Color_Service_Type input[name="item_service"]').change(function () {
//        let isChecked = true;
//        $('#div_Color_Service_Type input[name="item_service"]').each(function () {
//            if ($(this).is(":checked") == false) {
//                isChecked = false;
//            }
//        });
//        (isChecked)
//            ? $("#choose_all_service_type").prop("checked", true)
//            : $("#choose_all_service_type").prop("checked", false);
//    })
//}

//function Filter_Service_Care() {
//    let Token_Service_Care = "";
//    let Token_Service_Type = "";
//    $('#div_Color_Service_Care input[name="item_service"]').each(function () {
//        if ($(this).is(":checked")) {
//            let id = this.id.replace("choose_item_service_care_", "");
//            Token_Service_Care += id + ",";
//        };
//    });
//    $('#div_Color_Service_Type input[name="item_service"]').each(function () {
//        if ($(this).is(":checked")) {
//            let id = this.id.replace("choose_item_service_type_", "");
//            Token_Service_Type += id + ",";
//        };
//    })
//    Token_Service_Care = ',' + Token_Service_Care + ','
//    Token_Service_Type = ',' + Token_Service_Type + ','
//    let xx = document.getElementsByClassName('cal_app');
//    for (ii = 0; ii < xx.length; ii++) {
//        if (Number(xx[ii].dataset.schedulertype) < 2) {
//            if (!Token_Service_Care.includes(',' + xx[ii].dataset.serviceid + ',')) {
//                xx[ii].style.display = "none";
//            }
//            else {
//                xx[ii].style.display = "block";
//            }
//        }
//        else {
//            if (!Token_Service_Type.includes(',' + xx[ii].dataset.serviceid + ',')) {
//                xx[ii].style.display = "none";
//            }
//            else {
//                xx[ii].style.display = "block";
//            }
//        }
//    }
//    PopupColorBoard_ServiceCare();
//}

//function PopupColorBoard_ServiceCare() {
//    $(".divFiterdata,#ColorBoard_ServiceCare .arrow").toggleClass("active");
//}


//#endregion

//#region // Setting Calendar
//function PopupSetting_Calendar () {
//    $(".div_setting_calendar,#Setting_Calendar .arrow").toggleClass("active");
//}







//#endregion

//#region //
//function LoadComboDoctor(data, id) {
//    var myNode = document.getElementById(id);
//    if (myNode != null) {
//        myNode.innerHTML = '';
//        let stringContent = '';
//        if (data && data.length > 0) {
//            let EmpIsDoctor = data.filter(word => Number(word["ID"]) == sys_employeeID_Main);
//            if ($('#all_doctor').length <= 0 && EmpIsDoctor.length > 0) {
//                let item = EmpIsDoctor[0];
//                stringContent = '<div class="item" data-value=' + item.ID + '>' + item.NickName + '</div>';
//                data_Current_Doctor = item.ID;
//            } else {
//                for (var i = 0; i < data.length; i++) {
//                    let item = data[i];
//                    let tr =
//                        '<div class="item" data-value=' + item.ID + '>' + item.NickName + '</div>'
//                    stringContent = stringContent + tr;
//                }
//            }
//        }
//        document.getElementById(id).innerHTML = stringContent;
//    }
//}

//#endregion

//#region //
//function PopupNoteCalendar() {
//    $(".div_note_calendar,#Note_Calendar .arrow").toggleClass("active");
//}
//#endregion
//function cal_MLU_refresh_appointment () {
//    let cal_app = $(".cal_app");
//    if (cal_app.length != 0) {
//        cal_app.css({
//            "display": "block"
//        })
//    }
//}
//#endregion

//#region // Action


function callMLU_action (uid) {
    ser_AppointmentActionID = uid;
    AjaxLoad(url = "/Appointment/Appointment/?handler=LoadDataAction"
        , data = {'appointment': uid}
        , async = true
        , error = function () {notiError_SW();}
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result);
                if (data.length > 0) {
                    callMLU_filldata(data[0]);
                    $('#MLU_master').addClass('opacity-3 pe-none');
                    $('#MLU_actiontarea').removeClass('d-none');
                }

                else
                    notiError_SW();
            } else {
                notiError_SW();
            }
        }
        , sender = null
    );
}
function callMLU_filldata (data) {
    if (data != undefined && data != null) {
        let avatar = (data.Avatar != '') ? data.Avatar : Master_Default_Pic;
        $('#Apptem_Avatar').attr('src',  avatar);
        customer_status_app = data.StatusApp;
        customer_id_app = data.CustomerID;
        $('#Apptem_CustName').html(data.CustomerName);
        $('#Apptem_Branch').html(data.BranchName);
        $('#Apptem_Content').html(data.Content);
        if (typeof DataServiceCare !== 'undefined' && typeof DataService !== 'undefined'   ) {
            $('#Apptem_Service').html(((Number(data.TypeID) == 1)
                ? (Fun_GetString_ByToken(DataServiceCare, data.ServiceCare))
                : (Fun_GetString_ByToken(DataService, data.ServiceTreat))));
        }
        $('#Apptem_Doctor').html(data.DoctorName);
        let statusName = author_get("UserLang") == 'en' ? data.StatusNameLangOther : data.StatusName;
        $("#Apptem_Status").html(statusName);


        customer_phone_app = data.Phone;
        if (data?.Schedule_Code != '') {
            JsBarcode("#Apptem_Code2D", data.Schedule_Code, {
                lineColor: "#3A416F",
                textMargin: 0,
                width: 1,
                height: 30,
                displayValue: false
            });
        } else {
            $("#Apptem_Code2D").html('');
        }
        $('#Apptem_Code').html('<span title="Customer Code"> [' + data.Cust_Code + ']</span>');
        let CustCodeOld = data.CustCodeOld;
        if (CustCodeOld != "") $("#Apptem_Code").append('<span title="Code Old"> [' + CustCodeOld + ']</span>');
        let CustDocCode = data.DocumentCode;
        if (CustDocCode != "") $("#Apptem_Code").append('<div title="Document Code"> [' + CustDocCode + ']</div>');


        if (data.Room != '') $("#Apptem_Room").html(data.Level + ' - ' + data.Room);
        else $("#Apptem_Room").html('');
        if (data.IsCancel != "0") {
            $('#Apptem_Cancel').html(data.ReasonCancel)
            $("#Apptem_Cancel").show();
        }
        else $("#Apptem_Cancel").hide();
        $('#Apptem_DateFrom').html(data.Time_From + ' - ' + data.Time_To + ' ' + ConvertDateTimeToString_DOW(data.Date_From) + ' ' + formatDateClient(data.Date_From));
        //JsBarcode(".barcodeApppoinment").init();
        if (data.State != 0 && data.StatusApp == 1) {
            $("#Apptem_btnEdit").removeClass("d-none");
            $("#Apptem_btncancel").removeClass("d-none");
            $("#Apptem_btnprint").removeClass("d-none");
            $("#Apptem_btnroom").removeClass("d-none");
        }
        else {
            $("#Apptem_btnEdit").addClass("d-none");
            $("#Apptem_btncancel").addClass("d-none");
            $("#Apptem_btnprint").addClass("d-none");
            $("#Apptem_btnroom").addClass("d-none");
        }
    }
}
function Apptem_CustLink () {
    Apptem_Cancel();
    window.open("/Customer/MainCustomer?CustomerID=" + customer_id_app, '_blank');
    return false;
}
function Apptem_Print () {
    Apptem_Cancel();
    PrintTemplate(ser_AppointmentActionID, 0, 13, versionofWebApplication);
}
function Apptem_DestroyApp () {

    Apptem_Cancel();
    $("#DetailModal_Content").load("/Appointment/AppointmentCancelDetail?CurrentID=" + ser_AppointmentActionID + "&StatusID=" + customer_status_app);
    $('#DetailModal').modal('show');
    return false;
}
function Apptem_Cancel () {
    $('#MLU_master').removeClass('opacity-3 pe-none');
    $('#MLU_actiontarea').addClass('d-none');
    if (typeof DetectFormAppear !== 'undefined') {
        DetectFormAppear = 1;
    }
}
function Apptem_ChangeRoom () {
    Apptem_Cancel();
    $("#DetailModal_Content").load("/Appointment/ChangeRoom?CurrentID=" + ser_AppointmentActionID );
    $('#DetailModal').modal('show');
    return false;
}
function Apptem_Edit () {
    Apptem_Cancel();
    $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CurrentID=" + ser_AppointmentActionID + "&CustomerID=" + customer_id_app );
    $('#DetailModal').modal('show');
    return false;
}
function Apptem_Sms () {
    Apptem_Cancel();
    $('#DetailModal').html('');
    $("#DetailModal_Content").load("/SMS/SmsDetail?CustomerID="
        + customer_id_app
        + "&TicketID=" + 0
        + "&Type=" + "SMSContentGeneral"
        + "&TypeCare=" + 0
        + "&AppID=" + ser_AppointmentActionID);
    $('#DetailModal').modal('show');
    return false;
}
function Apptem_Call () {
    Apptem_Cancel();
    if (typeof CCF_OutcomCall !== 'undefined' && $.isFunction(CCF_OutcomCall)) {
        CCF_OutcomCall(encrypt_phone(customer_phone_app), customer_id_app, 0, 0);
    }
}
//#endregion


//#region //  Temporary

function callMLU_temporary (uid, branch, time, date, resource) {
    note__CurrentID = uid;
    callMLU_temporary_resetField();
    callMLU_temporary_event();
    if (note__CurrentID == 0) {
        note__doctorid = resource;
        note__branch = branch;
        $("#TempDate_from").flatpickr({
            dateFormat: 'd-m-Y H:i',
            enableTime: true,
            defaultDate: StringYMD_SPACE_HMTODate(date + " " + time),
        });
        $('#btnTempcustnew').hide();
        $('#btnTempsendsms').hide();
        $("#TempDoctor_ID").dropdown("refresh");
        $("#TempDoctor_ID").dropdown("set selected", Number(note__doctorid));

    }
    else {
        AjaxLoad(url = "/Appointment/Appointment/?handler=Loadtemdata"
            , data = {'id': Math.abs(note__CurrentID)}, async = true
            , error = function () {
                notiError_SW();
            }
            , success = function (result) {
                if (result != "0") {
                    let _data = JSON.parse(result)[0];
                    note__doctorid = _data.DoctorID;
                    note__branch = _data.Branch_ID;
                    $('#Tempphone1').val(_data.Phone);
                    $('#Tempnamecus').val(_data.Name);
                    $('#TempNoteSchedule').val(_data.Content);
                    $("#TempDate_from").flatpickr({
                        dateFormat: 'd-m-Y H:i',
                        enableTime: true,
                        defaultDate: _data.Date_From,
                    });
                    $('#TempDate_from').val(_data.Date_From);
                    $('#TemServiceCareToken').dropdown('refresh')
                    $('#TemServiceCareToken').dropdown('set selected', _data.Service.split(","));
                    $("#TempDoctor_ID").dropdown("refresh");
                    $("#TempDoctor_ID").dropdown("set selected", Number(note__doctorid));
                    $("#TempTimeSchedule").dropdown("refresh");
                    $("#TempTimeSchedule").dropdown("set selected", Number(_data.BlockTime));
                }
            }
        );
        $('#btnTempcustnew').show();
        $('#btnTempsendsms').show();
    }
    $('#MLU_master').addClass('opacity-3 pe-none');
    $('#MLU_teamporaryarea').removeClass('d-none');
}

function callMLU_temporary_resetField() {
    $("#TempSearchCustomer, #Tempnamecus, #Tempphone1, #TempNoteSchedule").val('');
    $("#txtTempCustCode, #txtTempCustName, #txtTempCreated, #txtTempSource, #txtTempBranch").empty();
    $('#TemServiceCareToken').dropdown('clear');
    $("#FieldTempSearchCustomer_Result").hide();
    $("#formappTemp").removeClass("d-none")
    $("#currentEmpTemp").addClass("d-none")
    note__CurrentCustomer = 0;
    note_data_customer_by_phone = [];
}

function callMLU_temporary_event() {
    let timer_OnchangeSearch;
    $("#TempSearchCustomer").unbind('keyup').keyup(function () {
        if ($(this).val().length > 0) $("#FieldTempSearchCustomer .btnsear_clear").removeClass('opacity-1').addClass('d-none');
        else $("#FieldTempSearchCustomer .btnsear_clear").addClass('opacity-1').addClass('d-none');
        $("#FieldTempSearchCustomer .fa-search").hide();
        $("#FieldTempSearchCustomer .spinner-border").show();
        clearTimeout(timer_OnchangeSearch);
        timer_OnchangeSearch = setTimeout(() => {
            let valueSearch = xoa_dau($(this).val().toLowerCase()).trim();
            Temp_OnSearch(valueSearch.replace(/[^a-zA-Z0-9 ]/g, ''));
        }, 500)
    });

    $("#FieldTempSearchCustomer .btnsear_clear").unbind('click').on('click', function (e) {
        $(this).addClass('opacity-1');
        $('#TempSearchCustomer').val('');
        $("#FieldTempSearchCustomer_Result").slideUp(200);
        $("#FieldTempSearchCustomer_ResultContent").empty();
        $("#FieldTempSearchCustomer .fa-search").show();
        $("#FieldTempSearchCustomer .spinner-border").hide();
        $("#currentEmpTemp").addClass("d-none")
        $("#formappTemp").removeClass("d-none")
        note__CurrentCustomer = 0;
    });
}

function TempClose () {
    $('#MLU_master').removeClass('opacity-3 pe-none');
    $('#MLU_teamporaryarea').addClass('d-none');
    DetectFormAppear = 1;
    CurrentID_Appointment_Temp == 0;
}

function Temp_OnSearch (textsearch) {
    if (textsearch.length > 3) {
        AjaxLoad(url = "/Appointment/Appointment/?handler=SearchCustomer"
            , data = { 'search': textsearch }
            , async = true
            , error = function () { notiError_SW(); }
            , success = function (result) {
                if (result != "0") {
                    note_data_customer_by_phone = JSON.parse(result);
                    $("#FieldTempSearchCustomer_ResultContent").empty();
                    $("#FieldTempSearchCustomer_NoResult").addClass('d-none');
                    if (note_data_customer_by_phone.length != 0) {
                        Temp_Render(note_data_customer_by_phone, 'FieldTempSearchCustomer_ResultContent');
                        //$("#FieldTempSearchCustomer_Result").slideDown(200);
                    }
                    else {
                        note__CurrentCustomer = 0;
                        $('#currentEmpTemp').addClass("d-none")
                        $("#FieldTempSearchCustomer_NoResult").removeClass('d-none');
                    }
                    $("#FieldTempSearchCustomer_Result").slideDown(200);
                }
            }
            , sender = null
            , before = function (e) {
            }
            , complete = function (e) {
                $("#FieldTempSearchCustomer .fa-search").show();
                $("#FieldTempSearchCustomer .spinner-border").hide();
                $("#FieldTempSearchCustomer .btnsear_clear").removeClass('d-none')
            }
        );
    }
    else {
        note__CurrentCustomer = 0;
        $('#currentEmpTemp').addClass("d-none")
        $("#FieldTempSearchCustomer .fa-search").show();
        $("#FieldTempSearchCustomer .spinner-border").hide();
        $("#FieldTempSearchCustomer .btnsear_clear").removeClass('d-none')
    }
}

async function Temp_Render(data, id) {
    return new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let item = data[i]
                    let tr = `
                    <li class="nav-item">
                        <a class="text-sm nav-link cursor-pointer item-cust" data-id="${item.ID}" data-hover>
                            <span class="text-primary"><i class="far fa-user text-xs me-2"></i>${item.CustCode}</span> - ${item.Name}
                        </a>
                    </li>
                    <hr class="horizontal dark my-0 opacity-2">
                    `
                    myNode.insertAdjacentHTML('beforeend', tr);
                }
                Temp_EventChooseCustomer(id);
            }
        }
    })
}

function Temp_EventChooseCustomer(id) {
    $('#' + id + ' .item-cust').click(function () {
        let curr_id = Number($(this).attr('data-id'));
        TempFillCustomer(curr_id);
    })
}

function TempExecuted () {
    var data = new Object();
    data.ServiceCare = $('#TemServiceCareToken').dropdown('get value');
    data.branch_ID = Number(note__branch);
    data.Doctor_ID = Number($('#TempDoctor_ID').dropdown('get value')) ? Number($('#TempDoctor_ID').dropdown('get value')) : Number(note__doctorid);
    data.Note = $('#TempNoteSchedule').val() ? $('#TempNoteSchedule').val() : "";
    data.Phone = $('#Tempphone1').val() ? $('#Tempphone1').val() : "";
    data.Name = $('#Tempnamecus').val() ? $('#Tempnamecus').val() : "";
    data.Date_from = $('#TempDate_from').val() ? $('#TempDate_from').val() : "";
    data.numberDateTo = Number($('#TempTimeSchedule').dropdown('get value')) ? Number($('#TempTimeSchedule').dropdown('get value')) : 0;
    if (note__CurrentCustomer != 0) {
        TempClose();
        $("#DetailModal_Content").html('');
        $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CustomerID=" + note__CurrentCustomer
            + "&Doctor_ID=" + data.Doctor_ID
            + "&Date_from=" + data.Date_from.replace(' ', 'x')
            + "&numberDateTo=" + data.numberDateTo
            + "&branch_ID=" + data.branch_ID);
        $('#DetailModal').modal('show');
    }
    else {
        $('#formappTemp').form('validate form');
        if ($('#formappTemp').form('is valid')) {
            AjaxLoad(url = "/Appointment/Appointment/?handler=ExcuteTemp"
                , data = {
                    'data': JSON.stringify(data)
                    , 'CurrentID': Math.abs(note__CurrentID)
                }, async = true
                , error = function () {
                    notiError_SW();
                }
                , success = function (result) {
                    if (result != "0") {
                        _d = JSON.parse(result)[0];
                        let isupdate = ((note__CurrentID) == "0" ? 0 : 1)

                        if (typeof update_cal_MLU_app !== 'undefined' && $.isFunction(update_cal_MLU_app)) {
                            update_cal_MLU_app(_d.ID, _d.Branch, Number(_d.DateFrom_Int), 0, 1, isupdate);
                        }
                        TempClose();
                    }
                }
            );
        }
        return false;
    }
}

function TempNewCustomer () {
    DetectFormAppear = 0;
    let Phone = $('#Tempphone1').val() ? $('#Tempphone1').val() : "";
    let Name = $('#Tempnamecus').val() ? $('#Tempnamecus').val() : "";
    let Date_from = $('#TempDate_from').val() ? $('#TempDate_from').val() : "";
    let ServiceCareToken = $('#TemServiceCareToken').dropdown('get value');
    let doctorID = $("#TempDoctor_ID").dropdown("get value");
    let TimeSchedule = Number($('#TempTimeSchedule').dropdown('get value')) ? Number($('#TempTimeSchedule').dropdown('get value')) : 0;
    let NoteSchedule = $('#TempNoteSchedule').val() ? $('#TempNoteSchedule').val() : "";

    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/CustomerDetail?"
        + 'branch=' + note__branch
        + '&servicecaretoken=' + ServiceCareToken
        + '&doctorid=' + doctorID
        + '&dateapp=' + encodeURI(Date_from)
        + '&custname=' + encodeURI(Name)
        + '&noteschedule=' + encodeURI(NoteSchedule)
        + '&phone=' + Phone.trim()
        + '&timeschedule=' + TimeSchedule.toString()
        + '&type=app_temp&apptemid=' + note__CurrentID.toString()
        + '&ver=' + versionofWebApplication
    );
    $('#DetailModal').modal('show');
}

function TempFillCustomer(id) {
    if (id != 0) {
        $("#FieldTempSearchCustomer_Result").hide();
        let data = note_data_customer_by_phone.filter(word => word["ID"] == id);
        if (data.length != 0) {
            TempFillCustomer_Executed(data);
        }
    }
}

function TempFillCustomer_Executed (data) {

    if (data.length != 0) {
        let item = data[0];
        $('#txtTempSource').html(item.Source);
        $('#txtTempCustCode').html(item.CustCode);
        $('#txtTempCreated').html(ConvertDateTimeUTC_DMYHM(item.Created));
        $('#txtTempBranch').html(item.BranchName);
        $('#txtTempCustAvatar').attr("src",item.Image!="" ? item.Image : Master_Default_Pic);
        $('#txtTempCustName').html(item.Name);
        $("#currentEmpTemp").removeClass("d-none")
        $("#formappTemp").addClass("d-none")
        let CustCodeOld = item.CustCodeOld;
        if (CustCodeOld != "") $("#txtTempCustCode").append('<span title="Code Old"> [' + CustCodeOld + ']</span>');
        let CustDocCode = item.DocumentCode;
        if (CustDocCode != "") $("#txtTempCustCode").append('<span title="Document Code"> [' + CustDocCode + ']</span>');

        note__CurrentCustomer = Number(item.ID);
    }
}

function TempSms_ToCust () {
    let NameTemp = encodeURIComponent($("#Tempnamecus").val());
    let Phone = $("#Tempphone1").val();

    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/SMS/SmsDetail?CustomerID=" + 0
        + "&TicketID=" + 0
        + "&Type=" + "SMSContentGeneral"
        + "&TypeCare=" + 0
        + "&AppID=" + 0
        + "&NameTemp=" + NameTemp
        + "&Phone=" + Phone
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
    return false;
}

//#endregion