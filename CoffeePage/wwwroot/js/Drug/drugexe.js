// #region // Event and fill row
function DSD_EventRow (key) {
    $('#DSD_Tablebody #row_' + key + ' .ui.dropdown').dropdown({
        allowCategorySelection: true,
        forceSelection: false,
        transition: "fade up"
    });
    $('#row_' + key + ' .numberExport').unbind('change').change(function () {
        let id = this.id.replace("DSD_number_", "")
        data_product[id].number = (Number(this.value)).toFixed(2);
        DSD_CountTotalExport(id);
    });
    $('#row_' + key + ' .productExport').unbind('change').change(function () {
        let idcom = this.id
        let id = idcom.replace("DSD_product_", "")
        let productid = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
        data_product[id].productID = productid;
        DSD_Fillunit(productid, id);
        if (IsPackageNumber == 1) {
            DSD_Fillpackage(productid, id);
        }
    });
    $('#row_' + key + ' .packageNumber').unbind('change').change(function () {
        let id = this.id.replace("DSD_packageNumber_", "");
        let value = (this.value).trim();
        if (value == "") data_product[id].package = "";
        else {
            data_product[id].package = value;
        }
    });
    $('#row_' + key + ' .unitProduct').unbind('change').change(function () {
        let idcom = this.id
        let id = idcom.replace("DSD_unit_", "")
        data_product[id].unitID = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
        DSD_CountTotalExport(id);
    });

    $('#DSD_Tablebody #row_' + key + ' .buttonDeleteClass').on('click', function (event) {
        let timespan = $(this).attr("data-id");
        delete data_product[timespan];
        $('#row_' + timespan).remove();
        event.stopPropagation();
    });
}
async function DSD_FillData (key, value) {
    return new Promise((resolve, reject) => {
        let _data = DataProduct
        if (DSD_extype == 2) _data = _data.filter((word) => {return word.IsMedicine == 1})
        Load_Combo(_data, "cbbDSD_product_" + key, true);
        $("#DSD_product_" + key).dropdown("refresh");
        $("#DSD_product_" + key).dropdown("set selected", value.productID);
        if (Number(value.productID) != 0) {
            DSD_Fillunit(value.productID, key);
            $("#DSD_unit_" + key).dropdown("refresh");
            $("#DSD_unit_" + key).dropdown("set selected", value.unitID);

            if (IsPackageNumber == 1) {
                DSD_Fillpackage(value.productID, key, value.exPackageID);
            }
        }
        $('#DSD_packageExpired_' + key).val(value.package)
        $('#DSD_packageNumber_' + key).val(value.package)
        $('#DSD_number_' + key).val(value.number);
        resolve();
    });
}
function DSD_Fillunit (id, random) {
    $('#DSD_unit_' + random).dropdown('clear');
    $('#DSD_unit_' + random).dropdown("refresh");
    let _data = [];
    let pro = DataProduct.filter(word => word["ID"] == id);
    if (pro && pro.length > 0) {
        _data.push({"ID": pro[0].UnitID, "Name": pro[0].UnitName});
        let unit = DataUnitProduct.filter(word => word["ProductID"] == id);
        if (unit != undefined && unit.length > 0) {
            for (let i = 0; i < unit.length; i++) {
                _data.push({"ID": unit[i].UnitChange, "Name": unit[i].UnitChangeName});
            }
        }
        Load_Combo(_data, "cbbDSD_unit_" + random, true);
        $('#DSD_unit_' + random).dropdown("clear");
        $('#DSD_unit_' + random).dropdown("refresh");
        $('#DSD_unit_' + random).dropdown("set selected", Number(_data[0].ID));
    }

}
function DSD_Fillpackage(productid, id, exPackageID = 0) {
    AjaxLoad(url = "/WareHouse/DrugStore/DrugStoreDetail/?handler=LoadPackNum"
        , data = {
            'ProductID': productid,
            'WareID': DSD_WareCurrent,
            'MasterID': DSD_ExportID
        }
        , async = true
        , error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data && data.length != 0) {

                    for (let i = 0; i < data.length; i++) {
                        DSD_DataPackage[data[i].ID] = data[i];
                        data[i].Name = data[i].Name;
                    }
                }
                Load_Combo(data, "cbbDSD_package_" + id, true);
                $('#DSD_package_' + id).dropdown("clear");
                $('#DSD_package_' + id).dropdown("refresh");
                $('#DSD_package_' + id).unbind('change').change(function () {
                    let idcom = this.id
                    let id = idcom.replace("DSD_package_", "");
                    let val = $(this).dropdown('get value');
                    if (val != undefined && val != 0) {
                        $('#DSD_packageNumber_' + id).val(DSD_DataPackage[val].Name);
                        $('#DSD_packageExpired_' + id).html(ConvertDateTimeUTC_DMY(DSD_DataPackage[val].DateExpired));
                        data_product[id].package = DSD_DataPackage[val].Name;
                    }
                    else {
                        $('#DSD_packageNumber_' + id).val('');
                        $('#DSD_packageExpired_' + id).html('');
                        data_product[id].package = '';
                    }
                });
                $('#DSD_package_' + id).dropdown("set selected", Number(exPackageID ?? 0));
            }
        }
        , sender = null
        , before = function (e) {
            $('#package_wait_' + id).show();
        }
        , complete = function (e) {
            $('#package_wait_' + id).hide();
        }
        , nolimit = 1
    );
}
//#endregion
//#region // Render Cell
function DSD_Renunit (randomNumber) {
    let resulf
        = `<div class="ui fluid search selection dropdown unitProduct form-control w-auto" id="DSD_unit_${randomNumber}">
                <input type="hidden" />
                <input class="search" autocomplete="off" tabindex="0" /><div class="default text">eg .${Outlang["Don_vi"]}</div>
                <div id="cbbDSD_unit_${randomNumber}" class="menu" tabindex="-1">
                </div>
            </div>`
    return resulf;
}
function DSD_Renproduct (randomNumber) {
    let resulf =
        `<div class="ui fluid search selection dropdown productExport form-control" id="DSD_product_${randomNumber}">
                <input type="hidden" />
                <input class="search" autocomplete="off" tabindex="0" />
                <div class="default text">eg .${Outlang["San_pham"]}</div>
                <div id="cbbDSD_product_${randomNumber}" class="menu" tabindex="-1">
                </div>
            </div>`;
    return resulf;

}
function DSD_Rennumber (randomNumber, value) {
    let resulf = `<input name="numberFloat" class="ps-2 numberExport form-control"
                id="DSD_number_${randomNumber}" value="${value}" placeholder="eg .${Outlang["So_luong"]}" />`
    resulf = resulf;
    return resulf;
}
function DSD_Renpackage (randomNumber, timespan) {
    if (IsPackageNumber == 0) return '';
    let resulf = `

                        <div class="ui fluid search selection dropdown packageNumber form-control w-auto" id="DSD_package_${randomNumber}">
                            <input type="hidden" />
                            <input class="search" autocomplete="off" tabindex="0" />
                            <div class="default text">eg .${Outlang["So_lo"]}</div>
                            <div id="cbbDSD_package_${randomNumber}" class="menu" tabindex="-1">
                            </div>
                        </div>
                        <input class="ps-2 form-control packageNumber" id="DSD_packageNumber_${randomNumber}"  />
                        <span class="input-group-text">
                            <i style="display:none" id="package_expired_error_${timespan}" class="fas fa-exclamation-triangle text-danger fw-bold"></i>
                            <i style="display:none" id="package_expired_success_${timespan}" class="fa-check fas text-success fw-bold "></i>
                            <span id="DSD_packageExpired_${randomNumber}" class="ms-2"></span>
                        </span>

                    `;
    return resulf;
}
//#endregion