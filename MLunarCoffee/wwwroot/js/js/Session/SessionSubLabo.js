
function Set_Session_SubLabo(strSubLabo) {

    let _session = {};
    if (strSubLabo.length != 0) {
        for (let i = 0; i < strSubLabo.length; i++) {
            let _e = {};
            _e.Name = strSubLabo[i].Name;
            _e.Avatar = strSubLabo[i].Avatar;
            _session[strSubLabo[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_SubLabo, JSON.stringify(_session));
}
function Get_Session_SubLabo() {
    try {
        let data_session = localStorage.getItem(_Session_SubLabo);
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


