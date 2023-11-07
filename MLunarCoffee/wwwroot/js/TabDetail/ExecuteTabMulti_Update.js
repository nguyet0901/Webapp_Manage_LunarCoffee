
function Tab_LoadData_Update () {
    if (TabCurrentID != "0" && TabCurrentID != "") {
        $("#checkboxUsingService").hide();
        AjaxLoad(url = "/Customer/Service/TabMulti/?handler=LoadDataDetail"
            , data = {'currentid': TabCurrentID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                let dataTab = JSON.parse(result);
                if (dataTab) {
                    CurrentDetailTab_ = dataTab.Table;
                    if (CurrentDetailTab_ != undefined && CurrentDetailTab_.length != 0) {
                        validating_unitprice = 1;
                        tab_changing_flag = 0;
                        final_Discount = Number(CurrentDetailTab_[0].Discount_Amount_Doctor);
                        final_Discount_per = Number(CurrentDetailTab_[0].DiscountForCus);
                        final_Discount_CTKM = Number(CurrentDetailTab_[0].Discount_Amount);
                        final_Discount_CTKM_per = Number(CurrentDetailTab_[0].Discount_Percent);
                        final_PriceRoot = Number(CurrentDetailTab_[0].Price_Root);
                        final_PriceDiscounted = Number(CurrentDetailTab_[0].Price_Discounted);
                        current_PriceDiscounted = Number(CurrentDetailTab_[0].Price_Discounted);
                        final_Discount_Voucher = Number(CurrentDetailTab_[0].Vourcher_Amount);
                        final_Discount_Voucher_per = Number(CurrentDetailTab_[0].Vourcher_Percent);
                        final_Discount_CusGroup_per = Number(CurrentDetailTab_[0].Discount_CusGroup_Percent);
                        final_Discount_CusGroup = Number(CurrentDetailTab_[0].Discount_CusGroup_Amount);
                        final_Discount_Card = Number(CurrentDetailTab_[0].Discount_Amount_Card);
                        final_Discount_CusMem_per = Number(CurrentDetailTab_[0].Discount_CusMem_Percent);
                        final_Discount_CusMem = Number(CurrentDetailTab_[0].Discount_CusMem_Amount);

                        final_PointUsed = Number(CurrentDetailTab_[0].PointUsed);
                        final_DiscountPointUsed = Number(CurrentDetailTab_[0].Discount_Point);

                        final_vatper = Number(CurrentDetailTab_[0].Vatper);
                        final_vatamount = Number(CurrentDetailTab_[0].Vatamount);
                        $("#vat_per").dropdown("clear");
                        $("#vat_per").dropdown("refresh");
                        $("#vat_per").dropdown("set selected", final_vatper);
                        $('#vat_amount').val(final_vatamount);

 
                        $('#tabpoint_used').val(CurrentDetailTab_[0].PointUsed);
                        $("#tabpoint_used").prop("disabled", true);
                        
                        $("#Telesale").dropdown("clear");
                        $("#Telesale").dropdown("refresh");
                        $("#Telesale").dropdown("set selected", CurrentDetailTab_[0].Telesale);

                        $("#Telesale2").dropdown("clear");
                        $("#Telesale2").dropdown("refresh");
                        $("#Telesale2").dropdown("set selected", CurrentDetailTab_[0].Telesale2);
 
                        $("#Consult1").dropdown("clear");
                        $("#Consult1").dropdown("refresh");
                        $("#Consult1").dropdown("set selected", CurrentDetailTab_[0].Consult1);
                        $("#Consult2").dropdown("refresh");
                        $("#Consult2").dropdown("set selected", CurrentDetailTab_[0].Consult2);
                        $("#Consult3").dropdown("refresh");
                        $("#Consult3").dropdown("set selected", CurrentDetailTab_[0].Consult3);
                        $("#Consult4").dropdown("refresh");
                        $("#Consult4").dropdown("set selected", CurrentDetailTab_[0].Consult4);
                        $("#KTV").dropdown("refresh");
                        $("#KTV").dropdown("set selected", CurrentDetailTab_[0].KTV);
                        $("#PatientID").dropdown("refresh");
                        $("#PatientID").dropdown("set selected", CurrentDetailTab_[0].Patient);
                        $("#dateCreatedExt.flatpickr.detail").flatpickr({
                            dateFormat: 'd-m-Y H:i',
                            enableTime: true,
                            defaultDate: new Date(CurrentDetailTab_[0].CreatedDate)
                        });

                        // Service   // Teeth
                        let TeethChoosing = CurrentDetailTab_[0].TeethChoosing.toString();
                        let TeethType = Number(CurrentDetailTab_[0].TeethType);
                        let TeethDenture = Number(CurrentDetailTab_[0].TeethDenture);
                        currentUnitTab = CurrentDetailTab_[0].Unit;
                        CurrentService_Choose = CurrentDetailTab_[0].Service_ID;
                        let data = data_service_root.filter(function (item) {
                            if (Number(item.ID) == Number(CurrentService_Choose)) return true;
                            else return false;

                        });
                        if (data != undefined && data != null && data.length == 1) {
                            $('#tabservice_input').val(data[0].Name);
                        }
                        OnChange_Services(TeethChoosing)
                        $("#txtEachTabNote").val(formatHTML(CurrentDetailTab_[0].Note))
                        Load_Teeth_By_Chossing_Service(0, TeethType, TeethDenture, 0);
                        Tab_LoadData_Update_To_Teeth(TeethChoosing);

                        // Quantity
                        $('#Tab_Service_Quantity').val(Number(CurrentDetailTab_[0].Quantity));
                        $('#service_timetotreat').val(Number(CurrentDetailTab_[0].TimeToTreatment));
                        // Unit Price
                        final_PriceUnit = Number(CurrentDetailTab_[0].Price_Unit);
                        $('#service_unit_price').val(final_PriceUnit)

                        // Check using
                        if (Number(CurrentDetailTab_[0].IsChoose) == 1) $("#IsCheckUsing").prop("checked", true);
                        else $("#IsCheckUsing").prop("checked", false);

                        // Selft Discount
                        $("#DiscountAmountPer").dropdown("refresh");
                        if (final_Discount_per != 0) {
                            $("#DiscountAmountPer").dropdown("set selected", 1);
                            $('#txtDiscountAmountPer').val(final_Discount_per);
                        }
                        else {
                            $("#DiscountAmountPer").dropdown("set selected", 2);
                            $('#txtDiscountAmountPer').val(final_Discount);
                        }

                        $("#ReasonDiscountID").dropdown("refresh");
                        $("#ReasonDiscountID").dropdown("set selected", CurrentDetailTab_[0].ReasonDiscountID);


                        // Discount CTKM
                        Tab_Detail_Loading_Program_By_Service(data_service_root
                            , data_discount
                            , CurrentDetailTab_[0].Service_ID, "ccbdiscountTabCombo");
                        $("#discountTabCombo").dropdown("refresh");
                        $("#discountTabCombo").dropdown("set selected", CurrentDetailTab_[0].Discount_ID);


                        // Voucher
                        Tab_Detail_Loading_Program_By_Service(data_service_root
                            , data_voucher
                            , CurrentDetailTab_[0].Service_ID, "ccbDiscountVoucher");
                        $("#DiscountVoucherID").dropdown("refresh");
                        $("#DiscountVoucherID").dropdown("set selected", CurrentDetailTab_[0].Voucher_ID);
                        $("#DiscountVoucherSeries").val(CurrentDetailTab_[0].Voucher_Series)


                        // Group
                        Tab_Detail_Loading_Program_By_Service(data_service_root
                            , data_customer_group
                            , CurrentDetailTab_[0].Service_ID, "ccbDiscountCustomerGroupID");
                        $("#DiscountCustomerGroupID").dropdown("refresh");
                        $("#DiscountCustomerGroupID").dropdown("set selected", CurrentDetailTab_[0].Discount_CusGroup_ID);

                         // member
                        Tab_Detail_Loading_Program_By_Service(data_service_root
                            , data_customer_mem
                            , CurrentDetailTab_[0].Service_ID, "ccbDiscountCustomerMemberID");
                        $("#DiscountCustomerMemberID").dropdown("refresh");
                        $("#DiscountCustomerMemberID").dropdown("set selected", CurrentDetailTab_[0].Discount_CusMem_ID);

                        // tiền thanh toán tối thiểu
                        $('#min_payment').val(CurrentDetailTab_[0].Min_Payment);

                        // Insurance
                        if (CurrentDetailTab_[0].IsInsurance == 1) {
                            $("#TabInsuranceID").dropdown("refresh");
                            $("#TabInsuranceID").dropdown("set selected", CurrentDetailTab_[0].InsuranceID);
                        }

                        // Installment
                        if (CurrentDetailTab_[0].IsInstallment == 1) {

                            $('#check_installment')[0].checked = true;
                            onTab_Install_Register();
                            _Installment_Term_Current = Number(CurrentDetailTab_[0].Installment_Term);
                            _Installment_Data_Current = CurrentDetailTab_[0].DataInstallment;
                            data_tab_installment = _Installment_Term_Current;
                            $('#installment_term').val(CurrentDetailTab_[0].Installment_Term);
                            $('#installment_firpaid').val(CurrentDetailTab_[0].PrepayAmount);
                            $('#date_installment').val(CurrentDetailTab_[0].InstallmentDate);
                        }


                        // Free service
                        if (CurrentDetailTab_[0].IsFree == 1) {
                            $("#ReasonFreeID").dropdown("refresh");
                            $("#ReasonFreeID").dropdown("set selected", CurrentDetailTab_[0].ReasonFree_ID);
                        }

                        $("#SourceService").dropdown("refresh");
                        $("#SourceService").dropdown("set selected", CurrentDetailTab_[0].ID_Source_CustService)

                        current_card_used = Number(CurrentDetailTab_[0].final_CardUsing);
                        data_card_using = (CurrentDetailTab_[0].data_card_using != "") ? (JSON.parse(CurrentDetailTab_[0].data_card_using)) : [];
                        for (i = 0; i < data_card_using.length; i++) {
                            let __id = data_card_using[i].CardID;
                            if (data_tab_card[__id] != undefined) {
                                data_tab_card[__id].ValueIni = data_tab_card[__id].ValueIni + data_card_using[i].Amount;
                                data_tab_card[__id].ValueUsed = data_tab_card[__id].ValueUsed + data_card_using[i].Amount;
                            }
                        }
                        for (i = 0; i < data_card_using.length; i++) {
                            let __id = data_card_using[i].CardID;
                            $('#card_detail_' + __id)[0].checked = true;

                        }
                        tab_changing_flag = 1;
                        validating_unitprice = 0;
                        Genre_PaymentReview();
                    }
                    else {
                        Genre_PaymentReview();
                    }

                    Check_Edit_Condition_Tab(CurrentDetailTab_[0].AmountPaid
                        , CurrentDetailTab_[0].AmountCommission
                        , CurrentDetailTab_[0].ExportSale
                        , dataTab.Table1
                        , dataTab.Table2
                        , CurrentDetailTab_[0].IsProduct
                        , CurrentDetailTab_[0].Unit
                        , CurrentDetailTab_[0].CurrentPercent
                        , CurrentDetailTab_[0].DataInstallment
                        , CurrentDetailTab_[0].TreatIndex
                    );
                }
            }

        );

    }
    else {
        $('.bodyViewEdit .btnSave').show();
        if (UsingConfirmService == 1) {
            $("#checkboxUsingService").show();
            $('#IsCheckUsing')[0].checked = true;
        } else {
            $("#checkboxUsingService").hide();
            $('#IsCheckUsing')[0].checked = true;
        }
        OnChange_Using_Service();
    }
    Tab_Check_View();
}
function Tab_LoadData_Update_To_Teeth (TeethChoosing) {
    if (TeethChoosing != "") {
        $('#teethChoosing').show();
        $('#Tab_Service_Quantity').attr('readonly', true);
        var x = document.getElementsByClassName("checkTeeth");
        var teehchose = TeethChoosing.split(",")
        for (let i = 0; i < x.length; i++) {
            for (let j = 0; j < teehchose.length; j++) {
                if (x[i].value == teehchose[j]) {
                    x[i].checked = true;
                }
            }
        }
    }
}
function Check_Edit_Condition_Tab (paid, paidcom, exportsale, treat, labo, IsProduct, unitid, CurrentPercent, DataInstallment,TreatIndex) {
    if (Number(paid) != 0) {checking_tab_detail_paid = Number(paid);}
    if (Number(paidcom) != 0) {checking_tab_detail_paidcom = Number(paidcom);}
    checking_tab_detail_export_sale = Number(exportsale);
    checking_tab_detail_labo = labo;
    checking_tab_detail_treat = treat;

    if (Check_Edit_Condition_Tab_Disable_By_Installement(DataInstallment)) {

        $("#Tab_Service_Quantity").prop("disabled", true);
        $("#check_installment").attr("disabled", true);
        $("#installment_term").prop("disabled", true);
        $("#installment_firpaid").prop("disabled", true);
        $("#date_installment").prop("disabled", true);
        $('.checkTeeth').each(function () {
            $(this).prop('disabled', true);
        });
        $('#discountTabCombo').addClass('disabled');
        $('#DiscountVoucherID').addClass('disabled');
        $('#DiscountCustomerGroupID').addClass('disabled');
        $("#txtDiscountAmountPer").prop("disabled", true);
        $('#DiscountAmountPer').addClass('disabled');
        $('#ReasonFreeID').addClass('disabled');

    }
    else {
        if (checking_tab_detail_export_sale != 0
            || checking_tab_detail_labo.length != 0
            || checking_tab_detail_treat.length != 0) {

            if (checking_tab_detail_export_sale != 0) {

                $("#Tab_Service_Quantity").prop("disabled", true);
            }
            else {
                if (Number(IsProduct) == 0) {
                    if (sys_DentistCosmetic == "1") {
                        if (Number(unitid) == 1 || Number(unitid) == 2) {
                            Check_Edit_Condition_Tab_Disable_By_Labo(checking_tab_detail_labo);
                            Check_Edit_Condition_Tab_Disable_By_Treat(checking_tab_detail_treat);
                        }
                        dataServiceTab = data_service_root.filter(word => Number(word["Unit"]) == Number(unitid));
                        data_service_root = dataServiceTab;
                        checking_tab_changing_service = 1;
                    }
                    else {
                        if (TreatIndex != undefined && TreatIndex != 0) {
                            $("#tabmulti_treatedindex").html(Outlang["Da_dieu_tri"]+' : '+formatNumber(TreatIndex));

                        }
                        //$("#Tab_Service_Quantity").prop("disabled", true);
                    }
                    dataServiceTab = data_service_root.filter(word => word["IsProduct"] == "0");
                    data_service_root = dataServiceTab;
                }
                else {
                    dataServiceTab = data_service_root.filter(word => word["IsProduct"] == "1");
                    data_service_root = dataServiceTab;
                }
                //Render_Customer_Service(dataServiceTab, "cbbServiceTab");
            }
        }
    }
}
function Check_Edit_Condition_Tab_Disable_By_Installement (data) {
    try {

        let result = false;
        let _obj = JSON.parse(data);
        if (_obj != undefined && _obj.length > 0) {
            let _obj_install = _obj.filter(word => Number(word["paid"]) > 0);
            if (_obj_install != undefined && _obj_install.length > 0) {
                result = true;
            }
        }

        return result;
    }
    catch (ex) {
        return false;
    }
}
function Check_Edit_Condition_Tab_Disable_By_Labo (data) {
    let _is_disabled = 0;
    var x = document.getElementsByClassName("checkTeeth");
    let teethlabo, value;
    for (j = 0; j < data.length; j++) {
        teethlabo = data[j].TeethLabo;
        if (teethlabo != "") {
            if (teethlabo.substring(0, 1) != ",")
                teethlabo = ',' + teethlabo;
        }
        for (let i = 0; i < x.length; i++) {
            value = ',' + x[i].value + ',';
            if (teethlabo.includes(value)) {
                x[i].disabled = true;
                _is_disabled = _is_disabled + 1;
            }
        }
    }
    if (_is_disabled > 0) {
        $('#adult_tab_teeth_type').attr("disabled", true);
        $('#child_tab_teeth_type').attr("disabled", true);
        $('#no_select_tab_teeth_type').attr("disabled", true);
        //$('#merge_tab_teeth_type').attr("disabled", true);
    }
}

function Check_Edit_Condition_Tab_Disable_By_Treat (data) {
    let _is_disabled = 0;
    var x = document.getElementsByClassName("checkTeeth");
    let teethtreat, value;
    for (j = 0; j < data.length; j++) {
        teethtreat = data[j].TeethTreate;
        if (teethtreat != "") {
            if (teethtreat.substring(0, 1) != ",")
                teethtreat = ',' + teethtreat;
        }
        for (let i = 0; i < x.length; i++) {
            value = ',' + x[i].value + ',';
            if (teethtreat.includes(value)) {
                x[i].disabled = true;
                _is_disabled = _is_disabled + 1;
            }
        }
    }
    if (_is_disabled > 0) {
        $('#adult_tab_teeth_type').attr("disabled", true);
        $('#child_tab_teeth_type').attr("disabled", true);
        //$('#merge_tab_teeth_type').attr("disabled", true);
        $('#no_select_tab_teeth_type').attr("disabled", true);

    }
}
function Check_Edit_Condition_Tab_Amount (data) {
    let result = "0";
    if (TabCurrentID != "0") {
        let total = Number(data.Price_Discounted);
        if (checking_tab_detail_paid != -1 || checking_tab_detail_paidcom != -1) {
            if (total < checking_tab_detail_paid || total < checking_tab_detail_paidcom)
                result = "1";
        }
    }
    return result;
}