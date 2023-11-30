const MSV_Name = {
    color: 'setting_Color',
    fontSize: 'setting_FontSize',
    darkMode: 'settingDarkMode',
    menutype: 'menu_type',
    menuposition: 'menu_position',
}
function MSV_Initiliaze () {
    MSVMenuPos_Set();
    MSV_InitiliazeExe();
}
const MSV_Listcolor = {
    'primary': {
        'bs-primary': '203, 12, 159',
        'bs-primary-from': '121, 40, 202',
        'bs-primary-to': '255, 0, 128',
        'bs-memu': '32, 0, 34',
    }
    , 'primary-2': {
        'bs-primary': '235, 51, 72',
        'bs-primary-from': '235, 51, 72',
        'bs-primary-to': '244, 92, 67',
        'bs-memu': '26, 4, 5',
    }
    , 'primary-3': {
        'bs-primary': '9, 124, 216',
        'bs-primary-from': '0, 74, 201',
        'bs-primary-to': '33, 170, 253',
        'bs-memu': '4, 34, 70',
    }
    , 'primary-4': {
        'bs-primary': '15, 116, 2',
        'bs-primary-from': '15, 116, 2',
        'bs-primary-to': '23, 165, 87',
        'bs-memu': '1, 18, 2',
    }
    , 'primary-5': {
        'bs-primary': '118, 0, 220',
        'bs-primary-from': '118, 0, 220',
        'bs-primary-to': '218, 34, 255',
        'bs-memu': '19, 2, 28',
    }
    , 'primary-6': {
        'bs-primary': '255, 81, 47',
        'bs-primary-from': '255, 81, 47',
        'bs-primary-to': '240, 152, 25',
        'bs-memu': '30, 12, 3',
    }
    , 'primary-7': {
        'bs-primary': '214, 51, 132',
        'bs-primary-from': '214, 51, 132',
        'bs-primary-to': '186, 22, 103',
        'bs-memu': '30, 2, 16',
    },

}

//#region ICD

const MSV_ICDArea = {
    "diagnose": 0
    ,"prescription" : 1
}
//#endregion
async function MSV_InitiliazeExe () {
    new Promise(() => {
        setTimeout(() => {
            MSVDarkmode_Set(MSVDarkmode_Get());
            MSVDarkmode_Event();
            MSVColor_Set();
            MSV_FontSet();
            MSV_ColorRender(MSV_Listcolor, 'Master_Bagde_Color');
        })
    })
}

//#region // SETTING DARKMODE
function MSVDarkmode_Event () {
    $("#dark-version").unbind('click').on("click", function () {
        if ($("#BodyMain").hasClass("dark-version"))
            MSVDarkmode_Set('light');
        else
            MSVDarkmode_Set('dark');
    })
}
function MSVDarkmode_Set (mode) {
    if (mode == 'light') {
        $("#BodyMain").removeClass('dark-version');
        $("#dark-version").prop("checked", false);
        $("#dark-versionarea .night").removeClass('d-none');
        $("#dark-versionarea .morning").addClass('d-none');
    }
    else {
        $("#BodyMain").addClass('dark-version');
        $("#dark-version").prop("checked", true);
        $("#dark-versionarea .night").addClass('d-none');
        $("#dark-versionarea .morning").removeClass('d-none');
    }
    localstorage_set(MSV_Name.darkMode, mode);
}
function MSVDarkmode_Get () {
    try {
        let result = 'light';
        let dataMode = localstorage_get(MSV_Name.darkMode);
        if (dataMode != '') {
            result = JSON.parse(dataMode).data;
        }
        return result;
    }
    catch (ex) {
        return 'light';
    }

}
//#endregion

//#region // SETTING COLOR
async function MSV_FontSet() {
    if (sys_Minify == 1)
        document.documentElement.style.setProperty('--bs-font-sans-serif', 'Be Vietnam Pro, sans-serif');
}
//#endregion

//#region // SETTING COLOR
async function MSV_ColorRender (data, id) {
    new Promise((resolve) => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = "";
            if (data && Object.values(data).length > 0) {
                for ([key, value] of Object.entries(data)) {
                    let bgColor = `background-image: linear-gradient(310deg, rgb(${value['bs-primary-from']}) 0%, rgb(${value['bs-primary-to']}) 100%)`;
                    let tr = `<span class="setting-color badge filter" style="${bgColor}" data-color="${key}" ></span>`;
                    myNode.insertAdjacentHTML('beforeend', tr);
                }
            }
        }
        MSVGeneral_Event();
    })
}
function MSVGeneral_Event () {
    $("#Master_Bagde_Color .setting-color").on("click", function () {
        let dataColor = $(this).attr('data-color');
        if (MSV_Listcolor && MSV_Listcolor[dataColor] != undefined) {
            localstorage_set(MSV_Name.color, JSON.stringify(MSV_Listcolor[dataColor]));
            MSVColor_Set();
        }
    })
    $("#Master_Bagde_Navtype .settype").on("click", function () {
        let datatype = $(this).attr('data-type');
        localstorage_set(MSV_Name.menutype, datatype);
        MSVColor_Set();
    })
    $("#Master_Bagde_Position .settype").on("click", function () {
        let datatype = $(this).attr('data-type');
        localstorage_set(MSV_Name.menuposition, datatype);
        $("#BodyMain").addClass('opacity-0');
        MSVMenuPos_Set();
         location.reload();
    })
}
function MSVColor_Get () {
    try {
        let result = {};
        let data = localstorage_get(MSV_Name.color);
        if (data && data != '') {
            let dataSetting = JSON.parse(data);
            result = JSON.parse(dataSetting.data);
        }
        return result;
    }
    catch (ex) {
        return {};
    }
}
function MSVMenuType_Get () {
    try {
        let result = "";
        let data = localstorage_get(MSV_Name.menutype);
        if (data && data != '') {
            let dataSetting = JSON.parse(data);
            result = dataSetting.data;
        }
        return result;
    }
    catch (ex) {
        return {};
    }
}
function MSVColor_Set () {
    try {
        let color_primary = "";
        let color_textprimary = "255, 255, 255";
        let dataSetting = MSVColor_Get();
        if (dataSetting && Object.values(dataSetting).length != 0) {
            color_primary = dataSetting['bs-memu'];
            document.documentElement.style.setProperty('--bs-primary', dataSetting['bs-primary']);
            document.documentElement.style.setProperty('--bs-primary-from', dataSetting['bs-primary-from']);
            document.documentElement.style.setProperty('--bs-primary-to', dataSetting['bs-primary-to']);
        }
        else if (defaultColor && defaultColor.length > 0) {
            dataSetting = JSON.parse(defaultColor.replace(/&quot;/g, '"'));
            color_primary = dataSetting['bs-memu'];
            document.documentElement.style.setProperty('--bs-primary', dataSetting['bs-primary']);
            document.documentElement.style.setProperty('--bs-primary-from', dataSetting['bs-primary-from']);
            document.documentElement.style.setProperty('--bs-primary-to', dataSetting['bs-primary-to']);
        }

        let _menutype = MSVMenuType_Get();
         
        if (_menutype == "color") {
            if (color_primary != "" && color_primary != undefined)
                document.documentElement.style.setProperty('--bs-menu', color_primary);
            else
                document.documentElement.style.setProperty('--bs-menu', '33, 37, 41');
            document.documentElement.style.setProperty('--bs-menu-text', color_textprimary);
            $("#sidenav-main").removeClass('bg-gray-100');
        }
        else {
            $("#sidenav-main").addClass('bg-gray-100');
            $("#sidenav-main .sidenav-header").addClass('bg-gray-100');
            //#f8f9fa 248, 249, 250

            document.documentElement.style.setProperty('--bs-menu', "");
            document.documentElement.style.setProperty('--bs-menu-text', "0, 19, 33");
        }
    }
    catch (ex) {
        return false;
    }
}
//#endregion

//#region // Pos
function MSVMenuPos_Get () {
    try {
        let result = {};
        let data = localstorage_get(MSV_Name.menuposition);
        if (data && data != '') {
            let dataSetting = JSON.parse(data);
            result = dataSetting.data;
        }
        return result;
    }
    catch (ex) {
        return {};
    }
}
function MSVMenuPos_Set () {
    try {
        let mode = MSVMenuPos_Get();
        if (mode == 'top') {
             $("#BodyMain").addClass('menutop-version'); 
        }
        else {
           $("#BodyMain").removeClass('menutop-version');
        }
    }
    catch (ex) {
        return false;
    }
}
//#endregion

//#region // show more 
function Master_ToggleShowContent () {
    $("#BodyMain").toggleClass('show_content_all');
    Master_ClosePopupContent();
}

function Master_ClosePopupContent () {
    $('#Master_ViewContent').removeClass('active');
    $('#Master_Content').html('');
}
function Master_ReadContentCollapse () {
    $('body').on('click', '.content_line_clamp', function () {
        event.stopPropagation();
        let isAll = $("#BodyMain").hasClass('show_content_all');
        if (this.scrollHeight > this.clientHeight || isAll) {
            let titleshowall = $('#Master_ShowAll');
            if (isAll) titleshowall.html(Outlang["Thu_gon_tat_ca"]);
            else titleshowall.html(Outlang["Hien_thi_tat_ca"]);
            $('#Master_ViewContent').addClass("active");
 
             $("#Master_Content").html($(this).html());
        
        }
    });

}
//#endregion
//#region ICD Event
function Master_ICDGetArea(key) {
    if (MSV_ICDArea && MSV_ICDArea[key] != undefined)
        return MSV_ICDArea[key]
    else return ''
}
//#endregion
//#region //  Version Load

function Master_Load_JS (callback) {
    try {

        ver_check();
        js_require('/assests/dist/SemanticUI/form.min.js');
        js_require('/assests/dist/plugins/sweetalert2/sweetalert2.min.js');
        js_require_notasync('/js/comon/noti_function.js', true);
        js_require('/js/comon/Remove_title.js');
        js_require_notasync('/assests/js/Swipe.js');
        js_require_notasync('/assests/js/detect-mobile.js');
        js_require_notasync('/assests/dist/flatpickr/flatpickr.js');
        js_require('/js/comon/render_teeth.js');
        js_require_notasync('/assests/dist/plugins/datatable/jquery.dataTables.js', true);
        js_require_notasync('/assests/dist/MultiColumnCombo/jquery.inputpicker.js');
        js_require_notasync('/assests/dist/NumberThousand/number-divider.js');
        js_require_notasync('/assests/dist/NumberThousand/jquery.number.js');
        js_require('/assests/dist/Guide/enjoyhint.min.js');
        js_require('/assests/dist/Phone/js/intlTelInput.min.js');
        js_require_notasync('/js/comon.js');
        js_require_notasync('/assests/dist/HightLight/dist/jquery.mark.js');
        js_require_notasync('/assests/library/jquery.signalR-2.4.1.min.js');
        js_require_notasync('/assests/dist/signalr/dist/browser/signalr.js');
        //js_require_notasync('/js/signalR/message_hub.js', true);
        //js_require_notasync('/js/signalR/online_hub.js', true);
        js_require_notasync('/js/signalR/notification_hub.js', true);
        js_require_notasync('/js/comon/mark.js', true);
        js_require_notasync('/js/comon/Datetime.js', true);
        js_require_notasync('/js/comon/permission_tabcontrol.js', true);
        js_require_notasync('/js/comon/Popup.js', true);
        js_require_notasync('/js/comon/Export.js', true);
        js_require_notasync('/js/comon/Combo_v2.js', true);
        js_require_notasync('/js/comon/Formatnumber.js', true);
        js_require_notasync('/js/comon/Other.js', true);
        js_require_notasync('/js/comon/report.js', true);
        js_require_notasync('/js/comon/Callcenter.js', true);
        js_require_notasync('/js/comon/Render.js', true);
        js_require_notasync('/js/comon/tablesort.js', true);
        js_require('/js/comon/Toast.js');
        js_require('/js/comon/NotiExe.js');
        js_require('/js/comon/userguide.js');
        js_require('/js/comon/print.js');
        js_require_notasync('/js/print/print.js', true);
        js_require_notasync('/assests/dist/ExportExcel/ExportELSX/xlsx.core.min.js', true);
        js_require_notasync('/assests/dist/ExportExcel/ExportELSX/FileSaver.js', true);
        js_require_notasync('/js/comon/CheckConnection.js');
        js_require_notasync('/js/comon/AlarmSchedule.js');
        js_require_notasync('/assests/dist/Barcode/JsBarcode.code128.min.js', true);
        js_require_notasync('/js/comon/Signation.js');
        js_require_notasync('/js/customjs/custom-tablefixer.js');
        js_require_notasync('/js/table_responsive.js');
        js_require('/js/Todo/Todo_Detail.js');
        js_require('/js/Labo/LaboPopup.js');
        js_require_notasync('/js/main-sidebar.js');
        js_require('/js/main.js');
        js_require_notasync('/assests/dist/plugins/cookie/js.cookie.js');
        js_require_notasync('/assests/dist/ExportExcel/ExportELSX/tableexport.min.js', true);
        js_require('/js/customjs/custom-font-size.js');
        js_require_notasync('/assests/library/jquery-ui.js', true);
        js_require('/assests/dist/Calloutside/calloutside.js');
        js_require_notasync('/assests/dist/Popperjs/popper.min.js');
        js_require('/assests/dist/Popperjs/tippy-bundle.umd.min.js');
        js_require_notasync('/js/comon/initialize_setting.js');
        js_require_notasync('/js/Shared/master.js');
        js_require('/js/comon/load_customer.js');
        js_require_notasync('/assests/dist/UploadJS/js/vendor/jquery.ui.widget.js', true);
        js_require_notasync('/assests/dist/UploadJS/js/jquery.iframe-transport.js', true);
        js_require_notasync('/assests/dist/UploadJS/js/jquery.fileupload.js', true);
        js_require_notasync('/assests/dist/ColorPicker/spectrum.js', true);
        js_require_notasync('/assests/dist/Paginationjs/paginationjs.min.js', true);
        js_require('/js/comon/customerimage2.js');
        js_require('/js/customjs/custom-dropdown.js');
        js_require('/assets/js/plugins/countup.min.js');
        js_require_notasync('/assests/dist/QRCode/qrcode.min.js', true);
        js_require_notasync('/js/log.js', true);
        js_require_notasync('/Print/printThis.js', true);
 
        if (sys_Minify !== undefined && Number(sys_Minify) === 0) {
            js_require_notasync('/Protect/index.js', true);
            js_require_notasync('/Protect/protect.js', true);
            js_require_notasync('/Protect/protect_20220717.js', true);
        }
        if (typeof callback == 'function') callback();
        if (Master_heartbeat == 1) js_require_notasync('/js/comon/KeepAlive.js');
 
        js_require_notasync('/assests/dist/lightGallery/lightgallery.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/thumbnail/lg-thumbnail.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/fullscreen/lg-fullscreen.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/zoom/lg-zoom.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/autoplay/lg-autoplay.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/share/lg-share.min.js', true);
        js_require_notasync('/assests/dist/lightGallery/plugins/rotate/lg-rotate.min.js', true);
    
    }
    catch (ex) {

    }
}
//#endregion



 