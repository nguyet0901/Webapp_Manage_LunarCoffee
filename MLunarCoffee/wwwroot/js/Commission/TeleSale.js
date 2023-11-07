function RevenueTelesale({
    DateFrom, DateTo, BranchID, MyLoad = 0, CurrentMaster = 0,
    MAXDATE = 31, LIMITLOAD = 70000,
    fnSetting_Before, fnSetting_Complete, // Callback setting
    fnMaster_Before, fnMaster_Complete, fnMaster_CountComplete, // Callback Master Load
    fnDetail_Before, fnDetail_Complete, fnDetail_CountComplete // Callback Detail Load
}) {

    RevenueTelesale.prototype.settings = this;

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
    this.IsTeleDevide = 0; // Tele (tele.js)

    //# Consult
    this.IsConDevide = 0;

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
        let settings = RevenueTelesale.prototype.settings;
        settings.LoadData_Prepare_Report();
    }

    this.LoadData_Prepare_Report = function () {
        try {
            let settings = RevenueTelesale.prototype.settings;
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
                        settings.IsTeleDevide = Number(_item.TeleSale_Devide);

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
        let settings = RevenueTelesale.prototype.settings;
        let { BranchID, DateFrom, DateTo, MyLoad, MAXDATE } = settings;
        let date = DateFrom + " to " + DateTo;
        let obj = {};
        obj.date = date;
        obj.maxdate = MAXDATE;
        obj.branch = BranchID;
        AjaxJWT(url = "/api/Commission/RevTele_LoadData"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (result != "" && result != "null" && result != "0" && MyLoad != 1) {
                    let data = JSON.parse(result);
                    if (typeof settings.RevenueMaster_Count === 'function')
                        settings.RevenueMaster_Count(data.Table, data.Table1);
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
            let settings = RevenueTelesale.prototype.settings;
            settings.DataRevMaster = [];
            let { CurrentMaster,  IsTeleFirst, IsTeleExProduct, IsTeleDevide } = settings;

            let maindt = [], realamount = 0, commisson = 0;
            let persons = 0;

            if (datacom && datacom.length > 0) {
                for (let i = 0; i < datacom.length; i++) {
                    if (datacom[i].EMP1 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP1;
                        e.commisson = datacom[i].Commission1;
                        e.realamount = datacom[i].Commission1;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP2 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP2;
                        e.commisson = datacom[i].Commission2;
                        e.realamount = datacom[i].Commission2;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP3 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP3;
                        e.commisson = datacom[i].Commission3;
                        e.realamount = datacom[i].Commission3;
                        maindt.push(e);
                    }
                }
            }
            
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let itemComm = data[i];

                    if (IsTeleFirst == 0 || (IsTeleFirst == 1 && Number(itemComm.IsFirstDay) == 1)) {
                        if (IsTeleExProduct == 0 || (IsTeleExProduct == 1 && Number(itemComm.IsProduct) == 0)) {

                            let ratio = (itemComm.Amount != 0
                                ? (itemComm.Price != 0 ? itemComm.Price : 1)
                                : 100); // Tỉ lệ theo giá nếu HH theo tiền mặt và 100% khi HH theo %

                            commisson = Math.floor(((itemComm.Amount != 0 ? (itemComm.Amount * itemComm.Quantity) : itemComm.Percent) * itemComm.Paid) / ratio);
                          
                            if(IsTeleDevide == 1){
                                persons = (itemComm.Emp != 0 ? 1 : 0) + (itemComm.Emp2 != 0 ? 1 : 0) + (itemComm.Emp3 != 0 ? 1 : 0);;
                                commisson = persons != 0 ? commisson / persons : 0;
                            }

                            realamount = Math.floor((commisson * itemComm.PerRevenue) / 100);

                            if (itemComm.Emp != 0 ) {
                                let e = {};
                                e.emp = itemComm.Emp;
                                e.realamount = realamount;
                                e.commisson = commisson;
                                maindt.push(e);
                            }

                            if (itemComm.Emp2 != 0) {
                                let e = {};
                                e.emp = itemComm.Emp2;
                                e.realamount = realamount;
                                e.commisson = commisson;
                                maindt.push(e);
                            }

                            if (itemComm.Emp3 != 0) {
                                let e = {};
                                e.emp = itemComm.Emp3;
                                e.realamount = realamount;
                                e.commisson = commisson;
                                maindt.push(e);
                            }
                            
                        }
                    }

                }

            }
            var result = [];
            let stt = 0;
            maindt.reduce(function (res, value) {
                if (!res[value.emp]) {
                    stt = stt + 1;
                    res[value.emp] = { stt: stt, emp: value.emp, realamount: 0, commisson: 0, empName: settings.RevenueDetail_EmpName(value.emp) };
                    result.push(res[value.emp])
                    settings.DataRevMaster.push(res[value.emp]);
                }
                res[value.emp].realamount += value.realamount;
                res[value.emp].commisson += value.commisson;
                return res;
            }, {});
            if (typeof settings.fnMaster_CountComplete === 'function')
                settings.fnMaster_CountComplete(result);
        })
    }

    //#endregion

    //#region // DETAIL

    this.RevenueDetail_Load = function (EmployeeID) {
        let settings = RevenueTelesale.prototype.settings;
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
        XhrRevenueLoad = AjaxJWT(url = "/api/Commission/RevTele_LoadDetail"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (typeof settings.fnDetail_Complete === 'function')
                    settings.fnDetail_Complete(result);
                if (result != "0") {
                    let datas = JSON.parse(result);
                    let data = datas.Table;
                    let datacom = datas.Table1;
                    let datamerge = data.concat(datacom);
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
            let settings = RevenueTelesale.prototype.settings;
            let { IsTeleFirst, IsTeleExProduct, CurrentMaster, IsTeleDevide } = settings;
            settings.DataRevDetail = [];
            let realamount = 0, commisson = 0;
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let itemComm = data[i];
                    let isManual = itemComm.Manual_Revenue;

                    if ((IsTeleFirst == 0 || (IsTeleFirst == 1 && Number(itemComm.IsFirstDay) == 1)) || isManual == 1) {
                        if ((IsTeleExProduct == 0 || (IsTeleExProduct == 1 && Number(itemComm.IsProduct) == 0)) || isManual == 1) {                            
                            let ratio = (itemComm.Amount != 0
                                ? (itemComm.Price != 0 ? itemComm.Price : 1)
                                : 100); // Tỉ lệ theo giá nếu HH theo tiền mặt và 100% khi HH theo %

                            commisson = Math.floor(((itemComm.Amount != 0 ? (itemComm.Amount * itemComm.Quantity) : itemComm.Percent) * itemComm.Paid) / ratio);

                            if (IsTeleDevide == 1) {
                                persons = (itemComm.Emp != 0 ? 1 : 0) + (itemComm.Emp2 != 0 ? 1 : 0) + (itemComm.Emp3 != 0 ? 1 : 0);;
                                commisson = persons != 0 ? commisson / persons : 0;
                            }

                            realamount = Math.floor((commisson * itemComm.PerRevenue) / 100);

                            if (CurrentMaster != 0) {
                                if (itemComm.Emp == CurrentMaster) settings.RevenueDetail_Push(itemComm, "Emp", "RealAmount1", realamount, commisson);
                                if (isManual == 0) {
                                    if (itemComm.Emp2 == CurrentMaster && IsTeleDevide == 1) settings.RevenueDetail_Push(itemComm, "Emp2", "RealAmount2", realamount, commisson);
                                    if (itemComm.Emp3 == CurrentMaster && IsTeleDevide == 1) settings.RevenueDetail_Push(itemComm, "Emp3", "RealAmount3", realamount, commisson);
                                }
                                else {
                                    if (itemComm.Emp2 == CurrentMaster) settings.RevenueDetail_Push(itemComm, "Emp2", "RealAmount2", realamount, commisson);
                                    if (itemComm.Emp3 == CurrentMaster) settings.RevenueDetail_Push(itemComm, "Emp3", "RealAmount3", realamount, commisson);
                                }
                            }
                            else {
                                if (itemComm.Emp != 0) settings.RevenueDetail_Push(itemComm, "Emp", "RealAmount1", realamount, commisson);
                                if (isManual == 0) {
                                    if (itemComm.Emp2 != 0) settings.RevenueDetail_Push(itemComm, "Emp2", "RealAmount2", realamount, commisson);
                                    if (itemComm.Emp3 != 0) settings.RevenueDetail_Push(itemComm, "Emp3", "RealAmount3", realamount, commisson);
                                }
                                else {
                                    if (itemComm.Emp2 != 0) settings.RevenueDetail_Push(itemComm, "Emp2", "RealAmount2", realamount, commisson);
                                    if (itemComm.Emp3 != 0) settings.RevenueDetail_Push(itemComm, "Emp3", "RealAmount3", realamount, commisson);
                                }
                            }
                        }
                    }
                }
                if (typeof settings.fnDetail_CountComplete === 'function')
                    settings.fnDetail_CountComplete(JSON.parse(JSON.stringify(settings.DataRevDetail)))

            }

            resolve();
        })
    }

    this.RevenueDetail_Push = function (item, fieldemp, fieldamount, realamount, commission) {
        try {
            let e = JSON.parse(JSON.stringify(item));
            let settings = RevenueTelesale.prototype.settings;
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
        let settings = RevenueTelesale.prototype.settings;
        let dataExport = { "stt": "STT", "empName": Outlang["Ten_nhan_vien"], "realamount": Outlang["Tong_tien"] };
        let nameFile = filename != '' ? filename : Outlang['Doanh_thu_tong'];
        await exportJsonToExcel(nameFile, settings.DataRevMaster, dataExport);
    }

    this.ExportDetail = async function (filename) {
        let settings = RevenueTelesale.prototype.settings;
        let dataExport = {
            "CustCode": Outlang["Ma_khach_hang"]
            , "CustName": Outlang["Khach_hang"]
            , "SourceID": [Outlang["Nguon"], (v) => {return Fun_GetName_ByID(RP_DataCustomerSource, v)}]
            , "DocCode": Outlang["Ma_ho_so"]
            , "CustPhone": {
                dataNamePer: 'CustPhone',
                data: [Outlang["So_dien_thoai"]]
            }
            , "CustCodeOld": Outlang["Ma_khach_hang_cu"] ? Outlang["Ma_khach_hang_cu"] : 'Mã khách hàng cũ'
            , "CustAge": [Outlang["Tuoi"] != undefined ? Outlang["Tuoi"] : 'Tuổi', (v) => {
                return (!v.includes('1900') ? Distance_Year_2Date(new Date(v), new Date()) : "")
            }]
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
            , "ServiceID": [Outlang["Dich_vu"], (v) => { return settings.RevenueDetail_ServiceName(v) }]
            , "Quantity": Outlang["So_luong"]
            , "Price": Outlang["Gia_tien"]
            , "Empname": Outlang["Ten_nhan_vien"]
            , "Empcode": Outlang["Ma_nhan_vien"] ?? "Mã nhân viên"      
            , "Commission": Outlang["Hoa_hong"]
            , "PerRevenue": Outlang["Phan_tram_doanh_thu"]
            , "RealAmount": Outlang["Doanh_thu"]
            , "Paid": Outlang["Tien_thanh_toan"]
            , "BranchName": Outlang["Chi_nhanh"]
            , "Date": [Outlang["Ngay_tao_dich_vu"], (v) => {return ConvertDateTimeUTC_DMY(v)}]
            , "DatePaid": [Outlang["Ngay_thanh_toan"], (v) => { return ConvertDateTimeUTC_DMY(v) }]
            , "Invoice": Outlang["Ma_hoa_don"]
            , "Manual_Revenue": [Outlang["Thu_cong"], (v) => { return v != 0 ? 'x' : '' }]
        };
        dataExport = Checking_TabControl_System_RebuildHeader(dataExport, tableBodyId = 'dtContentReportBody', PermissionTable_TabControl);
        let nameFile = filename != '' ? filename : Outlang["Doanh_thu"];
        await exportJsonToExcel(nameFile, settings.DataRevDetail, dataExport);
    }

    //#endregion

    //#region // OTHER

    this.RevenueDetail_ServiceName = function (id) {
        try {
            return RP_DataService[id].Name;
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

