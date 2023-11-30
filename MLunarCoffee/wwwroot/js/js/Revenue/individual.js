//#region  Executing count
function Revenue_Executing_Method(dataset) {
    report_data = [];
    let Treat_InTime = dataset.Table;
    let Treat_Before = dataset.Table2;
    let Treat_Manual = dataset.Table1;

    let Tab_Consul = dataset.Table3;
    let Tab_Consul_Manual = dataset.Table4;

    let _type = 0;
    let _dataMain = [];

    // #region // Treatment
    // BS ////////////
    let Treat_InTime_BS = Treat_InTime.filter(word =>
        Number(word["BS1"]) == Number(sys_employeeID_Main)
        || Number(word["BS2"]) == Number(sys_employeeID_Main)
        || Number(word["BS3"]) == Number(sys_employeeID_Main)
        || Number(word["BS4"]) == Number(sys_employeeID_Main)
    );
    let Treat_Before_BS = Treat_Before.filter(word =>
        Number(word["BS1"]) == Number(sys_employeeID_Main)
        || Number(word["BS2"]) == Number(sys_employeeID_Main)
        || Number(word["BS3"]) == Number(sys_employeeID_Main)
        || Number(word["BS4"]) == Number(sys_employeeID_Main)
    );
    let Treat_Manual_BS = Treat_Manual.filter(word =>
        Number(word["IsDoctor"]) == 1
    );
    let report_depentamount_BS = report_rule.DepentAmount_DOC;
    let report_way_to_count_BS = report_rule.WayCount_DOC;
    _type = 1;
    Revenue_Executing_Treat_Detail(sys_employeeID_Main, _type, _dataMain
        , Treat_InTime_BS, Treat_Before_BS, Treat_Manual_BS, report_depentamount_BS
        , report_way_to_count_BS);

    //////////////////


    // PT ////////////

    let Treat_InTime_PT = Treat_InTime.filter(word =>
        Number(word["PT1"]) == Number(sys_employeeID_Main)
        || Number(word["PT2"]) == Number(sys_employeeID_Main)
        || Number(word["PT3"]) == Number(sys_employeeID_Main)
        || Number(word["PT4"]) == Number(sys_employeeID_Main)
    );
    let Treat_Before_PT = Treat_Before.filter(word =>
        Number(word["PT1"]) == Number(sys_employeeID_Main)
        || Number(word["PT2"]) == Number(sys_employeeID_Main)
        || Number(word["PT3"]) == Number(sys_employeeID_Main)
        || Number(word["PT4"]) == Number(sys_employeeID_Main)
    );
    let Treat_Manual_PT = Treat_Manual.filter(word =>
        Number(word["IsAssistant"]) == 1
    );
    let report_depentamount_PT = report_rule.DepentAmount_ASS;
    let report_way_to_count_PT = report_rule.WayCount_ASS;
    _type = 2;
    Revenue_Executing_Treat_Detail(sys_employeeID_Main, _type, _dataMain
        , Treat_InTime_PT, Treat_Before_PT, Treat_Manual_PT, report_depentamount_PT
        , report_way_to_count_PT);

    //////////////////

    // TECH ////////////
    let Treat_InTime_TECH = Treat_InTime.filter(word =>
        Number(word["TECH1"]) == Number(sys_employeeID_Main)
        || Number(word["TECH2"]) == Number(sys_employeeID_Main)
    );
    let Treat_Before_TECH = Treat_Before.filter(word =>
        Number(word["TECH1"]) == Number(sys_employeeID_Main)
        || Number(word["TECH2"]) == Number(sys_employeeID_Main)
    );
    let Treat_Manual_TECH = Treat_Manual.filter(word =>
        Number(word["IsTech"]) == 1
    );
    let report_depentamount_TECH = report_rule.DepentAmount_TECH;
    let report_way_to_count_TECH = report_rule.WayCount_TECH;
    _type = 3;
    Revenue_Executing_Treat_Detail(sys_employeeID_Main, _type, _dataMain
        , Treat_InTime_TECH, Treat_Before_TECH, Treat_Manual_TECH, report_depentamount_TECH
        , report_way_to_count_TECH);

    //////////////////
    //#endregion


    //#region Consul ////////////
    let report_way_to_count_CONSUL = report_rule.WayCount_CONSUL;
    _type = 4;
    Revenue_Executing_Tab_Detail(sys_employeeID_Main, _type, _dataMain
        , Tab_Consul, Tab_Consul_Manual
        , report_way_to_count_CONSUL);

    //////////////////
    //#endregion

    report_data = _dataMain;
    Revenue_Load_Data_Indicator(report_data);
    Revenue_Render_Data_Report(report_data, "dtContentReportRevenueMasterBody", 0);
}
function Revenue_Executing_Treat_Detail(empid, _type, data, intime_data, before_data, manual_data, _depentamount, _way_to_count) {

    let _Percent_Serivice = 0, _Amount_Serivice = 0, _Percent = 0, _Amount = 0;
    let _typeName = '';
    let _num = 0;
    let _amount1 = 0, _amount2 = 0, _amount3 = 0, _amount4 = 0;
    let _Emp1 = 0, _Emp2 = 0, _Emp3 = 0, _Emp4 = 0;
    let datamain = [];
    if (_depentamount == 1) {
        Array.prototype.push.apply(datamain, intime_data, before_data);
    }
    else datamain = intime_data;

    for (i = 0; i < datamain.length; i++) {
        let _e = {};
        if (sys_dencos_Main == 0) {
            datamain[i].AmountForThisTreatment
                = ((datamain[i].TimeTreat != 0) ? datamain[i].Price / datamain[i].TimeTreat : 0)
                * datamain[i].TimeTreatIndex;
        }
        else {
            datamain[i].AmountForThisTreatment
                = (datamain[i].Price / 100) * datamain[i].PercentOfService;
        }
        datamain[i].IsRevenue = (Number(datamain[i].AmountForThisTreatment) <= Number(datamain[i].TotalPay))
            ? 1 : 0;

        if (sys_dencos_Main == 0) {
            datamain[i].Revenue = datamain[i].AmountForThisTreatment / datamain[i].TimeTreatIndex;
        }
        else {
            datamain[i].Revenue = (datamain[i].Price / 100) * datamain[i].NewPer;
        }


        switch (_type) {
            case 1:
                _num = ((datamain[i].BS1 != 0) ? 1 : 0)
                    + ((datamain[i].BS2 != 0) ? 1 : 0)
                    + ((datamain[i].BS3 != 0) ? 1 : 0)
                    + ((datamain[i].BS4 != 0) ? 1 : 0);
                _Emp1 = datamain[i].BS1;
                _Emp2 = datamain[i].BS2;
                _Emp3 = datamain[i].BS3;
                _Emp4 = datamain[i].BS4;
                _Percent_Serivice = datamain[i].BS_Percent_Serivice;
                _Amount_Serivice = datamain[i].BS_Amount_Serivice;
                _Percent = datamain[i].BS_Percent;
                _Amount = datamain[i].BS_Amount;
                _typeName = "Bác Sĩ"
                break;
            case 2:
                _num = ((datamain[i].PT1 != 0) ? 1 : 0)
                    + ((datamain[i].PT2 != 0) ? 1 : 0)
                    + ((datamain[i].PT3 != 0) ? 1 : 0)
                    + ((datamain[i].PT4 != 0) ? 1 : 0);
                _Emp1 = datamain[i].PT1;
                _Emp2 = datamain[i].PT2;
                _Emp3 = datamain[i].PT3;
                _Emp4 = datamain[i].PT4;
                _Percent_Serivice = datamain[i].PT_Percent_Serivice;
                _Amount_Serivice = datamain[i].PT_Amount_Serivice;
                _Percent = datamain[i].PT_Percent;
                _Amount = datamain[i].PT_Amount;
                _typeName = "Phụ Tá"
                break;
            case 3:
                _num = ((datamain[i].TECH1 != 0) ? 1 : 0)
                    + ((datamain[i].TECH2 != 0) ? 1 : 0);
                _Emp1 = datamain[i].TECH1;
                _Emp2 = datamain[i].TECH2;
                _Emp3 = 0;
                _Emp4 = 0;
                _Percent_Serivice = datamain[i].TECH_Percent_Serivice;
                _Amount_Serivice = datamain[i].TECH_Amount_Serivice;
                _Percent = datamain[i].TECH_Percent;
                _Amount = datamain[i].TECH_Amount;
                _typeName = "HT Chuyên Môn"
                break;
            default: break;
        }


        datamain[i].RevenueFor1Treatment = (_depentamount == 0)
            ? datamain[i].Revenue :
            ((datamain[i].IsRevenue == 1) ? datamain[i].Revenue : 0);

        // #region

        _amount1 = (_Emp1 != empid) ? 0 : Detail_Executing_GetRevenue(
            _num
            , _Percent_Serivice, _Amount_Serivice
            , _Percent, _Amount
            , datamain[i].Price, datamain[i].CardAmountUsing
            , datamain[i].Quantity, datamain[i].NewPer
            , datamain[i].RevenueFor1Treatment
            , _depentamount, _way_to_count
            , 1);
        _amount2 = (_Emp2 != empid) ? 0 : Detail_Executing_GetRevenue(
            _num
            , _Percent_Serivice, _Amount_Serivice
            , _Percent, _Amount
            , datamain[i].Price, datamain[i].CardAmountUsing
            , datamain[i].Quantity, datamain[i].NewPer
            , datamain[i].RevenueFor1Treatment
            , _depentamount, _way_to_count
            , 0);
        _amount3 = (_Emp3 != empid) ? 0 : Detail_Executing_GetRevenue(
            _num
            , _Percent_Serivice, _Amount_Serivice
            , _Percent, _Amount
            , datamain[i].Price, datamain[i].CardAmountUsing
            , datamain[i].Quantity, datamain[i].NewPer
            , datamain[i].RevenueFor1Treatment
            , _depentamount, _way_to_count
            , 0);
        _amount4 = (_Emp4 != empid) ? 0 : Detail_Executing_GetRevenue(
            _num
            , _Percent_Serivice, _Amount_Serivice
            , _Percent, _Amount
            , datamain[i].Price, datamain[i].CardAmountUsing
            , datamain[i].Quantity, datamain[i].NewPer
            , datamain[i].RevenueFor1Treatment
            , _depentamount, _way_to_count
            , 0);
        _e.Amount = _amount1 + _amount2 + _amount3 + _amount4;
        _e.ServiceID = datamain[i].ServiceID;
        _e.Before_Revenue = datamain[i].Before_Revenue;
        _e.Manual_Revenue = 0;
        _e.Date = datamain[i].TreatmentDate;
        _e.BranchID = datamain[i].BranchID;
        _e.CustID = datamain[i].CustID;
        _e.CustCode = datamain[i].CustCode;
        _e.CustName = datamain[i].CustName;
        _e.Type = _type;
        _e.TypeName = _typeName;
        data.push(_e)
        //#endregion

    }
    for (i = 0; i < manual_data.length; i++) {
        switch (_type) {
            case 1:
                _typeName = "Bác Sĩ"
                break;
            case 2:
                _typeName = "Phụ Tá"
                break;
            case 3:
                _typeName = "HT Chuyên Môn"
                break;
            default: break;
        }
        let _e = {};
        _amount1 = (manual_data[i].EMP1 == empid) ? manual_data[i].Commission1 : 0;
        _amount2 = (manual_data[i].EMP2 == empid) ? manual_data[i].Commission2 : 0;
        _amount3 = (manual_data[i].EMP3 == empid) ? manual_data[i].Commission3 : 0;
        _e.Amount = _amount1 + _amount2 + _amount3;
        _e.ServiceID = manual_data[i].ServiceID;
        _e.Before_Revenue = 0;
        _e.Manual_Revenue = 1;
        _e.Date = manual_data[i].Manual_Revenue_Date;
        _e.BranchID = manual_data[i].BranchID;
        _e.CustID = manual_data[i].CustID;
        _e.CustCode = manual_data[i].CustCode;
        _e.CustName = manual_data[i].CustName;
        _e.Type = _type;
        _e.TypeName = _typeName;
        data.push(_e)
    }
}function Detail_Executing_GetRevenue(NumPerson, Permision_Percent_Serivice, Permision_Amount_Serivice, Permision_Percent, Permision_Amount
    , Price, CardAmountUsing, Quantity, NewPer, RevenueFor1Treatment, _depentamount, _way_to_count, IsFirst) {

    let _res = 0, _price = 0, _percent = 0, _amount = 0, _revenue_amount = 0, _IsUsing_Specific_Revenue = 0;
    let _revenue_setting = 0;
    let _revenue_counting = 0;

    if (IsFirst == 1 && Permision_Percent + Permision_Amount > 0) {
        _percent = Permision_Percent;
        _amount = Permision_Amount;
        _IsUsing_Specific_Revenue = 1;
    }
    else if (IsFirst == 0 && Permision_Percent + Permision_Amount > 0) {
        _IsUsing_Specific_Revenue = -1;
    }
    else {
        _percent = Permision_Percent_Serivice;
        _amount = Permision_Amount_Serivice;
        _IsUsing_Specific_Revenue = 0;
    }
    if (_depentamount == 0) {
        _price = Price + CardAmountUsing;
    }
    else {
        _price = Price;
    }

    if (_IsUsing_Specific_Revenue == -1) _res = 0;
    else {
        // Find in setting
        if (_amount != 0) _revenue_setting = _amount * ((sys_dencos_Main == 1) ? Quantity : 1);
        else {
            if (sys_dencos_Main == 0) {
                _revenue_setting = _price * _percent / 100 / Quantity;
            }
            else {
                _revenue_setting = (_price * _percent / 100) * NewPer / 100;
            }
        }
        // Find in couting
        if (RevenueFor1Treatment > 0) {
            if (_amount != 0) _revenue_counting = _amount * ((sys_dencos_Main == 1) ? Quantity : 1);
            else {
                _revenue_counting = (RevenueFor1Treatment * _percent) / 100;
            }
        }



        if (_IsUsing_Specific_Revenue == 1) {
            if (_depentamount == 0) {
                _revenue_amount = _revenue_setting
            }
            else {
                _revenue_amount = _revenue_counting;
            }
            _res = _revenue_amount;
        }
        else {
            _res = Detail_GetRevenue_ByType(NumPerson, _revenue_counting, _revenue_setting, _depentamount, _way_to_count, IsFirst);
        }
    }

    return _res
}
function Detail_GetRevenue_ByType(num, revenue_count, revenue_setting, _depentamount, _way_to_count, isfirst) {

    let _res;
    if (_depentamount == 0) {
        if (_way_to_count == 1) {
            if (num == 0) _res = 0;
            else _res = revenue_setting / num;
        }
        else if (_way_to_count == 2) {
            if (isfirst == 1) _res = revenue_setting;
            else _res = 0;
        }
        else {
            _res = revenue_setting;
        }
    }
    else {
        if (_way_to_count == 1) {
            if (num == 0) _res = 0;
            else _res = revenue_count / num;
        }
        else if (_way_to_count == 2) {
            if (isfirst == 1) _res = revenue_count;
            else _res = 0;
        }
        else {
            _res = revenue_count;
        }
    }
    return _res;
}
function Revenue_Executing_Tab_Detail(empid, _type, data, intime_data,  manual_data, _way_to_count) {
    let datamain = intime_data;
    let _amount1 = 0, _amount2 = 0, _amount3 = 0, _amount4 = 0;
    let _num = 0;
    for (i = 0; i < datamain.length; i++) {
        let _e = {};
        _num = ((datamain[i].EMP1 != 0) ? 1 : 0)
            + ((datamain[i].EMP2 != 0) ? 1 : 0)
            + ((datamain[i].EMP3 != 0) ? 1 : 0)
            + ((datamain[i].EMP4 != 0) ? 1 : 0);

        _amount1 = (datamain[i].EMP1 != empid) ? 0 : Detail_Executing_GetRevenue_Consult(
            _num
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , _way_to_count
            , 1);
        _amount2 = (datamain[i].EMP2 != empid) ? 0 : Detail_Executing_GetRevenue_Consult(
            _num
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , _way_to_count
            , 0);
        _amount3 = (datamain[i].EMP3 != empid) ? 0 : Detail_Executing_GetRevenue_Consult(
            _num
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , _way_to_count
            , 0);
        _amount4 = (datamain[i].EMP4 != empid) ? 0 : Detail_Executing_GetRevenue_Consult(
            _num
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , _way_to_count
            , 0);

        _e.Amount = _amount1 + _amount2 + _amount3 + _amount4;
        _e.ServiceID = datamain[i].ServiceID;
        _e.Before_Revenue = 0;
        _e.Manual_Revenue = 0;
        _e.Date = datamain[i].Created;
        _e.BranchID = datamain[i].BranchID;
        _e.CustID = datamain[i].CustID;
        _e.CustCode = datamain[i].CustCode;
        _e.CustName = datamain[i].CustName;
        _e.Type = _type;
        data.push(_e)

    }
    for (i = 0; i < manual_data.length; i++) {
        let _e = {};
        _amount1 = (manual_data[i].EMP1 == empid) ? manual_data[i].Commission1 : 0;
        _amount2 = (manual_data[i].EMP2 == empid) ? manual_data[i].Commission2 : 0;
        _amount3 = (manual_data[i].EMP3 == empid) ? manual_data[i].Commission3 : 0;
        _e.Amount = _amount1 + _amount2 + _amount3;
        _e.ServiceID = manual_data[i].ServiceID;
        _e.Before_Revenue = 0;
        _e.Manual_Revenue = 1;
        _e.Date = manual_data[i].Manual_Revenue_Date;
        _e.BranchID = manual_data[i].BranchID;
        _e.CustID = manual_data[i].CustID;
        _e.CustCode = manual_data[i].CustCode;
        _e.CustName = manual_data[i].CustName;
        _e.Type = _type;
        data.push(_e)
    }

}
function Detail_Executing_GetRevenue_Consult(NumPerson, Permision_Percent_Serivice, Permision_Amount_Serivice, Permision_Percent, Permision_Amount
    , Price, Quantity, TotalPay, _way_to_count, IsFirst) {
    let _res = 0, _percent = 0, _amount = 0, _IsUsing_Specific_Revenue = 0;
    let _revenue = 0;

    if (IsFirst == 1 && Permision_Percent + Permision_Amount > 0) {
        _percent = Permision_Percent;
        _amount = Permision_Amount;
        _IsUsing_Specific_Revenue = 1;
    }
    else if (IsFirst == 0 && Permision_Percent + Permision_Amount > 0) {
        _IsUsing_Specific_Revenue = -1;
    }
    else {
        _percent = Permision_Percent_Serivice;
        _amount = Permision_Amount_Serivice;
        _IsUsing_Specific_Revenue = 0;
    }

    if (_IsUsing_Specific_Revenue == -1) _res = 0;
    else {
        // Find in setting
        if (_amount != 0) {
            _revenue = ((TotalPay / Price) * _amount) * Quantity;
        }
        else {
            _revenue = (TotalPay / Price) * Price * _percent / 100

        }

        _res = Detail_GetRevenue_ByType_Consult(NumPerson, _revenue, _way_to_count, IsFirst);
    }

    return _res
}
function Detail_GetRevenue_ByType_Consult(num, revenue_count, _way_to_count, isfirst) {

    let _res;
    if (_way_to_count == 1) {
        if (num == 0) _res = 0;
        else _res = revenue_count / num;
    }
    else if (_way_to_count == 2) {
        if (isfirst == 1) _res = revenue_count;
        else _res = 0;
    }
    else {
        _res = revenue_count;
    }
    return _res;
}
function Revenue_Render_Data_Report(data, id, type) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        let datadetail = data;
        if (data && data.length > 0) {
            if (type != 0) {
                datadetail = data.filter(word => Number(word["Type"]) == type);
            }
            if (datadetail && datadetail.length > 0) {
                for (var i = 0; i < datadetail.length; i++) {
                    let item = datadetail[i];
                    let tr = '<td>' + (i + 1) + '</td>'
                        + '<td><a href="/Customer/MainCustomer?CustomerID=' + item.CustID + "&ver=" + versionofWebApplication + '" target="_blank">' + item.CustCode + '</a></td>'
                        + '<td>' + item.CustName + '</td>'
                        + '<td>' + ((Number(item.IsCard) == 1) ? report_CardData[item.ServiceID] : report_ServiceData[item.ServiceID]) + '</td>'
                        + '<td>' + ((report_BranchData[item.BranchID] != undefined) ? report_BranchData[item.BranchID] :'')  + '</td>'
                        + '<td>' + formatDateToDMYHM(item.Date) + '</td>'
                        + '<td>' + formatNumber(item.Amount) + '</td>'
                        + '<td style="text-align:center">'
                        + ((item.Manual_Revenue) ? ('<i class="imgGrid vtt-icon vttech-icon-check"></i>') : (''))
                        + '</td>'
                    stringContent = stringContent + '<tr role="row">' + tr + '</tr>';
                }
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function Revenue_Load_Data_Indicator(data) {
    if (data != null && data != undefined) {
        let count_doc = data.filter(word => Number(word["Type"]) == 1).length;
        let count_ass = data.filter(word => Number(word["Type"]) == 2).length;
        let count_tech = data.filter(word => Number(word["Type"]) == 3).length;
        let count_consul = data.filter(word => Number(word["Type"]) == 4).length;
        let count_total = count_consul + count_doc + count_ass + count_tech;

        //Total Amount
        Count_Up('revenue_amount_treat_doctor', Revenue_Total_Amount_By_Type(report_data, 1))
        Count_Up('revenue_amount_treat', Revenue_Total_Amount_By_Type(report_data, 2))
        Count_Up('revenue_amount_tech_support', Revenue_Total_Amount_By_Type(report_data, 3))
        Count_Up('revenue_amount_consult', Revenue_Total_Amount_By_Type(report_data, 4))
        Count_Up('revenue_amount_total', Revenue_Total_Amount_By_Type(report_data, 0))
 

        //Count Times
        Count_Up('revenue_amount_treat_doctor', count_doc)
        Count_Up('revenue_value_time_treat', count_ass)
        Count_Up('revenue_value_time_tech_support', count_tech)
        Count_Up('revenue_value_time_consult', count_consul)
        Count_Up('revenue_value_time_total', count_total)
 

        //$('.revenue_value_count').each(function () {
        //    $(this).prop('Counter', 0).animate({
        //        Counter: $(this).text()
        //    }, {
        //        duration: 1000,
        //        easing: 'swing',
        //        step: function (now) {
        //            $(this).text(formatNumber(Math.ceil(now)));
        //        }
        //    });
        //});
    }
}
function Revenue_Total_Amount_By_Type(data, type) {
    let result = 0;
    if (data != null && data != undefined) {
        let data_temporary;
        if (type == 0) {
            data_temporary = data;
        }
        else {
            data_temporary = data.filter(word => Number(word["Type"]) == type);
        }
        if (data_temporary && data_temporary.length != 0) {
            for (let i = 0; i < data_temporary.length; i++) {
                result += Number(data_temporary[i].Amount);
            }
        }
    }
    return result;
}