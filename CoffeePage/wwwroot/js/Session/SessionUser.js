
function Set_Session_User(strUser) {
    let _session = {};
    if (strUser.length != 0) {
        for (let i = 0; i < strUser.length; i++) {
            let _e = {};
            _e.Name = strUser[i].Name;
            _e.Avatar = strUser[i].Avatar;
            _session[strUser[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_User, JSON.stringify(_session));
}
function Get_Session_User() {
    try {
        let data_session = localStorage.getItem(_Session_User);
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
function Get_Session_User_ByGroupID(groupID) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_User());

        for (const [key, value] of Object.entries(_data_session)) {
            if (value.GroupID != Number(GroupID)) {
                delete _data_session[key];
            }
        }

        return _data_session;
    }
    catch {
        return "";
    }
}
function Get_Session_User_Object_ByID(userid) {
    try {
        let result = null;
        let _data_session = JSON.parse(Get_Session_User());
        if (Object.values(_data_session).length > 0) {
            if (_data_session[userid] != undefined) {
                result = _data_session[userid] ;
            }
        }
        return result;
    }
    catch {
        return null;
    }
}
