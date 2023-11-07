
async function Location_GetList(Type, ValueCode, beforefunc, successfunc, failurefunc, completefunc) {   
    if (typeof beforefunc == 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {
            AjaxJWT(url = `/api/${Type}/GetList`
                , data = JSON.stringify({ "ParentCode": ValueCode }) 
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