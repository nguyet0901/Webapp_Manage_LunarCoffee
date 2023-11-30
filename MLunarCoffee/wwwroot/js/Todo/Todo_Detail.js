function Master_Todo_Open_Popup_Detail(e, currrentid, custid, ticketid) {

    let Todo_TicketID = (ticketid != undefined ? ticketid : 0);
    let Todo_CustomerID = (custid != undefined ? custid : 0);
    let Todo_CurrentID = (currrentid != undefined ? currrentid : 0);

    let Master_Div = $("#MasterContainer .pusher.pushable");
    let Master_Div_Scroll_Top = Master_Div.scrollTop();
    let Master_Top = $("#Master_Menu_Top");
    let Master_Top_Height = Master_Top.outerHeight();
    let Element_Div = $(e)
    let Window_Width = $(window).width();
    let Element_Width = Element_Div.outerWidth();
    let Element_Height = Element_Div.outerHeight();
    let Element_OffSet = Element_Div.offset();
    let Element_Position_Top = Element_OffSet.top;
    let Element_Position_Left = Element_OffSet.left;
    let Popup_Width = 400;
    let width_sidebar = $(".ui.sidebar.very.thin.icon").outerWidth();
    let Calc_Left = 0;
    if ((Window_Width - Element_Position_Left - width_sidebar) > Popup_Width) {
        Calc_Left = 0;
    }
    else if (Element_Position_Left - Popup_Width - width_sidebar + Element_Width > 0) {
        Calc_Left = -Popup_Width + Element_Width;
    }
    else {
        Calc_Left = - Element_Position_Left + width_sidebar + 5;
    }
    let Popup_Top = Element_Position_Top - Master_Top_Height + Master_Div_Scroll_Top + Element_Height;
    //let Popup_Left = Element_Position_Left + ((Window_Width - Element_Position_Left > Popup_Width) ? Element_Width : -Popup_Width);
    let Popup_Left = Element_Position_Left + Calc_Left;
    Master_Div.append($("#DetailModalPopup"));
    $("#DetailModalPopup").empty();
    $("#DetailModalPopup").css({
        "position": "absolute",
        "top": Popup_Top - 20,
        "left": Popup_Left,
        "display": "block",
        "width": Popup_Width + 'px',
        "max-width": "calc( 100% - " + width_sidebar + "px )"
    }).animate({
        "top": Popup_Top + 5
    })
    $("#DetailModalPopup").load("/ToDo/ToDoRemind/ToDoDetail?CurrentID=" + Todo_CurrentID + '&CustomerID=' + Todo_CustomerID + '&TicketID=' + Todo_TicketID + "&ver=" + versionofWebApplication);
}
function Master_Todo_Open_Popup_ChangeStatus(e, currrentid, custid, ticketid) {
    let Todo_TicketID = (ticketid != undefined ? ticketid : 0);
    let Todo_CustomerID = (custid != undefined ? custid : 0);
    let Todo_CurrentID = (currrentid != undefined ? currrentid : 0);

    let Master_Div = $("#MasterContainer .pusher.pushable");
    let Master_Div_Scroll_Top = Master_Div.scrollTop();
    let Master_Top = $("#Master_Menu_Top");
    let Master_Top_Height = Master_Top.outerHeight();
    let Element_Div = $(e)
    let Window_Width = $(window).width();
    let Element_Width = Element_Div.outerWidth();
    let Element_Height = Element_Div.outerHeight();
    let Element_OffSet = Element_Div.offset();
    let Element_Position_Top = Element_OffSet.top;
    let Element_Position_Left = Element_OffSet.left;
    let Popup_Width = 250;
    let width_sidebar = $(".ui.sidebar.very.thin.icon").outerWidth();
    let Calc_Left = 0;
    if ((Window_Width - Element_Position_Left - width_sidebar) > Popup_Width) {
        Calc_Left = 0;
    }
    else if (Element_Position_Left - Popup_Width - width_sidebar + Element_Width > 0) {
        Calc_Left = -Popup_Width + Element_Width;
    }
    else {
        Calc_Left = - Element_Position_Left + width_sidebar + 5;
    }

    let Popup_Top = Element_Position_Top - Master_Top_Height + Master_Div_Scroll_Top + Element_Height;
    //let Popup_Left = Element_Position_Left + ((Window_Width - Element_Position_Left > Popup_Width) ? Element_Width : -Popup_Width);
    let Popup_Left = Element_Position_Left + Calc_Left;
    Master_Div.append($("#DetailModalTodoChangeStatus"));
    $("#DetailModalTodoChangeStatus").empty();
    $("#DetailModalTodoChangeStatus").css({
        "position": "absolute",
        "top": Popup_Top - 20,
        "left": Popup_Left,
        "display": "block",
        "width": Popup_Width + 'px',
        "max-width": "calc( 100% - " + width_sidebar + "px )"
    }).animate({
        "top": Popup_Top
    })
    $("#DetailModalTodoChangeStatus").load("/ToDo/ToDoRemind/ToDoChangeStatus?CurrentID=" + Todo_CurrentID + '&CustomerID=' + Todo_CustomerID + '&TicketID=' + Todo_TicketID + "&ver=" + versionofWebApplication);
}
$(document).mouseup(function (e) {
    let todo_pop_detail = $('#DetailModalPopup');
    let todo_pop_changestatus = $('#DetailModalTodoChangeStatus');
    if (!todo_pop_changestatus.is(e.target)
        && todo_pop_changestatus.has(e.target).length === 0) {
        todo_pop_changestatus.hide();
    }
    if (!todo_pop_detail.is(e.target)
        && todo_pop_detail.has(e.target).length === 0) {
        todo_pop_detail.hide();
    }
});