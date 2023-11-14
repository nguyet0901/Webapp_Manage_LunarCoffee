
function _Add_Disease_(diseaseid, type, teethid, typecircle, path, face, root) {
    let find = false;
    for (ii = 0; ii < CurrentDisease.length; ii++) {

        if (CurrentDisease[ii].type == type && CurrentDisease[ii].teethid == teethid && CurrentDisease[ii].diseaseid == diseaseid && CurrentDisease[ii].typecircle == typecircle) {
            CurrentDisease[ii].diseaseid = diseaseid;
            CurrentDisease[ii].diseasename = data_disease.filter(word => word["ID"] == diseaseid)[0].Name;
            CurrentDisease[ii].empid = sys_employeeID_Main;
            CurrentDisease[ii].empname = sys_employeeName_Main;
            CurrentDisease[ii].userid = sys_userID_Main;
            CurrentDisease[ii].date = GetDateTime_Now_To_String();
            CurrentDisease[ii].updatetime = GetDateTime_Now_To_String();

            find = true;
            ii = CurrentDisease.length;
        }
    }
    if (!find) {

        let e = {};
        e.detailid = 0;
        e.diseaseid = diseaseid;
        e.diseasename = data_disease.filter(word => word["ID"] == diseaseid)[0].Name;
        e.type = type;
        e.teethid = teethid;
        e.typecircle = typecircle;
        e.pathcircle = path;
        e.face = face;
        e.root = root;
        e.userid = sys_userID_Main;
        e.empid = sys_employeeID_Main;
        e.empname = sys_employeeName_Main;
        e.date = GetDateTime_Now_To_String();
        e.updatetime = GetDateTime_Now_To_String();
        e.note = ""
        e.proposed_treat = '';
        e.classification = 0;
        e.DeleteButton = 1;
        CurrentDisease.push(e);

    }
}
function _Draw_Disease_Design_TEETH_WHOLE() {

    for (k = 0; k < CurrentDisease.length; k++) {
        if (Number(CurrentDisease[k].type) == 2) {
            // disease circle
            let datapath = data_layout_path.filter(word => word["ID"] == CurrentDisease[k].pathcircle)[0];
            if (datapath != undefined) {
                let path = datapath.pathData;
                let data = JSON.parse(data_disease.filter(word => word["ID"] == CurrentDisease[k].diseaseid)[0].Data).data[0].data;
                Particular_Draw_CIRCLE(CurrentDisease[k].teethid, CurrentDisease[k].typecircle, path, data);
            }
        }
        else {
            let data_temp = data_disease.filter(word => word["ID"] == CurrentDisease[k].diseaseid);
            let data = (data_temp != undefined && data_temp.length != 0) ? JSON.parse(data_temp[0].Data).data : [];

            for (let item = 0; item < data.length; item++) {

                switch (data[item].area) {

                    case "l": case "t": case "r": case "b": case "c":
                        let idcanvas = 'teeth_circle_' + data[item].area + '_' + CurrentDisease[k].teethid;
                        let pathincanvas = document.getElementById(idcanvas).attributes.data_path.value;
                        let datapath = data_layout_path.filter(word => word["ID"] == pathincanvas)[0];
                        if (datapath != undefined) {
                            let path = datapath.pathData;
                            Particular_Draw_CIRCLE(CurrentDisease[k].teethid, data[item].area, path, data[item].data);
                        }
                        break;
                    case "rootface":
                        Particular_Draw_FACEROOT(CurrentDisease[k].teethid, data[item].data, CurrentDisease[k].root, CurrentDisease[k].face);
                        break;
                }

            }
        }
    }
}
function Particular_GetPath_Image(imageid, typesign) {
    let _data_image_ = data_layout_image_disease.filter(word => word["ID"] == imageid)[0];
    let resulf = "";
    switch (typesign) {
        case "Root_1_U": resulf = _data_image_.Root_1_U; break;
        case "Root_3_U": resulf = _data_image_.Root_3_U; break;
        case "Root_1_D": resulf = _data_image_.Root_1_D; break;
        case "Root_3_D": resulf = _data_image_.Root_3_D; break;
        case "CL": resulf = _data_image_.CL; break;
        case "CT": resulf = _data_image_.CT; break;
        case "CR": resulf = _data_image_.CR; break;
        case "CB": resulf = _data_image_.CB; break;
        case "CC": resulf = _data_image_.CC; break;
        default: break;
    }
    return resulf;
}
function Particular_GetPath_Design (designid, typesign) {
    let _data_design_ = data_layout_design_disease.filter(word => word["ID"] == designid)[0];
    let resulf = "";
    switch (typesign) {
        case "Root_1_U": resulf = _data_design_.Root_1_U; break;
        case "Root_3_U": resulf = _data_design_.Root_3_U; break;
        case "Root_1_D": resulf = _data_design_.Root_1_D; break;
        case "Root_3_D": resulf = _data_design_.Root_3_D; break;
        default: break;
    }
    return resulf;
}
function Particular_Draw_FACEROOT(id, data, root, face) {
    for (i = 0; i < CanvasObject.length; i++) {

        if (CanvasObject[i].id == id && CanvasObject[i].type == "") {
            // debugger
            $('#teethfacetext_' + id).html('');
            $('#teethfacetext_' + id).css("color", "black");
            $('#teethfacetext_' + id).css("background-color", "transparent");

            let faceSign = CanvasObject[i].face_sign;
            let rootSign = CanvasObject[i].root_sign;
            let can = CanvasObject[i].canvas;
            let ctx = can.getContext('2d');
            // ctx.clearRect(0, 0, can.width, can.height);
            let dataroot = data_layout_path.filter(word => word["ID"] == root)[0];
            let dataface = data_layout_path.filter(word => word["ID"] == face)[0];

            for (yy = 0; yy < data.length; yy++) {
                if (data[yy].area == "root") {
                    let data_root = data[yy].data;
                    if (data_root != undefined && data_root.length != 0) {
                        let _2d = new Path2D(dataroot.pathData);
                        for (let xx = 0; xx < data_root.length; xx++) {
                            switch (data_root[xx].type) {
                                case "color":
                                    if (data_root[xx].having == "1") {
                                        ctx.fillStyle = data_root[xx].fill;
                                        ctx.strokeStyle = data_root[xx].stroke;
                                        ctx.fill(_2d);
                                        ctx.stroke(_2d);
                                    }
                                    break;
                                case "pattern":

                                    if (data_root[xx].having == "1") {
                                        let img = new Image();
                                        img.src = ser_imageFeature_Disease + data_root[xx].patternlink + '.png';
                                        img.onload = function () {
                                            var pattern = ctx.createPattern(img, 'repeat');
                                            ctx.fillStyle = pattern;
                                            ctx.fill(_2d);
                                        };
                                    }
                                    break;
                                case "image":
                                    if (data_root[xx].having == "1") {
                                        let linkimage = Particular_GetPath_Image(data_root[xx].imageid, rootSign);
                                        var img = new Image();
                                        img.src = ser_imageFeature_Disease + linkimage + '.svg';

                                        img.onload = function () {
                                            ctx.drawImage(img, 0, 0, 40, 90);
                                        }
                                    }
                                    break;
                                case "design":
                                    if (data_root[xx].having == "1") {
                                        let pathDesign = Particular_GetPath_Design(data_root[xx].designid, rootSign);
                                        let _design = new Path2D(pathDesign);
                                        ctx.fillStyle = data_root[xx].fill;
                                        ctx.strokeStyle = data_root[xx].stroke;
                                        ctx.fill(_design);
                                        ctx.stroke(_design);
                                        if (data_root[xx].ispattern == "1") {
                                            let img = new Image();
                                            img.src = ser_imageFeature_Disease + data_root[xx].patternlink + '.png';
                                            img.onload = function () {
                                                var pattern = ctx.createPattern(img, 'repeat');
                                                ctx.fillStyle = pattern;
                                                ctx.fill(_design);
                                            };
                                        }
                                    }
                                    break;
                                case "text":
                                    if (data_root[xx].having == "1") {
                                        $('#teethfacetext_' + id).html(data_root[xx].content);
                                        $('#teethfacetext_' + id).css("color", data_root[xx].stroke);
                                        $('#teethfacetext_' + id).css("background-color", data_root[xx].fill);
                                    }
                                    break;
                                default: break;
                            }
                        }
                    }
                }
                if (data[yy].area == "face") {
                    let data_face = data[yy].data;
                    if (data_face != undefined && data_face.length != 0) {
                        let _2d = new Path2D(dataface.pathData);
                        for (let xx = 0; xx < data_face.length; xx++) {
                            switch (data_face[xx].type) {
                                case "color":
                                    if (data_face[xx].having == "1") {
                                        ctx.fillStyle = data_face[xx].fill;
                                        ctx.strokeStyle = data_face[xx].stroke;
                                        ctx.fill(_2d);
                                        ctx.stroke(_2d);
                                    }
                                    break;
                                case "pattern":
                                    if (data_face[xx].having == "1") {
                                        let img = new Image();
                                        img.src = ser_imageFeature_Disease + data_face[xx].patternlink + '.png';
                                        img.onload = function () {
                                            var pattern = ctx.createPattern(img, 'repeat');
                                            ctx.fillStyle = pattern;
                                            ctx.fill(_2d);
                                        };
                                    }
                                    break;
                            }
                        }
                    }
                }


            }
            i = CanvasObject.length;
        }
    }
}
function Particular_Draw_CIRCLE(id, type, data_path, data) {
    for (i = 0; i < CanvasObject.length; i++) {
        if (CanvasObject[i].id == id && CanvasObject[i].type == type) {
            let can = CanvasObject[i].canvas;
            let ctx = can.getContext('2d');
            if (data != undefined && data.length != 0) {
                ctx.clearRect(0, 0, can.width, can.height);
                let _2d = new Path2D(data_path);
                for (let xx = 0; xx < data.length; xx++) {
                    switch (data[xx].type) {
                        case "color":
                            if (data[xx].having == "1") {
                                ctx.fillStyle = data[xx].fill;
                                ctx.strokeStyle = data[xx].stroke;
                                ctx.fill(_2d);
                                ctx.stroke(_2d);
                            }
                            break;
                        case "image":
                            if (data[xx].having == "1") {
                                let linkimage = Particular_GetPath_Image(data[xx].imageid, CanvasObject[i].path_sign);
                                var img = new Image();
                                img.src = ser_imageFeature_Disease + linkimage + '.svg';
                                img.onload = function () {
                                    if (Is_MAC_OS()) {
                                        ctx.drawImage(img, 0, 0, 40, 90);
                                    }
                                    else ctx.drawImage(img, 0, 0);
                                }
                            }
                            break;
                        case "pattern":
                            if (data[xx].having == "1") {
                                let img = new Image();
                                img.src = ser_imageFeature_Disease + data[xx].patternlink + '.png';
                                img.onload = function () {
                                    var pattern = ctx.createPattern(img, 'repeat');
                                    ctx.fillStyle = pattern;
                                    ctx.fill(_2d);
                                };
                            }
                            break;
                    }
                }
            }
            i = CanvasObject.length;
        }
    }
}
function Event_Click_Disease() {
    $(".diseaseitem").mousedown(function () {
        if (IDTeethFaceSelect.length != 0 || IDTeethCircleSelect != 0) {
            let diseaseID = $(this).attr('data_id');
            for (i = 0; i < IDTeethFaceSelect.length; i++) {
                // type = 1;//"faceroot";
                _Add_Disease_(diseaseID, 1, IDTeethFaceSelect[i].id, '', '', IDTeethFaceSelect[i].face, IDTeethFaceSelect[i].root);
            }

            for (i = 0; i < IDTeethCircleSelect.length; i++) {
                //type = 2;//"circle";
                _Add_Disease_(diseaseID, 2, IDTeethCircleSelect[i].id, IDTeethCircleSelect[i].type, IDTeethCircleSelect[i].path, '', '');
            }
            IDTeethFaceSelect = [];
            IDTeethCircleSelect = [];
            Clear_Selected_Teeth();
            _Draw_Disease_Design_TEETH_WHOLE();
            Render_Table_Desease_List(CurrentDisease, "dtContentDisease_DetailBody");
        }
    });
}
function Event_Click_Choose_FR_Teeth() {
    $(".teeth_part_root_face").mousedown(function () {
        if (IDTeethCircleSelect.length != 0) {
            IDTeethCircleSelect = [];
            Clear_Selected_Teeth_Cicle();
        }
        let teethID = $(this).attr('data_teeth');
        let face = $(this).attr('data_face');
        let root = $(this).attr('data_root');

        if (!$(this).hasClass("selectedColorFR")) {
            $(this).addClass("selectedColorFR");
            let e = {};
            e.id = teethID;
            e.face = face;
            e.root = root;
            IDTeethFaceSelect.push(e);
        }
        else {
            $(this).removeClass("selectedColorFR");
            for (i = IDTeethFaceSelect.length - 1; i >= 0; i--) {
                if (Number(IDTeethFaceSelect[i].id) == Number(teethID)) {
                    IDTeethFaceSelect.splice(i, 1);
                }
            }
        }

        ShowHide_Disease_ByType();
    });

}
function Event_Click_Choose_Circle_Teeth() {
    $(".div_circle_all").mousedown(function () {
        if (IDTeethFaceSelect.length != 0) {
            IDTeethFaceSelect = [];
            Clear_Selected_Teeth_FC();
        }

        let type = $(this).attr('data_type');
        let id = $(this).attr('data_id');
        let data_path = $(this).attr('data_path');
        if (!$(this).hasClass("selectedColorCircle")) {
            $(this).addClass("selectedColorCircle");
            let e = {};
            e.id = id;
            e.type = type;
            e.path = data_path;
            IDTeethCircleSelect.push(e);
        }
        else {
            $(this).removeClass("selectedColorCircle");
            for (i = IDTeethCircleSelect.length - 1; i >= 0; i--) {
                if (Number(IDTeethCircleSelect[i].id) == Number(id) && IDTeethCircleSelect[i].type == type) {
                    IDTeethCircleSelect.splice(i, 1);
                }
            }
        }
        ShowHide_Disease_ByType();
    });

}
function ShowHide_Disease_ByType() {

    if (IDTeethFaceSelect.length == 0 && IDTeethCircleSelect.length != 0) {
        let x = document.getElementsByClassName('diseaseitem');
        for (zz = 0; zz < x.length; zz++) {
            // Hide disease fc
            if (x[zz].attributes["data_type"].value == "1") {
                $('#' + 'desease_' + x[zz].attributes["data_id"].value).addClass('d-none');
            }
            else {
                $('#' + 'desease_' + x[zz].attributes["data_id"].value).removeClass('d-none');
            }
        }
    }
    else if (IDTeethCircleSelect.length == 0 && IDTeethFaceSelect.length != 0) {
        let x = document.getElementsByClassName('diseaseitem');
        for (zz = 0; zz < x.length; zz++) {
            // Hide disease fc
            if (x[zz].attributes["data_type"].value == "2") {
                $('#' + 'desease_' + x[zz].attributes["data_id"].value).addClass('d-none');
            }
            else {
                $('#' + 'desease_' + x[zz].attributes["data_id"].value).removeClass('d-none');

            }
        }
    }
    else {
        let x = document.getElementsByClassName('disease_for_face_root_teeth');
        for (zz = 0; zz < x.length; zz++) {
            $('#' + 'desease_' + x[zz].attributes["data_id"].value).removeClass('d-none');
        }
    }
}
function Clear_Selected_Teeth_FC() {
    let x = document.getElementsByClassName('teeth_part_root_face');
    for (i = 0; i < x.length; i++) {
        if (x[i].className.includes("selectedColorFR")) {
            x[i].className = "teeth_part_root_face";
        }
    }
}
function Clear_Selected_Teeth_Cicle() {
    let x = document.getElementsByClassName('div_circle_all');
    for (i = 0; i < x.length; i++) {
        if (x[i].className.includes("selectedColorCircle")) {
            x[i].className = "div_circle_all";
        }
    }
}
function Clear_Selected_Teeth() {
    Clear_Selected_Teeth_FC();
    Clear_Selected_Teeth_Cicle();
    ShowHide_Disease_ByType();
}


//#region  // Draw Default Teeth

function Execute_Clear_Draw_One_Teeth(teethid, type) {

    for (i = 0; i < CanvasObject.length; i++) {
        if (CanvasObject[i].id == teethid) {



            var ctx = CanvasObject[i].canvas.getContext('2d');
            ctx.beginPath();
            ctx.clearRect(0, 0, CanvasObject[i].canvas.width, CanvasObject[i].canvas.height);
            ctx.fillStyle = "#ffffff";
            ctx.globalAlpha = 0.2;
            ctx.globalAlpha = 1.0;
            ctx.lockMovementX = true;
            ctx.lockMovementY = true;
            ctx.lockScalingX = true;
            ctx.lockScalingY = true;
            ctx.lockUniScaling = true;
            ctx.lockRotation = true;
            ctx.hasControls = false;
            ctx.hasBorders = false;
            ctx.selectable = true;
            if (CanvasObject[i].data_circle == "") {
                let _face = data_layout_path.filter(word => word["ID"] == CanvasObject[i].data_face)[0];
                let _root = data_layout_path.filter(word => word["ID"] == CanvasObject[i].data_root)[0];
                ctx.strokeStyle = _face.strokeColor;
                ctx.stroke(new Path2D(_face.pathData));
                ctx.strokeStyle = _root.strokeColor;
                ctx.stroke(new Path2D(_root.pathData));
            }
            else {
                let path = data_layout_path.filter(word => word["ID"] == CanvasObject[i].data_circle)[0];
                ctx.strokeStyle = path.strokeColor;
                ctx.stroke(new Path2D(path.pathData));
            }
        }
    }
}

// #endregion

function Execute_Draw_Circle(can, ctx, id, type, data_path, data_sign) {
    let path = data_layout_path.filter(word => word["ID"] == data_path)[0];
    ctx.beginPath();
    ctx.clearRect(0, 0, can.width, can.height);
    let e = {};
    e.canvas = can;
    e.id = id;
    e.type = type;
    e.face_sign = "";
    e.root_sign = "";
    e.path_sign = data_sign;
    e.data_circle = data_path;
    e.data_face = "";
    e.data_root = "";
    CanvasObject.push(e);


    let _2d = new Path2D(path.pathData);
    ctx.globalAlpha = 0.2;
    ctx.globalAlpha = 1.0;
    ctx.lockMovementX = true;
    ctx.lockMovementY = true;
    ctx.lockScalingX = true;
    ctx.lockScalingY = true;
    ctx.lockUniScaling = true;
    ctx.lockRotation = true;
    ctx.hasControls = false;
    ctx.hasBorders = false;
    ctx.selectable = true;
    ctx.strokeStyle = path.strokeColor;
    ctx.fillStyle = path.fillColor;
    ctx.fill(_2d);
    ctx.stroke(_2d);


}
function Execute_Draw_Whole_Teeth() {
    $('.canvasTeethFace').each(function (i, obj) {
        var canvas = new fabric.Canvas(this.id);
        var ctx = canvas.getContext('2d');
        let data_face = $(this).attr('data_face');
        let data_root = $(this).attr('data_root');
        let data_id = $(this).attr('data_id');
        let datapath_face = data_layout_path.filter(word => word["ID"] == data_face)[0];
        let datapath_root = data_layout_path.filter(word => word["ID"] == data_root)[0];
        
        Execute_Draw_FR_Teeth(canvas, ctx, data_id, data_face, data_root, datapath_face.Sign, datapath_root.Sign)
    });

    $('.canvasTeethCircle').each(function (i, obj) {
        var canvas = new fabric.Canvas(this.id);
        var ctx = canvas.getContext('2d');
        let data_path = $(this).attr('data_path');
        let data_type = $(this).attr('data_type');
        let data_id = $(this).attr('data_id');
        let datapath_ = data_layout_path.filter(word => word["ID"] == data_path)[0];
        Execute_Draw_Circle(canvas, ctx, data_id, data_type, data_path, datapath_.Sign)
    });
}
function Execute_Draw_FR_Teeth(can, ctx, id, data_face, data_root, data_face_sign, data_root_sign) {
    let _face = data_layout_path.filter(word => word["ID"] == data_face)[0];
    let _root = data_layout_path.filter(word => word["ID"] == data_root)[0];
    let e = {};
    e.canvas = can;
    e.id = id;
    e.type = "";
    e.face_sign = data_face_sign;
    e.root_sign = data_root_sign;
    e.path_sign = "";
    e.data_circle = "";
    e.data_face = data_face;
    e.data_root = data_root;
    CanvasObject.push(e);

    let _2d_face = new Path2D(_face.pathData);
    let _2d_root = new Path2D(_root.pathData);
    ctx.clearRect(0, 0, 40, 90);
    ctx.globalAlpha = 0.2;
    ctx.fillStyle = "white";
    ctx.fillRect(0, 0, 40, 90);
    ctx.globalAlpha = 1.0;
    ctx.lockMovementX = true;
    ctx.lockMovementY = true;
    ctx.lockScalingX = true;
    ctx.lockScalingY = true;
    ctx.lockUniScaling = true;
    ctx.lockRotation = true;
    ctx.hasControls = false;
    ctx.hasBorders = false;
    ctx.selectable = true;


    ctx.strokeStyle = _face.strokeColor;
    ctx.fillStyle = _face.fillColor
    ctx.fill(_2d_face);
    ctx.stroke(_2d_face);


    ctx.strokeStyle = _root.strokeColor;
    ctx.fillStyle = _root.fillColor
    ctx.fill(_2d_root);
    ctx.stroke(_2d_root);

}



function Render_Disease_Teeh(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        let tr = "";
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                tr = '<li id="desease_' + item.ID + '" data_type="' + item.Type + '" data_id="' + item.ID + '" class="diseaseitem list-group-item border-0 d-flex justify-content-between ps-2 mb-2 border-radius-lg">'
                    +'<div class="d-flex align-items-center">'
                    + '<div class="me-3 text-center">'
                    + '<img style="height: 30px;width: 30px;" title="' + item.DescriptionTip + '" src="' + ser_imageFeature_Disease + item.ImageFeature + '.png" alt="label-image">'
                    +'</div>'
                    +'<div class="d-flex flex-column">'
                    + '<h6 class="mb-1 text-dark text-sm">' + item.Name+'</h6>'
                    + '<span class="text-xs">' + item.DescriptionTip+'</span>'
                    +'</div>'
                    +'</div>'
                    +'</li>'

                //tr = '<div id="desease_' + item.ID + '" data_type="' + item.Type + '" data_id="' + item.ID + '" class="ui image label diseaseitem">'
                //    + '<img title="' + item.DescriptionTip + '" src="' + ser_imageFeature_Disease + item.ImageFeature + '.png" alt="label-image">'
                //    + '<span class="disease_teeth_text">' + item.Name + '</span>'
                //    + '</div>';
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    Event_Click_Disease();
}
function Render_Table_Desease_List(data, id) {
    var myNode = document.getElementById(id);

    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let teethItem = data_layout_teeth.filter(word => word["ID"] == item.teethid);
                let teethName = teethItem[0].TeethName;
                let tr =
                    '<td class="d-none">' + item.diseaseid + '</td>'
                    + '<td class="d-none">' + item.teethid + '</td>'
                    + '<td class="d-none">' + item.type + '</td>'
                    + '<td class="d-none">' + item.typecircle + '</td>'
                    + '<td class="d-none">' + item.userid + '</td>'
                    + '<td class="vt-number-order"></td>'
                    + '<td>'
                    + '<span>' + item.updatetime + '</span>'
                    + '</td>'
                    + '<td>'
                    + (
                        (item.typecircle != '')
                            ? (teethName + ' ( ' + item.typecircle.toUpperCase() + ' )')
                            : teethName
                    )
                    + '</td>'
                    + '<td>' + item.diseasename + '</td>'
                    + '<td>' + AddCell_Treat_Teeth(item.proposed_treat, item.typecircle, item.type, item.diseaseid, item.teethid) + '</td>'
                    + '<td>' + AddCell_Classification(item.classification,item.typecircle, item.type, item.diseaseid, item.teethid ) + '</td>'
                    + '<td>' + AddCell_Note_Teeth(item.note,item.typecircle, item.type, item.diseaseid, item.teethid) + '</td>'

                    + ((item.DeleteButton === 1)
                        ? ('<td><i data_typecircle="' + item.typecircle + '" data_type="' + item.type + '" data_disease="' + item.diseaseid + '" data_teethid="' + item.teethid + '" class="deleteDisease vtt-icon vttech-icon-delete"></i> </td>')
                        : '<td></td>')

                stringContent = stringContent + '<tr role="row" class="vt-number">' + tr + '</tr>';
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    Event_Click_Status_List();
    Fill_data_class_To_Design();
    Event_Text_Change_NoteTeeth();
    $('#dtContentDisease_Detail').tablesort();
    $('#dtContentDisease_Detail .ui.dropdown')
        .dropdown({
            onShow: function (value, text, $selectedItem) {
                let ui_drop = $(this);
                Dropdown_Set_Position(ui_drop);
            },
            onHide: function (value, text, $selectedItem) {
                let ui_drop = $(this);
                Dropdown_Remove_Position(ui_drop);
            },
            allowCategorySelection: true,
            forceSelection: false
        });
}


function AddCell_Note_Teeth(note,typecircle, type, diseaseid, teethid ) {
    let resulf = '<input data_typecircle="'
        + typecircle + '" data_type="'
        + type + '" data_disease="'
        + diseaseid + '" data_teethid="'
        + teethid + '" class="form-control noteDiseaseTeeth" placeholder="eg .' + Outlang["Ghi_chu"] + '"  type="text"  value="'
        + note + '">';
    return resulf;
}
function AddCell_Treat_Teeth(treat, typecircle, type, diseaseid, teethid) {
    let resulf = '<input data_typecircle="'
        + typecircle + '" data_type="'
        + type + '" data_disease="'
        + diseaseid + '" data_teethid="'
        + teethid + '" class="form-control treatDiseaseTeeth" placeholder="eg .' + Outlang["Dieu_tri"] + '" type="text"  value="'
        + treat + '">';
    return resulf;
}
function AddCell_Classification(_classsi, typecircle, type, diseaseid, teethid) {
    let _id = 'classDiseaseTeeth' + '_' + typecircle + '_' + type + '_' + diseaseid + '_' + teethid;
    let resulf = '<div data_typecircle="'
        + typecircle + '" data_type="'
        + type + '" data_disease="'
        + diseaseid + '" data_teethid="'
        + teethid + '" class="ui fluid search selection dropdown form-control classDiseaseTeeth" id=' + _id+'><input type="hidden"/><i class="dropdown icon"></i>' +
        '<input  class="search" autocomplete="off" tabindex="0" /><div class="default text">eg .' + Outlang["Phan_loai"] + '</div><div class="menu" tabindex="-1">';

    for (i = 0; i < ser_classification_date.length; i++) {
        resulf = resulf + '<div class="item" data-value=' + ser_classification_date[i].ID + '>' + ser_classification_date[i].Name + '</div>'
    }
    resulf = resulf + '</div>';
    return resulf;

}
function Fill_data_class_To_Design() {
    for (ii = 0; ii < CurrentDisease.length; ii++) {
        let _id = 'classDiseaseTeeth' + '_'
            + CurrentDisease[ii].typecircle + '_'
            + CurrentDisease[ii].type + '_'
            + CurrentDisease[ii].diseaseid + '_'
            + CurrentDisease[ii].teethid;
        if ($('#' + _id).length) {
            $('#' + _id).dropdown('refresh')
            $('#' + _id).dropdown('set selected', CurrentDisease[ii].classification);
        }
    }
}
function Event_Click_Status_List() {
    $(".deleteDisease").click(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let typecircle = $(this).attr('data_typecircle');


        for (iiii = CurrentDisease.length - 1; iiii >= 0; iiii--) {
            if (CurrentDisease[iiii].diseaseid == diseaseid
                && CurrentDisease[iiii].teethid == teethid
                && CurrentDisease[iiii].type == type
                && CurrentDisease[iiii].typecircle == typecircle) {
                CurrentDisease.splice(iiii, 1);
                break;
            }
        }
        Clear_Selected_Teeth();
        Execute_Clear_Draw_One_Teeth(teethid, type);
        _Draw_Disease_Design_TEETH_WHOLE();
        Render_Table_Desease_List(CurrentDisease, "dtContentDisease_DetailBody");
        $('#teethfacetext_' + teethid).html('');
        $('#teethfacetext_' + teethid).css("color", "black");
        $('#teethfacetext_' + teethid).css("background-color", "transparent");
    });
}
function Event_Text_Change_NoteTeeth() {

    $(".treatDiseaseTeeth").change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let typecircle = $(this).attr('data_typecircle');
        for (ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].type == type
                && CurrentDisease[ii].teethid == teethid
                && CurrentDisease[ii].diseaseid == diseaseid
                && CurrentDisease[ii].typecircle == typecircle
            ) {
                CurrentDisease[ii].proposed_treat = $(this).val();
                ii = CurrentDisease.length;
            }
        }
    });
    $(".noteDiseaseTeeth").change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let typecircle = $(this).attr('data_typecircle');
        for (ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].type == type
                && CurrentDisease[ii].teethid == teethid
                && CurrentDisease[ii].diseaseid == diseaseid
                && CurrentDisease[ii].typecircle == typecircle
            ) {
                CurrentDisease[ii].note = $(this).val();
                ii = CurrentDisease.length;
            }
        }
    });
    $(".classDiseaseTeeth").unbind('change').change(function () {
        let diseaseid = $(this).attr('data_disease');
        let teethid = $(this).attr('data_teethid');
        let type = $(this).attr('data_type');
        let typecircle = $(this).attr('data_typecircle');
        for (ii = 0; ii < CurrentDisease.length; ii++) {
            if (CurrentDisease[ii].type == type
                && CurrentDisease[ii].teethid == teethid
                && CurrentDisease[ii].diseaseid == diseaseid
                && CurrentDisease[ii].typecircle == typecircle
            ) {
                let _value = $(this).dropdown('get value');
                if (_value != undefined) {
                    CurrentDisease[ii].classification = _value;
                    ii = CurrentDisease.length;
                }

            }
        }
    });

    $(".ui.dropdown").dropdown({
        allowCategorySelection: true,
        forceSelection: false
    });
}