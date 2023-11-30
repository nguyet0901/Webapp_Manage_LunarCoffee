//#region // Stage Service
function Add_new_row_stage_service() {
    let element = {};
    let id = (new Date()).getTime();
    element.id = 0;
    element.treatStageID = 0;
    element.percent = 0;
    element.percentcomplete = 0;
    data_stage_service[id] = element;
    Render_StageService_Add(id, element, 'dtTableStageServiceBody');
    return id;
}

function Event_Element_Stage_Service() {

    $(".StageService").bind('change').change(function () {
        let id = this.id.replace("StageService_", "");
        let treatStageID = Number($(this).dropdown('get value')) ? Number($(this).dropdown('get value')) : 0;
        data_stage_service[id].treatStageID = treatStageID;
    });

    $(".StagePercentService").bind('change').change(function () {
        let id = this.id.replace("StagePercentService_", "");
        $(this).attr("value", this.value);
        data_stage_service[id].percent = Number(this.value);
        StageService_Total_Percent();
    });

    $('#dtTableStageService tbody .buttonDeleteClass').bind('click').on('click', function (event) {
        let timespan = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        delete data_stage_service[timespan];
        $('#rowStage_' + timespan).remove();
        StageService_Total_Percent()
        event.stopPropagation();
    });
}

function StageService_Total_Percent() {
    let Total = 0;
    for ([key, value] of Object.entries(data_stage_service)) {
        Total += parseInt(value.percent);
        $("#TotalPercentStage").text(Total + '%');
    }
}

async function Render_StageService_Add(key, value, id) {
    return new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                let tr =
                    '<td class="d-none">' + key + '</td>'
                    + '<td class=" vt-number-order"></td>'
                    + '<td>' + AddCell_Stage(key) + '</td>'
                    + '<td>' + AddCell_Stage_Percent(key) + '</td>'
                    + '<td style="width: 50px;">'
                    + '<button class="buttonGrid"><i class="buttonDeleteClass vtt-icon vttech-icon-delete"></i></button>'
                    + '</td>'
                tr = '<tr class="rowStageService vt-number"  id=rowStage_' + key + '>' + tr + '</tr>';
                myNode.insertAdjacentHTML('beforeend', tr);
            }
            Fill_Data_Stage_Service(value, key);
            Event_Element_Stage_Service(key);
        }, 10);
    })
}

function AddCell_Stage(randomNumber) {
    //let resulf = '<input class="StageService form-control" id="StageService_' + randomNumber + '" type="text"/>';
    //resulf = resulf;
    //return resulf;
    let result = `
        <div class="ui fluid search selection dropdown StageService form-control" title="${randomNumber}" id="StageService_${randomNumber}">
            <input type="hidden"/>
            <i class="dropdown icon"></i>
            <input class="search" autocomplete="off" tabindex="0" />
            <div class="default text">${Outlang["Sys_buoc_dieu_tri"]}</div>
            <div id="cbbStageService_${randomNumber}" class="menu" tabindex="-1">
            </div>
        </div>
    `
    return result;
}
function AddCell_Stage_Percent(randomNumber) {
    let resulf = '<input class="StagePercentService form-control" id="StagePercentService_' + randomNumber + '" type="number"/>';
    resulf = resulf;
    return resulf;
}

function Fill_Data_Stage_Service(value, key) {
    Load_Combo(data_treat_stage, "cbbStageService_" + key, true);
    if (value) {
        $('#StageService_' + key).dropdown('clear');
        $('#StageService_' + key).dropdown("refresh");
        $('#StageService_' + key).dropdown("set selected", value.treatStageID);
        $('#StagePercentService_' + key).val(value.percent);
    }
}
function Checking_Validate_Stage_Service() {
    if (Object.values(data_stage_service).length > 0) {
        let is_stage_ = 0;
        let is_percent = 0;
        for ([key, value] of Object.entries(data_stage_service)) {
            if (value.treatStageID == 0) {
                $('#StageService_' + key).css('background-color', 'rgb(255 216 216)'); is_stage_ = 1;
            }
            else $('#StageService_' + key).css('background-color', 'white');

            if (value.percent < 0 || Number.isInteger(value.percent) == false) {
                $('#StagePercentService_' + key).css('background-color', 'rgb(255 216 216)'); is_stage_ = 1;
            }
            else $('#StagePercentService_' + key).css('background-color', 'white');

            is_percent += parseInt(value.percent);
        }

        if (is_percent != 100) $('#textShowMessage').html(Outlang["Tong_phan_tram_buoc_dieu_tri_phai_la_100"] + '%');
        if (is_stage_ == 1) $('#textShowMessage').html(Outlang["Kiem_tra_du_lieu_buoc_dieu_tri"]);
    }
    else {
        let isUseExStage = $('#SD_ConsumableStage')?.is(':checked') ? 1 : 0;
        if (sys_ExportConsumableAllowStage == 1 && isUseExStage == 1) $('#textShowMessage').html(Outlang["Kiem_tra_du_lieu_buoc_dieu_tri"]);
    }
}
//#endregion

function Appointment_Render_Doctor_Resource_Header() {
    let data_resouce = settings.resources;
    let $calendar = $(settings.Calendar);


    if (Object.values(data_resouce).length > 0) {
        let length_resouce = Object.values(data_resouce).length;
        let $header_container = $("<div></div>");
        $header_container.addClass("header_container");
        $header_container.css({
            "display": "flex",
            "position": "sticky",
            "top": "0",
            "background-color": "#ffffff",
            "z-index": 100
        })

        let $empty_box = $("<div></div>");
        $empty_box.css({
            "width": settings.widthtimeline + "px",
            "height": settings.height + "px",
            "background-color": "#ffffff",
            "position": "sticky",
            "left": 0,
            "z-index": 100
        })
        $header_container.append($empty_box);

        let $cal_header_container = $("<div></div>");
        $cal_header_container.addClass("cal_header_container");
        $cal_header_container.css({
            "display": "flex",
            "width": "calc( 100% - " + settings.widthtimeline + "px )"
        });
        $header_container.append($cal_header_container);
        for ([key, value] of Object.entries(data_resouce)) {
            let item = value;
            let $header = $("<div></div>");
            $header.html(item.Name);
            $header.css({
                "min-width": settings.width + "px",
                "width": ((1 / length_resouce) * 100) + "%",
                "height": settings.height + "px",
                "background-color": "#ffffff"
            })
            $header.attr("data-id", key);
            $header.addClass("cal_header");
            $cal_header_container.append($header);
        }

        $calendar.append($header_container);
    }
}