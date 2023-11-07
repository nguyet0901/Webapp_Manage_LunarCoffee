//#region // Master
function WareDetail_Setting(currentid, StockType, ViewDetail) {
   
    if (ViewDetail == 1) {
        $("#DetailImport").hide()
        $("#thRemoveProduct").hide();
    }
    else $("#DetailImport").css("display", "flex")


    $("#ExpiredDay").flatpickr({
        dateFormat: 'd-m-Y',
        enableTime: false
    });
    $("#Detail_Date").flatpickr({
        dateFormat: 'd-m-Y H:i',
        enableTime: true,
        time_24hr: true,
        maxDate: new Date(),
        defaultDate: new Date()
    });
    $('#txtWare_Amount').divide();
    switch (StockType) {
        case 1: {
            $('#ware_des').hide();
            $('#reason_export').hide();
            $('#import_amount').show();
            $('#stock_property').show();
            $('#stock_itemamount').show();
            //$('#stock_infoamount').show();
            $('#stock_infoamount').css("display","flex");
            $('#stock_totalamount').show();
            $('#stock_expiredday').show();
            $('#wa_header_import').show();
            $('#wa_header_change').hide();
            $('#wa_header_export').hide();
        }
            break;
        case 2: {
            $('#ware_des').hide();
            $('#reason_export').show();
            $('#import_amount').hide();
            $('#stock_property').show();
            $('#stock_itemamount').hide();
            $('#stock_infoamount').hide();
            $('#stock_totalamount').hide();
            $('#stock_expiredday').hide();
            $('#wa_header_import').hide();
            $('#wa_header_change').hide();
            $('#wa_header_export').show();
        }
            break;
        case 3: {
            $('#ware_des').show();
            $('#reason_export').hide();
            $('#import_amount').hide();
            $('#stock_property').hide();
            $('#stock_itemamount').hide();
            $('#stock_infoamount').hide();
            $('#stock_totalamount').hide();
            $('#stock_expiredday').hide();
            $('#wa_header_import').hide();
            $('#wa_header_change').show();
            $('#wa_header_export').hide();

        }
            break;
    }
    let HeightListProduct = $("#StockInfo").outerHeight() - $("#StockProductList").find('.card-header').outerHeight();
    $("#TableStockProduct").height(HeightListProduct - 32);

    let HeightProductItem = $("#ProductItem").outerHeight() - 55;
    $("#TableProduct").height(HeightProductItem);

    if (Number(currentid) != 0) {
        //$('#btnDeleteDetail').show();
        $('#btnPrintDetail').show();
    }
    ItemDetail_Refresh();
}
function WareDetail_Event() {
    //$('#dtContentDetail tbody ').on('click', '.buttonDeleteClass', function (e) {
    //    let idProduct = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
    //    ItemDetail_DeleteRow(idProduct)
    //    e.preventDefault();
    //});
    $('#StockProductBody').on('click', '.buttonDeleteClass', function (e) {
        let idProduct = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        ItemDetail_DeleteRow(idProduct)
        e.preventDefault();
    });
    //$('#dtContentDetail tbody ').on('click', '.buttonEditClass', function (e) {
    //    let idProduct = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
    //    ItemDetail_EditRow(idProduct);
    //    e.preventDefault();
    //});
    $('#StockProductBody').on('click', '.buttonEditClass', function (e) {
        let idProduct = Number($(this).closest('tr')[0].childNodes[0].innerHTML);
        ItemDetail_EditRow(idProduct);
        WareDetail_FocusEditProduct();
    });

    $("#dtContentProductBody").on("click", '.itemProduct', function (e) {
        e.preventDefault();
        let id = $(this).attr("data-id");
        $("#productDetail").dropdown("refresh");
        $("#productDetail").dropdown("set selected", id.toString());
        WareDetail_FocusEditProduct();
    })

    $('.input-group .remove.icon').on('click', function (e) {
        $(this).parent().siblings().val("");
        e.stopPropagation();
    });

    $("#Header_collapse").on("click", function () {
        let id = $(this).attr("data-id");
        if ($("#" + id).length) {
            $("#" + id).toggleClass("hidden")
        }
    })


    $('#searchingProduct').keyup(function () {
        if ($(this).val().length > 0) $(".btn_clear").removeClass('opacity-1');
        $("#wareSearch .fa-search").hide();
        $("#wareSearch .spinner-border").show();
        clearTimeout(data_TimerOnchange);
        data_TimerOnchange = setTimeout(function (e) {
            WareDetail_Search(xoa_dau($('#searchingProduct').val().toLowerCase()).trim());
        }, 500);
    });
}
function WareDetail_UpdateLastLock() {
    let WareIDChoose = Number($('#DetailWareID').dropdown('get value'));
    let date_Lock = DataComboWare.filter(word => word["ID"] == WareIDChoose);
    let lastestLock = '';
    if (date_Lock && date_Lock.length > 0 && "01-01-1900 00:00" != date_Lock[0].LastetLock) {
        lastestLock = date_Lock[0].LastetLock;
    }
    else {
        lastestLock = '';
    }
    $("#lock_date_detail").val(lastestLock);

    let Wareto = Number($('#DetailWareID_To').dropdown('get value'));
    date_Lock = DataComboWare.filter(word => word["ID"] == Wareto);
    lastestLock = '';
    if (date_Lock && date_Lock.length > 0 && "01-01-1900 00:00" != date_Lock[0].LastetLock) {
        lastestLock = date_Lock[0].LastetLock;
    }
    else {
        lastestLock = '';
    }
    $("#lock_date_detail_To").val(lastestLock);


    $("#Detail_Date").flatpickr({
        dateFormat: 'd-m-Y H:i',
        enableTime: true,
        time_24hr: true,
        minDate: lastestLock,
        maxDate: new Date(),
        defaultDate: new Date()
    });

}

function WareDetail_Search(text) {

    if (text == "") {
        ItemDetail_RenderProductList(data_product, "dtContentProductBody");
    }
    else {
        let data = data_product.filter((word) => xoa_dau(word["Name"].toLowerCase()).includes(text));
        ItemDetail_RenderProductList(data, "dtContentProductBody");
        ColorSearchFilterText(text, "itemProduct");
    }

    $("#wareSearch .fa-search").show();
    $("#wareSearch .spinner-border").hide();
}

function WareDetail_FocusEditProduct() {
    $('#BodyMain').scrollTo($("#ProductItem").offset().top - 50);
    $("#ProductItem").addClass("border-active");
    setTimeout(function () {
        $("#ProductItem").removeClass("border-active");
    }, 2000)
}
//#endregion