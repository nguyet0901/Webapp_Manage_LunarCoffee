function Treatment_Plan_Compare_Service() {
    let _type = $('#Compare_Service_TreatmentPlan').attr('data-type');
    if (_type == "nocompare") {
        Treatment_Plant_Prepare_Compare();
        ser_Treatment_Plan_Comparing = 1;

    }
    else {
        Treatment_Plant_Done_Compare();
        ser_Treatment_Plan_Comparing = 0;

    }
}
function Treatment_Plan_Compare_Event() {
    let _item = document.getElementsByClassName('Compare_treatment_plan');
    let _compare = [];
    let _compare_string = '';
    if (_item != undefined && _item.length != 0) {
        for (_i = 0; _i < _item.length; _i++) {
            _id = _item[_i].dataset.id;
            if (_item[_i].checked) {
                _e = {};
                _e.id = _id;
                _compare.push(_e);
                _compare_string = _compare_string + ',' + _id;
                if ($('#treatment_plan_' + _id).length && !$('#treatment_plan_' + _id).hasClass('recordcompareselected_plan')) {
                    $('#treatment_plan_' + _id).addClass('recordcompareselected_plan')
                }

            }
            else {
                if ($('#treatment_plan_' + _id).length && $('#treatment_plan_' + _id).hasClass('recordcompareselected_plan')) {
                    $('#treatment_plan_' + _id).removeClass('recordcompareselected_plan')
                }
            }
        }
    }
 
    if (_compare.length > 1) {
        Treatment_Plan_Compare_Executing(_compare, _compare_string);
    }
    else {
        Treatment_Plan_Compare_Executing_Clear();
    }
}

function Treatment_Plan_Compare_Executing(_compare, _compare_string) {
    AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=Load_Treatment_Plan_Compare"
        , data = {
            'CustomerID': ser_MainCustomerID,
            'treatment_plan': _compare_string }
        , async = true
        , error = function () { notiError_SW() }
        , success = function (result) {
            let data = JSON.parse(result);
            if (data != undefined) {
                Treatment_Plant_Compare_Render(_compare, data, "TabList_Compare_Treatment_Plan");
            }
        }
    );

}
function Treatment_Plan_Compare_Executing_Clear() {
    $('#TabList_Compare_Treatment_Plan').empty();
}
function Treatment_Plant_Compare_Render(_compare, data, id) {
    var myNode = document.getElementById(id);
    let stringContent = '';
    if (myNode != null) {
        myNode.innerHTML = '';
        for (k = 0; k < _compare.length; k++) {
            let _planid = _compare[k].id;
            let _plan = data.filter(word => Number(word.PlanID) == Number(_planid));
            if (_plan != undefined && _plan.length != 0) {
                let _planname = _plan[0].PlanName;
                let _total_price = formatNumber(_plan[0].TotalPrice);
                let _total_ini = formatNumber(_plan[0].TotalIni);
                let _total_dis = formatNumber(_plan[0].TotalDis);
                stringContent = stringContent +
                    '<div class="column" style="min-width:500px;">'
                    + '<div class="ui segments ">'
                    + '<div class="ui segment compare_treat_header">'
                    + '<h5 class="ui header" style="color: #0a406d;font-size: 14px;margin: auto;">' + _planname + '</h5>'
                    + '</div>'
                    + '<div class="ui segment compare_treatment_plan_column">'
                    + Treatment_Plant_Compare_Render_Each_Service(_plan)
                    + '</div>'
                    + '<div class="ui segment compare_treat_footer">'
                    + Treatment_Plant_Compare_Render_Footer(_total_ini, _total_dis, _total_price, _planid)
                    + '</div>'
                    + '</div>'
                    + '</div>';
            }

        }
    }
    document.getElementById(id).innerHTML = '<div class="ui equal width left aligned padded grid stackable"><div class="row compare_treatment_plan_content">' + stringContent + '</div></div>';
    Treatment_Plant_Compare_Choosing_Service_Event();
}
function Treatment_Plant_Compare_Render_Each_Service(__data) {
    let _re = ' <table class="ui selectable celled table accordion nosortwholetable"><thead class="hiddenCollapse"><tr role="row">'
        + '<th style="max-width:50px;min-width:50px;"></th>'
        + '<th>Dịch Vụ</th>'
        + '<th>Giảm</th>'
        + '<th style="max-width:150px;min-width:150px;">Thành Tiền</th>'
        + '</tr></thead><tbody>';
    let _tds = '';
    for (_k = 0; _k < __data.length; _k++) {

        _td = '<td>'
            + '<div class="ui checkbox" style="margin: 5px;transform: scale(1.23);">'
            + '<input type="checkbox" '
            + 'data-price="' + __data[_k].Price + '" '
            + 'data-priceini="' + __data[_k].PriceIni + '" '
            + 'data-tab="' + __data[_k].TabID + '" '
            + '  data-plan="' + __data[_k].PlanID + '" class="compare_choosing_service" checked>'
            + '<label class="coloring red"></label></div>'

            + '</td>'
            + '<td>'
            + '<span class="compare_treat_ser">' + __data[_k].ServiceName + '</span>'
            + '<span class="compare_treat_teeth">'
            //+ (
            //    (sys_dencos_Main == 1)
            //    ? (((Get_Session_Teeth_Names_ByToken(__data[_k].TeethChoosing, __data[_k].TeethType) != '') ? (' [ ' + Get_Session_Teeth_Names_ByToken(__data[_k].TeethChoosing, __data[_k].TeethType) + ' ] ') : ''))
            //    : (' [ ' + __data[_k].TimeToTreatment + ' times ] ')
            //)
            + '</span>'
            + '<span class="compare_treat_unitprice">'
            + __data[_k].Quantity + ' x ' + formatNumber(__data[_k].UnitPrice)
            + '</span>'
            + '</td>'
            + '<td>'
            + '<span class="compare_treat_ini">' + formatNumber(__data[_k].PriceIni) + '</span>'
            + '<span class="compare_treat_dis"> - ' + formatNumber(Number(__data[_k].PriceIni) - Number(__data[_k].Price)) + '</span>'
            + '</td>'
            + '<td>'
            + '<span class="compare_treat_final">' + formatNumber(__data[_k].Price) + '</span>'
            + '</td>'

        _tds = _tds + '<tr role="row">' + _td + '</tr>';
    }
    _re = _re + _tds + '</tbody></table>'
    return _re;
}
function Treatment_Plant_Compare_Render_Footer(ini, dis, final, planid) {
    return '<span id="compare_treat_total_ini_' + planid + '" class="compare_treat_total_ini">' + ini + '</span>'
        + '<span id="compare_treat_total_dis_' + planid + '" class="compare_treat_total_dis"> - ' + dis + '</span>'
        + '<div class="compare_total_line"></div>'
        + '<span id="compare_treat_total_final' + planid + '" class="compare_treat_total_final">' + final + '</span>'
        ;
}
function Treatment_Plant_Compare_Choosing_Service_Event() {
    $(".compare_choosing_service").on('click', function (event) {
        let _tab = Number($(this).attr("data-tab"));
        let _plan = Number($(this).attr("data-plan"));

        let _item = document.getElementsByClassName('compare_choosing_service');
        if (_item != undefined && _item.length != 0) {
            let total_price = 0;
            let total_price_ini = 0;
            for (_i = 0; _i < _item.length; _i++) {
                if (_item[_i].checked && Number(_item[_i].dataset.plan) == _plan) {
                    total_price = total_price + Number(_item[_i].dataset.price);
                    total_price_ini = total_price_ini + Number(_item[_i].dataset.priceini);
                }
            }
            $('#compare_treat_total_ini_' + _plan).html(formatNumber(total_price_ini));
            $('#compare_treat_total_final' + _plan).html(formatNumber(total_price));
            $('#compare_treat_total_dis_' + _plan).html(formatNumber(total_price_ini - total_price));
        }

    });
}