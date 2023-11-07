//--Description: @Type = 1 Cust - Paid Service / Product
//--				  =	2 Cust - Refund Service / Product
//--   				  = 3 Cust - Paid Deposit
//--   				  =	4 Cust - Refund Deposit
//--   				  = 5 Cust - Paid Card
//--   				  =	6 Cust - Refund Card
//--   				  = 7 Cust - Paid Medicine
//--   				  = 8 Cust - Broker Commission
//--   				  = 9 Branch - Receipt Branch
//--				  = 10 Branch - Expense
var APIAccount_dataSetting = [];
//#region //Excute

async function APIAccount_ExcutedEach(dataMain, topic = '', action = 'save') {
    return new Promise(resolve => {
        let paras = APIAccount_generatePara(dataMain);
        let { objResult, dataVoucher } = paras;
        if (typeof objResult != 'undefined' && typeof dataVoucher != 'undefined' && Object.keys(objResult).length > 0) {
            let branchid = 0, custid = 0, isupdate = 0;
            let topicVoucher = "";
            branchid = dataMain[0].BranchID;
            custid = dataMain[0].CustID;
            isupdate = dataMain[0].IsUpdate;
            topicVoucher = topic == '' ? dataMain[0].Code : topic;

            AjaxJWT(url = "/api/Accounting/Excuted"
                , data = JSON.stringify({
                    "para": JSON.stringify(objResult),
                    "dataVoucher": JSON.stringify(dataVoucher),
                    "page": APIAccount_getpath(),
                    "branchid": branchid,
                    "custid": custid,
                    "topic": topicVoucher,
                    "action": action,
                    "isupdate": isupdate
                })
                , async = true
                , success = function (result) {
                    resolve(result);
                }
            );
        }
        else {
            if (typeof failurefunc === 'function') failurefunc();
        }
    })
}

async function APIAccount_Excute(currentID, type, action = 'save', beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc === 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {

            if (typeof syn_Accountbrand != 'undefined' && syn_Accountbrand && syn_Accountbrand != "") {
                AjaxJWT(url = "/api/Accounting/LoadData"
                    , data = JSON.stringify({
                        "currentID": currentID,
                        "type": type,
                        "action": action
                    })
                    , async = true
                    , success = function (result) {
                        if (result != "0") {
                            let datas = JSON.parse(result);
                            APIAccount_dataSetting = datas.DataSetting;
                            let APIAccount_dataMain = datas.DataMain;                            
                            APIAccount_ExcutedEach(APIAccount_dataMain, topic = '', action).then(r => {
                                if (r == "1") {
                                    if (typeof successfunc === 'function') successfunc();
                                }
                                else {
                                    if (typeof failurefunc === 'function') failurefunc();
                                }
                            });
                        }
                        else {
                            if (typeof failurefunc === 'function') failurefunc();
                        }
                        resolve();
                        if (typeof completefunc === 'function') completefunc();
                    }
                );
            }
        }, 100)
    })
}
async function APIAccount_ExcuteMulti(data, action = 'save', beforefunc, successfunc, failurefunc, completefunc) {
    if (typeof beforefunc === 'function') beforefunc();
    new Promise((resolve) => {
        setTimeout(() => {

            if (typeof syn_Accountbrand != 'undefined' && syn_Accountbrand && syn_Accountbrand != "") {
                AjaxJWT(url = "/api/Accounting/LoadDataMulti"
                    , data = JSON.stringify({
                        "data": data
                    })
                    , async = true
                    , success = function (result) {
                        if (result != "0") {
                            let datas = JSON.parse(result);
                            let { DataSetting, ...dataMains } = datas;
                            APIAccount_dataSetting = DataSetting; debugger
                            let objKeys = Object.keys(dataMains);
                            let accPromises = [];
                            let dateNow = new Date();
                            let topic = `SyncMulti_${ConvertDateTimeUTC_YMD(dateNow)}`;
                            if (dataMains && objKeys.length > 0) {
                                for (let i = 0; i < objKeys.length; i++) {
                                    let key = objKeys[i];
                                    let dataMain = dataMains[key];
                                    accPromises.push(APIAccount_ExcutedEach(dataMain, topic, action));
                                }
                                Promise.all(accPromises).then(v => { });
                                if (typeof successfunc === 'function') successfunc();
                            }
                            else {
                                if (typeof failurefunc === 'function') failurefunc();
                            }
                            let APIAccount_dataMain = datas.DataMain;
                            
                        }
                        else {
                            if (typeof failurefunc === 'function') failurefunc();
                        }
                        resolve();
                        if (typeof completefunc === 'function') completefunc();
                    }
                );
            }
        }, 100)
    })
}

//#endregion
//#region //Generate Parameter
function APIAccount_generatePara(dataMain) {
    try {
        let objResult = {};
        let dataVoucher = [];
        let itemFirst = {};
        for (let i = 0; i < dataMain.length; i++) {
            let itemMain = dataMain[i];
            if (itemMain.IndexDetail == 1) {
                dataVoucher.push({
                    ID: itemMain.ID, Type: itemMain.Type, MasterGuid: itemMain.MasterGuid
                })
                itemFirst = { ...itemMain };
            }
            for (let j = 0; j < APIAccount_dataSetting.length; j++) {
                let itemSet = APIAccount_dataSetting[j];
                let orderSelf = 0
                while (orderSelf < itemSet.CountSelf) {
                    APIAccount_rsGetNode(itemSet, objResult, countRec = 0, itemMain, itemFirst, orderSelf)
                    orderSelf++;
                }
            }
        }
        return {objResult, dataVoucher};
    }
    catch {
        return {};
    }
}

function APIAccount_rsGetNode(item, objRes, countRec = 0, itemMain, itemFirst, orderSelf, index) {

    if (item.Path == "") {
        APIAccount_setValue(objRes, item, itemMain, orderSelf)
    }
    else {
        let pr = JSON.parse(item.Path);
        if (countRec < item.LevelPath) {
            let indArr = item.IsMaster == 1 ? itemMain.IndexMaster : itemMain.IndexDetail;
            indArr = ((indArr - 1) * item.CountSelf + orderSelf)
            let field = pr[countRec];
            let objPara;
            if (objRes[field]) {
                objPara = objRes[field] ?? {};
            }
            else {
                let posPara = index != undefined ? index - 1 : indArr;
                objPara = objRes[posPara][field] ?? {};
            }
            if (countRec == item.LevelPath - 1 && Object.keys(objPara).length > 0) {
                if (objPara[indArr] == undefined) objPara[indArr] = {};
                let obj = objPara[indArr];
                APIAccount_setValue(obj, item, itemMain, itemFirst, orderSelf)
            }
            let settMast = APIAccount_dataSetting[APIAccount_dataSetting.findIndex(e => e.ParaName == field)];
            index = settMast.IsMaster == 1 ? itemMain.IndexMaster : itemMain.IndexDetail;
            //index = ((index - 1) * item.CountSelf + orderSelf)
            APIAccount_rsGetNode(item, objPara, countRec + 1, itemMain, itemFirst, orderSelf, index);
        }
    }

}


function APIAccount_setValue(obj, item, itemMain, itemFirst, orderSelf) {
    switch (item.TypeData) {
        case "string":
        case "number":
        case "boolean":
        case "datetime":
            if (obj[item.ParaName] == undefined)
                if ((item.IsMaster == 1 && itemMain.IndexDetail == 1) || item.UseDataFirst == 1 || (item.IsMaster == 0)) {

                    let data = item.UseDataFirst == 1 ? { ...itemFirst } : { ...itemMain };

                    if (item.MapField != '') {
                        obj[item.ParaName] = APIAccount_convertDType(data[item.MapField], item.TypeData, item.TypeItem);
                    }
                    else {
                        if (item.Conditions != '') {
                            let val = APIAccount_handleDyCondition(item, data, orderSelf);
                            obj[item.ParaName] = APIAccount_convertDType(val, item.TypeData, item.TypeItem);
                        }
                        else {
                            obj[item.ParaName] = APIAccount_convertDType(item.DefaultValue, item.TypeData, item.TypeItem);
                        }
                    }
                }
            break;
        case "array":
            switch (item.TypeItem) {
                case "object":
                    if (obj[item.ParaName] == undefined) {
                        obj[item.ParaName] = [{}];
                    }
                    else {
                        //if (obj[item.ParaName][indArr - 1] == undefined)
                        //    obj[item.ParaName][indArr - 1] = {}
                    }
                    break;
                case "string":
                case "number":
                case "boolean":
                case "datetime":
                    if (obj[item.ParaName] == undefined) {
                        obj[item.ParaName] = [];
                    }
                    if ((item.IsMaster == 1 && itemMain.IndexDetail == 1) || item.UseDataFirst == 1 || (item.IsMaster == 0)) {

                        let data = item.UseDataFirst == 1 ? { ...itemFirst } : { ...itemMain };
                        let values;
                        if (item.MapField != '') {
                            values  = data[item.MapField];
                        }
                        else {
                            if (item.Conditions != '') {
                                let val = APIAccount_handleDyCondition(item, data, orderSelf);
                                values = APIAccount_convertDType(val, item.TypeData, item.TypeItem);
                            }
                            else {
                                values = APIAccount_convertDType(item.DefaultValue, item.TypeData, item.TypeItem);

                            }
                        }
                        obj[item.ParaName].push(values);
                    }
                    break;
            }
    }
}

function APIAccount_convertDType(value, type, typeitem) {
    try {
        let res;
        switch (type) {
            case "string":
                res = value.toString();
                break;
            case "number":
                res = !isNaN(Number(value)) ? Number(value) : value;
                break;
            case "boolean":
                res = (String(value).toLowerCase() === 'true');
                break;
            case "datetime":
                res = APIAccount_convertDateType(value, type, typeitem);
                break;
            case "default":
                res = value;
                break;
        }
        return res;
    }
    catch {
        return value;
    }
}

function APIAccount_convertDateType(value, type, typeitem) {
    try {
        let res;
        if (type == 'datetime') {
            let d = new Date(value),
                mon = (d.getMonth() + 1) < 10 ? `0${(d.getMonth() + 1)}` : (d.getMonth() + 1),
                dd = d.getDate() < 10 ? `0${d.getDate()}` : d.getDate(),
                yy = d.getFullYear(),
                hh = d.getHours() < 10 ? `0${d.getHours()}` : d.getHours(),
                mm = d.getMinutes() < 10 ? `0${d.getMinutes()}` : d.getMinutes(),
                ss = d.getSeconds() < 10 ? `0${d.getSeconds()}` : d.getSeconds();
            switch (typeitem) {
                case "yyyy-mm-dd":
                    res = `${yy}-${mon}-${dd}`;
                    break;
                case "yyyy-mm-dd hh:mm:ss":
                    res = `${yy}-${mon}-${dd} ${hh}:${mm}:${ss}`;
                    break;
                case "dd-mm-yyyy":
                    res = `${dd}-${mon}-${yy}`;
                    break;
                case "dd/mm/yyyy":
                    res = `${dd}/${mon}/${yy}`;
                    break;
                case "dd-mm-yyyy hh:mm:ss":
                    res = `${dd}-${mon}-${yy} ${hh}:${mm}:${ss}`;
                    break;
                case "dd/mm/yyyy hh:mm:ss":
                    res = `${dd}/${mon}/${yy} ${hh}:${mm}:${ss}`;
                    break;
                case "":
                    res = value;
                    break;
            }
        }
        else {
            res = value;
        }
        return res;
    }
    catch {
        return value;
    }
}

function APIAccount_handleDyCondition(item, data, orderSelf) {
    try {
        let result = '';
        let conds = JSON.parse(item.Conditions);
        let $orderSelf = orderSelf;
        let $item = { ...data };
        if (typeof $item != 'undefined' && typeof $orderSelf != 'undefined') {
            for (let i = 0; i < conds.length; i++) {

                if (eval(conds[i].condi)) {
                    result = eval(conds[i].result);
                    break;
                }

            }
        }
        return result;

    }
    catch (e) {
        return '';
    }
}

function APIAccount_getVoucherType(type, typeinout) {
    let result = 0;
    switch (type) {
        case 1:
            result = typeinout == 1 ? 1 : 2;
            break;
        case 2:
            result = typeinout == 1 ? 3 : 4;
            break;
        case 3:
            result = typeinout == 1 ? 5 : 6;
            break;
        case 4:
            result = 7;
            break;
        case 5:
            result = 9;
            break;
        case 6:
            result = 10;
            break;
        case 9:
            result = 8;
            break;
    }
    return result;
}
//#endregion


function APIAccount_getpath() {
    try {
        return location.pathname;
    }
    catch (e) {
        return "";
    }
}