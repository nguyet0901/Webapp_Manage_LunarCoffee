
function Set_Session_Product(strProduct) {
    let _session = {};
    if (strProduct.length != 0) {
        for (let i = 0; i < strProduct.length; i++) {
            let _e = {};
            _e.Name = strProduct[i].Name;
            _session[strProduct[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_Product, JSON.stringify(_session));
}
function Get_Session_Product() {
    try {
        let data_session = localStorage.getItem(_Session_Product);
        if (data_session != null && data_session != undefined) {
            return data_session;
        }
        else {
            return "[]";
        }
    }
    catch {
        return "[]";
    }
}
function Get_Session_Product_Names_ByToken(token_Product) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Product());
        if (Object.values(_data_session).length > 0) {
            let _Product = token_Product.split(",");
            if (_Product.length != 0) {
                for (let i = 0; i < _Product.length; i++) {
                    if (_data_session[_Product[i]] != undefined) {
                        result += (_data_session[_Product[i]].Name).trim() + ', ';
                    }
                }
            }
            result = (result.trim()).slice(0, -1);
        }
        return result;
    }
    catch {
        return "";
    }
}

