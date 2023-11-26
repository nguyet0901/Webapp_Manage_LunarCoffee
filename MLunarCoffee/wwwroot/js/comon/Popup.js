//#region  Popup Main
function CloseModal() {
    if ($(".MLUech-checkform") && $(".MLUech-checkform").length > 0 && $('.MLUech-checkform .MLUech-checkedform').length > 0) {
        let msg = Outlang["Nhung_thay_doi_ban_da_thuc_hien_co_the_khong_duoc_luu"] ?? 'Những thay đổi bạn đã thực hiện có thể không được lưu';
        const promise = notiConfirm(msg);
        promise.then(function () {
            $('#DetailModal_Content').empty();
            $("#DetailModal_Content").html('')
            $('#DetailModal').modal('hide');
            window.onbeforeunload = null;
        }, function () { });
    }
    else {
        $('#DetailModal_Content').empty();
        $("#DetailModal_Content").html('')
        $('#DetailModal').modal('hide');
    }

    //if (!$('#DetailModalLV2').hasClass('in')) {
    //    $('#DetailModal').modal('hide');
    //    $('#DetailModal_Content').empty();
    //    $("#DetailModal_Content").html('');
    //}
    //else {
    //    $('#DetailModalLV2').modal('hide');
    //    $('#DetailModalLV2_Content').empty();
    //    $("#DetailModalLV2_Content").html('')
    //    $("#DetailModal").css("opacity", "1");
    //}
}
 
function CloseModalSlide() {
    $('#divFistSlideGuide').modal('hide');
}
function PrepageModal2() {
    $("#DetailModal").css("opacity", "0");
    $("#DetailModalLV2_Content").html('')
    $("#DetailModal").prepend('<div id="topDetailPopup" style="position: absolute;height: 100 %;width: 100 %;z-index: 1;"> </div>');

}

        //#endregion