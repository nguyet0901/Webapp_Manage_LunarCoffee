function Set_Session_ServiceStage(strServiceStage) {
    let _session = {};
    if (strServiceStage.length != 0) {
        for (let i = 0; i < strServiceStage.length; i++) {
            let _e = {};
            _e.ServiceID = strServiceStage[i].ServiceID;
            _e.Name = strServiceStage[i].Name;
            _e.Percent = strServiceStage[i].Percent;
            _session[strServiceStage[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_ServiceStage, JSON.stringify(_session));
}
function Get_Session_ServiceStage() {
    try {
        let data_session = localStorage.getItem(_Session_ServiceStage);
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


function Get_Session_ServiceStage_Names_ByToken(token_stage) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_ServiceStage());
        if (Object.values(_data_session).length > 0) {
            let _Stage = token_stage.split(",");
            if (_Stage.length != 0) {
                for (let i = 0; i < _Stage.length; i++) {
                    if (_data_session[_Stage[i]] != undefined) {
                        result += (_data_session[_Stage[i]].Name).trim() + ' (' + _data_session[_Stage[i]].Percent + '%)' + ', ';
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