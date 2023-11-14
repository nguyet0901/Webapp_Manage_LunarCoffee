function Set_Session_Teeth(strTeeth) {
    let _session = {};
    if (strTeeth.length != 0) {
        for (let i = 0; i < strTeeth.length; i++) {
            let _e = {};
            _e.TeethName = strTeeth[i].TeethName;
            _e.TeethNameBaby = strTeeth[i].TeethNameBaby;
            _e.TeethNameMerge = strTeeth[i].TeethNameMerge;
            _session[strTeeth[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_Teeth, JSON.stringify(_session));
}
function Get_Session_Teeth() {
    try {
        let data_session = localStorage.getItem(_Session_Teeth);
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

function Get_Session_Teeth_Names_ByToken(token_Teeth, type) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Teeth());
        if (Object.values(_data_session).length > 0) {
            let _Teeth = token_Teeth.split(",");
            if (_Teeth.length != 0) {
                for (let i = 0; i < _Teeth.length; i++) {
                    if (_data_session[_Teeth[i]] != undefined) {
                        if (type == 0) {
                            result += (_data_session[_Teeth[i]].TeethName).trim() + ', ';
                        }
                        else if (type == 1) {
                            result += (_data_session[_Teeth[i]].TeethNameBaby).trim() + ', ';
                        }
                        else {
                            result += (_data_session[_Teeth[i]].TeethNameMerge).trim() + ', ';
                        }
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