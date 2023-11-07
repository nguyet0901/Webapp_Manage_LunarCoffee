function AD_TimeLineClear () {
    if (DoctorSlide_AppDetail != undefined && DoctorSlide_AppDetail != null) {
        DoctorSlide_AppDetail.TimeSlider("remove_all");

    }
}
function AD_TimeLineAdd (timecell) {
    setTimeout(function (e) {
    if (DoctorSlide_AppDetail != undefined && DoctorSlide_AppDetail != null) {
            DoctorSlide_AppDetail.TimeSlider("add", timecell);
        }
    }, 500);
}
function AD_TimeLineEdit (timecell) {
    if (DoctorSlide_AppDetail != undefined && DoctorSlide_AppDetail != null) {
        DoctorSlide_AppDetail.TimeSlider("edit", timecell);
    }
}
async function AD_TimeLineIni () {
    new Promise((resolve, reject) => {
        setTimeout(function (e) {
            if ($('#app_detail_ex_timeline').length) {
                var date = new Date();
                let date_begin = ConverDateTime_Only_Date_From_DateTime(date);
                var current_time = ConvertDateTime_To_Timespan_TimeZone(date_begin) + 3600 * 5.5 * 1000;
                DoctorSlide_AppDetail = $('#doctor_sheduler_in_one_day').TimeSlider({
                    show_ms: false,
                    timecell_enable_move: false,
                    ruler_enable_move: false,
                    timecell_enable_resize: false,
                    update_interval: 1000000,
                    hours_per_frame: 24,
                    graduation_step: 8,
                    distance_between_gtitle: 60,
                    start_timestamp: current_time,
                    init_cells: null
                });
                $("#doctor_sheduler_in_one_day .ruler").height(65);
                AD_TimeLineHover();
            }
        }, 400);
    })
}
function AD_TimeLineHover () {
    $("#doctor_sheduler_in_one_day").on("mouseenter", '.timecell-event', function () {
        $(".time-slider .prompts").addClass("hover");
    })
    $("#doctor_sheduler_in_one_day").on("mouseleave", '.timecell-event', function () {
        $(".time-slider .prompts").removeClass("hover");
    })
}
function AD_TimeLineLoad () {
    $("#button_save_appointment").prop("disabled", false);
    $("#textShowMessage").html('');
    let date = $('#App_Detail_Date_From').val() + ' ' + $('#App_Detail_DateFrom_Time').val();
    let date_now = GetDateTime_Now_Only_Date();
    let distance_day = Math.ceil((ConvertString_DMY_To_DateTime(date) - date_now) / (1000 * 60 * 60 * 24));
    let branchID = Number($('#App_Detail_Branch_ID').dropdown('get value')) ? Number($('#App_Detail_Branch_ID').dropdown('get value')) : 0;
    let doctorID = Number($('#App_Detail_Doctor_ID').dropdown('get value')) ? Number($('#App_Detail_Doctor_ID').dropdown('get value')) : 0;
    if (branchID != 0 && doctorID != 0 && $('#app_detail_ex_timeline').length != 0) {
        AD_TimeLineClear();
        AjaxLoad(url = "/Appointment/AppointmentDetail/?handler=LoaData_Extension_Doctor"
            , data = {
                'BranchID': branchID, 'EmpID': doctorID
                , 'dateFrom': ConvertString_DMY_To_StringYMD(date)
                , 'dateTo': ConvertString_DMY_To_StringYMD(date)
                , 'AppID': ser_CurrentAppointmentID
            }
            , async = true
            , error = function () {notiError_SW();}
            , success = function (result) {
                if (result != "0") {
                    $('#app_detail_ex_timeline').show();
                    let _dataex = JSON.parse(result);
                    let _data_scheduler = _dataex.Sheduler;
                    let _data_work = _dataex.Work;
                    for (_ii = 0;_ii < _data_scheduler.length;_ii++) {
                        let e = {};
                        e.start = ConvertDateTime_To_Timespan_TimeZone((new Date(_data_scheduler[_ii].Date_From)).addDays(-distance_day));
                        e.stop = ConvertDateTime_To_Timespan_TimeZone((new Date(_data_scheduler[_ii].Date_To)).addDays(-distance_day));
                        let _b = {};
                        if (Number(_data_scheduler[_ii].IsCurrent) == 0) {
                            e._id = "Doctor_Scheduler_Ext_" + _data_scheduler[_ii].ID;
                            _b.background = "#5eb9fd";
                            e.style = _b;
                        }
                        else {
                            e._id = "Doctor_Scheduler_Ext_Current";
                            _b.animation = 'pulse 2s infinite';
                            e.style = _b;
                        }
                        AD_TimeLineAdd(e)

                    }
                    AD_TimeLineCheck(_data_scheduler, formatDMYHHMM_To_Date(date));

                    let is_out_time_line = 0, is_none_time_line = 0;
                    for (_ii = 0;_ii < _data_work.length;_ii++) {
                        if (Number(_data_work[_ii].Off) == 0) {
                            let e = {};
                            let f_work = ConvertDateTime_To_Timespan_TimeZone((new Date(_data_work[_ii].Date_From)).addDays(-distance_day));
                            let t_work = ConvertDateTime_To_Timespan_TimeZone((new Date(_data_work[_ii].Date_To)).addDays(-distance_day));
                            let d_app = ConvertDateTime_To_Timespan_TimeZone(formatDMYHHMM_To_Date(date).addDays(-distance_day));
                            e._id = "Doctor_Work_Ext_" + _ii;
                            e.start = f_work;
                            e.stop = t_work;
                            let _b = {};
                            _b.background = "#29b87e38";
                            e.style = _b;
                            AD_TimeLineAdd(e);
                            if (d_app >= f_work && d_app <= t_work) {
                                is_out_time_line = 2;
                            }
                            else {
                                if (is_out_time_line != 2) {
                                    is_out_time_line = 1;
                                }
                            }
                        } else {
                            is_none_time_line = 1;
                        }
                    }
                    if (is_out_time_line == 1) {
                        $("#textShowMessage").html(Outlang["Lich_hen_nam_trong_lich_lam_viec_cua_bac_si"] ?? 'Lịch hẹn phải nằm trong lịch làm việc của bác sĩ');
                        $("#button_save_appointment").prop("disabled", true);
                    }
                    if (is_none_time_line == 1) {
                        $("#textShowMessage").html(Outlang["Bac_si_khong_co_lich_lam_viec"] ?? 'Bác sĩ không có lịch làm việc');
                        $("#button_save_appointment").prop("disabled", true);
                    }

                    AD_TimeLineDetect();
                    let _timecelt = document.getElementsByClassName('timecell-event');
                    if (_timecelt != undefined) {
                        for (i = 0;i < _timecelt.length;i++) {
                            if (_timecelt[i].id.includes('tDoctor_Work_Ext_') || _timecelt[i].id.includes('tDoctor_Scheduler_Ext_Current_'))
                                $('#' + _timecelt[i].id).css('z-index', -1);
                        }
                    }
                }
            }
            , sender = null
            , before = function () {
                $('#appdoctor_ext_waiting').show();
            },
            complete = function (e) {
                $('#appdoctor_ext_waiting').hide();
            }
        );

    }
    else {
        AD_TimeLineClear();
        AD_TimeLineDetect();
    }

}
function AD_TimeLineCheck (dataSchedule, Time) {
    $("#textShowMessageDuplicate").hide();
    if (dataSchedule != undefined && dataSchedule.length != 0) {
        let TimeSelected = (new Date(Time)).getTime();
        let CountDuplicate = 0;
        for (let i = 0;i < dataSchedule.length;i++) {
            let item = dataSchedule[i];
            if (Number(item.ID) != ser_CurrentAppointmentID) {
                let Date_From = (new Date(item.Date_From)).getTime();
                let Date_To = (new Date(item.Date_To)).getTime();
                if (TimeSelected >= Date_From && TimeSelected <= Date_To) {
                    CountDuplicate = 1;
                    break;
                }
            }
        }
        if (CountDuplicate == 1) {
            $("#textShowMessageDuplicate").show();
        }
    }
}