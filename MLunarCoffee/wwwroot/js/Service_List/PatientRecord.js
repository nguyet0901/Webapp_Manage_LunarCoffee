function Patiend_Record_ShowAll_Service() {
    let _type = $('#Show_Service_All_Record').attr('data-type');
    if (_type == "noview") {
        Patient_Record_Prepare_Functioning();
        Tab_List_Customer_Service_Execute(0, 0);
    }
    else {
        Patient_Record_Done_Functioning();
        Tab_List_Customer_Service();
    }
}

function Patient_Record_Add() {
    $('#Div_Tab_List_Service_Master').hide();
    $('#Div_Tab_Service_Detail').show();
    $('#Div_Tab_Service_Detail').empty();

    $("#Div_Tab_Service_Detail").load("/Customer/PatientRecord/PatientRecordDetail?CustomerID=" + ser_MainCustomerID
        + "&Type=" + 1
        + "&ver=" + versionofWebApplication);
}
function Patient_Record_Print() {
    if (ser_current_patient_record != 0) {
        PrintTemplate(ser_current_patient_record, ser_MainBranchID, 24, versionofWebApplication, ser_MainCustomerID);
    }
}

function Patient_Record_Delete() {
    const promise = notiConfirm();
    promise.then(function () { Execute_Patient_Record_Delete(ser_current_patient_record); }, function () { });
}
function Execute_Patient_Record_Delete(id) {
    AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=DeleteItemPatientRecord"
        , data = {
            'id': id
        }
        , async = true
        , error = function () { notiError_SW() }
        , success = function (result) {
            if (result == "1") {
                notiSuccess();
                delete ser_current_data_patient_record[ser_current_patient_record];
                let size = Object.keys(ser_current_data_patient_record).length;
                if (size == 0) ser_current_patient_record = 0;
                else {
                    let firstkey = 0;
                    for ([key, value] of Object.entries(ser_current_data_patient_record)) {
                        if (firstkey == 0) firstkey = key;
                    }
                    ser_current_patient_record = firstkey;
                    Tab_List_Customer_Patient_Record_Trigger(firstkey);
                    let _id = 'patient_record_' + firstkey;
                    if ($('#' + _id).length) {
                        $('#' + _id).remove();
                    }
                }
                //Tab_List_Customer_Patient_Record_Load();
            } else {
                notiError_SW();
            }
        }
    );
}

function Tab_List_Customer_Patient_Record_Trigger(_idtrigger) {
    let size = Object.keys(ser_current_data_patient_record).length;
    
    if (size != 0) {
        if (ser_current_data_patient_record[_idtrigger] != undefined) {
            let _id = 'patient_record_' + _idtrigger;
            if ($('#' + _id).length) {
                $('#' + _id).trigger("click");
            }
        }
    }
}
function Tab_List_Customer_Patient_Record_Event() {
    $(".patient_record_item").on('click', function (event) {
        let _item = document.getElementsByClassName('patient_record_item');
        let id = Number($(this).attr("data-id"));
        let lock = Number($(this).attr("data-lock"));
        let id_setting = 'imgExecute_PatientRecord_' + id;
        $('.imgExecute_PatientRecord').hide();
        $('#' + id_setting).show();
        for (_i = 0; _i < _item.length; _i++) {
            if (Number(_item[_i].dataset.id) == Number(id)) {
                if (!_item[_i].className.includes('recordselected')) _item[_i].className = _item[_i].className + ' recordselected';
            }
            else _item[_i].className = 'patient_record_item';
        }
        //$('#cus_service_list_button').show();
        ser_current_patient_record = Number(id);
        ser_current_patient_record_lock = Number(lock);
        ser_current_treatment_plant_id = 0;
        ser_current_treatment_plant_data = [];
        if (Number(ser_sys_Treatment_Plan) == 1 && ser_current_patient_record_lock == 0 && $('#buttonadd_treatment_plain').length)
            $('#buttonadd_treatment_plain').show();
        else if ($('#buttonadd_treatment_plain').length)
            $('#buttonadd_treatment_plain').hide();
        Tab_List_Customer_Service();

        event.stopPropagation();

    });
    $(".patient_record_detail").on('click', function (event) {
        if (ser_current_patient_record != 0) {
            $('#Div_Tab_List_Service_Master').hide();
            $('#Div_Tab_Service_Detail').show();
            $('#Div_Tab_Service_Detail').empty();
            $("#Div_Tab_Service_Detail").load("/Customer/PatientRecord/PatientRecordDetail?CustomerID=" + ser_MainCustomerID
                + "&PatientRecordID=" + ser_current_patient_record
                + "&ver=" + versionofWebApplication);
        }

    });
    $(".imgExecute_PatientRecord").click(function (event) {
        let left = $(this).closest(".patient_record_item").position().left
        $("#popupExecute_PatientRecord").css({ left: left + 90 });
        $("#popupExecute_PatientRecord").css({ top: 17 });
        $("#popupExecute_PatientRecord").show();
        $('#popupExecute_PatientRecord').dropdown('clear');

        event.stopPropagation();
    });

}

//#region // Load and render
function Render_Tab_List_Patient_Record(data, id) {
    var myNode = document.getElementById(id);

    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        for ([key, value] of Object.entries(data)) {
            let tr = Render_Tab_List_Patient_Record_Each(key, value)
            stringContent = tr+stringContent ;
        }

        document.getElementById(id).innerHTML = stringContent;
    }
    Tab_List_Customer_Patient_Record_Event();
}
function Render_Tab_List_Patient_Record_Add(_key, _value, id) {
    let tr = Render_Tab_List_Patient_Record_Each(_key, _value)
    document.getElementById(id).innerHTML = tr + document.getElementById(id).innerHTML;
    Tab_List_Customer_Patient_Record_Event();
}
function Render_Tab_List_Patient_Record_Each(_key, _value) {
    let item = _value;
    let img = (item.Image != "") ? item.Image : Master_Default_Pic;
    return '<div class="text-center px-3 position-relative">'
        + '<a id="patient_record_' + _key
        + '" data-lock="' + item.IsClose
        + '" data-id="' + _key
        + '" class="patient_record_item">'
        + '<img alt="' + Outlang["Hinh_anh_benh_nhan"] + '" class="avatar avatar-lg  avatar-sm border border-secondary" onerror="' + Master_OnLoad_Error_Image(this) + '" src="' + img + '"  />'
        + '<span class="patient_record_detail mt-2 cursor-pointer text-primary mb-0  text-sm text-nowrap">' + item.TeamplateName +'</span>'
        + '<span class="patientrename cursor-pointer mt-2 text-dark mb-0  text-sm text-nowrap">' + item.TeamplateName + '</span>'
        + '</a>'
        + '</div>';
}
