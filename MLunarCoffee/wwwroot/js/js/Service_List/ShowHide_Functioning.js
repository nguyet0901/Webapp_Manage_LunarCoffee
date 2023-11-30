//function Treatment_Plant_Prepare_ShowAll() {
//    if ($('#treatment_plan_div_content_view').length && !$('#treatment_plan_div_content_view').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_view').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_tab').length && !$('#treatment_plan_div_tab').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_tab').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_service_list_button').length && !$('#cus_service_list_button').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#cus_service_list_button').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_using_treatment_plan').length && !$('#cus_using_treatment_plan').hasClass('cus_using_treatment_plan_expand')) {
//        $('#cus_using_treatment_plan').addClass('cus_using_treatment_plan_expand')
//    }
//    if ($('#Compare_Service_TreatmentPlan').length) $('#Compare_Service_TreatmentPlan').hide();
//    $('#Show_Service_All_Plan').attr("data-type", "view");
//    $("#Show_Service_All_Plan_img").attr("src", "/assests/img/ButtonImg/left_arrow.png");   
//}
//function Treatment_Plant_Done_ShowAll() {
//    if ($('#treatment_plan_div_content_view').length && $('#treatment_plan_div_content_view').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_view').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_tab').length && $('#treatment_plan_div_tab').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_tab').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_service_list_button').length && $('#cus_service_list_button').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#cus_service_list_button').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_using_treatment_plan').length && $('#cus_using_treatment_plan').hasClass('cus_using_treatment_plan_expand')) {
//        $('#cus_using_treatment_plan').removeClass('cus_using_treatment_plan_expand')
//    }
//    if ($('#Compare_Service_TreatmentPlan').length) $('#Compare_Service_TreatmentPlan').show();
//    $('#Show_Service_All_Plan').attr("data-type", "noview");
//    $("#Show_Service_All_Plan_img").attr("src", "/assests/img/ButtonImg/listall.png");
//}
//function Treatment_Plant_Prepare_Compare() {
//    $('#Compare_Service_TreatmentPlan').attr("data-type", "compare");
//    $("#Compare_Service_TreatmentPlan_img").attr("src", "/assests/img/ButtonImg/left_arrow.png");
    
//    $(".Compare_treatment_plan").prop("checked", false);
//    $('.Compare_treatment_plan_div').show();
//    if ($('#Show_Service_All_Plan').length) $('#Show_Service_All_Plan').hide();
//    if ($('#TabList_Service_Main_Content').length) $('#TabList_Service_Main_Content').hide();
//    if ($('#TabList_Compare_Treatment_Plan').length) {
//        $('#TabList_Compare_Treatment_Plan').empty()
//        $('#TabList_Compare_Treatment_Plan').show();
//    }
//    if ($('#buttonadd_treatment_plain').length) $('#buttonadd_treatment_plain').hide();
//    if ($('#cus_using_patient_record').length) $('#cus_using_patient_record').hide();
//    $('.imgExecute_Treatment_Plan').hide();
//    $('.guide_header_tablist_service').hide();
//    let _item = document.getElementsByClassName('treatment_plan_item');
//    for (_i = 0; _i < _item.length; _i++) {
//        _item[_i].className = 'treatment_plan_item';
//    }
//}
//function Treatment_Plant_Done_Compare() {
//    $('#Compare_Service_TreatmentPlan').attr("data-type", "nocompare");
//    $("#Compare_Service_TreatmentPlan_img").attr("src", "/assests/img/ButtonImg/compare.png");
//    $('.Compare_treatment_plan_div').hide();
//    $('.guide_header_tablist_service').show();
    
//    if ($('#Show_Service_All_Plan').length) $('#Show_Service_All_Plan').show();

//    if ($('#TabList_Service_Main_Content').length) $('#TabList_Service_Main_Content').show();
//    if ($('#TabList_Compare_Treatment_Plan').length) {
//        $('#TabList_Compare_Treatment_Plan').empty()
//        $('#TabList_Compare_Treatment_Plan').hide();
//    }

//    if ($('#buttonadd_treatment_plain').length) $('#buttonadd_treatment_plain').show();
//    if ($('#cus_using_patient_record').length) $('#cus_using_patient_record').show();
//    Tab_List_Customer_Treatment_Plan_Trigger_First();
    

    
//}
//function Patient_Record_Prepare_Functioning() {
//    if ($('#treatment_plan_div_content_view').length && !$('#treatment_plan_div_content_view').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_view').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_content_compare').length && !$('#treatment_plan_div_content_compare').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_compare').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_tab').length && !$('#treatment_plan_div_tab').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_tab').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_service_list_button').length && !$('#cus_service_list_button').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#cus_service_list_button').addClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_using_treatment_plan').length && !$('#cus_using_treatment_plan').hasClass('cus_using_treatment_plan_expand')) {
//        $('#cus_using_treatment_plan').addClass('cus_using_treatment_plan_expand')
//    }
//    if ($('#Compare_Service_TreatmentPlan').length) $('#Compare_Service_TreatmentPlan').hide();
//    if ($('#Show_Service_All_Plan').length) $('#Show_Service_All_Plan').hide();
//    if ($('#patient_record_list').length) $('#patient_record_list').hide();
//    $('#Show_Service_All_Record').attr("data-type", "view");
//    $("#Show_Service_All_Record_img").attr("src", "/assests/img/ButtonImg/left_arrow.png");

//}
//function Patient_Record_Done_Functioning() {
//    if ($('#treatment_plan_div_content_view').length && $('#treatment_plan_div_content_view').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_view').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_content_compare').length && $('#treatment_plan_div_content_compare').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_content_compare').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#treatment_plan_div_tab').length && $('#treatment_plan_div_tab').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#treatment_plan_div_tab').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_service_list_button').length && $('#cus_service_list_button').hasClass('cus_using_treatment_plan_hidden')) {
//        $('#cus_service_list_button').removeClass('cus_using_treatment_plan_hidden')
//    }
//    if ($('#cus_using_treatment_plan').length && $('#cus_using_treatment_plan').hasClass('cus_using_treatment_plan_expand')) {
//        $('#cus_using_treatment_plan').removeClass('cus_using_treatment_plan_expand')
//    }
//    if ($('#Compare_Service_TreatmentPlan').length) $('#Compare_Service_TreatmentPlan').show();
//    if ($('#Show_Service_All_Plan').length) $('#Show_Service_All_Plan').show();
//    if ($('#patient_record_list').length) $('#patient_record_list').show();
//    $('#Show_Service_All_Record').attr("data-type", "noview");
//    $("#Show_Service_All_Record_img").attr("src", "/assests/img/ButtonImg/listall.png");
//}