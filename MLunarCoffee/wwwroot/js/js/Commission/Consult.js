﻿
function RevenueConsult({
    DateFrom, DateTo, BranchID, MyLoad = 0, CurrentMaster = 0,
    MAXDATE = 31, LIMITLOAD = 70000,
    fnSetting_Before, fnSetting_Complete, // Callback setting
    fnMaster_Before, fnMaster_Complete, fnMaster_CountComplete, // Callback Master Load
    fnDetail_Before, fnDetail_Complete, fnDetail_CountComplete // Callback Detail Load
}) {

    RevenueConsult.prototype.settings = this;

    //#region //

    this.DateFrom = DateFrom;
    this.DateTo = DateTo;
    this.BranchID = BranchID;
    this.MyLoad = MyLoad;
    this.CurrentMaster = CurrentMaster;

    this.MAXDATE = MAXDATE;
    this.LIMITLOAD = LIMITLOAD;

    this.XhrRevenueLoad = null;

    //#endregion

    //#region // REVENUE SETTINGS

    //# Doctor
    this.IsDocDevide = 0;
    this.IsDocGreater = 0;
    this.IsDocTreatDevide = 0; // THEO ĐIỀU TRỊ (DoctorByTreat.js)

    //# Assistant
    this.IsAssDevide = 0;
    this.IsAssGreater = 0;
    this.IsAssTreatDevide = 0; // THEO ĐIỀU TRỊ (AssistantByTreat.js)

    //# Tele
    this.IsTeleFirst = 0;
    this.IsTeleExProduct = 0; // Tele (tele.js)

    //# Consult
    this.IsConDevide = 0;
    this.IsConEquaDivided = 1;

    //# CSKH
    this.IsCSKHExFirst = 0;

    //# Support
    this.IsSuppDevide = 0;

    //#endregion

    //#region // DATA MASTER & DETAIL

    this.DataRevMaster = [];

    this.DataRevDetail = [];

    //#endregion

    //#region // CALL BACK

    this.fnSetting_Before = fnSetting_Before;
    this.fnSetting_Complete = fnSetting_Complete;

    this.fnMaster_Before = fnMaster_Before;
    this.fnMaster_Complete = fnMaster_Complete;
    this.fnMaster_CountComplete = fnMaster_CountComplete;

    this.fnDetail_Before = fnDetail_Before;
    this.fnDetail_Complete = fnDetail_Complete;
    this.fnDetail_CountComplete = fnDetail_CountComplete;

    //#endregion

    //#region // INIT

    this.Init = function () {
        let settings = RevenueConsult.prototype.settings;
        settings.LoadData_Prepare_Report();
    }

    this.LoadData_Prepare_Report = function () {
        try {
            let settings = RevenueConsult.prototype.settings;
            AjaxJWT(url = "/api/Commission/RevenueSettings"
                , data = {}
                , async = true
                , success = function (result) {
                    let _data = [];
                    if (result != "" && result != "null") {
                        _data = JSON.parse(result);
                        let _item = _data[0];
                        settings.IsDocDevide = Number(_item.Doc_Devide);
                        settings.IsDocGreater = Number(_item.Doc_Greater);
                        settings.IsDocTreatDevide = Number(_item.Docnomoney_Devide);

                        settings.IsAssDevide = Number(_item.KTVPTM_Devide);
                        settings.IsAssGreater = Number(_item.KTVPTM_Greater);

                        settings.IsTeleFirst = Number(_item.TeleSale_First);
                        settings.IsTeleExProduct = Number(_item.TeleSale_ExProduct);

                        settings.IsConDevide = Number(_item.Consult_Devide);

                        settings.IsCSKHExFirst = Number(_item.Cskh_Exfirst);

                        settings.IsSuppDevide = Number(_item.Support_Devide);
                    }
                    if (typeof settings.fnSetting_Complete === 'function')
                        settings.fnSetting_Complete(result, _data);
                    settings.LoadData_Report();
                }
                , before = function (e) {
                    if (typeof settings.fnSetting_Before === 'function') settings.fnSetting_Before();
                }
            );
        }
        catch (ex) {

        }
    }

    //#endregion

    //#region // MASTER

    this.LoadData_Report = function () {
        let settings = RevenueConsult.prototype.settings;
        let { BranchID, DateFrom, DateTo, MyLoad, MAXDATE } = settings;
        let date = DateFrom + " to " + DateTo;
        let obj = {};
        obj.date = date;
        obj.maxdate = MAXDATE;
        obj.branch = BranchID;
        AjaxJWT(url = "/api/Commission/RevConsult_LoadData"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (result != "" && result != "null" && result != "0" && MyLoad != 1) {
                    let data = JSON.parse(result);
                    let datamain = (data.Table).concat(data.Table2)
                    let datacom = (data.Table1).concat(data.Table3)
                    if (typeof settings.RevenueMaster_Count === 'function')
                        settings.RevenueMaster_Count(datamain, datacom);
                }
                if (typeof settings.fnMaster_Complete === 'function')
                    settings.fnMaster_Complete(result);
            }
            , before = function (e) {
                if (typeof settings.fnMaster_Before === 'function')
                    settings.fnMaster_Before();
            }
        );
        return false;

    }

    this.RevenueMaster_Count = async function (data, datacom) {
        new Promise((resolve, reject) => {
            let settings = RevenueConsult.prototype.settings;
            settings.DataRevMaster = [];
            let { IsConDevide } = settings;

            let maindt = [], realamount = 0, perjson = 0, RePercent = 0;
            if (datacom && datacom.length > 0) {
                for (let i = 0; i < datacom.length; i++) {
                    if (datacom[i].EMP1 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP1;
                        e.realamount = datacom[i].Commission1;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP2 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP2;
                        e.realamount = datacom[i].Commission2;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP3 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP3;
                        e.realamount = datacom[i].Commission3;
                        maindt.push(e);
                    }
                }
            }
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {

                    realamount = data[i].Percent != 0 ? ((data[i].Percent * data[i].Paid) / 100) : data[i].Amount;
                    RePercent = Number(data[i].RePercent);
                    RePercent = (RePercent > 0 && RePercent <= 100) ? RePercent : 100;
                    realamount = realamount * RePercent / 100;
                    if (IsConDevide == 1) {
                        perjson = (data[i].Emp1 != 0 ? 1 : 0) + (data[i].Emp2 != 0 ? 1 : 0) + (data[i].Emp3 != 0 ? 1 : 0) + (data[i].Emp4 != 0 ? 1 : 0);
                        realamount = perjson != 0 ? realamount / perjson : 0;
                    }
                    if (data[i].Emp1 != 0) {
                        let e = {};
                        e.emp = data[i].Emp1;
                        e.realamount = realamount;
                        maindt.push(e);
                    }
                    if (data[i].Emp2 != 0 && IsConDevide == 1) {
                        let e = {};
                        e.emp = data[i].Emp2;
                        e.realamount = realamount;
                        maindt.push(e);
                    }
                    if (data[i].Emp3 != 0 && IsConDevide == 1) {
                        let e = {};
                        e.emp = data[i].Emp3;
                        e.realamount = realamount;
                        maindt.push(e);
                    }
                    if (data[i].Emp4 != 0 && IsConDevide == 1) {
                        let e = {};
                        e.emp = data[i].Emp4;
                        e.realamount = realamount;
                        maindt.push(e);
                    }
                }

            }
            var result = [];
            let stt = 0;
            let totalAmount = 0;
            maindt.reduce(function (res, value) {
                if (!res[value.emp]) {
                    stt = stt + 1;
                    res[value.emp] = { stt: stt, emp: value.emp, realamount: 0, empName: settings.RevenueDetail_EmpName(value.emp) };
                    result.push(res[value.emp])
                    settings.DataRevMaster.push(res[value.emp]);
                }
                res[value.emp].realamount += value.realamount;
                totalAmount = totalAmount + value.realamount;
                return res;
            }, {});

            if (typeof settings.fnMaster_CountComplete === 'function')
                settings.fnMaster_CountComplete(result);
        })
    }

    //#endregion

    //#region // DETAIL

    this.RevenueDetail_Load = function (EmployeeID) {
        let settings = RevenueConsult.prototype.settings;
        settings.CurrentMaster = EmployeeID;
        let { BranchID, DateFrom, DateTo, MAXDATE, LIMITLOAD, XhrRevenueLoad } = settings;
        if (XhrRevenueLoad && XhrRevenueLoad.readyState != 4) XhrRevenueLoad.abort();
        let date = DateFrom + " to " + DateTo;
        let obj = {};
        obj.date = date;
        obj.maxdate = MAXDATE;
        obj.limit = LIMITLOAD;
        obj.empid = settings.CurrentMaster;
        obj.branch = BranchID;
        XhrRevenueLoad = AjaxJWT(url = "/api/Commission/RevConsult_LoadDetail"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (typeof settings.fnDetail_Complete === 'function')
                    settings.fnDetail_Complete(result);
                if (result != "0") {
                    let data = JSON.parse(result);
                    let datatreat = data.Table;
                    let datamerge = datatreat.concat(data.Table1);
                    datamerge = datamerge.concat(data.Table2);
                    datamerge = datamerge.concat(data.Table3);
                    settings.RevenueDetail_Count(datamerge);

                }
            }
            , before = function (e) {
                if (typeof settings.fnDetail_Before === 'function')
                    settings.fnDetail_Before();

            }
        );
        return false;
    }

    this.RevenueDetail_Count = async function (data) {
        new Promise((resolve, reject) => {
            let settings = RevenueConsult.prototype.settings;
            let { IsConDevide, CurrentMaster } = settings;
            settings.DataRevDetail = [];

            let realamount = 0, perjson = 0, RePercent = 0, commission = 0;
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {

                    commission = data[i].Percent != 0 ? ((data[i].Percent * data[i].Paid) / 100) : data[i].Amount;

                    RePercent = Number(data[i].RePercent);
                    RePercent = (RePercent > 0 && RePercent <= 100) ? RePercent : 100;
                    if (IsConDevide == 1) {
                        perjson = (data[i].Emp1 != 0 ? 1 : 0) + (data[i].Emp2 != 0 ? 1 : 0) + (data[i].Emp3 != 0 ? 1 : 0) + (data[i].Emp4 != 0 ? 1 : 0);
                        commission = perjson != 0 ? commission / perjson : 0;
                    }
                    realamount = commission * RePercent / 100;

                    if (CurrentMaster != 0) {
                        if (data[i].Manual_Revenue == 0) {
                            if (data[i].Emp1 == CurrentMaster) settings.RevenueDetail_Push(data[i], "Emp1", "RealAmount1", commission, realamount);
                            if (data[i].Emp2 == CurrentMaster && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp2", "RealAmount2", commission, realamount);
                            if (data[i].Emp3 == CurrentMaster && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp3", "RealAmount3", commission, realamount);
                            if (data[i].Emp4 == CurrentMaster && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp4", "RealAmount4", commission, realamount);
                        }
                        else {
                            if (data[i].Emp1 == CurrentMaster) settings.RevenueDetail_Push(data[i], "Emp1", "RealAmount1", commission, realamount);
                            if (data[i].Emp2 == CurrentMaster) settings.RevenueDetail_Push(data[i], "Emp2", "RealAmount2", commission, realamount);
                            if (data[i].Emp3 == CurrentMaster) settings.RevenueDetail_Push(data[i], "Emp3", "RealAmount3", commission, realamount);
                            if (data[i].Emp4 == CurrentMaster) settings.RevenueDetail_Push(data[i], "Emp4", "RealAmount4", commission, realamount);
                        }
                    }
                    else {
                        if (data[i].Manual_Revenue == 0) {
                            if (data[i].Emp1 != 0) settings.RevenueDetail_Push(data[i], "Emp1", "RealAmount1", commission, realamount);
                            if (data[i].Emp2 != 0 && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp2", "RealAmount2", commission, realamount);
                            if (data[i].Emp3 != 0 && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp3", "RealAmount3", commission, realamount);
                            if (data[i].Emp4 != 0 && IsConDevide == 1) settings.RevenueDetail_Push(data[i], "Emp4", "RealAmount4", commission, realamount);
                        }
                        else {
                            if (data[i].Emp1 != 0) settings.RevenueDetail_Push(data[i], "Emp1", "RealAmount1", commission, realamount);
                            if (data[i].Emp2 != 0) settings.RevenueDetail_Push(data[i], "Emp2", "RealAmount2", commission, realamount);
                            if (data[i].Emp3 != 0) settings.RevenueDetail_Push(data[i], "Emp3", "RealAmount3", commission, realamount);
                            if (data[i].Emp4 != 0) settings.RevenueDetail_Push(data[i], "Emp4", "RealAmount4", commission, realamount);
                        }
                    }

                }

                if (typeof settings.fnDetail_CountComplete === 'function')
                    settings.fnDetail_CountComplete(JSON.parse(JSON.stringify(settings.DataRevDetail)))

            }

            resolve();
        })
    }

    this.RevenueDetail_Push = function (item, fieldemp, fieldamount, commission, realamount) {
        try {
            let e = JSON.parse(JSON.stringify(item));
            let settings = RevenueConsult.prototype.settings;
            e.RealAmount = (item.Manual_Revenue == 0 ? realamount : item[fieldamount]);
            e.Commission = (item.Manual_Revenue == 0 ? commission : item[fieldamount]);
            e.Emp = item[fieldemp];
            e.Empname = settings.RevenueDetail_EmpName(item[fieldemp]);
            e.Empcode = settings.RevenueDetail_EmpCode(item[fieldemp]);
            settings.DataRevDetail.push(e);
        }
        catch (ex) {

        }
    }

    //#endregion

    //#region // EXPORT

    this.ExportMaster = async function (filename) {
        let settings = RevenueConsult.prototype.settings;
        let dataExport = { "stt": "STT", "empName": Outlang["Ten_nhan_vien"], "realamount": Outlang["Tong_tien"] };
        let nameFile = filename != '' ? filename : Outlang['Doanh_thu_tong'];
        await exportJsonToExcel(nameFile, settings.DataRevMaster, dataExport);
    }

    this.ExportDetail = async function (filename) {
        let settings = RevenueConsult.prototype.settings;

        let dataExport = {
            "CustCode": Outlang["Ma_khach_hang"]
            , "CustName": Outlang["Khach_hang"]
            , "SourceID": [Outlang["Nguon"], (v) => {return Fun_GetName_ByID(RP_DataCustomerSource, v)}]
            , "DocCode": Outlang["Ma_ho_so"]
            , "CustPhone": {
                dataNamePer: 'CustPhone',
                data: [Outlang["So_dien_thoai"]]
            }
            , "CustCodeOld": Outlang["Ma_khach_hang_cu"] != undefined ? Outlang["Ma_khach_hang_cu"] : 'Mã khách hàng cũ'
            , "CustAge": Outlang["Tuoi"] != undefined ? Outlang["Tuoi"] : 'Tuổi'
            , "CustGender": [Outlang["Gioi_tinh"], (v, { CustGender }) => { return (CustGender == 60 ? Outlang["Sys_nam"] : Outlang["Sys_nu1"]) }]
            , "CustAddress": {
                dataNamePer: 'CustAddress',
                data: [Outlang["Sys_dia_chi_khach_hang1"]]
            }
            , "CustCommuneID": {
                dataNamePer: 'CustCommu',
                data: [Outlang["Phuong_xa"] ?? 'Phường xã', (v) => {return (Fun_GetName_ByID(RP_DataCommune, v))}]
            }
            , "CustDistrictID": {
                dataNamePer: 'CustDistrict',
                data: [Outlang["Quan_huyen"] ?? 'Quận huyện', (v) => {return (Fun_GetName_ByID(RP_DataDistrict, v))}]
            }
            , "CustCityID": {
                dataNamePer: 'CustCity',
                data: [Outlang["Sys_thanh_pho"] ?? 'Thành phố', (v) => {return (Fun_GetName_ByID(RP_DataCity, v))}]
            }
            , "SerCode": Outlang["Ma_dich_vu"]
            , "ServiceID": [Outlang["Dich_vu"], (v) => {return settings.RevenueDetail_ServiceName(0, v)}]
            , "Card": [Outlang["The"], (v) => {return settings.RevenueDetail_ServiceName(v, 0)}]
            , "SaleCode": Outlang["Ma_len_don"]
            , "Quantity": Outlang["So_luong"]
            , "Price": Outlang["Gia_tien"]
            , "Empname": Outlang["Nhan_vien"]
            , "Empcode": Outlang["Ma_nhan_vien"] ?? "Mã nhân viên"
            , "Commission": Outlang["Hoa_hong"]
            , "RePercent": ["% " + Outlang["Doanh_thu"], (v) => {return (v != 0 ? v : 100)}]
            , "RealAmount": Outlang["Doanh_thu"]
            , "ServiceCatID": [Outlang["Nhom_dich_vu"], (v) => {return Fun_GetName_ByID(RP_DataServiceCat, v)}]
            , "Paid": Outlang["Tien_thanh_toan"]
            , "Date": [Outlang["Ngay_tao_dich_vu"], (v) => {return ConvertDateTimeUTC_DMY(v)}]
            , "DatePaid": [Outlang["Ngay_thanh_toan"], (v) => {return ConvertDateTimeUTC_DMY(v)}]
            , "BranchName": Outlang["Chi_nhanh"]
            , "Invoice": Outlang["Ma_hoa_don"]
            , "IsProduct": [Outlang["Loai_dich_vu"] ?? 'Loại dịch vụ', (v) => {return settings.RevenueDetail_ServiceType(v)}]
            , "Note": [Outlang["Ghi_chu"]]
            , "Manual_Revenue": [Outlang["Thu_cong"], (v) => { return v != 0 ? 'x' : '' }]
            
        };
        dataExport = Checking_TabControl_System_RebuildHeader(dataExport, tableBodyId = 'dtContentReportBody', PermissionTable_TabControl);
        let nameFile = filename != '' ? filename : Outlang["Doanh_thu"];
        await exportJsonToExcel(nameFile, settings.DataRevDetail, dataExport);
    }

    //#endregion

    //#region // OTHER

    this.RevenueDetail_ServiceName = function (card, service) {
        try {
            if (card != 0) return RP_DataCard[card].Name;
            else return RP_DataService[service].Name;
        }
        catch (ex) {
            return "";
        }
    }
    this.RevenueDetail_ServiceType = function (type) {debugger
        try {
            let result = '';
            switch (type) {
                case 0: result = Outlang["Dich_vu"] ?? 'Dịch vụ'
                    break;
                case 1: result = Outlang["San_pham"] ?? 'Sản phẩm'
                    break;
                case 2: result = Outlang["The"] ?? 'Thẻ'
                    break;
            }
            return result;
        }
        catch (ex) {
            return "";
        }
    }

    this.RevenueDetail_EmpName = function (id) {
        try {

            return RP_DataEmployee[id] != undefined ? RP_DataEmployee[id].Name : '';
        }
        catch (ex) {
            return "";
        }
    }
    this.RevenueDetail_EmpCode = function (id) {
        try {

            return RP_DataEmployee[id] != undefined ? RP_DataEmployee[id].Code : '';
        }
        catch (ex) {
            return "";
        }
    }
    //#endregion

}
