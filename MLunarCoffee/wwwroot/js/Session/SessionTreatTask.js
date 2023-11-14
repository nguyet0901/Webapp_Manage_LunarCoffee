
function Set_Session_TreatTask(strTreatTask) {
    let _session = {};
    if (strTreatTask.length != 0) {
        for (let i = 0; i < strTreatTask.length; i++) {
            let _e = {};
            _e.Name = strTreatTask[i].Name;
            _session[strTreatTask[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_TreatTask, JSON.stringify(_session));
}
function Get_Session_TreatTask() {
    try {
        let data_session = localStorage.getItem(_Session_TreatTask);
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
function Get_Session_TreatTask_Name_ByToken(token_TreatTask) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_TreatTask());
        if (Object.values(_data_session).length > 0) {
            let _treatTask = token_TreatTask.split(",");
            if (_treatTask.length != 0) {
                for (let i = 0; i < _treatTask.length; i++) {
                    if (_data_session[_treatTask[i]] != undefined) {
                        result += (_data_session[_treatTask[i]].Name).trim() + ', ';
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

