async function Render_PaymentRefund(data, id) {
    new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';           
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let tr = '<div class="d-flex"> '
                        + '<div class="my-auto">'
                        + '<div class="h-100">'
                        + '<h6 class="mb-0">' + item.TabName
                        + '</h6>'
                        + '<a class="text-sm text-body">' + GetDateTime_String_DMY(item.CreatedTab) + '</a>'
                        + '</div>'
                        + '</div>'
                        + AddCell_CollectMoney(item.TabID, item.TabPaid)
                        + '</div>'
                        + '<div class="ps-5 pb-3 pt-0 ms-3">'
                        + AddCell_PayCurrent(item.TabID, item.TabPaid, item.TabAmount)
                        + '</div>'
                    myNode.insertAdjacentHTML('beforeend', tr); 
                }
            } 
        }
        Payment_Refund_Event();
    })
}
function AddCell_CollectMoney(TabID, TabPaid) {
    resulf = '<div class="my-auto ms-auto ">'
        + '<span id="collect_amount_' + TabID + '" data-id="' + TabID + '" data-amount="' + TabPaid + '" class="badge bg-gradient-success my-auto collect_amount cursor-pointer">' + Outlang["Hoan_tien"] +'</span>'
        + '<span id="remove_amount_' + TabID + '" data-id="' + TabID + '" data-amount="' + TabPaid + '" class="badge bg-gradient-danger my-auto remove_amount cursor-pointer">' + Outlang["Hoan_tac"] +'</span>'
        + '</div>'
    return resulf;
}

function AddCell_PayCurrent(TabID, TabPaid, TabAmount) {
    let paydiv = '<div class="bg-gray-100 border-radius-lg p-2 my-2 pe-0">'
        + '<div class="row col-12 align-items-center font-weight-bold">'
        + '<div class="col-md-12 col-xl-6">'
        + '<p class="font-weight-bold text-sm text-dark my-auto ps-sm-2">' + Outlang["So_tien_hoan"] + '</p>'
        + '</div>'
        + '<div class="col-md-12 col-xl-6 pe-0">'
        + '<input data-id="' + TabID + '" data-tableft="'
        + TabPaid + '" id=payCurrent_' + TabID + '  class="form-control money tableftMoney"/>'
        + '</div>'
        + '</div>'
        + '</div>';
    let paidbeforediv = '<div class="bg-gray-100 border-radius-lg p-2 my-2 pe-0">'
        + '<div class="row col-12 align-items-center font-weight-bold">'
        + '<div class="col-md-12 col-xl-6">'
        + '<p class="font-weight-bold text-sm text-dark my-auto ps-sm-2">' + Outlang["Da_thanh_toan"] + '</p>'
        + '</div>'
        + '<div class="col-md-12 col-xl-6 pe-0">'
        + '<div  class="font-weight-bold text-sm text-dark ms-sm-auto mt-sm-0 money">' + formatNumber(TabPaid) + ' | ' + formatNumber(TabAmount) + '</div>'
        + '</div>'
        + '</div>'
        + '</div>';

    let canceldiv = '<div class="bg-gray-100 border-radius-lg p-2 my-2 pe-0">'
        + '<div class="row col-12 align-items-center font-weight-bold">'
        + '<div class="col-md-12 col-xl-6">'
        + '<p class="font-weight-bold text-sm text-dark my-auto ps-sm-2">' + Outlang["Khoa_dich_vu_sau_khi_hoan_tien"] + '</p>'
        + '</div>'
        + '<div class="col-md-12 col-xl-6 pe-0">'
        + '<div class="form-check">'
        + '<input data-id="' + TabID + '" id="checkPayment_' + TabID + '" class="form-check-input pr-2 checkPayment" type="checkbox">'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>';

    return paydiv + paidbeforediv + canceldiv
}

function Payment_Refund_Event() {
    $('.money.tableftMoney').divide();

    $('#dtTablePaymentDetail .collect_amount').bind('click', function () {
        let id = $(this).attr('data-id');
        let Amount = $(this).attr('data-amount');
        $('#payCurrent_' + id).val((Amount < 0 ? 0 : Amount));
        data_payment_customer[id].Amount = Amount;
        $('#collect_amount_' + id).hide();
        $('#remove_amount_' + id).show();
    });

    $('#dtTablePaymentDetail .remove_amount').bind('click', function () {
        let id = $(this).attr('data-id');
        data_payment_customer[id].Amount = 0;
        data_payment_customer[id].IsBlock = 0;
        $('#payCurrent_' + id).val(0)
        $('#collect_amount_' + id).show();
        $('#remove_amount_' + id).hide();
    });

    $('#dtTablePaymentDetail .tableftMoney').bind('change', function () {
        $("#save_refund").attr("disabled", true);
        let tabid = Number($(this).attr('data-id'));
        let checkCloseService = $("#checkPayment_" + tabid);
        let maxvalue = Number($(this).attr('data-tableft'));
        let value = this.value.replace(/,/g, '');
        if (!(!isNaN(parseFloat(value)) && isFinite(value))) {
            value = 0;
            $(this).val(value);
            checkCloseService.prop("checked", false);
            //checkCloseService.prop("disabled", false);
            checkCloseService.trigger('change');
        }
        else {
            value = Number(value);
            if (value > maxvalue) {

                document.getElementById("textShowMessage").innerHTML = Outlang["So_tien_hoan_lon_hon_so_tien_khach_dong"];
                $('#payCurrent_' + tabid).addClass('paidment_error')
                value = 0;
            }
            else if (value < 0) {
                value = 0;
                $('#payCurrent_' + tabid).addClass('paidment_error')
                document.getElementById("textShowMessage").innerHTML = Outlang["So_tien_nho_hon_0"];
            }
            else {
                $('#payCurrent_' + tabid).removeClass('paidment_error')
            }
            data_payment_customer[tabid].Amount = value;
            checkCloseService.prop("checked", true);
            //checkCloseService.prop("disabled", true);
            checkCloseService.trigger('change');
            CountAllAmount();
        }
    });
    $('#dtTablePaymentDetail .checkPayment').bind('change', function () {
        let id = $(this).attr('data-id');
        let checked = $(this).is(':checked');
        data_payment_customer[id].IsBlock = (checked) ? 1 : 0;
    });

}