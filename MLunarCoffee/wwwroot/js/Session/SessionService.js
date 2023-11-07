function Set_Session_Service(strService) {
    let _session = {};
    if (strService.length != 0) {
        for (let i = 0; i < strService.length; i++) {
            let _e = {};
            _e.Name = strService[i].Name;
            _e.Color = strService[i].Color;
            _e.Type = strService[i].Type;
            _session[strService[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_Service, JSON.stringify(_session));
}
function Get_Session_Service() {
    try {
        let data_session = localStorage.getItem(_Session_Service);
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
function Get_Session_Service_Names_ByToken(token_Service) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Service());
        if (Object.values(_data_session).length > 0) {
            let _Service = token_Service.split(",");
            if (_Service.length != 0) {
                for (let i = 0; i < _Service.length; i++) {
                    if (_data_session[_Service[i]] != undefined) {
                        result += (_data_session[_Service[i]].Name).trim() + ', ';
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

function Get_Session_Service_Names_ByJsonServiceCombo(json_service_combo) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Service());
        if (Object.values(_data_session).length > 0) {
            let data = JSON.parse(json_service_combo); 
            let StringContent = "";
            if (Object.values(data).length > 0) {
                for ([key, value] of Object.entries(data)) { 
                    if (_data_session[key] != undefined) {
                        StringContent += '<div class="cur_service">' + (_data_session[key].Name).trim() + '</div>';
                    }
                };
                result += StringContent;
            } 
        }
        return result;
    }
    catch {
        return "";
    }
}

function Get_Session_Service_Color_ByToken(token_Service) {
    try {
        let result = "#ffffff";
        let _data_session = JSON.parse(Get_Session_Service());
        if (Object.values(_data_session).length > 0) {
            let _Service = token_Service.split(",");
            if (_Service.length != 0) {
                for (let i = 0; i < _Service.length; i++) {
                    if (_data_session[_Service[i]] != undefined) {
                        result = _data_session[_Service[i]].Color;
                        return result;
                    }
                }
                
            }

        }
        return result;
    }
    catch {
        return "#ffffff";
    }
}

function Get_Session_Service_To_Service_Type_ByToken(token_Service) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Service());
        if (Object.values(_data_session).length > 0) {
            let _Service = token_Service.split(",");
            if (_Service.length != 0) {
                let obj = {};
                for (let i = 0; i < _Service.length; i++) {
                    if (_data_session[_Service[i]] != undefined) {
                        let type = _data_session[_Service[i]].Type;
                        obj[type] = type;
                    }
                }
                if (Object.values(obj).length > 0) {
                    for ([key, value] of Object.entries(obj)) {
                        result += key + ',';
                    }
                    result = ',' + result;
                }             
            }
        }
        return result;
    }
    catch {
        return "";
    }
}
function Get_Session_ServiceID_From_ServiceType(_ServiceType) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Service());
        if (Object.values(_data_session).length > 0) { 
            if (_ServiceType != 0) { 
               
                if (Object.values(_data_session).length > 0) {
                    for ([key, value] of Object.entries(_data_session)) {
                        if (value.Type == _ServiceType) { 
                            result += key + ',';                 
                        }
                        
                    }
                    
                }
            }
        }
        return result;
    }
    catch {
        return "";
    }
}