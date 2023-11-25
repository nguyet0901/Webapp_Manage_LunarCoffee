async function apiban_getList(para, beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/Banner/GetList"
                , data = JSON.stringify({
                    'BannerID': para?.BannerID ?? 0,
                    'PagingNumber': para?.PagingNumber ?? 1,
                    'Limit': para?.Limit ?? 3,
                })
                , async = true
                , success = function (result) {
                    if (result != "0") {
                        if (typeof successfunc == 'function') successfunc(result);
                    }
                    else {
                        if (typeof failurefunc == 'function') failurefunc(result);
                    }
                    resolve();
                    if (typeof completefunc == 'function') completefunc();
                }
            );
        }, 100)
    })
}

async function apiban_getDetail(id, beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/Banner/GetDetail/" + id
                , data = null
                , async = true
                , success = function (result) {
                    if (result != "0") {
                        if (typeof successfunc == 'function') successfunc(result);
                    }
                    else {
                        if (typeof failurefunc == 'function') failurefunc(result);
                    }
                    resolve();
                    if (typeof completefunc == 'function') completefunc();
                }
            );
        }, 100)
    })
}