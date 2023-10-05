
//#region // Abort Ajax
var Global_AjaxLoad = {};
function AjaxLoad_XhrIsExist(url) {
    if (url == undefined || url == '') return;
    if (Global_AjaxLoad[url] != undefined) {
        if (Global_AjaxLoad[url].readyState != 4)
            Global_AjaxLoad[url].abort();
        AjaxLoad_XhrDelete(url);
    }
}
function AjaxLoad_XhrPush(url, xhr) {
    if (url == undefined || url == '') return;
    if (Global_AjaxLoad[url] == undefined) {
        Global_AjaxLoad[url] = xhr;
    }
}
function AjaxLoad_XhrDelete(url) { 
    if (url == undefined || url == '') return;
    if (Global_AjaxLoad[url] != undefined) {
        delete Global_AjaxLoad[url];
    }
}
//#endregion 

function AjaxLoad(url, data, async, error, success, sender, before, complete) {
    var xhrequest = $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: 'application/x-www-form-urlencoded',
        async: async,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (error != undefined && error != null && error.length != 0) error();
        },
        success: function (result) {
            AjaxLoad_XhrDelete(url);
            if (result != undefined)
                if (success != undefined && success != null && success.length != 0) success(result);
        },
        beforeSend: function (xhr) {
            AjaxLoad_XhrIsExist(url);
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            if (before != undefined && before != null && before.length != 0) before();
            if (sender != undefined && sender != null && sender.length != 0)
                sender.css('pointer-events', 'none');
        },
        complete: function (e) {
            if (complete != undefined && complete != null && complete.length != 0) complete(e);
            if (sender != undefined && sender != null && sender.length != 0)
                sender.css('pointer-events', 'auto');
        }
    });
    AjaxLoad_XhrPush(url, xhrequest);
    return xhrequest;
}

function AjaxJWT (url, data, async, success, before) {
    return $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: 'application/json; charset=utf-8',
        async: async,
        success: function (result) {
            if (result != undefined)
                if (success != undefined && success != null && success.length != 0)
                    success(result);
        }
        , beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
            if (before != undefined && before != null && before.length != 0) before();
        },
    });
}
function AjaxApi (url, data, async, success, before, complete) {
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: 'application/json; charset=utf-8',
        async: async,
        success: function (result) {
            if (result != undefined)
                if (success != undefined && success != null && success.length != 0)
                    success(result);
        }
        , beforeSend: function (e) {
            if (before != undefined && before != null && before.length != 0) before();
        },
        complete: function (e) {
            if (complete != undefined && complete != null && complete.length != 0) complete(e);
        }
    });
}

function AjaxUpload (url, inputid, success, error) {
    $('#' + inputid).unbind("change");
    $('#' + inputid).change(function () {
        let input = document.getElementById(inputid);
        let files = input.files;
        let formData = new FormData();
        for (let i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }

        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (error != undefined && error != null && error.length != 0) error();
                },
                success: function (result) {
                    if (result != undefined)
                        if (success != undefined && success != null && success.length != 0)
                            success(result);
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
                }
            }
        );
    })
}
function AjaxUpload_Multi (url, inputid, success, error, before, complete, funmaxrange) {
    $('#' + inputid).unbind("change");
    $('#' + inputid).change(function () {
        var promises = [];
        let input = document.getElementById(inputid);
        let files = input.files;
        if (files.length <= 0 || files.length > 5) {
            if (funmaxrange != undefined && funmaxrange != null)
                funmaxrange();
        }
        else {
            for (let i = 0; i != files.length; i++) {
                promises.push(AjaxUpload_MultiExe(url, files[i], files[i].name, success, error, before));
            }
            Promise.all(promises).then((values) => { });
        }
    });
}
function AjaxUpload_MultiExe (url, file, namefile, success, error, before) {
    return new Promise(resolve => {
        let formData = new FormData();
        formData.append("files", file);
        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (error != undefined && error != null && error.length != 0) error(namefile);
                },
                success: function (result, e) {
                    if (result != undefined) {
                        if (success != undefined && success != null && success.length != 0)
                            success(result, namefile);
                    }
                    resolve(true);
                },
                beforeSend: function (xhr, e) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
                    if (before != undefined && before != null && before.length != 0)
                        before(namefile);
                }
            }
        );
    });

}
function AjaxSendEmail (url, formData, async, success, error, before, complete) {
    return new Promise(resolve => {
        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                async: async,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (error != undefined && error != null && error.length != 0) error();
                },
                success: function (result, e) {
                    if (result != undefined)
                        if (success != undefined && success != null && success.length != 0)
                            success(result);
                },
                beforeSend: function (xhr, e) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
                    if (before != undefined && before != null && before.length != 0) before();
                },
                complete: function (e) {
                    if (complete != undefined && complete != null && complete.length != 0) complete(e);
                }
            }
        );
    });
}

function AjaxSendEmailMulti(url, formData, async, success, error, before, complete) {
    return new Promise(resolve => {
        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                async: async,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (error != undefined && error != null && error.length != 0) error();
                },
                success: function (result, e) {
                    if (result != undefined)
                        if (success != undefined && success != null && success.length != 0)
                            success(result);
                },
                beforeSend: function (xhr, e) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
                    if (before != undefined && before != null && before.length != 0) before();
                },
                complete: function (e) {
                    if (complete != undefined && complete != null && complete.length != 0) complete(e);
                }
            }
        );
    });
}

function AjaxUpload_Image (url, inputid) {
    return new Promise((resolve, reject) => {
        let input = document.getElementById(inputid);
        let files = input.files;
        let formData = new FormData();
        for (let i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }

        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    reject('');
                },
                success: function (result) {
                    if (result != undefined)
                        resolve(result);
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("WebToken"));
                }
            }
        );
    });
}


