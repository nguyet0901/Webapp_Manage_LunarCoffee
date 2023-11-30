//Sắp xếp Page Active trước
function Sort_Page_Actice() {
    $("#CurrentFace_Chossen .div_face_page_total").each(function () {
        let image = $(this).children("img.pageAvatarpage");
        let pageActive = image.hasClass("pageAvatarpage_Not_Permission");
        if (pageActive == true) {
            $(this).css("order", "2");
        }
        else {
            $(this).css("order", "1");
        }
    });
}
