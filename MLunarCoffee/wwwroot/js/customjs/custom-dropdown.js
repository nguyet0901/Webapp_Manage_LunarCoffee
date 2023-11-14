
function Dropdown_Set_Position(ele) {
    try {
        let offset = ele.offset();
        let scroll_window = $(window).scrollTop();
        let width = ele.outerWidth();
        let height = ele.outerHeight();

        let dropdown_temp = $('<div class="ui-dropdown-temp ui fluid dropdown"></div>')
        dropdown_temp.css({ "height": height });
        dropdown_temp.insertAfter(ele); // Thêm 1 element vào nội dung ( Giữ Được Chiều Cao )

        ele.attr("data-Offset", offset.top);
        ele.css({ "top": offset.top - scroll_window, "width": width, "position": "fixed", "transition": "none" });
        $(window).scroll(function () {
            let dropTop = parseInt(ele.css('top'));
            if (dropTop != 0) {
                let windowScroll = $(this).scrollTop();
                let offsetTop = Number(ele.attr("data-Offset"));
                ele.css("top", offsetTop - windowScroll);
            }
        })
    }
    catch (ex) {

    }
}

function Dropdown_Remove_Position(ele) {
    try {
        $(window).unbind('scroll');
        ele.siblings(".ui-dropdown-temp").remove();
        setTimeout(() => {
            ele.removeAttr("style");
            ele.removeAttr("data-Offset");
        }, 150)
    }
    catch (ex) {

    }
}