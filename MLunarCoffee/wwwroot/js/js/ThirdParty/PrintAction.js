async function pra_getTypeList(beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/DataPrint/GetTypeList"
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

async function pra_getComandList(beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/DataPrint/GetComandList"
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

async function pra_getList(beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = `/api/DataPrint/GetList/${sys_dencos_Main}`
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
async function pra_getItem(code,beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = `/api/DataPrint/GetItem/${code}`
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