// #endregion

// #region // render teeth
function RenderTeethFront(data, id, teeth) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        teeth = ',' + teeth;
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < 16; i++) {
                let item = data[i];
                let idstring = ',' + item.ID + ',';
                let tr = "";
                if ((teeth.includes(idstring) == true)) {
                    tr = '<div class="column" style="height: 70px">'
                        + '<img style="min-width: 30px" src="/assests/img/TeethTab/' + item.UrlImage + '.png">'
                        + '<div class="ui checkbox" style="margin: 5px">'
                        + '<input type="checkbox" value="' + item.ID + '" class="checkTeeth" data-id="' + item.ID + '"/>'
                        + '<label class="coloring red"></label>'
                        + '</div>'
                        + '</div>'
                }
                else {
                    tr = '<div class="column" style="height: 70px">'
                        + '<img style="opacity:0.3;min-width: 30px" src="/assests/img/TeethTab/' + item.UrlImage + '.png">'
                        + '</div>'
                }


                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    EventLinkClick_CheckBoxList();
}
function RenderTeethBellow(data, id, teeth) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        teeth = ',' + teeth;
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 16; i < 32; i++) {
                let item = data[i];
                let idstring = ',' + item.ID + ',';
                let tr = "";
                if ((teeth.includes(idstring) == true)) {
                    tr = '<div class="column">'
                        + '<div class="ui checkbox" style="margin: 5px">'
                        + '<input type="checkbox" value="' + item.ID + '" class="checkTeeth" data-id="' + item.ID + '" />'
                        + '<label class="coloring red"></label>'
                        + '</div><img style="min-width: 30px" src="/assests/img/TeethTab/' + item.UrlImage + '.png"></div>'
                }
                else {
                    tr = '<div class="column">'
                        + '<div style="height: 29px;"></div>'
                        + '<img style="opacity:0.3;min-width: 30px" src="/assests/img/TeethTab/' + item.UrlImage + '.png"></div>'
                }

                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    EventLinkClick_CheckBoxList();
}
function renderLaboTemplate(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let listQuiz = item.Quiz;
                let tr = "<div class='groupItem'  style='display:flex;" + item.style + "'>"
                for (var k = 0; k < listQuiz.length; k++) {
                    itemQuiz = listQuiz[k];
                    tr = tr + renderLaboQuestions(itemQuiz);
                }
                tr = tr + "</div>"
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }

}
function renderLaboQuestions(item) {
    let tr = '';
    tr = '<div style="width:' + item.percent + '%; ' + item.style + '">'
        + '<span class="header">' + item.title + '</span>'
        + '<div class="content ' + item.vector + '" >';
    let dataListItems = item.listitems;
    if (dataListItems && dataListItems.length > 0) {
        for (var k = 0; k < dataListItems.length; k++) {
            let item = dataListItems[k];
            tr = tr + RenderItemCheckList(item);
        }
    }
    tr = tr + '</div></div>';
    return tr;

}
function RenderItemCheckList(item) {
    let tr = "";
    switch (item.type) {
        case "checkbox":
            tr = tr + '<div class="field"><div class="ui checkbox">'
                + '<input type="checkbox" class="checkQA" value="small" id="' + item.id + '" data-id="' + item.id + '">'
                + '<label for="' + item.id + '">' + item.label + '</label>'
                + '</div></div>';
            break;
        case "checkboxOther":
            tr = tr + '<div class="field"><div class="ui checkbox">'
                + '<input type="checkbox" class="checkQA" value="small" id="' + item.id + '" data-id="' + item.id + '">'
                + '<label><input type="text" placeholder="Other answers..." id="other' + item.id + '" onchange="getValueCheckboxOther(' + item.id + ')"></label>'
                + '</div></div>';
            break;
        case "checkbox_fillnumber": //checkboxfillnumber
            tr = tr + '<div class="field"><div class="ui checkbox" style="display: inline-flex">'
                + '<input type="checkbox" class="checkQA" value="small" id="' + item.id + '" data-id="' + item.id + '">'
                + '<label>' + item.label + '<input type="text" style="max-width: ' + item.width + '; padding-left: 10px" class="no-outline" placeholder="" id="other' + item.id + '" onchange="getValueCheckboxOther(' + item.id + ')">' + item.unit + '</label>'
                + '</div></div>';
            break;
        case "header":
            tr = tr + '<div class="field">'
                + '<label class="header">' + item.text + '</label>'
                + '</div>';
            break;
        case "text":
            tr = tr + '<div class="field">'
                + '<label style="color: rgba(0,0,0,.87);transition: color .1s ease;font-size: 1em;">' + item.text + '</label>'
                + '</div>';
            break;

        case "checkbox_img":
            tr = tr + '<div class="column" style="position: relative;padding:10px 10px 30px 10px"><img src="/assests/img/pontic/' + item.link + '" class="ui image">'
                + '<label>' + item.title + '</label><br/>'
                + '<div class="ui checkbox" style="position: absolute;bottom: 0;left: 50%;transform: translateX(-50%)"><input type="checkbox" name="small" id="' + item.id + '" class="checkQA" data-id="' + item.id + '" />'
                + '<label></label>'
                + '</div></div>'
            break;
        case "img":
            tr = tr + '<div class="column" style="position: relative;padding:10px 10px 30px 10px"><img style="' + item.style + '" src="/assests/img/pontic/' + item.link + '" class="ui image"></div>'
            break;
        case "lable_note":
            tr = tr + '<div class="field" style="display:flex;white-space: nowrap;">'
                + '<span class="header">' + item.label + '</span>'
                + '<input type="text" style=" padding-left: 10px" class="no-outline" placeholder="" id="' + item.id + '" onchange="getAnswerDataType_Note(' + item.id + ')">'
                + '</div>';
            break;
        case "textarea":
            tr = tr + '<textarea id="' + item.id + '" onchange="getAnswerDataType_Note(' + item.id + ')" name="content" style="max-height: 1em"></textarea>'
            break;

        default: break;
    }
    return tr;
}
// #endregion 

// #region // envent 
function EventLinkClick_TeethCheck() {
    $(".checkTeeth").change(function () {
        let count = 0;
        var x = document.getElementsByClassName("checkTeeth");
        for (let i = 0; i < x.length; i++) {
            if (x[i].checked) count = count + 1;
        }
        $('#Labo_Unit').val(count);
        ChangingEvent_Labo();
    });
}
function EventLinkClick_CheckBoxList() {
    $(".checkQA").on('click', function (event) {
        var id = $(this).attr("data-id");
        if (this.checked == false) {
            for (let i = dataAnswerLabo.length - 1; i >= 0; i--) {
                if (dataAnswerLabo[i].id == id) {
                    dataAnswerLabo.splice(i, 1);
                    break;
                }
            }
        }
        else {
            var myEle = document.getElementById("other" + id);
            if (myEle) {
                var answer = $('#other' + id).val() ? $('#other' + id).val() : "";
                if (answer != "") {
                    let itemCheckedOther = { type: "checkother", id: id, text: answer };
                    dataAnswerLabo.push(itemCheckedOther);
                }
            }
            else {
                let itemChecked = { type: "checkbox", id: id };
                dataAnswerLabo.push(itemChecked);
            }
        }
    });
}
function ChangingEvent_Labo() {
    let number = $('#Labo_Unit').val() ? Number($('#Labo_Unit').val()) : 0;
    let unitprice = $('#Labo_Unit_Price').val() ? Number($('#Labo_Unit_Price').val()) : 0;
    $('#Labo_Total_Price').val(number * unitprice);
}
// #endregion 

// load data answer update
function loadAnswerUpdate() {
    //if (dataAnswerLabo != undefined && dataAnswerLabo != null && dataAnswerLabo.length > 0) {
    //    for (i = 0; i < dataAnswerLabo.length; i++) {
    //        var item = dataAnswerLabo[i];
    //        switch (item.type) {
    //            case "checkbox":
    //                var myEle = document.getElementById(item.id);
    //                if (myEle) {
    //                    $("#" + item.id).prop("checked", true);
    //                }
    //                break;
    //            case "checkother":
    //                var myEle = document.getElementById(item.id);
    //                if (myEle) {
    //                    $("#" + item.id).prop("checked", true);
    //                }
    //                var myEle1 = document.getElementById("other" + item.id);
    //                if (myEle1) {
    //                    $('#other' + item.id).val(item.text);
    //                }
    //                break;
    //            case "note":
    //                var myEle = document.getElementById(item.id);
    //                if (myEle) {
    //                    $("#" + item.id).val(item.text);
    //                }
    //                break;
    //            default: break;
    //        }
    //    }
    //}
}

// lay value cua cau tra loi them
function getValueCheckboxOther(id) {
    AnswerID_Other = 'other' + id;
    if ($('#' + id).is(":checked")) {
        let itemCheckedOther = {};
        var answer = $('#' + AnswerID_Other).val() ? $('#' + AnswerID_Other).val() : "";
        let data = dataAnswerLabo.filter(word => word["id"] == id);
        if (data != undefined && data != null && data.length != 0) {
            for (let i = dataAnswerLabo.length - 1; i >= 0; i--) {
                if (dataAnswerLabo[i].id == id) {
                    dataAnswerLabo.splice(i, 1);
                    break;
                }
            }
            itemCheckedOther = { type: "checkother", id: id, text: answer };
        }
        else {
            itemCheckedOther = { type: "checkother", id: id, text: answer };
        }
        dataAnswerLabo.push(itemCheckedOther);
    }
}

// get answer type (note)
function getAnswerDataType_Note(id) {
    var answer = $('#' + id).val() ? $('#' + id).val() : "";
    let itemnote = { type: "note", id: id, text: answer };
    dataAnswerLabo.push(itemnote);
}
// render template
function FillTeethWhenUpdate(TeethChoosing) {
    if (TeethChoosing != "") {
        $('#teethChoosing').show();
        var x = document.getElementsByClassName("checkTeeth");
        var teehchose = TeethChoosing.split(",")
        for (let i = 0; i < x.length; i++) {
            for (let j = 0; j < teehchose.length; j++) {
                if (x[i].value == teehchose[j]) {
                    x[i].checked = true;
                }
            }
        }
    }
}