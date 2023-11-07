//#region // LoadData
async function PInfo_LoadInfo () {
    return new Promise((resolve) => {
        AjaxLoad(url = "/Facebook/Chating/Profileinfo/?handler=LoadInfo"
            , data = {
                "PID": fb_pageid
                , "PSID": fb_userid
            }
            , async = true
            , error = null
            , success = function (result) {
                if (result != "0") {
                    PI_Flag = 1;
                    let data = JSON.parse(result);
                    if (data && data.length > 0) {
                        let custid = data[0].CusID;
                        if (custid != 0) {
                            $("#PI_Created").html(!data[0].Created.includes("1900") ? ConvertDateTimeToString_D_M_Y(data[0].Created) : '-');
                            $("#PI_CreatedBy").html(data[0].CreateBy != '' ? data[0].CreateBy : '-');
                            $("#PI_CusCode").html(data[0].CusCode != '' ? data[0].CusCode : '-');
                            $("#PI_CusCode").attr("target", "_blank");
                            $("#PI_CusCode").attr("href", '/Customer/MainCustomer?CustomerID=' + data[0].CusID + '&ver=' + versionofWebApplication);
                            $('#PI_cusarea').removeClass('d-none');
                            $('#PI_cusnoarea').addClass('d-none');
                        }
                        else PInfo_ClearCusIn();
                        PInfo_ClearEmp();
                        if (data[0].AssignID != 0) {
                            let DTAssign = PI_DTUser.filter(item => {return item.ID == data[0].AssignID});
                            if (DTAssign && DTAssign.length == 1) {
                                $("#PI_UserID").dropdown("set selected", DTAssign[0].ID);
                            }
                            else {
                                $("#PI_UserID").addClass("disabled");
                                $("#PI_UserID").dropdown("set text", data[0].AssignName);
                            }
                        }
                    }
                    else {
                        PInfo_ClearEmp();
                        PInfo_ClearCusIn();
                    }
                    PI_Flag = 0;
                }
            }
            , sender = null,
            before = function (e) {
                $('#PI_cusnoarea').addClass('d-none');
                $('#PI_cusarea').addClass('d-none');
            },
            complete = function (e) {
               
            }
        );
    })
}
 
function PInfo_ClearEmp () {
    $("#PI_UserID").dropdown("clear");
    $("#PI_UserID").removeClass("disabled");
}
function PInfo_ClearCusIn () {
    $("#PI_Created").html('-');
    $("#PI_CreatedBy").html('-');
    $("#PI_CusCode").html('-');
    $("#PI_CusCode").removeAttr("target");
    $("#PI_CusCode").removeAttr("href");
    $('#PI_cusarea').addClass('d-none');
    $('#PI_cusnoarea').removeClass('d-none');
}
//#endregion
//#region // Assign emp
function PInfo_ExecutedAssign (UserID) {
    AjaxLoad(url = "/Facebook/Chating/Profileinfo/?handler=ExecutedAssign"
        , data = {
            "PID": fb_pageid
            , "PSID": fb_userid
            , "AssignID": UserID
        }
        , async = true
        , error = null
        , success = function (result) {
            if (result == "0") {
                notiError_SW();
            }
        }
    );
}
//#endregion
//#region // Tag
async function PInfo_RenderTag (data, id) {
    new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            for (var [key, value] of Object.entries(data)) {
                let item = value;
                let tr = `
                            <div class="row my-1">
                                <div class="form-check w-auto  d-flex align-items-center">
                                    <input id="pntag_${item.ID}" class="form-check-input ftag pr-2 btnTag" data-id="${item.ID}" type="checkbox">
                                    <div class="text-sm px-3 mt-1 py-1 ms-2 rounded-2" style="background: ${item.Color}2b; color: ${item.Color};">${item.Name}</div>
                                </div>
                            </div>
                        `;
                myNode.insertAdjacentHTML('beforeend', tr);
            }
        }
        PInfo_EventTag();
        resolve();
    })
}
async function PInfo_FillTag (ftags) {
    return new Promise((resolve) => {
        PI_Flag = 1;
        let _ob = ftags.split('t');
        $(".btnTag").prop('checked', false);
        for (let ii = 0; ii < _ob.length; ii++) {
            if (_ob[ii] != "") {
                $(".btnTag[data-id='" + _ob[ii] + "']").prop('checked', true);
            }
        }
        PI_Flag = 0;
        resolve();
    })
}
function PInfo_EventTag () {
    $("#fbp_tag .btnTag").unbind("click").on("click", function () {
        if (PI_Flag == 0) {
            let y = document.getElementsByClassName("btnTag");
            fb_currentag = '';
            if (y && y.length > 0) {
                for (let i = 0; i < y.length; i++) {
                    let item = y[i];
                    let ID = $(item).attr("data-id");
                    if ($(item).prop("checked")) {
                        fb_currentag = fb_currentag + ID + 't';
                    }
                }
            }
            if (fb_currentag.length >= 0) {
                if (fb_currentag.charAt(0) != "t") fb_currentag = 't' + fb_currentag;
                let _ob = fb_currentag.split('t');
                let firsttag = 0;
                for (let ii = 0; ii < _ob.length; ii++) {
                    if (_ob[ii] != "") {
                        firsttag = Number(_ob[ii]);
                        break;
                    }
                }
                if (fb_currentag == "t") fb_currentag = "";
                PInfo_ExecutedTag(firsttag, fb_currentag, succfun = function (tags) {
                    PInfo_UpdateCon(fbm_conid, tags);
                });
            }
        }
    })
}
async function PInfo_ExecutedTag (_tagid, _tag, succfun) {
    return new Promise((resolve) => {
        AjaxJWT(url = "/api/FacebookBridge/UpdateTag"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
                , 'tagid': _tagid
                , 'tag': _tag
            })
            , async = true
            , success = function (result) {
                succfun(_tag);
            }
        );
        resolve();
    })
}

async function PInfo_UpdateCon (conver, tag) {
    return new Promise((resolve) => {
        $('#con' + conver).attr('data-tag', tag);
        FBC_TagRender(conver, tag);
        resolve();
    })
}
//#endregion