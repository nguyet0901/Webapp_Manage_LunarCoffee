
//#region // Content Main Grid

//#endregion
function Todo_Detail_SH_WhenClose(isClose) {
    if (isClose == 1) {
        $('#todo_message_input').prop('disabled', true);
        $('#tododetail_send_close').show();
        $('#todo_message_send').hide();
        $('#tododetail_edit').hide();
        $('.todo_mess_edit').hide();
        $('.todo_mess_delete').hide();
        
    }
    else {
        $('#todo_message_input').prop('disabled', false);
        $('#tododetail_send_close').hide();
        $('#todo_message_send').show();
        $('#tododetail_edit').show();
    }
}

// #region // Info Detail Event
function Todo_Detail_Reaction_Header_Click() {
    if (!$('#tododetail_reaction_a').hasClass('disabled'))
        $('#tododetail_reaction_a').addClass('disabled')
    let userstring = '"' + sys_userID_Main + '"';
    Todo_Detail_Reaction_Header_Execute(userstring)
}
function Todo_Detail_Reaction_Header_Execute(userstring) {
    AjaxLoad(url = "/Todo/Personal/TodoDetail/?handler=Reaction_Todo"
        , data = {
            'CurrentID': current_todo_id,
            'userstring': userstring
        }, async = true, error = null
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result)[0];
                let isReaction = Number(data.IsReaction)
                if (isReaction == 1) {
                    if (!$('#tododetail_reaction_icon').hasClass('liked'))
                        $('#tododetail_reaction_icon').addClass('liked')
                }
                else {
                    if ($('#tododetail_reaction_icon').hasClass('liked'))
                        $('#tododetail_reaction_icon').removeClass('liked')
                }
                $('#tododetail_reaction_icon').attr('data-reaction', data.DataReaction);
                if ($('#tododetail_reaction_a').hasClass('disabled'))
                    $('#tododetail_reaction_a').removeClass('disabled')

                $('#tododetail_reaction_num').html(JSON.parse(data.DataReaction).length)
            }
        });
}
function Todo_Detail_Update_Read() {
    AjaxLoad(url = "/Todo/Personal/TodoDetail/?handler=Update_Read_Todo"
        , data = { 'CurrentID': current_todo_id}, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                let id = "todo_item_list_" + result;
                if ($('#' + id).find('.todo_item_content').length) {
                    if ($('#' + id).find('.todo_item_content').hasClass('un_read'))
                        $('#' + id).find('.todo_item_content').removeClass('un_read')
                }
                if (typeof MS_MessageLoad !== 'undefined' && $.isFunction(MS_MessageLoad)) {
                    MS_MessageLoad();
                }
            }
    });
}
//#endregion

// #region // Message Detail Event
function Todo_Detail_Reaction_Message_Event() {
    $(".item_message_like").unbind().on("click", function () {
        let id = $(this).attr('data-id');
        if (!$(this).hasClass('disabled'))
            $(this).addClass('disabled')
        let userstring = '"' + sys_userID_Main + '"';
        Todo_Detail_Reaction_Message_Execute(userstring, id)
    })
    $(".todo_mess_edit").unbind().on("click", function () {
        let id = $(this).attr('data-id');
        if ($('#todo_mess_content_' + id).length) {
            let content = $('#todo_mess_content_' + id).html();
            $('#todo_message_input').val(content);
            Todo_Detail_BeginEdit(id);
        }
    })
    $(".todo_mess_delete").unbind().on("click", function () {
        let id = $(this).attr('data-id');
        const promise = notiConfirm();
        promise.then(function () { Todo_Detail_Message_Delete_Execute(id); }, function () { });
    })
    $(".todo_mess_editinfo").popup({
        transition: "scale up",
        position: "top right"
    });
}

function Todo_Detail_Reaction_Message_Execute(userstring, id) {
    AjaxLoad(url = "/Todo/Personal/TodoDetail/?handler=Reaction_Message"
        , data = {
            'messid': id,
            'userstring': userstring
        }, async = true, error = null
        , success = function (result) {
            if (result != "") {

                let data = JSON.parse(result)[0];
                let isReaction = Number(data.IsReaction)
                let idicon = 'todo_mess_' + id;
                let idcount = 'todo_mess_count' + id;
                if (isReaction == 1) {
                    if (!$('#' + idicon).hasClass('liked'))
                        $('#' + idicon).addClass('liked')
                }
                else {
                    if ($('#' + idicon).hasClass('liked'))
                        $('#' + idicon).removeClass('liked')
                }
                $('#' + idicon).attr('data-reaction', data.DataReaction);
                if ($('#' + idicon).hasClass('disabled'))
                    $('#' + idicon).removeClass('disabled')

                $('#' + idcount).html(JSON.parse(data.DataReaction).length)
            }
        });
}
function Todo_Detail_Message_Send() {
    let contentini = $('#todo_message_input').val();
    let content = contentini.split(' ').join('');
    if (content.length < 1) {
        if (!$('#todo_message_input').hasClass('noticeMess'))
            $('#todo_message_input').addClass('noticeMess')
    }
    else {
        if (!$('#todo_message_input').hasClass('disabled'))
            $('#todo_message_input').addClass('disabled')

        if ($('#todo_message_input').hasClass('noticeMess'))
            $('#todo_message_input').removeClass('noticeMess')
        $('#todo_message_input').val('');
        Todo_Detail_Message_Send_Execute(contentini) 
    }
}
function Todo_Detail_Message_Cancel() {
    Todo_Detail_UnEdit();
}                                                                     
function Todo_Detail_Message_Send_Execute(content) {
    AjaxLoad(url = "/Todo/Personal/TodoDetail/?handler=Sending_Mess"
        , data = {
            'TodoID': current_todo_id,
            'content': content,
            'currentid': todo_detail_current
        }, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                if ($('#todo_message_input').hasClass('disabled'))
                    $('#todo_message_input').removeClass('disabled')
                ToDoDetail_LoadMessage(result);
                if (todo_detail_current == 0) {
                    TodoList_LoadData(current_todo_id, 0, 1);
                }
                Todo_Detail_UnEdit();
            }
            else {
                toto_detail_close = 1;
                Todo_Detail_SH_WhenClose(toto_detail_close);
            }
        });
}
function Todo_Detail_UnEdit() {
    todo_detail_current = 0;
    $('.todo_message_item').removeClass('message_edit')
    $('#todo_message_cancel').hide();
    $('#todo_message_input').val('')
}
function Todo_Detail_BeginEdit(id) {
    todo_detail_current = id;
    let _iddiv = 'todo_message_item_' + id;
    $('.todo_message_item').removeClass('message_edit')
    if (!$('#' + _iddiv).hasClass('disabled'))
        $('#' + _iddiv).addClass('message_edit')
    $('#todo_message_cancel').show();
    
}
function Todo_Detail_Message_Delete_Execute(id) {
    AjaxLoad(url = "/Todo/Personal/TodoDetail/?handler=Delete_Mess"
        , data = {
            'messid': id
            , 'todoID': current_todo_id
        }, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                if ($('#todo_message_item_' + id).length) {
                    $('#todo_message_item_' + id).remove();
                }
                Todo_Detail_UnEdit();
            }
            else {
                toto_detail_close = 1;
                Todo_Detail_SH_WhenClose(toto_detail_close);
            }
        });
}
//#endregion
