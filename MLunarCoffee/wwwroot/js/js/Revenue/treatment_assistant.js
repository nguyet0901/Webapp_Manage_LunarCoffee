//#region  Executing count
function Revenue_Executing_Method(dataset, branchid) {
    let datamain = [];
    let Treat_InTime = dataset.Table;
    let Treat_Manual = dataset.Table1;
    if (report_depentamount == 1) {
        let Treat_Before = dataset.Table2;
        Array.prototype.push.apply(datamain, Treat_InTime);
        Array.prototype.push.apply(datamain, Treat_Before);
    }
    else datamain = Treat_InTime;
    if (Number(branchid) != 0) {
        datamain = datamain.filter(word => word["BranchID"] == Number(branchid));
        Treat_Manual = Treat_Manual.filter(word => word["BranchID"] == Number(branchid));
    }

    for (i = 0; i < datamain.length; i++) {

        if (datamain[i].PT1 + datamain[i].PT2 + datamain[i].PT3 + datamain[i].PT4 != 0) {
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

            datamain[i].NumPerson = ((datamain[i].PT1 != 0) ? 1 : 0)
                + ((datamain[i].PT2 != 0) ? 1 : 0)
                + ((datamain[i].PT3 != 0) ? 1 : 0)
                + ((datamain[i].PT4 != 0) ? 1 : 0);

            datamain[i].RevenueFor1Treatment = (report_depentamount == 0)
                ? datamain[i].Revenue :
                ((datamain[i].IsRevenue == 1) ? datamain[i].Revenue : 0);

            // #region

            datamain[i].RealAmount1 = Detail_Executing_GetRevenue(
                datamain[i].NumPerson
                , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
                , datamain[i].Permision_Percent, datamain[i].Permision_Amount
                , datamain[i].Price, datamain[i].CardAmountUsing
                , datamain[i].Quantity, datamain[i].NewPer
                , datamain[i].RevenueFor1Treatment
                , 0);
            datamain[i].RealAmount2 = Detail_Executing_GetRevenue(
                datamain[i].NumPerson
                , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
                , datamain[i].Permision_Percent, datamain[i].Permision_Amount
                , datamain[i].Price, datamain[i].CardAmountUsing
                , datamain[i].Quantity, datamain[i].NewPer
                , datamain[i].RevenueFor1Treatment
                , 0);
            datamain[i].RealAmount3 = Detail_Executing_GetRevenue(
                datamain[i].NumPerson
                , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
                , datamain[i].Permision_Percent, datamain[i].Permision_Amount
                , datamain[i].Price, datamain[i].CardAmountUsing
                , datamain[i].Quantity, datamain[i].NewPer
                , datamain[i].RevenueFor1Treatment
                , 0);
            datamain[i].RealAmount4 = Detail_Executing_GetRevenue(
                datamain[i].NumPerson
                , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
                , datamain[i].Permision_Percent, datamain[i].Permision_Amount
                , datamain[i].Price, datamain[i].CardAmountUsing
                , datamain[i].Quantity, datamain[i].NewPer
                , datamain[i].RevenueFor1Treatment
                , 0);
            //#endregion

        }
    }

    dataDetail = datamain;
    dataDetail_manual = Treat_Manual;

    RenderReportMaster(report_EmployeeData, "dtContentReportMasterBody");
    if ($('.masterRow')[0]) {
        $('.masterRow')[0].click();
    }


}
function Detail_Executing_GetRevenue(NumPerson, Permision_Percent_Serivice, Permision_Amount_Serivice, Permision_Percent, Permision_Amount
    , Price, CardAmountUsing, Quantity, NewPer, RevenueFor1Treatment, IsFirst) {

    let _res = 0, _price = 0, _percent = 0, _amount = 0, _revenue_amount = 0, _IsUsing_Specific_Revenue = 0;
    let _revenue_setting = 0;
    let _revenue_counting = 0;

    if (IsFirst == 0 && Permision_Percent + Permision_Amount > 0) {
        _percent = Permision_Percent;
        _amount = Permision_Amount;
        _IsUsing_Specific_Revenue = 1;
    }
    //else if (IsFirst == 0 && Permision_Percent + Permision_Amount > 0) {
    //    //_IsUsing_Specific_Revenue = -1;
    //    _percent = Permision_Percent_Serivice;
    //    _amount = Permision_Amount_Serivice;
    //    _IsUsing_Specific_Revenue = 0;
    //}
    else {
        _percent = Permision_Percent_Serivice;
        _amount = Permision_Amount_Serivice;
        _IsUsing_Specific_Revenue = 0;
    }
    if (report_depentamount == 0) {
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
            if (report_depentamount == 0) {
                _revenue_amount = Detail_GetRevenue_ByType(NumPerson, _revenue_counting, _revenue_setting, IsFirst);//_revenue_setting
            }
            else {
                _revenue_amount = _revenue_counting;
            }
            _res = _revenue_amount;
        }
        else {
            _res = Detail_GetRevenue_ByType(NumPerson, _revenue_counting, _revenue_setting, IsFirst);
        }
    }

    return _res
}
function Detail_GetRevenue_ByType(num, revenue_count, revenue_setting, isfirst) {

    let _res;
    if (report_depentamount == 0) {
        if (report_way_to_count == 1 || report_way_to_count == 2) {
            if (num == 0) _res = 0;
            else _res = revenue_setting / num;
        }
        //else if (report_way_to_count == 2) {
            //if (isfirst == 1)
                //_res = revenue_setting / num;
            //else _res = 0;
        //}
        else {
            _res = revenue_setting;
        }
    }
    else {
        if (report_way_to_count == 1) {
            if (num == 0) _res = 0;
            else _res = revenue_count / num;
        }
        else if (report_way_to_count == 2) {
            if (isfirst == 1) _res = revenue_count;
            else _res = 0;
        }
        else {
            _res = revenue_count;
        }
    }
    return _res;
}
//function filterDataMasterByBranch(BranchID) {

//    let data = report_EmployeeData;
//    if (BranchID != 0) {
//        let IDBranchstring = ',' + BranchID.toString() + ',';
//        data = data.filter(word => {
//            if ((',' + word["BranchID"] + ',').includes(IDBranchstring)
//                || Number(word["isAllBranch"]) == 1) return word;
//        }
//        );
//    }
//    RenderReportMaster(data, "dtContentReportMasterBody");
//    if ($('.masterRow')[0]) {
//        $('.masterRow')[0].click();
//    }
//}
//#endregion
//#region  Render master
function RenderReportMaster_TimeJoin(empid) {
    let timejoin = 0;
    for (_i = 0; _i < dataDetail.length; _i++) {
        if (dataDetail[_i].PT1 == empid || dataDetail[_i].PT2 == empid || dataDetail[_i].PT3 == empid || dataDetail[_i].PT4 == empid)
            timejoin = timejoin + 1;
    }
    for (_i = 0; _i < dataDetail_manual.length; _i++) {
        if (dataDetail_manual[_i].EMP1 == empid || dataDetail_manual[_i].EMP2 == empid || dataDetail_manual[_i].EMP3 == empid )
            timejoin = timejoin + 1;
    }
    return timejoin;
}
function RenderReportMaster_Amount(empid) {
    let amount = 0;
    for (_i = 0; _i < dataDetail.length; _i++) {
        if (dataDetail[_i].PT1 == empid) amount = amount + dataDetail[_i].RealAmount1;
        if (dataDetail[_i].PT2 == empid) amount = amount + dataDetail[_i].RealAmount2;
        if (dataDetail[_i].PT3 == empid) amount = amount + dataDetail[_i].RealAmount3;
        if (dataDetail[_i].PT4 == empid) amount = amount + dataDetail[_i].RealAmount4;
    }
    for (_i = 0; _i < dataDetail_manual.length; _i++) {
        if (dataDetail_manual[_i].EMP1 == empid) amount = amount + dataDetail_manual[_i].Commission1;
        if (dataDetail_manual[_i].EMP2 == empid) amount = amount + dataDetail_manual[_i].Commission2;
        if (dataDetail_manual[_i].EMP3 == empid) amount = amount + dataDetail_manual[_i].Commission3;

    }

    return amount;
}
function RenderReportMaster(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        let totalAmount = 0;
        if (data && data.length > 0) {

            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let timejoin = RenderReportMaster_TimeJoin(item.ID);
                let amount = RenderReportMaster_Amount(item.ID);

                if (timejoin != 0) {
                    totalAmount = totalAmount + amount;
                    let obj = Fun_GetObject_ByID(DataEmployee, item.ID);
                    let tr =
                        '<td class="d-none">' + item.ID + '</td>'
                        + '<td class="d-none">' + item.Name + '</td>'
                        + '<td class="vt-number-order"></td>'
                        + '<td>'
                        + '<div class="d-flex">'
                        + '<div class="icon icon-shape icon-sm me-2 text-center">'
                        + '<img class="avatar avatar-xs mt-2" src="' + obj.Avatar + '" alt="label-image">'
                        + '</div>'
                        + '<div class="d-flex flex-column">'
                        + '<h6 class="text-dark text-sm mb-0">' + item.Name + '</h6>'
                        + '<span class="text-xs">'
                        + formatNumber(Math.ceil(amount))

                        + '</span> '
                        + '</div>'
                        + '</div>'

                        + '</td>';





                    stringContent = stringContent + '<tr class="vt-number masterRow" data-items="' + timejoin + '" data-amount="' + amount + '" data-name="' + item.Name + '" data-id="' + item.ID + '">' + tr + '</tr>';

                }
            }
        }
        $('#lbTotalAmount').html('Tổng: ' + formatNumber(Math.round(totalAmount)));
        document.getElementById(id).innerHTML = stringContent;
        EventrowClick();

    }
}
//#endregion
//#region  Render master
function ViewDetail(idDetail, items, amount) {
    let data = dataDetail.filter(word => word["PT1"] == idDetail || word["PT2"] == idDetail || word["PT3"] == idDetail || word["PT4"] == idDetail);
    let data_manual = dataDetail_manual.filter(word => word["EMP1"] == idDetail || word["EMP2"] == idDetail || word["EMP3"] == idDetail);
    if (data != undefined && data_manual != undefined) {
        $('#lbTotalAmount_Detail').html('Tổng: ' + amount);
        $('#lbTotalTimes_Detail').html('Times : ' + items);
        RenderReportDetail(data, data_manual, "dtContentReportBody", idDetail);
    }
}

function RenderReportDetail(data, data_manual, id, idDetail) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        let index_data = 0;
        if (data && data.length > 0) {
            for (let i = 0; i < data.length; i++) {
                index_data = i;
                let item = data[i];
                let tr =
                    '<td class="vt-number-order"></td>'
                    + '<td><a target="_blank" href="/Customer/MainCustomer?CustomerID=' + item.CustID + "&ver=" + versionofWebApplication + '">' + item.CustCode + '</a></td>'
                    + '<td>' + item.CustName + '</td>'
                    + '<td>'
                    + RenderReport_Detail_Get_ServiceName(item.ServiceID)
                    + ((Number(sys_dencos_Main) == 1)
                        ? ('<span class="statustreat_row">'
                            + (item.NewPer + ' % làm mới')
                            + '</span>')
                        : ('<span class="statustreat_row">' + ('Lần điều trị : ' + item.TimeTreatIndex + ' | ' + item.TimeTreat) + '</span>')
                    )
                    + '</td>'
                    + '<td style="text-align:center">'
                    + ((Number(item.IsRevenue) == 1)
                        ? ('<i class="imgGrid vtt-icon vttech-icon-check"></i>')
                        : ('<i class="imgGrid vtt-icon vttech-icon-uncheck"></i>'))
                    + '</td>'
                    + '<td><span class="money_row">'
                    + formatNumber(
                        Math.ceil(
                            ((item.PT1 != idDetail) ? 0 : item.RealAmount1)
                            +
                            ((item.PT2 != idDetail) ? 0 : item.RealAmount2)
                            +
                            ((item.PT3 != idDetail) ? 0 : item.RealAmount3)
                            +
                            ((item.PT4 != idDetail) ? 0 : item.RealAmount4)
                        )
                    )

                    + '</td>'
                    + '<td>' + ConvertDateTimeUTC_DMY(item.TreatmentDate) + '</td>'
                    + '<td>'
                    + ((Number(item.Manual_Revenue) == 1) ? '<i class="imgGrid vtt-icon vttech-icon-check"></i>' : '')
                    + '</td>'



                stringContent = stringContent
                    + ((Number(item.Before_Revenue) == 0) ? '<tr class="vt-number" role="row">' : '<tr role="row" class="vt-number outrangerow">')
                    + tr
                    + '</tr>';
            }
        }

        if (data_manual && data_manual.length > 0) {
            for (let i = 0; i < data_manual.length; i++) {
                let item = data_manual[i];
                let tr =
                    '<td class="vt-number-order"></td>'
                    + '<td><a target="_blank" href="/Customer/MainCustomer?CustomerID=' + item.CustID + "&ver=" + versionofWebApplication + '">' + item.CustCode + '</a></td>'
                    + '<td>' + item.CustName + '</td>'
                    + '<td>'
                    + RenderReport_Detail_Get_ServiceName(item.ServiceID)
                    + ((Number(sys_dencos_Main) == 1)
                        ? ('<span class="statustreat_row">'
                            + (item.NewPer + ' % làm mới')
                            + '</span>')
                        : ('<span class="statustreat_row">' + ('Lần điều trị : ' + item.TimeTreatIndex + ' | ' + item.TimeTreat) + '</span>')
                    )
                    + '</td>'
                    + '<td style="text-align:center">'
                    + '<i class="imgGrid vtt-icon vttech-icon-check"></i>'
                    + '</td>'
                    + '<td><span class="money_row">'
                    + formatNumber(
                        Math.ceil(
                            ((item.EMP1 != idDetail) ? 0 : item.Commission1)
                            +
                            ((item.EMP2 != idDetail) ? 0 : item.Commission2)
                            +
                            ((item.EMP3 != idDetail) ? 0 : item.Commission3)
                        )
                    )

                    + '</td>'
                    + '<td>' + ConvertDateTimeUTC_DMY(item.Manual_Revenue_Date) + '</td>'
                    + '<td>'
                    + ((Number(item.Manual_Revenue) == 1) ? '<i class="imgGrid vtt-icon vttech-icon-check"></i>' : '')
                    + '</td>'



                stringContent = stringContent
                    + ((Number(item.Before_Revenue) == 0) ? '<tr class="vt-number" role="row">' : '<tr role="row" class="vt-number outrangerow">')
                    + tr
                    + '</tr>';
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function RenderReport_Detail_Get_ServiceName(id) {
    try {
        return report_ServiceData[id]
    }
    catch (ex){
        return "";
    }
}
//#endregion