var Desk_ColorCode_Type_Announcement = {
    New: '<span class="badge badge-md bg-gradient-success">new</span>'
    , Edit: '<span class="badge badge-md bg-gradient-info">change</span>'
    , Delete: '<span class="badge badge-md bg-gradient-danger">delete</span>'
};
var Desk_Name_Type_Announcement = {edit: "CHANGE", movein: "IN", moveout: "OUT"};


function Desk_Add_Announcement (data, curtime, type_anno, oldroom, newroom) {

    let idDiv = 'announce_desk_content_' + type_anno;
    let classname_item = 'announcement_item_' + type_anno;
    let classname_remove = 'button_remove_noti_app_' + type_anno;
    let preappid = 'announce_item_' + type_anno;
    let appid = preappid + '_' + data.appointment_id;
    Desk_Remove_announcement(appid);
    Desk_Annouce_Delete_Annouce_Cookie(data.appointment_id, type_anno);
    var myNode = document.getElementById(idDiv);
    if (myNode != null) {
        let row = '<div id="' + appid + '" class="' + classname_item + ' announcement_general card overflow-hidden" data-id="' + data.appointment_id + '" >'
            + '<div class="p-2 ps-2 ">'
            + '<p class="text-sm mb-0 text-capitalize font-weight-bold content">'
            + '<i class="remove icon desk_delete_button ' + classname_remove + '" data-preappid="' + preappid + '" data-id="' + data.appointment_id + '" ></i>'

            + '<span class="badge badge-md bg-gradient-secondary me-2">' + curtime + '</span>'
            + Desk_Announcement_Type(data.type)
            + '<a class="ms-2 link" target="_blank" href="/Customer/MainCustomer?CustomerID=' + data.cust_id + '&ver=' + versionofWebApplication + '">' + data.cust_name + '</a>'
            + '</p>'
            // + '<p class="text-sm mb-0">'

            //// + Desk_Name_Announcement_By_Type_To_String(data.type, type_anno, oldroom, newroom)

            // + '</p>'
            + '</div>'
            + '</div>';

        //let row1 = '<div id="' + appid + '" class="' + classname_item + ' announcement_general" style="right:0px;"   data-id="' + data.appointment_id + '">'
        //    + '<div class="Announcements-desk__item last-info" style="background-color:' + Desk_ColorCode_By_Type(data.type) + '">'
        //    + '<p class="margin-b-0 ellipsis_one_line">'
        //    + '<a class="link" target="_blank" href="/Customer/MainCustomer?CustomerID=' + data.cust_id + '&ver=' + versionofWebApplication + '">'
        //    + data.cust_name + '</a>'
        //    + '</p></br>'
        //    + '<p class="margin-b-0 ellipsis_one_line">'
        //    + '<span class="_cur_time_info">' + curtime + '</span>'
        //    + '<span class="_cur_name_info">' + Desk_Name_Announcement_By_Type_To_String(data.type, type_anno, oldroom, newroom) + '</span>'
        //    + '</p>'
        //    + '<i class="remove icon desk_delete_button ' + classname_remove + '" data-preappid="' + preappid + '" data-id="' + data.appointment_id + '" ></i>'
        //    + '</div>'
        //    + '</div>'

        $("#" + idDiv).append(row);
        Desk_Annouce_Add_Annouce_Cookie(data, type_anno, oldroom, newroom);

        //animation new announcement
        let item_anno = $('#' + appid + '.' + classname_item);
        if (!item_anno.is(':first-child')) {
            item_anno.insertBefore('.' + classname_item + ':first-child').css({top: "-60px", opacity: 0}).animate({top: '0', opacity: 1}, 500);
        }
        else {
            //item_anno.css({ top: "-60px", opacity: 0 }).animate({ top: '0', opacity: 1 }, 500);
        }
    }

    //Event Remove Annouce
    $('.' + classname_remove).on("click", function () {
        let _id = Number($(this).attr("data-id"));
        let preappid = $(this).attr("data-preappid");
        let _appid = preappid + '_' + _id;
        if ($('#' + _appid).length > 0) {
            $('#' + _appid).css({left: "0"}).animate({left: '100%'}, 500, function () {
                $('#' + appid).remove();
            });
            Desk_Annouce_Delete_Annouce_Cookie(_id, type_anno)
        }
    })


    if ($('.' + classname_item).length > 10) {
        let last_item = $('.' + classname_item + ':last-child');
        if (last_item.length > 0) {
            let _id = last_item.attr("data-id");
            $('.' + classname_item + ':last-child').remove();
            Desk_Annouce_Delete_Annouce_Cookie(_id, type_anno)
        }
    }
}


function Desk_Annouce_Add_Annouce_Cookie (item, type_anno, oldroom, newroom) {

    let e = {};
    var d = new Date();
    e.appointment_id = item.appointment_id;
    e.cust_id = item.cust_id;
    e.cust_code = item.cust_code;
    e.cust_name = item.cust_name;
    e.type = item.type;
    e.branch_id = item.branch_id;
    e.status_id = item.status_id;
    e.doctor_id = item.doctor_id;
    e.type_anno = type_anno;
    e.oldroom = (oldroom != undefined && oldroom != 'undefined') ? oldroom : '';
    e.newroom = (newroom != undefined && newroom != 'undefined') ? newroom : '';
    e.date = d.getFullYear() + d.getMonth().toString().padStart(2, "0") + d.getDate().toString().padStart(2, "0")
        + d.getHours().toString().padStart(2, "0")
        + d.getMinutes().toString().padStart(2, "0");
    let _data = Desk_Get_Local_Storage(nameCookie_annouce);
    _data.push(e);
    Desk_Set_Local_Storage(nameCookie_annouce, _data)
}
function Desk_Get_Local_Storage (localname) {
    let __currentdata = localStorage.getItem(localname);
    if (__currentdata == null || __currentdata == '' || __currentdata == undefined || __currentdata == '[]') {
        localStorage.setItem(localname, '');
        return [];
    }

    else return JSON.parse(__currentdata);


}
function Desk_Set_Local_Storage (localnamem, _data) {
    localStorage.setItem(localnamem, JSON.stringify(_data));
}

async function Desk_Annouce_Delete_Annouce_Cookie (id, type_anno) {
    let _data = Desk_Get_Local_Storage(nameCookie_annouce);
    for (i = 0; i < _data.length; i++) {
        if (_data[i].type_anno == type_anno && _data[i].appointment_id == id) {
            _data.splice(i, 1);
            i = _data.length;
        }
    }
    Desk_Set_Local_Storage(nameCookie_annouce, _data)
}

async function Desk_Restore_Annouce_From_Cookie () {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {

                let d = new Date();
                let curdate = d.getFullYear() + d.getMonth().toString().padStart(2, "0") + d.getDate().toString().padStart(2, "0");
                let index_date, index_item;

                let _data = Desk_Get_Local_Storage(nameCookie_annouce);
                for (i = _data.length - 1; i >= 0; i--) {
                    index_date = _data[i].date.substring(0, 8);
                    if (index_date != curdate) {
                        _data.splice(i, 1);
                    }
                }

                if (_data.length > 0) {
                    _data.sort((a, b) => (Number(a.date) < Number(b.date)) ? 1 : -1)
                    for (__i = 0; __i < _data.length; __i++) {
                        let _item = _data[__i];
                        index_date = _item.date.substring(0, 8);
                        index_item = _item.date.substring(8, 10) + ':' + _item.date.substring(10, 12);
                        if (index_date == curdate) {
                            if (Number(_item.branch_id) == Master_branchID) {

                                if (Is_Desk_Branch == 1 && _item.type_anno == "branch")
                                    Desk_Add_Announcement(_item, index_item, _item.type_anno);
                                if (Is_Desk_Doctor == 1 && _item.type_anno == "doctor" && Number(_item.doctor_id) == Desk_Manage_Doctor && Desk_Manage_Doctor != 0)
                                    Desk_Add_Announcement(_item, index_item, _item.type_anno);
                                if (Is_Desk_Cashier == 1 && _item.type_anno == "cashier" && Number(_item.status_id) == Desk_StatusCashier)
                                    Desk_Add_Announcement(_item, index_item, _item.type_anno);
                                if (Is_Desk_Xquang == 1 && _item.type_anno == "xquang" && Number(_item.status_id) == Desk_StatusXray)
                                    Desk_Add_Announcement(_item, index_item, _item.type_anno);
                            }
                        }
                    }
                }
                resolve()
            },

        )
    });
}


function Desk_Annouce_Delete_Annouce_Cookie_All_Type (type_anno) {
    let _data = Desk_Get_Local_Storage(nameCookie_annouce);
    for (i = _data.length - 1; i >= 0; i--) {

        if (_data[i].type_anno == type_anno) {
            _data.splice(i, 1);
        }
    }
    Desk_Set_Local_Storage(nameCookie_annouce, _data)
}
function Desk_Remove_All_Announcement () {
    $('.announcement_item_' + Desk_Type).remove();
    Desk_Annouce_Delete_Annouce_Cookie_All_Type(Desk_Type);
}
function Desk_Remove_announcement (appid) {
    if ($('#' + appid).length > 0) {
        $('#' + appid).remove();
    }
}

//#region //Announcement Setting Volume
function Desk_Announcement_Volume (userid, namecookie) {
    let item = {};
    item.ID = userid;
    item.key = 1;
    $('#Desk_volume').click(function () {

        if (Cookies.get(namecookie) != undefined) {
            let data_volume = JSON.parse(Cookies.get(namecookie));
            if (data_volume[userid] != undefined) {
                if (data_volume[userid].key == 1) {
                    data_volume[userid].key = 0;
                    Desk_Announcement_Volume_Change_Icon(0);
                }
                else {
                    data_volume[userid].key = 1
                    Desk_Announcement_Volume_Change_Icon(1);
                }
                Cookies.set(namecookie, data_volume);
            }
            else {
                data_volume[userid] = item;
                Cookies.set(namecookie, data_volume);
                Desk_Announcement_Volume_Change_Icon(1);
            }
        }
        else {
            let data = {};
            data[userid] = item;
            Cookies.set(namecookie, data);
            Desk_Announcement_Volume_Change_Icon(1);
        }
    });
    Desk_Announcement_Volume_Detect(userid, namecookie);
}
function Desk_Announcement_Volume_Detect (userid, namecookie) {
    if (Cookies.get(namecookie) != undefined) {
        let data_volume = JSON.parse(Cookies.get(namecookie));
        if (data_volume[userid] != undefined) {
            if (data_volume[userid].key == 1) {
                Desk_Announcement_Volume_Change_Icon(1);
            }
        }
    }
}
function Desk_Announcement_Volume_Change_Icon (val) {
    $("#Desk_volume").children().removeAttr("class");
    if (val == 1) {
        $("#Desk_volume").children().addClass("volume icon off");
    }
    else {
        $("#Desk_volume").children().addClass("volume icon up");
    }
}

function Desk_Announcement_Volume_Audio (userid) {
    if (Cookies.get(Desk_Manage_Volume) != undefined) {
        let data_volume = JSON.parse(Cookies.get(Desk_Manage_Volume));
        if (data_volume[userid] != undefined) {
            if (data_volume[userid].key == 0) {
                Desk_Announcement_Play_Audio();
            }
        }
    }
    else {
        Desk_Announcement_Play_Audio();
    }
}

function Desk_Announcement_Play_Audio () {
    let audio = new Audio('/assests/dist/Sound/sound_announcement.mp3');
    audio.play();
}
//#endregion

function Desk_Announcement_Type (type) {
    let result = "";
    let Color_New = Desk_ColorCode_Type_Announcement.New;
    let Color_Edit = Desk_ColorCode_Type_Announcement.Edit;
    let Color_Delete = Desk_ColorCode_Type_Announcement.Delete;
    switch (type) {
        case "new":
            result = Color_New;
            break;
        case "edit":
            result = Color_Edit;
            break;
        case "movein":
            result = Color_New;
            break;
        case "moveout":
            result = Color_Delete;
            break;
        case "cancel":
            result = Color_Delete;
            break;
        case "delete":
            result = Color_Delete;
            break;
        default:
            break;
    }
    return result;
}
function Desk_Name_Announcement_By_Type_To_String (type, type_anno, oldroom, newroom) {
    let result = "";
    if (type_anno == "room") {
        result = oldroom + '->' + newroom
    }
    else {
        let _Edit = Desk_Name_Type_Announcement.edit;
        let _MovieIn = Desk_Name_Type_Announcement.movein;
        let _MovieOut = Desk_Name_Type_Announcement.moveout;
        switch (type) {
            case "edit":
                result = _Edit;
                break;
            case "movein":
                result = _MovieIn;
                break;
            case "moveout":
                result = _MovieOut;
                break;
            default:
                result = type;
                break;
        }
    }
    return result;
}



