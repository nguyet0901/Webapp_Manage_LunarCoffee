
function onClickKeyComboTab_Search (event) {
    event.preventDefault();
    event.stopPropagation();
    if (dataServiceCombo != undefined && dataServiceCombo != null) {
        Render_Customer_Service_Combo_Div(DataService, dataServiceCombo, "tabservice_combo_table_Body");
        $('#tabservice_combo_result').addClass('active');
        Event_SearchRowComboService_Div();
    }
    else {
        $('#tabservice_combo_result').removeClass('active');
    }
}

function onKeyupTab_Combo_Search () {

    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
                let searchtext = $('#tabservice_input').val();
                let search = xoa_dau(searchtext).trim().toLowerCase();
                let data = dataServiceCombo.filter(function (item) {
                    let _name = xoa_dau(item.ComboName).toLowerCase();
                    if (_name.includes(search)) return true;
                    else return false;
                }).sort((a, b) => {
                    if (a.ComboName.toLowerCase().indexOf(search) > b.ComboName.toLowerCase().indexOf(search)) {
                        return 1;
                    } else if (a.ComboName.toLowerCase().indexOf(search) < b.ComboName.toLowerCase().indexOf(search)) {
                        return -1;
                    } else {
                        if (a.ComboName > b.ComboName)
                            return 1;
                        else
                            return -1;
                    }
                });
                if (data != undefined && data != null) {
                    Render_Customer_Service_Combo_Div(DataService, data, "tabservice_combo_table_Body");
                    $('#tabservice_combo_table_Body').show();
                    Event_SearchRowComboService_Div();
                }
                else {
                    $('#tabservice_combo_table_Body').hide();
                }
                ColorSearchFilterText_Combo(searchtext, '.seachRowComboServicediv');
                resolve()
            }
        )
    })

}

function Event_SearchRowComboService_Div () {

    $(".rowcombotabservice").on("click", function () {
        let _servicelist = '';
        let _selected_comboid = $(this).attr('data-value');
        let data = dataServiceCombo.filter(function (item) {
            if (Number(item.ID) == Number(_selected_comboid)) return true;
            else return false;

        });
        if (data != undefined && data != null && data.length == 1) {
            $('#tabservice_combo_input').val(data[0].ComboName);
            _servicelist = data[0].ListService;
            CurrentTabCombo_Choose = _selected_comboid
        }
        OnChange_ComboServices(_servicelist);
        $('#tabservice_combo_result').removeClass('active');
    })
}

function OnChange_ComboServices (_servicelist) {
    let dt_servicelist = JSON.parse(_servicelist);
    let _obj_ser = data_service_root.filter(word => dt_servicelist[word["ID"]]);
    if (_obj_ser != undefined && _obj_ser.length != 0) {
        $('#service_list_combo').show();
        Render_Customer_Service_Div_From_Combo(_obj_ser, dt_servicelist, "tabservice_from_combo_Body");
        //Event_Click_Selected_Service_FromCombo();
        Event_ClickDetailRowComboService_Div();
    }
    else {
        $('#service_list_combo').hide();
    }


}
function Event_ClickDetailRowComboService_Div () {
    $(".rowCombotabservice").on("click", function () {
        let _selectedid = $(this).attr('data-value');
        CurrentService_Choose = _selectedid;
        $(this).addClass("selected").siblings().removeClass("selected");
        isUsingTabComboService = 1;
        OnChange_Services();
        isUsingTabComboService = 1;
        CurrentServiceTabCombo_Choose = CurrentTabCombo_Choose;
        let _numdisper = $(this).attr('data-percent');
        let _numdisamount = $(this).attr('data-amount');
        let _numtimes = $(this).attr('data-times');
        if (Number(_numtimes) != 0 && sys_DentistCosmetic == 0) {
            $('#Tab_Service_Quantity').val(_numtimes);
        }
        if (Number(_numdisamount) != 0) {
            $("#DiscountAmountPer").dropdown("set selected", 2);
            $('#txtDiscountAmountPer').val(_numdisamount);
        }
        else {
            if (Number(_numdisper) != 0) {
                $("#DiscountAmountPer").dropdown("set selected", 1);
                $('#txtDiscountAmountPer').val(_numdisper);
            }
        }
    })
}
//function Event_Click_Selected_Service_FromCombo () {
//    $(".comboser_select").on("click", function () {
//        let _selected_id = $(this).attr('data-value');
//        if (_selected_id != CurrentService_Choose) {
//            CurrentService_Choose = _selected_id;
//            isUsingTabComboService = 1;
//            OnChange_Services();
//            isUsingTabComboService = 1;
//            SaveEachTab(1);
//        }
//        else {
//            isUsingTabComboService = 1;
//            SaveEachTab(1);
//        }
//    });
//}
