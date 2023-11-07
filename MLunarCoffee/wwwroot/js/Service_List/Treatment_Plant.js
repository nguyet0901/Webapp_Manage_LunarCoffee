function Treatment_Plan_LoadInfo(cusid, plantid) {
    AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=LoadInfo_Treatment_Plant"
        , data = {
            'custID': cusid, 'plantid': plantid
        }
        , async = true
        , error = function () { notiError_SW() }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                let data_info = data["Table"];

                if (data_info != undefined && data_info.length == 1) {
                    Write_Value_Info_Total_List(data_info[0])
                }

            }
        }
        , sender = null
        , before = function () {
            Refresh_Value_Info_Total_List();
        }
        , complete = function (e) {
        }
    );
}
function Refresh_Value_Info_Total_List() {
    $('#treat_Total_price').html("0");
    $('#treat_Total_price_confirm').html("0");
    $('#treat_Total_price_nonconfirm').html("0");
}
function Write_Value_Info_Total_List(data_info) {
    $('#treat_Total_price').html(formatNumber(data_info.Total_price));
    //$('#treat_Total_price_percent_treat').html(formatNumber(data_info.Total_price_percent_treat));
    $('#treat_Total_price_confirm').html(formatNumber(data_info.Total_price_confirm));
   // $('#treat_Total_price_paid').html(formatNumber(data_info.Total_price_paid));
    $('#treat_Total_price_nonconfirm').html(formatNumber(data_info.Total_price_nonconfirm));
    //$('#treat_Total_price_left').html(formatNumber(data_info.Total_price_left));

}


function Treatment_Plan_Delete() {
    const promise = notiConfirm();
    promise.then(function () { Execute_Treatment_Plan_Delete(ser_current_treatment_plant_id); }, function () { });
}
function Execute_Treatment_Plan_Delete(id) {
    AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=Delete_Treatment_Plan"
        , data = {'id': id}
        , async = true
        , error = function () { notiError_SW() }
        , success = function (result) {
            if (result == "1") {
                notiSuccess();
                if (ser_current_patient_record != 0) {
                    Tab_List_Customer_Patient_Record_Trigger(ser_current_patient_record);
                }
                else {
                    delete ser_current_treatment_plant_data[id];
                    $('#treatment_plan_' + id).remove();
                    Tab_List_Customer_Treatment_Plan_Trigger_First();
                }
            } else {
                notiPopup_Type("delete_treatment_plan_error");
            }
        }

    );
}

function Treatment_Plan_Detail() {
    if (ser_current_treatment_plant_id != 0) {
        $('#Div_Tab_List_Service_Master').hide();
        $('#Div_Tab_Service_Detail').show();
        $('#Div_Tab_Service_Detail').empty();
        $("#Div_Tab_Service_Detail").load("/Customer/Service/TreatmentPlan/TreatmentPlanInfo?CustomerID=" + ser_MainCustomerID
            + "&TreatmentPlanID=" + ser_current_treatment_plant_id
            + "&PatientRecordID=" + ser_current_patient_record
            + "&ver=" + versionofWebApplication);
    }
}
function Treatment_Plan_Close() {
    if (ser_current_treatment_plant_id != 0) {
        const promise = notiConfirm_Type("close_treatment_plan");
        promise.then(function () { ExcTreatment_Plan_Close_Open(ser_current_treatment_plant_id, 1); }, function () { });
    }
}
function Treatment_Plan_Open() {
    if (ser_current_treatment_plant_id != 0) {
        const promise = notiConfirm_Type("open_treatment_plan");
        promise.then(function () { ExcTreatment_Plan_Close_Open(ser_current_treatment_plant_id, 0); }, function () { });
    }
}
function Treatment_Plan_Choose (Value) {
    if (ser_current_treatment_plant_id != 0) {
        const promise = notiConfirm_Type("change_the_selected");
        promise.then(function () {
            let data = [];
            let tokenTabID = ser_current_treatment_plant_data[ser_current_treatment_plant_id].Token_Tab;
            let listTokenTab = tokenTabID.split(',');
            for (let i = 0; i < listTokenTab.length; i++) {
                if (listTokenTab[i] != '') {
                    let elm = $('#dtContentTab .ConfirmUsing[data-id=' + listTokenTab[i] + ']');
                    let Package = elm[0].getAttribute('data-package');
                    let CurrentValue = (elm[0].checked ? 1 : 0);
                    if (CurrentValue != Value) {
                        let e = {
                            "TabID": listTokenTab[i]
                            , "Value": Value
                            , "Package": Package
                        };
                        data.push(e);
                    }
                }
            }
            TabService_Change_Choose(data);
        }, function () { });
    }
}
function ExcTreatment_Plan_Close_Open(id, Status) {
    AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=IsCloseTreatmentPlan"
        , data = { 'TreatmentPlanID': id, 'Status': Status}
        , async = true
        , error = function () { notiError_SW() }
        , success = function (result) {
            if (result != "0") {
                notiSuccess();
                Tab_List_Customer_Treatment_Plant_Load(ser_current_patient_record);
            } else {
                notiError_SW();
            }
        }

    );
}


function Treatment_Plan_Print() {
    let ID = Number(ser_current_treatment_plant_id);
    syslog_cutcon('p', ser_MainCustomerID, '');
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load('/Print/print?Type=treatmentplan_depit&DetailID=' + ID);
    $('#DetailModal').modal('show');
}

function Treatment_Plan_Clone() {
    if (ser_current_treatment_plant_id != 0) {
        AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=Clone_Treatment_Plan"
            , data = { 'id': ser_current_treatment_plant_id}
            , async = true
            , error = function () { notiError_SW() }
            , success = function (result) {
                if (result != "0") {
                    notiSuccess();
                    Tab_List_Customer_Service_Execute(0, 0);
                } else {
                    notiError_SW();
                }
            }

        );
    }
}
function Treatment_Plan_Add() {
    ser_current_treatment_plant_id = 0;
    $('#Div_Tab_List_Service_Master').hide();
    $('#Div_Tab_Service_Detail').show();
    $('#Div_Tab_Service_Detail').empty();
    $("#Div_Tab_Service_Detail").load("/Customer/Service/TabMulti?CustomerID="
        + ser_MainCustomerID
        + "&PatientRecordID=" + ser_current_patient_record
        + "&Plan=" + 0
        + "&ver=" + versionofWebApplication);
}

function Tab_List_Customer_Treatment_Plan_Trigger_First() {
    let size = Object.keys(ser_current_treatment_plant_data).length;

    if (size != 0) {
        let firstkey = Object.keys(ser_current_treatment_plant_data)[0];


        if (ser_current_treatment_plant_data[firstkey] != undefined) {
            let _id = 'treatment_plan_' + firstkey;
            if ($('#' + _id).length) {
                $('#' + _id).trigger("click");
            }
        }

    }
}
function Tab_List_Customer_Treatment_Plan_Trigger(_idtrigger) {

    let size = Object.keys(ser_current_treatment_plant_data).length;

    if (size != 0) {
        if (ser_current_treatment_plant_data[_idtrigger] != undefined) {
            let _id = 'treatment_plan_' + _idtrigger;
            if ($('#' + _id).length) {
                $('#' + _id).trigger("click");
            }
        }

    }
}

function Render_Tab_List_Treatment_Plant_Status(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr = '<a class="item button_treatment_plan_status" data-status="' + item.ID + '">'
                    + '<span>'
                    + '<i id="button_treatment_plan_status_' + item.ID + '"></i>'
                    + item.Name
                    + '</span></a>';
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    Tab_List_Current_Treatment_Plant_Status_Event();
}
function Tab_List_Current_Treatment_Plant_Status_Event() {
    $("#Treat_Plan_Status").on('click', '.button_treatment_plan_status', function (event) {
        let statusID = $(this).attr('data-status');
        AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=Treatment_Plan_Status_Exchange"
            , data = {
                "StatusID": statusID,
                "TreatmentPlantID": ser_current_treatment_plant_id }
            , async = true
            , error = function () { notiError_SW() }
            , success = function (result) {
                if (result != "0") {
                    let _data = JSON.parse(result);
                    if (_data != undefined && _data.length == 1) {

                        ser_current_treatment_plant_data[ser_current_treatment_plant_id].ExchangeStatus = _data[0].ID;
                        ser_current_treatment_plant_data[ser_current_treatment_plant_id].ExchangeStatusName = _data[0].Name;
                        let _id = 'exchangestatus_' + ser_current_treatment_plant_id;
                        if ($('#' + _id).length) {
                            $('#' + _id).html(_data[0].Name);
                        }
                    }
                } else {
                    notiError_SW();
                }
            }

        );
    });
}

function Render_Tab_List_Treatment_Plant(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';

        for ([key, value] of Object.entries(data)) {
            //for (var i = 0; i < data.length; i++) {
            let item = value;
            let createdby = Fun_GetObject_ByID(DataEmployee, item.EmpID)
            let img = (createdby != null) ? createdby.Avatar : Master_Default_Pic;


            let tr = '<li id="treatment_plan_' + key
                + '" data-id="' + key
                + '" data-lock="' + item.IsLock
                + '" class="d-flex align-items-center nav-item treatment_plan_item" role="presentation">'
                + '<a class="d-flex align-items-center nav-link p-0 text-sm" data-bs-toggle="pill"   role="tab">'
                + '<img class="d-inline-flex icon-shape icon-xs mx-2 bg-gradient-dark shadow text-center" src="' + img + '"/>'
                + '<p class=" text-md mb-0">' + item.Name + '</p>'
                + '<span id="exchangestatus_' + key + '" class="text-xs exchangestatus_name">' + item.ExchangeStatusName + '</span>'
                + '<i data-id="' + key + '" id="imgExecute_Treatment_Plan_' + key + '" class=" text-primary  imgExecute_Treatment_Plan text-lg px-3 py-2 fas fa-ellipsis-v"></i>'
                + '</a>'
                + '</li>';


            stringContent = tr + stringContent;
        }

        document.getElementById(id).innerHTML = stringContent;
    }
    $('.imgExecute_Treatment_Plan').hide();
}
function Tab_List_Customer_Treatment_Plan_Event() {
    $(".treatment_plan_item").unbind('click').on('click', function (event) {
        $('#Treatment_Plan_Folder').hide();
        $('#FolderTreatmentPlan').empty();
        $('#ImgTreatmentPlan').empty();
        $('#service_info_list').hide();

        if (ser_Treatment_Plan_Comparing == 0) {
            let _item = document.getElementsByClassName('treatment_plan_item');
            let id = Number($(this).attr("data-id"));
            let id_setting = 'imgExecute_Treatment_Plan_' + id;
            $('.imgExecute_Treatment_Plan').hide();
            $('#' + id_setting).show();
            for (_i = 0; _i < _item.length; _i++) {
                if (Number(_item[_i].dataset.id) == Number(id)) {
                    if (!_item[_i].className.includes('active'))
                        _item[_i].className = _item[_i].className + ' active';
                }
                else _item[_i].className = _item[_i].className.replace('active','');
            }
            ser_current_treatment_plant_id = Number(id);
            $("#ImgTreatmentPlan").empty();
            Tab_List_Customer_Service();
            Tab_List_Load_Folder();
            Treatment_Plan_LoadInfo(ser_MainCustomerID, ser_current_treatment_plant_id);
        }
    });
    $("#tab_list_treatment_plant").on('click', '.Compare_treatment_plan', function (event) {
        Treatment_Plan_Compare_Event();
    });

    $(".imgExecute_Treatment_Plan").click(function (event) {
        event.preventDefault();
        event.stopPropagation();
        let pos_top = $(this).parent().position().top - $(this).height();
        let pos_left = $(this).position().left;
        if ($(window).width() < 625) {
            pos_left -= 200;
        }
        let _key = Number($(this).attr("data-id"));
        let currentstatus = ser_current_treatment_plant_data[_key].ExchangeStatus;
        if (currentstatus != undefined) {
            $('.button_treatment_plan_status_check').hide();
            let _checkstatus = 'button_treatment_plan_status_' + currentstatus;
            if ($('#' + _checkstatus).length) {
                $('#' + _checkstatus).show();
            }
        }

        $("#popupExecute_Treatment_Plan").css({
            "display": "block"
            , "left": pos_left
            , "top": pos_top+50
        }).animate({
            left: pos_left
            , top: pos_top + 60
            , display: "block"
        }, 200);
        $('#popupExecute_Treatment_Plan').dropdown('clear');
        event.stopPropagation();
    });
}