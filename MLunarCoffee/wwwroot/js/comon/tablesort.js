/*
	A simple, lightweight jQuery plugin for creating sortable tables.
	https://github.com/kylefox/jquery-tablesort
	Version 0.0.11
*/

(function ($) {
    $.tablesort = function ($table, settings) {
        var self = this;
        this.$table = $table;
        this.$thead = this.$table.find('thead');
        this.settings = $.extend({}, $.tablesort.defaults, settings);
        this.$sortCells = this.$thead.length > 0 ? this.$thead.find('th:not(.no-sort)') : this.$table.find('th:not(.no-sort)');
        this.$sortCells.on('click.tablesort', function () {
            try {
                let _tablesortclassname = this.parentNode.offsetParent.className;
                if (_tablesortclassname.includes('nosortwholetable'))
                    return false;
                else {
                    //if (LanguageMLU)
                    //    LanguageMLU.Terminate();
                    self.sort($(this));
                }
            }
            catch (ex) {

            }

        });
        this.index = null;
        this.$th = null;
        this.direction = null;
    };

    function removeAccents(str) {
        var AccentsMap = [
            "aàảãáạăằẳẵắặâầẩẫấậ",
            "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
            "dđ", "DĐ",
            "eèẻẽéẹêềểễếệ",
            "EÈẺẼÉẸÊỀỂỄẾỆ",
            "iìỉĩíị",
            "IÌỈĨÍỊ",
            "oòỏõóọôồổỗốộơờởỡớợ",
            "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
            "uùủũúụưừửữứự",
            "UÙỦŨÚỤƯỪỬỮỨỰ",
            "yỳỷỹýỵ",
            "YỲỶỸÝỴ"
        ];
        for (var i = 0; i < AccentsMap.length; i++) {
            var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
            var char = AccentsMap[i][0];
            str = str.replace(re, char);
        }
        return str;
    }

    $.tablesort.prototype = {

        sort: function (th, direction) {
            var start = new Date(),
                self = this,
                table = this.$table,
                rowsContainer = table.children('tbody').length > 0 ? table.children('tbody') : table,
                rows = rowsContainer.children('tr').has('td, th'),
                cells = rows.children(':nth-child(' + (th.index() + 1) + ')').filter('td, th'),
                sortBy = th.data().sortBy,
                sortedMap = [];
            table.addClass("sort-pending")
            var unsortedValues = cells.map(function (idx, cell) {
                if (sortBy)
                    return (typeof sortBy === 'function') ? sortBy($(th), $(cell), self) : sortBy;
                else if (!isNaN($(this).text())) return Number($(this).text());
                return ($(this).data().sortvalue != null ? removeAccents($(this).data().sortvalue.toString()) : removeAccents($(this).text()));
            });
            if (unsortedValues.length === 0) return;

            //click on a different column
            if (this.index !== th.index()) {
                this.direction = 'asc';
                this.index = th.index();
            }
            else if (direction !== 'asc' && direction !== 'desc')
                this.direction = this.direction === 'asc' ? 'desc' : 'asc';
            else
                this.direction = direction;

            direction = this.direction == 'asc' ? 1 : -1;

            //self.$table.trigger('tablesort:start', [self]);
            //self.log("Sorting by " + this.index + ' ' + this.direction);

            // Try to force a browser redraw
            self.$table.css("display");
            // Run sorting asynchronously on a timeout to force browser redraw after
            // `tablesort:start` callback. Also avoids locking up the browser too much.
            setTimeout(function () {
                self.$sortCells.removeClass(self.settings.asc + ' ' + self.settings.desc);
                for (let i = 0, length = unsortedValues.length; i < length; i++) {
                    sortedMap.push({
                        index: i,
                        cell: cells[i],
                        row: rows[i],
                        value: unsortedValues[i]
                    });
                }

                sortedMap.sort((a, b) => {
                    return self.settings.compare(a.value, b.value) * direction;
                })
                const rendertable = async (item) => {
                    setTimeout(() => {
                        for (let j = 0; j < item.length; j++) {
                            rowsContainer.append($(item[j].row));
                        }
                    }, 10);
                }

                var array = sliceIntoChunks(sortedMap, 100);
                if (array != undefined && array.length != 0) rendertable(array[0]);
                for (let i = 1; i < array.length; i++) {
                    rendertable(array[i]);
                }
                th.addClass(self.settings[self.direction]);
                self.$table.css("display");
                //if (LanguageMLU)
                //    LanguageMLU.Initialize();
                table.removeClass("sort-pending")
            }, unsortedValues.length > 2000 ? 300 : 100);
        },

        log: function (msg) {
            if (($.tablesort.DEBUG || this.settings.debug) && console && console.log) {
                console.log('[tablesort] ' + msg);
            }
        },

        destroy: function () {
            this.$sortCells.off('click.tablesort');
            this.$table.data('tablesort', null);
            return null;
        }

    };

    $.tablesort.DEBUG = false;

    $.tablesort.defaults = {
        debug: $.tablesort.DEBUG,
        asc: 'sorted ascending',
        desc: 'sorted descending',
        compare: function (a, b) {
            if (a > b) {
                return 1;
            } else if (a < b) {
                return -1;
            } else {
                return 0;
            }
        }
    };

    $.fn.tablesort = function (settings) {
        var table, sortable, previous;
        return this.each(function () {
            table = $(this);
            previous = table.data('tablesort');
            if (previous) {
                previous.destroy();
            }
            table.data('tablesort', new $.tablesort(table, settings));
        });
    };

})(window.Zepto || window.jQuery);


(function ($) {
    $.tablecontrol = function ($table, settings) {
        var self = this;
        this.$table = $table;
        this.$thead = this.$table.find('thead');
        this.settings = $.extend({}, $.tablecontrol.defaults, settings);
        this.$sortCells = this.$thead.length > 0 ? this.$thead.find('th.MLU-filter') : null;
        this.$sortCells.on('click.tablecontrol', function (e) {
            let _cell = $(this);
            let _index = _cell.index();
            self.popup(_cell, _index);
        });
        this.index = null;
        this.$th = null;
        this.direction = null;
    };

    $.tablecontrol.prototype = {
        popup: function (th) {

            var self = this,
                table = this.$table
                thindex = th.index() + 1;
            let thindex1 = th.index() + 1; 
            let theadall = th.parent().children('th');
            for (let j = 0; j < thindex1; j++) {
                if (theadall[j] != undefined && theadall[j].colSpan != undefined && theadall[j].colSpan > 1) {
                    thindex += (theadall[j].colSpan - 1);
                }
            } 
            self.settings.thindexcurr = thindex;
            self.outClick();
            let { left, top } = th.offset();
            let _popup = document.createElement("div"),
                _popupHeader = document.createElement("div"),
                _popupBody = document.createElement("div"),
                _popupFooter = document.createElement("div");
            _popup.className = 'card z-index-sticky col-w-300 rounded-3 position-absolute text-sm p-2 pt-3 shadow-lg shadow-xl ' + self.settings.class.popup;
            _popup.style.right = ($(window).width() - left - th.outerWidth()) + 'px';
            _popup.style.top = (top + th.outerHeight()) + 'px';
            _popupHeader.className = 'card-hearder py-0 px-2';
            _popupBody.className = 'card-body py-2 px-2 text-start rounded-2 max-height-250 overflow-auto';
            _popupFooter.className = 'mt-2 d-flex justify-content-end';
            _popupFooter.innerHTML = `
                    <button class="btn btn-sm badge bg-secondary ms-2 mb-0 btnfilterunique_close">Đóng</button>
                    <button class="btn btn-sm badge bg-gradient-primary ms-2 mb-0 btnfilterunique_ok">OK</button>
                `
            _popupHeader.innerHTML = `
                    <div class="mb-2">
                        <div class="input-group">
                        <input id="inputfilterunique" class="form-control" type="text" placeholder="Tìm kiếm">
                        <div class="input-group-text">
                            <i class="text-sm px-1 fas fa-sort-alpha-down cursor-pointer filteruniquesort" data-sort="asc"></i>
                            <i class="text-sm px-1 fas fa-sort-alpha-up cursor-pointer filteruniquesort" data-sort="desc"></i>
                        </div>
                    </div>

                    </div>
                    <div id="filterwaiting" class="position-absolute start-50 text-center translate-middle-x waitingdiv z-index-3">
                        <div class="spinner-border text-primary" role="status">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>
                `
            self.settings.popup = {
                table: table,
                popup: _popup,
                popupHeader: _popupHeader,
                popupBody: _popupBody,
                popupFooter: _popupFooter
            };
            _popup.append(_popupHeader);
            _popup.append(_popupBody);
            _popup.append(_popupFooter);
            $("#" + self.settings.bodyappend).append(_popup);

            self.isComplete(false);
            setTimeout(() => {
                let rowsContainer = table.children('tbody').length > 0 ? table.children('tbody') : table,
                    rows = rowsContainer.children('tr').has('td, th'),
                    cells = rows.children(':nth-child(' + thindex + ')').filter('td, th');
                self.rows = rows;
                self.eventHandle(self.settings.popup, thindex);
                if (cells.length != 0) {
                    var dataunique = self.uniqueCells(cells);
                    self.datafilter = dataunique;
                    self.settings.fillthead = self.settings.thindex[thindex] ? 0 : 1;
                    self.render(_popupBody, Object.values(dataunique));
                }
                else {
                    self.isComplete(true);
                }
            }, 10);
        },
        reRender: function (cells, _popupBody) {
            var self = this;
            var dataunique = self.uniqueCells(cells);
            self.datafilter = dataunique;
            self.render(_popupBody, Object.values(dataunique));
        },
        outClick: function () {
            var self = this;
            $(document).mouseup(function (e) {
                let popup = $(self.settings.popup.popup);
                if (!popup.is(e.target) && popup.has(e.target).length === 0) {
                    self.removePopup();
                }
            });
        },
        uniqueCells: function (cells) {
            let dataCell = {};
            cells.map((idx, cell) => {
                let nameCell = cell.textContent;
                if (nameCell == '') nameCell = '(blank)'
                if (dataCell[nameCell]) {
                    let item = dataCell[nameCell].Cell;
                    item.push(idx);
                }
                if (cell && nameCell != undefined && !dataCell[nameCell]) {
                    let e = {};
                    e.Name = nameCell
                    e.Cell = [];
                    e.Cell.push(idx);
                    dataCell[nameCell] = e;
                }
            });
            let idx = 1;
            return Object.values(dataCell)
                .sort((a, b) => { return a.Name - b.Name })
                .reduce((pre, arr) => {
                    arr.key = idx;
                    pre[idx] = arr;
                    idx++;
                    return pre;
                }, {});
        },
        render: async function (ele, data, isSearch = false) {
            setTimeout(() => {
                let self = this;
                let isChecked = self.settings.fillthead == 1 ? `checked="checked"` : '';
                let stringHeader = ``;
                let stringContent = ``;
                ele.innerHTML = '';
                if (!isSearch) {
                    stringHeader = `
                        <div class="form-check rowfilteruniqueall">
                                <input id="filterunique_0" value="0" class="filteruniqueall form-check-input pr-2" type="checkbox" ${isChecked}>
                            <label class="custom-control-label mb-0" style="vertical-align: bottom;" for="filterunique_0">${Outlang["Sys_tat_ca"]}</label>
                        </div>
                    `
                }
                else {
                    stringHeader = `
                        <div class="form-check rowfilteruniqueall">
                                <input id="filterunique_-1" value="-1" class="filterunique filteruniqueall form-check-input pr-2" type="checkbox" ${isChecked}>
                            <label class="custom-control-label mb-0" style="vertical-align: bottom;" for="filterunique_-1">
                                 Tất cả kết quả tìm kiếm
                            </label>
                        </div>
                    `
                }

                for (let i = 0; i < data.length; i++) {
                    let item = data[i];
                    let cellLength = (item.Cell).length;
                    if (item.Name == '(blank)') {
                        stringHeader += `
                            <div class="rowfilterunique form-check">
                                <input id="filterunique_${item.key}" value="${item.key}" class="filterunique form-check-input pr-2" type="checkbox" ${isChecked}>
                                <label class="custom-control-label mb-0" style="vertical-align: bottom;" for="filterunique_${item.key}">
                                        ${item.Name} <span class="text-danger">(${cellLength})</span>
                                </label>
                            </div>
                        `
                    }
                    else { 
                        stringContent += `
                            <div class="rowfilterunique form-check">
                                <input id="filterunique_${item.key}" value="${item.key}" class="filterunique form-check-input pr-2" type="checkbox" ${isChecked}>
                                <label class="custom-control-label mb-0" style="vertical-align: bottom;" for="filterunique_${item.key}">
                                        ${item.Name} <span class="text-danger">(${cellLength})</span>
                                </label>
                            </div>
                        `
                    }
                }
                ele.innerHTML = stringHeader + stringContent;
                self.fillthead();
                self.isComplete(true);
                self.eventClick(ele);
            }, 10)
        },
        fillthead: function () {
            let self = this;
            if (self.settings.fillthead == 0) {
                let thfill = self.settings.thindex[self.settings.thindexcurr]
                let pupopBody = $(self.settings.popup.popupBody);
                let itemAll = pupopBody.find('#filterunique_0');
                if (thfill.includes(0)) {
                    itemAll.prop('checked', true).trigger('change');
                }
                else {
                    for (let i = 0; i < thfill.length; i++) {
                        let item = thfill[i];
                        let itemChk = pupopBody.find('.filterunique[value="' + item + '"]');
                        itemAll.parent().after($(itemChk).parent());
                        itemChk.prop('checked', true).trigger('change');
                    }
                }
                self.settings.fillthead = 1;
            }
        },
        isComplete: function (isBool = false) {
            if (isBool) {
                $("#filterwaiting").hide();
            }
            else {
                $("#filterwaiting").show();
            }
        },
        eventClick: function (ele) {
            $(ele).find('.filterunique:not(.filteruniqueall)').unbind('change').change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                let isChecked = $(this).is(":checked");
                if (!isChecked && $(ele).find('.filteruniqueall').is(":checked")) {
                    $(this).parent().siblings().children('.filteruniqueall').prop("checked", false);
                }
            });
            $(ele).find('.filteruniqueall').unbind('change').change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                let isAll = $(this).is(":checked");
                $(this).parent().siblings().children('.filterunique').prop("checked", isAll);
            });
        },
        eventHandle: function (args, index) {
            let self = this;
            let timeOutSearch;
            $(args.popupFooter).on('click', '.btnfilterunique_ok', function (e) {
                e.preventDefault();
                e.stopPropagation();
                let arr = [];
                if ($(args.popupBody).find('#filterunique_0').is(':checked')) {
                    arr.push(0);
                }
                else {
                    $(args.popupBody).find('.filterunique').each((function () {
                        if ($(this).is(':checked')) {
                            arr.push(Number($(this).val()));
                        }
                    }));
                }
                self.settings.thindex[index] = arr;
                self.filter(arr)
            });
            $(args.popupFooter).on('click', '.btnfilterunique_close', function (e) {
                e.preventDefault();
                e.stopPropagation();
                self.removePopup();
            });
            $(args.popupHeader).on('keyup', '#inputfilterunique', function (e) {
                e.preventDefault();
                e.stopPropagation();
                let input = $(this);
                clearTimeout(timeOutSearch);
                timeOutSearch = setTimeout(() => {
                    self.searchUnique();
                }, 300)
            });
            $(args.popupHeader).find('.filteruniquesort').unbind('click').on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                self.isComplete(false);
                $(this).addClass('active text-primary').siblings('.filteruniquesort').removeClass('active text-primary');
                timeOutSearch = setTimeout(() => {
                    self.searchUnique();
                }, 100);
            });
        },
        searchUnique: function () {
           
            let self = this;
            let dataFill = [];
            let popup = self.settings.popup;
            let popupHeader = $(popup.popupHeader);
            let input = popupHeader.find("#inputfilterunique");
            let isSearch = false;
            let textsearch = xoa_dau(input.val()).trim().toLocaleLowerCase();
            let typesort = popupHeader.find('.filteruniquesort.active').length
                ? popupHeader.find('.filteruniquesort.active').attr('data-sort')
                : 'asc';
            if (textsearch == '') {
                dataFill = Object.values(self.datafilter);
            }
            else {
                dataFill = Object.values(self.datafilter).filter((item) => {
                    return (xoa_dau(item.Name).toLocaleLowerCase()).includes(textsearch)
                });
                isSearch = true;
            }
            if (typesort == 'asc') {
                dataFill = dataFill.sort((a, b) => {
                    if (a.Name < b.Name) {
                        return -1;
                    }
                    if (a.Name > b.Name) {
                        return 1;
                    }
                    return 0;
                });
            }
            else {
                dataFill = dataFill.sort((a, b) => {
                    if (a.Name > b.Name) {
                        return -1;
                    }
                    if (a.Name < b.Name) {
                        return 1;
                    }
                    return 0;
                })
            }
            self.render(popup.popupBody, dataFill, isSearch);
        },
        removePopup: function () {
            let self = this;
            self.settings.popup.popup.remove();
        },
        filter: function (arr) {
            let self = this;
            if (arr.lenght != 0) {
                let isCheckAll = arr.includes(0)
                if (isCheckAll) {
                    $(self.rows).show()
                }
                else {
                    $(self.rows).hide()
                    for (let i = 0; i < arr.length; i++) {
                        if (arr[i] != '-1') {
                            let dataFilter = self.datafilter[arr[i]];
                            let cell = dataFilter.Cell;
                            for (let j = 0; j < cell.length; j++) {
                                $(self.rows[cell[j]]).show();
                            }
                        }
                    }
                }
            }
        },
        log: function (msg) {
            if (($.tablecontrol.DEBUG || this.settings.debug) && console && console.log) {
                console.log('[tablecontrol] ' + msg);
            }
        },
        destroy: function () {
            this.$sortCells.off('click.tablecontrol');
            this.$table.data('tablecontrol', null);
            return null;
        }
    };

    $.tablecontrol.DEBUG = false;

    $.tablecontrol.defaults = {
        bodyappend: 'BodyMainOther',
        debug: $.tablecontrol.DEBUG,
        asc: 'sorted ascending',
        desc: 'sorted descending',
        compare: function (a, b) {
            if (a > b) {
                return 1;
            } else if (a < b) {
                return -1;
            } else {
                return 0;
            }
        },
        popup: {},
        class: {
            popup: 'MLU-filter-popup'
        },
        thindexcurr: 0,
        thindex: {},
        fillthead: 0,
    };

    $.fn.tablecontrol = function (settings) {
        var table, sortable, previous;
        return this.each(function () {
            table = $(this);
            previous = table.data('tablecontrol');
            if (previous) {
                previous.destroy();
            }
            table.data('tablecontrol', new $.tablecontrol(table, settings));
        });
    };

})(window.Zepto || window.jQuery);