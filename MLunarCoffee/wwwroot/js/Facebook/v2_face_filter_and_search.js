var face_data_filter = [];
var face_fitler_tag = "";
var face_fitler_staff = "";
var face_fitler_date = "";

function FaceFilter_Clear() {

    face_data_filter["profile"] = "0";
    $('#Filter_Profile').attr('src', '/assests/img/Face/profile.png')
    face_data_filter["mess"] = "0";
    $('#Filter_Mess').attr('src', '/assests/img/Face/mess.png')
    face_data_filter["com"] = "0";
    $('#Filter_Comment').attr('src', '/assests/img/Face/comment.png')
    face_data_filter["reply"] = "0";
    $('#Filter_Reply').attr('src', '/assests/img/Face/reply.png')
    face_data_filter["new"] = "0";
    $('#Filter_New').attr('src', '/assests/img/Face/new.png')
    face_data_filter["star"] = "0";
    $('#Filter_Star').attr('src', '/assests/img/Face/star.png')

    face_fitler_tag = "";
    FaceFilter_Tag_Reset();
    face_fitler_staff = "";
    FaceFilter_Staff_Reset();
    face_fitler_date = "";
    FaceFilter_Date_Reset();
    Face_Load_Conversation_List(numload_conver_begin, 0, 0, 0, "");
    Face_Filter_Executing();
}
function FaceFilter_Profile() {

    if (face_data_filter["profile"] == undefined || face_data_filter["profile"] == "0") {
        $('#Filter_Profile').attr('src', '/assests/img/Face/profile_.png')
        face_data_filter["profile"] = "1";
    }
    else if (face_data_filter["profile"] == "1") {
        $('#Filter_Profile').attr('src', '/assests/img/Face/profile__.png')
        face_data_filter["profile"] = "2";
    }
    else {
        face_data_filter["profile"] = "0";
        $('#Filter_Profile').attr('src', '/assests/img/Face/profile.png')
    }
    Face_Filter_Executing();
}
function FaceFilter_Mess() {
    if (face_data_filter["mess"] == undefined || face_data_filter["mess"] == "0") {
        $('#Filter_Mess').attr('src', '/assests/img/Face/mess_.png')
        face_data_filter["mess"] = "1";
    }
    else {
        face_data_filter["mess"] = "0";
        $('#Filter_Mess').attr('src', '/assests/img/Face/mess.png')
    }
    Face_Filter_Executing();
}
function FaceFilter_Comment() {
    if (face_data_filter["com"] == undefined || face_data_filter["com"] == "0") {
        $('#Filter_Comment').attr('src', '/assests/img/Face/comment_.png')
        face_data_filter["com"] = "1";
    }
    else {
        face_data_filter["com"] = "0";
        $('#Filter_Comment').attr('src', '/assests/img/Face/comment.png')
    }
    Face_Filter_Executing();
}
function FaceFilter_Reply() {
    if (face_data_filter["reply"] == undefined || face_data_filter["reply"] == "0") {
        $('#Filter_Reply').attr('src', '/assests/img/Face/reply_.png')
        face_data_filter["reply"] = "1";
    }
    else {
        face_data_filter["reply"] = "0";
        $('#Filter_Reply').attr('src', '/assests/img/Face/reply.png')
    }
    Face_Filter_Executing();
}
function FaceFilter_New() {
    if (face_data_filter["new"] == undefined || face_data_filter["new"] == "0") {
        $('#Filter_New').attr('src', '/assests/img/Face/new_.png')
        face_data_filter["new"] = "1";
    }
    else {
        face_data_filter["new"] = "0";
        $('#Filter_New').attr('src', '/assests/img/Face/new.png')
    }
    Face_Filter_Executing();
}
function FaceFilter_Star() {
    if (face_data_filter["star"] == undefined || face_data_filter["star"] == "0") {
        $('#Filter_Star').attr('src', '/assests/img/Face/star_.png')
        face_data_filter["star"] = "1";
    }
    else {
        face_data_filter["star"] = "0";
        $('#Filter_Star').attr('src', '/assests/img/Face/star.png')
    }
    Face_Filter_Executing();
}


// #region // Staff
function FaceFilter_Staff_Event() {
    face_fitler_staff = $('#TokenTagStaff').dropdown('get value') ? $('#TokenTagStaff').dropdown('get value') : ''
    Face_Filter_Executing();
}
function FaceFilter_Staff_Close() {
    $('#popupFiter_Staff').hide();
    if (face_fitler_staff == "") $('#Filter_Staff').attr('src', '/assests/img/Face/user.png')
    else $('#Filter_Staff').attr('src', '/assests/img/Face/user_.png')
}
function FaceFilter_Staff_Reset() {
    $("#TokenTagStaff").dropdown("refresh");
    $("#TokenTagStaff").dropdown("clear");
    face_fitler_staff = "";
    $('#Filter_Staff').attr('src', '/assests/img/Face/user.png')
    $('#popupFiter_Staff').hide();
    Face_Filter_Executing();
}

// #endregion

// #region // TAG

function FaceFilter_Tag_Close() {
    $('#popupFiter_Tag').hide();
    if (face_fitler_tag == "") $('#Filter_Tag').attr('src', '/assests/img/Face/tag.png')
    else $('#Filter_Tag').attr('src', '/assests/img/Face/tag_.png')
}
function FaceFilter_Tag_Reset() {
    var x = document.getElementsByClassName("facebooktagfilter");
    for (let i = 0; i < x.length; i++) {
        x[i].checked = false
    }

    face_fitler_tag = "";
    $('#Filter_Tag').attr('src', '/assests/img/Face/tag.png')
    $('#popupFiter_Tag').hide();
    Face_Filter_Executing();
}
function FaceFilter_Tag_Event() {
    $(".facebooktagfilter").change(function () {
        face_fitler_tag = "";
        var x = document.getElementsByClassName("facebooktagfilter");
        for (let i = 0; i < x.length; i++) {
            if (x[i].checked) {
                face_fitler_tag = face_fitler_tag + x[i].value + ",";
            }
        }
        Face_Filter_Executing();
    });
}



// #endregion

// #region // Date

function FaceFilter_Date_Reset() {

    var now = new Date();
    var datatime = formatDateClient(now.setDate(now.getDate())) + ' to ' + formatDateClient(new Date());
    $("#dataDateFiter").flatpickr({
        dateFormat: 'd-m-Y',
        mode: "range",
        enableTime: false,
        inline: true,
        defaultDate: datatime
    });
    face_fitler_date = "";
    $('#Filter_date').attr('src', '/assests/img/Face/calendar.png')
    $('#popupFiter_Tag').hide();
    $('#filter_date_time_alarm').prop('checked', false);

    Face_Filter_Executing();
}
function FaceFilter_Date_Close() {
    $('#popupFiter_Date').hide();
    if (face_fitler_date == "") $('#Filter_date').attr('src', '/assests/img/Face/calendar.png')
    else $('#Filter_date').attr('src', '/assests/img/Face/calendar_.png')
}
function FaceFilter_Date_Event() {

    $(".itemdateFiter").click(function () {
        let day = $(this).attr('data-date') ? Number($(this).attr('data-date')) : 0;
        var now = new Date();
        var datatime = '';
        if (day != 1)
            datatime = formatDateClient(now.setDate(now.getDate() - day)) + ' to ' + formatDateClient(new Date());
        else {
            var now1 = new Date();
            datatime = formatDateClient(now1.setDate(now.getDate() - day)) + ' to ' + formatDateClient(now.setDate(now.getDate() - day));
        }
        $("#dataDateFiter").flatpickr({
            dateFormat: 'd-m-Y',
            mode: "range",
            enableTime: false,
            inline: true,
            defaultDate: datatime
        });
        $("#dataDateFiter").val(datatime);


    });
}
function FaceFilter_Date_OK() {
    face_fitler_date = $('#dataDateFiter').val() ? $('#dataDateFiter').val() : new Date();
    let loadmore = 1;
    let filter = 1;
    Face_Load_Conversation_List(numload_conver_begin, loadmore, filter, 0, "");
    FaceFilter_Date_Close();
}
// #endregion

function Face_Fitler_Click() {
    $('#popupFiter_Tag').hide();
    $('#popupFiter_Staff').hide();
    $('#popupFiter_Date').hide();
    $('#popupMessengerTemplate').hide();
}
function Face_Filter_Executing() {
    let _x = document.querySelectorAll('.messegerItem,.messegerCommentItem')
    for (l = 0; l < _x.length; l++) {
        _x[l].style.display = 'block';
    }
    let staff_filter = "," + face_fitler_staff + ",";
    let tag_filter = (face_fitler_tag != "") ? face_fitler_tag.split(",") : [];

    let _staff, _tag;
    for (l = 0; l < _x.length; l++) {
        if ((face_data_filter["profile"] == "1" && _x[l].attributes.data_ticket.value == "0") ||
            (face_data_filter["profile"] == "2" && _x[l].attributes.data_ticket.value != "0")) {
            _x[l].style.display = 'none';
        }
        if (face_data_filter["mess"] == "1" && _x[l].attributes.data_typeconver.value == "com") {
            _x[l].style.display = 'none';
        }
        if (face_data_filter["com"] == "1" && _x[l].attributes.data_typeconver.value == "mess") {
            _x[l].style.display = 'none';
        }
        if (face_data_filter["reply"] == "1" && $('#smallavatar_page_' + _x[l].id).is(':visible')) {
            _x[l].style.display = 'none';
        }
        if (face_data_filter["star"] == "1" && !$('#star_' + _x[l].id).is(':visible')) {
            _x[l].style.display = 'none';
        }

        if (face_data_filter["new"] == "1" && !_x[l].className.includes('newMessage')) {
            _x[l].style.display = 'none';
        }

        if (staff_filter != ",,") {
            _staff = "," + _x[l].attributes.data_staff.value + ",";
            if (!staff_filter.includes(_staff)) {
                _x[l].style.display = 'none';
            }
        }
        if (tag_filter != [] && tag_filter.length != 0) {
            let show = 0;
            _tag = "," + _x[l].attributes.data_tag.value + ",";
            for (jj = 0; jj < tag_filter.length - 1; jj++) {
                if (_tag.includes("," + tag_filter[jj] + ",")) {
                    show = 1;
                    jj = tag_filter.length - 1;
                }
            }
            if (show == 0) _x[l].style.display = 'none';
        }
    }
}
function Face_Filter_Rasing_Event() {
    $("#Filter_date").click(function (event) {
        Face_Fitler_Click();
        $('#popupFiter_Date').show();
        event.stopPropagation();
    });
    $("#popupFiter_Date").click(function (event) {
        event.stopPropagation();
    });


    $("#Filter_Tag").click(function (event) {
        Face_Fitler_Click();
        $('#popupFiter_Tag').show();
        event.stopPropagation();
    });
    $("#popupFiter_Tag").click(function (event) {
        event.stopPropagation();
    });


    $("#Filter_Staff").click(function (event) {
        Face_Fitler_Click();
        $('#popupFiter_Staff').show();
        event.stopPropagation();
    });
    $("#popupFiter_Staff").click(function (event) {
        event.stopPropagation();
    });
    $(document).click(function () {
        FaceFilter_Date_Close();
        FaceFilter_Tag_Close();
        FaceFilter_Staff_Close();
        Face_Clear_Search_Teamplate();
    });

    var input = document.getElementById("searchingFacebook");
    input.addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            Face_SearchConversation();
        }
    });
    $("#btnMessengerTemplate").click(function (event) {
        Face_Fitler_Click();
        $('#popupMessengerTemplate').show();
        event.stopPropagation();
    });
    $("#header_popup_teamplate,#searchingMessengerTemplate").click(function (event) {
        event.stopPropagation();
    });


    
}
function onKeyupSearchingTeamplate() {
    let search = xoa_dau($('#searchingMessengerTemplate').val().toLowerCase());
    let data = face_data_load_initialize_template.filter(word => {
        return (xoa_dau(word["Title"]).toLowerCase().includes(search));
    });
    if (data != undefined && data != null && data.length != 0) {
        Face_Render_Template(data, "MessengerTemplateBody");
        Face_Search_Teamplate_Event();
        ColorSearchFilterText(search, "seachRow");
    }
}
function Face_Search_Teamplate_Event() {
    $(".face_template_div").on('click', function (event) {
        let id = this.id;
        id = id.replace('face_template_', 'face_template_description_'); 
        $('#txtContentMessage_Chatting').val($('#' + id).html());
        $('#popupMessengerTemplate').hide();
        event.stopPropagation();
    });
}
function Face_SearchConversation() {
    let keyword = $('#searchingFacebook').val();
    if (keyword.length >= 3) {
        Face_Load_Conversation_List(numload_conver_begin, 0, 0, 1, keyword);
    }
}
function Face_RefreshLoad() {
    $("#searchingFacebook").val('');
    FaceFilter_Clear();
}

function Face_Clear_Search_Teamplate() {
    $('#popupMessengerTemplate').hide();
    $('#searchingMessengerTemplate').val('');
    Face_Render_Template(face_data_load_initialize_template, "MessengerTemplateBody");
    Face_Search_Teamplate_Event();
}
// #endreion