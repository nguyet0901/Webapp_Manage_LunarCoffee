//#region  Executing count
function Revenue_Executing_Method(dataset, branchid) {

    let consul_auto = dataset.Table;
    dataDetail_manual = dataset.Table1;

    let datamain = consul_auto;
    if (Number(branchid) != 0) {
        datamain = datamain.filter(word => word["BranchID"] == Number(branchid));
        dataDetail_manual = dataDetail_manual.filter(word => word["BranchID"] == Number(branchid));
    }

    for (i = 0; i < datamain.length; i++) {
        datamain[i].NumPerson = ((datamain[i].EMP1 != 0) ? 1 : 0)
            + ((datamain[i].EMP2 != 0) ? 1 : 0)
            + ((datamain[i].EMP3 != 0) ? 1 : 0)
            + ((datamain[i].EMP4 != 0) ? 1 : 0);

        datamain[i].RealAmount1 = Detail_Executing_GetRevenue(
            datamain[i].NumPerson
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , 1);
        datamain[i].RealAmount2 = Detail_Executing_GetRevenue(
            datamain[i].NumPerson
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , 0);
        datamain[i].RealAmount3 = Detail_Executing_GetRevenue(
            datamain[i].NumPerson
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , 0);
        datamain[i].RealAmount4 = Detail_Executing_GetRevenue(
            datamain[i].NumPerson
            , datamain[i].Permision_Percent_Serivice, datamain[i].Permision_Amount_Serivice
            , datamain[i].Permision_Percent, datamain[i].Permision_Amount
            , datamain[i].Price
            , datamain[i].Quantity
            , datamain[i].TotalPay
            , 0);

    }

    dataDetail = datamain;

    RenderReportMaster(report_EmployeeData, "dtContentReportMasterBody");
    if ($('.masterRow')[0]) {
        $('.masterRow')[0].click();
    }

}
function Detail_Executing_GetRevenue(NumPerson, Permision_Percent_Serivice, Permision_Amount_Serivice, Permision_Percent, Permision_Amount
    , Price, Quantity, TotalPay, IsFirst) {
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

        _res = Detail_GetRevenue_ByType(NumPerson, _revenue, IsFirst);
    }

    return _res
}
function Detail_GetRevenue_ByType(num, revenue_count, isfirst) {

    let _res;
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
    return _res;
}


//#endregion
//#region  Render master
function RenderReportMaster_TimeJoin(empid) {
    let timejoin = 0;
    for (_i = 0; _i < dataDetail.length; _i++) {
        if (dataDetail[_i].EMP1 == empid || dataDetail[_i].EMP2 == empid || dataDetail[_i].EMP3 == empid || dataDetail[_i].EMP4 == empid)
            timejoin = timejoin + 1;
    }
    for (_i = 0; _i < dataDetail_manual.length; _i++) {
        if (dataDetail_manual[_i].EMP1 == empid || dataDetail_manual[_i].EMP2 == empid || dataDetail_manual[_i].EMP3 == empid)
            timejoin = timejoin + 1;
    }
    return timejoin;
}
function RenderReportMaster_Amount(empid) {
    let amount = 0;
    for (_i = 0; _i < dataDetail.length; _i++) {
        if (dataDetail[_i].EMP1 == empid) amount = amount + dataDetail[_i].RealAmount1;
        if (dataDetail[_i].EMP2 == empid) amount = amount + dataDetail[_i].RealAmount2;
        if (dataDetail[_i].EMP3 == empid) amount = amount + dataDetail[_i].RealAmount3;
        if (dataDetail[_i].EMP4 == empid) amount = amount + dataDetail[_i].RealAmount4;
    }
    for (_i = 0; _i < dataDetail_manual.length; _i++) {
        if (dataDetail_manual[_i].EMP1 == empid) amount = amount + dataDetail_manual[_i].RealAmount1;
        if (dataDetail_manual[_i].EMP2 == empid) amount = amount + dataDetail_manual[_i].RealAmount2;
        if (dataDetail_manual[_i].EMP3 == empid) amount = amount + dataDetail_manual[_i].RealAmount3;

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

                        + '</td>'
       

                    stringContent = stringContent + '<tr class="vt-number masterRow" data-items="' + timejoin + '" data-amount="' + amount + '" data-name="' + item.Name + '" data-id="' + item.ID + '">' + tr + '</tr>';
                 
                }
            }
        }
        Count_Up('lbTotalAmount', totalAmount)
 
        document.getElementById(id).innerHTML = stringContent;
        EventrowClick();
    
    }
}
//#endregion
//#region  Render master
function ViewDetail(idDetail, items, amount) {
    var data = dataDetail.filter(word => word["EMP1"] == idDetail || word["EMP2"] == idDetail || word["EMP3"] == idDetail || word["EMP4"] == idDetail);
    let data_manual = dataDetail_manual.filter(word => word["EMP1"] == idDetail || word["EMP2"] == idDetail || word["EMP3"] == idDetail);
    if (data != undefined && data_manual != undefined) {
        Count_Up('lbTotalAmount_Detail', amount)
        Count_Up('lbTotalTimes_Detail', items)
 
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
            for (var i = 0; i < data.length; i++) {
                index_data = i;
                let item = data[i];
                let tr =
                    '<td class="vt-number-order"></td>'
                    + '<td><a target="_blank" href="/Customer/MainCustomer?CustomerID=' + item.CustID + "&ver=" + versionofWebApplication + '">' + item.CustCode + '</a></td>'
                    + '<td>' + item.CustName + '</td>'
                    + '<td>'
                    + ((Number(item.IsCard) == 1) ? (RenderReport_Detail_Get_CardName(item.ServiceID)) : (RenderReport_Detail_Get_ServiceName(item.ServiceID)))
                    + '</td>'
                    + '<td>' + (item.IsProduct==1?"X":"") + '</td>'
                    + '<td><span class="money_row">'
                    + formatNumber(
                        Math.ceil(
                            ((item.EMP1 != idDetail) ? 0 : item.RealAmount1)
                            +
                            ((item.EMP2 != idDetail) ? 0 : item.RealAmount2)
                            +
                            ((item.EMP3 != idDetail) ? 0 : item.RealAmount3)
                            +
                            ((item.EMP4 != idDetail) ? 0 : item.RealAmount4)
                        )
                    )
                    + '</td>'
                    + '<td>'
                    + ((Number(item.IsManual) == 1)
                        ? ConvertDateTimeUTC_DMY(item.Created)
                        : (''))
                    + '</td>'




                stringContent = stringContent
                    + '<tr class="vt-number" role="row">'
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
                    + ((Number(item.IsCard) == 1) ? (RenderReport_Detail_Get_CardName(item.ServiceID)) : (RenderReport_Detail_Get_ServiceName(item.ServiceID)))
                    + '</td>'
                    + '<td style="text-align:center;">' + (item.IsProduct == 1 ? "X" : "") + '</td>'
                    + '<td><span class="money_row">'
                    + formatNumber(
                        Math.ceil(
                            ((item.EMP1 != idDetail) ? 0 : item.RealAmount1)
                            +
                            ((item.EMP2 != idDetail) ? 0 : item.RealAmount2)
                            +
                            ((item.EMP3 != idDetail) ? 0 : item.RealAmount3)
                        )
                    )

                    + '</td>'
                    + '<td>' + ConvertDateTimeUTC_DMY(item.Created) + '</td>'

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
function RenderReport_Detail_Get_CardName(id) {
    try {
        return report_CardData[id]
    }
    catch (ex){
        return "";
    }
}
//#endregion