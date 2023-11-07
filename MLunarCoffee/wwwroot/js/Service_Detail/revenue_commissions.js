//#region // Revenue doctor
var changingflag_Revenue = 1;

function Add_new_row_revenue_doctor() {
    let element = {};
    let id = (new Date()).getTime();
    element.id = 0;
    element.doctorid = 0;
    element.amount_treat = 0;
    element.percent_treat = 0;
    data_revenue_doctor[id] = element;
    Render_RevenueDoctor_Add(id, element, 'dtTableRevenueDoctorBody');
    return id;
}
async function Render_RevenueDoctor_Add(key, value, id) {
    return new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<td class="d-none">' + key + '</td>'
                    + '<td class="vt-number-order"></td>'
                    + '<td>' + AddCell_Revenue_Doctor(key) + '</td>'
                    + '<td>' + AddCell_Revenue_Amount(key, value.amount_treat) + '</td>'
                    + '<td>' + AddCell_Revenue_Percent(key, value.percent_treat) + '</td>'
                    + '<td style="width: 50px;">'
                    + '<button class="buttonGrid"><i class="buttonDeleteClass vtt-icon vttech-icon-delete"></i></button>'
                    + '</td>'
                tr = '<tr class="rowRevenueDoctor vt-number"  id=rowRevenueDoctor_' + key + '>' + tr + '</tr>';
                myNode.insertAdjacentHTML('beforeend', tr);
            }
            $('.revenue_amount').divide();
            Fill_data_revenue_doctor_To_Design(data_revenue_doctor);
            Event_Element_Revenue_Doctor();
        }, 10);
    })
}
function AddCell_Revenue_Amount(randomNumber, number) {
    let resulf = '<input class="revenue_amount form-control" id="revenue_amount_' + randomNumber + '"  value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_Revenue_Percent(randomNumber, number) {
    let resulf = '<input class="revenue_percent form-control" id="revenue_percent_' + randomNumber + '" maxlength="3" value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_Revenue_Doctor(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown doctorRevenue form-control" title="' + randomNumber + '" id="doctorRevenue_' + randomNumber
        + '"><input type="hidden"/><i class="dropdown icon"></i>' +
        '<input id="revenueDoctorSearch_' + randomNumber + '" class="search" autocomplete="off" tabindex="0" /><div class="default text">Doctor</div><div id="cbbrevenueDoctor_' + randomNumber + '" class="menu" tabindex="-1">';

    for (i = 0; i < data_doctor.length; i++) {
        resulf = resulf + '<div class="item" data-value=' + data_doctor[i].ID + '>' + data_doctor[i].Name + '</div>'
    }
    resulf = resulf + '</div>';
    return resulf;
}

function Fill_data_revenue_doctor_To_Design(data) {
    for ([key, value] of Object.entries(data)) {
        if (value.doctorid != '') {
            $('#doctorRevenue_' + key).dropdown('refresh')
            $('#doctorRevenue_' + key).dropdown('set selected', value.doctorid);
        }
        $('#revenue_amount_' + key).val(value.amount_treat);
        $('#revenue_percent_' + key).val(value.percent_treat);
    }
}
function Event_Element_Revenue_Doctor() {
    //$(".doctorRevenue").dropdown({
    //    allowCategorySelection: true,
    //    forceSelection: false
    //});
    $('#dtTableRevenueDoctor .ui.dropdown').dropdown({
        onShow: function () {
            let ui_drop = $(this);
            Dropdown_Set_Position(ui_drop);
        },
        onHide: function () {
            let ui_drop = $(this);
            Dropdown_Remove_Position(ui_drop);
        },
        allowCategorySelection: true,
        forceSelection: false
    });
    $(".doctorRevenue").change(function () {
        let id = this.id.replace("doctorRevenue_", "")
        let _v = $('#' + this.id).dropdown('get value');
        data_revenue_doctor[id].doctorid = _v;
        changingflag_Revenue = 1
    });
    $(".revenue_percent").change(function () {
        if (changingflag_Revenue == 1) {
            changingflag_Revenue = 0;
            let id = this.id.replace("revenue_percent_", "")
            $("#revenue_amount_" + id).val("0");
            data_revenue_doctor[id].amount_treat = 0;
            data_revenue_doctor[id].percent_treat = Number($(this).val());
            changingflag_Revenue = 1;
        }
    });
    $(".revenue_amount").change(function () {
        if (changingflag_Revenue == 1) {
            changingflag_Revenue = 0;
            let id = this.id.replace("revenue_amount_", "")
            $("#revenue_percent_" + id).val("0");
            data_revenue_doctor[id].percent_treat = 0;
            data_revenue_doctor[id].amount_treat = Number($(this).val());
            changingflag_Revenue = 1;
        }
    });

    //$('.doctorRevenue').keyup(function (event) {
    //    let idcom = this.id
    //    let id = idcom.replace("doctorRevenue_", "")
    //    onKeyupRevenueDoctor(id);
    //});

    $('#dtTableRevenueDoctor tbody').on('click', '.buttonDeleteClass', function (event) {
        let timespan = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        delete data_revenue_doctor[timespan];
        $('#rowRevenueDoctor_' + timespan).remove();
        event.stopPropagation();
    });
}
function Checking_Validate_Revenue_Doctor() {
    let isprice_ = 0;
    let ismatch = 0;
    let branchcheck = '';
    let minpricecheck = 0;
    for ([key, value] of Object.entries(data_revenue_doctor)) {

        if (value.doctorid == "") {
            $('#doctorRevenue_' + key).css('background-color', 'rgb(255 216 216)'); isprice_ = 1;
        }
        else {
            $('#doctorRevenue_' + key).css('background-color', 'white');
        }

        let AmountService = Number($("#txtAmountServiceDetail").val());

        if (!$.isNumeric(value.amount_treat) || Number(value.amount_treat) < 0 || Number(value.amount_treat) > AmountService
            || (Number(value.amount_treat) == 0 && Number(value.percent_treat) == 0)) {
            $('#revenue_amount_' + key).css('background-color', 'rgb(255 216 216)');
            if (Number(value.amount_treat) > AmountService) {
                minpricecheck = 1;
            }
            isprice_ = 1;
        }
        else {
            $('#revenue_amount_' + key).css('background-color', 'white');
        };

        if (!$.isNumeric(value.percent_treat) || Number(value.percent_treat) < 0 || Number(value.percent_treat) > 100
            || (Number(value.amount_treat) == 0 && Number(value.percent_treat) == 0)) {
            $('#revenue_percent_' + key).css('background-color', 'rgb(255 216 216)');
            isprice_ = 1;
        }
        else $('#revenue_percent_' + key).css('background-color', 'white');

        if (branchcheck.includes('[' + value.doctorid + ']')) {
            ismatch = 1;
            $('#rowRevenueDoctor_' + key).css('background-color', 'rgb(255 216 216)');
        }
        else {
            $('#rowRevenueDoctor_' + key).css('background-color', 'white');
        }

        branchcheck = branchcheck + '[' + value.doctorid + ']'
    }

    if (isprice_ == 1 || ismatch == 1) $('#textShowMessage').html('Kiểm tra dữ liệu hoa hồng bác sĩ');
    if (minpricecheck == 1) $('#textShowMessage').html('Hoa hồng bác sĩ không nhỏ hơn 0 và lớn hơn giá dịch vụ');
}

//function onKeyupRevenueDoctor(id) {
//    let search = xoa_dau($('#revenueDoctorSearch_' + id).val().toLowerCase());
//    let data = data_doctor.filter(word => {
//        return ((xoa_dau(word["Name"]).toLowerCase()).includes(search));
//    });
//    if (data != undefined && data != null && data.length != 0)
//        LoadCombo(data, "cbbrevenueDoctor_" + id);
//}
//#endregion

//#region // Revenue consult
var changingflag_Revenue_Consult = 1;

function Add_new_row_revenue_consult() {
    let element = {};
    let id = (new Date()).getTime();
    element.id = 0;
    element.consultid = 0;
    element.amount_consult = 0;
    element.percent_consult = 0;
    data_revenue_consult[id] = element;
    Render_RevenueConsult_Add(id, element, 'dtTableRevenueConsultBody');
    return id;
}

async function Render_RevenueConsult_Add(key, value, id) {
    return new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<td class="d-none">' + key + '</td>'
                    + '<td class="vt-number-order"></td>'
                    + '<td>' + AddCell_revenue_consult(key) + '</td>'
                    + '<td>' + AddCell_revenue_consult_amount(key, value.amount_consult) + '</td>'
                    + '<td>' + AddCell_revenue_consult_percent(key, value.percent_consult) + '</td>'
                    + '<td style="width: 50px;">'
                    + '<button class="buttonGrid"><i class="buttonDeleteClass vtt-icon vttech-icon-delete"></i></button>'
                    + '</td>'
                tr = '<tr class="rowRevenueconsult vt-number"  id=rowRevenueconsult_' + key + '>' + tr + '</tr>';
                myNode.insertAdjacentHTML('beforeend', tr);
            }
            $('.revenue_consult_amount').divide();
            Fill_data_revenue_consult_To_Design(data_revenue_consult);
            Event_Element_Revenue_Consult();
        }, 10);
    })
}
function AddCell_revenue_consult_amount(randomNumber, number) {
    let resulf = '<input class="revenue_consult_amount form-control" id="revenue_consult_amount_' + randomNumber + '"  value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_revenue_consult_percent(randomNumber, number) {
    let resulf = '<input class="revenue_consult_percent form-control" id="revenue_consult_percent_' + randomNumber + '" maxlength="3" value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_revenue_consult(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown ConsultRevenue form-control" title="' + randomNumber + '" id="ConsultRevenue_' + randomNumber
        + '"><input type="hidden"/><i class="dropdown icon"></i>' +
        '<input id="revenueconsultSearch_' + randomNumber + '" class="search" autocomplete="off" tabindex="0" /><div class="default text">Employee</div><div id="cbbrevenueconsult_' + randomNumber + '" class="menu" tabindex="-1">';

    for (i = 0; i < data_employee.length; i++) {
        resulf = resulf + '<div class="item" data-value=' + data_employee[i].ID + '>' + data_employee[i].Name + '</div>'
    }
    resulf = resulf + '</div>';
    return resulf;
}

function Fill_data_revenue_consult_To_Design(data) {
    for ([key, value] of Object.entries(data)) {
        if (value.consultid != '') {
            $('#ConsultRevenue_' + key).dropdown('refresh')
            $('#ConsultRevenue_' + key).dropdown('set selected', value.consultid);
        }
        $('#revenue_consult_amount_' + key).val(value.amount_consult);
        $('#revenue_consult_percent_' + key).val(value.percent_consult);
    }
}
function Event_Element_Revenue_Consult() {
    //$(".ConsultRevenue").dropdown({
    //    allowCategorySelection: true,
    //    forceSelection: false
    //});
    $('#dtTableRevenueConsult .ui.dropdown').dropdown({
        onShow: function () {
            let ui_drop = $(this);
            Dropdown_Set_Position(ui_drop);
        },
        onHide: function () {
            let ui_drop = $(this);
            Dropdown_Remove_Position(ui_drop);
        },
        allowCategorySelection: true,
        forceSelection: false
    });
    $(".ConsultRevenue").change(function () {
        let id = this.id.replace("ConsultRevenue_", "")
        let _v = $('#' + this.id).dropdown('get value');
        data_revenue_consult[id].consultid = _v;
        changingflag_Revenue_Consult = 1
    });
    $(".revenue_consult_percent").change(function () {
        if (changingflag_Revenue_Consult == 1) {
            changingflag_Revenue_Consult = 0;
            let id = this.id.replace("revenue_consult_percent_", "")
            $("#revenue_consult_amount_" + id).val("0");
            data_revenue_consult[id].amount_consult = 0;
            data_revenue_consult[id].percent_consult = Number($(this).val());
            changingflag_Revenue_Consult = 1;
        }
    });
    $(".revenue_consult_amount").change(function () {
        if (changingflag_Revenue_Consult == 1) {
            changingflag_Revenue_Consult = 0;
            let id = this.id.replace("revenue_consult_amount_", "")
            $("#revenue_consult_percent_" + id).val("0");
            data_revenue_consult[id].percent_consult = 0;
            data_revenue_consult[id].amount_consult = Number($(this).val());
            changingflag_Revenue_Consult = 1;
        }
    });

    //$('.ConsultRevenue').keyup(function (event) {
    //    let idcom = this.id
    //    let id = idcom.replace("ConsultRevenue_", "")
    //    onKeyupRevenueconsult(id);
    //});

    $('#dtTableRevenueConsult tbody').on('click', '.buttonDeleteClass', function (event) {
        let timespan = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        delete data_revenue_consult[timespan];
        $('#rowRevenueconsult_' + timespan).remove();
        event.stopPropagation();
    });
}
function Checking_Validate_Revenue_consult() {
    let isprice_ = 0;
    let ismatch = 0;
    let branchcheck = '';
    let minpricecheck = 0;
    for ([key, value] of Object.entries(data_revenue_consult)) {

        if (value.consultid == "") {
            $('#ConsultRevenue_' + key).css('background-color', 'rgb(255 216 216)'); isprice_ = 1;
        }
        else {
            $('#ConsultRevenue_' + key).css('background-color', 'white');
        }

        let AmountService = Number($("#txtAmountServiceDetail").val());

        if (!$.isNumeric(value.amount_consult) || Number(value.amount_consult) < 0 || Number(value.amount_consult) > AmountService
            || (Number(value.amount_consult) == 0 && Number(value.percent_consult) == 0)) {
            $('#revenue_consult_amount_' + key).css('background-color', 'rgb(255 216 216)');
            if (Number(value.amount_consult) > AmountService) {
                minpricecheck = 1;
            }
            isprice_ = 1;
        }
        else {
            $('#revenue_consult_amount_' + key).css('background-color', 'white');
        };

        if (!$.isNumeric(value.percent_consult) || Number(value.percent_consult) < 0 || Number(value.percent_consult) > 100
            || (Number(value.amount_consult) == 0 && Number(value.percent_consult) == 0)) {
            $('#revenue_consult_percent_' + key).css('background-color', 'rgb(255 216 216)');
            isprice_ = 1;
        }
        else $('#revenue_consult_percent_' + key).css('background-color', 'white');

        if (branchcheck.includes('[' + value.consultid + ']')) {
            ismatch = 1;
            $('#rowRevenueconsult_' + key).css('background-color', 'rgb(255 216 216)');
        }
        else {
            $('#rowRevenueconsult_' + key).css('background-color', 'white');
        }

        branchcheck = branchcheck + '[' + value.consultid + ']'
    }

    if (isprice_ == 1 || ismatch == 1) $('#textShowMessage').html('Kiểm tra dữ liệu hoa hồng tư vấn');
    if (minpricecheck == 1) $('#textShowMessage').html('Hoa hồng tư vấn không nhỏ hơn 0 và lớn hơn giá dịch vụ');
}

//function onKeyupRevenueconsult(id) {
//    let search = xoa_dau($('#revenueconsultSearch_' + id).val().toLowerCase());
//    let data = data_employee.filter(word => {
//        return ((xoa_dau(word["Name"]).toLowerCase()).includes(search));
//    });
//    if (data != undefined && data != null && data.length != 0)
//        LoadCombo(data, "cbbrevenueconsult_" + id);
//}
//#endregion

//#region // Revenue tech
var changingflag_Revenue_tech = 1;

function Add_new_row_revenue_tech() {
    let element = {};
    let id = (new Date()).getTime();
    element.id = 0;
    element.techid = 0;
    element.amount_tech = 0;
    element.percent_tech = 0;
    data_revenue_tech[id] = element;
    Render_RevenueTech_Add(id, element, 'dtTableRevenueTechBody');
    return id;
}

async function Render_RevenueTech_Add(key, value, id) {
    return new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            let tr =
                '<td class="d-none">' + key + '</td>'
                + '<td class="vt-number-order"></td>'
                + '<td>' + AddCell_Revenue_tech(key) + '</td>'
                + '<td>' + AddCell_revenue_tech_amount(key, value.amount_tech) + '</td>'
                + '<td>' + AddCell_revenue_tech_percent(key, value.percent_tech) + '</td>'
                + '<td style="width: 50px;">'
                + '<button class="buttonGrid"><i class="buttonDeleteClass vtt-icon vttech-icon-delete"></i></button>'
                + '</td>'
            tr = '<tr class="rowRevenuetech vt-number"  id=rowRevenuetech_' + key + '>' + tr + '</tr>';
            myNode.insertAdjacentHTML('beforeend', tr);
        }
        $('.revenue_tech_amount').divide();
        Fill_data_revenue_tech_To_Design(data_revenue_tech);
        Event_Element_Revenue_Tech();
    })
};
function AddCell_revenue_tech_amount(randomNumber, number) {
    let resulf = '<input class="revenue_tech_amount form-control" id="revenue_tech_amount_' + randomNumber + '"  value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_revenue_tech_percent(randomNumber, number) {
    let resulf = '<input class="revenue_tech_percent form-control" id="revenue_tech_percent_' + randomNumber + '" maxlength="3" value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_Revenue_tech(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown TechRevenue form-control" title="' + randomNumber + '" id="TechRevenue_' + randomNumber
        + '"><input type="hidden"/><i class="dropdown icon"></i>' +
        '<input id="revenuetechSearch_' + randomNumber + '" class="search" autocomplete="off" tabindex="0" /><div class="default text">Employee</div><div id="cbbrevenuetech_' + randomNumber + '" class="menu" tabindex="-1">';

    for (i = 0; i < data_tech.length; i++) {
        resulf = resulf + '<div class="item" data-value=' + data_tech[i].ID + '>' + data_tech[i].Name + '</div>'
    }
    resulf = resulf + '</div>';
    return resulf;
}

function Fill_data_revenue_tech_To_Design(data) {
    for ([key, value] of Object.entries(data)) {
        if (value.techid != '') {
            $('#TechRevenue_' + key).dropdown('refresh')
            $('#TechRevenue_' + key).dropdown('set selected', value.techid);
        }
        $('#revenue_tech_amount_' + key).val(value.amount_tech);
        $('#revenue_tech_percent_' + key).val(value.percent_tech);
    }
}
function Event_Element_Revenue_Tech() {
    //$(".TechRevenue").dropdown({
    //    allowCategorySelection: true,
    //    forceSelection: false
    //});
    $('#dtTableRevenueTech .ui.dropdown').dropdown({
        onShow: function () {
            let ui_drop = $(this);
            Dropdown_Set_Position(ui_drop);
        },
        onHide: function () {
            let ui_drop = $(this);
            Dropdown_Remove_Position(ui_drop);
        },
        allowCategorySelection: true,
        forceSelection: false
    });
    $(".TechRevenue").change(function () {
        let id = this.id.replace("TechRevenue_", "")
        let _v = $('#' + this.id).dropdown('get value');
        data_revenue_tech[id].techid = _v;
        changingflag_Revenue_tech = 1
    });
    $(".revenue_tech_percent").change(function () {
        if (changingflag_Revenue_tech == 1) {
            changingflag_Revenue_tech = 0;
            let id = this.id.replace("revenue_tech_percent_", "")
            $("#revenue_tech_amount_" + id).val("0");
            data_revenue_tech[id].amount_tech = 0;
            data_revenue_tech[id].percent_tech = Number($(this).val());
            changingflag_Revenue_tech = 1;
        }
    });
    $(".revenue_tech_amount").change(function () {
        if (changingflag_Revenue_tech == 1) {
            changingflag_Revenue_tech = 0;
            let id = this.id.replace("revenue_tech_amount_", "")
            $("#revenue_tech_percent_" + id).val("0");
            data_revenue_tech[id].percent_tech = 0;
            data_revenue_tech[id].amount_tech = Number($(this).val());
            changingflag_Revenue_tech = 1;
        }
    });

    //$('.TechRevenue').keyup(function (event) {
    //    let idcom = this.id
    //    let id = idcom.replace("TechRevenue_", "")
    //    onKeyupRevenuetech(id);
    //});

    $('#dtTableRevenueTech tbody').on('click', '.buttonDeleteClass', function (event) {
        let timespan = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        delete data_revenue_tech[timespan];
        $('#rowRevenuetech_' + timespan).remove();
        event.stopPropagation();
    });
}
function Checking_Validate_Revenue_Tech() {
    let isprice_ = 0;
    let ismatch = 0;
    let branchcheck = '';
    let minpricecheck = 0;
    for ([key, value] of Object.entries(data_revenue_tech)) {

        if (value.techid == "") {
            $('#TechRevenue_' + key).css('background-color', 'rgb(255 216 216)'); isprice_ = 1;
        }
        else {
            $('#TechRevenue_' + key).css('background-color', 'white');
        }

        let AmountService = Number($("#txtAmountServiceDetail").val());

        if (!$.isNumeric(value.amount_tech) || Number(value.amount_tech) < 0 || Number(value.amount_tech) > AmountService
            || (Number(value.amount_tech) == 0 && Number(value.percent_tech) == 0)) {
            $('#revenue_tech_amount_' + key).css('background-color', 'rgb(255 216 216)');
            if (Number(value.amount_tech) > AmountService) {
                minpricecheck = 1;
            }
            isprice_ = 1;
        }
        else {
            $('#revenue_tech_amount_' + key).css('background-color', 'white');
        };

        if (!$.isNumeric(value.percent_tech) || Number(value.percent_tech) < 0 || Number(value.percent_tech) > 100
            || (Number(value.amount_tech) == 0 && Number(value.percent_tech) == 0)) {
            $('#revenue_tech_percent_' + key).css('background-color', 'rgb(255 216 216)');
            isprice_ = 1;
        }
        else $('#revenue_tech_percent_' + key).css('background-color', 'white');

        if (branchcheck.includes('[' + value.techid + ']')) {
            ismatch = 1;
            $('#rowRevenuetech_' + key).css('background-color', 'rgb(255 216 216)');
        }
        else {
            $('#rowRevenuetech_' + key).css('background-color', 'white');
        }

        branchcheck = branchcheck + '[' + value.techid + ']'
    }

    if (isprice_ == 1 || ismatch == 1) $('#textShowMessage').html('Kiểm tra dữ liệu hoa hồng điều trị');
    if (minpricecheck == 1) $('#textShowMessage').html('Hoa hồng điều trị không nhỏ hơn 0 và lớn hơn giá dịch vụ');
}

//function onKeyupRevenuetech(id) {
//    let search = xoa_dau($('#revenuetechSearch_' + id).val().toLowerCase());
//    let data = data_tech.filter(word => {
//        return ((xoa_dau(word["Name"]).toLowerCase()).includes(search));
//    });
//    if (data != undefined && data != null && data.length != 0)
//        LoadCombo(data, "cbbrevenuetech_" + id);
//}
//#endregion

//#region // Revenue assistant
var changingflag_Revenue_assistant = 1;

function Add_new_row_revenue_assistant() {
    let element = {};
    let id = (new Date()).getTime();
    element.id = 0;
    element.assistantid = 0;
    element.amount_assistant = 0;
    element.percent_assistant = 0;
    data_revenue_assistant[id] = element;
    Render_RevenueAssistant_Add(id, element, 'dtTableRevenueAssistantBody');
    return id;
}
async function Render_RevenueAssistant_Add(key, value, id) {
    return new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<td class="d-none">' + key + '</td>'
                    + '<td class="vt-number-order"></td>'
                    + '<td>' + AddCell_revenue_assistant(key) + '</td>'
                    + '<td>' + AddCell_revenue_assistant_amount(key, value.amount_assistant) + '</td>'
                    + '<td>' + AddCell_revenue_assistant_percent(key, value.percent_assistant) + '</td>'
                    + '<td style="width: 50px;">'
                    + '<button class="buttonGrid"><i class="buttonDeleteClass vtt-icon vttech-icon-delete"></i></button>'
                    + '</td>'
                tr = '<tr class="rowRevenueassistant vt-number"  id=rowRevenueAssistant_' + key + '>' + tr + '</tr>';
                document.getElementById(id).innerHTML = document.getElementById(id).innerHTML + tr;
            }
            $('.revenue_assistant_amount').divide();
            Fill_data_revenue_assistant_To_Design(data_revenue_assistant);
            Event_Element_Revenue_Assistant();
        }, 10);
    })
}
function AddCell_revenue_assistant_amount(randomNumber, number) {
    let resulf = '<input class="revenue_assistant_amount form-control" id="revenue_assistant_amount_' + randomNumber + '"  value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_revenue_assistant_percent(randomNumber, number) {
    let resulf = '<input class="revenue_assistant_percent form-control" id="revenue_assistant_percent_' + randomNumber + '" maxlength="3" value="' + number + '" />';
    resulf = resulf;
    return resulf;
}
function AddCell_revenue_assistant(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown assistantRevenue form-control" title="' + randomNumber + '" id="assistantRevenue_' + randomNumber
        + '"><input type="hidden"/><i class="dropdown icon"></i>' +
        '<input id="revenueassistantSearch_' + randomNumber + '" class="search" autocomplete="off" tabindex="0" /><div class="default text">Employee</div><div id="cbbrevenueassistant_' + randomNumber + '" class="menu" tabindex="-1">';

    for (i = 0; i < data_assist.length; i++) {
        resulf = resulf + '<div class="item" data-value=' + data_assist[i].ID + '>' + data_assist[i].Name + '</div>'
    }
    resulf = resulf + '</div>';
    return resulf;
}

function Fill_data_revenue_assistant_To_Design(data) {
    for ([key, value] of Object.entries(data)) {
        if (value.assistantid != '') {
            $('#assistantRevenue_' + key).dropdown('refresh')
            $('#assistantRevenue_' + key).dropdown('set selected', value.assistantid);
        }
        $('#revenue_assistant_amount_' + key).val(value.amount_assistant);
        $('#revenue_assistant_percent_' + key).val(value.percent_assistant);
    }
}
function Event_Element_Revenue_Assistant() {
    //$(".assistantRevenue").dropdown({
    //    allowCategorySelection: true,
    //    forceSelection: false
    //});
    $('#dtTableRevenueAssistant .ui.dropdown').dropdown({
        onShow: function () {
            let ui_drop = $(this);
            Dropdown_Set_Position(ui_drop);
        },
        onHide: function () {
            let ui_drop = $(this);
            Dropdown_Remove_Position(ui_drop);
        },
        allowCategorySelection: true,
        forceSelection: false
    });
    $(".assistantRevenue").change(function () {
        let id = this.id.replace("assistantRevenue_", "")
        let _v = $('#' + this.id).dropdown('get value');
        data_revenue_assistant[id].assistantid = _v;
        changingflag_Revenue_assistant = 1
    });
    $(".revenue_assistant_percent").change(function () {
        if (changingflag_Revenue_assistant == 1) {
            changingflag_Revenue_assistant = 0;
            let id = this.id.replace("revenue_assistant_percent_", "")
            $("#revenue_assistant_amount_" + id).val("0");
            data_revenue_assistant[id].amount_assistant = 0;
            data_revenue_assistant[id].percent_assistant = Number($(this).val());
            changingflag_Revenue_assistant = 1;
        }
    });
    $(".revenue_assistant_amount").change(function () {
        if (changingflag_Revenue_assistant == 1) {
            changingflag_Revenue_assistant = 0;
            let id = this.id.replace("revenue_assistant_amount_", "")
            $("#revenue_assistant_percent_" + id).val("0");
            data_revenue_assistant[id].percent_assistant = 0;
            data_revenue_assistant[id].amount_assistant = Number($(this).val());
            changingflag_Revenue_assistant = 1;
        }
    });

    //$('.assistantRevenue').keyup(function (event) {
    //    let idcom = this.id
    //    let id = idcom.replace("assistantRevenue_", "")
    //    onKeyupRevenueAssistant(id);
    //});

    $('#dtTableRevenueAssistant tbody').on('click', '.buttonDeleteClass', function (event) {
        let timespan = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        delete data_revenue_assistant[timespan];
        $('#rowRevenueAssistant_' + timespan).remove();
        event.stopPropagation();
    });
}
function Checking_Validate_Revenue_assistant() {
    let isprice_ = 0;
    let ismatch = 0;
    let branchcheck = '';
    let minpricecheck = 0;
    for ([key, value] of Object.entries(data_revenue_assistant)) {

        if (value.assistantid == "") {
            $('#assistantRevenue_' + key).css('background-color', 'rgb(255 216 216)'); isprice_ = 1;
        }
        else {
            $('#assistantRevenue_' + key).css('background-color', 'white');
        }

        let AmountService = Number($("#txtAmountServiceDetail").val());

        if (!$.isNumeric(value.amount_assistant) || Number(value.amount_assistant) < 0 || Number(value.amount_assistant) > AmountService
            || (Number(value.amount_assistant) == 0 && Number(value.percent_assistant) == 0)) {
            $('#revenue_assistant_amount_' + key).css('background-color', 'rgb(255 216 216)');
            if (Number(value.amount_assistant) > AmountService) {
                minpricecheck = 1;
            }
            isprice_ = 1;
        }
        else {
            $('#revenue_assistant_amount_' + key).css('background-color', 'white');
        };

        if (!$.isNumeric(value.percent_assistant) || Number(value.percent_assistant) < 0 || Number(value.percent_assistant) > 100
            || (Number(value.amount_assistant) == 0 && Number(value.percent_assistant) == 0)) {
            $('#revenue_assistant_percent_' + key).css('background-color', 'rgb(255 216 216)');
            isprice_ = 1;
        }
        else $('#revenue_assistant_percent_' + key).css('background-color', 'white');

        if (branchcheck.includes('[' + value.assistantid + ']')) {
            ismatch = 1;
            $('#rowRevenueAssistant_' + key).css('background-color', 'rgb(255 216 216)');
        }
        else {
            $('#rowRevenueAssistant_' + key).css('background-color', 'white');
        }

        branchcheck = branchcheck + '[' + value.assistantid + ']'
    }

    if (isprice_ == 1 || ismatch == 1) $('#textShowMessage').html('Kiểm tra dữ liệu hoa hồng hỗ trợ chuyên môn');
    if (minpricecheck == 1) $('#textShowMessage').html('Hoa hồng hỗ trợ chuyên môn không nhỏ hơn 0 và lớn hơn giá dịch vụ');
}

//function onKeyupRevenueAssistant(id) {
//    let search = xoa_dau($('#revenueassistantSearch_' + id).val().toLowerCase());
//    let data = data_assist.filter(word => {
//        return ((xoa_dau(word["Name"]).toLowerCase()).includes(search));
//    });
//    if (data != undefined && data != null && data.length != 0)
//        LoadCombo(data, "cbbrevenueassistant_" + id);
//}
//#endregion