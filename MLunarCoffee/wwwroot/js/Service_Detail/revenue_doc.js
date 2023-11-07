//#region // Render

function SRE_Doc_AddRevenue() {
    let element = {};
    let id = (new Date()).getTime();
    element.EmployeeID = 0;
    element.ReAmount = 0;
    element.RePer = 0;
    Sre_DataMain_Doc[id] = element;
    SRE_Doc_RenderRow(id, element, 'sre_doc_list');
    return id;
}

async function SRE_Doc_RenderRow(key, value, id) {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<li id=sre_row_' + key + ' data-id="' + key + '" class=" row row-add border-dashed border-1 position-relative border-secondary border-radius-lg pb-3  border-0 d-flex justify-content-between ps-0 mb-1 border-radius-lg ">'
                    + (' <i data-id="' + key + '" class="opacity-2 position-absolute buttonDeleteClass cursor-pointer text-gradient text-danger vtt-icon vttech-icon-delete" style="width: 40px;right: 6px;top:0;"></i> ')
                    + '<div class="row col-12 w-100 mx-0">'
                    + '<div class="col-12 col-md-6 px-1 mt-2">'
                    + '<label>' + Outlang["Bac_si"] + '</label>'
                    + SRE_Doc_RenderRow_AddEmployee(key)
                    + '</div>'
                    + '<div class="col-12 col-md-3 px-1 mt-2">'
                    + '<label>' + Outlang["Sys_tien_mat"] + '</label>'
                    + '<input id="sre_doctoramount_' + key + '" value="0" type="text" class="sre_doctoramount fw-bold form-control d-block">'
                    + '</div>'
                    + '<div class="col-12 col-md-3 px-1 mt-2">'
                    + '<label>' + Outlang["Phan_tram"] + '</label>'
                    + '<input id="sre_doctorpercent_' + key + '" value="0" type="text" class="sre_doctorpercent fw-bold form-control d-block">'
                    + '</div>'
                    + '</div>'
                    + '</li>';
                myNode.insertAdjacentHTML("beforeend", tr);
            }
            SRE_Doc_FillData(key, value);
            SRE_Doc_EventRow(key);
            resolve();
        }, 10)
    });
}

function SRE_Doc_RenderRow_AddEmployee(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown sre_doctor form-control" id="sre_doctor_' + randomNumber
        + '"><input type="hidden"/><input class="search" autocomplete="off" tabindex="0" /><div class="default text">eg .' + Outlang["Bac_si"] + '</div>'
        + '<div id="cbbsre_doctor_' + randomNumber + '" class="menu" tabindex="-1">';
    resulf = resulf + '</div></div>';
    return resulf;
}

async function SRE_Doc_FillData(key, value) {
    return new Promise((resolve, reject) => {
        Load_Combo(Object.values(Sre_DataDoctor), "cbbsre_doctor_" + key, true);
        $("#sre_doctor_" + key).dropdown("refresh");
        $("#sre_doctor_" + key).dropdown("set selected", value.EmployeeID);
        $("#sre_doctoramount_" + key).val(value.ReAmount);
        $("#sre_doctorpercent_" + key).val(value.RePer);
        resolve();
    });
};

function SRE_Doc_EventRow(key) {

    $("#sre_doc_list #sre_doctoramount_" + key).divide();
    $("#sre_doc_list #sre_doctorpercent_" + key).divide();
    $('#sre_doc_list #sre_row_' + key + '.ui.dropdown').dropdown({
        allowCategorySelection: true,
        forceSelection: false,
        transition: "fade up"
    });

    $('#sre_doc_list #sre_row_' + key + ' .sre_doctor').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        let idcom = this.id
        let id = idcom.replace("sre_doctor_", "")
        Sre_DataMain_Doc[id].EmployeeID = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
    });

    $('#sre_doc_list #sre_row_' + key + ' .sre_doctoramount').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        Sre_FlagChange = 0;
        let id = this.id.replace("sre_doctoramount_", "");
        let value = Number($(this).val());
        if (isNaN(value) || value < 0) {
            $(this).val(0);
            value = 0;
        }
        $("#sre_doctorpercent_" + key).val(0);
        Sre_DataMain_Doc[id].ReAmount = value;
        Sre_DataMain_Doc[id].RePer = 0;
        Sre_FlagChange = 1;
    });

    $('#sre_doc_list #sre_row_' + key + ' .sre_doctorpercent').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        Sre_FlagChange = 0;
        let id = this.id.replace("sre_doctorpercent_", "");
        let value = Number($(this).val());
        if (isNaN(value) || value < 0 || value > 100) {
            $(this).val(0);
            value = 0;
        }
        $("#sre_doctoramount_" + key).val(0);
        Sre_DataMain_Doc[id].ReAmount = 0;
        Sre_DataMain_Doc[id].RePer = value;
        Sre_FlagChange = 1;
    });

    $('#sre_doc_list #sre_row_' + key + ' .buttonDeleteClass').on('click', function (event) {
        let timespan = $(this).attr("data-id");
        delete Sre_DataMain_Doc[timespan];
        $('#sre_row_' + timespan).remove();
        event.stopPropagation();
    });
}

//#endregion