function Set_Session_ServiceCare(strServiceCare) {
    let _session = {};
    if (strServiceCare.length != 0) {
        for (let i = 0; i < strServiceCare.length; i++) {
            let _e = {};
            _e.Name = strServiceCare[i].Name;
            _e.Color = strServiceCare[i].Color;
            _session[strServiceCare[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_ServiceCare, JSON.stringify(_session));
}
function Get_Session_ServiceCare() {
    try {
        let data_session = localStorage.getItem(_Session_ServiceCare);
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
function Get_Session_ServiceCare_Names_ByToken(token_ServiceCare) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_ServiceCare());
        if (Object.values(_data_session).length > 0) {
            let _ServiceCare = token_ServiceCare.split(",");
            if (_ServiceCare.length != 0) {
                for (let i = 0; i < _ServiceCare.length; i++) {
                    if (_data_session[_ServiceCare[i]] != undefined) {
                        result += (_data_session[_ServiceCare[i]].Name).trim() + ', ';
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
function Get_Session_ServiceCare_Color_ByToken(token_ServiceCare) {
    try {
        let result = "#ffffff";
        let _data_session = JSON.parse(Get_Session_ServiceCare());
        if (Object.values(_data_session).length > 0) {
            let _ServiceCare = token_ServiceCare.split(",");
            if (_ServiceCare.length != 0) {
                for (let i = 0; i < _ServiceCare.length; i++) {
                    if (_data_session[_ServiceCare[i]] != undefined) {
                        result = _data_session[_ServiceCare[i]].Color;
                        return result;
                    }
                };

            }
        }
        return result;
    }
    catch {
        return "#ffffff";
    }
}