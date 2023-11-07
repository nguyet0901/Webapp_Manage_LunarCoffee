function FBR_ReceiptMess (re) {
    let reconverid = re.converid;
    let refromid = re.fromid;
    let reicon = re.icon;
    let rebody = re.body;
    let repageid = re.pageid;
    let retitle = re.title;
    let remid = re.mid;
    let refromname = re.fromname;
    let refrompicture = re.frompicture;
    if (fb_pageid == repageid) {
        if ($('#con' + reconverid).length) {
            FBR_ChangeConver(refromid);
            FBR_ChangeContent(refromid, reicon, rebody);
            if (fb_userid == refromid) {
                FBR_LoadReceip(remid);
            }
            else {
                //FBR_ShowSignnoti(repageid);
                FBR_Notinew(refromname, refrompicture, rebody);
            } 
        }
        else {
            FBC_Load(_converid = reconverid, _isloadmore = 0, _search = "", _tag=0);
            FBR_Notinew(refromname, refrompicture, rebody);
        }
        
    }
    else {
        FBR_ShowSignnoti(repageid);
        FBR_Notinew(refromname, refrompicture, rebody);
    }
}
function FBR_ShowSignnoti (repageid) {
    $("#pr" + repageid).addClass('unread');
    $("#fb_pageunreadcount").addClass('newmess');
}
 
//#region // Update element
async function FBR_ChangeConver (fromid) {
    return new Promise((resolve) => {
        $(".from" + fromid).prependTo("#fbc_list");
        $(".from" + fromid).addClass("unread");
    })
}
async function FBR_ChangeContent (fromid, icon,body) {
    return new Promise((resolve) => {
        if (body != "") $(".snip" + fromid).html(body);
        else $(".snip" + fromid).html('Gửi file');
        $(".psend" + fromid).addClass('d-none');
    })
}
//#endregion

//#region // Load particular

async function FBR_LoadReceip (remid) {
    return new Promise((resolve) => {
        $.ajax({
            url: fl_messdetail.replace('{messid}', remid),
            dataType: "json",
            type: "GET",
            data: JSON.stringify({}),
            contentType: 'application/json; charset=utf-8',
            async: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
            success: function (result) {
                document.getElementById("fbm_content").insertAdjacentHTML("beforeEnd"
                    , FBM_RenderEach(result, temp = 0));
                FBM_Scroll("bottom");

                
            }
        })
    })
}

//#endregion

//#region Noti new message
async function FBR_Notinew (name,pic,text) {
    return new Promise(resolve => {
        let _idrandom = FBR_Randomid();
        let tr =
            `<div id="noti_${_idrandom}" class="alertfade alert alert-dark mt-2 text-sm alert-dismissible p-2 mb-0 text-white" role="alert">
                <button type="button" class="btn-close text-lg opacity-10 p-1" data-bs-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
                <div class="d-flex align-items-center me-4">
                    <img src=${pic} class="avatar avatar-sm me-2"/>
                    <div class="">
                        <div class="twoline  fw-bolder">
                            ${name}
                        </div>
                        <div class="twoline">
                            ${text}
                        </div>
                    </div>
                </div>
                
            </div>`;
        $('#fb_receiparea').html($('#fb_receiparea').html() + tr);
        
        let _ib = $('#fb_receiparea .alert');
        if (_ib != undefined && _ib.length >1) {
            $('#fb_receiparea').find('div:first').remove();
        }
        FBR_Closenew(_idrandom);
        setTimeout(() => {
            $(`#noti_${_idrandom}`).removeClass('alertfade');
        }, 1000);
       
    });
}

async function FBR_Closenew (_idrandom) {
    return new Promise(resolve => {
        setTimeout(() => {
             $('#noti_' + _idrandom).remove();
        }, 10000);
      
    });
}
function FBR_Randomid () {
    return Math.floor(Math.random() * 100001).toString() + '-' + (new Date()).getTime().toString();
}
//#endregion