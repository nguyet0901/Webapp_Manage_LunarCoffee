//#region // Function Global

(function ($) {
    var MLU_CustomJQueryFPFunc = jQuery.fn.flatpickr;
    var MLU_CustomerFPConstrutor = flatpickr;
    jQuery.fn.flatpickr = function (e) {
        e = { ...e, locale: "MLU" };
        return MLU_CustomJQueryFPFunc.call(this,e);
    }

    flatpickr = function (e, t) {
        t = { ...t, locale: "MLU" };
        return MLU_CustomerFPConstrutor.call(this, e, t);
    }

    $(window).click(function (e) {
        try {
            var clickover = $(e.target);
            if (!clickover.hasClass("collapse")
                && !clickover.hasClass("form-check-input")
                && !clickover.hasClass("form-switch")
                && (clickover.parent() != undefined && !clickover.parent().hasClass("collapse"))) {
                $('.collapse').not('.collapsesticky').collapse('hide');
            }

            let itemmenus = $(".item-menu");
            if ($(itemmenus).is(e.target) || $(itemmenus).has(e.target).length !== 0) {
                let _menu = clickover.closest('.nav-menus');
                if (_menu.hasClass('nav-mobile-toggle')) {
                    _menu.removeClass('nav-mobile-toggle');
                }
                else {
                    _menu.addClass('nav-mobile-toggle');
                }
            }
            else {
                $(".nav.nav-menus").removeClass("nav-mobile-toggle");
            }

            // SHOW MORE CONTENT
            let content_more = $("#Master_ViewContent");
            let content = $(".content_line_clamp");
            if (!$(content_more).is(e.target) && $(content_more).has(e.target).length === 0
                && !$(content).is(e.target) && $(content).has(e.target).length === 0) {
                $("#Master_ViewContent").removeClass('active');
            }

            // REMOVE TOOLTIP & POPOVER
            let attrTooltip = clickover.attr('data-bs-toggle');
            if (attrTooltip == 'undefined' || (attrTooltip != 'tooltip' && attrTooltip != 'popover')) {
                $('.tooltip.fade.show').remove();
            }

        }
        catch (e) {

        }
    });

    $(window).on('mousewheel mousemove mouseout', function (e) {
        try {
            var clickover = $(e.target);
            let attrTooltip = clickover.attr('data-bs-toggle');
            if (attrTooltip == 'undefined' || (attrTooltip != 'tooltip' && attrTooltip != 'popover')) {
                $('.tooltip.fade.show:not(:last-child)').remove();
            }
        }
        catch (ex) {

        }
    })

    $(document).on("click", ".vtcondition .sign", function (e) {
        try {
            $(this).next().collapse('toggle');
            $(this).attr('aria-expanded', !($(this).attr('aria-expanded') == 'true'))
        }
        catch (e) {
        }
    });

    $(document).on("show.bs.collapse", ".collapse", function (e) {
        try {
            if (!$(this).hasClass('fulllap')) {
                var popupWidth = $(this).outerWidth()
                var marginLeft = parseInt($("main.main-content").css("marginLeft"));
                var offleft = $(this).parent().offset().left;
                if (offleft - marginLeft < popupWidth) {
                    $(this).css('left', - (offleft - marginLeft) + 20);
                    if (popupWidth > window.outerWidth) {
                        $(this).css({
                            "width": window.outerWidth - 40,
                            "min-width": window.outerWidth - 40,
                            "max-width": window.outerWidth - 40
                        });
                    }
                };
            }
        }
        catch (e) { }
    });

    $(document).on("click", ".nav-menus", function (e) {
        try {







            //var popupWidth = $(this).outerWidth()
            //var marginLeft = parseInt($("main.main-content").css("marginLeft"));
            //var offleft = $(this).parent().offset().left;

            //if (offleft - marginLeft < popupWidth) {
            //    $(this).css('left', - (offleft - marginLeft) + 20);
            //    if (popupWidth > window.outerWidth) {
            //        $(this).css({
            //            "width": window.outerWidth - 40,
            //            "min-width": window.outerWidth - 40,
            //            "max-width": window.outerWidth - 40
            //        });
            //    }
            //};
        }
        catch (e) { }
    });

    $.fn.NavMobile = function (target) {
        let element = $(this);
        switch (target) {
            case 'show':
                element.addClass("nav-mobile-toggle");
                break;
            case 'hide':
                element.removeClass("nav-mobile-toggle");
                break;
        }
    };


})(jQuery);


function Master_OnLoad_Error_Image(image) {

    image.src = Master_Default_Pic;
    return true;
}

function Master_Onload_SuccessPic(img, mwi, mhe) {
    try {
        let wi = img.width;
        let he = img.height;
        if (wi >= he) {
            img.style.maxHeight = mhe.toString() + 'px';
        }
        else img.style.maxWidth = mwi.toString() + 'px';
    }
    catch {

    }
}

function Master_OnLoad_Success_Image() {

}

function Count_Up(id, value, isDecimal) {
    if (document.getElementById(id)) {
        let countUp = new CountUp(id, value, { decimalPlaces: (!isDecimal == 1 ? 0 : 2) });
        if (!countUp.error) countUp.start();
    }
}

function ToolPopper() {
    let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    let tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
    let popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    let popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })
}
//#endregion
//#region // Other
function ChangeAssistname(cn, rep, repted) {
    var x = document.getElementsByClassName(cn);
    if (x != null && x != undefined) {
        for (i = 0; i < x.length; i++) {
            x[i].innerHTML = x[i].innerHTML.replace(rep, repted);
        }
    }

}
$('.ui.dropdown .remove.icon').on('click', function (e) {
    $(this).parent('.dropdown').dropdown('clear');
    e.stopPropagation();
});


//#endregion

//#region  // Validation require
function Checking_Require_Validation(name) {
    Checking_Require_Validation_Exe(RequireValidation, name ?? '');
}
function Checking_Require_Validation_Exe(data, name) {
    if (data != undefined && data != null && data.length > 0) {
        x = document.getElementsByClassName("_validation_option_");
        name = (name.toLowerCase()).startsWith('/pages/') ? (name.toLowerCase()).replace('/pages/', '/') : name;
        let findchar = name.substr(name.length - 1, 1);
        let namecheck = findchar == '/' ? name : name + '/';
        if (x != undefined && x.length != 0) {
            for (i = x.length - 1; i >= 0; i--) {
                let datatab = x[i].attributes["data-validation"].value;
                if (data != undefined && data.length != 0) {
                    for (j = 0; j < data.length; j++) {
                        if (data[j].ControlName == datatab && namecheck === data[j].PagePath) {
                            switch (x[i].nodeName.toLowerCase()) {
                                case 'input':
                                    x[i].setAttribute("name", data[j].FieldName);
                                    break;
                                case 'textarea':
                                    x[i].setAttribute("name", data[j].FieldName);
                                    break;
                            }
                            j = data.length;
                        }
                    }
                }
            }
        }
    }
}
//#endregion
//#region  // Noti validation require
(function ($) {
    $.fn.Require_Validation = function () {

        let field = this.find('.field.error');
        let content = '';
        if (field.length > 0) {
            field.each(function (index) {
                if (field[index].children[0].nodeName.toLowerCase() == 'label') {
                    let item = field[index].children[0].textContent;
                    content += item.replace('*', '') + ", ";
                }

            });
            if (this.find('.noti-validate')) {
                $('.noti-validate', this).remove();
            }
            if (content != '') {
                this.append('<div class="noti-validate text-sm">' + Outlang["Yeu_cau"] + ': ' + content + '</div>');
                return false;
            }
        }
        return true;
    };
})(jQuery);
//#endregion
//#region  // Permission
function Checking_TabControl_Permission() {
    Checking_TabControl_Permission_Exe(PermissionTable_TabControl);
}

function CheckPermissionColumnInGrid(dt, name, gridName) {

    if (dt != null && dt != undefined && dt.length != 0) {
        for (var i = 0; i < dt.length; i++) {
            let controlname = dt[i].ControlName;
            let linkPage = dt[i].LinkPage;
            if (linkPage === name && dt[i].ControlType == 2) {
                if ($('#' + controlname).length) {
                    let table = document.getElementById(gridName);
                    let column = table.rows[0];
                    if (column != undefined && column != null && column.cells != null && column.cells != undefined && column.cells.length != 0) {
                        let countNoneDisplay = 0;
                        for (let i = 0; i < column.cells.length; i++) {
                            if (column.cells[i].id != undefined && column.cells[i].id == controlname) {
                                let index = i + 1;
                                $('table#' + gridName + ' td:nth-child(' + index + '),table#' + gridName + ' th:nth-child(' + index + ')').empty();
                                $('table#' + gridName + ' td:nth-child(' + index + '),table#' + gridName + ' th:nth-child(' + index + ')').hide();
                            }
                        }
                    }
                }
            }
        }
    }
}
function CheckPermissionControlInPage(dt, name) {
    try {
        if (dt != null && dt != undefined && dt.length != 0) {
            for (let i = 0; i < dt.length; i++) {
                let controlname = dt[i].ControlName;
                let linkPage = dt[i].LinkPage;
                name = (name.toLowerCase()).startsWith('/pages/') ? (name.toLowerCase()).replace('/pages/', '/') : name;
                let findchar = name.substr(name.length - 1, 1);
                let namecheck = findchar == '/' ? name : name + '/';
                if (linkPage.toLowerCase() === namecheck.toLowerCase() && dt[i].ControlType == 1) {
                    if ($('#' + controlname).length) {
                        $('#' + controlname).hide();
                        $('#' + controlname).empty();
                    }
                }
            }
        }
    }
    catch (ex) {

    }
}

//#endregion
//#region  // Check Connection
function Master_Checking_CallCenter() {
    Callcenter_CheckUsing(usingCallCenter, checkingCallCenter, callextension, linkClickToCall
        , calloutbound, calllogintimeexpired, callport, calldomain, callpassword
    );
}
function CheckCallCenter_Click() {
    Master_Checking_CallCenter();
}

function CheckVoiceDevice_Click() {
    CheckingvoiceDevice();
}
function CheckConnection_Click() {
    let _checkingimageNetwork = Master_Data._checkingimageNetwork;
    CheckConnection(_checkingimageNetwork);
}

//#region // Dark-mode

// Light Mode / Dark Mode
function darkMode(el) {
    const body = document.getElementsByTagName('body')[0];
    const hr = document.querySelectorAll('div:not(.sidenav) > hr');
    const hr_card = document.querySelectorAll('div:not(.bg-gradient-dark) hr');
    const text_btn = document.querySelectorAll('button:not(.btn) > .text-dark');
    const text_span = document.querySelectorAll('span.text-dark, .breadcrumb .text-dark');
    const text_span_white = document.querySelectorAll('span.text-white, .breadcrumb .text-white');
    const text_strong = document.querySelectorAll('strong.text-dark');
    const text_strong_white = document.querySelectorAll('strong.text-white');
    const text_nav_link = document.querySelectorAll('a.nav-link.text-dark');
    const text_nav_link_white = document.querySelectorAll('a.nav-link.text-white');
    const secondary = document.querySelectorAll('.text-secondary');
    const bg_gray_100 = document.querySelectorAll('.bg-gray-100');
    const bg_gray_600 = document.querySelectorAll('.bg-gray-600');
    const btn_text_dark = document.querySelectorAll('.btn.btn-link.text-dark, .material-icons.text-dark');
    const btn_text_white = document.querySelectorAll('.btn.btn-link.text-white, .material-icons.text-white');
    const card_border = document.querySelectorAll('.card.border');
    const card_border_dark = document.querySelectorAll('.card.border.border-dark');

    const svg = document.querySelectorAll('g');

    if (!el.getAttribute("checked")) {
        body.classList.add('dark-version');
        for (var i = 0; i < hr.length; i++) {
            if (hr[i].classList.contains('dark')) {
                hr[i].classList.remove('dark');
                hr[i].classList.add('light');
            }
        }

        for (var i = 0; i < hr_card.length; i++) {
            if (hr_card[i].classList.contains('dark')) {
                hr_card[i].classList.remove('dark');
                hr_card[i].classList.add('light');
            }
        }
        for (var i = 0; i < text_btn.length; i++) {
            if (text_btn[i].classList.contains('text-dark')) {
                text_btn[i].classList.remove('text-dark');
                text_btn[i].classList.add('text-white');
            }
        }
        for (var i = 0; i < text_span.length; i++) {
            if (text_span[i].classList.contains('text-dark')) {
                text_span[i].classList.remove('text-dark');
                text_span[i].classList.add('text-white');
            }
        }
        for (var i = 0; i < text_strong.length; i++) {
            if (text_strong[i].classList.contains('text-dark')) {
                text_strong[i].classList.remove('text-dark');
                text_strong[i].classList.add('text-white');
            }
        }
        for (var i = 0; i < text_nav_link.length; i++) {
            if (text_nav_link[i].classList.contains('text-dark')) {
                text_nav_link[i].classList.remove('text-dark');
                text_nav_link[i].classList.add('text-white');
            }
        }
        for (var i = 0; i < secondary.length; i++) {
            if (secondary[i].classList.contains('text-secondary')) {
                secondary[i].classList.remove('text-secondary');
                secondary[i].classList.add('text-white');
                secondary[i].classList.add('opacity-8');
            }
        }
        for (var i = 0; i < bg_gray_100.length; i++) {
            if (bg_gray_100[i].classList.contains('bg-gray-100')) {
                bg_gray_100[i].classList.remove('bg-gray-100');
                bg_gray_100[i].classList.add('bg-gray-600');
            }
        }
        for (var i = 0; i < btn_text_dark.length; i++) {
            btn_text_dark[i].classList.remove('text-dark');
            btn_text_dark[i].classList.add('text-white');
        }
        for (var i = 0; i < svg.length; i++) {
            if (svg[i].hasAttribute('fill')) {
                svg[i].setAttribute('fill', '#fff');
            }
        }
        for (var i = 0; i < card_border.length; i++) {
            card_border[i].classList.add('border-dark');
        }
        el.setAttribute("checked", "true");
    } else {
        body.classList.remove('dark-version');
        for (var i = 0; i < hr.length; i++) {
            if (hr[i].classList.contains('light')) {
                hr[i].classList.add('dark');
                hr[i].classList.remove('light');
            }
        }
        for (var i = 0; i < hr_card.length; i++) {
            if (hr_card[i].classList.contains('light')) {
                hr_card[i].classList.add('dark');
                hr_card[i].classList.remove('light');
            }
        }
        for (var i = 0; i < text_btn.length; i++) {
            if (text_btn[i].classList.contains('text-white')) {
                text_btn[i].classList.remove('text-white');
                text_btn[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < text_span_white.length; i++) {
            if (text_span_white[i].classList.contains('text-white') && !text_span_white[i].closest('.sidenav') && !text_span_white[i].closest('.card.bg-gradient-dark')) {
                text_span_white[i].classList.remove('text-white');
                text_span_white[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < text_strong_white.length; i++) {
            if (text_strong_white[i].classList.contains('text-white')) {
                text_strong_white[i].classList.remove('text-white');
                text_strong_white[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < text_nav_link_white.length; i++) {
            if (text_nav_link_white[i].classList.contains('text-white') && !text_nav_link_white[i].closest('.sidenav')) {
                text_nav_link_white[i].classList.remove('text-white');
                text_nav_link_white[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < secondary.length; i++) {
            if (secondary[i].classList.contains('text-white')) {
                secondary[i].classList.remove('text-white');
                secondary[i].classList.remove('opacity-8');
                secondary[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < bg_gray_600.length; i++) {
            if (bg_gray_600[i].classList.contains('bg-gray-600')) {
                bg_gray_600[i].classList.remove('bg-gray-600');
                bg_gray_600[i].classList.add('bg-gray-100');
            }
        }
        for (var i = 0; i < svg.length; i++) {
            if (svg[i].hasAttribute('fill')) {
                svg[i].setAttribute('fill', '#252f40');
            }
        }
        for (var i = 0; i < btn_text_white.length; i++) {
            if (!btn_text_white[i].closest('.card.bg-gradient-dark')) {
                btn_text_white[i].classList.remove('text-white');
                btn_text_white[i].classList.add('text-dark');
            }
        }
        for (var i = 0; i < card_border_dark.length; i++) {
            card_border_dark[i].classList.remove('border-dark');
        }
        el.removeAttribute("checked");
    }
};
//#endregion

//#region // Get member
function sys_getmember(data, amount) {
    let result = '';
    if (data != undefined && data.length != 0) {
        if (amount != undefined) {
            amount = Number(amount);

            if (amount >= data[data.length - 1].AmountTo) result = data[data.length - 1].Name;
            else if (amount <= data[0].AmountFrom) result = data[0].Name;
            else {
                let member = data.filter(function (wor) {
                    return (Number(wor.AmountFrom) <= amount && Number(wor.AmountTo) >= amount);
                });
                if (member != undefined && member.length > 0) {
                    result = member[0].Name;
                }
            }
        }
    }
    return result;
}

function sys_getmember_color(data, amount) {
    let result = '';
    if (data != undefined && data.length != 0) {
        if (amount != undefined) {
            amount = Number(amount);
            if (amount >= data[data.length - 1].AmountTo) result = data[data.length - 1].ColorCode;
            else if (amount <= data[0].AmountFrom) result = data[0].ColorCode;
            else {
                let member = data.filter(function (wor) {
                    return (Number(wor.AmountFrom) <= amount && Number(wor.AmountTo) >= amount);
                });
                if (member != undefined && member.length > 0) {
                    result = member[0].ColorCode;
                }
            }
        }
    }
    return result;
}