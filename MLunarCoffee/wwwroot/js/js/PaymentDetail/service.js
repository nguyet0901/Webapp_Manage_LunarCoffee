const { format } = require("path/posix");

function Payment_Detail_Render_Service (ServiceCatName, TeethChoosing, TeethType, createdby, created) {
    let obj = Fun_GetObject_ByID(DataUser, createdby);
    let name = (obj != null) ? obj.Name : 'unknown';
    let createdinfo = '<p class="text-sm text-dark d-inline ms-2">' + name + ' , ' + GetDateTime_String_DMY(created) + '</p>';
    let result = (TeethChoosing != '') ? Fun_GetTeeth_ByToken(DataTeeth, TeethChoosing, TeethType) : ServiceCatName;
    return '<p class="d-inline text-sm text-dark my-auto">' + result + createdinfo + '</p>';
}
function Payment_Detail_Render_Content (id, minpay, paid, totalamount) {

    let minpaydiv = '';
    if (minpay > 0) {
        minpaydiv =
            '<div class="d-sm-flex text-sm text-dark ms-auto my-auto px-2 py-1">'
        + '<p class="text-sm text-dark ms-auto my-auto">'
        + '<span class="text-sm text-dark my-auto pe-2">' + Outlang["Thu_toi_thieu"] + '</span>'
        + '<span class="fw-bold text-dark">' + formatNumber(minpay) + '</span>'
        + '</p>'
        + '</div>'
    }
    let result =
        '<div class="d-sm-flex text-sm text-dark ms-auto my-auto px-2 py-1">'
        + '<p class="text-sm text-dark ms-auto my-auto">'
        + '<span class="text-sm text-dark my-auto pe-2">' + Outlang["Da_thanh_toan"] + '</span>'
        + '<span class="fw-bold text-dark">' + formatNumber(paid) + ' / ' + formatNumber(totalamount) + '</span>'
        + '</p>'
        + '</div>'
        + minpaydiv;
    return result;
}

async function Payment_Detail_Render (data, id, id2) {
    
    new Promise((resolove) => {
        var myNode = document.getElementById(id);
        var myNode_TreatInDay = document.getElementById(id2);
        if (myNode != null && myNode_TreatInDay != null) {
            myNode.innerHTML = myNode_TreatInDay.innerHTML = '';
            let isTreatInday = 0, isPaymentAll = 0;
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let item = data[i];
                    item.AmountTreat = item.PercentTreat != 0 ? Math.floor((item.TabAmount * item.PercentTreat) / 100) : 0;
           
                    let subtitle = Payment_Detail_Render_Service(item.ServiceCatName, item.TeethChoosing, item.TeethType, item.Created_By, item.CreatedTab);
                    let content = Payment_Detail_Render_Content(item.TabID, item.MinPayment, item.TabPaid, item.TabAmount);

                    let isinstall = ((item.IsInstallment == 1)
                        ? `<a data-id="${item.TabID}" class="ms-2 text-sm text-danger text-normal border-bottom installment">${Outlang["Tra_gop"]}</a>` : ``);
                    let isFree = (item.TabAmount == 0) ? 'tabFree' : "";
                    let tr = `
                    <div class="rowPaymentDetail ${isFree}" >
                        <div class="d-flex">
                            <div class="my-auto ms-3">
                                <div class="h-100">
                                    <h6 class="mb-0 fw-bold">
                                    ${item.TabName}${isinstall}
                                    </h6>
                                    ${subtitle}
                                </div>
                            </div>

                        </div>
                        <div class="pb-3 ms-3">
                            ${AddCell_PayCurrent(item.TabID, item.TabLeft, item.TabPaid, item.TabAmount, item.PercentTreat, item.AmountTreat, item.IsInstallment, item.PrepayAmount, item.HavingStage, item.ManualAmountAllDay)}
                            ${content}
                        </div>
                    </div>
                    `
                    if (item.TreatInDay == 1) {
                        myNode_TreatInDay.insertAdjacentHTML('beforeend', tr);
                        isTreatInday = 1;
                    }
                    else {
                        myNode.insertAdjacentHTML('beforeend', tr);
                        isPaymentAll = 1;
                    }
                }
            }
            if (isTreatInday == 1) $("#title_treat_inday").fadeIn(100);
            else $("#title_treat_inday").remove();
            if (isPaymentAll == 1) $("#title_treat_other").fadeIn(100);
        }
        Payment_Detail_Each_Event();
        resolove();
    })
}
function AddCell_CollectMoney (TabID, AmountTreat, TabAmount, TabPaid, HavingStage, ManualAmountAllDay) {
     
    let resulf = '';
    let AmountTreatLeft;
    let collectAllAmount = '';
    if (Number(ser_Main_Raise_Percent) == 1) {
        AmountTreatLeft = AmountTreat - TabPaid;
        AmountTreatLeft = ((AmountTreatLeft > 0) ? AmountTreatLeft : 0);
        collectAllAmount = '<span id="collect_all_amount_' + TabID + '" data-id="' + TabID + '" data-all-amount="' + (TabAmount - TabPaid) + '" class="btn badge bg-gradient-success my-auto me-2 collect_all_amount">' + Outlang["Sys_tat_ca"] + '</span>'
    }
    else {
 
        if (ser_sys_DentistCosmetic == 0 && HavingStage == 1) {
            // la tham my,nhung tinh theo buoc dieu tri
            AmountTreatLeft = AmountTreat - TabPaid;
            AmountTreatLeft = ((AmountTreatLeft > 0) ? AmountTreatLeft : 0);
        }
        else if (ser_sys_DentistCosmetic == 0 && sys_TreatManualamount == 1) {
            // la tham my,tien dieu tri tu nhap
            AmountTreatLeft = ManualAmountAllDay - TabPaid;
            AmountTreatLeft = ((AmountTreatLeft > 0) ? AmountTreatLeft : 0);
        }
        else {
            AmountTreatLeft = TabAmount - TabPaid;
        }
    }

    resulf = '<div class="my-auto ms-auto ">'
        + collectAllAmount
        + '<span id="collect_amount_' + TabID + '" data-id="' + TabID + '" data-amount="' + AmountTreatLeft + '" class="btn badge bg-gradient-success my-auto collect_amount">' + Outlang["Thu_tien"] + '</span>'
        + '<span id="remove_amount_' + TabID + '" data-id="' + TabID + '" class="btn badge bg-gradient-danger my-auto remove_amount">' + Outlang["Hoan_tac"] +'</span>'
        + '</div>'
    return resulf;
}
function AddCell_PayCurrent (randomNumber, TabLeft, TabPaid, TabAmount, PercentTreat, AmountTreat, IsInstallment, PrepayAmount, HavingStage, ManualAmountAllDay) {
    let collectamount = AddCell_CollectMoney(randomNumber, AmountTreat, TabAmount, TabPaid, HavingStage, ManualAmountAllDay);
    let paydiv = '<div class="input-group flex-nowrap mb-2">'

        + '<input data-left="' + Number(TabLeft) + '" data-treatfee="' + Number(AmountTreat) +'" class="form-control money payCurrent" type="text" id="payCurrent_' + randomNumber + '" ' + (sys_must_used_deposit == "1" ? "disabled" : "") + '/>'
        + '<div class="input-group-text">'
        + collectamount
        + '</div>'
        + '</div>';

    let depositdiv =
        '<div id="label_minus_' + randomNumber + '" class="label_minus">'
        + '<div class="d-sm-flex text-sm text-dark ms-auto my-auto px-2 py-1">'
        + '<p class="text-sm text-dark ms-auto my-auto">'
        + '<span class="text-sm text-dark my-auto pe-2">' + Outlang["Can_tru_tien_coc"] +'</span>'
        + '<span id="deposit_minus_' + randomNumber + '" class="fw-bold text-dark money">0</span>'
        + '</p>'
        + '</div>'
        + '</div>'

    let treatinday = '', treattext = '';;
    if (Number(ser_Main_Raise_Percent) == 1) {
        treatinday = PercentTreat + '% |' + formatNumber(AmountTreat);
        treattext = Outlang["Da_dieu_tri"]
    }
    else {
        treatinday = formatNumber(TabAmount - TabPaid);
        treattext = Outlang["Chua_thanh_toan"];
    }



    let needdiv =
        '<div class="d-sm-flex text-sm text-dark ms-auto my-auto px-2 py-1">'
        + '<p class="text-sm text-dark ms-auto my-auto">'
        + ((Number(ser_Main_Raise_Percent) == 1)
            ? ('<span class="payCurrentNotiTreat me-2 d-none cursor-pointer">'
                + '<i data-bs-toggle="tooltip" title="' + 'Số tiền thanh toán lớn hơn chi phí điều trị' + '"  class="text-gradient text-md text-danger fas fa-info-circle" ></i>'
                + '</span>')
            : '')
        + '<span class="text-sm text-dark my-auto pe-2">' + treattext + '</span>'
        + '<span class="fw-bold text-dark">' + treatinday + '</span>'
        + '</p>'
        + '</div>';
    let divInstallment = ((IsInstallment == 1 && TabPaid < PrepayAmount)
        ? (
              '<div class="d-sm-flex text-sm text-dark ms-auto my-auto px-2 py-1">'
            + '<p class="text-sm text-dark ms-auto my-auto">'
            + '<span class="text-sm text-dark my-auto pe-2">' + Outlang["Tra_truoc"] + '</span>'
            + '<span  class="fw-bold text-dark money">' + formatNumber(PrepayAmount - TabPaid) + '</span>'
            + '</p>'
            + '</div>'
 )
        : '')
    return paydiv + depositdiv + needdiv + divInstallment;

}
function Payment_Detail_Event () {

    
    if (ser_TotalDeposit <= 0) {
        $("#deposit_info").hide();
        ser_IsUsingDeposit = 0;
    }
    else {
        $("#deposit_info").show();
        if (sys_must_used_deposit == 1) {
            $("#deposit_info_check").hide();
        }
        else {
            $("#deposit_info_check").show();
        }
        if (sys_DepositNonPriority == 1) {
            $("#PaymentDeposit").prop('checked', false);
            ser_IsUsingDeposit = 0;
        }
        else ser_IsUsingDeposit = 1;
    }
    //$(".paid_detail_tabname,.paidpayment,.image.createddetailpaid,.timecreated,").popup({
    //    transition: "scale up",
    //    position: "top right"
    //});
   
}


function Checking_Add_Amount (tabid, amount) {
    $('#collect_amount_' + tabid).hide();
    $('#collect_all_amount_' + tabid).hide();
    $('#remove_amount_' + tabid).show();
    data_payment_customer[tabid].PayCurrent = amount;
    if (data_payment_customer[tabid].TabAmount == 0)
        data_payment_customer[tabid].IsPaidFree = 1
    if (ser_IsUsingDeposit == 1) Checking_Amount_Execute(tabid);
    else {
        $('#payCurrent_' + tabid).val(amount);
        Payment_Detail_CheckTreatFee(tabid);
    }
}

function Checking_Add_All_Amount(tabid, amount) {
    $('#collect_amount_' + tabid).hide();
    $('#collect_all_amount_' + tabid).hide();
    $('#remove_amount_' + tabid).show();
    data_payment_customer[tabid].PayCurrent = amount;
    if (data_payment_customer[tabid].TabAmount == 0)
        data_payment_customer[tabid].IsPaidFree = 1
    if (ser_IsUsingDeposit == 1) Checking_Amount_Execute(tabid);
    else {
        $('#payCurrent_' + tabid).val(amount);
        Payment_Detail_CheckTreatFee(tabid);
    }
}

function Checking_Remove_Amount (tabid) {
    $('#collect_amount_' + tabid).show();
    $('#collect_all_amount_' + tabid).show();
    $('#remove_amount_' + tabid).hide();
    data_payment_customer[tabid].PayCurrent = 0;
    data_payment_customer[tabid].MinusDeposit = 0;
    data_payment_customer[tabid].IsPaidFree = 0
    if (ser_IsUsingDeposit == 1) Checking_Amount_Execute(tabid);
    else {
        $('#payCurrent_' + tabid).val(0);
        Payment_Detail_CheckTreatFee(tabid);
    }
}
function Checking_Amount_Execute (tabid) {

    let data = Object.values(data_payment_customer);
    data = data.sort((a, b) => (a.TabID < b.TabID) ? 1
        : ((b.TabID < a.TabID) ? -1 : 0))
    if (data != undefined && data.length != 0) {
        let deposit_total = ser_TotalDeposit;
        let deposit_used = 0, deposit_left = 0, deposit_index = 0, amount_index = 0;
        for (let i = 0; i < data.length; i++) {
            if (data[i].MinusDeposit == 0 || data[i].TabID == tabid) {
                deposit_used = deposit_left = deposit_index = amount_index = 0;
                for (let j = 0; j < data.length; j++) {
                    if (i != j)
                        deposit_used = deposit_used + Number(data[j].MinusDeposit);
                }
                deposit_left = deposit_total - deposit_used;
                if (deposit_left >= Number(data[i].PayCurrent)) {
                    deposit_index = Number(data[i].PayCurrent);
                    amount_index = 0;
                }
                else {
                    deposit_index = deposit_left;
                    amount_index = Number(data[i].PayCurrent) - deposit_left;
                }
                data[i].MinusDeposit = deposit_index;
                data[i].PayCurrent = amount_index;

                $('#payCurrent_' + data[i].TabID).val(Number(amount_index));
                if (Number(deposit_index) != 0) {
                    $('#deposit_minus_' + data[i].TabID).html(formatNumber(deposit_index));
                    $('#label_minus_' + data[i].TabID).show();

                }
                else {
                    $('#label_minus_' + data[i].TabID).hide();
                }

                data_payment_customer[data[i].TabID].MinusDeposit = Number(deposit_index);
                data_payment_customer[data[i].TabID].PayCurrent = Number(amount_index);
                Payment_Detail_CheckTreatFee(data[i].TabID);
            }
        }
    }
    deposit_total = 0;
    for ([key, value] of Object.entries(data_payment_customer)) {
        deposit_total = deposit_total + Number(value.MinusDeposit);
    }
    $('#AmountDepositLeft').val(Number(ser_TotalDeposit) - Number(deposit_total));
}



async function Payment_Detail_RenderServicePayment(data, id) {
    var myNode = document.getElementById(id);
    if (myNode) {
        myNode.innerHTML = "";
        let stringContent = "";
        let totalSer = 0;
        for (let [key, value] of Object.entries(data)) {
            if (value && value.PayCurrent == 0 && value.IsPaidFree == 0) continue;
            let li =
                `<li class="border-0 d-flex mb-0 py-0 px-0" style="display: list-item !important;">
                    <div class="d-flex gap-3 w-100 ms-0">
                        <div>
                            <span class="text-sm text-dark ellipsis_one_line">${value.ServiceName}</span>
                        </div>
                        <div class="ms-auto">
                            <span class="text-sm text-dark ">${formatNumber(value.PayCurrent)}</span>
                        </div>
                    </div>
                </li>`
            stringContent += li;
            totalSer++;
        }
        myNode.innerHTML = stringContent;
        $("#pd_listserpaynumber").html(totalSer)
    }
}