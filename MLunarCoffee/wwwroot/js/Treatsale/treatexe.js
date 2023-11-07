// #region // Event and fill row
function Met_EventRow (key) {
    $('#Met_warebody #row_' + key + ' .ui.dropdown').dropdown({
        allowCategorySelection: true,
        forceSelection: false,
        transition: "fade up"
    });
    $('#row_' + key + ' .numberExport').unbind('change').change(function () {
        let id = this.id.replace("numberExport_", "")
        data_product[id].number = (Number(this.value)).toFixed(2);
        Met_CountTotal(id);
    });
    $('#row_' + key + ' .metpackageNumber').unbind('change').change(function () {
        let id = this.id.replace("met_packageNumber_", "");
        let value = (this.value).trim();
        if (value == "") data_product[id].package = "";
        else {
            data_product[id].package = value;
        }
    });

    $('#row_' + key + ' .productExport').unbind('change').change(function () {
        let idcom = this.id
        let id = idcom.replace("productExport_", "")
        let productid = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
        data_product[id].productID = productid;
        Met_Fillunit(productid, id);
        if (IsPackageNumber == 1) {
            Met_Fillpackage(productid, id);
        }
    });

    $('#row_' + key + ' .unitProduct').unbind('change').change(function () {
        let idcom = this.id
        let id = idcom.replace("unitProduct_", "")
        data_product[id].unitID = Number($('#' + idcom).dropdown('get value')) ? Number($('#' + idcom).dropdown('get value')) : 0;
        Met_CountTotal(id);
    });
    $('#Met_warebody #row_' + key + ' .buttonDeleteClass').on('click', function (event) {
        let timespan = $(this).attr("data-id");
        delete data_product[timespan];
        $('#row_' + timespan).remove();
        event.stopPropagation();
    });
}
function Met_Fillunit (id, random) {
    $('#unitProduct_' + random).dropdown('clear');
    $('#unitProduct_' + random).dropdown("refresh");
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
        Load_Combo(_data, "cbbunitProduct_" + random, true);
        $('#unitProduct_' + random).dropdown("clear");
        $('#unitProduct_' + random).dropdown("refresh");
        $('#unitProduct_' + random).dropdown("set selected", Number(_data[0].ID));
    }

}
async function Met_FillData (key, value) {
 
    return new Promise((resolve, reject) => {
        let _data = DataProduct
        if (met_extype == 2) _data = _data.filter((word) => {return word.IsMedicine == 1})
        Load_Combo(_data, "cbbproductExport_" + key, true);
        $("#productExport_" + key).dropdown("refresh");
        $("#productExport_" + key).dropdown("set selected", value.productID);
        if (Number(value.productID) != 0) {
            Met_Fillunit(value.productID, key);
            $("#unitProduct_" + key).dropdown("refresh");
            $("#unitProduct_" + key).dropdown("set selected", value.unitID);
            if (IsPackageNumber == 1) {
                Met_Fillpackage(value.productID, key);
            }
        }
        
        $('#met_packageNumber_' + key).val(value.package);
        $('#packageNumber_' + key).val(value.package);
        resolve();
    });
}
function Met_Fillpackage (productid, id) {

    AjaxLoad(url = "/WareHouse/TreatmentSale/ExportDetail/?handler=LoadPackNum"
        , data = {
            'ProductID': productid
        }
        , async = true
        , error = null
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data && data.length != 0) {
                    for (let i = 0; i < data.length; i++) {
                        Met_DataPackage[data[i].ID] = data[i];
                        data[i].Name = data[i].Name;
                    }

                }
                Load_Combo(data, "cbbmet_package_" + id, true);
                $('#met_package_' + id).dropdown("clear");
                $('#met_package_' + id).dropdown("refresh");
                $('#met_package_' + id).unbind('change').change(function () {
                    let idcom = this.id
                    let id = idcom.replace("met_package_", "");
                    let val = $(this).dropdown('get value');
                    if (val != undefined && val != 0) {
                        $('#met_packageNumber_' + id).val(Met_DataPackage[val].Name);
                        $('#met_packageExpired_' + id).html(ConvertDateTimeUTC_DMY(Met_DataPackage[val].DateExpired));
                        data_product[id].package = Met_DataPackage[val].Name;
                    }
                    else {
                        $('#met_packageNumber_' + id).val('');
                        $('#met_packageExpired_' + id).html('');
                        data_product[id].package = '';
                    }
                });
            }
        }
        , sender = null
        , before = function (e) {
            $('#Met_packwait_' + id).show();
        }
        , complete = function (e) {
            $('#Met_packwait_' + id).hide();
        }
        , nolimit = 1
    );
}
//#endregion
//#region // Render Cell
function Met_Renproduct (randomNumber) {
    let resulf =
        `<div class="ui fluid search selection dropdown productExport form-control" id="productExport_${randomNumber}">
                <input type="hidden" />
                <input class="search" autocomplete="off" tabindex="0" />
                <div class="default text">eg .${Outlang["San_pham"]}</div>
                <div id="cbbproductExport_${randomNumber}" class="menu" tabindex="-1">
                </div>
            </div>`;
    return resulf;
}
function Met_Rennumber (randomNumber, value) {
    let resulf = `<input name="numberFloat" class="ps-2 numberExport form-control"
                id="numberExport_${randomNumber}" value="${value}" placeholder="eg .${Outlang["So_luong"]}" />`
    resulf = resulf;
    return resulf;

}
function Met_Renunit (randomNumber) {
    let resulf
        = `<div class="ui fluid search selection dropdown unitProduct form-control w-auto" id="unitProduct_${randomNumber}">
                <input type="hidden" />
                <input class="search" autocomplete="off" tabindex="0" /><div class="default text">eg .${Outlang["Don_vi"]}</div>
                <div id="cbbunitProduct_${randomNumber}" class="menu" tabindex="-1">
                </div>
            </div>`
    return resulf;

}
function Met_Renpackage (randomNumber, value) {
    if (IsPackageNumber == 0) return '';
    let resulf = `

                        <div class="ui fluid search selection dropdown packageNumber form-control w-auto" id="met_package_${randomNumber}">
                            <input type="hidden" />
                            <input class="search" autocomplete="off" tabindex="0" />
                            <div class="default text">eg .${Outlang["So_lo"]}</div>
                            <div id="cbbmet_package_${randomNumber}" class="menu" tabindex="-1">
                            </div>
                        </div>
                        <input class="ps-2 form-control metpackageNumber" id="met_packageNumber_${randomNumber}"  />
                        <span id="met_packageExpired_${randomNumber}" class="input-group-text"></span>

                    `;
    return resulf;

}
//#endregion