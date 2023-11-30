////data_teeth_rule_type
//data_teeth_rule_type_customer
function Load_Teeth_By_Chossing_Service(changing_service, TeethType, isdenture, not_rule) {
    if (changing_service == 0) {

        if (not_rule == undefined || not_rule != 0) {
            let refre_teeth = Tab_Detail_Detect_Type_Teeth(data_teeth_rule_type, data_teeth_rule_type_customer);
            if (refre_teeth != -1) {
                TeethType = refre_teeth;
            }
        }
        $('#Tab_Service_Quantity').val(0);
        RenderTeethFront(dataTeeth, "divteethFront", TeethType, isdenture);
        RenderTeethBellow(dataTeeth, "divteethBellow", TeethType, isdenture);
    }
    if (Number(TeethType) == 0) $("#adult_tab_teeth_type").prop("checked", true);
    else if (Number(TeethType) == 1) $("#child_tab_teeth_type").prop("checked", true);
    else $("#merge_tab_teeth_type").prop("checked", true);

    $("#divContentteeth .checkTeeth").change(function () {

        let count = 0;
        var x = document.querySelectorAll("#divContentteeth .checkTeeth");
        for (let i = 0; i < x.length; i++) {
            if (x[i].checked) count = count + 1;
        }
        $('#Tab_Service_Quantity').val(count);
        Tab_Count_Price_For_All();
    });

    //Event Click Image Teeth
    $("#divContentteeth .imageTeeth").click(function () {
        let checkbox_siblings = $(this).siblings(".checkbox");
        let checkbox = checkbox_siblings.children(".checkTeeth");
        if (checkbox.is(':checked')) {
            checkbox.prop("checked", false).trigger("change");
        }
        else {
            checkbox.prop("checked", true).trigger("change");
        }
    })

    Tab_Check_View();
}


function onchange_tab_teeth_adult() {
    if (tab_changing_flag == 1) {
        tab_changing_flag = 0;
        if ($("#adult_tab_teeth_type").is(":checked")) {
            let _gum = 0;
            if (currentUnitTab == 2) _gum = 1;
            Load_Teeth_By_Chossing_Service(0, 0, _gum, 0);
        }
    }
    tab_changing_flag = 1;
}
function onchange_tab_teeth_baby() {
    if (tab_changing_flag == 1) {
        tab_changing_flag = 0;
        if ($("#child_tab_teeth_type").is(":checked")) {
            let _gum = 0;
            if (currentUnitTab == 2) _gum = 1;
            Load_Teeth_By_Chossing_Service(0, 1, _gum, 0);
        }
    }
    tab_changing_flag = 1;
}
function onchange_tab_teeth_no_select() {
    if (tab_changing_flag == 1) {
        tab_changing_flag = 0;
        if ($("#no_select_tab_teeth_type").is(":checked")) {
            currentUnitTab = 4;
            TeethType = 0;
            Tab_Detail_Teeth_Hide();
        }
    }
    tab_changing_flag = 1;
}


function onchange_tab_teeth_merge() {
    if (tab_changing_flag == 1) {
        tab_changing_flag = 0;
        if ($("#merge_tab_teeth_type").is(":checked")) {
            let _gum = 0;
            if (currentUnitTab == 2) _gum = 1;
            Load_Teeth_By_Chossing_Service(0, 2, _gum, 0);
        }
    }
    tab_changing_flag = 1;
}
function Tab_Detail_Detect_Type_Teeth(_rule, _info) {
    try {
        let result = -1;
        if (_rule != undefined && _rule.length > 0) {
            for (y = 0; y < _rule.length; y++) {
                let data_rule = JSON.parse(_rule[y].Data);
                let _type = _rule[y].Type;
                switch (Number(_type)) {
                    case 1:
                        {
                            result = Tab_Detail_Detect_Type_Teeth_Age(data_rule, _info);
                        }
                        break;
                    default: break;
                }
                if (result != -1) y = _rule.length;
            }
        }

        return result;
    }
    catch (ex)  {
        return -1;
    }
}
function Tab_Detail_Detect_Type_Teeth_Age(_detail, _info) {
    try {

        let result = -1;
        let _age = Number(_info[0].Year_Birth);
        for (l = 0; l < _detail.length; l++) {
            if (_age >= _detail[l].age_from && _age < _detail[l].age_to) {
                result = Number(_detail[l].target);
            }
            if (result != -1) l = _detail.length;
        }
        return result;
    }
    catch (ex) {
        return -1;
    }
}

//#region // Cookie

async function TM_CookieTeethRender (id, callback) {
    new Promise((resolve) => {
        try {
            setTimeout(() => {
                let _data = TM_CookieGet();
                var myNode = document.getElementById(id);
                if (myNode != null) {
                    if (_data && _data.length > 0) {
                        let count = 3;
                        let _content = '';
                        for (let i = _data.length - 1; i >= 0; i--) {
                            let item = _data[i];
                            count = count - 1;
                            if (count > 0) {
                                let tr =
                                    `<li class="nav-item d-block mt-2 w-100" role="presentation">
                                         <a class="cooselected nav-link cursor-pointer py-1" data-bs-toggle="pill" data-value="${item.ID}" role="tab">
                                             <span class="fw-bold pe-2 text-dark">${item.Number} ${Outlang["Sys_rang"]}</span>
                                             <span class="">${item.Name}</span>
                                         </a>
                                      </li>`;
                                _content = _content + tr;
                            }
                        }
                        if (_content != '') {
                            _content = `<span class="d-block text-dark text-sm">${Outlang["Chon_gan_day"]}</span>` + _content;
                        }
                        myNode.innerHTML = _content;
                    }
                }
                callback();
            }, 100)

        }
        catch (e) {

        }
    });
}

function TM_CookieSet (_teeth, _type) {
    try {
        if (_teeth != "") {
            let _coname = 'tmselected';
            let _cocur = getCookie(_coname);
            let _coval = [];
            let _ismatch = 0;
            if (_cocur != undefined && _cocur != "") {
                let _check = JSON.parse(_cocur);
                for (let ii = 0; ii < _check.length; ii++) {
                    if (_check[ii].ID == _teeth) _ismatch = 1;
                }
            }
            if (_ismatch == 0) {
                if (_cocur != undefined && _cocur != "") {
                    _coval = JSON.parse(_cocur);
                    if (_coval.length > 3) {
                        _coval = _coval.slice(1);
                    }
                }
                let _obj = TM_CookieTeethName(_teeth, _type);
                if (_obj != "" && _obj.Number != 0) {
                    _coval.push({"ID": _teeth, "Number": _obj.Number, "Name": _obj.Name});
                    setCookie('tmselected', JSON.stringify(_coval), 1);
                    }
            }

        }
    }
    catch (e) {

    }
}
function TM_CookieGet () {
    try {
        let _coname = 'tmselected';
        let _cocur = getCookie(_coname);
        if (_cocur != undefined && _cocur != "") {
            return JSON.parse(_cocur);
        }
        else return [];
    }
    catch (e) {
        return [];
    }

}
function TM_CookieTeethName (_teeth, _type) {
    try {
        _teeth = ',' + _teeth + ',';
        let obj = {};
        let _result = '';
        let _num = 0;
        for (let _i = 0; _i < dataTeeth.length; _i++) {
            let _item = dataTeeth[_i];


            let idstring = ',' + _item.ID + ',';
            if ((_teeth.includes(idstring))) {
                _num = _num + 1;
                if (_type == 0) {
                    _result = _result + _item.TeethName + ',';
                }
                else if (_type == 1) {
                    _result = _result + _item.TeethNameBaby + ',';
                }
                else {
                    _result = _result + _item.TeethNameMerge + ',';
                }
            }
        }
        if (_result.length > 1 && _result.charAt(_result.length - 1) == ',')
            _result = _result.substring(0, _result.length - 1);
        obj.Number = _num;
        obj.Name = _result;
        return obj;
    }
    catch (e) {
        return "";
    }
}

function TM_CookieTeethEvent () {
    $('#divteethcookie .cooselected').unbind('click').click(function () {
        tab_changing_flag = 0;
        let count = 0;
        let TeethChoosing = $(this).attr('data-value');
        if (TeethChoosing != undefined) {
            var x = document.getElementsByClassName("checkTeeth");
            var teehchose = TeethChoosing.split(",")
            for (let i = 0; i < x.length; i++) {
                x[i].checked = false;
            }
            for (let i = 0; i < x.length; i++) {
                for (let j = 0; j < teehchose.length; j++) {
                    if (x[i].value == teehchose[j]) {
                        x[i].checked = true;
                        count = count + 1;
                    }
                }
            }
            $('#Tab_Service_Quantity').val(count);
            tab_changing_flag = 1;
            Tab_Count_Price_For_All();
        }

    });
}
//#endregion