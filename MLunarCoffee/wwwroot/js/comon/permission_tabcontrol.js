function Checking_TabControl_System_RebuildHeader (dataHeader, tableBodyId, dataper) {
    try {
        let tBody = $(`#${tableBodyId}`)
        let objHeader = {};
        for ([key, value] of Object.entries(dataHeader)) {
            let isAllow = true;
            if (value?.dataNamePer != undefined) {
                let td = `td[data-name="${value?.dataNamePer}"]`;
                let permisRow = tBody.find(`${td} ._tab_control_, ${td}._tab_control_, ._tab_control_[data-name="${value?.dataNamePer}"]`).first();
                let hasCheckPer = permisRow.length > 0 && (permisRow.data('checkper') ?? 0) == 1;
                let perTab = permisRow.length > 0 ? permisRow.data('tab') ?? "" : "";
                isAllow = !hasCheckPer || Checking_DataTab_IS_Allow(dataper, perTab) == 1;
            }

            let isShow = value?.isShow ?? true;

            if (isShow && isAllow) {
                objHeader[key] = value?.data ?? (value ?? [])
            }
        }
        return objHeader;
    }
    catch {
        return Object.entries(dataHeader).reduce((newObj, [key, value]) => {
            newObj[key] = value?.data ?? []
            return newObj;
        }, {})
    }
}
function Checking_TabControl_Permission_Exe (data) {
    //let obj = $('._tab_control_').not('[data-checkper="1"]');
    let obj = $('._tab_control_').filter(function () {
        return ($(this).data("checkper") != 1) || ($(this).data("tab-change") == 1);
    });
    obj.addClass('progressper');
    let p = Checking_TabControl_Permission_Exeasync(data, obj);
    p.then(function (v) { });
}
async function Checking_TabControl_Permission_Exeasync (data, objper) {
    new Promise((resolve, reject) => {
        if (data != undefined) {
            if (objper != undefined && objper.length != 0) {
                for (let i = objper.length - 1; i >= 0; i--) {
                    let checkDataTab = objper[i].attributes["data-tab"];
                    let checkDataStaff = objper[i].attributes["data-staff"];
                    if (checkDataTab != undefined) {
                        let datatab = objper[i].attributes["data-tab"].value;
                        let datastaff = (typeof(checkDataStaff) != 'undefined' ? objper[i].attributes["data-staff"].value : checkDataStaff);
                        let ispercontrol = true;
                        if (datatab != "" && datatab != "undefined") {
                            if (Checking_DataTab_IS_Allow(data, datatab) == 0 || (sys_TeleLevel != 0 && Master_PermissionTele == 1 && typeof (datastaff) != "undefined" && sys_userID_Main != datastaff)) {
                                switch (objper[i].nodeName.toLowerCase()) {
                                    case 'td':
                                        switch (datatab) {
                                            case "confirm_using_service":
                                                objper[i].innerHTML = "<i class='green check icon'></i>";
                                                break;
                                            case "phone_customer":
                                                if (objper[i].innerHTML.length > 0) {
                                                    objper[i].innerHTML = secret_phone(objper[i].innerHTML);
                                                } else {
                                                    objper[i].innerHTML = "";
                                                }
                                                break;
                                            default: objper[i].innerHTML = "";
                                        }
                                        break;
                                    case 'button':
                                        objper[i].remove();
                                        break;
                                    case 'a':
                                    case 'div':
                                    case 'p':
                                    case 'span':
                                    case 'label':
                                    case 'li':
                                        switch (datatab) {
                                            case "phone_customer":
                                                if (objper[i].innerHTML != "") {
                                                    if (objper[i].dataset.info != 0) {
                                                        objper[i].innerHTML = secret_phone(objper[i].innerHTML);
                                                    } else {
                                                        objper[i].remove();
                                                    }
                                                } else ispercontrol = false;
                                                break;
                                            default: objper[i].remove();
                                        }
                                        break;
                                    case 'i':
                                        objper[i].remove();
                                        break;

                                    case 'tr':
                                        objper[i].remove();
                                        break;
                                    case 'input':
                                        if (objper[i].value != "") {
                                            objper[i].setAttribute("class", "sercu form-control");
                                            objper[i].setAttribute("disabled", "true");
                                        } else ispercontrol = false;
                                        break;
                                    default:
                                        ispercontrol = false;
                                        break;
                                }
                            }
                        }
                        if (ispercontrol) objper[i].setAttribute("data-checkper", "1");
                    }
                }
            }
            objper.removeClass('progressper');
        }
        resolve(true);
    });
}

function Checking_DataTab_IS_Allow (data, datatab) {
    let result = 0;
    if (data != undefined && Object.values(data).length != 0) {
        if (data[datatab] != undefined) {
            result = 1;
        }
    }
    return result;
}
function secret_phone (phone) {
    let value = "*******";
    phone = removeHTML(phone);
    value = value + phone.substring(phone.length - 3, phone.length)
    return value;
}

function removeHTML (str) {
    var tmp = document.createElement("DIV");
    tmp.innerHTML = str;
    return tmp.textContent || tmp.innerText || "";
}
