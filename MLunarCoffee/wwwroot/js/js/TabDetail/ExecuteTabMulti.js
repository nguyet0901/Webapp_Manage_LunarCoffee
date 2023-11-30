////const { element } = require("angular");

// #region // Onchange Service
function OnChange_Services (teeth_in_update) {

    $("#helpchosing_program").hide();
    if (tab_changing_flag == 1 || teeth_in_update != undefined) {
        Tab_Detail_Format_Param();
        let service_id;
        service_id = CurrentService_Choose;
        let _discountVoucher = Tab_Detail_Loading_Program_By_Service(data_service_root, data_voucher, service_id, "ccbDiscountVoucher");
        let _discountCTKM = Tab_Detail_Loading_Program_By_Service(data_service_root, data_discount, service_id, "ccbdiscountTabCombo");
        let _discountCustGroup = Tab_Detail_Loading_Program_By_Service(data_service_root, data_customer_group, service_id, "ccbDiscountCustomerGroupID");
        let _discountCustMem = Tab_Detail_Loading_Program_By_Service(data_service_root, data_customer_mem, service_id, "ccbDiscountCustomerMemberID");
        console.log(data_discount)
        if (teeth_in_update == undefined) {
            Tab_Detail_HelpingChoosing_Program(_discountCTKM, _discountCustMem);
        }
        Tab_Detail_Installment_Show(data_service_root, service_id);
        Tab_Detail_Loading_Tag_Note_By_Service(data_note_service, service_id, "area_list_tag_note");
        Tab_Detail_FreeSevice_Show();
        Tab_Detail_Teeth_Show_Hide(service_id, teeth_in_update);
        if (service_id != 0) {
            Price_Branch_Flag = '';
            Price_Branch_Exchange = 0;
            let Service = data_service_root.filter(word => word["ID"] == service_id);
            if ( Service.length > 0 &&  Service[0].IsProduct == 1) {
                $('#service_timetotreat').prop('disabled', true);
            }
            else $('#service_timetotreat').prop('disabled', false);

            let Price, Price_Max;
            let Price_Branch_Exist = (Service.length > 0) ? (Service[0]["Price_Branch_Exist"]) : 0;
            $("#Tab_Service_UnitName").html(Service[0].UnitName);
            $("#Tab_Service_UnitName").attr('title', Service[0].UnitName);
            if (Number(Price_Branch_Exist) == 0) {
                Price = (Service.length > 0) ? (Service[0]["Price"]) : 0;
                Price_Max = (Service.length > 0) ? (Service[0]["Price_Max"]) : 0;
                unit_price_maximum = Price_Max;
                unit_price_minnimum = Price;
            }
            else {
                Price_Branch_Flag = (Service.length > 0) ? (Service[0]["Price_Branch_Flag"]) : '';
                Price_Branch_Exchange = (Service.length > 0) ? (Service[0]["Price_Branch_Exchange"]) : 0;
                Price = (Service.length > 0) ? (Service[0]["Price_Branch"]) : 0;
                Price_Max = (Service.length > 0) ? (Service[0]["Price_Branch_Max"]) : 0;
                Price = Price * Price_Branch_Exchange;
                Price_Max = Price_Max * Price_Branch_Exchange;
                unit_price_maximum = Price_Max;
                unit_price_minnimum = Price;
            }
            $('#service_unit_price').val(ser_ChoosePriceMax == '1' ? Price_Max : Price);
            if (Price != Price_Max) {
                let price_range = "(&nbsp" + formatNumber(Price) + "&nbsp~&nbsp" + formatNumber(Price_Max) + "&nbsp)"
                if (Price_Branch_Flag != '' && Price_Branch_Flag != 'vn') {
                    price_range = price_range + '<i class="' + Price_Branch_Flag + ' flag price_serice_flag"></i>'
                    price_range = price_range + '<span class="price_serice_exchange">' + formatNumber(Price_Branch_Exchange) + '</span>'
                }
                $('#service_unit_price_name').html(price_range);
                $('#service_unit_price').attr('readonly', false);
                $('#service_unit_price').css('background-color', 'white');
            }
            else {
                $('#service_unit_price_name').html('');
                $('#service_unit_price').attr('readonly', true);
                $('#service_unit_price').css('background-color', '#e6e6e6');
            }
            $('#vat_chkusing').prop('checked', false);
      
        }
        Tab_Detail_Card_Changing_ByService();
        Tab_Count_Price_For_All();

        return false;
    }
}

function Tab_Detail_Format_Param () {
    $('#context2 .item[data-tab]').removeClass("tab_noti");
}

function Tab_Detail_Teeth_Show_Hide (ServiceTab, teeth_in_update) {

    if (sys_DentistCosmetic == 1 && ServiceTab != 0) {
        let _obj_ser = data_service_root.filter(word => word["ID"] == ServiceTab)[0];
        if (_obj_ser == undefined) return;
        currentUnitTab = Number(_obj_ser.Unit);
        currentUnitTypeTab = _obj_ser.UnitType // Kiểm tra đơn vị thuộc răng hoặc từng hàm => Hiển thị biểu đồ răng.
        data_tab_current_is_product = Number(_obj_ser.IsProduct);
        let TeethType = Number(_obj_ser.TeethType);
        if ((currentUnitTypeTab == 'teeth' || currentUnitTypeTab == 'eachtooth') && data_tab_current_is_product == 0 && teeth_in_update == undefined) {
            if (checking_tab_changing_service == 0) {
                $('#teethChoosing').show();
                $('#Tab_Service_Quantity').attr('readonly', true);
                $('#Tab_Service_Quantity').css('background-color', '#e6e6e6');
                $('#Tab_Service_Quantity').val(0);
                TM_CookieTeethRender("divteethcookie", function () {
                    TM_CookieTeethEvent();
                });
            }
            let TeethDenture = (currentUnitTab == 2) ? 1 : 0;
            Load_Teeth_By_Chossing_Service(checking_tab_changing_service, TeethType, TeethDenture);
        }
        else {
            Tab_Detail_Teeth_Hide();
        }
    }
}
function Tab_Detail_Teeth_Hide () {
    $('#Tab_Service_Quantity').attr('readonly', false);
    $('#Tab_Service_Quantity').css('background-color', 'white');
    $('#divteethFront').empty();
    $('#divteethBellow').empty();
    $('#divteethcookie').empty();
    $('#teethChoosing').hide();
    $('#Tab_Service_Quantity').val(1);
}
// #endregion

// #region // Onchange unit price
function Tab_Changing_Unit_Price () {
    if (validating_unitprice == 0) {
        validating_unitprice = 1;
        let currentUnitPrice = Number($('#service_unit_price').val() ? $('#service_unit_price').val() : 0);
        if (currentUnitPrice < unit_price_minnimum) $('#service_unit_price').val(unit_price_minnimum);
        if (currentUnitPrice > unit_price_maximum) $('#service_unit_price').val(unit_price_maximum);
        validating_unitprice = 0;
        Tab_Count_Price_For_All();
    }
}
// #endregion
// #region // Onchange service type
function OnChange_Services_Type () {

}

// #endregion
// #region // Onchange using service
function OnChange_Using_Service () {

    Tab_Detail_Card_Changing_ByService();
    Tab_Count_Price_For_All();
}
function OnChange_Help_Choosing_Program_CTKM () {
    if (help_program_changing_flag == 0) {
        help_program_changing_flag = 1;

        if ($('#IsCheckChossing_CTKM')[0].checked == true) {
            $("#IsCheckChossing_CUSMEM").prop("checked", false);
            // add class css
            let _id = $('#IsCheckChossing_CTKM')[0].dataset.id;
            $("#discountTabCombo").dropdown("refresh");
            $("#discountTabCombo").dropdown("set selected", _id);
            $("#tab_discount").addClass("tab_noti");
            if ($("#tab_customer_group").length != 0)
                $("#tab_customer_group").removeClass("tab_noti");
        }
        else {
            if ($("#tab_discount").length != 0)
                $("#tab_discount").removeClass("tab_noti");
            $('#discountTabCombo').dropdown('clear');
        }
        help_program_changing_flag = 0;
    }
}
function OnChange_Help_Choosing_Program_CUSMEM () {
    if (help_program_changing_flag == 0) {
        help_program_changing_flag = 1;
        if ($('#IsCheckChossing_CUSMEM')[0].checked == true) {
            $("#IsCheckChossing_CTKM").prop("checked", false);
            // add class css
            let _id = $('#IsCheckChossing_CUSMEM')[0].dataset.id;
            $("#DiscountCustomerMemberID").dropdown("refresh");
            $("#DiscountCustomerMemberID").dropdown("set selected", _id);
            $("#tab_customer_group").addClass("tab_noti");
            if ($("#tab_discount").length != 0)
                $("#tab_discount").removeClass("tab_noti");
        }
        else {
            if ($("#tab_customer_group").length != 0)
                $("#tab_customer_group").removeClass("tab_noti");
            $('#DiscountCustomerMemberID').dropdown('clear');

        }
        help_program_changing_flag = 0;
    }

}
// #endregion

// #region // Add multi service
function Current_Choosing_Service_Add (elementItem) {
    let key = (new Date()).getTime();
    Current_Service_Tab_Chossing[key] = elementItem;
    Render_Current_Selected_Service_Tab(key, elementItem, "part_service_temp_data");

    let data = JSON.parse(elementItem.data_card_using)
    for (i = 0; i < data.length; i++) {
        let _ID = data[i].CardID;
        let _Amount = data[i].Amount;
        data_tab_card[_ID].ValueIni = data_tab_card[_ID].ValueIni - _Amount;
    }
}
function Render_Current_Selected_Service_Tab (key, value, id) {

    var myNode = document.getElementById(id);
    if (myNode != null) {
        let discount = '', insurance = '', install = '', cardused, isfree = '', ischeck='',vat='';
        insurance = (value.IsInsurance == 1) ? `<span class="text-dark fw-normal ms-2">${Outlang["Bao_hiem"]}</span>` : '';
        install = (value.IsInstallment == 1) ? `<span class="text-dark fw-normal ms-2">${Outlang["Tra_gop"]}</span>` : '';
        isfree = (value.IsFree == 1) ? `<span class="text-dark fw-danger ms-2">${Outlang["Mien_phi"]}</span>` : '';
        cardused = (value.current_card_used != 0) ? `<span class="text-dark fw-normal ms-2">${Outlang["Sys_the"]} :${formatNumber(value.current_card_used)}</span>` : '';
        ischeck = (value.IsChoose == 1) ? `<i class="vtt-icon vttech-icon-check text-primary"></i>` : '';
        vat = (value.Vat_amount != 0) ? `<span class="text-dark fw-normal ms-2">${Outlang["VAT"]}</span> : ${formatNumber(value.Vat_amount)}` : '';
   
        if (value.IsFree == 0 && (value.Price_Root - value.current_card_used + value.Vat_amount) != value.Price_Discounted)
            discount = `<span class="text-danger fw-normal ms-2">${Outlang["C_Khau"]} : ${formatNumber(value.Price_Root - value.Price_Discounted - value.current_card_used + value.Vat_amount)} </span>`;
        let tr =
            '<li id=temporary_service_' + key + ' class="border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg temporary_service">'
            + '<div class="d-flex align-items-center">'
            + '<div class="">' + ischeck + '</div>'
            + '<div class="d-flex flex-column ps-2"><a data-id="' + key + '" class="mb-1 text-primary cursor-pointer fw-bold  text-sm edit_service">' + value.ServiceName + '</a>'
            + '<div class="mt-n2">'
            + '<span class="text-sm text-dark font-weight-bold">' + formatNumber(value.Price_Discounted) + '</span>'
            + discount + isfree + cardused + insurance + install + vat
            + '</div>'
            + '</div></div>'
            + '<div class="d-flex">'
            + '<button class="btn btn-link btn-icon-only btn-rounded btn-sm text-dark my-auto temp_ser_img_delete">'
            + '<i id="delete_temp_' + key + '" class="remove icon buttonDeleteClass text-sm text-danger"></i>'
            + '</button>'
            + '</div>'
            + '</li>'


        document.getElementById(id).innerHTML = document.getElementById(id).innerHTML + tr;
    }
    ServiceTem_Event();
    Genre_PaymentAllItem();
}

function ServiceTem_Event () {
    $('#part_service_temp_data .buttonDeleteClass').unbind().click(function (event) {
        let timespan = this.id.replace('delete_temp_', '');
        if (Current_Service_Tab_Chossing[timespan] != undefined) {
            let data = JSON.parse(Current_Service_Tab_Chossing[timespan].data_card_using)
            for (i = 0; i < data.length; i++) {
                let _ID = data[i].CardID;
                let _Amount = data[i].Amount;
                data_tab_card[_ID].ValueUsed = data_tab_card[_ID].ValueUsed - _Amount;
                data_tab_card[_ID].ValueLeft = data_tab_card[_ID].ValueLeft + _Amount;
                data_tab_card[_ID].ValueIni = data_tab_card[_ID].ValueIni + _Amount;
            }
            Tab_Detail_Card_Render(data_tab_card, "card_tab_detail_list");
            delete Current_Service_Tab_Chossing[timespan];
            $('#temporary_service_' + timespan).remove();
            Genre_PaymentAllItem();
            event.stopPropagation();
        }
    });
    $('#part_service_temp_data .edit_service').unbind().click(function (event) {
        let timespan = $(this).attr('data-id');
        let objedit = JSON.parse(JSON.stringify(Current_Service_Tab_Chossing[timespan]));
        if (Current_Service_Tab_Chossing[timespan] != undefined) {
            let data = JSON.parse(Current_Service_Tab_Chossing[timespan].data_card_using);
            if (data.length == 0) {
                delete Current_Service_Tab_Chossing[timespan];
                $('#temporary_service_' + timespan).remove();
                Genre_PaymentAllItem();
                event.stopPropagation();
                ServiceTem_Load(objedit);
            }

        }
    });
}
function ServiceTem_Load (obj) {
    tab_changing_flag = 0;
    validating_unitprice = 1;
    final_Discount = Number(obj.Discount_Amount_Doctor);
    final_Discount_per = Number(obj.discountForCust);
    final_Discount_CTKM = Number(obj.Discount_Amount);
    final_Discount_CTKM_per = Number(obj.Discount_Per);
    final_PriceRoot = Number(obj.Price_Root);
    final_PriceDiscounted = Number(obj.Price_Discounted);
    current_PriceDiscounted = Number(obj.Price_Discounted);
    final_Discount_Voucher = Number(obj.Discount_Voucher);
    final_Discount_Voucher_per = Number(obj.Discount_Voucher_Per);
    final_Discount_CusGroup_per = Number(obj.Discount_CusGroup_Percent);
    final_Discount_CusGroup = Number(obj.Discount_CusGroup_Amount);
    final_Discount_CusMem_per = Number(obj.Discount_CusMem_Percent);
    final_Discount_CusMem = Number(obj.Discount_CusMem_Amount);
     
    final_vatper = Number(obj.Vat_per); 
    final_vatamount = Number(obj.Vat_amount);  

    let TeethChoosing = obj.teethChoosing.toString();
    let TeethType = Number(obj.TeethType);
    let TeethDenture = Number(obj.TeethDenture);
    currentUnitTab = obj.Unit;

    CurrentService_Choose = obj.ServiceID;
    let data = data_service_root.filter(function (item) {
        if (Number(item.ID) == Number(CurrentService_Choose)) return true;
        else return false;

    });
    if (data != undefined && data != null && data.length == 1) {
        $('#tabservice_input').val(data[0].Name);
    }
    OnChange_Services(TeethChoosing);
    $("#txtEachTabNote").val(formatHTML(obj.Note));
    Load_Teeth_By_Chossing_Service(0, TeethType, TeethDenture, 0);
    Tab_LoadData_Update_To_Teeth(TeethChoosing);
    $('#Tab_Service_Quantity').val(Number(obj.Quantity));
    $('#service_unit_price').val(obj.Price_Unit);
    if (Number(obj.IsChoose) == 1) $("#IsCheckUsing").prop("checked", true);
    else $("#IsCheckUsing").prop("checked", false);
    $("#DiscountAmountPer").dropdown("refresh");
    let amountdis = Number(obj.Discount_Amount_Doctor);
    let perdis = Number(obj.discountForCust);
    if (perdis != 0) {
        $("#DiscountAmountPer").dropdown("set selected", 1);
        $('#txtDiscountAmountPer').val(perdis);
    }
    else {
        $("#DiscountAmountPer").dropdown("set selected", 2);
        $('#txtDiscountAmountPer').val(amountdis);
    }

    // Discount CTKM
    Tab_Detail_Loading_Program_By_Service(data_service_root
        , data_discount
        , obj.ServiceID, "ccbdiscountTabCombo");
    $("#discountTabCombo").dropdown("refresh");
    $("#discountTabCombo").dropdown("set selected", obj.Discount_ID);


    // Voucher
    Tab_Detail_Loading_Program_By_Service(data_service_root
        , data_voucher
        , obj.ServiceID, "ccbDiscountVoucher");
    $("#DiscountVoucherID").dropdown("refresh");
    $("#DiscountVoucherID").dropdown("set selected", obj.Discount_VoucherID);
    $("#DiscountVoucherSeries").val(obj.Discount_Voucher_Series)

    // Group
    Tab_Detail_Loading_Program_By_Service(data_service_root
        , data_customer_group
        , obj.ServiceID, "ccbDiscountCustomerGroupID");
    $("#DiscountCustomerGroupID").dropdown("refresh");
    $("#DiscountCustomerGroupID").dropdown("set selected", obj.Discount_CusGroup_ID);

    Tab_Detail_Loading_Program_By_Service(data_service_root
        , data_customer_mem
        , obj.ServiceID, "ccbDiscountCustomerMemberID");
    $("#DiscountCustomerMemberID").dropdown("refresh");
    $("#DiscountCustomerMemberID").dropdown("set selected", obj.Discount_CusMem_ID);


    $("#vat_per").dropdown("refresh");
    $("#vat_per").dropdown("set selected", obj.Vat_per);
    $('#vat_amount').val(obj.Vat_amount);
 

    // tiền thanh toán tối thiểu
    $('#min_payment').val(obj.Min_Payment);

    // Insurance
    if (obj.IsInsurance == 1) {
        $("#TabInsuranceID").dropdown("refresh");
        $("#TabInsuranceID").dropdown("set selected", obj.InsuranceID);
    }

    // Installment
    if (obj.IsInstallment == 1) {

        $('#check_installment')[0].checked = true;
        onTab_Install_Register();
        _Installment_Term_Current = Number(obj.Installment_Term);
        _Installment_Data_Current = obj.DataInstallment;
        data_tab_installment = _Installment_Term_Current;
        $('#installment_term').val(obj.Installment_Term);
        $('#installment_firpaid').val(obj.PrepayAmount);
        $('#date_installment').val(obj.InstallmentDate);
    }


    // Free service
    if (obj.IsFree == 1) {
        $("#ReasonFreeID").dropdown("refresh");
        $("#ReasonFreeID").dropdown("set selected", obj.ReasonFree_ID);
    }

    $("#SourceService").dropdown("refresh");
    $("#SourceService").dropdown("set selected", obj.SourceService)



    Genre_PaymentReview();
    tab_changing_flag = 1;
    validating_unitprice = 0;
}
function TM_Resetallfield () {
    CurrentService_Choose = 0;
    $('#tabservice_input').val('');
    $('#Tab_Service_Quantity').attr('readonly', false);
    $('#Tab_Service_Quantity').css('background-color', 'white');
    $('#divteethFront').empty();
    $('#divteethBellow').empty();
    $('#teethChoosing').hide();
    $('#Tab_Service_Quantity').val(1);
    unit_price_minnimum = 0;
    unit_price_maximum = 0;
    $('#service_unit_price_name').html('');
    $('#service_unit_price').val(0);
    $('#service_timetotreat').val(0);
    $("#vat_per").dropdown("clear");
    $("#vat_amount").val(0);

    $('#discountTabCombo').dropdown('clear');
    $('#DiscountVoucherID').dropdown('clear');
    $('#DiscountVoucherSeries').addClass("d-none");
    $('#DiscountVoucherSeries').val(0);
    $('#installment_term').val(12);
    $('#installment_firpaid').val(0);
    $('#date_installment').val(1);
    $('#txtDiscountAmountPer').val(0);
    $('#tabpoint_used').val(0);
    $('#tabpoint_having').html('');
 
    $('#txtEachTabNote').val("");
    document.getElementById("textShowMessage").innerHTML = '';
    $('#tabprogram_dicount').removeClass("hightlight");
    $('#check_installment')[0].checked = false;
    $('.installment_field').prop('disabled', true);

    $('#IsCheckChossing_CTKM')[0].checked = false;
    $('#IsCheckChossing_CUSMEM')[0].checked = false;
    $("#helpchosing_program").hide();
    $("#TabInsuranceID").dropdown("clear");
    $("#ReasonFreeID").dropdown("clear");

    var date = new Date();
    $("#dateCreatedExt").val(formatDateClient(date));
    $(".flatpickr.detail").flatpickr({
        dateFormat: 'd-m-Y H:i',
        enableTime: true,
        defaultDate: new Date()
    });
    final_PriceUnit = 0;
    final_PriceRoot = 0;
    final_Discount = 0;
    final_Discount_per = 0;
    final_Discount_CTKM = 0;
    final_Discount_CTKM_per = 0;
    final_Discount_Voucher = 0;
    final_Discount_Voucher_per = 0;
    final_PointUsed = 0;
    final_DiscountPointUsed = 0;
    // final_CardUsing = 0;
    current_card_used = 0;
    data_card_using = [];

    final_PriceDiscounted = 0;
    final_Discount_CusGroup = 0;
    final_Discount_CusGroup_per = 0;
    Price_Branch_Flag = '';
    Price_Branch_Exchange = 0;

}

// #endregion

// #region // Payment Area
function Genre_PaymentAllItem () {
    let price = 0,tax=0, final = 0;
    for ([key, value] of Object.entries(Current_Service_Tab_Chossing)) {
        price = price + value.Price_Root;
        final = final + value.Price_Discounted;
        tax = tax + value.Vat_amount;
    }
    $('#ttab_price').html(formatNumber(price));
    
    $('#ttab_dis').html(formatNumber(price - final + tax));

    if (tax != 0) {
        $('#ttab_final').html(`${formatNumber(final)} <br><span class="text-sm text-primary">${Outlang["VAT"]}:${formatNumber(tax)}</span>`);
    }
    else $('#ttab_final').html(formatNumber(final));

}
function Genre_PaymentReview () {
    $('#final_price').html('');
    let stringcontent = '';
 

    stringcontent = '<div class="d-flex justify-content-between">'
        + '<span class="mt-1 text-sm">' + Outlang["Gia_tien"] + '</span>'
        + '<span class="text-dark ms-4">' + formatNumber(final_PriceRoot) + '</span></div>'
        + Genre_PaymentReview_Detail(Outlang["Giam_theo_hang_khach_hang"], final_Discount_CusMem_per, final_Discount_CusMem)
        + Genre_PaymentReview_Detail(Outlang["Diem_tich_luy"], 0, final_DiscountPointUsed)
        + Genre_PaymentReview_Detail(Outlang["Giam_theo_nhom_khach_hang"], final_Discount_CusGroup_per, final_Discount_CusGroup)
        + Genre_PaymentReview_Detail(Outlang["Giam_theo_the_tra_truoc"], final_Discount_Card_per, final_Discount_Card)
        + Genre_PaymentReview_Detail(Outlang["Giam_ctkm"], final_Discount_CTKM_per, final_Discount_CTKM)
        + Genre_PaymentReview_Detail(Outlang["Voucher"], final_Discount_Voucher_per, final_Discount_Voucher)
        + Genre_PaymentReview_Detail(Outlang["Giam_chiet_khau"], final_Discount_per, final_Discount)
        + Genre_PaymentReview_Detail("VAT", final_vatper, final_vatamount)
        + Genre_PaymentReview_Detail_Card()
        + Genre_PaymentReview_install(data_tab_installment)
        + Genre_PaymentReview_insurance(data_tab_insurance)
        + '<hr class="horizontal dark my-2"><div class="d-flex justify-content-between">'
        + '<span class="mt-1 text-sm">' + Outlang["Thanh_tien"] + '</span>'
        + '<span class="text-dark font-weight-bold ms-4">' + formatNumber(final_PriceDiscounted) + '</span></div>'



    $('#final_price').html(stringcontent);
    Genre_PaymentReview_CardEvent();

}
function Genre_PaymentReview_Detail_Card () {
    let result = '';
    let manualclass = CardManual == 1 ? '' : 'd-none';
    if (current_card_used > 0) {
        result =
            `<div class="d-flex justify-content-between">
                <span class="mt-1 text-sm">${Outlang["Thanh_toan_bang_the_tra_truoc"]}</span>
                <span class="text-dark ms-4">
                    ${formatNumber(current_card_used)}
                </span>
            </div>
            `
        for (let i = 0; i < data_card_using.length; i++) {
            let _cardobj = data_tab_card[data_card_using[i].CardID];
            if (_cardobj != undefined) {
                let _cardname = _cardobj.Name;
                result = result
                    +
                    `<div class="${manualclass} my-2  me-n2 ms-0 ms-lg-5 align-items-center">
                        <span class="fst-italic text-dark text-xs">${_cardname}</span>
                        <input id="cardmanual_${data_card_using[i].CardID}" class="cardmanual mt-0 form-control me-n2   fs-6 text-md p-1 px-2 text-end  " type="text" value=${data_card_using[i].Amount}>
                        
                    </div>`;
            }
           
        }
    }
    return result;
}
function Genre_PaymentReview_CardEvent () {
    $('#final_price .cardmanual').divide();
    $("#final_price .cardmanual").unbind('change').change(function () {
        Tab_Count_Price_For_All();
    })
   
}
function Genre_PaymentReview_Detail (name, per, amount) {
    let _r = '';
    if (Number(amount) != 0 || Number(per) != 0) {
        _r = '<div class="d-flex justify-content-between">'
            + '<span class="mt-1 text-sm">' + name + '</span>'
            + '<span class="text-dark ms-4">'
            + ((per == 0) ? '' : '<span class="pe-2 text-primary">(' + per + '%)</span>') + formatNumber(amount)
            + '</span></div>'

    }
    return _r;
}

function Genre_PaymentReview_insurance (__val) {
    if (__val == '') return '';
    else {

        return '<div class="d-flex justify-content-between">'
            + '<span class="mt-1 text-sm">' + __val + '</span>'
            + '</div>'
    }
}

function Genre_PaymentReview_install (__val) {
    if (__val == '') return '';
    else {
        return '<div class="d-flex justify-content-between">'
            + '<span class="mt-1 text-sm"> ' + Outlang["Thang_tra_gop"] + __val + '</span>'
            + '</div>'

    }
}


// #endregion

// #region list not service
function Tab_Detail_Loading_Tag_Note_By_Service (data, service_id, id_area_tag) {
    // chỉ sử dụng khi thêm mới
    //if (TabCurrentID == "0") {
    Remove_All_Note_Tab();
    let data_note = data.filter(word => word["Service_ID"] == service_id);
    // nếu có nội dung ghi chú
    if (data_note && data_note.length > 0) {
        Render_List_TagNote_Service(data_note, id_area_tag);
        $('#' + id_area_tag).show();
    }
    else {
        document.getElementById(id_area_tag).innerHTML = '';
        $('#' + id_area_tag).hide();
    }
    //}
}

function Render_List_TagNote_Service (data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        let stringContent = '';
        for (var i = 0; i < data.length; i++) {
            let item = data[i];
            let tr = '<div class="form-check">'
                + '<input class="checkTagNote form-check-input pr-2" type="checkbox" id="checkTagNote_' + item.ID + '">'
                + '<label class="pe-2 font-weight-normal custom-control-label" for="checkTagNote_' + item.ID + '">' + item.Tag + '</label>'
                + '</div>'

            stringContent = stringContent + tr;
        }

        document.getElementById(id).innerHTML = stringContent;
    }
    Event_Element_TagNote_Service();
}

function Event_Element_TagNote_Service () {
    $('.checkTagNote').change(function () {
        let content_note = '';
        let count_note = 0;
        $('.checkTagNote').each(function () {
            if ($(this).is(":checked") == true) {
                let id = this.id.replace("checkTagNote_", "");
                let data_item = data_note_service.filter(word => word["ID"] == id);
                if (data_item && data_item.length > 0) {
                    content_note += data_item[0].Explain + '\n';
                    count_note++;
                }
            }
        })
        if (count_note != 0) {
            $("#remove_note_icon").show();
        }
        else $("#remove_note_icon").hide();
        $("#txtEachTabNote").val(content_note);
    });
}
function Remove_All_Note_Tab () {
    $("#txtEachTabNote").val("");
    $("#remove_note_icon").hide();
    if ($("#area_list_tag_note").innerHTML != "") {
        $('.checkTagNote').removeAttr('checked');
        $(".checkTagNote").removeAttr("disabled");
    }
}
// #endregion


//#region // Div Search
async function onClickKeyTab_Search (event) {
    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
                event.preventDefault();
                event.stopPropagation();
                if (data_service_root != undefined && data_service_root != null) {
                    let TabServicetype = Number($('#TabServicetype').dropdown('get value')) ? Number($('#TabServicetype').dropdown('get value')) : 0;
                    if (TabServicetype != 0) {
                        $('.rowtabservice').hide();
                        $('.rowtabservice.type_' + TabServicetype).show();
                    }
                    else $('.rowtabservice').show();
                    $('#tabservice_result').addClass("active");
                    Event_SearchRowService_Div();
                }
                else {
                    $('#tabservice_result').removeClass("active");

                }
                resolve()
            }
        )
    })



}
async function onKeyupTab_Search (search) {
    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
                let TabServicetype = Number($('#TabServicetype').dropdown('get value')) ? Number($('#TabServicetype').dropdown('get value')) : 0;
                if (TabServicetype != 0) {
                    $('.rowtabservice').addClass('d-none');
                    $('.rowtabservice.type_' + TabServicetype).removeClass('d-none');
                }
                else $('.rowtabservice').removeClass('d-none');

                let obj = $('.rowtabservice');

                if (obj != undefined && obj.length != 0) {
                    for (let i = 0; i < obj.length; i++) {
                        let dataset = obj[i].dataset;
                        let _id = dataset.value != undefined ? dataset.value : "0";
                        let _name = dataset.name != undefined ? dataset.name.toLowerCase() : "";
                        let _nameOther = dataset.nameOther != undefined ? dataset.nameOther.toLowerCase() : "";
                        let _service_code = dataset.code != undefined ? dataset.code.toLowerCase() : "";
                        let _short_name = dataset.shortname != undefined ? dataset.shortname.toLowerCase() : "";
                        if (Number(CurrentService_Choose) == Number(_id)
                            || _name.includes(search)
                            || _nameOther.includes(search)
                            || _short_name.includes(search)
                            || _service_code.includes(search)) {
                            obj[i].style.display = "table-row";
                        }
                        else {
                            obj[i].style.display = "none"
                        }
                    }
                }

                $("#tab_SearchContent .fa-search").show();
                $("#tab_SearchContent .spinner-border").hide();
                $("#tab_SearchContent .btnsear_clear").removeClass('d-none')
                $('#tabservice_result').addClass("active");
                Event_SearchRowService_Div();

                ColorSearchFilterText_Combo(search, '.seachRowServicediv');
                resolve()
            }
        )
    })

}

function Event_SearchRowService_Div () {
    $(".rowtabservice").unbind().click(function () {
        let _selectedid = $(this).attr('data-value');
        CurrentService_Choose = _selectedid;
        let data = data_service_root.filter(function (item) {
            if (Number(item.ID) == Number(_selectedid)) return true;
            else return false;

        });
        if (data != undefined && data != null && data.length == 1) {
            $('#tabservice_input').val(data[0].Name);
        }
        isUsingTabComboService = 0;
        OnChange_Services();
        $('#tabservice_result').removeClass("active");
    })
}
//#endregion

// #region // Execute
function SaveEachTab (continute) {
    
    if ($('#Treatment_Plan_Name').length) $('#Treatment_Plan_Name').css('background-color', 'white');
    if ($('#Treatment_Plan_Name').length && $('#Treatment_Plan_Name').val() == '') {
        $('#Treatment_Plan_Name').css('background-color', 'rgb(255 216 216)');
        return false;
    }

    if (continute == 0 && Number(TabCurrentID) == 0 && Object.keys(Current_Service_Tab_Chossing).length != 0) {
        Executing_Save_Current_Choosing_Service();
        return false;
    }

    document.getElementById("textShowMessage").innerHTML = "";
    Tab_Count_Price_For_All();
    let ServiceTab = 0;
    ServiceTab = CurrentService_Choose;
    let Telesale = Number($('#Telesale').dropdown('get value')) ? Number($('#Telesale').dropdown('get value')) : 0;
    let Telesale2 = Number($('#Telesale2').dropdown('get value')) ? Number($('#Telesale2').dropdown('get value')) : 0;
    let Consult1 = Number($('#Consult1').dropdown('get value')) ? Number($('#Consult1').dropdown('get value')) : 0;
    let Consult2 = Number($('#Consult2').dropdown('get value')) ? Number($('#Consult2').dropdown('get value')) : 0;
    let Consult3 = Number($('#Consult3').dropdown('get value')) ? Number($('#Consult3').dropdown('get value')) : 0;
    let Consult4 = Number($('#Consult4').dropdown('get value')) ? Number($('#Consult4').dropdown('get value')) : 0;
    let KTV = Number($('#KTV').dropdown('get value')) ? Number($('#KTV').dropdown('get value')) : 0;
    let Min_Payment = Number($('#min_payment').val()) ? Number($('#min_payment').val()) : 0;
    let service_timetotreat = Number($('#service_timetotreat').val()) ? Number($('#service_timetotreat').val()) : 0;
    let dateCreated = (ser_ChooseDateCustomer == "1") ? ($('#dateCreatedExt').val() ? $('#dateCreatedExt').val() : formatDate_to_ddmmyyyyHHMM(new Date())) : formatDate_to_ddmmyyyyHHMM(new Date());
    let quantity = $('#Tab_Service_Quantity').val() ? $('#Tab_Service_Quantity').val() : 0;
    let Patient = Number($('#PatientID').dropdown('get value')) ? Number($('#PatientID').dropdown('get value')) : 0;

    if (((Consult1 == Consult3) && (Consult1 + Consult3) != 0)
        || ((Consult1 == Consult4) && (Consult1 + Consult4) != 0)
        || ((Consult3 == Consult4) && (Consult3 + Consult4) != 0)) {
        document.getElementById("textShowMessage").innerHTML = Outlang["Tu_van_vien_bi_trung"];
    }
    $('#Consult1').css('background-color', 'white');


    $('#tabservice_input').css('background-color', 'white');

    $('#discountTabCombo').css('background-color', 'white');
    $('#Tab_Service_Quantity').css('background-color', 'white');
    $('#min_payment').css('background-color', 'white');
    if (Consult1 == "") {
        document.getElementById("textShowMessage").innerHTML = Outlang["Chon_cg/bs_tu_van"];
        $('#Consult1').css('background-color', 'rgb(255 216 216)');
    }
    if (ServiceTab == 0) {
        document.getElementById("textShowMessage").innerHTML = Outlang["Chon_dich_vu"];
        $('#tabservice_input').css('background-color', 'rgb(255 216 216)');

    }
 
    if (current_card_used < 0 || final_PriceDiscounted < 0) {
        document.getElementById("textShowMessage").innerHTML = Outlang["Tien_khong_duoc_nho_hon_0"];
    }
    if (quantity <= 0 || quantity > 10000) {
        document.getElementById("textShowMessage").innerHTML = Outlang["So_luong_lon_hon_0_va_nho_hon_10000"];
        $('#Tab_Service_Quantity').css('background-color', 'rgb(255 216 216)');
    }
    if ((Min_Payment < 0 || Min_Payment > final_PriceDiscounted) && Min_Payment != 0) {
        document.getElementById("textShowMessage").innerHTML = Outlang["Tien_thanh_toan_toi_thieu_khong_hop_le"];
        $('#min_payment').css('background-color', 'rgb(255 216 216)');
    }

    
    let _discountid = Number($('#discountTabCombo').dropdown('get value')) ? Number($('#discountTabCombo').dropdown('get value')) : 0;
    let _chdis = TM_CheckDispro(quantity, _discountid);
    if (!_chdis) {
        $('#discountTabCombo').css('background-color', 'rgb(255 216 216)');
        document.getElementById("textShowMessage").innerHTML = Outlang["Kiem_tra_du_lieu"];
    }

    let teethChoosing = "";
    let TeethType = 0;
    let TeethDenture = 0;
    if (sys_DentistCosmetic == 1 && (currentUnitTypeTab == 'teeth' || currentUnitTypeTab == 'eachtooth') && data_tab_current_is_product == 0) {
        var x = document.querySelectorAll("#divContentteeth .checkTeeth");
        for (let i = 0; i < x.length; i++) {
            if (x[i].checked) teethChoosing = teethChoosing + x[i].value + ",";
        }
        if ($("#adult_tab_teeth_type").is(":checked")) TeethType = 0;
        else if ($("#child_tab_teeth_type").is(":checked")) TeethType = 1;
        else TeethType = 2;
        TeethDenture = (currentUnitTab == 2) ? 1 : 0;
    }
    if (document.getElementById("textShowMessage").innerHTML == "") {
        let typeDiscountAmountPer = Number($('#DiscountAmountPer').dropdown('get value')) ? Number($('#DiscountAmountPer').dropdown('get value')) : 0;
        let valueDiscountAmountPer = Number($('#txtDiscountAmountPer').val() ? $('#txtDiscountAmountPer').val() : 0);
        let percent_minus = 0;
        let amount_minus = 0;
        if (typeDiscountAmountPer == 1) {percent_minus = valueDiscountAmountPer; amount_minus = (final_PriceRoot / 100) * valueDiscountAmountPer}
        else {percent_minus = 0; amount_minus = valueDiscountAmountPer;}

        var elementItem = {};
        elementItem.ServiceID = ServiceTab;
        elementItem.ServiceName = data_service_root.filter(word => word["ID"] == ServiceTab)[0]["Name"];
        elementItem.Note = $("#txtEachTabNote").val() ? $("#txtEachTabNote").val() : "";
        elementItem.Quantity = quantity;
        elementItem.discountForCust = percent_minus;
        elementItem.DiscountForCus_Amount = 0;
        elementItem.Discount_ID = _discountid;
        elementItem.ReasonDiscountID = Number($('#ReasonDiscountID').dropdown('get value')) ? Number($('#ReasonDiscountID').dropdown('get value')) : 0;
        elementItem.Price_Unit = final_PriceUnit;
        elementItem.Price_Root = final_PriceRoot;
        elementItem.Discount_Amount = final_Discount_CTKM;
        elementItem.Discount_Per = final_Discount_CTKM_per;
        elementItem.Discount_CusGroup_Amount = final_Discount_CusGroup;
        elementItem.Discount_CusGroup_Percent = final_Discount_CusGroup_per;
        elementItem.Discount_CusGroup_ID = Number($('#DiscountCustomerGroupID').dropdown('get value')) ? Number($('#DiscountCustomerGroupID').dropdown('get value')) : 0;

        elementItem.Discount_CusMem_Amount = final_Discount_CusMem;
        elementItem.Discount_CusMem_Percent = final_Discount_CusMem_per;
        elementItem.Discount_CusMem_ID = Number($('#DiscountCustomerMemberID').dropdown('get value')) ? Number($('#DiscountCustomerMemberID').dropdown('get value')) : 0;

        elementItem.Price_Branch_Flag = Price_Branch_Flag;
        elementItem.Price_Branch_Exchange = Price_Branch_Exchange;

        elementItem.Discount_Voucher = final_Discount_Voucher;
        elementItem.Discount_Voucher_Per = final_Discount_Voucher_per;

        elementItem.Discount_VoucherID = Number($('#DiscountVoucherID').dropdown('get value')) ? Number($('#DiscountVoucherID').dropdown('get value')) : 0;
        elementItem.Discount_Voucher_Series = Number($("#DiscountVoucherSeries").val()) ? Number($("#DiscountVoucherSeries").val()) : 0;

        elementItem.Vat_per = Number($('#vat_per').dropdown('get value')) ? Number($('#vat_per').dropdown('get value')) : 0;
        elementItem.Vat_amount = Number($("#vat_amount").val()) ? Number($("#vat_amount").val()) : 0;

        elementItem.Discount_Amount_Doctor = amount_minus;
        elementItem.Discount_Amount_Card = final_Discount_Card;
        elementItem.Price_Discounted = final_PriceDiscounted;
        // elementItem.final_CardUsing = final_CardUsing;
        elementItem.current_card_used = current_card_used;
        elementItem.data_card_using = JSON.stringify(data_card_using);
        elementItem.Unit = currentUnitTab;
        elementItem.Min_Payment = Min_Payment;
        elementItem.Telesale = Telesale;
        elementItem.Telesale2 = Telesale2;
        elementItem.Consult1 = Consult1;
        elementItem.Consult2 = Consult2;
        elementItem.Consult3 = Consult3;
        elementItem.Consult4 = Consult4;
        elementItem.KTV = KTV;
        elementItem.Patient = Patient;
        elementItem.dateCreated = dateCreated;
        elementItem.teethChoosing = teethChoosing;
        elementItem.TeethType = TeethType;
        elementItem.TeethDenture = TeethDenture;
        elementItem.Time = (new Date()).getTime();
        elementItem.SourceService = Number($("#SourceService").dropdown('get value')) ? Number($("#SourceService").dropdown('get value')) : 0;

        if ($('#check_installment')[0].checked == true) {
            elementItem.IsInstallment = 1;
            elementItem.InstallmentFirPaid = $('#installment_firpaid').val() ? $('#installment_firpaid').val() : 0;
            elementItem.InstallmentDay = $('#date_installment').val() ? $('#date_installment').val() : 0;
            elementItem.Installment_Term = $('#installment_term').val() ? $('#installment_term').val() : 0;
        }
        else {
            elementItem.IsInstallment = 0;
            elementItem.InstallmentDay = 0;
            elementItem.InstallmentFirPaid = 0;
            elementItem.Installment_Term = 0;
        }
        elementItem.jsonInstallment = _Installment_Data_Current;
        if (_Installment_Term_Current != elementItem.Installment_Term || final_PriceDiscounted != current_PriceDiscounted) {
            elementItem.jsonInstallment = Tab_Detail_Installment_Create_Json(
                elementItem.Installment_Term
                , (final_PriceDiscounted - elementItem.InstallmentFirPaid));
        } else {
            elementItem.jsonInstallment = "";
        }

        // dich vu free
        elementItem.IsFree = 0;
        let reason_free = Number($('#ReasonFreeID').dropdown('get value')) ? Number($('#ReasonFreeID').dropdown('get value')) : 0;
        elementItem.ReasonFree_ID = reason_free;

        if (reason_free != 0) {
            elementItem.IsFree = 1;
            elementItem.Price_Discounted = 0;
        }

        // sử dụng bảo hiểm
        elementItem.IsInsurance = 0;
        let _insurance = Number($('#TabInsuranceID').dropdown('get value')) ? Number($('#TabInsuranceID').dropdown('get value')) : 0;
        elementItem.Insurance_ID = _insurance;
        if (_insurance != 0) {
            elementItem.IsInsurance = 1;
        }

        let IsChoose = 1;
        if (document.getElementById("IsCheckUsing")) {
            if ($('#IsCheckUsing')[0].checked == true) {
                IsChoose = 1;
            } else {
                IsChoose = 0;
            }
        }
        elementItem.IsChoose = IsChoose;
        if (dataPointHaving != 0 && dataPointToMoney != 0) {
            elementItem.PointUse = final_PointUsed;
            elementItem.DiscountPointUsed = final_DiscountPointUsed;
        }
        else {
            elementItem.PointUse = 0;
            elementItem.DiscountPointUsed = 0;
        }

        if (sys_TreatManualIndex == 1) {
            let _timetotreat = service_timetotreat;
            if (!isNaN(_timetotreat) && _timetotreat != "" && _timetotreat > 0) {
                _timetotreat = Math.floor(_timetotreat);
                elementItem.TimeToTreat = _timetotreat
            }
            else elementItem.TimeToTreat = 0;
         
        }
        else {
            elementItem.TimeToTreat = 0;
       
        }
        $('#form3').form('validate form');
        if ($('#form3').form('is valid')) {
            if (continute == 1) {
                Current_Choosing_Service_Add(elementItem);
                if ($('#service_combo_' + CurrentService_Choose).length) {
                    $('#service_combo_' + CurrentService_Choose).addClass('rowservice_combo_selected');
                }
            }
            else {
                let result = Check_Edit_Condition_Tab_Amount(elementItem);
                if (result == "1") {
                    document.getElementById("textShowMessage").innerHTML = Outlang["Gia_dich_vu_phai_lon_hon_tong_tien_da_dong_va_tien_chi_hoa_hong"];
                    notiWarning(Outlang["Gia_dich_vu_phai_lon_hon_tong_tien_da_dong_va_tien_chi_hoa_hong"]);
                }
                else {
                    Current_Choosing_Service_Add(elementItem);
                    Executing_Save_Current_Choosing_Service();
                }
            }
            TM_CookieSet(teethChoosing, TeethType);
            TM_Resetallfield();
            Genre_PaymentReview();
        }
        $('#form3').Require_Validation();
    }
    return false;
}
function TM_CheckDispro (num, discountid) {
    if (TabCurrentID == 0) {

      
        let _da = data_discount.filter(function (el) {
            return el.ID==discountid
        });
        if (_da != undefined && _da.length == 1) {
            let _it = _da[0];
            if (_it.Numberuse == 1) {
                if (Number(num) >= Number(_it.Numberfrom) && Number(num) <= Number(_it.Numberto)) {
                    return true;
                }
                else return false;
            }
            else return true;
        } else return true;
    }
    else return true;
}
function Executing_Save_Patient_Record (TemplateID) {
    let ID = 0;
    if (TemplateID != 0) {
        AjaxLoad(url = "/Customer/Service/TabMulti/?handler=Executing_Patient_Record"
            , data = {
                "TemplateID": TemplateID,
                "CustomerID": ser_MultiTabCustomerID
            }
            , async = false
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result)[0];
                    ser_current_patient_record = data.ID;
                    patientRecordID = data.ID;
                    let e = {};
                    e.IsClose = data.IsClose;
                    e.TeamplateName = data.TeamplateName;
                    e.Created = data.Created;
                    e.Created_By = data.Created_By;
                    e.Image = data.Image;
                    ser_current_data_patient_record[data.ID] = e;
                    Render_Tab_List_Patient_Record_Add(data.ID, e, "tab_list_patient_record");
                    Tab_List_Customer_Patient_Record_Trigger(ser_current_patient_record);
                } else {
                    notiError_SW();
                }
            }
        );

    }
    return ID;
}
function Executing_Selected_Patient_Record () {
    let TemplateID = $('.patient_record_template.active').attr('data-id');
    let data = Template_PatientRecord[TemplateID];
    Executing_Save_Patient_Record(data.ID);
    Executing_Save_Current_Choosing_Service();
    Close_Patient_Record_Template();
}
function Render_Patient_Record_Template (data) {

    $('#list_patient_record_template').html('');
    let content = '';
    for (let i = 0; i < data.length; i++) {
        content += '<li data-id="' + data[i].ID + '" class="list-group-item border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg patient_record_template">'
            + '<div class="d-flex ms-2">'
            + '<div class="icon icon-shape icon-sm me-3 text-center">'
            + '<img class="avatar avatar-sm m-0" src="' + data[i].Image + '" alt="label-image">'
            + '</div><div class="d-flex flex-column">'
            + '<h6 class="text-dark text-sm mb-0">' + data[i].Name + '</h6>'
            + '</div>'
            + '</div><div class="d-flex">'
            + '<button class="btn btn-link btn-icon-only btn-rounded btn-sm text-dark icon-move-right my-auto"> '
            + '<i class="ni ni-bold-right" aria-hidden="true"></i>'
            + '</button>'
            + '</div>'
            + '</li>';
    }
    $('#list_patient_record_template').html(content);
    $('#tab_master').hide();
    $('#modal_select_patient_record').show();
    $('#modal_select_patient_record').on('click', '.patient_record_template', function () {
        $('.patient_record_template').removeClass('active');
        $(this).addClass('active');
    })
}
function Check_Patient_Record_Template () {
    let isExecuting = false;
    let TokentService = ',';
    for ([key, value] of Object.entries(Current_Service_Tab_Chossing)) {
        TokentService += value.ServiceID + ',';
    }

    AjaxLoad(url = "/Customer/Service/TabMulti/?handler=Checked_Patient_Record_Template"
        , data = {
            "TokentService": TokentService
        }
        , async = false
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "") {
                let data = JSON.parse(result);
                if (data.length == 1) {
                    Executing_Save_Patient_Record(data[0].ID);
                    isExecuting = true;
                } else if (data.length > 1) {
                    for (let i = 0; i < data.length; i++) {
                        Template_PatientRecord[data[i].ID] = data[i];
                    }
                    Render_Patient_Record_Template(data);
                    isExecuting = false;
                } else {
                    isExecuting = true;

                }
            }
        }
    );
    return isExecuting;
}
function Close_Patient_Record_Template () {

    $('#tab_master').show();
    $('#modal_select_patient_record').hide();
}
function Executing_Save_Current_Choosing_Service () {
    let data = [];
    if (TabCurrentID == 0 && Number(ser_sys_Patient_Record) == 1 && patientRecordID == 0 && Tab_Multi_Treat_Plan == 0) {
        if (Check_Patient_Record_Template() == false)
            return false;
    }
    for ([key, value] of Object.entries(Current_Service_Tab_Chossing)) {
        value.Patient = patientRecordID;
        data.push(value);
    }
    let plan_name = ($('#Treatment_Plan_Name').length) ? $('#Treatment_Plan_Name').val() : '';
    let dateBegin = ($('#date-begin').length) ? $('#date-begin').val() : '';
    let dateEnd = ($('#date-end').length) ? $('#date-end').val() : '';
    let comboID = (isUsingTabComboService == 1 ? CurrentServiceTabCombo_Choose : 0)
    var objexetab = data;
    AjaxLoad(url = "/Customer/Service/TabMulti/?handler=ExecuteData"
        , data = {
            'data': JSON.stringify(objexetab)
            , 'CustomerID': ser_MultiTabCustomerID
            , 'Changing': checking_tab_changing_service
            , 'Patient': patientRecordID
            , 'Plan': Tab_Multi_Treat_Plan
            , 'Plan_Name': plan_name
            , 'DateBegin': dateBegin
            , 'DateEnd': dateEnd
            , 'Plan_Note': ''
            , 'ComboID': comboID
            , 'CurrentID': TabCurrentID
        }
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                if (result == "2") {
                    notiError(Outlang["Dau_tien_phai_nhap_tien_su"]);
                }
                else if (result == "-1") {
                    notiError(Outlang["Sai_dinh_dang"] + " : " + Outlang["Voucher"]);
                }
                else if (result == "-2") {
                    notiError(Outlang["Sai_dinh_dang"] + " : " + Outlang["Sys_the"]);
                }
                else {
                    notiSuccess();
                    TabList_Service_Loading_After_Modified_Service(current_adding_new_plan);
                    if (typeof MainCust_AfterChangeCard === 'function') {
                        MainCust_AfterChangeCard(ser_MultiTabCustomerID, objexetab);
                    }

                    Tab_List_Service_Close_Detail();
                    let codes = result.split(",");
                    if (codes != undefined && codes.length != 0) {
                        for (let cc = 0; cc < codes.length; cc++) {
                            if (codes[cc] != undefined && codes[cc] != '')
                                syslog_cutser(TabCurrentID == 0 ? 'i' : 'u', codes[cc], ser_MultiTabCustomerID, '')
                        }
                    }
                }

            }
            else {
                notiError_SW();
            }
        }
        , sender = $("#btn_save_close_multi_tab")
    );

}
// #endregion

