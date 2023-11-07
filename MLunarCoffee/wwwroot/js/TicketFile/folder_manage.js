function Ticket_File_Manual_UpdateUnSeen() {

    if (Ticket_File_Manual_CurrentID != 0) {
        let _id = 'folder_ticket_unseen_' + Ticket_File_Manual_CurrentID;
        if (!$('#' + _id).length) {
            Ticket_File_Manual_UpdateSeen(Ticket_File_Manual_CurrentID, 0);
        }
    }
}
function Ticket_File_Manual_UpdateSeen(id, isseen) {
    AjaxLoad(url = "/Marketing/TicketFile/TicketFile_Folder_Manage?handler=Update_SeenFile"
        , data = { "FileID": id , "IsSeen": isseen }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {

                let _id_parent = 'filtr_item_file_' + id;
                if (Number(isseen) == 1) {

                    if ($('#' + _id_parent).hasClass('filtr_item_file_unseen')) {
                        $('#' + _id_parent).removeClass('filtr_item_file_unseen');
                    }
                }
                else {

                    if (!$('#' + _id_parent).hasClass('filtr_item_file_unseen')) {
                        $('#' + _id_parent).addClass('filtr_item_file_unseen');
                    }
                }
                if (typeof TicketFile_Master_LoadTotalSeenFile !== 'undefined' && $.isFunction(TicketFile_Master_LoadTotalSeenFile)) {
                    TicketFile_Master_LoadTotalSeenFile();
                }
            }
        }
    );

}
function Ticket_File_Manual_Prepare_UpdateName() {
    if (Ticket_File_Manual_CurrentID != 0) {      
        let _id = 'folder_ticket_name_' + Ticket_File_Manual_CurrentID;
        if ($('#' + _id).length) {
            $('#' + _id).prop('disabled', false);
            $('#' + _id).focus();
            if (!$('#' + _id).hasClass('folder_ticket_name_progressing')) {
                $('#' + _id).addClass('folder_ticket_name_progressing')
            }
            $('#' + _id).focusout(function () {
                Ticket_File_Manual_Done_UpdateName();
            });
        }
    }
}
function Ticket_File_Manual_Done_UpdateName() {
    if (Ticket_File_Manual_CurrentID != 0) {
        let _id = 'folder_ticket_name_' + Ticket_File_Manual_CurrentID;
        if ($('#' + _id).length) {
            $('#' + _id).prop('disabled', true);
            if ($('#' + _id).hasClass('folder_ticket_name_progressing')) {
                $('#' + _id).removeClass('folder_ticket_name_progressing')
            }
            Ticket_File_Manual_UpdateName(Ticket_File_Manual_CurrentID, $('#' + _id).val());
        }
        Ticket_File_Manual_CurrentID = 0;
    }
}
function Ticket_File_Manual_UpdateName(id, fileName) {
    AjaxLoad(url = "/Marketing/TicketFile/TicketFile_Folder_Manage?handler=Update_Name"
        , data = { "FileID": id , "FileName": fileName }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {

            }
        }
    );
}
function Ticket_File_Manual_Button_Popup(_devide) {
    if (Ticket_File_Manual_CurrentID != 0) {
        let _id = 'folder_ticket_unseen_' + Ticket_File_Manual_CurrentID;
        if ($('#' + _id).length) {
            $('#btnTicket_File_Manual_UpdateUnSeen').hide();
        }
        else $('#btnTicket_File_Manual_UpdateUnSeen').show();
        if (Number(_devide) == 1) {
            $('#btnTicket_File_Manual_Devide').hide();
        }
        else $('#btnTicket_File_Manual_Devide').show();
    }
}



function Ticket_File_Manual_Devide() {
    if (Ticket_File_Manual_CurrentID != 0) {
        $('#Ticket_File_Manual_Detail').show();
        $('#Ticket_File_Manual_Main').hide();
        $('#Ticket_File_Manual_Folder_Detail').empty();
        $('#Ticket_File_Manual_Folder_Detail').load('/Marketing/TicketFile/TicketFile_Folder_Devide?'
            + 'ImportID=' + Ticket_File_Manual_ImportID
            + '&BlockIndex=' + Ticket_File_Manual_BlockIndex
            + '&ver=' + versionofWebApplication);

        $('#Ticket_File_Manual_Header_Back').html(Ticket_File_Manual_CurrentName);
    }

    
}