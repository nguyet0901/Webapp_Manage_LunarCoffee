function FBC_GetLink (_ver, _id, _token, _action) {
  
    fl_conver = (flr_conver.replace('{version}', _ver))
        .replace('{pageid}', _id)
        .replace('{action}', ((_action != '') ? `&folder=${_action}` :``))
        + "&access_token=" + _token;
 
    fl_profile = flr_profile.replace('{version}', _ver) + "&access_token=" + _token;
    fl_avatar = flr_avatar + "&access_token=" + _token;
    fl_messlist = flr_messlist.replace('{version}', _ver) + "&access_token=" + _token;
    fl_messdetail = flr_messdetail.replace('{version}', _ver) + "&access_token=" + _token;
    fl_sendmess = (flr_sendmess.replace('{version}', _ver)).replace('{pageid}', _id);
    fl_attachupload = flr_attachupload.replace('{version}', _ver) + "?access_token=" + _token;
    fl_block = (flr_block.replace('{version}', _ver)).replace('{pageid}', _id);
    fl_checkblock = (flr_checkblock.replace('{version}', _ver)).replace('{pageid}', _id)
        + "&access_token=" + _token;
    fl_actionmess = (flr_actionmess.replace('{version}', _ver)) + "?access_token=" + _token;
     
}
function FBC_LoadReset () {
    fbl_flag = 0;
    $('#fbf_shohide').prop('checked', false);
    $('#fbf_follow').prop('checked', false);
    $('#fbf_read').prop('checked', false);
    $("#fbf_taglist .filter ").removeClass('active');
    fbl_flag = 1;
    FBC_Load();
}
function FBC_Load( _converid = 0, _isloadmore = 0, _search = "" ,_tag=0) {
    if (fbl_flag == 1) {
        if (_isloadmore == 0 && _converid == 0) {
            fb_begindate = 0;
            $('#fbc_list').html('');
            $('#fbc_begin').removeClass('d-none');
            $('#fbc_chatting').addClass('d-none');
        }
 
        let _ishide = ($('#fbf_shohide').is(":checked") ? 1 : 0);
        let _isstar = ($('#fbf_follow').is(":checked") ? 1 : 0);
        let _isread = ($('#fbf_read').is(":checked") ? 1 : 0);
 
        if (fb_xhrloadconver && fb_xhrloadconver.readyState != 4) fb_xhrloadconver.abort();
        fb_xhrloadconver = AjaxJWT(url = "/Api/FacebookBridge/LoadConversation"
            , data = JSON.stringify({
                "converid": _converid,
                "pageID": fb_pageid,
                "numBeginDate": fb_begindate,
                "limit": fb_limit,
                "search": _search,
                "read": _isread,
                "star": _isstar,
                "hide": _ishide,
                "tagid": _tag

            })
            , async = true
            , success = function (result) {
                let _data = JSON.parse(result);
                if (_converid != 0) {
                    if (_data && _data.length != 0) {
                        let _itemmess = $("#con" + _converid);
                        let _itemcontent = FBC_RenderEach(_data[0]);
                        if (_itemmess.length != 0) {
                            _itemmess.parent().replaceWith(_itemcontent);
                            $("#con" + _converid).parent().slideToggle(10, function () {
                                $(this).prependTo('#fbc_list').slideToggle(300);
                            });
                        }
                        else {
                            $("#fbc_list").prepend(_itemcontent);
                        }
           
                        FBC_Event();
                    }
             
                }
                else {
                    if (_data && _data.length != 0) {
                        FBC_Render(_data, "fbc_list");
                        fb_begindate = (_data[_data.length - 1]?.numUpdateTime || fb_begindate);
                        fb_scrollstop = 0;
                    }
                    else {
                        if (_isloadmore == 1) {
                            fb_scrollstop = 1;
                        }
                        else {
                            $('#fbc_listemp').removeClass('d-none');
                            $('#fbc_list').addClass('d-none');
                        }
                    }
                }
            
                $('#fbc_wait').addClass('d-none');
                $("#fb_seachmess .fa-search").show();
                $("#fb_seachmess .spinner-border").hide();
            }
            , before = function (e) {
                $('#fbc_wait').removeClass('d-none');
                $('#fbc_listemp').addClass('d-none');
                $('#fbc_list').removeClass('d-none');
            }
        );
    }
    //$.ajax({
    //    url: fl_conver,
    //    dataType: "json",
    //    type: "GET",
    //    data: JSON.stringify({}),
    //    contentType: 'application/json; charset=utf-8',
    //    async: true,
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        notiError();
    //    },
    //    success: function (result) {
    //        $('#fbc_list').html('');
    //        FBC_Render(result.data,"fbc_list");
    //    },
    //    beforeSend: function () {
    
    //        $('#fbc_wait').removeClass('d-none');
    //    },
    //    complete: function () {
    //        $('#fbc_wait').addClass('d-none');
    //        $("#fbc_chatlistmaster").addClass("active");
    //    }

    //})
}
function FBC_ConverReload(_converid = 0) {
    if (_converid && _converid != 0) {
        FBC_Load(_converid, 0, '',0);
    }
}

//#region // Filter
function fbf_FilterExe(){
    FBC_Load(0,0,"",0);
}
 
async function fbf_RenderTag (data, id) {
    new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            for (var [key, value] of Object.entries(data)) {
                let item = value;
                let tr = `
                    <li data-id="${item.ID}" class="nav-item p-2 rounded-2 filter text-start cursor-pointer w-100 my-1 ">
                          <a class="text-sm px-3  py-1 rounded-2" style="background: ${item.Color}2b; color: ${item.Color};">${item.Name}</a>
                    </li>
 
                        `;
                myNode.insertAdjacentHTML('beforeend', tr);
            }
        }
        fbf_EventTag();
        resolve();
    })
}
function fbf_FilterClose () {
    $('#fbc_filter').removeClass('show');
}
function fbf_EventTag () {
    $("#fbf_taglist .filter ").unbind("click").on("click", function () {
        let tag = 0;
        if ($(this).hasClass('active')) {
            tag = 0;
            $("#fbf_taglist .filter ").removeClass('active');
        }
        else {
            $("#fbf_taglist .filter ").removeClass('active');
            $(this).addClass('active');
            tag = $(this).attr('data-id');
        }
 
        FBC_Load(0, 0, "", tag);
    })
  
}
//#endregion

//#region // RENDER CONVERSATION
async function FBC_Render (data, id) {
    return new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let item = data[i];
                    let tr = FBC_RenderEach(item);
                    myNode.insertAdjacentHTML("beforeend", tr);
                    FBC_TagRender(item.conserid, item.Tag);
                }
            }
            FBC_Event();
        }
    })
}
function FBC_RenderEach (item) {
    try {
        if ($("#con" + item.conserid).length != 0) {
            return "";
        }
        let _cread = '', _star = '',_ishide='';
        if (item.isunread==1 || (fb_pageid != item.lastsender && item.pread_time <= item.update_time)) {
            _cread = 'unread';
        }
        if (item.isstar == 1) _star = `isstar`;
        if (item.Ishide == 1) _ishide = `ishide`;

        let _avatar = '/default.png';
        let pic = (item.frompicture != '') ? item.frompicture : _avatar;
        let result = `
                 <li class="nav-item position-relative" role="presentation">
                    <a id="con${item.conserid}" data-id="${item.conserid}"
                        data-star="${item.isstar}"
                        data-tag="${item.Tag}"
                        data-hide="${item.Ishide}"
                        data-toid="${fb_pageid}"
                        data-fromid="${item.fromid}" data-name="${item.fromname}"
                        data-url="${pic}" class="${_cread} from${item.fromid} itemmes text-sm nav-link cursor-pointer px-2">
                        <div class="d-flex conitem">
                            <div class="position-relative">
                                <i id="hide${item.conserid}" class="fas ms-n2 fa-eye fs-6 position-absolute opacity-0 text-danger ${_ishide}"></i>
                                 <img data-fromid="${item.fromid}" data-id="${item.conserid}" class="proavatar   border border-1  " onerror="Master_OnLoad_Error_Image(this)" src="${pic}" alt="label-image" />
                            </div>
                            <div  class="ms-2">
                                <h6 class="name text-sm mb-0 ellipsis_one_line">${item.fromname}</h6>
                                <div class="mt-0 d-flex text-secondary align-items-center">
                                    ${fb_pageid == item.lastsender ? `<span id="psend${item.conserid}" class="psend${item.fromid} pe-1">Bạn:</span>`:``}
                                    <div id="snip${item.conserid}" class="snip${item.fromid} content twoline">${item.snippet}</div>
                                   
                                </div>
                                <div id="tag${item.conserid}" class="d-flex mt-1">
                                    
                                </div>
                            </div>
                            <div class="ms-auto ps-0 width-48-px text-end">
                                <span class="text-xs fst-italic time">${ConvertDateTimeUTC_NoYear(item.update_time)}</span>
                                <div class="d-flex align-items-center position-relative mt-2">
                                    <i id="star${item.conserid}" class="fas fa-star fs-6 ms-auto opacity-0 text-warning ${_star}"></i>
                                    <div class="circle badge badge-xs badge-circle bg-gradient-success p-0 d-none ms-auto"><span></span></div>
                                </div>
                                 
                                
                            </div>
                        </div>
                    </a>
                    <hr class="horizontal dark my-0 opacity-2">
                </li>
            `
        return result;  
    }
    catch (ex) {
        return '';
    }
}
function FBC_RenderEachAttach (_lastmes) {
    if (_lastmes.message != undefined && _lastmes.message != "") {
        return `<span class="twoline">${_lastmes.message}</span>`;
    }
    else {
        if (_lastmes.sticker != undefined && _lastmes.sticker != "") {
            return `<img src="${_lastmes.sticker}" class="avatar avatar-xs"/>`; 
        }
        if (_lastmes.attachments != undefined && _lastmes.attachments.data[0] != undefined) {
            let _fiattch = _lastmes.attachments.data[0];
            let _name = _fiattch.name != undefined ? _fiattch.name : '';
            if (_fiattch.mime_type.includes('image')) return `<i class="text-primary fas fa-image pe-1"></i> Gửi hình ảnh`;
            else if (_fiattch.mime_type.includes('audio')) return `<i class="text-primary fas fa-volume-up pe-1"></i>${_name}`;
            else if (_fiattch.mime_type.includes('video')) return `<i class="text-primary fas fa-video pe-1"></i>${_name}`
            else return `<i class="far fa-file-alt pe-2"></i>${_name}` 
        }
    }
    return "";
}
async function FBC_TagRender (conver,tag) {
    return new Promise((resolve) => {
 
        let _tagarea = document.getElementById("tag" + conver);
        if (_tagarea != null) {
            _tagarea.innerHTML = "";
            let _ob = tag.split('t');
            let _dy = [];
            let _maxblock = 2;
            let _indexblock = 0;
            let _content = '';
            for (let ii = 0; ii < _ob.length; ii++) {
                if (_ob[ii] != "") {
                    _dy.push(_ob[ii]);
                }
            }

            for (ii = 0; ii < _dy.length; ii++) {
                _indexblock = _indexblock + 1;
                if (_indexblock <= _maxblock) {
                    let item = fb_fbtag[_dy[ii]];
                    if (item != undefined && item != "") {
                        _content = _content +
                            `<div class="contag text-sm me-1 rounded-2" title="${item.Name}"  style="background: ${item.Color}2b; color: ${item.Color};">
                        ${item.Name}
                    </div>`
                    }
                }
                else {
                    let _moretag = '';
                    for (k = _maxblock; k < _dy.length; k++) {
                        let _m = fb_fbtag[_dy[k]];
                        _moretag = _moretag +
                            `<div class="contag text-sm me-1 rounded-2" title="${_m.Name}"  style="background: ${_m.Color}2b; color: ${_m.Color};">
                                ${_m.Name} </div>`
                    }
                    _content = _content +
                        `<div class="position-relative d-inline ps-0">
                            <a class="contag my-1"  data-bs-toggle="collapse" href="#tagmore${conver}" role="button" aria-expanded="false" aria-controls="collapseExample" style="background: #bdbdbd29;color: #000000;" >
                                + ${_dy.length - _indexblock + 1} tags
                            </a>
                            <div class="collapse position-absolute z-index-4 end-1 more top-100 mt-2" id="tagmore${conver}" style="min-width: 250px; z-index: 1000;">
                                <div class="card card-body shadow-lg">
                                    ${_moretag}
                                </div>
                            </div>
                        </div>`
                    break;
                }
            }
            _tagarea.innerHTML = _content;
        }

      
         
    })
}
async function FBC_GetAvatar () {
    return new Promise((resolve) => {
        $("#fbc_list .proavatar").each(function (index) {
            let _id = $(this).attr('data-id');
            let _fromid = $(this).attr('data-fromid');
            let _url = fl_avatar.replace('{uid}', _fromid);
            $(this).attr("src", _url);
            $('#con' + _id).attr("data-url", _url);
        });
    })
}
function FBC_Event () {
    $('#fbc_list .itemmes').unbind('click').click(function () {
        $('#fbc_list .itemmes').removeClass('active');
        $(this).addClass('active');
        fbm_conid = $(this).attr('data-id');
        let _fromid = $(this).attr('data-fromid');
        let _toid = $(this).attr('data-toid');
        fb_userid = _fromid == fb_pageid ? _toid : _fromid;
        FMB_Ini(fbm_conid, $(this).attr('data-name'), $(this).attr('data-url'));
        $('#fbc_begin').addClass('d-none');
        $('#fbc_chatting').removeClass('d-none');
        $("#fbc_chatlistmaster").removeClass('active');
        let _star = $(this).attr('data-star');
        let _hide = $(this).attr('data-hide');
        fb_currentag = $(this).attr('data-tag');
        PInfo_Ini();
        if (typeof FBM_FillChating === 'function') FBM_FillChating(_star, _hide);
        if (typeof FBM_Markseen === 'function') FBM_Markseen();
        if (typeof FBM_Markread === 'function') FBM_Markread(isread = 1);
        if (typeof FBM_ResetFieldInput === 'function') FBM_ResetFieldInput();

    });
}
//#endregion

//#region // Update element

async function FBC_Movetotop (_id) {
    return new Promise((resolve) => {
        $("#con" + _id).prependTo("#fbc_list");
    })
}
async function FBC_ChangeContent (_id,_mess,_type,_senderid ){
    return new Promise((resolve) => {
        switch (_type) {
            case "": {
                $("#snip" + _id).html(_mess);
                break;
            }
            default: {
                $("#snip" + _id).html('Gửi file');
                break;
            }
        }
        if (_senderid == fb_pageid) {
            $("#psend" + _id).removeClass('d-none');
        }
        else {
            $("#psend" + _id).addClass('d-none');
        }
    })
}

//#endregion