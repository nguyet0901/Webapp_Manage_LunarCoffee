//#region // Render

function SRE_Con_AddRevenue() {
    let element = {};
    let id = (new Date()).getTime();
    element.EmployeeID = 0;
    element.ReAmount = 0;
    element.RePer = 0;
    Sre_DataMain_Con[id] = element;
    SRE_Con_RenderRow(id, element, 'sre_con_list');
    return id;
}

async function SRE_Con_RenderRow(key, value, id) {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<li id=sre_row_' + key + ' data-id="' + key + '" class=" row row-add border-dashed border-1 position-relative border-secondary border-radius-lg pb-3  border-0 d-flex justify-content-between ps-0 mb-1 border-radius-lg ">'
                    + (' <i data-id="' + key + '" class="opacity-2 position-absolute buttonDeleteClass cursor-pointer text-gradient text-danger vtt-icon vttech-icon-delete" style="width: 40px;right: 6px;top:0;"></i> ')
                    + '<div class="row col-12 w-100 mx-0">'
                    + '<div class="col-12 col-md-6 px-1 mt-2">'
                    + '<label>' + Outlang["Nhan_vien"] + '</label>'
                    + SRE_Con_RenderRow_AddEmployee(key)
                    + '</div>'
                    + '<div class="col-12 col-md-3 px-1 mt-2">'
                    + '<label>' + Outlang["Sys_tien_mat"] + '</label>'
                    + '<input id="sre_conamount_' + key + '" value="0" type="text" class="sre_conamount fw-bold form-control d-block">'
                    + '</div>'
                    + '<div class="col-12 col-md-3 px-1 mt-2">'
                    + '<label>' + Outlang["Phan_tram"] + '</label>'
                    + '<input id="sre_conpercent_' + key + '" value="0" type="text" class="sre_conpercent fw-bold form-control d-block">'
                    + '</div>'
                    + '</div>'
                    + '</li>';
                myNode.insertAdjacentHTML("beforeend", tr);
            }
            SRE_Con_FillData(key, value);
            SRE_Con_EventRow(key);
            resolve();
        }, 10)
    });
}

function SRE_Con_RenderRow_AddEmployee(randomNumber) {
    let resulf = '<div class="ui fluid search selection dropdown sre_con form-control" id="sre_con_' + randomNumber
        + '"><input type="hidden"/><input class="search" autocomplete="off" tabindex="0" /><div class="default text">eg .' + Outlang["Nhan_vien"] + '</div>'
        + '<div id="cbbsre_con_' + randomNumber + '" class="menu" tabindex="-1">';
    resulf = resulf + '</div></div>';
    return resulf;
}

async function SRE_Con_FillData(key, value) {
    return new Promise((resolve, reject) => {
        Load_Combo(Object.values(Sre_DataEmployee), "cbbsre_con_" + key, true);
        $("#sre_con_" + key).dropdown("refresh");
        $("#sre_con_" + key).dropdown("set selected", value.EmployeeID);
        $("#sre_conamount_" + key).val(value.ReAmount);
        $("#sre_conpercent_" + key).val(value.RePer);
        resolve();
    });
};

function SRE_Con_EventRow(key) {

    $("#sre_con_list #sre_conamount_" + key).divide();
    $("#sre_con_list #sre_conpercent_" + key).divide();
    $('#sre_con_list #sre_row_' + key + '.ui.dropdown').dropdown({
        allowCategorySelection: true,
        forceSelection: false,
        transition: "fade up"
    });

    $('#sre_con_list #sre_row_' + key + ' .sre_con').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        let idcom = this.id
        let id = idcom.replace("sre_con_", "")
        Sre_DataMain_Con[id].EmployeeID = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
    });

    $('#sre_con_list #sre_row_' + key + ' .sre_conamount').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        Sre_FlagChange = 0;
        let id = this.id.replace("sre_conamount_", "");
        let value = Number($(this).val());
        if (isNaN(value) || value < 0) {
            $(this).val(0);
            value = 0;
        }
        $("#sre_conpercent_" + key).val(0);
        Sre_DataMain_Con[id].ReAmount = value;
        Sre_DataMain_Con[id].RePer = 0;
        Sre_FlagChange = 1;
    });

    $('#sre_con_list #sre_row_' + key + ' .sre_conpercent').unbind('change').change(function () {
        if (Sre_FlagChange == 0) return;
        Sre_FlagChange = 0;
        let id = this.id.replace("sre_conpercent_", "");
        let value = Number($(this).val());
        if (isNaN(value) || value < 0 || value > 100) {
            $(this).val(0);
            value = 0;
        }
        $("#sre_conamount_" + key).val(0);
        Sre_DataMain_Con[id].ReAmount = 0;
        Sre_DataMain_Con[id].RePer = value;
        Sre_FlagChange = 1;
    });

    $('#sre_con_list #sre_row_' + key + ' .buttonDeleteClass').on('click', function (event) {
        let timespan = $(this).attr("data-id");
        delete Sre_DataMain_Con[timespan];
        $('#sre_row_' + timespan).remove();
        event.stopPropagation();
    });
}

//#endregion