function AD_OnBranchEvent () {
    if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1) AD_LoadInfo();
    if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1 && $('#app_detail_ex_timeline').length != 0) {
        AD_TimeLineLoad();
    }
}
function AD_OnDoctorEvent () {
    if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1 && $('#app_detail_ex_timeline').length != 0) {
        AD_TimeLineLoad();
    }

}
function AD_OnTimeEvent () {
    if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1 && $('#app_detail_ex_timeline').length != 0) {

        AD_TimeLineDetect();
    }
}
function AD_OnDateFormEvent () {
    clearTimeout(Timer_Change_Appointment);
    Timer_Change_Appointment = setTimeout(function (e) {
        if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1) AD_LoadInfo();
        if (typeof App_Detail_Flag !== 'undefined' && App_Detail_Flag == 1 && $('#app_detail_ex_timeline').length != 0) {
            AD_TimeLineLoad();
        }

    }, 500);



}
function AD_LoadInfo () {

    let date = $('#App_Detail_Date_From').val() + ' ' + $('#App_Detail_DateFrom_Time').val();
    let branchID = Number($('#App_Detail_Branch_ID').dropdown('get value')) ? Number($('#App_Detail_Branch_ID').dropdown('get value')) : 0;

    AjaxLoad(url = "/Appointment/AppointmentDetail/?handler=GetInfoAppointment"
        , data = {
            'CustomerID': ser_CustomerAppointmentID
            , 'BranchID': branchID
            , 'Date': date
            , 'CurrentAppID': ser_CurrentAppointmentID
        }
        , async = true
        , error = function () {notiError_SW();}
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result);
                let dataBranch = data.Table;
                let dataCustomer = data.Table1;
                AD_RenderInfo(dataBranch, dataCustomer);
            }
        }
        , sender = null
    );

}

function AD_RenderInfo (dataBranch, dataCustomer) {
    if (dataBranch && dataBranch.length > 0) {
        //$("#app_detail_branchname").html(dataBranch[0].BranchName)
        //$("#app_branch_current").html(dataBranch[0].BranchName)
        $("#app_total_morning").html(dataBranch[0].TotalApp_M)
        $("#app_total_afternoon").html(dataBranch[0].TotalApp_A)
        //$("#app_date_from").html(dataBranch[0].DateInfo)
    }
    if (dataCustomer && dataCustomer.length > 0) {
        $("#app_customer_next").html(dataCustomer[0].Date_From)
        // $("#app_customer_next").html('<u>' + dataCustomer[0].ScheduleCode + '</u>: ' + dataCustomer[0].Date_From)
    }
    else {
        $("#app_customer_next").html('-');
    }
}
function AD_OnTypeEvent (value) {
    $('#AppDetail_TypeArea .appde_text').removeClass('active');
    $('#AppDetail_TypeArea .appde_text[data-id=' + value + ']').addClass('active');
    if (value == 1) {
        $(".cbbApp_Detail_ServiceCare").show();
        $(".ccbApp_Detail_ServiceTreatment").hide();

        

        $("#App_Detail_ServiceTreatmentToken").dropdown("clear");
        document.getElementById("App_Detail_Consult_ID").classList.remove("disabled");
    }
    else {
        $(".cbbApp_Detail_ServiceCare").hide();
        $(".ccbApp_Detail_ServiceTreatment").show();
        $("#App_Detail_ServiceCareToken").dropdown("clear");
        $("#App_Detail_Consult_ID").dropdown("clear");
        document.getElementById("App_Detail_Consult_ID").classList.add("disabled");
        
    }
}
function AD_TimeLineDetect () {
    if ($('#Doctor_Scheduler_Ext_Current').length) {
        let obj = $('#Doctor_Scheduler_Ext_Current')[0];
        let e = {};
        e._id = obj.attributes.id.nodeValue;

        let date = $('#App_Detail_Date_From').val() + ' ' + $('#App_Detail_DateFrom_Time').val();
        let date_now = GetDateTime_Now_Only_Date();
        let _minute = Number($('#App_Detail_TimeSchedule').dropdown('get value'));
        let distance_day = Math.ceil((ConvertString_DMY_To_DateTime(date) - date_now) / (1000 * 60 * 60 * 24));

        let _start = ConvertString_DMYHM_To_DateTime(date).addDays(-distance_day);
        let _stop = ConvertString_DMYHM_To_DateTime(date).addDays(-distance_day).addMinutes(_minute);
        e.start = ConvertDateTime_To_Timespan_TimeZone(_start);
        e.stop = ConvertDateTime_To_Timespan_TimeZone(_stop);

        AD_TimeLineEdit(e);
    }
    else {
        let e = {};
        e._id = "Doctor_Scheduler_Ext_Current";
        let date = $('#App_Detail_Date_From').val() + ' ' + $('#App_Detail_DateFrom_Time').val();
        let date_now = GetDateTime_Now_Only_Date();
        let _minute = Number($('#App_Detail_TimeSchedule').dropdown('get value'));
        let distance_day = Math.ceil((ConvertString_DMY_To_DateTime(date) - date_now) / (1000 * 60 * 60 * 24));
        let _start = ConvertString_DMYHM_To_DateTime(date).addDays(-distance_day);
        let _stop = ConvertString_DMYHM_To_DateTime(date).addDays(-distance_day).addMinutes(_minute);
        e.start = ConvertDateTime_To_Timespan_TimeZone(_start);
        e.stop = ConvertDateTime_To_Timespan_TimeZone(_stop);
        AD_TimeLineAdd(e);
    }
}
function AD_SaveChecking (_data) {

    AjaxLoad(url = "/Appointment/AppointmentDetail/?handler=CheckDataDetail"
        , data = {
            'BranchID': _data.branch_ID
            , 'CustomerID': ser_CustomerAppointmentID
            , 'DateFrom': _data.Date_from
            , 'CurrentAppID': ser_CurrentAppointmentID
        }
        , async = false
        , error = function () {notiError_SW();}
        , success = function (result) {
            let dataResult = JSON.parse(result)
            if (dataResult[0].result == "1") {
                AD_SaveExecuted(_data);
            }
            else {
                let result = dataResult[0].result;
                let schedule_id = dataResult[0].id;
                ser_NearAppointmentID = dataResult[0].id;
                let date_from_new = _data.Date_from;
                switch (result) {
                    case "AppInDay":
                        AD_DuplicateShow(schedule_id, "AppInDay", date_from_new)
                        break;
                    case "AppNear":
                        if (ser_CurrentAppointmentID == "0") {
                            AD_DuplicateShow(schedule_id, "AppNear", date_from_new);
                        }
                        else {
                            AD_SaveExecuted(_data);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        , sender = null
    );

}
function AD_SaveExecutedLoad () {

    if (typeof IsPageGeneral === 'function' && IsPageGeneral == 1) {
        location.reload();
    }

    if (typeof LoadCustomerCareAjax === 'function') {
        notiSuccess();
        LoadCustomerCareAjax();
    }

    if (typeof TicketAction_SchedulerLoad === 'function') {
        TicketAction_SchedulerLoad();
    }
    if (typeof GeneralInfo_LoadApp === 'function') {
        GeneralInfo_LoadApp();
    }
    if (typeof LoadTickedValueAjax === 'function') {
        notiSuccess();
        LoadTickedValueAjax();
    }
    if (typeof LoadScheduleAjax === 'function') {
        notiSuccess();
        LoadScheduleAjax();
    }
    if (typeof SSO_handleNextSchedule === 'function') {
        notiSuccess();
        SSO_handleNextSchedule(ser_CustomerAppointmentID);
    }
    if (typeof DAHD_CallBackAfterHandle === 'function') {
        DAHD_CallBackAfterHandle(ser_AC_AppointmentID);
    }

    else if (typeof Treatment_List_Service_Load === 'function') {
        notiSuccess();
        Treatment_List_Service_Load();
    }
    else if (typeof callvtt_branchload === 'function') {

    }
    else if (typeof TicketAction_LoadInfo === 'function') {
        TicketAction_LoadInfo();
    }
    else if (typeof GeneralInfo_LoadApp === 'function') {
        GeneralInfo_LoadApp();
    }
    else {
        location.reload();
    }

    // load main app next
    if (typeof LoadCustomerScheduleNext === 'function') {
        LoadCustomerScheduleNext();
    }

    if (typeof AD_CloseModal === 'function') {
        AD_CloseModal();
    }
    else {
        CloseModal();
    }
    
}

//#region // Duplicate
function AD_DuplicateBack () {
    $('#appointmentDetail').removeClass("appdetail_disable");
    $('#appointmentDuplicate').addClass("d-none");
    $('#AD_DuplicateArea').addClass('opacity-0');
}
function AD_DuplicateSave () {
    AD_DuplicateBack()
    AD_SaveAction(2);
}
function AD_DuplicateShow (idSchedule, type, date_from_new) {
    date_from_new = date_from_new.split(" ")[0];
    AjaxLoad(url = "/Appointment/AppointmentDetail/?handler=Duplicate"
        , data = {'AppID': idSchedule}
        , async = true
        , error = function () {notiError_SW();}
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result);
                let DataApp = data.DataApp;
                let header = '';
                if (type == "AppInDay") {
                    header = 'Duplicate';
                    $('#AD_DuplicateHeader').removeClass('text-dark');
                    $('#AD_DuplicateHeader').addClass('text-danger');
                    $('#AD_DuplicateReject').removeClass('d-none');
                    $('#AD_DuplicateSaveBtn').addClass('d-none');
                    $('#AD_DuplicateNotice').addClass('d-none');

                }
                if (type == "AppNear") {
                    header = 'Notice';
                    $('#AD_DuplicateHeader').addClass('text-dark');
                    $('#AD_DuplicateHeader').removeClass('text-danger');
                    $('#AD_DuplicateReject').addClass('d-none');
                    $('#AD_DuplicateSaveBtn').removeClass('d-none');
                    $('#AD_DuplicateNotice').removeClass('d-none');
                }
                $('#AD_DuplicateHeader').html(header);
                $("#AD_DuplicateHour").html(DataApp[0].HourApp);
                $("#AD_DuplicateDate").html(DataApp[0].DateApp + ' ' + DataApp[0].HourApp);
                $("#AD_DuplicateBranch").html(DataApp[0].BranchName);
                $('#AD_DuplicateCreated').html(DataApp[0].UserCreated);
                $('#appointmentDetail').addClass("appdetail_disable");
                $('#appointmentDuplicate').removeClass("d-none");
                $('#AD_DuplicateArea').removeClass('opacity-0');
            }
        }
        , sender = null
    );

    return false;
}
//#endregion
//#region // ReTreat
function AD_RetreatShow (CustomerID) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Appointment/AppointmentReTreatUpdate" + "?CustomerID=" + CustomerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

    // alert(2)
    // PrepageModal2();
    // $("#DetailModalLV2_Content").load("/Appointment/AppointmentReTreatUpdate" + "?CustomerID=" + CustomerID + "&ver=" + versionofWebApplication);
    // $('#DetailModalLV2').modal('show');
    return false;
}
//#endregion