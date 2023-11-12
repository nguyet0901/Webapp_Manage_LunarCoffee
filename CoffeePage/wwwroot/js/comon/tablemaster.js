(function ($) {
    $.fn.TableView = function (param) {
        var defaults = {
            row: "tbody tr",
            event: "click",
            colspan: 0,
            attr: "data-id",
            colorrow: "#e2f2ff61",
            forceSelection: false, //hiện 1 detail false, hiện nhiều true
        };
        var settings = $.extend({}, defaults, param);
        var table = this;

        table.init = function () {
            settings.table = this;
            Table_View_Click_Row();
        };
        function Table_View_Click_Row() {
            let _table = $(settings.table);
            if (settings.forceSelection) {
                _table.unbind(settings.event).on(settings.event, settings.row, function () {
                    let attr_row = $(this).attr(settings.attr);
                    if (attr_row && attr_row != undefined) {
                        let classrow = "table_row_view_" + attr_row;
                        if (_table.find("." + classrow).length == 0) {
                            let tr_ele = $('<tr ></tr>');
                            tr_ele.addClass("table_row_view_" + attr_row);
                            Table_View_Count_Colspan();
                            let td_ele = $('<td colspan="' + settings.colspan + '" style="padding:0 10px !important;background:' + settings.colorrow + '"></td>');
                            tr_ele.append(td_ele);
                            tr_ele.insertAfter($(this));
                            if (typeof settings.content !== 'undefined' && $.isFunction(settings.content)) {
                                settings.content(attr_row, td_ele);
                            }
                        }
                        else {
                            _table.find("." + classrow).remove();
                        }
                    }
                })
            }
            else {
                _table.unbind(settings.event).on(settings.event, settings.row, function () {
                    let attr_row = $(this).attr(settings.attr);
                    if (attr_row && attr_row != undefined) {
                        let classrow = "table_row_view_" + attr_row;
                        if (_table.find("." + classrow).length == 0) {
                            _table.find(".table_row_view").remove();
                            let tr_ele = $('<tr ></tr>');
                            tr_ele.addClass("table_row_view table_row_view_" + attr_row);
                            Table_View_Count_Colspan();
                            let td_ele = $('<td colspan="' + settings.colspan + '" style="padding:0 10px !important;background:' + settings.colorrow + '"></td>');
                            tr_ele.append(td_ele);
                            tr_ele.insertAfter($(this));
                            if (typeof settings.content !== 'undefined' && $.isFunction(settings.content)) {
                                settings.content(attr_row, td_ele);
                            }
                        }
                        else {
                            _table.find("." + classrow).remove();
                        }
                    }
                })
            }
        }
        //tính colspan của table
        function Table_View_Count_Colspan() {
            let _table = $(settings.table);
            if (settings.colspan == 0) {
                let th = _table.find("thead th");
                let length_col_span = 0;
                th.each(function () {
                    if ($(this).is(":visible")) {
                        let col = Number($(this).attr("colspan"));
                        if (!Number.isNaN(col) && col != undefined && col != 0)
                            length_col_span += col;
                        else length_col_span++;
                    }
                })
                settings.colspan = length_col_span;
            }
        }
        table.init();
    };
})(jQuery);