var TableReponsive;
(function ($) {
    TableReponsive = {
        Refresh: function () {
            try {
                if (optionsobservers == undefined) {
                    optionsobservers = {
                        childList: true
                    };
                }
                CreateObserversMutation_Respon();

            }
            catch (ex) {

            }
        },
        StickyHeader: function () {
            try {
                TableStickyHeader();
            }
            catch (ex) {

            }
        },
        StickyColumn: function (container) {
            try {
                TableStickyColumn(container);
            }
            catch (ex) {

            }
        },
        Handle: function () {
            Table_Responsive();
        }
    }
    //#region // Responsive
    var optionsobservers;
    var observers_responsive;
    function CreateObserversMutation_Respon () {
        if (observers_responsive != undefined && observers_responsive != null) {
            TerminateMutaion();
            Table_Responsive();
        }
        else {
            observers_responsive = new MutationObserver(CallbackMutation_Respon);
        }
        InitMutation();
    }

    function TerminateMutaion () {
        if (observers_responsive != undefined && observers_responsive != null) {
            observers_responsive.disconnect();
        }
    }
    function CallbackMutation_Respon (mutations) {
        Table_Responsive();
    }
    function InitMutation () {
        //Thead Table
        if ($("table").size() > 0) {
            $("table").each(function () {
                let _table_tbody = $(this).find("tbody")
                let _table_tbody_size = $(this).find("tbody").size();
                if (_table_tbody_size > 0) {
                    for (let i = 0; i < _table_tbody_size; i++) {
                        if (observers_responsive != undefined && observers_responsive != null) {
                            observers_responsive.observe(_table_tbody[i], optionsobservers);
                        }
                    }
                }
            });
        }
    }
    //var Table_Array_List_Button = [
    //    "buttonDeleteClass"
    //    , "buttonEditClass"
    //    , "buttonActionClass"
    //    , "buttonPrintClass"
    //    , "buttonSignClassTreatment,"
    //    , "buttonCancelClass"
    //    , "buttonCreateAppTreat"
    //    , "buttonHavingCC"
    //    , "buttonNoneCC"
    //];

    function Table_Responsive () {
        if ($("table").size() > 0) {
            $("table").each(function () {
                let _data_thead = "data-table-thead";

                //Thead Table
                let _table_thead = $(this).find("thead");
                let _table_thead_tr = _table_thead.find("tr");
                let _table_thead_tr_size = _table_thead.find("tr").size();

                //Tbody Table
                let _table_tbody = $(this).find("tbody");
                let _table_tbody_tr = _table_tbody.find("tr");
                let _table_tbody_tr_size = _table_tbody.find("tr").size();

                if (_table_tbody_tr_size > 0) {
                    let _table_thead_tr_num = 0;
                    if (_table_thead_tr_size > 1) {
                        _table_thead_tr_num = _table_thead_tr_size - 1;
                    }

                    //GET Data List Th Table
                    let _table_thead_th = _table_thead_tr.eq(_table_thead_tr_num).find("th");
                    let _table_thead_th_size = _table_thead_th.size();
                    let data_Thead = [];
                    for (let i = 0; i < _table_thead_th_size; i++) {
                        var _head_col_text = _table_thead_th.eq(i).text();
                        data_Thead[i] = _head_col_text;
                    }
                    _table_tbody_tr.each(function (index, element) {
                        let _array_parameter_rowspan = [];
                        let _table_tbody_tr_td_ele = $(this).children("td");
                        _table_tbody_tr_td_ele.each(function (idx, ele) {
                            let _rowspan_td = Number($(this).attr("rowspan"));
                            if (_rowspan_td != undefined && _rowspan_td > 1) {
                                _array_parameter_rowspan.push([index, idx, _rowspan_td]);
                            }
                            if (_array_parameter_rowspan.length > 0) {
                                for (let j = 0; j < _array_parameter_rowspan.length; j++) {
                                    if (_array_parameter_rowspan[j][0] != index) {
                                        if ((_array_parameter_rowspan[j][2] > 0)
                                            && (idx >= _array_parameter_rowspan[j][1])) {
                                            idx++;
                                        }
                                    }
                                }
                            }
                            if (this.hasAttribute("data-table-thead")) {

                            } else {
                                //let _td_content = $(ele).html();
                                //if (_td_content != "") {
                                //    _td_content = `<div class="td-content">${_td_content}</div>`;
                                //    $(ele).html(_td_content);
                                //}
                                $(this).attr(_data_thead, data_Thead[idx] + " : ");
                            }
                            //Table_Responsive_Action_Button_Td($(this));
                        })
                        if (_array_parameter_rowspan.length > 0) {
                            for (let k = _array_parameter_rowspan.length - 1; k >= 0; k--) {
                                _array_parameter_rowspan[k][2] = Number(_array_parameter_rowspan[k][2]) - 1;
                                if (Number(_array_parameter_rowspan[k][2]) == 0) {
                                    _array_parameter_rowspan.splice(-1, 1)
                                }
                            }
                        }
                    });
                }
            });
        }
    }


    //function Table_Responsive_Action_Button_Td(_ele) {
    //    let _table_tbody_tr_td_button = _ele.find(".imgGrid");

    //    if (_table_tbody_tr_td_button.length > 0) {

    //        let StringAppend = "";
    //        _table_tbody_tr_td_button.each(function () {
    //            for (let k = 0; k < Table_Array_List_Button.length; k++) {
    //                if ($(this).hasClass(Table_Array_List_Button[k])) {
    //                    if (($(this).parent().is(":visible") == true && $(this).parent().hasClass("_tab_control_") == true)
    //                        || $(this).parent().hasClass("_tab_control_") == false) {
    //                        StringAppend += Table_Responsive_Get_String_Action(Table_Array_List_Button[k], _ele.find("." + Table_Array_List_Button[k]));
    //                    }
    //                }
    //            }
    //        })
    //        if (StringAppend != "" && _ele.find(".action_responsive").length == 0) {
    //            _ele.children().addClass("displaynone_table");
    //            _ele.append('<div class="action_responsive"><div class="action_responsive_content">' + StringAppend + '</div></div>')
    //        }
    //    };
    //}

    //function Table_Responsive_Get_String_Action(str, ele) {
    //    let result = "";
    //    switch (str) {
    //        case "buttonDeleteClass":
    //            result = '<button class="' + str + ' ui red label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Delete</button>';
    //            break;
    //        case "buttonEditClass":
    //            result = '<button class="' + str + ' ui teal label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Edit</button>';
    //            break;
    //        case "buttonActionClass":
    //            result = '<button class="' + str + ' ui blue label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Take Care</button>';
    //            break;
    //        case "buttonPrintClass":
    //            result = '<button class="' + str + ' ui green label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Print</button>';
    //            break;
    //        case "buttonSignClassTreatment":
    //            result = '<button class="' + str + ' ui green label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Sign</button>';
    //            break;
    //        case "buttonCancelClass":
    //            result = '<button class="' + str + ' ui red label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Cancel</button>';
    //            break;
    //        case "buttonCreateAppTreat":
    //            result = '<button class="' + str + ' ui blue label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Creat Appointment</button>';
    //            break;
    //        case "buttonNoneCC":
    //            result = '<button class="' + str + ' ui blue label" ' + Table_Responsive_Get_All_Attributes_Element(ele) + ' >Not Yet Customer Care</button>';
    //            break;
    //        case "buttonHavingCC":
    //            result = '<i style="width: 30px;height: 30px;margin: 0;" class="MLU-icon MLUech-icon-cctreat"></i>';
    //            break;
    //        default:
    //            break;
    //    }
    //    return result;
    //}

    //function Table_Responsive_Get_All_Attributes_Element($node) {
    //    let result = ""
    //    let data_attr = "data-";
    //    $.each($node[0].attributes, function (index, attribute) {
    //        if ((attribute.name).includes(data_attr) == true) {
    //            result += ' ' + attribute.name + '="' + attribute.value + '" ';
    //        }
    //    });
    //    return result;
    //}
    //#endregion
    //#region // Responsive
    function TableStickyHeader () {
        let $ele = $(".vt-header-sticky.vt-hs-height");
        let $eleparent = $ele.closest('.vt-header-parent').length > 0 ? $ele.closest('.vt-header-parent') : $ele.parent();
        let windowHeight = window.innerHeight;
        let posTop = !isNaN(Number($ele.offset()?.top)) ? Number($ele.offset()?.top) : 0;
        let padParrentEle = !isNaN(Number($eleparent.innerHeight() - $eleparent.height()))
            ? Number($eleparent.innerHeight() - $eleparent.height()) : 0;
        let padEle = !isNaN(Number($ele.innerHeight() - $ele.height()))
            ? Number($ele.innerHeight() - $ele.height()) : 0;
        let marParrentEle = $eleparent.css("margin-top")?.replace(/[^\d.-]+/g, '');
        marParrentEle = !isNaN(Number(marParrentEle)) ? Number(marParrentEle) : 0;
        let marEle = $ele.css("margin-top")?.replace(/[^\d.-]+/g, '');
        marEle = !isNaN(Number(marEle)) ? Number(marEle) : 0;
        let totalHeight = Math.abs(padParrentEle) + Math.abs(padEle) + Math.abs(marParrentEle) + Math.abs(marEle);
        if (posTop < windowHeight / 2) {
            totalHeight += posTop;
        }
        if(sys_isMobile == 0) $ele.css("height", `calc(100vh - ${totalHeight}px)`, 'important')
    }

    async function TableStickyColumn(container) {
        new Promise(resolve => {
            try {
                let $tables = container != undefined ? container : $('.MLUech-fixedcol').closest('table');
                if ($tables && $tables.length > 0) {
                    $tables.each(function () {
                        if ($(this)[0] != undefined) {
                            let $ele = $(this).find(".MLUech-fixedcol");
                            let rectTable = $(this)[0].getBoundingClientRect();
                            let posLeft = rectTable?.left ?? 0;
                            if ($ele && $ele.length > 0) {
                                var promises = [];
                                let blockNum = $ele.length > 30 ? Math.ceil($ele.length / 3) : $ele.length;
                                let array = sliceIntoChunks($ele, blockNum);
                                if (array != undefined && array.length != 0) {
                                    for (var i = 0; i < array.length; i++) {
                                        promises.push(TableStickColEach(array[i], posLeft));
                                    }
                                    Promise.all(promises);
                                }
                            }
                        }
                        else {

                        }
                    });
                }
            }
            catch (e) {
                console.log(e)
            }
            resolve();
        })
    }
    async function TableStickColEach(data, posLeft) {
        new Promise(res => {
            for (let _i = 0; _i < data.length; _i++) {
                let item = $(data[_i]);
                let rect = item[0]?.getBoundingClientRect() ?? {};
                let rectLeft = !isNaN(Number(rect?.left ?? 0)) ? Number(rect?.left ?? 0) : 0;
                let distPosLeft = (rectLeft - posLeft) > 0 ? (rectLeft - posLeft) : 0;

                setTimeout(() => {
                    item.css("left", `${distPosLeft}px`);
                }, 10)
            }
            res();
        })

    }

    //#endregion
})(jQuery);