
function Desk_AppInDay (app_ID, doctor_id, status_id, branch_ID) {
    if (branch_ID == Master_branchID) {
        AjaxLoad(url = "/Desk/DeskMaster/?handler=LoadApp"
            , data = {
                "AppID": app_ID
                , "DoctorID": doctor_id
                , "StatusID": status_id
                , "BranchID": branch_ID
            }, async = true
            , error = function () {
                notiError_SW();
            }
            , success = function (result) {
                let data = JSON.parse(result);
                if (data && data.length == 1) {
                    let item = data[0];
                    Desk_UpdateRow(item);
                }
                else {
                    Desk_RemoveRow(app_ID);
                }
            }
        );
    }
}
function Desk_UpdateRow (item) {
    let ID = item.ID;
    let StatusTime = item.StatusTime;
    let Color = item.Color;
    let StatusName = item.StatusName;
    if ($('#Row_App_Inday_' + ID).length > 0) {
        let rgbaCol = 'rgba(' + parseInt(Color.slice(-6, -4), 16)
            + ',' + parseInt(Color.slice(-4, -2), 16)
            + ',' + parseInt(Color.slice(-2), 16)
            + ',0.5)';
        $('#Row_App_Inday_' + ID).css("background-color", rgbaCol);
        if ($('#appstatusname_' + ID).length > 0) {
            $('#appstatusname_' + ID).css("background-color", Color);
            $('#appstatusname_' + ID).html(StatusName);
        }
        if ($('#time_ago_status_' + ID).length > 0) {
            let _statustime = Face_Convert_Date_To_DOW_HM(StatusTime);
            $('#time_ago_status_' + ID).html(((_statustime != '00:00') ? _statustime : ''));
        }
    }
    else {
        LoadAppointmentListAjax(ID);
    }

}
function Desk_RemoveRow (app_ID) {
    if ($('#Row_App_Inday_' + app_ID).length > 0) $('#Row_App_Inday_' + app_ID).remove();
}
function Desk_RoomInDay (data) {
    
    RealTime_Remove(data.oldroom_id, data.oldchair_id);
    RealTime_Add(data.newroom_id, data.newchair_id, data.cust_id, data.cust_name, data.current_doctor, new Date());
}




