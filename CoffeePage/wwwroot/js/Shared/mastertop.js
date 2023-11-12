//#region // Event
function MS_MainEvent () {
    $("#MS_NotiClear").click(function () {

        MS_NotiUpdateRead(-1, 0);
        $(".ms_notiitem").remove();
        $("#MS_NotiContent hr").remove();

    });
    $("#MS_NotiReadAll").click(function () {
        MS_NotiUpdateRead(1, 0);
        $(".ms_notiitem").removeClass("active");
    });

    $(document).on("mousedown", function (e) {
        let search = $("#MS_SearchMainResult");
        if (!$(search).is(e.target) && $(search).has(e.target).length === 0) {
            $("#MS_SearchMainResult").hide();
        }
        let noti = $("#MS_NotiArea");
        if (!$(noti).is(e.target) && $(noti).has(e.target).length === 0) {
            $("#MS_NotiArea").hide();
        }
        let popup = $("#MS_PopupArea");
        if (!$(popup).is(e.target) && $(popup).has(e.target).length === 0) {
            $("#MS_PopupArea").hide();
        }
    });

    $("#Master_Top .MS_DashboardDetail").click(function () {
        window.location.href = "/Dash/Dash_Master?ver=" + versionofWebApplication;
    });
    $(".title-popup").popup({
        transition: "scale up",
        position: "right center"
    });
}
//#endregion

//#region // Popup
function MS_PopupUserShow () {
    let position = MS_GET_Position_Popup($('#MS_PopupArea').parent(), $('#MS_PopupArea'));
    $("#MS_PopupArea").css({
        "display": "block"
        , "top": position.top + 10
        , "left": position.left-30
        , "width": ((position.width != undefined) ? (position.width) : "")
    }).animate({
        top: position.top
    }, 300);
    event.stopPropagation();
}
function MS_PopupFillData () {
    $('#MS_PopupAvatar').attr({'src': Master_circleAvatar, 'title': sys_userName_Main});
    $('#MS_PopupName').html(sys_userName_Main);
    $('#MS_PopupGroupName').html(Master_roleGroupCurrent);
    $('#MS_PopupEmpName').html(sys_employeeName_Main);

}
//#endregion

//#region // Searching
$('#MS_SearchInput').keyup(function () {

    if ($(this).val().length > 0) $("#MS_SearchContent .btnsear_clear").removeClass('opacity-1').addClass('d-none');
    else $("#MS_SearchContent .btnsear_clear").addClass('opacity-1').addClass('d-none');
    
    $("#MS_SearchContent .spinner-border").show();

    clearTimeout(Timer_Search_Customer);
    Timer_Search_Customer = setTimeout(function (e) {
        let valueSearch = $('#MS_SearchInput').val().toLowerCase().trim();
        let valuenosign = xoa_dau(valueSearch).replace(/[^a-zA-Z0-9 ]/g, '');
        let issign = valueSearch == valuenosign ? 0 : 1;
        MS_SearchExecute(valueSearch, valuenosign, issign);
    }, 500);
    setTimeout(function (e) {
        $("#MS_SearchContent .spinner-border").hide();
    }, 1000);
});

$('#MS_SearchInput').focus(function () {
    clearTimeout(Timer_Search_Customer);
    Timer_Search_Customer = setTimeout(function (e) {
         
        let valueSearch = $('#MS_SearchInput').val().toLowerCase().trim();
        let valuenosign = xoa_dau(valueSearch).replace(/[^a-zA-Z0-9 ]/g, '');
        let issign = 0;
        if(CheckIsSeachLastName(valueSearch)){
            issign = 1;
            valuenosign = getLastName(valueSearch);
        }
        //let issign = (valueSearch == valuenosign && !MS_IsSeachLastName(valuenosign)) ? 0 : 1;
        MS_SearchExecute(valueSearch, valuenosign, issign);
    }, 500);
});

$("#MS_SearchContent").on('click', '.btnsear_clear', function (e) {
    $('#MS_SearchInput').val('');
    $("#MS_SearchContent .btnsear_clear").addClass('opacity-1');
    $("#MS_SearchResult").empty();
    Count_Up("MS_CountResult", 0);
    $("#MS_SearchMainResult").hide();
 
    $("#MS_SearchContent .spinner-border").hide();
});

function MS_SearchText(text) {
    try {
        return xoa_dau(text).replace(/[^a-zA-Z0-9 ]/g, '').toLowerCase();
    }
    catch (ex) {
        return '';
    }
}

function MS_SearchExecute (_textsign, _search_text, _isSign) {

    if (seaching_ajax_request && seaching_ajax_request.readyState != 4) seaching_ajax_request.abort();
    if (_search_text.length >= 3) {
        seaching_ajax_request = AjaxLoad(url = "/Master/Master_Top/?handler=SearchQuick"
            , data = {
                'textsign': _textsign,
                'textsearch': _search_text,
                'isInt': (!isNaN(_search_text) && _search_text.length <= 10) ? 1 : 0,
                'IsSign': _isSign
            }
            , async = true
            , error = function () {notiError_SW();}
            , success = function (result) {
                if (result != "0") {
                     
                    let _isresult = 1;
                    let data = JSON.parse(result);
                    let _datapri = [];
                    let _datanopri = [];
                    let _datafilter = [];
                    _search_text = _search_text.toLowerCase();
                    if (data != undefined && data.length != 0) {
                        _datapri = data.filter(word =>
                            MS_SearchText(word["Name_NoSign"]).includes(_search_text)
                            || MS_SearchText(word["CustCodeOld"]).includes(_search_text)
                            || MS_SearchText(word["CustCode"]).includes(_search_text)
                            || MS_SearchText(word["DocumentCode"]).includes(_search_text)
                            || word["CustomerPhone"].includes(_search_text)
                            || word["CustomerPhone2"].includes(_search_text)
                            || word["MemberCode"].includes(_search_text)
                            || word["TicketID"] != 0
                        );
                         
                        _datanopri = data.filter(word =>
                               !MS_SearchText(word["Name_NoSign"]).includes(_search_text)
                            && !MS_SearchText(word["CustCodeOld"]).includes(_search_text)
                            && !MS_SearchText(word["CustCode"]).includes(_search_text)
                            && !MS_SearchText(word["DocumentCode"]).includes(_search_text)
                            && !word["CustomerPhone"].includes(_search_text)
                            && !word["CustomerPhone2"].includes(_search_text)
                            && !word["MemberCode"].includes(_search_text)
                            && !word["TicketID"] != 0
                        );
                        _datafilter = _datapri.concat(_datanopri);
                        if (_datafilter != undefined && _datafilter.length != 0) {
                            _isresult = 1;
                        }
                        else _isresult = 0;
                    }
                    else _isresult = 0;

                    if (_isresult == 1) {
                        if (_isSign == "1") {

                            _datafilter = _datafilter.sort((a, b) => {
                                if (a["CustomerName"].toLowerCase().includes(_textsign)) return -1;
                            });
                        }
                        MS_SearchRender(_datafilter, "MS_SearchResult");
                        Count_Up("MS_CountResult", _datafilter.length);
                        sys_Hightlight(_search_text, "search_text_name");
                        sys_Hightlight(_search_text, "search_text_phone");
                        sys_Hightlight(_search_text, "search_text_code");
                        sys_Hightlight(xoa_dau(_textsign), "search_text_name");
                        sys_Hightlight(xoa_dau(_textsign), "search_text_phone");
                        sys_Hightlight(xoa_dau(_textsign), "search_text_code");
 
                        $("#MS_SearchMainResult").show();
                        let pos_top = $('#MS_SearchMainResult').parent().position().top;
                        $("#MS_SearchMainResult").css({
                            "display": "block"
                            , "top": pos_top + 20
                        }).animate({
                            top: pos_top + 40
                        }, 300);

                        Checking_TabControl_Permission();
                    }
                    else {
                        $("#MS_SearchResult").empty();
                        Count_Up("MS_CountResult", 0);
                        $("#MS_SearchMainResult").hide();
                        $("#MS_SearchContent .btnsear_clear").removeClass('d-none');
                        $("#MS_SearchMainResult").show();
                        let pos_top = $('#MS_SearchMainResult').parent().position().top;
                        $("#MS_SearchMainResult").css({
                            "display": "block"
                            , "top": pos_top + 20
                        }).animate({
                            top: pos_top + 40
                        }, 300);
                    }
                }
            }
            , sender = null, before = null
            , complete = function (e) {
                $("#MS_SearchContent .spinner-border").hide();
                $("#MS_SearchContent .btnsear_clear").removeClass('d-none')
            }
        );
    }
    else {
        $("#MS_SearchContent .spinner-border").hide();
        $("#MS_SearchResult").empty();
        Count_Up("MS_CountResult", 0);
        $("#MS_SearchMainResult").hide();
        $("#MS_SearchContent .btnsear_clear").removeClass('d-none')
    }
}

function MS_SearchRender (data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let gender = (item.Gender == 61)
                    ? `<i class="me-1 text-sm fas fa-venus" style="color: rgb(255, 107, 133);"></i>`
                    : `<i class="me-1 text-sm fas fa-mars" style="color: rgb(47, 137, 255);"></i>`;
                let redirect = item.CustomerID != 0
                    ? '/Customer/MainCustomer?CustomerID=' + item.CustomerID
                    : '/Marketing/TicketAction?CustomerID=0&TicketID=' + item.TicketID
                let tr = `
                    <li class="border-0 p-0 list-group-item rounded-0 border-bottom-sm">
                        <a class="search_result d-block p-3" target="_blank" href="${redirect}&ver=${versionofWebApplication}">
                            <div class="d-flex flex-column justify-content-center ps-2">
                                <div class="text-md mb-0">
                                     ${gender}
                                    <h1 class="search_text_name fw-bold text-sm text-dark pe-2  mb-0 d-inline">
                                        ${item.CustomerName}
                                    </h1>
                                    <p class="search_text_code mb-0 text-primary text-sm d-inline">
                                        <span>${((item.CustCode != '') ? item.CustCode : MS_SearchRender_GeneralCode(item.TicketID))}</span>
                                        ${item.DocumentCode != '' ? `<span class="px-2">${item.DocumentCode}</span>` : ``}
                                        ${item.MemberCode != '' ? `<span class="px-2">${item.MemberCode}</span>` : ``}
                              
                                        <span class="toogleitem" data-name="MS_CodeCusOld">${((item.CustCodeOld != '' && item.CustCodeOld != '0') ? '[' + item.CustCodeOld + ']' : '')}</span>
                                    </p>
                                </div>
                                <div class="text-sm text-dark mb-0">
                                    <i class="fas fa-mobile-alt text-xs"></i>
                                    ${((item.CustomerPhone != '')
                                            ? (`<p class="search_text_phone _tab_control_ ps-2 text-sm mb-0 d-inline" data-staff="${item.StaffID}" data-tab="phone_customer" >${item.CustomerPhone}</p>`)
                                            : ``) }
                                    ${((item.CustomerPhone2 != '')
                                            ? (`<p class="search_text_phone _tab_control_ ps-2 text-sm mb-0 d-inline" data-staff="${item.StaffID}" data-tab="phone_customer" >${item.CustomerPhone2}</p>`)
                                            : ``)}
                                </div>
                                <div class="text-sm text-dark mb-0 toogleitem" data-name="MS_Birthday">
                                    <i class="fas fa-birthday-cake text-xs me-n1"></i>
                                    ${(!item.Birthday.includes("1900"))
                                            ? (`<p class="search_text_phone _tab_control_ ps-2 d-inline text-sm mb-0">${ConvertDateTimeToString_D_M_Y(item.Birthday)}</p>`)
                                            : ``}
                                </div>
                            </div>
                        </a>
                    </li>
                `
                myNode.insertAdjacentHTML("beforeend", tr);
            }
            MS_ShTable.Refresh();
        }
    }
}

function MS_SearchRender_GeneralCode(id) {
    try {
        let result = '';
        let stringTemm = '00000000';
        stringTemm = stringTemm + id.toString();
        let lengthTemp = stringTemm.length;
        result = stringTemm.slice(lengthTemp - 8, lengthTemp)
        return result;
    }
    catch (ex) {
        return '';
    }
}
//#endregion

//#region // Noti
function MS_NotiLoad () {
    AjaxLoad(url = "/Master/Master_Top/?handler=NotiItemCount"
        , data = ({})
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                let _num = data[0].Number_UnRead;
                if (Number(_num) != 0) {
                    $("#MS_NotiTotal").removeClass('d-none');
                    $("#MS_NotiTotal").html(data[0].Number_UnRead);
                }
                else {
                    $("#MS_NotiTotal").addClass('d-none')
                }
            }
            else {
                $("#MS_NotiTotal").addClass('d-none')
            }
        }
        , sender = null

    );
}
function MS_NotiUpdateRead (isread_all, id, todo) {
    AjaxLoad(url = "/Master/Master_Top/?handler=NotiExecute"
        , data = {
            'IsReadAll': isread_all
            , 'NotiID': id
            , 'todo': (todo != undefined) ? todo : 0
        }
        , async = true
        , error = function () {notiError_SW();}
        , success = function (result) {
            if (result == "1") {
                MS_NotiLoad();
            }
        }
        , sender = null
    );
}

function MS_NotiEvent () {
    $(".ms_notiitem").click(function (event) {
        let _id_noti = $(this).attr("data-id");
        let _is_todo = $(this).attr("data-todo");
        $(this).removeClass("active");
        MS_NotiUpdateRead(0, _id_noti, _is_todo);
    });
}
function MS_NotiShow () {
    let position = MS_GET_Position_Popup($('#MS_NotiArea').parent(), $('#MS_NotiArea'));
    $("#MS_NotiArea").css({
        "display": "block"
        , "top": position.top + 10
        , "left": position.left
        , "width": ((position.width != undefined) ? (position.width) : "")
    }).animate({
        top: position.top
    }, 300);
    AjaxLoad(url = "/Master/Master_Top/?handler=NotiItemLoad"
        , data = {}
        , async = true
        , error = function () {notiError_SW();}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                MS_NotiRender(data, "MS_NotiContent");
                $("#MS_NotiEmpty").hide();
                $("#MS_NotiClear").show();
                $("#MS_NotiReadAll").show();
            }
            else {
                $("#MS_NotiArea_action").hide();
                $("#MS_NotiEmpty").show();
                $("#MS_NotiClear").hide();
                $("#MS_NotiReadAll").hide();
            }
        }
        , sender = null
        , before = function () {
            $('#MS_NotiContent').empty();
            $('#MS_NotiContent').hide();
            $('#MS_NotiWaiting').show();
        }
        , complete = function (e) {
            $('#MS_NotiContent').show();
            $('#MS_NotiWaiting').hide();
        }
    );
    event.stopPropagation();
}
function MS_NotiRender_GetLInk (_todo, _type, _link, _custid, _ticketid) {
    let result = '';
    if (_todo == 0) {
        switch (_type) {
            case 1:
            case 2:
            case 4:
            case 5:
                {
                    result = _link + '?ver=' + versionofWebApplication;
                    result = '<a href="' + result + '" target="_blank" class="detail ps-2 fw-bold text-dark opacity-2">' + Outlang["Chi_tiet"] + '</a>';
                }
                break;
            case 3:
                {
                    result = _link + _custid + '&ver=' + versionofWebApplication;
                    result = '<a href="' + result + '" target="_blank" class="detail ps-2 fw-bold text-dark opacity-2">' + Outlang["Chi_tiet"] + '</a>';
                }
                break;
            case 6:
                {
                    result = _link + _ticketid + '&CustomerID=' + _custid+ '&ver=' + versionofWebApplication;
                    result = '<a href="' + result + '" target="_blank" class="detail ps-2 fw-bold text-dark opacity-2">' + Outlang["Chi_tiet"] + '</a>';
                }
                break;
            case 7:
                {
                    result = _link + '?ver=' + versionofWebApplication;
                    result = '<a href="' + result + '" target="_blank" class="detail ps-2 fw-bold text-dark opacity-2">' + Outlang["Chi_tiet"] + '</a>';
                }
                break;
            case 8:
                {
                    result = _link + '?ver=' + versionofWebApplication;
                    result = '<a href="' + result + '" target="_blank" class="detail ps-2 fw-bold text-dark opacity-2">' + Outlang["Chi_tiet"] + '</a>';
                }
                break;
            default: break;
        }
    }
    else {
        result = '';
    }
    return result;
}
function MS_NotiRender_GetAction (_todo, _type) {
    let result = '';
    if (_todo == 0) {
        switch (_type) {
            case 1:
            case 2:
                {
                    result = '<span class="badge badge-sm badge-circle badge-floating bg-gradient-info position-absolute border-white" style="bottom: -3px;right: -4px;border: 2px solid white;">'
                        + '<i class="vtt-icon vttech-icon-list-ticket-file text-white" style="font-size: 10px;"></i>'
                        + '</span>';
                }
                break;
            case 3:
                {
                    result = '<span class="badge badge-sm badge-circle badge-floating bg-gradient-success position-absolute border-white" style="bottom: -3px;right: -4px;border: 2px solid white;">'
                        + '<i class="fas fa-chevron-right text-white" style="font-size: 10px;"></i>'
                        + '</span>';


                }
                break;
            default: break;
        }
    }
    else {
        result = '<span class="badge badge-sm badge-circle badge-floating bg-gradient-success position-absolute border-white" style="bottom: -3px;right: -4px;border: 2px solid white;">'
            + '<i class="vtt-icon vttech-icon-task  text-white" style="font-size: 10px;"></i>'
            + '</span>';

    }
    return result;
}
function MS_NotiRender (data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {

            for (let i = 0; i < data.length; i++) {
                let item = data[i];
                let _classname = (Number(item.IsRead) == 1) ? '' : 'active';
                let sendername = '', senderimage = '', _type = '';
                let isUnRead = (Number(item.IsRead) == 1) ? ''
                    : '<span class="unread badge badge-md badge-dot me-2 ms-auto opacity-0"><i class="bg-gradient-primary"></i></span>';


                _type = MS_NotiRender_GetAction(item.IsTodo, item.Type);
                let obj = Fun_GetObject_ByID(MTDataEmployee, item.UserSend);
                if (obj != null) {
                    senderimage = obj.Avatar;
                    sendername = obj.Name;
                }
                else {
                    senderimage = Master_Default_Pic;
                    sendername = '';
                }
                let _timeago = GetTimeAgo_FromCurrent(item.DateNoti);
                let _link = MS_NotiRender_GetLInk(item.IsTodo, item.Type, item.Link, item.CustomerID, item.TicketID);
                stringContent = stringContent
                    + '<li class="ms_notiitem list-group-item border-0 d-flex align-items-center mb-2 p-2 rounded-3 ' + _classname + '" data-id="' + item.ID + '" '
                    + '" data-todo="' + item.IsTodo + '">'
                    + '<div class="w-30 avatar avatar-sm me-3  position-relative">'
                    + '<img src="' + senderimage + '"  class="border-radius-lg shadow">'
                    + _type
                    + '</div>'
                    + '<div class="w-70 d-flex align-items-start flex-column justify-content-center">'
                    + '<span class="text-dark mb-0 text-sm fw-bold ellipsis_one_line">' + item.ContentNoti + '</span>'
                    + '<p class="text-dark mb-0 text-xs">' + _timeago + _link + '</p>'
                    + '</div>'
                    + isUnRead
                    + '</li>'
                    + '<hr class="horizontal dark mt-0 mb-0">';
            }
        }
        document.getElementById(id).innerHTML = stringContent;
        MS_NotiEvent();
    }
}
// #endregion

//#region // Other
function MS_SearchEnterExecute () {
    let _page = location.pathname.split('/').slice(-1)[0];
    let search = xoa_dau($("#MS_SearchInput").val().toLowerCase()).trim();
    if (_page == "Searching") {
        window.open("/Searching/Searching?SeachingText=" + search, "_self");
    }
    else {
        window.open("/Searching/Searching?SeachingText=" + search);
    }
}



function MS_GET_Position_Popup (parent, child, isScroll = false) {
    let position = {
        top: 0,
        left: 0
    };
    let windowWidth = $(window).width();
    let parentHeight = parent.outerHeight();
    let parentWidth = parent.outerWidth();
    let childHeight = child.outerHeight();
    let childWidth = child.outerWidth();

    let parentOffset = parent.offset();
    let parentTop = parentOffset.top;
    let parentLeft = parentOffset.left;


    if (windowWidth - parentLeft > childWidth) {
        position.left = 0;
    }
    else if (parentLeft - childWidth + parentWidth > 0) {
        position.left = -childWidth + parentWidth;
    }
    else {
        position.left = -parentLeft + 10;
        position.width = windowWidth - 20;
    }
    position.top = parentHeight;
    return position;
}
//#endregion

//#region // Sidebar

if (document.querySelector('.fixed-plugin')) {
    let fixedPlugin = document.querySelector('.fixed-plugin');
    let fixedPluginButton = document.querySelector('.fixed-plugin-button');
    //let fixedPlugin = document.querySelector('.fixed-plugin');
    let fixedPluginButtonNav = document.querySelectorAll('.fixed-plugin-button-nav');
    let fixedPluginCloseButton = document.querySelectorAll('.fixed-plugin-close-button');
    let fixedPluginCard = document.querySelector('.fixed-plugin .card');

    let navbar = document.getElementById('navbarBlur');
    let buttonNavbarFixed = document.getElementById('navbarFixed');

    if (fixedPluginButton) {
        fixedPluginButton.onclick = function () {
            if (!fixedPlugin.classList.contains('show')) {
                fixedPlugin.classList.add('show');
            } else {
                fixedPlugin.classList.remove('show');
            }
        }
    }

    if (fixedPluginButtonNav) {
        fixedPluginButtonNav.forEach(element => {
            element.onclick = function () {
                if (!fixedPlugin.classList.contains('show')) {
                    fixedPlugin.classList.add('show');
                } else {
                    fixedPlugin.classList.remove('show');
                }
            }
        })
    }
    fixedPluginCloseButton.forEach(function (el) {
        el.onclick = function () {
            fixedPlugin.classList.remove('show');
        }
    })

    document.querySelector('body').onclick = function (e) {
        if (e.target != fixedPluginButton && e.target != fixedPluginButtonNav[0] && e.target != fixedPluginButtonNav[1] && e.target.closest('.fixed-plugin .card') != fixedPluginCard) {
            fixedPlugin.classList.remove('show');
        }
    }

    if (navbar) {
        if (navbar.getAttribute('data-scroll') == 'true' && buttonNavbarFixed) {
            //buttonNavbarFixed.setAttribute("checked", "false");
        }
    }

}

// click to minimize the sidebar or reverse to normal
if (document.querySelector('.sidenav-toggler')) {

    var sidenavToggler = document.querySelectorAll('.sidenav-toggler');
    var sidenavShow = document.getElementsByClassName('g-sidenav-show')[0];
    var toggleNavbarMinimize = document.getElementById('navbarMinimize');

    if (sidenavShow) {
        sidenavToggler.forEach(element => {
            element.onclick = function () {

                if (!sidenavShow.classList.contains('g-sidenav-hidden')) {
                    sidenavShow.classList.remove('g-sidenav-pinned');
                    sidenavShow.classList.add('g-sidenav-hidden');
                    if (toggleNavbarMinimize) {
                        toggleNavbarMinimize.click();
                        toggleNavbarMinimize.setAttribute("checked", "true");
                    }
                } else {
                    sidenavShow.classList.remove('g-sidenav-hidden');
                    sidenavShow.classList.add('g-sidenav-pinned');
                    if (toggleNavbarMinimize) {
                        toggleNavbarMinimize.click();
                        toggleNavbarMinimize.removeAttribute("checked");
                    }
                }
            };
        })
    }

    if (window.innerWidth < 1200) {
        var mainbody = document.getElementById('BodyMain');
        if (mainbody)
            mainbody.classList.add('g-sidenav-hidden');
    }
}

//#endregion



