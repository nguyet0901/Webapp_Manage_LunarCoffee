var CanTeethAreaName = {
    "l": "Trái"
    , "r": "Phải"
    , "t": "Trên"
    , "b": "Dưới"
    , "c": "Giữa"
    , "face": ""
    ,"root":""
}; 

//#region // Canvas Execute
function Canele_Pattern (color, size) {
    let patternCanvas = document.createElement("canvas");
    let patternContext = patternCanvas.getContext("2d");
    patternCanvas.width = size;
    patternCanvas.height = size;
    patternContext.fillStyle = "transparent";
    patternContext.strokeStyle = color;
    patternContext.lineWidth = 1
    patternContext.fillRect(0, 0, patternCanvas.width, patternCanvas.height);
    patternContext.moveTo(0, 0);
    patternContext.lineTo(size, size);
    patternContext.stroke();
    return patternCanvas;
}
function Can_DrawDefault (objs, w, h, callback, unselect, goselect) {
    objs.each(function (i, obj) {
        var canvasid = this.id;
        var updown = this.dataset.updown;
        var sign = this.dataset.sign;
        var imgex = this.dataset.imgex;
 
        var src = DendisR_DetectTeethSrc(updown, sign, imgex);
        var type = DendisR_DetectType(updown, sign);
        var canvas = document.getElementById(canvasid);
        var ctx = canvas.getContext('2d');
        canvas.height = h;
        canvas.width = w;
        var img = new Image();
        img.onload = function () {
            ctx.drawImage(img, 0, 0, w, h);
            var xmlhttp = new XMLHttpRequest();
            var paths = {};
            xmlhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var xmlDoc = $.parseXML(this.response);
                    var _path = $(xmlDoc).find("path");
                    for (var i = 0; i < _path.length; i++) {
                        var x = _path[i];
                        var id = x.id;
                        var path = x.getAttribute("d");
                        var p = new Path2D(path);
                        if (id != '') paths[canvasid + '_' + id] = {
                            'path': p,
                            'type': type
                        };
                        Can_DrawReset(ctx, p);
                    }
                    if (typeof callback !== 'undefined' && typeof callback === 'function') callback(paths);
                    canvas.addEventListener("click", function (e) {
                        for (var i = 0; i < _path.length; i++) {
                            var r = this.getBoundingClientRect();
                            var canid = this.dataset.id;
                            if (canid != undefined) {
                                var x1 = e.clientX - r.left
                                var y1 = e.clientY - r.top;
                                var x = _path[i];
                                let dataid = x.id;
                                if (dataid != '') {
                                    var path = x.getAttribute("d");
                                    var p = new Path2D(path);
                                    if (ctx.isPointInPath(p, x1, y1)) {
                                        let objid = dataid.toString() + '-' + canid.toString();
                                        if (TeethSelected[objid] != undefined && TeethSelected[objid] != "") {
                                            if (typeof unselect !== 'undefined' && typeof unselect === 'function') unselect(dataid, canid);
                                        }
                                        else {
                                            Can_DrawSelected(ctx, p);
                                            if (typeof goselect !== 'undefined' && typeof goselect === 'function') goselect(dataid, canid);
                                        }
                                    }
                                }
                            }
                        }
                    })
                }
            };
            xmlhttp.open("GET", src, true);
            xmlhttp.send();
        }
        img.src = src;
    });

}
function Can_DrawSelected (ctx, p) {
    ctx.beginPath();
    let pattern = ctx.createPattern(canpattern, "repeat");
    ctx.fillStyle = pattern;
    ctx.strokeStyle = "transparent";
    ctx.lineWidth = 1;
    ctx.stroke(p);
    ctx.fill(p);
}
function Can_DrawReset (ctx, p) {
    ctx.beginPath();
    ctx.strokeStyle = "transparent";
    ctx.fillStyle = "transparent";

    ctx.stroke(p);
    ctx.fill(p);
}
function Can_ClearWholeTeeth (teeth, calback) {
    Can_DrawDefault($('#teethcircle_' + teeth), TeethCircle, TeethCircle
        , function () {
            Can_DrawDefault($('#teethface_' + teeth), TeethWidth, TeethHeight
                , function () {
                    calback();
                });
        });

}
function Can_ClearCircle (teeth, calback) {
    Can_DrawDefault($('#teethcircle_' + teeth), TeethCircle, TeethCircle
        , function () {
            calback();
        });

}
function Can_ClearRootFace (teeth, calback) {
    Can_DrawDefault($('#teethface_' + teeth), TeethWidth, TeethHeight
        , function () {
            calback();
        });

}
//#endregion
//#region // Detect Teeth Location
function DendisR_DetectTeethSrc (updown, sign, imgex) {
    if (updown != undefined && sign != undefined) {
        if (imgex != "") return imgex;
        if (updown == "U") {
            if (sign == "8") return "/Canvas/rangham_tren.svg";
            else return "/Canvas/rangcua_tren.svg";
        }
        else {
            if (sign == "12") return "/Canvas/rangham_duoi.svg";
            else return "/Canvas/rangcua_duoi.svg";
        }
    }
    else {
        return "/Canvas/matrang.svg";
    }
    
}
function DendisR_DetectType (updown, sign) {
    if (updown != undefined && sign != undefined) {
        if (updown == "U") {
            if (sign == "8") return "r3u";
            else return "r1u";
        }
        else {
            if (sign == "12") return "r3d";
            else return "r1d";
        }
    }
    else {
        return "circle";
    }
}
//#endregion

//#region // Desease Execute
async function Dendis_RenderDisease (data, id) {
    new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
           
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let partname = CanTeethAreaName[item.Part];
                    let picturecat = DiseaseCatList[item.Type] ? ser_imageFeature_Disease + DiseaseCatList[item.Type].CatImg : Master_Default_Pic
                    let tr = `
                        <td class="vt-number-order"></td>
                        <td>
                            <div class="d-flex">
                                <div class="avatar avatar-xs me-3">
                                    <img src="${picturecat}" alt="kal" class="rounded-0">
                                </div>
                                <div class="d-flex flex-column justify-content-center text-center" style="width: 40px;">
                                    <h6 class="mb-0 text-sm fw-bold  text-dark">${TeethCollkv[item.Teethid].TeethName}</h6>
                                    <p class="mb-0 text-xs">${partname ?? ''}</p>
                                </div>
                            </div>
                        </td>
                        <td>${DiseaseList[item.Diseaseid].Name}</td>
                        <td>${Dendis_RenderClass(item.Classification, item.Part, item.Type, item.Diseaseid, item.Teethid)}</td>
                        <td>${Dendis_RenderTreat(item.Treat, item.Part, item.Type, item.Diseaseid, item.Teethid)}</td>
                        <td>${Dendis_RenderNote(item.Note, item.Part, item.Type, item.Diseaseid, item.Teethid)}</td>
                        <td>${Dendis_RenderButton(item.DeleteButton, item.Part, item.Type, item.Diseaseid, item.Teethid)}</td>
                    `
                    stringContent = stringContent + '<tr role="row" class="vt-number">' + tr + '</tr>';
        
                }
            }
            document.getElementById(id).innerHTML = stringContent;
        }
        Dendis_DiseaseFill(data);
        Dendis_DiseaseEvent();
    });
}
function Dendis_DiseaseFill (data) {
    for (let i = 0; i < data.length; i++) {
        let item = data[i];
        let _id = 'classDiseaseTeeth' + '_' + item.Part + '_' + item.Type + '_' + item.Diseaseid + '_' + item.Teethid;
        $("#" + _id).dropdown("refresh");
        $("#" + _id).dropdown("set selected", item.Classification);
    }
}
function Dendis_RenderClass (classi, part, type, diseaseid, teethid) {
    let _id = 'classDiseaseTeeth' + '_' + part + '_' + type + '_' + diseaseid + '_' + teethid;
    let resulf = `<div
        data_part="${part}" data_type="${type}" data_disease="${diseaseid}" data_teethid="${teethid}" 
        class="ui fluid search selection dropdown form-control classDiseaseTeeth" id=${_id}><input type="hidden"/><i class="dropdown icon"></i>
        <input  class="search" autocomplete="off" tabindex="0" /><div class="default text"></div><div class="menu" tabindex="-1">`;

    for (i = 0; i < DisClassi.length; i++) {
        resulf = resulf + `<div class="item" data-value=${DisClassi[i].ID}>${DisClassi[i].Name}</div>`
    }
    resulf = resulf + `</div>`;
    return resulf;

}

function Dendis_RenderNote (note, part, type, diseaseid, teethid) {
    let resulf = `<input data_part="${part}" data_type="${type}" data_disease="${diseaseid}" data_teethid="${teethid}" 
       class="form-control noteDiseaseTeeth"  type="text"  value="${note}">`;
    return resulf;
}
function Dendis_RenderTreat (treat, part, type, diseaseid, teethid) {
    let resulf = `<input data_part="${part}" data_type="${type}" data_disease="${diseaseid}" data_teethid="${teethid}" 
       class="form-control treatDiseaseTeeth"  type="text"  value="${treat}">`;
    return resulf;
}
function Dendis_RenderButton (iddelete, part, type, diseaseid, teethid) {
    let resulf = '';
    if (iddelete == 1)
        resulf = `<a class="deleteDisease ms-auto ms-auto p-2 py-1" data_part="${part}" 
                data_type="${type}" data_disease="${diseaseid}" data_teethid="${teethid}">
                <i class="text-danger fas fa-times"  ></i>
            </a>`;
    return resulf;
}
function Dendis_DiseaseEvent () {
    $(".ui.dropdown.classDiseaseTeeth").dropdown({
        allowCategorySelection: true,
        forceSelection: false
    });
    $("#CanDisease_DetailBody .deleteDisease").unbind().click(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let part = $(this).attr('data_part');
        for (let ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].Type == type
                && CurrentDisease[ii].Teethid == teethid
                && CurrentDisease[ii].Diseaseid == diseaseid
                && CurrentDisease[ii].Part == part) {
                $('#teethfacetext_' + teethid).html('');
                $('#teethfacetext_' + teethid).css("color", "black");
                $('#teethfacetext_' + teethid).css("background-color", "transparent");
                CurrentDisease.splice(ii, 1);
                break;
            }
        }
        Dencan_Draw1Teeth(teethid, '', '')
        Dendis_RenderDisease(CurrentDisease, "CanDisease_DetailBody");

    });
    $("#CanDisease_DetailBody .treatDiseaseTeeth").unbind().change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let part = $(this).attr('data_part');
        for (let ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].Type == type
                && CurrentDisease[ii].Teethid == teethid
                && CurrentDisease[ii].Diseaseid == diseaseid
                && CurrentDisease[ii].Part == part
            ) {
                CurrentDisease[ii].Treat = $(this).val();
                break;
            }
        }
    });
    $("#CanDisease_DetailBody .noteDiseaseTeeth").unbind().change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let part = $(this).attr('data_part');
        for (let ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].Type == type
                && CurrentDisease[ii].Teethid == teethid
                && CurrentDisease[ii].Diseaseid == diseaseid
                && CurrentDisease[ii].Part == part
            ) {
                CurrentDisease[ii].Note = $(this).val();
                break;
            }
        }
    });
    $("#CanDisease_DetailBody .classDiseaseTeeth").unbind('change').change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let part = $(this).attr('data_part');
        for (ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].Type == type
                && CurrentDisease[ii].Teethid == teethid
                && CurrentDisease[ii].Diseaseid == diseaseid
                && CurrentDisease[ii].Part == part
            ) {
                let _value = $(this).dropdown('get value');
                if (_value != undefined) {
                    CurrentDisease[ii].Classification = _value;
                    ii = CurrentDisease.length;
                }
            }
        }
    });
}
//#endregion