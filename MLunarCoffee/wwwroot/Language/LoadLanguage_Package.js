

var LanguageVTT;
var language_global_noti_text = [];
var Global_Language_Chossing;
(function ($) {
 
    LanguageVTT = {

        RefeshcbbLanguageCurrent: function () {
            Global_Language_Chossing = GetLanguageCurrent();
            isChange_Language = 0;
            $("#cbbChangeLanguage ").dropdown("set selected", Global_Language_Chossing);
            isChange_Language = 1;
        },
        Init: function () {
            this.RefeshcbbLanguageCurrent();
            Language_LoadNoti_ToGlobal();
            if (Global_Language_Chossing == "en") {
                xmlLang_dynamic_Url ="/Language/ENG/dynamic.xml"
                xmlLang_static_Url = "/Language/ENG/static.xml"
                ChangeLanguage_Detect_Dynamic_File();
            }
            else {
                xmlLang_static_Url = "/Language/VN/static.xml"
            }
            ChangeLanguage_Detect_Static_File();
        },
        Refresh: function () {
            try {
                if (Global_Language_Chossing == "en") {
                    if (optionsobservers == undefined) optionsobservers = {
                        childList: true
                    };
                    CreateObserversMutation();
                }
                ChangeLanguage_Static();
            }
            catch (ex) {

            }
        },
        ChangeLan_Menu_NotMutaion: function () {
            if (Global_Language_Chossing != "vn" && xmlLang_dynamic != undefined) {
   
                let _menu = $('[data-menu]');
                if (_menu != undefined) {
                    for (_l = 0; _l < _menu.length; _l++) {
                        let _x = _menu[_l].innerHTML;

                        let _node = xmlLang_dynamic.getElementsByTagName("value");
                        let _tag = _menu[_l].dataset.menu;
                        for (_i = 0; _i < _node.length; _i++) {
                            if (_node[_i].parentNode.nodeName == _tag) {
                                _x = _x.replaceAll(_node[_i].getAttribute("vn"), _node[_i].innerHTML);
                                _x = _x.replaceAll(xoa_dau(_node[_i].getAttribute("vn")).toLowerCase(), _node[_i].innerHTML.toLowerCase());
                            }
                        }
                        _menu[_l].innerHTML = _x;
                    }
                }
            }
        },
        ChangeLan_Poppup_NotMutaion: function () {
            if (Global_Language_Chossing != "vn" && xmlLang_dynamic != undefined) {
                let _menu = $('[data-popupuser]');
                if (_menu != undefined) {
                    for (_l = 0; _l < _menu.length; _l++) {
                        let _x = _menu[_l].innerHTML;
                        let _node = xmlLang_dynamic.getElementsByTagName("value");
                        let _tag = _menu[_l].dataset.popupuser;
                        for (_i = 0; _i < _node.length; _i++) {
                            if (_node[_i].parentNode.nodeName == _tag) {
                                _x = _x.replaceAll(_node[_i].getAttribute("vn"), _node[_i].innerHTML);
                            }
                        }
                        _menu[_l].innerHTML = _x;
                    }
                }
            }
        },
        ChangeLanguage: function () {
            let _lan = $('#cbbChangeLanguage').dropdown('get value');
            if (_lan != undefined && isChange_Language == 1) {
                localStorage.setItem("vttechlanguage", _lan)
                location.reload();
            }
        },
        Terminate: function () {
            TerminateMutaion();
        },
        Initialize: function () {
            InitializeMutation();
        }

    }
    // #region // Load Data Language
    var xmlLang_static;
    var xmlLang_static_Url;
    var xmlLang_dynamic_Url;
    var xmlLang_noti_Url =  "/Language/noti.js?ver=" + (new Date()).getTime();
    var isChange_Language;

    var xmlLang_dynamic;
   

    var observers_Dynamic;
    var optionsobservers;
    function GetLanguageCurrent() {
        try {
            return localStorage.getItem("vttechlanguage");
        }
        catch (ex) {
            return "vn";
        }
    }
    function Language_LoadNoti_ToGlobal() {
        if (language_global_noti_text.length == 0) {
            $.loadScript(xmlLang_noti_Url, function () {
                let _e;
                for (_p = 0; _p < data_noti_language.length; _p++) {
                    _e = {};
                    _e.title = data_noti_language[_p].title;
                    _e.value = {
                        vn: data_noti_language[_p].vn,
                        en: data_noti_language[_p].en
                    };
                    language_global_noti_text.push(_e);
                }
            });
        }
    }
    function ChangeLanguage_Detect_Static_File() {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                xmlLang_static = this.responseXML;
            }
        };
        xhttp.open("GET", xmlLang_static_Url, false);
        xhttp.send();
    }
    function ChangeLanguage_Detect_Dynamic_File() {
        var xhttp_d = new XMLHttpRequest();
        xhttp_d.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                xmlLang_dynamic = this.responseXML;
            }
        };
        xhttp_d.open("GET", xmlLang_dynamic_Url, false);
        xhttp_d.send();
    }



    //endregion



    // #region // Dynamic
    function CreateObserversMutation() {
        if (observers_Dynamic != undefined && observers_Dynamic != null) {
            TerminateMutaion();
            Active_Change_ManualMutation();
        }
        else {
            observers_Dynamic = new MutationObserver(CallbackMutation);
        }
        InitializeMutation();
    }
    function TerminateMutaion() {
        if (observers_Dynamic != undefined && observers_Dynamic != null) {
            observers_Dynamic.disconnect();
        }
    }
    function InitializeMutation() {
        let langelement = $('[data-languagedyn]');
        for (_l = 0; _l < langelement.length; _l++) {
            if (langelement[_l].attributes["data-languagedyn"].value != "") {
                if (observers_Dynamic != undefined && observers_Dynamic != null) {

                    observers_Dynamic.observe(langelement[_l], optionsobservers);
                }
            }
        }
    }
    function Active_Change_ManualMutation(obj) {
        let langelement = $('[data-languagedyn]');
        for (_ii = 0; _ii < langelement.length; _ii++) {
            let _tag = langelement[_ii].dataset.languagedyn;
            let _node = xmlLang_dynamic.getElementsByTagName("value");
            let _x = langelement[_ii].innerHTML;
            for (_i = 0; _i < _node.length; _i++) {
                if (_node[_i].parentNode.nodeName == _tag) {
                    if (_node[_i].getAttribute("vn") != _node[_i].innerHTML)
                        _x = _x.replaceAll(_node[_i].getAttribute("vn"), _node[_i].innerHTML);
                }
            }
            langelement[_ii].innerHTML = _x;
        }
    }
    function CallbackMutation(mutations) {
        for (let mutation of mutations) {
            if (mutation.type === 'childList') {
                let _tag = mutation.target.dataset.languagedyn;
                let _node = xmlLang_dynamic.getElementsByTagName("value");
                let _x = mutation.target.innerHTML;
                for (_i = 0; _i < _node.length; _i++) {
                    if (_node[_i].parentNode.nodeName == _tag) {
                        _x = _x.replaceAll(_node[_i].getAttribute("vn"), _node[_i].innerHTML);
                    }
                }
                TerminateMutaion();
                mutation.target.innerHTML = _x;
                Mutation_Change_Previouse_Sibling_Combo(mutation.target, _tag, _node);
                Mutation_Detect_Table_Sort(mutation.target);

                InitializeMutation();
            }
        }
    }
    function Mutation_Detect_Table_Sort(obj) {
        try {
            if (obj.parentNode != undefined && obj.parentNode.nodeName.toLowerCase() == "table") {
                $('#' + obj.parentNode.id).tablesort();
            }
        }
        catch (ex) {

        }
    }
    function Mutation_Change_Previouse_Sibling_Combo(obj, tag, node) {
        let _sib = obj.previousElementSibling;
        if (_sib != undefined) {
            let _x = _sib.innerHTML;
            for (_i = 0; _i < node.length; _i++) {
                if (node[_i].parentNode.nodeName == tag) {
                    _x = _x.replaceAll(node[_i].getAttribute("vn"), node[_i].innerHTML);
                }
            }
            _sib.innerHTML = _x;
        }
    }
    // #endregion

    // #region // Static

    async function ChangeLanguage_Static() {
        let languagestatic = $('[data-languagestatic]');
        languagestatic.addClass('prepareChangeLanguage');
        await ChangeLanguage_StaticAsync();
        $('.prepareChangeLanguage').removeClass('prepareChangeLanguage');
        languagestatic.each(function () {
            this.removeAttribute("data-languagestatic");
            this.setAttribute("data-fontchanged",'true');
        });

        //Thay đổi kích thước chữ
        if (typeof Check_Change_Font_Size !== 'undefined' && $.isFunction(Check_Change_Font_Size))
            Check_Change_Font_Size();
    }
    function ChangeLanguage_StaticAsync(extension) {
        return new Promise((resolve, reject) => {
            setTimeout(
                () => {
                    if (xmlLang_static != undefined) {
                        let langelement = Global_Language_Chossing == 'vn' ? $('.header_form_main[data-languagestatic],.header_form_child[data-languagestatic]') : $('[data-languagestatic]');
                        for (_l = 0; _l < langelement.length; _l++) {
                            let _tag = langelement[_l].attributes["data-languagestatic"].value;
                            if (_tag != undefined && _tag != "") {
                                if (xmlLang_static.getElementsByTagName(_tag)[0] != undefined && xmlLang_static.getElementsByTagName(_tag)[0].childNodes[0] != undefined) {
                                    langelement[_l].removeAttribute("data-languagestatic");
                                    langelement[_l].innerHTML = xmlLang_static.getElementsByTagName(_tag)[0].childNodes[0].nodeValue;
                                }
                            }
                        }
                    }
                    resolve()
                }
            )
        })
    }
    // #endregion

})(jQuery);
LanguageVTT.Init();
