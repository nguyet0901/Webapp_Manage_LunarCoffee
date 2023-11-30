
function Set_Session_Employee(strEmployee) {
    let _session = {};
    if (strEmployee.length != 0) {
        for (let i = 0; i < strEmployee.length; i++) {
            let _e = {};
            _e.Name = strEmployee[i].Name;
            _e.Avatar = strEmployee[i].Avatar;
            _session[strEmployee[i].ID] = _e;
        }
    }
    localStorage.setItem(_Session_Employee, JSON.stringify(_session));
}
function Get_Session_Employee() {
    try {
        let data_session = localStorage.getItem(_Session_Employee);
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
function Get_Session_Employee_Names_ByToken(token_Employee) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Employee());
        if (Object.values(_data_session).length > 0) {
            let _Employee = token_Employee.split(",");
            if (_Employee.length != 0) {
                for (let i = 0; i < _Employee.length; i++) {
                    if (_data_session[_Employee[i]] != undefined) {
                        result += (_data_session[_Employee[i]].Name).trim() + ', ';
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
function Get_Session_Employee_Names_ByID(empID) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Employee());
        if (Object.values(_data_session).length > 0) {
            if (_data_session[empID] != undefined) {
                result = _data_session[empID].Name;
            }
        }
        return result;
    }
    catch {
        return "";
    }
}
function Get_Session_Employee_ByGroupID(groupID) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Employee());

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
function Get_Session_Employee_Avatar_ByID(empID) {
    try {
        let result = "";
        let _data_session = JSON.parse(Get_Session_Employee());
        if (Object.values(_data_session).length > 0) {
            if (_data_session[empID] != undefined) {
                result = _data_session[empID].Avatar;
            }
        }
        return result;
    }
    catch {
        return "";
    }
}
function Get_Session_Employee_Object_ByID(empID) {
    try {
        let result = null;
        let _data_session = JSON.parse(Get_Session_Employee());
        if (Object.values(_data_session).length > 0) {
            if (_data_session[empID] != undefined) {
                result = _data_session[empID] ;
            }
        }
        return result;
    }
    catch {
        return null;
    }
}
