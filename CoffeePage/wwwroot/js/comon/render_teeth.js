
function RenderTeethFront(data, id, TeethType, isdenture) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        let _class_less = '';
        myNode.innerHTML = '';
        let stringContent = '';
        if (Number(isdenture) == 0) {
            if (Number(TeethType) == 0) {
                data = data.filter(word => word["IsAdult"] == 1 && word["IsGum"] == 0);
            }
            else if (Number(TeethType) == 1) {
                data = data.filter(word => word["IsChild"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            else {
                data = data.filter(word => word["IsMerge"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            let _from = 0;
            let _to = data.length / 2;
            if (data && data.length > 0) {
                let _name, _link;
                for (var i = _from; i < _to; i++) {
                    let item = data[i];
                    if (Number(TeethType) == 0) {
                        _name = item.TeethName; _link = item.UrlImage;
                    }
                    else if (Number(TeethType) == 1) {
                        _name = item.TeethNameBaby; _link = item.UrlImage_Baby;
                    }
                    else {
                        _name = item.TeethNameMerge; _link = item.UrlImage_Merge;
                    }

                    let tr =
                        '<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '"><img class="imageTeeth" src="/assests/img/TeethTab/' + _link + '.png">'
                        + '<div class="teeth_name_chart_front text-primary">' + _name + '</div>'
                        + '<div class="form-check ">'
                        + '<input class="form-check-input pr-2 checkTeeth front" value=' + item.ID + ' type="checkbox">'
                        + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                        + '</div>'
                        + '</div>'

                    stringContent = stringContent + tr;
                }
            }
        }
        else {
            data = data.filter(word => word["IsGum"] == 1);
            if (data && data.length == 2) {
                let _name, _link;
                let item = data[0];
                if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                let tr =
                    '<div class="teeth_gum_name_chart_front d-flex justify-content-center">'
                    + '<div class="form-check d-flex align-items-center">'
                    + '<input class="form-check-input pr-2 checkTeeth" value=' + item.ID + ' type="checkbox">'
                    + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                    + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                    + '</div>'
                //+ '<div class="teeth_name_chart_front imageTeeth">' + _name + '</div>'


                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function RenderTeethBellow(data, id, TeethType, isdenture) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        let _class_less = '';
        myNode.innerHTML = '';
        let stringContent = '';
        if (Number(isdenture) == 0) {
            if (Number(TeethType) == 0) {
                data = data.filter(word => word["IsAdult"] == 1 && word["IsGum"] == 0);
            }
            else if (Number(TeethType) == 1) {
                data = data.filter(word => word["IsChild"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            else {
                data = data.filter(word => word["IsMerge"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            let _from = data.length / 2;
            let _to = data.length;
            if (data && data.length > 0) {
                let _name, _link;
                for (var i = _from; i < _to; i++) {
                    let item = data[i];
                    if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                    else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                    else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }

                    let tr =

                        '<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '">'

                        + '<div class="form-check ">'
                        + '<input class="form-check-input pr-2 checkTeeth bellow" value=' + item.ID + ' type="checkbox">'
                        + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                        + '</div>'
                        + '<div class="teeth_name_chart_bellow text-primary" class="imageTeeth">' + _name + '</div>'
                        + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'

                    stringContent = stringContent + tr;
                }
            }
        }
        else {
            data = data.filter(word => word["IsGum"] == 1);
            if (data && data.length == 2) {
                let _name, _link;
                let item = data[1];
                if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                let tr =
                    '<div class="teeth_gum_name_chart_bellow d-flex justify-content-center">'
                    + '<div class="form-check d-flex align-items-center">'
                    + '<input class="form-check-input pr-2 checkTeeth" value=' + item.ID + ' type="checkbox">'
                    + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                    + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                    + '</div>'

                //+ '<div class="teeth_name_chart_bellow imageTeeth">' + _name + '</div>'


                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function RenderTeethFront_TeethChoose(data, id, TeethType, isdenture, teeth_choosing) {
    teeth_choosing = ',' + teeth_choosing
    var myNode = document.getElementById(id);
    if (myNode != null) {
        let _class_less = '';
        myNode.innerHTML = '';
        let stringContent = '';
        if (Number(isdenture) == 0) {
            if (Number(TeethType) == 0) {
                data = data.filter(word => word["IsAdult"] == 1 && word["IsGum"] == 0);
            }
            else if (Number(TeethType) == 1) {
                data = data.filter(word => word["IsChild"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            else {
                data = data.filter(word => word["IsMerge"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            let _from = 0;
            let _to = data.length / 2;
            if (data && data.length > 0) {
                let _name, _link;
                for (var i = _from; i < _to; i++) {
                    let item = data[i];
                    let idstring = ',' + item.ID + ',';
                    let tr = '';
                    if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                    else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                    else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                    if ((teeth_choosing.includes(idstring) == true)) {
                         tr =
                            '<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '"><img class="imageTeeth" src="/assests/img/TeethTab/' + _link + '.png">'
                            + '<div class="teeth_name_chart_front">' + _name + '</div>'
                            + '<div class="form-check ">'
                            + '<input class="form-check-input pr-2 checkTeeth front" value=' + item.ID + ' type="checkbox">'
                            + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                            + '</div>'
                            + '</div>'
                         
                    }
                    else {
                         tr =
                            '<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '"><img class="imageTeeth" src="/assests/img/TeethTab/' + _link + '.png">'
                            + '<div class="teeth_name_chart_front">' + _name + '</div>'
                            + '<div class="form-check ">'
                         + '<input class="form-check-input pr-2 checkTeeth front disabled" value=' + item.ID + ' type="checkbox" disabled>'
                            + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                            + '</div>'
                            + '</div>'


                       
                    }
                    stringContent = stringContent + tr;
                }
            }
        }
        else {
            data = data.filter(word => word["IsGum"] == 1);
            if (data && data.length == 2) {
                let _name, _link;
                let item = data[0];
                if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                let idstring = ',' + item.ID + ',';
                let tr = '';
                if ((teeth_choosing.includes(idstring) == true)) {
                    

                    tr =
                        '<div class="teeth_gum_name_chart_front d-flex justify-content-center">'
                        + '<div class="form-check d-flex align-items-center">'
                        + '<input class="form-check-input pr-2 checkTeeth" value=' + item.ID + ' type="checkbox">'
                        + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                        + '<img src="/assests/img/TeethTab/' + _link + '.png"></div>'
                        + '</div>'
        

                }
                else {
                    tr =
                        '<div class="teeth_gum_name_chart_front d-flex justify-content-center">'
                        + '<div class="form-check d-flex align-items-center">'
                    + '<input class="form-check-input pr-2 checkTeeth disabled" value=' + item.ID + ' type="checkbox" disabled>'
                        + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                        + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                        + '</div>'

                    //tr =
                    //    '<div class="teeth_gum_name_chart_front d-flex justify-content-center">'
                    //    + '<div class="teeth_name_chart_front">' + _name + '</div>'
                    //    + '<img src="/assests/img/TeethTab/' + _link + '.png"></div>'
                }
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}
function RenderTeethBellow_TeethChoose(data, id, TeethType, isdenture, teeth_choosing) {
    teeth_choosing = ',' + teeth_choosing
    var myNode = document.getElementById(id);
    if (myNode != null) {
        let _class_less = '';
        myNode.innerHTML = '';
        let stringContent = '';
        if (Number(isdenture) == 0) {
            if (Number(TeethType) == 0) {
                data = data.filter(word => word["IsAdult"] == 1 && word["IsGum"] == 0);
            }
            else if (Number(TeethType) == 1) {
                data = data.filter(word => word["IsChild"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            else {
                data = data.filter(word => word["IsMerge"] == 1 && word["IsGum"] == 0);
                _class_less = ' _less';
            }
            let _from = data.length / 2;
            let _to = data.length;
            if (data && data.length > 0) {
                let _name, _link;
                for (var i = _from; i < _to; i++) {
                    let item = data[i];
                    let idstring = ',' + item.ID + ',';
                    let tr = '';
                    if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                    else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                    else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                    if ((teeth_choosing.includes(idstring) == true)) {
                        tr ='<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '">'

                            + '<div class="form-check ">'
                            + '<input class="form-check-input pr-2 checkTeeth bellow" value=' + item.ID + ' type="checkbox">'
                            + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                            + '</div>'
                            + '<div class="teeth_name_chart_bellow" class="imageTeeth">' + _name + '</div>'
                            + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                    }
                    else {
                        tr = '<div class="p-0 col-w-teeth text-center position-relative col' + _class_less + '">'

                            + '<div class="form-check ">'
                            + '<input class="form-check-input pr-2 checkTeeth bellow disabled "  type="checkbox" disabled>'
                            + '<label class="custom-control-label font-weight-normal checkTeeth_label" ></label>'
                            + '</div>'
                            + '<div class="teeth_name_chart_bellow" class="imageTeeth">' + _name + '</div>'
                            + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                    }
                    stringContent = stringContent + tr;
                }
            }
        }
        else {
            data = data.filter(word => word["IsGum"] == 1);
            if (data && data.length == 2) {
                let _name, _link;
                let item = data[1];
                if (Number(TeethType) == 0) { _name = item.TeethName; _link = item.UrlImage; }
                else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; _link = item.UrlImage_Baby; }
                else { _name = item.TeethNameMerge; _link = item.UrlImage_Merge; }
                let idstring = ',' + item.ID + ',';
                let tr = '';
                if ((teeth_choosing.includes(idstring) == true)) {
                    tr =
                        '<div class="teeth_gum_name_chart_bellow d-flex justify-content-center">'
                        + '<div class="form-check d-flex align-items-center">'
                        + '<input class="form-check-input pr-2 checkTeeth" value=' + item.ID + ' type="checkbox">'
                        + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                        + '<img src="/assests/img/TeethTab/' + _link + '.png"></div>'
                        + '</div>'

                    //+ '<div class="teeth_name_chart_bellow">' + _name + '</div>'

                }
                else {
                      tr =
                        '<div class="teeth_gum_name_chart_bellow d-flex justify-content-center">'
                        + '<div class="form-check d-flex align-items-center">'
                        + '<input class="form-check-input pr-2 checkTeeth disabled" value=' + item.ID + ' type="checkbox" disabled>'
                        + '<label class="mt-2 ms-2 custom-control-label font-weight-normal checkTeeth_label" >' + _name + '</label>'
                        + '<img src="/assests/img/TeethTab/' + _link + '.png" class="imageTeeth"></div>'
                        + '</div>'

                   
                }
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}

function RenderGumBellow(data, TeethType) {
    let result = '';
    data = data.filter(word => word["IsGum"] == 1);
    if (data && data.length == 2) {
        let item = data[1];
        let _name;
        if (Number(TeethType) == 0) { _name = item.TeethName; }
        else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; }
        else { _name = item.TeethNameMerge; }
        result =
            '<input class="form-check-input  checkTeeth gumbellow" type="checkbox"   value=' + item.ID + '>'
            + '<label class="custom-control-label font-weight-normal checkTeeth_label" for="">' + _name + '</label>'
        //result = '<input type="checkbox" value=' + item.ID + ' class="checkTeeth gumbellow" />'
        //    + '<label class="checkTeeth_label" for="">' + _name+'</label>'
    }
    return result;
}
function RenderGumFront(data, TeethType) {
    let result = '';
    data = data.filter(word => word["IsGum"] == 1);
    if (data && data.length == 2) {
        let item = data[0];
        let _name;
        if (Number(TeethType) == 0) { _name = item.TeethName; }
        else if (Number(TeethType) == 1) { _name = item.TeethNameBaby; }
        else { _name = item.TeethNameMerge; }
        result =
            '<input class="form-check-input checkTeeth gumfront" type="checkbox"   value=' + item.ID + '>'
            + '<label class="custom-control-label font-weight-normal checkTeeth_label" for="">' + _name + '</label>'
        //result = '<input type="checkbox" value=' + item.ID + ' class="checkTeeth gumfront" />'
        //    + '<label class="checkTeeth_label" for="">' + _name + '</label>'
    }
    return result;
}