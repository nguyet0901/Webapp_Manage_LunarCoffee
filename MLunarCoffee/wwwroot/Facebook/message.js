

function FBM_Load(mconid, ismore = 0) {
    try {
        if (fcd_XhrLoadMess && fcd_XhrLoadMess.readyState != 4) fcd_XhrLoadMess.abort();
        let urlMess = (ismore == 0 ? (fl_messlist.replace('{conid}', mconid)) : fcd_UrlMessNext);
        if (ismore == 0) {
            $("#fbm_content").html('');
        }
        if (urlMess == '') return;
        fcd_XhrLoadMess = $.ajax({
            url: urlMess,
            dataType: "json",
            type: "GET",
            data: JSON.stringify({}),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.responseJSON.error.code == 100) {
                    
                    $('#fbc_profileinfo').addClass('nonexist');
                    $('#fbc_chatting').addClass('nonexist');
                }
                FBM_LoadComplete(ismore);
            },
            success: function (result) {
 
                let _datamess = result?.messages || result;
                if (_datamess && _datamess?.data && _datamess?.data != '') {
                    FBM_Render(_datamess?.data, "fbm_content", ismore);
                }
                FBM_GetUrlMess(_datamess);
            },
            beforeSend: function () {
                $('#fbc_chatting').removeClass('nonexist');
                $('#fbc_profileinfo').removeClass('nonexist');
                FBM_LoadBefore(ismore);
            },
            complete: function () {

            }
        })
    }
    catch (ex) {
        FBM_LoadComplete(ismore);
    } 
}

function FBM_GetUrlMess(data) {
    try {
        let dataPaging = data?.paging;
        if (dataPaging) {
            fcd_UrlMessNext = dataPaging?.next || "";
        }
        else {
            fcd_UrlMessNext = "";
        }
    }
    catch (ex) {
        fcd_UrlMessNext = "";
    }
}

function FBM_LoadBefore(ismore) {
    $('#fbm_wait').removeClass('d-none');
    if (ismore == 1) return;
    $('#fbm_content').addClass('opacity-0');
}

function FBM_LoadComplete(ismore = 0) {
    if (ismore == 0) FBM_Scroll('bottom');
 
    $('#fbm_wait').addClass('d-none');
    if (ismore == 1) return;
    setTimeout(() => {
        $('#fbm_content').removeClass('opacity-0');
    }, 100)
}
async function FBM_Render (data, id, ismore = 0) {
    return new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            let _in = 0, _indow = '', _dow = '';
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    _in++;
                    _dow = ConvertDateTimeToString_DOW(data[i].created_time);
                    if (_in > 10 || i == 0 || _indow != _dow) {
                        myNode.insertAdjacentHTML("afterbegin",
                            ` <div class="d-flex my-2 text-dark align-items-center text-sm justify-content-center">
                                <div class="date text-xs">${ConvertDateTimeToString_DOW(data[i].created_time)} , ${ConvertDateTimeUTC_Time(data[i].created_time)}</div>
                            </div>`);
                        _in = 0;
                        _indow = _dow;
                    }
                    let item = data[i];
                    let tr = FBM_RenderEach(item, temp = 0);
                    myNode.insertAdjacentHTML("afterbegin", tr);
                }
            }
            FBM_MarkReponse();
            FBM_LoadComplete(ismore);
            FBM_Event();
        }
    })
}
function FBM_RenderEach (item,temp) {
    try {
        let _istemp = '', _issent = '', _iserror = '', _isphone = '';
        let _ispagesend = 'usersend';
        if (temp == 0) {
            let _from = item.from;
            if (_from.id == fb_pageid) _ispagesend = 'pagesend';
            else {
                if (item.message != undefined) {
                    let _phonnum = item.message.replaceAll(' ', '').trim();
                    if (!isNaN(_phonnum) && (/\d{10}/.test(_phonnum))) {
                        _isphone = `<i   data-phone="${_phonnum}" class="addprofile cursor-pointer fas fa-copy text-sm text-secondary opacity-1 text-primary position-absolute top-0 end-0"></i>`;
                    }
                        
                }
            }
        }
        else {
            _istemp = 'temp';
            _ispagesend = 'pagesend';
          
        }
        if ((new Date((new Date()).getTime() - 10 * 60000)) < new Date(item.created_time) && (_ispagesend == 'pagesend')) {
            if (temp == 0) _issent = `<i class="senticon far fa-check-circle"></i>`;
            else {
                _issent = `<div class="spinner-border senticon wait text-primary" role="status">
                                <span class="sr-only">Loading...</span>
                            </div>`;
                _iserror = `<i class="fas fa-exclamation fail senticon text-danger opacity-0"></i>`

            } 
            
        }

        let content = FBM_RenderEachAttach(item, temp);
        let result =
            `<div id="${item.id}" class="row blocksend ${_ispagesend} ${_istemp}" >
                    <div class="col-auto">
                        <div class="card card-plain">
                            <div class="card-body py-2 px-0 position-relative">
                                <div data-bs-toggle="tooltip" data-time="${ConvertDateTimeToTimeSpan(item.created_time)}" title="${ConvertDateTimeUTC_Time(item.created_time)}" class="con_itemsend text-dark mb-1">
                                    ${content}
                                    ${_isphone}
                                    ${_issent}
                                    ${_iserror}
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>`
        return result;
    }
    catch (ex) {
        return '';
    }
}

function FBM_RenderEachAttach (_mes, _temp) {
    if (_temp == 0) {
        if (_mes.message != undefined && _mes.message != "") {
            return `<div class="content">${_mes.message}</div>`;
        }
        else {
            if (_mes.sticker != undefined && _mes.sticker != "") {
                return `<img onerror="Master_OnLoad_Error_Image(this)" src="${_mes.sticker}" class="sticker"/>`;
            }

            if (_mes.attachments != undefined && _mes.attachments.data.length > 0) {
                let _att = _mes.attachments.data;
                let _imgobj = [];
                let _img = '', _audio = '', _video = '', _file = '';

                for (let i = 0; i < _att.length; i++) {
                    let _name = _att[i].name != undefined ? _att[i].name : '';

                    if (_att[i].mime_type.includes('image')) _imgobj.push(_att[i]);
                    else if (_att[i].mime_type.includes('audio')) {
                        _audio = _audio
                            + `<div class="audio">
                                    <audio controls
                                        <source src="${_att[i].file_url}" type="${_att[i].mime_type}">
                                    </audio>
                                </div>` ;
                    }
                    else if (_att[i].mime_type.includes('video')) {

                        _video = _video
                            + `<div class="video">
                                    <div class="position-relative">
                                        <img id="matfeat${_att[i].id}" onerror="Master_OnLoad_Error_Image(this)" class="videofeature cursor-pointer" src="${_att[i].video_data.preview_url}"/>
                                        <i data-id="${_att[i].id}" class="play cursor-pointer position-absolute top-50 start-50 translate-middle fs-1 p-5 text-white far fa-play-circle"></i>
                                        <video id="matvideo${_att[i].id}" controls>
                                              <source src="${_att[i].video_data.url}" type="${_att[i].mime_type}">
                                        </video>
                                    </div>
                                </div>` ;
                    }
                    else {
                        _file = _file
                            + `<div class="file">
                                    <div class="d-flex align-items-center">
                                    <i class="fas fa-arrow-circle-down pe-2 "></i>
                                        <a class="cursor-pointer text-sm" href="${_att[i].file_url}">${_name}</a>
                                   </div>
                                </div>` ;
                    }
                }
                if (_imgobj.length > 0) {

                    for (let i = 0; i < _imgobj.length; i++) {
                        _img = _img
                            + `<div class="col-auto p-2"><img onerror="Master_OnLoad_Error_Image(this)" class="imgsend cursor-pointer" data-src="${_imgobj[i].image_data.url}" src="${_imgobj[i].image_data.url}"/></div>`;

                    }
                    _img = `<div class="image row">` + _img + `</div>`;
                }



                return `<div  class="media">${_img}${_audio}${_video}${_file} </div>`;
            }
        }
    }
    else {
        if (_mes.message != undefined && _mes.message != "") {
            return `<div class="content">${_mes.message}</div>`;
        }
        else {
            return `<img onerror="Master_OnLoad_Error_Image(this)" src="${_mes.linktemp}" class="sticker"/>`;
        }
    }
    return "";
}

function FBM_Event() {
    $('#fbm_content .play').unbind('click').click(function () {
        let _id = $(this).attr('data-id');
        $(this).remove();
        $(`#matfeat${_id}`).remove();
        $(`#matvideo${_id}`).addClass('show');
        $(`#matvideo${_id}`).trigger('play');
    });
    $('#fbm_content .imgsend').unbind('click').click(function () {
        let _src = $(this).attr('data-src');
        window.open(_src);
    });
    $('#fbm_content .addprofile').unbind('click').click(function () {
        let _phone = $(this).attr('data-phone');
        fbm_coppied = _phone;
        $('#fbm_content .addprofile').removeClass('copied');
        $(this).addClass('copied');
    });
    ToolPopper(); 
}
function FBM_MarkReponse() {
    return new Promise((resolve) => {
        if ($("#fbm_content .senticonavatar").length != 0) {
            return;
        }
        AjaxJWT(url = "/api/FacebookBridge/MarkResponse"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
            })
            , async = true
            , success = function (result) {
                let data = JSON.parse(result);
                if (data && data.length != 0) {
                    let time = ConvertDateTimeToTimeSpan(data[0].uread_time);
                    let itemsend = $('.blocksend.pagesend .con_itemsend');
                    if (itemsend) {
                        for (let i = itemsend.length - 1; i >= 0; i--) {
                            let datatime = Number($(itemsend[i]).attr('data-time'));
                            if (datatime <= time) {
                                $(itemsend[i]).append(`<div class="senticonavatar" style="background-image: url(${fcd_MessAvatar})"></div>`)
                                break;
                            }
                        }
                    }
                }
            }
            , before = function (e) {
             
            }

        );
        resolve();
    })
}

function FBM_LoadParticulr (mdetailid, random, failurefunc, successfunc) {
 
    $.ajax({
        url: fl_messdetail.replace('{messid}', mdetailid),
        dataType: "json",
        type: "GET",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            failurefunc(random);
        },
        success: function (result) {
 
            if (result.message != undefined) {
                successfunc(random, result);
            }
            else failurefunc(random);
        }
    })
}

//#region // MESS INPUT

function FBM_GetAttachmentExt (formdata) {
    let datatype = ['image', 'video', 'audio', 'file', 'template'];
    let index = datatype.indexOf(formdata.type.split('/')[0])
    return (index != -1 ? datatype[index] : 'file');
}

function FBM_AttachmentUpload(formdata, dataType, random) {
    return new Promise((resolve) => {
        if (!formdata) {
            resolve({attachment_id: 0, type: ''});
            return;
        }
        let dataAttachment = {
            "attachment": {
                "type": dataType,
                "payload": {
                    "is_reusable": true
                }
            }
        }
        const form = new FormData();
        form.append('message', JSON.stringify(dataAttachment));
        form.append('filedata', formdata);
        $.ajax({
            url: fl_attachupload,
            data: form,
            cache: false,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (result) {
                resolve({
                    ...result,
                    type: dataType
                });
            },
            erorr: function (e) {
                FBM_SendFail(random)
                resolve({
                    attachment_id: 0,
                    type: ''
                });
            }
        });
    })
}

async function FBM_SendMesssage(mess, type, formdata, random, messtype = 'RESPONSE') {
    FBM_SendPrepare(random, mess);
    let dataAttach = (formdata != undefined && formdata != '')
        ? await FBM_AttachmentUpload(formdata, type, random )
        : undefined;
    let dataSend = {
        "recipient": {
            "id": fb_userid
        },
        "message": {},
        "messaging_type": messtype,
        "access_token": fb_pagetoken
    }
    if (mess != '') {
        dataSend.message.text = mess;
    }
    if (dataAttach && dataAttach.attachment_id != 0) {
        dataSend.message.attachment = {
            "type": type,
            "payload": {
                "attachment_id": dataAttach.attachment_id
            }
        };
    };
    if (messtype != 'RESPONSE') {
        if (messtype == 'MESSAGE_TAG')
            dataSend.tag = 'ACCOUNT_UPDATE'
    }
    $.ajax({
        url: fl_sendmess,
        data: dataSend,
        type: 'POST',
        success: function (result) {
            if (result.message_id != undefined) {
                FBM_SendSuccess(result.message_id, random,mess,type);
            }
            else FBM_SendFail(random);
        },
        error: function (e) {
            FBM_SendFail(random)
        },
        beforeSend: function () {
                
        }
    });
    
}

function FBM_Send () {
    try {
 
        let _idrandom = FBM_Randomid();
        let _mess = ($("#fcd_message").val()).trim();
        if (_mess && _mess != "") FBM_SendMesssage(_mess, type = '', formdata = '', _idrandom);
        if (fcd_FormFiles != null) {
            for (let [key, value] of fcd_FormFiles.entries()) {
                let type = FBM_GetAttachmentExt(value);
                FBM_SendMesssage(mess = "", type, value, _idrandom);
            }
        }
        FBM_ResetFieldInput();
    }
    catch (ex) {
        FBM_ResetFieldInput();
    }
}

function FBM_ResetFieldInput () {
    $("#fcd_message").val("");
    $("#fcd_message").height('unset');
    $("#fcd_fileview").html("");
    fcd_FormFiles = new FormData();
}

function FBM_Randomid() {
    return Math.floor(Math.random() * 100001).toString() + '-' + (new Date()).getTime().toString();
}

async function FBM_SendFail (random) {
    return new Promise((resolve) => {
        setTimeout(() => {
            $('#' + random).addClass('error');
            resolve();
        }, 10);
    })
}

async function FBM_SendSuccess (messid,random,mess,type) {
    return new Promise((resolve) => {
        setTimeout(() => {
            FBM_LoadParticulr(messid, random
                , failurefunc = function (r) {
                    FBM_SendFail(r);
                }
                , successfunc = function (r, item) {
                    $('#' + r).replaceWith(FBM_RenderEach(item, temp = 0));
                    FBC_Movetotop(fbm_conid);
                    FBC_ChangeContent(fbm_conid, mess, type, fb_pageid);
                    FBM_Event();
                    FBM_PlayTing();
                }
 
            )
            resolve();
        });
    })
}
async function FBM_PlayTing () {
    await new Promise((resolve, reject) => {
        setTimeout(
            () => {
                try {
                    if (!document.hidden) {
                        let beat = new Audio('/ting.mp3');
                        beat.play();
                    }


                } catch (e) { }
                resolve()
            }
        )
    });

}
 function FBM_SendPrepare (random, mess) {
     
     let item = {};
     item.id = random;
     item.created_time = new Date();
     item.message = mess;
     item.linktemp = '/default.png';

    document.getElementById("fbm_content").insertAdjacentHTML("beforeEnd"
        , FBM_RenderEach(item, temp = 1));
    FBM_Scroll("bottom");
}

//#endregion

//#region // Scroll
 
async function FBM_Scroll (dimen) {
    return new Promise((resolve) => {
        setTimeout(() => {
 
            var objDiv = document.getElementById("fbm_content");
            switch (dimen) {
                case "bottom":
                    {
                        objDiv.scrollIntoView({behavior: 'auto', block: 'end'});
                        break;
                    }
                case "top":
                    { 
                        objDiv.scrollIntoView({behavior: 'auto', block: 'start'});
                        break;
                    }
            }
        
        }, 10);
    })
}
//#endregion

//#region // Fill chat
function FBM_FillChating (_star, _hide){
    $('#fbm_staruser').attr('data-star', _star);
    if (_star == 1) {
        $('#fbm_staruser .fa-star').addClass('text-warning');
    }
    else {
        $('#fbm_staruser .fa-star').removeClass('text-warning');
    }

    $('#fbm_hideuser').attr('data-hide', _hide);
    if (_hide == 1) {
        $('#fbm_hideuser .fa-eye').addClass('text-danger');
    }
    else {
        $('#fbm_hideuser .fa-eye').removeClass('text-danger');
    }
}
//#endregion

//#region // Block
function FBM_BlockAndUn () {
    if ($('#fbm_blockuser').attr('data-block') == "block") {
        FBM_UnBlock();
    }
    else {
        FBM_Block();
    }
}
async function FBM_Block () {
 
    return new Promise((resolve) => {
        $.ajax({
            url: fl_block,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                "psid": fb_userid
                , "access_token": fb_pagetoken
            }),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.responseJSON.error.message != undefined) {
                    notiError(XMLHttpRequest.responseJSON.error.message);
                }
            },
            success: function (result) {

                if (JSON.stringify(result).includes('true')) {
                    $('#fbm_blockuser').addClass('blocked');
                    $('#fbm_blockuser').attr('data-block', 'block');
                }
                else {
                    notiError("Có lỗi xảy ra");
                }
            }
            , beforeSend: function () {
                $('#fbm_blockuser').addClass('executing');

            },
            complete: function () {
                $('#fbm_blockuser').removeClass('executing');
            }

        });
        resolve();
    })
   
 
}
async function FBM_UnBlock () {
    return new Promise((resolve) => {
        $.ajax({
            url: fl_block,
            dataType: "json",
            type: "DELETE",
            contentType: "application/json",
            data: JSON.stringify({
                "psid": fb_userid
                , "access_token": fb_pagetoken
            }),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.responseJSON.error.code == 100) {
                    notiError("Không thể thao tác trên người này");
                }
            },
            success: function (result) {
                if (JSON.stringify(result).includes('true')) {
                    $('#fbm_blockuser').removeClass('blocked');
                    $('#fbm_blockuser').attr('data-block', '');
                }
                else {
                    notiError("Có lỗi xảy ra");
                }
            },
            beforeSend: function () {
                $('#fbm_blockuser').addClass('executing');

            },
            complete: function () {
                $('#fbm_blockuser').removeClass('executing');
            }

        });
        resolve();
    })
   

}
async function FBM_CheckBlock () {
    return new Promise((resolve) => {
         
        $.ajax({
            url: fl_checkblock.replace('{uid}', fb_userid),
            dataType: "json",
            type: "GET",
            data: JSON.stringify({}),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                notiError();
            },
            success: function (result) {
                if (result.data != undefined && result.data != '' && result.data.length == 1) {
                    $('#fbm_blockuser').addClass('blocked');
                    $('#fbm_blockuser').attr('data-block', 'block');

                }
                else {
                    $('#fbm_blockuser').removeClass('blocked');
                    $('#fbm_blockuser').attr('data-block', '');
                }
            },
            beforeSend: function () {
                $('#fbm_blockuser').addClass('executing');

            },
            complete: function () {
                $('#fbm_blockuser').removeClass('executing');
            }

        })
        resolve();
    })
   

}
//#endregion

//#region // Typing on
async function FBM_Typingon () {
    return new Promise((resolve) => {
        setTimeout(() => {
            var dataSend = {
                "recipient": {
                    "id": fb_userid.toString()
                },
                "sender_action": "typing_on"

            }
            $.ajax({
                url: fl_actionmess,
                data: dataSend,
                type: 'POST',
                success: function (result) {
                },
                error: function (e) {
                }
                , beforeSend: function () {

                }
            });
            resolve();
        }, 10);
    })

    
}
async function FBM_Markseen () {
    return new Promise((resolve) => {
        setTimeout(() => {
            var dataSend = {
                "recipient": {
                    "id": fb_userid.toString()
                },
                "sender_action": "mark_seen"

            }
            $.ajax({
                url: fl_actionmess,
                data: dataSend,
                type: 'POST',
                success: function (result) {

                },
                error: function (e) {

                }
                , beforeSend: function () {

                }
            });
            resolve();
        }, 10);
    })
}
//#endregion

//#region // Read , star, sent , hide
 
async function FBM_Markread (isread) {
    return new Promise((resolve) => {
        AjaxJWT(url = "/api/FacebookBridge/Markread"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
                , 'status': isread
                , 'content': ""
            })
            , async = true
            , success = function (result) {

            }
            , before = function (e) {
                if (isread == 0) $('#con' + fbm_conid).addClass('unread');
                else $('#con' + fbm_conid).removeClass('unread');
            }
 
        );
        resolve();
    })
}
function FBM_MarkStar () {
     
    let isstar = 0;
    if ($('#fbm_staruser').attr('data-star') == 1) isstar = 0;
    else isstar = 1;
    if (isstar == 1) {
        $('#star' + fbm_conid).addClass('isstar');
        $('#fbm_staruser .fa-star').addClass('text-warning');
    }
    else {
        $('#star' + fbm_conid).removeClass('isstar');
        $('#fbm_staruser .fa-star').removeClass('text-warning');
    }
    $('#con' + fbm_conid).attr('data-star', isstar);
    $('#fbm_staruser').attr('data-star', isstar);
    FBM_MarkStarExe(isstar);
}
async function FBM_MarkStarExe (isstar) {
    return new Promise((resolve) => {
        AjaxJWT(url = "/api/FacebookBridge/Star"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
                , 'status': isstar
                , 'content': ""
            })
            , async = true
            , success = function (result) {
            }
        );
        resolve();
    })
}
async function MarkSent (snippet) {
    return new Promise((resolve) => {
        AjaxJWT(url = "/api/FacebookBridge/MarkSent"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
                , 'status': 0
                , 'content': snippet
            })
            , async = true
            , success = function (result) {
            }
        );
        resolve();
    })
}


function FBM_MarkHide () {
    
    let ishide = 0;
    if ($('#fbm_hideuser').attr('data-hide') == 1) ishide = 1;
    else ishide = 0;

    if (ishide == 0) {
        $('#hide' + fbm_conid).addClass('ishide');
        $('#fbm_hideuser .fa-eye').addClass('text-danger');
        $('#fbm_hideuser').attr('data-hide', 1);
        $('#con' + fbm_conid).attr('data-hide', 1);
        FBM_MarkHideExe(1);
    }
    else {
        $('#hide' + fbm_conid).removeClass('ishide');
        $('#fbm_hideuser .fa-eye').removeClass('text-danger');
        $('#fbm_hideuser').attr('data-hide', 0);
        $('#con' + fbm_conid).attr('data-hide', 0);
        FBM_MarkHideExe(0);
    }
   
   
  
}
async function FBM_MarkHideExe (ishide) {
    return new Promise((resolve) => {
        AjaxJWT(url = "/api/FacebookBridge/Hide"
            , data = JSON.stringify({
                'converid': fbm_conid
                , 'keypage': fb_pageid
                , 'status': ishide
                , 'content': ""
            })
            , async = true
            , success = function (result) {
            }
        );
        resolve();
    })
}
//#endregion

//#region // Extent info
function FBM_Exteninfo () {
    if ($('#fbc_profileinfo').hasClass('active')) {
        $('#fbc_profileinfo').removeClass('active');
        $('#fbm_userextent').html('Xem thêm');
    }
    else {
        $('#fbc_profileinfo').addClass('active');
        $('#fbm_userextent').html('Thu gọn');
    }
}

function FBM_ExtenInfoAction() {
    $("#fbm_btncontent").toggleClass('active');
}

//#endregion