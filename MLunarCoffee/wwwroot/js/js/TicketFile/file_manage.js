
function TF_IM_Divide() {
    if (TF_IM_CurrentID != 0 && TF_IM_Type != 0 && typeof TFM_OpenDetail === 'function') {
        let href = ''
        if (TF_IM_Type == 1)
            href = '/Marketing/TicketFile/TicketFile_File_Devide';
        else
            href = '/Marketing/TicketFile/TicketFile_Folder_Devide';
        TFM_OpenDetail(href + '?CurrentID=' + TF_IM_CurrentID + '&ver=' + versionofWebApplication);
    }
}

function TF_IM_Move() {
    if (TF_IM_CurrentID != 0 && typeof TFM_OpenDetail === 'function') {
        TFM_OpenDetail('/Marketing/TicketFile/TicketFile_File_Move?ImportID=' + TF_IM_CurrentID + '&ver=' + versionofWebApplication);
    }
}

function TF_IM_ConvertCust() {
    $("#TFM_DetailContent").empty();
    if (TF_IM_CurrentID != 0 && typeof TFM_OpenDetail === 'function') {
        TFM_OpenDetail('/Marketing/TicketFile/TicketFile_File_ConvertCustomer?ImportID=' + TF_IM_CurrentID + '&ver=' + versionofWebApplication);
    }
}




function TF_IM_Delete() {
    if (TF_IM_CurrentID != 0) {
        const promise = notiConfirm();
        promise.then(function () {
            TF_IM_Delete_Execute(TF_IM_CurrentID);
        }, function () { });
    }
}

function TF_IM_Delete_Execute(id) {
    AjaxLoad(url = "/Marketing/TicketFile/TicketFile_File_Manage/?handler=DeleteItem"
        , data = { 'id': id }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result == "1") {
                notiSuccess();
                TF_IM_Load();
            } else {
                notiError_SW();
            }
        }
    );
}