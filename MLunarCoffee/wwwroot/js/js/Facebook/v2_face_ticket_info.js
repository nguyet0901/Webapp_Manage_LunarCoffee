function Face_Load_Time() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Load_Time"
        , data = { 'page': face_value_current_page, 'client': face_value_current_client }, async = true, error = null
        , success = function (result) {
            if (result != "0") Face_Render_Time(JSON.parse(result), "face_ticket_time");
        }
    ); 
}

function Face_Render_Time(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let dr = '<li class="clearfix"><div class="message-data align-right">'
                    + '<span class="message-data-time">' + ConvertDateTimeToStringDMY_HM(item.created) + '</span>'
                    + '<span class="message-data-name">' + item.username + '</span> <i class="icon circle me"></i>'
                    + '</div>'
                    + '<div class="message other-message float-right">'
                    + ConvertDateTimeToStringDMY_HM(item.callbacktime)
                    + '</div>'
                    + '</li>'
                stringContent = stringContent + dr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Load_Note() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Load_Note"
        , data = { 'page': face_value_current_page, 'client': face_value_current_client }, async = true, error = null
        , success = function (result) {
            if (result != "0") Face_Render_Note(JSON.parse(result), "face_ticket_note");
        }
    );
}
function Face_Render_Note(data, id) {

    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let dr = '<li class="clearfix"><div class="message-data align-right">'
                    + '<span class="message-data-time">' + ConvertDateTimeToStringDMY_HM(item.created) + '</span>'
                    + '<span class="message-data-name">' + item.username + '</span> <i class="icon circle me"></i>'
                    + '</div>'
                    + '<div class="message other-message float-right">'
                    + item.content
                    + '</div>'
                    + '</li>'
                stringContent = stringContent + dr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}

function Face_Update_Design_Staff(id, staff, staffname) {
    try {
        $('#' + id).attr("data_staff", staff)
        face_data_conversation_list[id].staff = staff;
        face_data_conversation_list[id].staffname = staffname;
        $('#staffname_' + id).html(staffname);
    }
    catch (exception) {

    }
}
function Face_Update_Design_Ticket(id, ticketid) {
    try {
        $('#' + id).attr("data_ticket", ticketid)
        face_data_conversation_list[id].ticket = ticketid
        $('#_img_profile_' + id).show();
    }
    catch (exception) {

    }
}

function Face_Update_Design_ServiceCare(id, ServiceCareToken) {
    try {
        $('#' + id).attr("data_servicecare", ServiceCareToken)
        face_data_conversation_list[id].servicecare = ServiceCareToken;
    }
    catch (exception) {

    }
}
function Face_Update_Design_Discount(id, discount, discountname) {
    try {
        $('#' + id).attr("data_discount", discount)
        $('#' + id).attr("data_discountname", discountname)
        face_data_conversation_list[id].discount = discount;
        face_data_conversation_list[id].discountname = discountname;
    }
    catch (exception) {

    }
}
function Face_Update_Design_Area(id, area) {
    try {
        $('#' + id).attr("data_area", area)
        face_data_conversation_list[id].area = area;
    }
    catch (exception) {

    }
}
function Face_Update_Design_Tag(id, tag) {
    try {
        $('#' + id).attr("data_tag", tag)
        face_data_conversation_list[id].tag = tag;
        let tagname = Face_Render_Tag(tag);
        $('#tag_' + id).html(tagname);
    }
    catch (exception) {

    }
}

function Face_SaveServiceCare() {
    let ServiceCareToken = $('#ServiceCareFace').dropdown('get value') ? $('#ServiceCareFace').dropdown('get value') : ''
    if (ServiceCareToken != null && facebook_begin_save_ticket == 1) {
        if (face_value_current_mess_post == "MESS") {

            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveServiceCare"
                , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'servicecare': ServiceCareToken }, async = true
                , error = function () {
                    Face_Noti_Failure_Execute_Ticket();
                }
                , success = function (result) {
                    if (result == "0") Face_Noti_Failure_Execute_Ticket();
                    else {
                        Face_Noti_Success_Execute_Ticket();
                        Face_Update_Design_ServiceCare(face_value_current_conver_id, ServiceCareToken)
                    }
                }
            );
             
        }
    }
}
function Face_SaveArea() {
    let area = (Number($('#AreaFace').dropdown('get value')) ? Number($('#AreaFace').dropdown('get value')) : 0)
    if (area != 0 && facebook_begin_save_ticket == 1) {
        if (face_value_current_mess_post == "MESS") {
            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveArea"
                , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'area': area }, async = true
                , error = function () {
                    Face_Noti_Failure_Execute_Ticket();
                }
                , success = function (result) {
                    if (result == "0") Face_Noti_Failure_Execute_Ticket();
                    else {
                        Face_Noti_Success_Execute_Ticket();
                        Face_Update_Design_Area(face_value_current_conver_id, area)
                    }
                }
            );
            
        }
    }
}
function Face_SaveDiscount() {
    let discount = $('#DiscountFace').dropdown('get value') ? $('#DiscountFace').dropdown('get value') : 0;
    let discountname = $('#DiscountFace').dropdown('get text') ? $('#DiscountFace').dropdown('get text') : 0;
    if (face_value_current_mess_post == "MESS" && facebook_begin_save_ticket == 1) {
        AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveDiscount"
            , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'discount': discount }, async = true
            , error = function () {
                Face_Noti_Failure_Execute_Ticket();
            }
            , success = function (result) {
                if (result == "0") Face_Noti_Failure_Execute_Ticket();
                else {
                    Face_Noti_Success_Execute_Ticket();
                    Face_Update_Design_Discount(face_value_current_conver_id, discount, discountname)
                }
            }
        );
         
    }
}
function Face_SaveStaff() {
    let staff = $('#DectectStaffOnConver').dropdown('get value') ? $('#DectectStaffOnConver').dropdown('get value') : 0;
    let staffname = $('#DectectStaffOnConver').dropdown('get text') ? $('#DectectStaffOnConver').dropdown('get text') : 0;
    if (face_value_current_mess_post == "MESS" && facebook_begin_save_ticket == 1) {
        AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveStaff"
            , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'staff': staff }, async = true
            , error = function () {
                Face_Noti_Failure_Execute_Ticket();
            }
            , success = function (result) {
                if (result == "0") Face_Noti_Failure_Execute_Ticket();
                else {
                    Face_Noti_Success_Execute_Ticket();
                    Face_Update_Design_Staff(face_value_current_conver_id, staff, staffname)
                }
            }
        );
         
    }
}
function Face_SaveTag() {
    $(".facebooktag").change(function () {
        if (face_value_current_mess_post == "MESS" && facebook_begin_save_ticket == 1) {
            let currentchecktag = "";
            var x = document.getElementsByClassName("facebooktag");
            for (let i = 0; i < x.length; i++) {
                if (x[i].checked) {
                    currentchecktag = currentchecktag + x[i].value + ",";
                }
            }
            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveTag"
                , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'tag': currentchecktag }, async = true
                , error = function () {
                    Face_Noti_Failure_Execute_Ticket();
                }
                , success = function (result) {
                    if (result == "0") Face_Noti_Failure_Execute_Ticket();
                    else {
                        Face_Noti_Success_Execute_Ticket();
                        facebook_begin_save_ticket = 0;
                        Face_Update_Design_Tag(face_value_current_conver_id, currentchecktag);
                        facebook_begin_save_ticket = 1;
                    }
                }
            );
            
        }
    });
}
function Face_SaveNote() {
    let note = ($('#txtFace_Note').val()).trim();
    $('#txtFace_Note').val('');
    if (note != "" && facebook_begin_save_ticket == 1) {
        if (face_value_current_mess_post == "MESS") {
            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveNote"
                , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'note': note }, async = true               
                , error = function () { notiError(); }
                , success = function (result) {
                    if (result == "0") notiError();
                    else Face_Load_Note();
                }
            );
           
        }
    }
}
function Face_SaveCallBackTime() {
    let callbacktime = $('#Date_from_call_back').val() ? $('#Date_from_call_back').val() : "";
    if (facebook_begin_save_ticket == 1) {
        if (face_value_current_mess_post == "MESS") {
            AjaxLoad(url = "/Facebook/Messenger/?handler=Face_SaveCallBackTime"
                , data = { 'page': face_value_current_page, 'client': face_value_current_client, 'callbacktime': callbacktime }, async = true
                , error = function () { notiError(); }
                , success = function (result) {
                    if (result == "0") notiError();
                    else Face_Load_Time();
                }
            );
             
        }
    }
}

function Face_Noti_Success_Execute_Ticket() {
    let _obj = document.getElementById('face_noti_success');
    _obj.classList.remove('show');
    void _obj.offsetWidth;
    _obj.classList.add('show');

}
function Face_Noti_Failure_Execute_Ticket() {
    let _obj = document.getElementById('face_noti_failure');
    _obj.classList.remove('show');
    void _obj.offsetWidth;
    _obj.classList.add('show');

}
function Face_Fill_Data_When_Click_Page(pageid) {
    if (face_data_load_initialize_page != undefined) {
        let _data = face_data_load_initialize_page[pageid]
        $('#namePageCurrent_Executing').html(_data.name);
        $('#avatarPageCurrent_Executing').attr("src", face_link_image_fanpage.replace('{keycode}', pageid));
    }

}
function Face_Fill_Data_When_Click_Comment(avatar) {
    facebook_begin_save_ticket = 0;
    $("#DectectStaffOnConver").hide();
    $("#ticketinfoLink").hide();
    $("#ticketinfoFace").hide();

    $('#profile_pageID').html(face_value_current_page);
    $('#profile_PSID').html(face_value_current_client);
    $('#face_name').html(face_value_current_client_name);
    $('#face_email').html(face_value_current_client_name);
    Face_Load_Image_Asyn("profile_Avatar", avatar)
    facebook_begin_save_ticket = 1;
}
function Face_Fill_Data_When_Click_Message(tag, servicecare, discount, area, discountname, star, staff, avatar) {
    facebook_begin_save_ticket = 0;
    $('#profile_pageID').html(face_value_current_page);
    $('#profile_PSID').html(face_value_current_client);
    $('#face_name').html(face_value_current_client_name);
    $('#face_email').html(face_value_current_client_name);
    Face_Load_Image_Asyn("profile_Avatar", avatar)
    Face_Fill_Data_When_Click_Message_Tag(tag);
    Face_Fill_Data_When_Click_Message_Ticket_Info();
    $("#ServiceCareFace").dropdown("refresh");
    $("#ServiceCareFace").dropdown("clear");
    if (servicecare != "") {
        $("#ServiceCareFace").dropdown("set selected", servicecare.split(","));
    }
    Face_Obj_Datecallback_time.setDate(new Date());
    $('#txtFace_Note').val('')

    $("#AreaFace").dropdown("refresh");
    $("#AreaFace").dropdown("clear");
    $("#AreaFace").dropdown("set selected", area);

    $("#DiscountFace").dropdown("refresh");
    $("#DiscountFace").dropdown("clear");
    $("#DiscountFace").dropdown("set selected", discount);
    $("#DectectStaffOnConver").dropdown("refresh");
    $("#DectectStaffOnConver").dropdown("clear");
    $("#DectectStaffOnConver").dropdown("set selected", staff);
    $('#ticketinfoFace_firsttab').click();
    $("#ticketinfoFace_notetab").click(function () {
        Face_Load_Note();
    });
    $("#ticketinfoFace_timetab").click(function () {
        Face_Load_Time();
    });
    $("#ticketinfoFace_history").click(function () {
        Face_Load_Data_History_Comment();
    });
    $("#DectectStaffOnConver").show();
    $("#ticketinfoLink").show();
    $("#ticketinfoFace").show();
    if (face_value_current_client_name == '')
        Face_Get_Info_Click(face_value_current_client);

    facebook_begin_save_ticket = 1;
}
function Face_Get_Info_Click(_id) {
    let _url = facebook_link_get_info_client.replace('{version}', facebook_version);
    _url = _url.replace('{client}', _id) + "?access_token=" + Face_Get_Token_Access_From_PageID(face_value_current_page);;
    $.ajax({
        url: _url,
        dataType: "json",
        type: "GET",
        contentType: "application/json",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (XMLHttpRequest.responseJSON.error.code == 10) {
                Face_Error_API(10, (XMLHttpRequest.responseJSON != undefined) ? XMLHttpRequest.responseJSON.error.message : "");
            }
        },
        success: function (result) {
            if (result != null && result != undefined) {
                face_value_current_client_name = result.first_name + result.last_name
                $('#clientname_' + face_value_current_conver_id).html(face_value_current_client_name);
                $('#face_name').html(face_value_current_client_name);
                $('#face_email').html(face_value_current_client_name);


            }
        }
    });
}

function Face_Event_Ticket_Area() {
    $("#face_tic_linkface").click(function (event) {
        let link = facebook_link_inbox_mail_teamplate.replace('{link}', facebook_link_inbox_mail);
        window.open(link);
    });
}
function Face_Fill_Data_When_Click_Message_Ticket_Info() {
    $('#ticketinfo').hide();
    $('#ticketinfoLink').hide();
    $('#face_tic_blocked').hide();
    $('#face_tic_un_block').hide();
    $('#face_area_get_phone').hide();
    $("#face_tic_profile").unbind();
    $("#face_tic_ticket").unbind();
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Load_TicketInfo"
        , data = { 'page': face_value_current_page, 'client': face_value_current_client }, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data != undefined && data.length == 1) {

                    $('#profile_name_facebook').html(data[0].name);
                    $('#profile_phone1').html(data[0].phone);
                    $('#profile_phone2').html(data[0].phone2);
                    $('#profile_gender').html(data[0].gender);
                    $('#profile_birthday').html(ConvertDateTimeUTC_DMY(ConvertToDateRemove1900(data[0].birthday)));
                    $('#ticketinfo').show();

                    $('#face_tic_newticket').hide();
                    if (Number(data[0].customer) != 0) {
                        $('#face_tic_profile').show();
                        $("#face_tic_profile").unbind().click(function (event) {
                            window.open("/Customer/MainCustomer?CustomerID=" + Number(data[0].customer) + "&ver=" + versionofWebApplication);
                        });
                    }
                    else $('#face_tic_profile').hide();
                    $('#face_tic_ticket').show();

                    $("#face_tic_ticket").unbind().click(function (event) {
                        window.open("/Marketing/TicketAction?TicketID=" + Number(data[0].ID) + "&ver=" + versionofWebApplication);
                    });
                    Face_Update_Design_Ticket(face_value_current_conver_id, data[0].ID);



                }
                else {
                    $('#ticketinfo').hide();
                    $('#face_tic_newticket').show();
                    $('#face_tic_profile').hide();
                    $('#face_tic_ticket').hide();
                    $('#face_area_get_phone').show();
                }
            }
            else $('#ticketinfo').hide();
        },
        sender = null,
        before = function () {
            $('#ticketinfoLink').hide();
        },
        complete = function (e) {
            $('#ticketinfoLink').show();
        }
    );
     
}
function Face_Fill_Data_When_Click_Message_Tag(tag) {
    tag = "," + tag + ",";
    let __x = document.getElementsByClassName('facebooktag');
    if (__x != undefined) {
        for (__i = 0; __i < __x.length; __i++) {
            let __id = __x[__i].id.replace('tickettag', '');
            __id = ',' + __id + ',';
            if (tag.includes(__id)) {
                __x[__i].checked = true;
            }
            else __x[__i].checked = false;
        }
    }
}
function Face_Load_Data_History_Comment() {
    AjaxLoad(url = "/Facebook/Messenger/?handler=Face_Loading_History_Comment"
        , data = { 'page': face_value_current_page, 'client': face_value_current_client }, async = true, error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result).Table;
                Face_Render_History_Comment(data, "history_comment_div");
                let dataTime = JSON.parse(result).Table1;
                let starttime = ConvertDateTimeUTC_Time_DOWFULLDAY(dataTime[0].starttime);
                let closettime = ConvertDateTimeUTC_Time_DOWFULLDAY(dataTime[0].closettime);
                $('#face_history_begin_chat').html(starttime);
                $('#face_history_closed_chat').html(closettime);
            }
        }
    );
     
}
function Face_Render_History_Comment(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        //

        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let tr = "";
                let item = data[i];
                let created_time = ConvertDateTimeUTC_Time_DOWFULLDAY(item.created);
                let classname = '';

                if (i % 2 != 0) classname = "history_face_comment_event";
                tr = '<div class="history_face_comment ' + classname + '">'
                    + '<a target="_blank" href="' + item.url + '">' + created_time + '</a>'
                    + '</div>'
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Render_Message_Having_Phone(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length && i < 5; i++) {
                let tr = "";
                let item = data[i];
                let created_time = ConvertDateTimeUTC_Time_DOWFULLDAY(item.created_time);

                stringContent = stringContent + Face_Render_Message_Having_Phone_Each(created_time, item.phone);
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Face_Render_Message_Having_Phone_Each(created_time, phone) {
    let tr = '<li class="clearfix"><div class="message-data align-right" style="height: 20px;margin-bottom: 0px !important;">'
        + '<span class="message-data-time get_phone_phone_label_face">'
        + created_time + '</span>'
        + '</div><div class="get_phone_phone_item_face">'
        + phone
        + '</div></li>';
    return tr;
}

function Face_Event_Click_Create_Ticket() {
    $(".get_phone_phone_item_face").on('click', function (event) {
        let phone = this.innerHTML;
        let ServiceCareToken = $('#ServiceCareFace').dropdown('get value') ? $('#ServiceCareFace').dropdown('get value') : ''
        $("#DetailModal_Content").html('');
        $("#DetailModal_Content").load("/Marketing/TicketDetail?phone=" + phone + "&name=" + encodeURI(face_value_current_client_name) + "&faceid=" + face_value_current_client + "&KeyPage=" + face_value_current_page + "&servicecare=" + ServiceCareToken + "&ver=" + versionofWebApplication);
        $('#DetailModal').modal('show');
        return false;
    });
}
function Face_Event_NewTicket_Area() {
    $("#face_tic_newticket").click(function (event) {
        let ServiceCareToken = $('#ServiceCareFace').dropdown('get value') ? $('#ServiceCareFace').dropdown('get value') : ''
        $("#DetailModal_Content").html('');
        $("#DetailModal_Content").load("/Marketing/TicketDetail?name=" + encodeURI(face_value_current_client_name) + "&faceid=" + face_value_current_client + "&KeyPage=" + face_value_current_page + "&servicecare=" + ServiceCareToken + "&ver=" + versionofWebApplication);
        $('#DetailModal').modal('show');
        return false;
    });
}
function Face_Load_Ticket_Profile() {
    Face_Fill_Data_When_Click_Message_Ticket_Info();
}