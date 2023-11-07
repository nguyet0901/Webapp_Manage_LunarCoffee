//#region //Show More
//id: id của danh sách checkbox
//classColumn: class của checkbox
//attribute: Giá trị thuộc tính muốn lấy của checkbox
//tab: Tên phân biệt
//nameCookie:  Tên Cookie
//colthead: id hoặc class của thead table
//coltd: id hoặc class của td table

function Checking_Column_ShowMore(id, classColumn, attribute, tab, nameCookie, colthead, coltd) {
    let Data_Show_More_All_Tab;
    let url_PathName = window.location.pathname;
    let isExist_Show_More = 0;
    if (Cookies.get(nameCookie) != undefined) {
        Data_Show_More_All_Tab = JSON.parse(Cookies.get(nameCookie));
        if (Data_Show_More_All_Tab.length != 0) {
            for (let i = 0; i < Data_Show_More_All_Tab.length; i++) {
                if (Data_Show_More_All_Tab[i].url == url_PathName && Data_Show_More_All_Tab[i].tab == tab) {
                    let items = Data_Show_More_All_Tab[i].content;
                    for (let j = 0; j < items.length; j++) {
                        let col = items[j].col;
                        let isChecked = items[j].checked;
                        if (isChecked == true) {
                            $(colthead + col).show();
                            $(coltd + col).show();
                            $('#' + id + ' .' + classColumn + '[' + attribute + '= "' + col + '"]').prop("checked", true);
                        }
                        else {
                            $(colthead + col).hide();
                            $(coltd + col).hide();
                            $('#' + id + ' .' + classColumn + '[' + attribute + '= "' + col + '"]').prop("checked", false);
                        }
                    }
                    isExist_Show_More = 1;
                    break;
                }
            }
            if (isExist_Show_More == 0) {
                Hide_Default_Checked_ShowMore(classColumn, attribute, colthead, coltd);
            }
        }

    }
    else {
        Hide_Default_Checked_ShowMore(classColumn, attribute, colthead, coltd);
    }
}

function Onchange_Checked_ShowMore(classColumn, attribute, tab, nameCookie, colthead, coltd) {
    $("." + classColumn).on('click', function (event) {
        let url_PathName = window.location.pathname;
        let Json_Show_More_All_Tab = [];
        let NumberColumn = $(this).attr(attribute);
        let isChecked = false;
        let isExistCheckbox = 0;

        if ($(this).is(":checked")) {
            isChecked = true;
            $(colthead + NumberColumn).show();
            $(coltd + NumberColumn).show();
        }
        else {
            $(colthead + NumberColumn).hide();
            $(coltd + NumberColumn).hide();
        }

        //Kiểm tra Cookie tồn tại chưa. Thêm Cookie hoặc tab chưa có thì thêm mới
        if (Cookies.get(nameCookie) != undefined) {
            Json_Show_More_All_Tab = JSON.parse(Cookies.get(nameCookie));
            let isExist = 0;
            for (let i = 0; i < Json_Show_More_All_Tab.length; i++) {
                if (Json_Show_More_All_Tab[i].url == url_PathName && Json_Show_More_All_Tab[i].tab == tab) {
                    isExist = 1;
                }
            }
            if (isExist == 0) {
                Json_Show_More_All_Tab.push(Import_Data_ShowMore(classColumn, attribute, tab));
            }
        }
        else {
            Json_Show_More_All_Tab.push(Import_Data_ShowMore(classColumn, attribute, tab));
        }

        //Thay đỗi giá trị vừa checked
        for (let i = 0; i < Json_Show_More_All_Tab.length; i++) {
            if (Json_Show_More_All_Tab[i].url == url_PathName && Json_Show_More_All_Tab[i].tab == tab) {
                let items = Json_Show_More_All_Tab[i].content;
                for (let j = 0; j < items.length; j++) {
                    if (items[j].col == NumberColumn) {
                        items[j].checked = isChecked;
                        isExistCheckbox = 1;
                        break;
                    }
                }
                if (isExistCheckbox == 0) {
                    items.push({
                        "col": NumberColumn,
                        "checked": isChecked
                    });
                }
                break;
            }
        }
        Cookies.set(nameCookie, JSON.stringify(Json_Show_More_All_Tab));
    });
}

function Import_Data_ShowMore(classColumn, attr, tab) {
    let url_PathName = window.location.pathname;
    let Data_ShowMore = {};
    Data_ShowMore.url = url_PathName;
    Data_ShowMore.tab = tab;
    Data_ShowMore.content = [];

    let Checkbox = document.getElementsByClassName(classColumn);
    for (let i = 0; i < Checkbox.length; i++) {
        let isCheck = false;
        let col = Checkbox[i].getAttribute(attr);
        if (Checkbox[i].checked == true) {
            isCheck = true;
        }
        Data_ShowMore.content.push({
            "col": col,
            "checked": isCheck
        });
    }
    return Data_ShowMore;
}

function Hide_Default_Checked_ShowMore(classColumn, attribute, colthead, coltd) {
    let checkbox = document.getElementsByClassName(classColumn);
    if (checkbox != null && checkbox != undefined) {
        for (i = 0; i < checkbox.length; i++) {
            let value = checkbox[i].getAttribute(attribute);
            if (checkbox[i].checked == false) {
                $(colthead + value).hide();
                $(coltd + value).hide();
            }
            else {
                $(colthead + value).show();
                $(coltd + value).show();
            }
        }
    }
}

//#region appointment in day: chọn kiểu show dữ liệu
function Change_TypeShow_Appointment(nameCookie, tab, type) {
    let url_PathName = window.location.pathname;
    let Json_Type_Show_All_Tab = [];
    //Kiểm tra Cookie tồn tại chưa. Thêm Cookie hoặc tab chưa có thì thêm mới
    if (Cookies.get(nameCookie) != undefined) {
        Json_Type_Show_All_Tab = JSON.parse(Cookies.get(nameCookie));
        let isExist = 0;
        for (let i = 0; i < Json_Type_Show_All_Tab.length; i++) {
            if (Json_Type_Show_All_Tab[i].url == url_PathName && Json_Type_Show_All_Tab[i].tab == tab) {
                isExist = 1;
            }
        }
        if (isExist == 0) {
            Json_Type_Show_All_Tab.push(Import_Data_TypeShow(type, tab));
        }
    }
    else {
        Json_Type_Show_All_Tab.push(Import_Data_TypeShow(type, tab));
    }

    //Thay đỗi giá trị vừa checked
    for (let i = 0; i < Json_Type_Show_All_Tab.length; i++) {
        if (Json_Type_Show_All_Tab[i].url == url_PathName && Json_Type_Show_All_Tab[i].tab == tab) {
            Json_Type_Show_All_Tab[i].type = type;
            break;
        }
    }
    Cookies.set(nameCookie, JSON.stringify(Json_Type_Show_All_Tab));
}
function Import_Data_TypeShow(type, tab) {
    let url_PathName = window.location.pathname;
    let Data_TypeShow = {};
    Data_TypeShow.url = url_PathName;
    Data_TypeShow.tab = tab;
    Data_TypeShow.type = type;
    return Data_TypeShow;
}

//function Checking_Appointment_TypeShow(nameCookie, tab) {
//    let type_show = 1;
//    let url_PathName = window.location.pathname;
//    if (Cookies.get(nameCookie) != undefined) {
//        let Json_Type_Show = JSON.parse(Cookies.get(nameCookie));
//        if (Json_Type_Show && Json_Type_Show.length > 0) {
//            for (let i = 0; i < Json_Type_Show.length; i++) {
//                if (Json_Type_Show[i].url == url_PathName && Json_Type_Show[i].tab == tab) {
//                    type_show = Json_Type_Show[i].type;
//                    break;
//                }
//            }
//        }
//    }
//    return type_show;
//}
        //#endregion
////#endregion

(function ($) {

    $.fn.TableExpandColumn = function (param) {
        var base = this;
        var settings = $.extend({}, param);
         
        base.init = function () {
            settings.Table = this;
            TableExpandColumn_Event();
            TableExpandColumn_Ini ()
        };
        base.init();

        function TableExpandColumn_Ini () {

            let $table = $(settings.Table);
            let $itemsToogle = $(settings.shtoogle);

            if ($table.length > 0 && $itemsToogle.length > 0) {
                if ($table[0] != undefined) {
                    let _spename = ($table.attr('data-name') != undefined && $table.attr('data-name') != "") ? $table.attr('data-name') : $table[0].id;
       
                    let _storage = Master_Setting_Get(_spename);
                    let _savedsh = {};
                    if (_storage != '' && _storage != null) _savedsh = JSON.parse(_storage);
                    for (let _i = 0; _i < $itemsToogle.length; _i++) {
                        let _dataname = $itemsToogle[_i].dataset.name;
                        if (_savedsh[_dataname] == true) {
                            $itemsToogle[_i].checked = true;
                        }
                    }

                    TableExpandColumn_Action();
                }
                else {

                }
              
            }

        }
        function TableExpandColumn_Action () {
            let $table = $(settings.Table);
            let $itemsToogle = $(settings.shtoogle);
            if ($table.length > 0 && $itemsToogle.length > 0) {
                for (let _i = 0; _i < $itemsToogle.length; _i++) {
                    let _dataname = $itemsToogle[_i].dataset.name;
                    let $th = $table.find("thead").find('th[data-name="' + _dataname + '"]');
                    let $td = $table.find("tbody").find('td[data-name="' + _dataname + '"]');
                    let $specific = $table.find('.toogleitem[data-name="' + _dataname + '"]');
                    if ($th.length > 0) {
                        if ($itemsToogle[_i].checked) $th.show();
                        else $th.hide();
                    }
                    if ($td.length > 0) {
                        if ($itemsToogle[_i].checked) $td.show();
                        else $td.hide();
                    }
                    if ($specific.length > 0) {
                        if ($itemsToogle[_i].checked) $specific.show();
                        else $specific.hide();
                    }
                }
            }
        }
        function TableExpandColumn_Event () {
            let $itemsToogle = $(settings.shtoogle);
            $itemsToogle.unbind('change').change(function (event) {
                let $table = $(settings.Table);
                if ($table.length > 0) {
                    let _spename = ($table.attr('data-name') != undefined && $table.attr('data-name') != "") ? $table.attr('data-name') : $table[0].id;

                    let _dataname = $(this).attr('data-name');
                    let $th = $table.find("thead").find('th[data-name="' + _dataname + '"]');
                    let $td = $table.find("tbody").find('td[data-name="' + _dataname + '"]');
                   let $specific = $table.find('.toogleitem[data-name="' + _dataname + '"]');

                    TableExpandColumn_SetStorage(_spename, _dataname, $(this).is(":checked"));

                    if ($th.length > 0) {
                        if ($(this).is(":checked")) $th.show();
                        else $th.hide();
                    }
                    if ($td.length > 0) {
                        if ($(this).is(":checked")) $td.show();
                        else $td.hide();
                    }

                     if ($specific.length > 0) {
                        if ($(this).is(":checked")) $specific.show();
                        else $specific.hide();
                    }
                }
            });
        }
        function TableExpandColumn_SetStorage (n, k, v) {
            let value = Master_Setting_Get(n);
            let __v = {};
            if (value != '' && value != null) {
                __v = JSON.parse(value);
                __v[k] = v;
            }
            else {
                __v[k] = v;
            }
            Master_Setting_Set(n, JSON.stringify(__v));
        }
        var TableExpandColumn = {
            Refresh: function () {
                TableExpandColumn_Action();
            }
        };
        return TableExpandColumn;
    };
})(jQuery);