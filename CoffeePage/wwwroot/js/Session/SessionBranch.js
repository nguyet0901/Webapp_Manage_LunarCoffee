
function Set_Session_Branch(strBranch) {
    let _session = {};
    if (strBranch.length != 0) {
        for (let i = 0; i < strBranch.length; i++) {
            let _e = {};
            _e.Name = strBranch[i].Name;
            _e.ShortName = strBranch[i].ShortName;
            _session[strBranch[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_Branch, JSON.stringify(_session));
}
function Get_Session_Branch() {
    try {
        let data_session = localStorage.getItem(_Session_Branch);
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
function Get_Session_Branch_ByString(token_branch) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Branch());
        if (Object.values(_data_session).length > 0) {
            let _branch = token_branch.split(",");
            if (_branch.length != 0) {
                for (let i = 0; i < _branch.length; i++) {
                    if (_data_session[_branch[i]] != undefined) {
                        result += (_data_session[_branch[i]].Name).trim() + ', ';
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


