async function apipro_getList(para, beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/Product/GetList"
                , data = JSON.stringify({
                    'TypeID': para?.TypeID ?? 0,
                    'ProductID': para?.ProductID ?? 0,
                    'HasDisable': para?.HasDisable ?? 0,
                    'PagingNumber': para?.PagingNumber ?? 1,
                    'TextSearch': para?.TextSearch ?? "",
                    'Limit': para?.Limit ?? 1000,
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