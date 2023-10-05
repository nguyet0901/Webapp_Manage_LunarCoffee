//Fixed Thead, left, right, tfoot Table
//$("#dtContentMaterial").tableHeadFixer({ "head": true, "left": 0, "class":"fixed-side" });
//[0] head : true --> fixed Thead
//[1] foot : true --> fixed Tfoot
//[2] left or right : quantity
//[3] Class fixed left or right
(function ($) {
    $.fn.setheightmaxcontent = function () {
        var content = $(this);
        var offset = content.offset();
        var top = offset.top + 1;
        $(this).css('cssText', 'height: calc(100vh - ' + top + 'px );overflow-y:auto;overflow-x: hidden; padding:0;margin-top:1em;');
    };
    $.fn.setheightmaxcontentXY = function () {
        var content = $(this);
        var offset = content.offset();
        var top = offset.top + 10;
        $(this).css('cssText', 'height: calc(99vh - ' + top + 'px );overflow:auto; padding:0;margin-top:1em;border:none');
    };
    $.fn.setheightmaxcontentzoom = function () {
        var content = $(this);
        var offset = content.offset();
        var top = offset.top + 10;
        var percent = $("#BodyMain").css("zoom");
        if (percent < 1) {
            percent = (1 - percent + 0.1) * 2;
            top = top - (percent * top);
        }
        $(this).css('cssText', 'height: calc(98vh - ' + top + 'px );overflow:auto; padding:0;margin-top:1em;border:none;');
    };
    $.fn.tableHeadFixer = function (param) {
        var defaults = {
            head: true,
            foot: false,
            left: 0,
            right: 0,
            headleft: false,
            class: "",
            height: 0,
            dropdown: 0,
        };

        var settings = $.extend({}, defaults, param);

        return this.each(function () {
            settings.table = this;
            if (settings.height == 1) {
                $(settings.table).parent().setheightmaxcontentXY();
            }
            settings.parent = $("<div></div>");
            setParent();

            if (settings.foot == true)
                fixFoot();

            if (settings.left > 0 && settings.headleft == true)
                fixLeft();

            if (settings.right > 0)
                fixRight();

            if (settings.head == true)
                fixHead();


            if (settings.dropdown > 0) {
                fixDropdown();
            }
            // self.setCorner();

            $(window).resize(function () {
                $(this).parent('.AppendTable').trigger("scroll");
            });

            $(this).parent('.AppendTable').trigger("scroll");
        });

        function setTable(table) {

        }


        function setParent() {
            var container = $(settings.table).parent();
            var parent = $(settings.parent);
            var table = $(settings.table);
            if (!container.hasClass('AppendTable')) {
                table.before(parent)
                parent.append(table);
                parent.append(parent);
                parent.addClass('AppendTable');
                let height = container.height() - 2;
                parent
                    .css({
                        'width': '100%',
                        'height': (settings.height == 1) ? height + "px" : "100%",
                        'overflow': 'auto',
                        'max-height': container.css("max-height"),
                        'min-height': container.css("min-height"),
                        'max-width': container.css('max-width'),
                        'min-width': container.css('min-width'),
                        'border-top': '1px solid rgba(34, 36, 38, .1)',
                    });
            }
            parent = $('.AppendTable');
            parent.scroll(function () {
                var scrollWidth = parent[0].scrollWidth;
                var clientWidth = parent[0].clientWidth;
                var scrollHeight = parent[0].scrollHeight;
                var clientHeight = parent[0].clientHeight;
                var top = parent.scrollTop();
                var left = parent.scrollLeft();

                if (settings.head)
                    this.find("thead tr > *").css("top", top - 1);

                if (settings.foot)
                    this.find("tfoot tr > *").css("bottom", scrollHeight - clientHeight - top - 2);

                if (settings.left > 0 && settings.headleft == true)
                    settings.leftColumns.css("left", left);

                if (settings.right > 0)
                    settings.rightColumns.css("right", scrollWidth - clientWidth - left);
            }.bind(table));
        }

        function fixHead() {
            var thead = $(settings.table).find("thead");
            var tr = thead.find("tr");
            var cells = thead.find("tr > *");
            var classleft = settings.class;
            var cellfix = thead.find("tr > *." + classleft);
            setBackground(cells);
            cells.css({
                'position': 'relative',
                'z-index': '7'
            });
            if (settings.headleft == true) {
                cellfix.css({
                    'z-index': '8',
                });
            }
        }

        function fixFoot() {
            var tfoot = $(settings.table).find("tfoot");
            var tr = tfoot.find("tr");
            var cells = tfoot.find("tr > *");

            setBackground(cells);
            cells.css({
                'position': 'relative'
            });
        }

        function fixLeft() {
            var table = $(settings.table);

            var fixColumn = settings.left;
            var classleft = settings.class;

            settings.leftColumns = $();

            for (var i = 1; i <= fixColumn; i++) {
                settings.leftColumns = settings.leftColumns
                    .add(table.find("tr > *." + classleft));
            }
            var columnnot = table.find("tr > th:not(." + classleft + ")");
            columnnot.each(function (k, cell) {
                var cell = $(cell);
                cell.css({
                    'z-index': '5'
                });
            })

            var column = settings.leftColumns;

            column.each(function (k, cell) {
                var cell = $(cell);
                if (cell.not("th").length) {
                    setBackground(cell, 'white');
                }
                else {
                    setBackground(cell);
                }
                cell.css({
                    'position': 'relative',
                    'z-index': '6'
                });
                cell.last().css({ "border-right": "1px solid rgba(34,36,38,.1)" });
            });


        }

        function fixRight() {
            var table = $(settings.table);

            var fixColumn = settings.right;
            var classright = settings.class;

            settings.rightColumns = $();

            for (var i = 1; i <= fixColumn; i++) {
                settings.rightColumns = settings.rightColumns
                    .add(table.find("tr > *." + classright));
            }

            var column = settings.rightColumns;

            column.each(function (k, cell) {
                var cell = $(cell);

                setBackground(cell);
                cell.css({
                    'position': 'relative'
                });
            });

        }

        function fixDropdown() {
            var fixClass = settings.class;
            var column = $("." + fixClass + " .ui.selection.dropdown");
            column.each(function (k, cell) {
                var cell = $(cell);
                cell.css({
                    'z-index': '3'
                });
            });
        }

        function setBackground(elements, color) {
            elements.each(function (k, element) {
                var element = $(element);
                var parent = $(element).parent();

                var elementBackground = element.css("background-color");
                elementBackground = (elementBackground == "transparent" || elementBackground == "rgba(0, 0, 0, 0)") ? null : elementBackground;

                var parentBackground = parent.css("background-color");
                parentBackground = (parentBackground == "transparent" || parentBackground == "rgba(0, 0, 0, 0)") ? null : parentBackground;

                var background = parentBackground ? parentBackground : "white";
                background = elementBackground ? elementBackground : background;
                if (color != undefined) {
                    element.css("background-color", color);
                }
                else {
                    element.css("background-color", background);
                }

            });
        }
    };
})(jQuery);