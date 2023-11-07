function Work_Scheduler_Get_From_TokenDoc (_branchid, data_begin, data_end, _timeline, _extension, shiftdata, _emp) {

    let timeline = Work_Scheduler_GetArrayTimeLineAllEmployee(_timeline);
    let extension = Work_Scheduler_GetArrayTimeLineAllEmployee(_extension);
    let DateFrom = data_begin;
    let DateTo = data_end;
    let NumEmployee = _emp.length;
    let _result_scheduler = [];
    for (let numEmp = 0; numEmp < NumEmployee; numEmp++) {
        let EmpID = Number(_emp[numEmp]["EmpID"]);

        for (let dateindex = DateFrom; dateindex <= DateTo; dateindex = dateindex.addDays(1)) {
            let date = formatdDateTime_To_Date(new Date(dateindex.getTime()));
            let ws = [];
            let Havingextension = false;
            // Tim trong extension truoc
            for (i = 0; i < extension.length; i++) {
                if (date.getTime() == (StringYMDTODate(extension[i].Date)).getTime() && EmpID == extension[i].EmployeeID) {
                    Havingextension = true;
                    ws = extension[i].workScheduler;
                    i = extension.Length;
                }
            }
            ///////////////////////////////
            if (Havingextension) {
                if (ws.length == 1)// extension thi chi co 1 gia tri data
                {
                    let off = ws[0].off;
                    if (off) // off ngay hom do
                    {
                        let _re = {};
                        _re["EmpID"] = EmpID;
                        _re["Date_From"] = date;
                        _re["Date_To"] = date.addHours(23);
                        _re["BranchID"] = 0;
                        _re["Color"] = "";
                        _re["ShiftCode"] = "";
                        _re["IsWork"] = 0;
                        _re["Date"] = ConvertDT_To_StringYMD_Int(date);
                        _result_scheduler.push(_re);

                    }
                    else {
                        let shift = ws[0].shift;
                        for (i = 0; i < shift.length; i++) {

                            if (_branchid == 0 || shift[i].branch == _branchid) {
                                let dataTableShift = Work_Scheduler_GetShiftDetail(shiftdata, shift[i].shift);
                                if (dataTableShift != undefined && dataTableShift != "[]") {
                                    for (j = 0; j < dataTableShift.length; j++) {
                                        let dr = {};
                                        let fromHour = dataTableShift[j]["HourFrom"];
                                        let _hf = Number(fromHour.split(':')[0]);
                                        let _mf = Number(fromHour.split(':')[1]);
                                        let toHour = dataTableShift[j]["HourTo"];
                                        let _ht = Number(toHour.split(':')[0]);
                                        let _mt = Number(toHour.split(':')[1]);
                                        dr["EmpID"] = EmpID;
                                        dr["Date_From"] = (new Date(dateindex.getTime())).addHours(_hf).addMinutes(_mf);
                                        dr["Date_To"] = (new Date(dateindex.getTime())).addHours(_ht).addMinutes(_mt);
                                        dr["BranchID"] = Number(shift[i].branch);
                                        dr["Color"] = dataTableShift[j]["ColorCode"];
                                        dr["ShiftCode"] = dataTableShift[j]["Code"];
                                        dr["IsWork"] = 1;
                                        dr["Date"] = ConvertDT_To_StringYMD_Int(date);
                                        _result_scheduler.push(dr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else {

                for (i = timeline.length - 1; i >= 0; i--) {
                    if (date >= StringYMDTODate(timeline[i].Date) && EmpID == timeline[i].EmployeeID) {
                        ws = timeline[i].workScheduler;
                        i = -1;
                    }
                }
                if (ws != null) {

                    let dayofweek = Number(date.getDay());
                    let off = false;
                    let shift = [];
                    for (let i = 0; i < ws.length; i++) {
                        if (ws[i].dayofweek == dayofweek.toString()) {
                            off = ws[i].off;
                            shift = ws[i].shift;
                            i = ws.Length;
                        }
                    }

                    if (shift != [] && off == false) {
                        for (let _ii = 0; _ii < shift.length; _ii++) {
                            if (_branchid == 0 || shift[_ii].branch == _branchid || shift[_ii].branch == 0) {

                                let dataTableShift = Work_Scheduler_GetShiftDetail(shiftdata, shift[_ii].shift);
                                if (dataTableShift != undefined && dataTableShift != "[]") {
                                    for (let jj = 0; jj < dataTableShift.length; jj++) {

                                        let dr = {};
                                        let fromHour = dataTableShift[jj]["HourFrom"];
                                        let _hf = Number(fromHour.split(':')[0]);
                                        let _mf = Number(fromHour.split(':')[1]);

                                        let toHour = dataTableShift[jj]["HourTo"];
                                        let _ht = Number(toHour.split(':')[0]);
                                        let _mt = Number(toHour.split(':')[1]);
                                        dr["EmpID"] = EmpID;
                                        dr["Date_From"] = (new Date(dateindex.getTime())).addHours(_hf).addMinutes(_mf);
                                        dr["Date_To"] = (new Date(dateindex.getTime())).addHours(_ht).addMinutes(_mt);
                                        dr["BranchID"] = Number(shift[_ii].branch);
                                        dr["Color"] = dataTableShift[jj]["ColorCode"];
                                        dr["ShiftCode"] = dataTableShift[jj]["Code"];
                                        dr["IsWork"] = 1;
                                        dr["Date"] = ConvertDT_To_StringYMD_Int(date);
                                        _result_scheduler.push(dr);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

    }
    return _result_scheduler;
}
function Work_Scheduler_GetArrayTimeLineAllEmployee (_dt) {

    try {
        let works = [];
        if (_dt != undefined && _dt.length != 0) {
            for (let i = 0; i < _dt.length; i++) {
                let work = {};
                work.Date = _dt[i]["Date"];
                work.EmployeeID = _dt[i]["EmpID"];
                let _d = JSON.parse(_dt[i]["Data"]);
                let ws = [];
                for (k = 0; k < _d.length; k++) {
                    let w = {};
                    w.dayofweek = (_d[k]["dayofweek"] != null) ? _d[k]["dayofweek"] : "";
                    w.off = _d[k]["off"];
                    let _datashifts = _d[k]["shift"];
                    let _shifts = [];
                    for (z = 0; z < _datashifts.length; z++) {
                        let shi = {};
                        shi.shift = _datashifts[z]["shift"];
                        shi.branch = _datashifts[z]["branch"];
                        _shifts.push(shi);
                    }
                    w.shift = _shifts;
                    ws.push(w);
                }
                work.workScheduler = ws;
                works.push(work);

            }
            return works;
        }
        return [];
    }
    catch (ex) {
        return [];
    }
}
function Work_Scheduler_GetShiftDetail (shidtList, shiftString) {
    try {
        let DataResulf = [];
        shiftString = "," + shiftString + ",";
        for (let ii = 0; ii < shidtList.length; ii++) {
            id = shidtList[ii]["ID"];
            id = "," + id + ",";
            if (shiftString.includes(id)) {
                let dr = {};

                dr["Code"] = shidtList[ii]["Code"];
                dr["ColorCode"] = shidtList[ii]["ColorCode"];
                dr["HourFrom"] = shidtList[ii]["HourFrom"];
                dr["HourTo"] = shidtList[ii]["HourTo"];
                dr["ID"] = shidtList[ii]["ID"];
                DataResulf.push(dr);
            }
        }
        return DataResulf;
    }
    catch (ex) {
        return "[]";
    }
}

function Work_Scheduler_Get_From_Employee (_branchid, _shiftid, data_begin, data_end, _timeline, _extension, shiftdata, EmpID) {
    let timeline = Work_Scheduler_GetArrayTimeLineAllEmployee(_timeline);
    let extension = Work_Scheduler_GetArrayTimeLineAllEmployee(_extension);
    let DateFrom = data_begin;
    let DateTo = data_end;
    let _result_scheduler = [];
    for (let dateindex = DateFrom; dateindex <= DateTo; dateindex = dateindex.addDays(1)) {
        let date = formatdDateTime_To_Date(new Date(dateindex.getTime()));
        let ws = [];
        let Havingextension = false;
        // Tim trong extension truoc
        for (i = 0; i < extension.length; i++) {
            if (date.getTime() == (StringYMDTODate(extension[i].Date)).getTime()) {
                Havingextension = true;
                ws = extension[i].workScheduler;
                i = extension.Length;
            }
        }
        ///////////////////////////////
        if (Havingextension) {
            if (ws.length == 1)// extension thi chi co 1 gia tri data
            {
                let off = ws[0].off;
                if (off) // off ngay hom do
                {
                    let _re = {};
                    _re["EmpID"] = EmpID;
                    _re["Date_From"] = date;
                    _re["Date_To"] = date.addHours(23);
                    _re["BranchID"] = 0;
                    _re["Color"] = "";
                    _re["IsWork"] = 0;
                    _re["Date"] = ConvertDT_To_StringYMD_Int(date);
                    _re["BranchID"] = 0;
                    _result_scheduler.push(_re);

                }
                else {
                    let shift = ws[0].shift;
                    for (i = 0; i < shift.length; i++) {

                        if (_branchid == 0 || shift[i].branch == _branchid) {
                            let dataTableShift = Work_Scheduler_GetShiftDetail(shiftdata, shift[i].shift);
                            if (dataTableShift != undefined && dataTableShift != "[]") {
                                for (j = 0; j < dataTableShift.length; j++) {
                                    if (_shiftid == 0 || dataTableShift[j]["ID"] == _shiftid) {
                                        let dr = {};
                                        let fromHour = dataTableShift[j]["HourFrom"];
                                        let _hf = Number(fromHour.split(':')[0]);
                                        let _mf = Number(fromHour.split(':')[1]);
                                        let toHour = dataTableShift[j]["HourTo"];
                                        let _ht = Number(toHour.split(':')[0]);
                                        let _mt = Number(toHour.split(':')[1]);
                                        dr["EmpID"] = EmpID;
                                        dr["Date_From"] = (new Date(dateindex.getTime())).addHours(_hf).addMinutes(_mf);
                                        dr["Date_To"] = (new Date(dateindex.getTime())).addHours(_ht).addMinutes(_mt);
                                        dr["Color"] = dataTableShift[j]["ColorCode"];
                                        dr["IsWork"] = 1;
                                        dr["Date"] = ConvertDT_To_StringYMD_Int(date);
                                        _result_scheduler.push(dr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else {

            for (i = timeline.length - 1; i >= 0; i--) {
                if (date >= StringYMDTODate(timeline[i].Date)) {
                    ws = timeline[i].workScheduler;
                    i = -1;
                }
            }
            if (ws != null) {

                let dayofweek = Number(date.getDay());
                let off = false;
                let shift = [];
                for (let i = 0; i < ws.length; i++) {
                    if (ws[i].dayofweek == dayofweek.toString()) {
                        off = ws[i].off;
                        shift = ws[i].shift;
                        i = ws.Length;
                    }
                }

                if (shift != [] && off == false) {
                    for (let _ii = 0; _ii < shift.length; _ii++) {
                        if (_branchid == 0 || shift[_ii].branch == _branchid || shift[_ii].branch == 0) {

                            let dataTableShift = Work_Scheduler_GetShiftDetail(shiftdata, shift[_ii].shift);
                            if (dataTableShift != undefined && dataTableShift != "[]") {
                                for (let jj = 0; jj < dataTableShift.length; jj++) {
                                    if (_shiftid == 0 || dataTableShift[jj]["ID"] == _shiftid) {
                                        let dr = {};
                                        let fromHour = dataTableShift[jj]["HourFrom"];
                                        let _hf = Number(fromHour.split(':')[0]);
                                        let _mf = Number(fromHour.split(':')[1]);

                                        let toHour = dataTableShift[jj]["HourTo"];
                                        let _ht = Number(toHour.split(':')[0]);
                                        let _mt = Number(toHour.split(':')[1]);
                                        dr["EmpID"] = EmpID;
                                        dr["Date_From"] = (new Date(dateindex.getTime())).addHours(_hf).addMinutes(_mf);
                                        dr["Date_To"] = (new Date(dateindex.getTime())).addHours(_ht).addMinutes(_mt);
                                        dr["Color"] = dataTableShift[jj]["ColorCode"];
                                        dr["IsWork"] = 1;
                                        dr["Date"] = ConvertDT_To_StringYMD_Int(date);
                                        dr["BranchID"] = shift[_ii].branch;
                                        _result_scheduler.push(dr);
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    //}
    return _result_scheduler;
}

