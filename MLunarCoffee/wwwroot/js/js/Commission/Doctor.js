
function RevenueDoctor({
    DateFrom, DateTo, BranchID, MyLoad = 0, CurrentMaster = 0,
    MAXDATE = 31, LIMITLOAD = 70000,
    fnSetting_Before, fnSetting_Complete, // Callback setting
    fnMaster_Before, fnMaster_Complete, fnMaster_CountComplete, // Callback Master Load
    fnDetail_Before, fnDetail_Complete, fnDetail_CountComplete // Callback Detail Load
}) {

    RevenueDoctor.prototype.settings = this;

    //#region //

    this.DateFrom = DateFrom;
    this.DateTo = DateTo;
    this.BranchID = BranchID;
    this.MyLoad = MyLoad;
    this.CurrentMaster = 0;

    this.MAXDATE = MAXDATE;
    this.LIMITLOAD = LIMITLOAD;

    this.XhrRevenueLoad = null;

    //#endregion

    //#region // REVENUE SETTINGS

    //# Doctor
    this.IsDocDevide = 0;
    this.IsDocGreater = 0;
    this.IsDocExCost = 0;

    this.IsDocTreatDevide = 0; // THEO ĐIỀU TRỊ (DoctorByTreat.js)
    this.IsDocTreatExCost = 0;

    //# Assistant
    this.IsAssDevide = 0;
    this.IsAssGreater = 0;
    this.IsAssTreatDevide = 0; // THEO ĐIỀU TRỊ (AssistantByTreat.js)

    //# Tele
    this.IsTeleFirst = 0;
    this.IsTeleExProduct = 0; // Tele (tele.js)

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
        let settings = RevenueDoctor.prototype.settings;
        settings.LoadData_Prepare_Report();
    }

    this.LoadData_Prepare_Report = function () {
        try {
            let settings = RevenueDoctor.prototype.settings;
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
                        settings.IsDocExCost = (Number(_item.Doc_ExCost) > 0 ? 1 : 0);

                        settings.IsDocTreatDevide = Number(_item.Docnomoney_Devide);
                        settings.IsDocTreatExCost = (Number(_item.DocTreat_ExCost) > 0 ? 1 : 0);

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
        let settings = RevenueDoctor.prototype.settings;
        let { BranchID, DateFrom, DateTo, MyLoad, MAXDATE } = settings;
        let date = DateFrom + " to " + DateTo;
        let obj = {};
        obj.date = date;
        obj.maxdate = MAXDATE;
        obj.branch = BranchID;
        AjaxJWT(url = "/api/Commission/RevDoctor_LoadData"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (result != "" && result != "null" && result != "0" && MyLoad != 1) {
                    let data = JSON.parse(result);
                    if (typeof settings.RevenueMaster_Count === 'function')
                        settings.RevenueMaster_Count(data.Table, data.Table1, data.Table2);
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

    this.RevenueMaster_Count = async function (data, datacom, datatotal) {
        new Promise((resolve, reject) => {
            let settings = RevenueDoctor.prototype.settings;
            settings.DataRevMaster = [];
            let { IsDocGreater, IsDocDevide, IsDocExCost } = settings;
            let maindt = [], realamount = 0, perjson = 0;
            if (datacom && datacom.length > 0) {
                for (let i = 0; i < datacom.length; i++) {
                    if (datacom[i].EMP1 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP1;
                        e.realamount = datacom[i].Commission1;
                        e.totalPaid = datacom[i].TotalPaid;
                        e.tabid = datacom[i].TabID;
                        e.paymentPeriod = datacom[i].PaymentPeriod;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP2 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP2;
                        e.realamount = datacom[i].Commission2;
                        e.totalPaid = datacom[i].TotalPaid;
                        e.tabid = datacom[i].TabID;
                        e.paymentPeriod = datacom[i].PaymentPeriod;
                        maindt.push(e);
                    }
                    if (datacom[i].EMP3 != 0) {
                        let e = {};
                        e.emp = datacom[i].EMP3;
                        e.realamount = datacom[i].Commission3;
                        e.totalPaid = datacom[i].TotalPaid;
                        e.tabid = datacom[i].TabID;
                        e.paymentPeriod = datacom[i].PaymentPeriod;
                        maindt.push(e);
                    }
                }
            }
            if (data && data.length > 0) {
                let tabindex = 0;
                let amountindex = 0
                for (let i = 0; i < data.length; i++) {
                    let newpercent = (sys_dencos_Main == 1) ? data[i].NewPer : (data[i].TimeTreat != 0
                        //? (100 * data[i].TimeTreatIndex / data[i].TimeTreat)
                        ? (100 * 1 / data[i].TimeTreat)
                        : 0);

                    if (IsDocExCost == 1) { // Kiểm tra trừ chi phí labo
                        data[i].Price -= data[i].PriceLabo;
                    }
                    let amountequalper = newpercent != 0 ? (data[i].Price * newpercent / 100) : 0;
                    let revenue = data[i].Amount != 0 ? data[i].Amount : Number((amountequalper * data[i].Percent) / 100);
                    if (data[i].TabID != tabindex) {
                        tabindex = data[i].TabID;
                        amountindex = Number(data[i].TotalPaid) - Number(data[i].TotalCurrentPaid);
                    }
                    if (amountindex > 0) {
                        if (Number(data[i].TreatInDate) == 0) {
                            data[i].OldRealAmount = 0;
                            if (amountindex >= amountequalper) data[i].OldRealAmount = revenue;
                            else if (amountindex > 0) {
                                if (IsDocGreater == 0) data[i].OldRealAmount = (amountindex < revenue ? amountindex : revenue);
                                else data[i].OldRealAmount = 0;
                            }
                            else data[i].OldRealAmount = 0;
                        }
                        amountindex = amountindex != 0 ? (amountindex - amountequalper) : 0;
                    }
                    else data[i].OldRealAmount = 0;
                }
                tabindex = 0;
                amountindex = 0;
                for (let i = 0; i < data.length; i++) {
                    let newpercent = (sys_dencos_Main == 1) ? data[i].NewPer : (data[i].TimeTreat != 0
                        //? (100 * data[i].TimeTreatIndex / data[i].TimeTreat)
                        ? (100 * 1 / data[i].TimeTreat)
                        : 0);


                    let amountequalper = newpercent != 0 ? (data[i].Price * newpercent / 100) : 0;
                    let revenue = data[i].Amount != 0 ? data[i].Amount : Number((amountequalper * data[i].Percent) / 100);
                    if (data[i].TabID != tabindex) {
                        tabindex = data[i].TabID;
                        amountindex = Number(data[i].TotalPaid);
                    }
                    if (amountindex > 0) {
                        if (amountindex >= amountequalper) data[i].RealAmount = revenue;
                        else if (amountindex > 0) {
                            if (IsDocGreater == 0) data[i].RealAmount = (amountindex < revenue ? amountindex : revenue);
                            else data[i].RealAmount = 0;
                        }

                        else data[i].RealAmount = 0;
                        if (Number(data[i].TreatInDate) == 1 && data[i].RealAmount != 0) {
                            data[i].Havingrevenue = 1;
                        }
                        if (Number(data[i].TreatInDate) == 0 && data[i].RealAmount != data[i].OldRealAmount) {
                            data[i].Havingrevenue = 1;
                            data[i].RealAmount = data[i].RealAmount - data[i].OldRealAmount;
                        }
                        amountindex = amountindex != 0 ? (amountindex - amountequalper) : 0;
                    }
                    else {
                        data[i].Havingrevenue = 0;
                        data[i].RealAmount = 0;
                    }

                }
                for (let i = 0; i < data.length; i++) {
                    if (Number(data[i].Havingrevenue) == 1)  {
                        let RePercent = Number(data[i].RePercent);
                        RePercent = (RePercent > 0 && RePercent <= 100) ? RePercent : 100;
                        realamount = data[i].RealAmount * RePercent / 100;

                        if (IsDocDevide == 1) {
                            perjson = (data[i].BS1 != 0 ? 1 : 0) + (data[i].BS2 != 0 ? 1 : 0) + (data[i].BS3 != 0 ? 1 : 0) + (data[i].BS4 != 0 ? 1 : 0);
                            realamount = perjson != 0 ? realamount / perjson : 0;
                        }
                        if (data[i].BS1 != 0) {
                            let e = {};
                            e.emp = data[i].BS1;
                            e.realamount = realamount;
                            e.totalPaid = data[i].TotalPaid;
                            e.tabid = data[i].TabID;
                            e.paymentPeriod = data[i].PaymentPeriod;
                            maindt.push(e);
                        }
                        if (data[i].BS2 != 0 && IsDocDevide == 1) {
                            let e = {};
                            e.emp = data[i].BS2;
                            e.realamount = realamount;
                            e.totalPaid = data[i].TotalPaid;
                            e.tabid = data[i].TabID;
                            e.paymentPeriod = data[i].PaymentPeriod;
                            maindt.push(e);
                        }
                        if (data[i].BS3 != 0 && IsDocDevide == 1) {
                            let e = {};
                            e.emp = data[i].BS3;
                            e.realamount = realamount;
                            e.totalPaid = data[i].TotalPaid;
                            e.tabid = data[i].TabID;
                            e.paymentPeriod = data[i].PaymentPeriod;
                            maindt.push(e);
                        }
                        if (data[i].BS4 != 0 && IsDocDevide == 1) {
                            let e = {};
                            e.emp = data[i].BS4;
                            e.realamount = realamount;
                            e.totalPaid = data[i].TotalPaid;
                            e.tabid = data[i].TabID;
                            e.paymentPeriod = data[i].PaymentPeriod;
                            maindt.push(e);
                        }
                    }
                }

            }
            var result = [];
            let stt = 0;
            let dttab = {};
            maindt.reduce(function (res, value) {  
                if (!res[value.emp]) {
                    stt = stt + 1;
                    res[value.emp] = { stt: stt, emp: value.emp, realamount: 0, empName: settings.RevenueDetail_EmpName(value.emp),  totalPaid: 0, paymentPeriod: 0 };
                    result.push(res[value.emp]);
                    settings.DataRevMaster.push(res[value.emp])
                }
                res[value.emp].realamount += value.realamount;
                if (dttab?.[value.emp]?.[value.tabid] != 1) {
                    res[value.emp].totalPaid += value.totalPaid;
                    res[value.emp].paymentPeriod += value.paymentPeriod;    
                    if (dttab[value.emp]) {
                        dttab[value.emp][value.tabid] = 1;
                    }
                    else {
                        dttab[value.emp] = {};
                        dttab[value.emp][value.tabid] = 1;
                    }
                }
                return res;

            }, {});
            if (typeof settings.fnMaster_CountComplete === 'function') {
                settings.fnMaster_CountComplete(result, datatotal);
            }
        })
    }

    //#endregion

    //#region // DETAIL

    this.RevenueDetail_Load = function (EmployeeID) {
        let settings = RevenueDoctor.prototype.settings;
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
        XhrRevenueLoad = AjaxJWT(url = "/api/Commission/RevDoctor_LoadDetail"
            , data = JSON.stringify(obj)
            , async = true
            , success = function (result) {
                if (typeof settings.fnDetail_Complete === 'function')
                    settings.fnDetail_Complete(result);
                if (result != "0") {
                    let data = JSON.parse(result);
                    let datatreat = data.Table;
                    let datacom = data.Table1;
                    let datamerge = datatreat.concat(datacom);
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
            let settings = RevenueDoctor.prototype.settings;
            let { IsDocDevide, CurrentMaster, IsDocGreater, IsDocExCost } = settings;
            settings.DataRevDetail = [];
            let realamount = 0, perjson = 0, commission = 0;
            if (data && data.length > 0) {
                let tabindex = 0;
                let amountindex = 0
                for (let i = 0; i < data.length; i++) {
                    let newpercent = (sys_dencos_Main == 1) ? data[i].NewPer : (data[i].TimeTreat != 0
                        ? (100 * 1 / data[i].TimeTreat)
                        : 0);
                    if (IsDocExCost == 1) { // Kiểm tra trừ chi phí labo
                        data[i].Price -= data[i].PriceLabo;
                    }
                    let amountequalper = newpercent != 0 ? (data[i].Price * newpercent / 100) : 0;
                    let revenue = data[i].Amount != 0 ? data[i].Amount : Number((amountequalper * data[i].Percent) / 100);
                    if (data[i].TabID != tabindex) {
                        tabindex = data[i].TabID;
                        amountindex = Number(data[i].TotalPaid) - Number(data[i].TotalCurrentPaid);
                    }
                    if (amountindex > 0) {
                        if (Number(data[i].TreatInDate) == 0) {
                            data[i].OldRealAmount = 0;
                            if (amountindex >= amountequalper) data[i].OldRealAmount = revenue;
                            else if (amountindex > 0) {
                                if (IsDocGreater == 0) data[i].OldRealAmount = (amountindex < revenue ? amountindex : revenue);
                                else data[i].OldRealAmount = 0;
                            }
                            else data[i].OldRealAmount = 0;
                        }
                        amountindex = amountindex != 0 ? (amountindex - amountequalper) : 0;
                    }
                    else data[i].OldRealAmount = 0;
                }
                tabindex = 0;
                amountindex = 0;

                for (let i = 0; i < data.length; i++) {
                    let newpercent = (sys_dencos_Main == 1) ? data[i].NewPer : (data[i].TimeTreat != 0
                        //? (100 * data[i].TimeTreatIndex / data[i].TimeTreat)
                        ? (100 * 1 / data[i].TimeTreat)
                        : 0);
                    let amountequalper = newpercent != 0 ? (data[i].Price * newpercent / 100) : 0;
                    let revenue = data[i].Amount != 0 ? data[i].Amount : Number((amountequalper * data[i].Percent) / 100);
                    if (data[i].TabID != tabindex) {
                        tabindex = data[i].TabID;
                        amountindex = Number(data[i].TotalPaid);
                    }
                    if (amountindex > 0) {
                        if (amountindex >= amountequalper) data[i].RealAmount = revenue;
                        else if (amountindex > 0) {
                            if (IsDocGreater == 0) data[i].RealAmount = (amountindex < revenue ? amountindex : revenue);
                            else data[i].RealAmount = 0;
                        }

                        else data[i].RealAmount = 0;
                        if (Number(data[i].TreatInDate) == 1 && data[i].RealAmount != 0) {
                            data[i].Havingrevenue = 1;
                        }
                        if (Number(data[i].TreatInDate) == 0 && data[i].RealAmount > data[i].OldRealAmount) {
                            data[i].Havingrevenue = 1;
                            data[i].RealAmount = data[i].RealAmount - data[i].OldRealAmount;
                        }
                        amountindex = amountindex != 0 ? (amountindex - amountequalper) : 0;
                    }
                    else {
                        data[i].Havingrevenue = 0;
                        data[i].RealAmount = 0;
                    }

                }

                for (let i = 0; i < data.length; i++) {
                    if (data[i].Manual_Revenue == 0) {
                        if (Number(data[i].Havingrevenue) == 1)  {
                            commission = data[i].RealAmount;
                            let RePercent = Number(data[i].RePercent);
                            RePercent = (RePercent > 0 && RePercent <= 100) ? RePercent : 100;
                            realamount = commission * RePercent / 100;

                            if (IsDocDevide == 1) {
                                perjson = (data[i].BS1 != 0 ? 1 : 0) + (data[i].BS2 != 0 ? 1 : 0) + (data[i].BS3 != 0 ? 1 : 0) + (data[i].BS4 != 0 ? 1 : 0);
                                realamount = perjson != 0 ? realamount / perjson : 0;
                                commission = perjson != 0 ? commission / perjson : 0;
                            }
                            if (CurrentMaster != 0) {
                                if (data[i].BS1 == CurrentMaster) settings.RevenueDetail_Push(data[i], "BS1", "RealAmount1", commission, realamount);
                                if (data[i].BS2 == CurrentMaster && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS2", "RealAmount2", commission, realamount);
                                if (data[i].BS3 == CurrentMaster && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS3", "RealAmount3", commission, realamount);
                                if (data[i].BS4 == CurrentMaster && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS4", "RealAmount4", commission, realamount);
                            }
                            else {

                                if (data[i].BS1 != 0) settings.RevenueDetail_Push(data[i], "BS1", "RealAmount1", commission, realamount);
                                if (data[i].BS2 != 0 && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS2", "RealAmount2", commission, realamount);
                                if (data[i].BS3 != 0 && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS3", "RealAmount3", commission, realamount);
                                if (data[i].BS4 != 0 && IsDocDevide == 1) settings.RevenueDetail_Push(data[i], "BS4", "RealAmount4", commission, realamount);
                            }
                        }
                    }
                    else {
                        if (CurrentMaster != 0) {
                            if (data[i].BS1 == CurrentMaster) settings.RevenueDetail_Push(data[i], "BS1", "RealAmount1", commission, data[i].RealAmount1);
                            if (data[i].BS2 == CurrentMaster) settings.RevenueDetail_Push(data[i], "BS2", "RealAmount2", commission, data[i].RealAmount2);
                            if (data[i].BS3 == CurrentMaster) settings.RevenueDetail_Push(data[i], "BS3", "RealAmount3", commission, data[i].RealAmount3);

                        }
                        else {

                            if (data[i].BS1 != 0) settings.RevenueDetail_Push(data[i], "BS1", "RealAmount1", commission, data[i].RealAmount1);
                            if (data[i].BS2 != 0) settings.RevenueDetail_Push(data[i], "BS2", "RealAmount2", commission, data[i].RealAmount2);
                            if (data[i].BS3 != 0) settings.RevenueDetail_Push(data[i], "BS3", "RealAmount3", commission, data[i].RealAmount3);

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
            let settings = RevenueDoctor.prototype.settings;
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
        let settings = RevenueDoctor.prototype.settings;
        let dataExport = {"stt": "STT", "empName": Outlang["Ten_nhan_vien"], "realamount": Outlang["Tong_tien"], "totalPaid": Outlang["Tong_tien_thu"] ?? 'Tổng tiền thu', "paymentPeriod": Outlang["Thu_trong_ky"] ?? 'Thu trong kỳ'};
        let nameFile = filename != '' ? filename : Outlang['Doanh_thu_tong'];
        await exportJsonToExcel(nameFile, settings.DataRevMaster, dataExport);
    }

    this.ExportDetail = async function (filename) {
        let settings = RevenueDoctor.prototype.settings;
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
                data : [Outlang["Sys_dia_chi_khach_hang1"]]
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
            , "Service_Code": Outlang["Ma_dich_vu"] != undefined ? Outlang["Ma_dich_vu"] : 'Mã dịch vụ'
            , "ServiceID": [Outlang["Dich_vu"], (v) => {return settings.RevenueDetail_ServiceName(v)}]
            , "ServiceType": (Outlang["Loai_dich_vu"] != undefined ? Outlang["Loai_dich_vu"] : 'Loại dịch vụ')
            , "SalesCode": Outlang["Ma_len_don"]
            , "Quantity": Outlang["So_luong"]
            , "Price": Outlang["Gia_dich_vu"]
            , "Detail": [Outlang["Chi_tiet"], (v, { TimeTreatIndex, TimeTreat, TeethType, TeethChoosing, }) =>
            {
                return ((Number(sys_dencos_Main) == 1)
                    ? (`${(TeethChoosing != '') ? Fun_GetTeeth_ByToken(DataTeeth, TeethChoosing, TeethType) : ''} `)
                    : (Outlang['Lan_dieu_tri'] + ': ' + TimeTreatIndex + ' | ' + TimeTreat));
            }]
            , "NewPer": [`${Outlang["Hoan_thanh"] ?? 'Hoàn thành'} %`, (v, { NewPer, TimeTreatIndex, TimeTreat }) => {
                return ((Number(sys_dencos_Main) == 1)
                    ? (NewPer)
                    : (Number(TimeTreat) != 0 ? ((Number(TimeTreatIndex) / Number(TimeTreat)) * 100).toFixed(2) : 0));
            }]
            , "PercentOfService": Outlang["Tong_hoan_thanh"] ?? "Tổng hoàn thành"
            , "Empname": Outlang["Nhan_vien"]            
            , "Empcode": Outlang["Ma_nhan_vien"] ?? "Mã nhân viên"           
            , "Commission": Outlang["Hoa_hong"]
            , "RePercent": [Outlang["Phan_tram_doanh_thu"], (v) => { return (v != 0 ? v : 100) }]
            , "RealAmount": Outlang["Doanh_thu"]
            
            , "PaymentPeriod": Outlang["Thu_trong_ky"]
            , "PaymentService": Outlang["Tong_tien_thu"] == undefined ? 'Tiền thu dịch vụ' : Outlang["Tong_tien_thu"]
            , "Date": [Outlang["Ngay_dieu_tri"], (v) => { return ConvertDateTimeUTC_DMY(v) }]
            , "BranchName": Outlang["Chi_nhanh"]
            , "Note": Outlang["Ghi_chu_hoa_hong"] == undefined ? 'Ghi chú hoa hồng' : Outlang["Ghi_chu_hoa_hong"]
            , "Content_Follow": (Outlang["Noi_dung_dieu_tri"] != undefined ? Outlang["Noi_dung_dieu_tri"] : 'Nội dung điều trị')
            , "Manual_Revenue": [Outlang["Thu_cong"], (v, {RealAmount}) => {return (v == 1 ? RealAmount : 0)}]
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
