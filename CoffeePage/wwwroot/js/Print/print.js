(function () {

    //js_require('/js/scale2fit.js');

    js_require('/js/comon/Formatnumber.js');
    js_require('/js/comon/Other.js');
    js_require_notasync('/js/comon/Combo_v2.js');
    js_require('/assests/dist/Barcode/JsBarcode.code128.min.js');

    js_require('/js/comon/initialize_setting.js');
    js_require('/js/comon/permission_tabcontrol.js');


})();

function LoadCombo_Template (url, idcombo, callback_loadrow) {
    $.ajax({
        url: url,
        dataType: "json",
        type: "POST",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError();
        },
        success: function (result) {
            if (result.d != "") {
                let data = JSON.parse(result.d);
                Load_Combo(data, idcombo, true);
                $("#" + idcombo).parent().dropdown("refresh");
                $("#" + idcombo).parent().dropdown("set selected", data[0].ID);
                callback_loadrow();
            }
        }
    });
}
function LoadRow_Master (url, parameter, callback) {
    $.ajax({
        url: url,
        dataType: "json",
        type: "POST",
        data: parameter,
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError();
        },
        success: function (result) {
            if (result.d != "") {
                let data = JSON.parse(result.d);
                callback(data)
            } else {
                notiError();
            }
        }
    });
}

//#region // Render
function Render_Row (data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';

        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr =
                    '<td style="display:none !important">' + item.ID + '</td>'
                    + '<td style="text-align:center">' + (i + 1) + '</td>'
                    + '<td>'
                    + '<span class="code">' + item.ServiceCode + '</span>'
                    + '<span class="name">' + item.ServiceName + '</span>'
                    + '<span class="amount">' + formatNumber(item.Amount) + '</span>'
                    + '</td>'
                    + '<td class="print_button_choose text-center" >'
                    + '<div class="form-check form-check-inline mx-0">'
                    + '<input type="checkbox" title=' + item.ID + ' class="row_to_print form-check-input" checked/>'
                    + '<label class="coloring red"></label></div>'
                    + '</td>'
                stringContent = stringContent + '<tr role="row">' + tr + '</tr>';
            }
        }

        document.getElementById(id).innerHTML = stringContent;
    }
}
function Render_Treatment (data, id, callback) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr = item.DocName + ' : (' + item.ServiceName + ') ' + item.Content_Follow
                stringContent += tr + ' \n';
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
//#endregion

//#region // Template detail
function Detail_Template (url, templateid, callback) {

    $.ajax({
        url: url,
        dataType: "json",
        type: "POST",
        data: JSON.stringify({'id': templateid}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            notiError();
        },
        success: function (result) {
            if (result.d != "[]") {
                let data = JSON.parse(result.d);
                callback(data);
            }
        }
    });
}
function Render_Template (rows, elements, idheader, idbody, idfooter) {

    let stringContext = '';
    let header = '';
    let content = '';
    let footer = '';

    for (i = 0; i < rows.length; i++) {
        stringContext = '';
        let value = JSON.parse(rows[i].Value);
        let classname = (rows[i].Class != '') ? rows[i].Class : 'content';



        let styleborder = Render_Template_Border(rows[i].BorderTop, rows[i].BorderLeft, rows[i].BorderBottom, rows[i].BorderRight);
        let stylePaddingMargin = 'display: flex ;'
            + ((rows[i].Padding.trim() != '') ? ('padding:' + rows[i].Padding.trim() + ';') : ('padding:' + 'auto;'))
            + ((rows[i].Margin.trim() != '') ? ('margin:' + rows[i].Margin.trim() + ';') : ('margin:' + 'auto;'));

        if (rows[i].Table != 0) classname = classname + ' fix';
        stringContext = '<div   id="r_' + rows[i].ID + '" class="' + classname + '" style="' + stylePaddingMargin + styleborder + '">';
        if (rows[i].Table == 1) {
            stringContext = stringContext + Render_Template_Table(value, elements);
            tableid = rows[i].ID
        }
        else {
            for (j = 0; j < Object.keys(value).length; j++) {
                let item = value[j];
                if (item != undefined) {
                    stringContext = stringContext + Render_Template_group(item, elements);
                }
            }
        }
        stringContext = stringContext + '</div>';
        if (classname.includes('header')) {
            header = header + stringContext;
        }

        if (classname.includes('content')) {

            content = content + stringContext;
        }
        if (classname.includes('footer')) {

            footer = footer + stringContext;
        }
    }


    $('#' + idheader).html(header);
    $('#' + idbody).html(content);
    $('#' + idfooter).html(footer);

}
function Render_Template_group (item, elements) {
    let dtdata = item.data.slice(0, -1).split(',');
    let elms = [];
    for (s = 0; s < dtdata.length; s++) {
        let dtsl = dtdata[s];
        let els = elements.filter(function (elm) {
            if (elm.ID == dtsl)
                return true;
            else
                return false;
        });
        if (els.length > 0)
            elms.push(els[0]);
    }
    let str = '<div style="' + item.style + '">';
    for (k = 0; k < elms.length; k++) {
        str = str + elms[k].Element;
    }
    str = str + '</div>';
    return str;
}
function Render_Template_Table (obj, elements) {

    pf_cols_table = [];
    let stringContext = "";
    for (j = 0; j < Object.keys(obj).length; j++) {
        let item = obj[j];

        if (item != undefined) {
            let column = elements.filter(word => word["ID"] == item.data.replace(",", ""));
            if (column != undefined && column.length != 0) {
                stringContext = stringContext + column[0].Element;
                pf_cols_table.push(column[0]);
            }
        }
    }
    return stringContext;

}
function Render_Template_Border (bordertop, borderleft, borderbottom, borderright) {
    let result = '';
    result = result + ((bordertop != "") ? ('border-top:' + bordertop + ';') : '');
    result = result + ((borderleft != "") ? ('border-left:' + borderleft + ';') : '');
    result = result + ((borderbottom != "") ? ('border-bottom:' + borderbottom + ';') : '');
    result = result + ((borderright != "") ? ('border-right:' + borderright + ';') : '');
    result = ((result != '') ? result : '');
    return result;
}

//function Render_Template_each(obj, elements, tableid, BorderTop, BorderLeft, BorderBottom, BorderRight) {

//    pf_cols_table = [];
//    let stringContext = "";
//    let isRow = 0;
//    let totalcol = 0;
//    stringContext = stringContext + '<tr  role="row">';
//    for (j = 0; j < Object.keys(obj).length; j++) {
//        let item = obj[j];

//        if (item != undefined) {
//            let column = elements.filter(word => word["ID"] == item.data.replace(",", ""));
//            if (column != undefined && column.length != 0) {
//                if (column[0].Type == 'Col' && column[0].Colspan >= 1) {
//                    if (totalcol >= Object.keys(obj).length) {
//                        isRow = 1;
//                        totalcol = Number(column[0].Colspan);
//                    } else {
//                        isRow = 0;
//                        totalcol += Number(column[0].Colspan);
//                    }
//                }
//                if (isRow == 1) {
//                    if (j != 0)
//                        stringContext = stringContext + '</tr><tr role="row">';
//                    else
//                        stringContext = stringContext + '<tr role="row">';
//                }
//                stringContext = stringContext + '<th style="' + item.style + 'Font-weight: bold !important;" colspan="' + column[0].Colspan + '">' + column[0].Element + '</th>'

//                pf_cols_table.push(column[0]);
//            }
//        }
//    }

//    return '<table ' + Render_Template_each_row(BorderTop, BorderLeft, BorderBottom, BorderRight) + '"><thead class="hiddenCollapse">' + stringContext + '</tr></thead><tbody id="' + tableid + '"></tbody></table>';
//}
//function Render_Template_each_row(bordertop, borderleft, borderbottom, borderright) {
//    let result = '';
//    result = result + ((bordertop != "") ? ('border-top:' + bordertop + ';') : '');
//    result = result + ((borderleft != "") ? ('border-left:' + borderleft + ';') : '');
//    result = result + ((borderbottom != "") ? ('border-bottom:' + borderbottom + ';') : '');
//    result = result + ((borderright != "") ? ('border-right:' + borderright + ';') : '');
//    result = ((result != '') ? ('style="' + result + '"') : '');
//    return result;
//}
//#endregion




//#region // Fill DATA
function Fill_Data_Element (elements, dataInfo) {

    if (elements != undefined)
        for (i = 0; i < elements.length; i++) {
            elm = elements[i];
            if (document.getElementById(elm.Name) != undefined) {
                switch (elm.Type) {
                    case 'img':
                        if (dataInfo[elm.Name] != undefined)
                            document.getElementById(elm.Name).setAttribute("src", dataInfo[elm.Name]);
                        break;
                    default:
                        if (dataInfo[elm.Name] != undefined) {
                            if (elm.Name == "Phone") {
                                $('#' + elm.Name).html(dataInfo[elm.Name]);
                                $('#' + elm.Name).attr('data-tab', 'phone_customer');
                                $('#' + elm.Name).addClass('_tab_control_');
                            } else {
                                $('#' + elm.Name).html(dataInfo[elm.Name]);
                            }
                        }

                        break;
                }
            }
        }

    if ($('.barcode'))
        JsBarcode(".barcode").init();

}
function Fill_Data_Row (tableid, data, dtIDRender) {
    $(".rowtable").remove();
    if (tableid != 0) {
        if (dtIDRender && dtIDRender.length > 0) {
            if (data != undefined && data.length != 0) {
                let obj = $("#r_" + tableid)
                for (let i = data.length - 1; i >= 0; i--) {
                    let objclone = obj.clone();
                    if (objclone[0] != undefined) {
                        objclone[0].id = objclone[0].id + '_data_' + i;
                        objclone[0].className = 'content rowtable'
                    }
                    let child = objclone.children();
                    for (ii = 0; ii < child.length; ii++) {
                        let name = child[ii].id;
                        if (data[i][name] != undefined) {
                            child[ii].innerHTML = data[i][name];
                        }
                    }
                    obj.after(objclone);

                }
            }
            else {

            }
        }
    }

}
//#endregion

//#region // Count and paging print form
function Paging_Form (fixid, tableid, containerid, header, body, footer) {
    try {
        let objtable = $('#' + tableid);
        let pt_table = Convert_PX_To_MM(parseInt(objtable.css('paddingTop')));
        let pb_table = Convert_PX_To_MM(parseInt(objtable.css('paddingBottom')));                    //10

        let objheader = $('#' + header);
        let he_header = Convert_PX_To_MM(objheader.height());
        let mt_header = Convert_PX_To_MM(parseInt(objheader.css('paddingTop')));
        let mb_header = Convert_PX_To_MM(parseInt(objheader.css('paddingBottom')));      //2 m

        let objfooter = $('#' + footer);
        let he_footer = Convert_PX_To_MM(objfooter.height());
        let mt_footer = Convert_PX_To_MM(parseInt(objfooter.css('paddingTop')));
        let mb_footer = Convert_PX_To_MM(parseInt(objfooter.css('paddingBottom')));        //3m

        let objbody = $('#' + body);
        let mt_body = Convert_PX_To_MM(parseInt(objbody.css('paddingTop')));
        let mb_body = Convert_PX_To_MM(parseInt(objbody.css('paddingBottom')));

        let objfixtable = $("#r_" + fixid)

        if (objfixtable != undefined) {

            let he_totalofbody = Convert_PX_To_MM(objtable.height()) - he_header - he_footer
                - mt_header - mb_header
                - mt_footer - mb_footer
                - mt_body - mb_body
                - pt_table - pb_table
            // - Convert_PX_To_MM(objfixtable.height());    // 5m

            if (he_totalofbody > 0) {
                let arrayrow = [];
                let indexpage = 1;
                let totalheightrow = 0;
                let _indexh = 0;
                let _row = $(".content");
                for (i = 0; i < _row.length; i++) {
                    if (!_row[i].className.includes('fix')) {
                        _indexh = _indexh + Convert_PX_To_MM(Number($('#' + _row[i].id).height()));
                        if (_indexh >= he_totalofbody - 5) {
                            totalheightrow = totalheightrow + Convert_PX_To_MM(Number($('#' + _row[i].id).height()));
                            if (totalheightrow > he_totalofbody - 5) {
                                indexpage = indexpage + 1;
                                Paging_Form_Generate(containerid, tableid, objheader.clone(), objfooter.clone(), objfixtable.clone(), arrayrow, indexpage);
                                arrayrow = [];
                                totalheightrow = 0;
                            }
                            arrayrow.push($('#' + _row[i].id).clone());
                            $('#' + _row[i].id).remove();
                        }
                    }
                }
                indexpage = indexpage + 1;
                Paging_Form_Generate(containerid, tableid, objheader.clone(), objfooter.clone(), objfixtable.clone(), arrayrow, indexpage);
            }
        }




    }


    catch (ex) {

    }
}
function Paging_Form_Generate (containerid, tableid, header, footer, fixbody, arrayrow, indexpage) {

    let clonetable = $('#' + tableid).clone();
    clonetable.id = tableid + '_' + indexpage;
    clonetable.empty();
    clonetable.append(header);
    //clonetable.append(fixbody);
    for (ii = 0; ii < arrayrow.length; ii++) {
        clonetable.append(arrayrow[ii]);
    }
    clonetable.append(footer);
    $('#' + containerid).append(clonetable);
}

//#endregion

//#region // Print 2022
function fn_Print (printedid, beforefun, afterfun) {
    try {
        let datacss = ['/Print/print.css'
            , '/assests/dist/FontAwesome/css/all.min.css'
            , '/Print/softui.css'
            , '/css/root_element_grid.css'
        ];
        for (let i = 0; i < datacss.length; i++) {
            datacss[i] = datacss[i] + '?ver=' + (new Date()).getTime();
        }        
        let NumCopy = $("#printf_copy").val() != '' && Number($("#printf_copy").val()).toFixed(0) > 0 ? Number($("#printf_copy").val()).toFixed(0) : "1";
        $('#' + printedid).printThis(
            {
                loadCSS: datacss                
                ,canvas: true
                , importCSS: true
                , numCopyPage: NumCopy
                , beforePrint: function () {
                    if (beforefun != undefined) beforefun();
                }
                , afterPrint: function () {
                    if (afterfun != undefined) afterfun();
                }
            });
    }
    catch (ex) { }
}
//#endregion