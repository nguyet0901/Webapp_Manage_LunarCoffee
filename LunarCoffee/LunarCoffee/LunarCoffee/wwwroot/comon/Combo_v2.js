
// Load_Combo(DataBranch, "cbbBranch", true);
var Timer_Search_Combo;
function Load_Combo(dt, id, isRequire, allItem) {
    let data = dt;
    if (data && data.length > 0) {
        data.map(e => {
            if (e.NameLangKey != undefined && e.NameLangKey != '')
                e.Name = Outlang[e.NameLangKey];
            else if (e.NameLangOther != undefined && e.NameLangOther != '')
                e.Name = e.NameLangOther;
        });
    }
    if (isRequire != undefined) {
        if (allItem != undefined) {
            if (data != undefined && data.length >= 1) {
                let item_all = JSON.parse(JSON.stringify(data[0]));
                if (item_all.ID != undefined && item_all.Name != undefined && item_all.ID != 0) {
                    item_all.ID = 0;
                    item_all.Name = allItem;
                    if (item_all.Icon != undefined) item_all.Icon = '';
                    if (item_all.Avatar != undefined) item_all.Avatar = '';
                    data = [item_all].concat(data);
                }
            }
        }
        let myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let item = data[i];
                    let icon = (item.Icon != undefined && item.Icon != '') ? ('<i class="' + item.Icon + ' me-2"></i>') : '';
                    let avatar = (item.Avatar != undefined && item.Avatar != '')
                        ? ('<img class="avatar avatar-xs" src="' + ((item.Avatar != "") ? item.Avatar : Master_Default_Pic) + '" onerror="Master_OnLoad_Error_Image(this)" alt="label-image">')
                        : '';
                    let tr =
                        '<div class="item" data-value="' + item.ID + '">' + icon + avatar + item.Name + '</div>'
                    stringContent = stringContent + tr;
                }
            }
            document.getElementById(id).innerHTML = stringContent;
            OnCombo_FillOption(id, data, isRequire);
        }
        if (allItem != undefined && data.length >= 1) return 0;
    }
}
function OnCombo_FillOption(id, data, isRequire) {
    let isMulti = 0;
    let parent = $('#' + id).parent().get(0);
    if ($(parent).attr('class').includes('multiple')) isMulti = 1;
    if (!$(parent).find(".search")) {
        $(parent).prepend('<input class="search" autocomplete="off" tabindex="0" />');
    }
    $(parent).find(".dropdown").remove();
    $(parent).prepend("<i class='dropdown icon'></i>");

    $(parent).find(".search").unbind('keyup');
    $(parent).find(".search").on('keyup', function (e) {
        if (![40, 39, 38, 37, 34, 33, 133].includes(e.keyCode)) {
            OnCombo_SearchTimeout(id, data, $(this).val(), 'Name', 'item', OnCombo_SearchRender);
        }
    });


    if (isMulti == 0) {
        $(parent).find('.dropdown.icon').unbind('click').on('click', function (e) {
            $(parent).find(".search").focus();
            e.preventDefault();
        });
        $(parent).unbind('click');
        $(parent).on('click', function (e) {
            OnCombo_Search(id, data, $(parent).find(".search").val(), 'Name', 'item', OnCombo_SearchRender);
        });
        if (!isRequire) {
            $(parent).find(".remove").remove();
            $(parent).prepend("<i class='remove icon'></i>");
            $(parent).find('.remove.icon').unbind('click').on('click', function (e) {
                $(this).parent('.dropdown').dropdown('clear');
                $(this).siblings("input.search").val("");
                e.stopPropagation();
            });
        }
        else {
            $(parent).find(".remove").remove();
        }
        $(parent).find('.text').unbind('click').on('click', function (e) {
            $(parent).find(".search").focus();
        });
    }
    else {
        $(parent).find('.search').unbind('click').on('click', function (e) {
            $(parent).dropdown('show');
            e.preventDefault();
        });
        //$(parent).unbind('click').on('click', function (e) {
        //    $(parent).find(".search").focus();
        //    e.preventDefault();
        //});
        //$('.ui.label.transition.visible .delete.icon').unbind('click').on('click', function (e) {
        //    $(this).parent('.dropdown').dropdown('clear');
        //    e.stopPropagation();
        //});
        //$(parent).find('.search').unbind('focus').on('focus', function (e) {
        //   // OnCombo_Search(id, data, $(parent).find(".search").val(), 'Name', 'item', OnCombo_SearchRender);
        //});
    }

}
function OnCombo_Search(comboid, data, searchkey, columnname, classrow, functionLoad) {
    try {
        searchkey = xoa_dau(searchkey).toLowerCase();
        let _data = data.filter(word => {
            return (xoa_dau(word[columnname]).toLowerCase().includes(searchkey));
        });
        if (_data != undefined && _data != null && _data.length != 0) {
            functionLoad(_data, comboid);
            ColorSearchFilterText_Combo(searchkey, '#' + comboid + ' > .' + classrow);
        }
        else {
            //functionLoad(data, comboid);
        }
        return false;
    }
    catch (ex){
        return false;
    }
}
function OnCombo_SearchTimeout(comboid, data, searchkey, columnname, classrow, functionLoad) {
    try {
        let parent = $('#' + comboid).parent().get(0);
        $(parent).addClass("loading");
        clearTimeout(Timer_Search_Combo);
        Timer_Search_Combo = setTimeout(function (e) {
            OnCombo_Search(comboid, data, searchkey, columnname, classrow, functionLoad);
            $(parent).removeClass("loading");
        }, 300)
    }
    catch (ex) {
        return false;
    }
}
function OnCombo_SearchRender(data, id) {
    let _isMulti = 0;
    let parent = $('#' + id).parent().get(0);
    if ($(parent).attr('class').includes('multiple')) _isMulti = 1;
    if (parent != undefined) {
        let value = $(parent).dropdown('get value');
        let myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    let item = data[i];
                    let icon = (item.Icon != undefined) ? ('<i class="' + item.Icon + ' me-2"></i>') : '';
                    let avatar = (item.Avatar != undefined)
                        ? ('<img class="avatar avatar-xs" src="' + ((item.Avatar != "") ? item.Avatar : Master_Default_Pic) + '" onerror="Master_OnLoad_Error_Image(this)" alt="label-image">')
                        : '';
                    let tr = '';
                    if (_isMulti == 0) {
                        let _classitem = '';
                        _classitem = ((',' + value + ',').includes(',' + item.ID + ',')) ? 'item active' : 'item';
                        tr = '<div class="' + _classitem + '"  data-value="' + item.ID + '">' + icon + avatar + item.Name + '</div>'
                    }
                    else {
                        if (!(',' + value + ',').includes(',' + item.ID + ','))
                            tr = '<div class="item"  data-value="' + item.ID + '">' + icon + avatar + item.Name + '</div>'
                    }
                    stringContent = stringContent + tr;
                }
            }
            myNode.innerHTML = stringContent;
        }
    }
}
