// #region // font size browser
function Change_Font_Size_Element(element, listclass,classunchange,namecookie) {
    
    if (Cookies.get(namecookie) != undefined) {
        let _font_size_default = Cookies.get(namecookie);
        for (_l = 0; _l < element.length; _l++) {
            let _has_font = $(element[_l]).hasClass(classunchange);
            let _has_button = $(element[_l]).hasClass("button");
            let _has_ribbon = $(element[_l]).hasClass("ribbon");
            let _has_dropdown_icon = $(element[_l]).hasClass("icon");
            let _has_total_header = $(element[_l]).hasClass("label_total_header");
            let _parent_is_table = $(element[_l]).closest("table");
            let _parent_is_dropdown = $(element[_l]).parent(".dropdown");
            if (!_parent_is_table.hasClass(classunchange)) {
                _parent_is_table.removeClass(listclass).addClass(_font_size_default);
            }
            if (_parent_is_dropdown.length > 0) {
                _parent_is_dropdown.removeClass(listclass).addClass(_font_size_default);
            }
            if (!_has_font && !_has_ribbon && _parent_is_dropdown.length == 0 && !_has_dropdown_icon) {
                $(element[_l]).removeClass(listclass).addClass(_font_size_default);
            }
        }
    }
   
}
function Check_Change_Font_Size() {
    if (typeof ListSizeFontBrowser !== 'undefined') {
        let listclassfont = "";
        for (let i = 0; i < ListSizeFontBrowser.length; i++) {
            listclassfont += ListSizeFontBrowser[i].class + " ";
        }
        let elementChange = $('.dropdown, span, input, a, textarea, [data-languagedyn], [data-fontchanged]');
        let elementUnchange = "font-unchanged";
        let nameCookie = "MLUECH_Font_Size_Browser";
        Change_Font_Size_Element(elementChange, listclassfont, elementUnchange, nameCookie);
    }
}
// #endregion