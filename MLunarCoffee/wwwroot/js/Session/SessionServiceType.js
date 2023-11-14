function Set_Session_ServiceType(strServiceType) {
    let _session = {};
    if (strServiceType.length != 0) {
        for (let i = 0; i < strServiceType.length; i++) {
            let _e = {};
            _e.ID = strServiceType[i].ID;
            _e.Name = strServiceType[i].CategoryName;
            _e.Color = strServiceType[i].Color;
            _session[strServiceType[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_ServiceType, JSON.stringify(_session));
}
function Get_Session_ServiceType() {
    try {
        let data_session = localStorage.getItem(_Session_ServiceType);
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
function Get_Session_ServiceType_ByString(token_ServiceType) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_ServiceType());
        if (Object.values(_data_session).length > 0) {
            let _ServiceType = token_ServiceType.split(",");
            if (_ServiceType.length != 0) {
                for (let i = 0; i < _ServiceType.length; i++) {
                    if (_data_session[_ServiceType[i]] != undefined) {
                        result += (_data_session[_ServiceType[i]].Name).trim() + ', ';
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