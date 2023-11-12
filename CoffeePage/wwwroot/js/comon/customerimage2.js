 
var Blodcusmax_size = sys_Custimgblod != 0 ? sys_Custimgblod : 2000;
var Blodcus_blockshare = sys_Custimgblockshare != 0 ? sys_Custimgblockshare : 0;
var Blodcus_iswamark = sys_Custimgwatermark != "" ? sys_Custimgwatermark : "";

var dataImageCustomer = [];
var dataFileCustomer = [];
var editteamplate = 0;
var dataImageForm;
var DataTemplateList = [];
var DataTemplate = [];
var TemplateID = 0;
var pluginInstance = null;
var CIF_dynamicGallery = {};
function ImageCustomer_Ini() {
    DragDropEvent_Item_Img();
}
function ImageCustomer_GetSizeInDay() {
    AjaxLoad(url = "/Customer/CustomerImage/?handler=GetSizeInDay"
        , data = {}
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (CI_LimitSizeUploadInday != 0 && data[0].SizeByte > CI_LimitSizeUploadInday) {
                    $("#uploadButton").remove();
                    $("#uploadButton_File").remove();
                }
            }
        }
    );
}

//#region // Event
function CustomerImage_Event() {
    $("#lightgalleryEdit .design_img_item").unbind('click').click(function (e) {
        if ($(this).hasClass('prepareDetele'))
            $(this).removeClass('prepareDetele');
        else $(this).addClass('prepareDetele');
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    })
    $("#lightgalleryEdit").unbind('click').click(function (e) {
        e.stopImmediatePropagation();
    })
    $("#lightgallery_FileEdit .file_pdf").unbind('click').click(function (e) {
        if ($(this).hasClass('prepareDetele'))
            $(this).removeClass('prepareDetele');
        else $(this).addClass('prepareDetele');
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    })
}

function CustomerImage_GalleryEvent() {
    $("#lightgallery .CustImage_clsLightGallery").unbind('click').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
        let numCreated = $(this).attr('data-numCreated') ? $(this).attr('data-numCreated') : "";
        if (numCreated != "") {
            if (CIF_dynamicGallery[numCreated])
                CIF_dynamicGallery[numCreated].openGallery($(this).index());
        }
    })
}

function ImageFile_Insert(type, folderid, name, realname, realnamefeature, SizeByte) {
    AjaxLoad(url = "/Customer/CustomerImage/?handler=InsertImage&ver=" + realname
        , data = {
            'type': type
            , 'folderid': folderid
            , 'name': name
            , 'namefeature': ((realnamefeature != undefined) ? realnamefeature : '')
            , 'realname': realname
            , 'sizebyte': SizeByte
        }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {
                CustomerImage_Load(folderid, result, (type == "image") ? 1 : 2);
            }
        }
    );


}
//#endregion

//#region // Edit Image And File
function ImageEdit_Begin() {
    ImageEdit_BeginSH();
    $("#lightgalleryEdit").show();
    $("#lightgallery").hide();
    $("#lightgallery_File").hide();
    $("#lightgallery_FileEdit").hide();
    $('#lightgalleryEdit').html($('#lightgallery').html());
    $("#seg_file").hide();
    CustomerImage_Event();
}
function ImageEdit_BeginSH() {
    $('.before_load').remove();
    $('.error_load').remove();
    $('.beforepdf_load').remove();
    $('.errorpdf_load').remove();
    $(".actionImg").hide();
    $(".actionFile").hide();
    $("#list-button_sort").hide();
    $("#noticeDelete").show();
}
function ImageEdit_CancelSH() {
    $(".actionImg").show();
    $(".actionFile").show();
    $("#lightgalleryEdit").hide();
    $("#lightgallery_FileEdit").hide();
    $("#lightgallery").show();
    $("#lightgallery_File").show();
    $("#list-button_sort").show();
    $("#noticeDelete").hide();
    $("#seg_image").show();
    $("#seg_file").show();
}
function ImageEdit_Cancel() {
    ImageEdit_CancelSH();
}
function ImageEdit_Save() {
    let imagedelete = [];
    var x = document.getElementsByClassName("prepareDetele");

    let type = 1;
    if ($("#lightgallery_FileEdit").is(":visible")) type = 2;
    if (x.length > 0) {
        for (let i = 0; i < x.length; i++) {
            let element = {};
            element.id = x[i].dataset.id;
            element.value = x[i].dataset.realname;
            element.feaure = x[i].dataset.feature;
            imagedelete.push(element);
        }
        AjaxLoad(url = "/Customer/CustomerImage/?handler=DeleteImage"
            , data = {
                'data': JSON.stringify(imagedelete), "cusID": ser_MainCustomerID
                , "folder": CurrentFolderName_Customer
                , "type": type
            }
            , async = true
            , error = function () { notiError_SW(); }
            , success = function (result) {
                if (result == "1") {

                    CustomerImage_Load(CurrentFolderID_Customer, 0, type);
                    LoadImgByForm(CurrentFolderID_Customer);
                    notiSuccess();
                    syslog_cutimg('d'
                        , (typeof CurrentFolderName_Customer !== 'undefined' && CurrentFolderName_Customer != '') ? CurrentFolderName_Customer : ''
                        , ser_MainCustomerID);
                }
                else {
                    notiError_SW();
                }
                ImageEdit_CancelSH();
            }

        );
    } else {
        $("#delimg_textShowMessage").html(Outlang["Chon_anh_de_xoa"] + "!");
    }
}
function FileEdit_Begin() {
    ImageEdit_BeginSH();
    $("#lightgallery_FileEdit").show();
    $("#lightgallery_File").hide();
    $("#lightgallery").hide();
    $("#seg_image").hide();
    $('#lightgallery_FileEdit').html($('#lightgallery_File').html());
    CustomerImage_Event();
}
// #endregion
//#region // Preview all
function ImagePreview_All () {
    let iszoom = 0;
    if ($('#cbf_preview').attr('data-zoom') == 0) {
        $('#cbf_preview').html(Outlang["Thu_nho"]);
        $('#cbf_preview').attr('data-zoom', 1);
        iszoom = 1;
    }
    else {
        $('#cbf_preview').html(Outlang["Phong_to"]);
        $('#cbf_preview').attr('data-zoom', 0);
        iszoom = 0;
    }
    let _obj = $('#lightgallery .design_img_item');
    _obj.each(function (i, obj) {
        let _real = $(this)[0].href;
        let _tiny = $(this)[0].style.backgroundImage;
        if (iszoom == 1) {
            $(this).css("background-image", `url("${_real}")`);
            $(this).addClass('expand');
        }
        else {
            $(this).removeClass('expand');
        }
       
       
    });
}
//#endregion


//#region // Render Image
function CustomerImage_Load(id, detail_id, type) {
    let idetail = (detail_id != undefined) ? detail_id : 0;
    AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadImageByFolder"
        , data = {
            'currentFolderID': id
            , 'idetail': idetail
            , 'type': type
        }
        , async = false
        , error = function () {
        }
        , success = function (result) {
            let data = JSON.parse(result);
            let dataImage = data.Table; let dataFile = data.Table1;
            if (dataImage != undefined && dataImage.length != 0) {
                if (idetail != 0) {
                    let dateNum = (dataImage[i]?.CreatedNum).toString().substring(0, 8); 
                    if (dataImageCustomer && dataImageCustomer.length > 0 && ConvertYMDInt_ToDate(dataImageCustomer[0]?.CreatedNum) == ConvertYMDInt_ToDate(dataImage[0]?.CreatedNum)) {
                        let el = ImageCustomer_RenderEach(dataImage[0]);
                        $('#CustImage_lightgallery' + dateNum).prepend(el);
                    }
                    else {
                        let el = ImageCustomer_RenderCollapseImg(dataImage[0]);
                        $('#lightgallery').prepend(el);
                    }
                    ImageCustoemr_updateGalery(dataImage[0], 1);
                    dataImageCustomer.push(dataImage[0]);
                }
                else {
                    dataImageCustomer = dataImage;
                    if (dataImage != undefined) {
                        ImageCustomer_Render(dataImage, "lightgallery");
                    }
                }
                ImageCustomer_Ini()
            }
            else {
                if (Number(type == 1)) {
                    dataImageCustomer = [];
                    ImageCustomer_Render(dataImageCustomer, "lightgallery");
                }
            }
            if (dataFile != undefined && dataFile.length != 0) {
                if (idetail != 0) {
                    let el = FileCustomer_RenderEach(dataFile[0]);
                    $('#' + "lightgallery_File").prepend(el);
                    dataFileCustomer.push(dataFile[0])
                }
                else {
                    dataFileCustomer = dataFile;
                    if (data != undefined) {
                        FileCustomer_Render(dataFile, "lightgallery_File");
                    }
                }
            }
            else {
                if (Number(type == 2)) {
                    dataFileCustomer = [];
                    FileCustomer_Render(dataFileCustomer, "lightgallery_File");
                }
            }
            if (id != 0) {
                if ((dataImage.length + dataFile.length) != 0) {
                    $("#item_folder_" + id).html(dataImage.length + dataFile.length + " files ");
                }
                else {
                    $("#item_folder_" + id).html('');
                }
            }
            CustomerImage_GalleryEvent();
        }

    );
}

function FileCustomer_Render(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data.length > 0) {
            for (let i = 0; i < data.length; i++) {
                let el = FileCustomer_RenderEach(data[i]);
                $('#' + id).prepend(el);
            }
        }

    }
}
function FileCustomer_RenderEach(item) {
    let link_file = sys_HTTPImageRoot + item.CustomerID
        + '/' + item.FolderName + '/file/' + item.RealName;
    let typeClass
    switch (item.Type) {
        case 'docx':
            typeClass = 'fa-file-word';
            break;
        case 'doc':
            typeClass = 'fa-file-word';
            break;
        case 'pdf':
            typeClass = 'fa-file-pdf';
            break;
        case 'xlsx':
            typeClass = 'fa-file-excel';
            break;
        case 'xls':
            typeClass = 'fa-file-excel';
            break;
        case 'mp4': case 'wmv': case 'mkv': case 'flv': case 'mpg': case 'mov':
            typeClass = 'fas fa-file-video';
            break;
        case 'mp3': case 'wma': case 'wav': case 'flac': case 'aac': case 'ogg':
            typeClass = 'fas fa-file-audio';
            break;
    }
    let tr = '<a class="border-dashed border-1 border-secondary border-radius-md p-1 pt-2 m-2 file_pdf d-inline-flex" target="_blank" href="'
        + link_file
        + '" data-name="' + item.Name
        + '" data-id="' + item.ID
        + '" data-realname="' + item.RealName
        + '" data-feature="' + ''
        + '" data-created="' + item.Created + '" data-createdby="'
        + item.CreatedBy + '"' + 'data-title="'
        + item.Name + '" data-content="'
        + item.CreatedBy + '\n'
        + item.Created + '">'
        + '<div class="mx-auto">'
        + '<i class="fas ' + typeClass + ' p-2 text-gradient text-info text-xl" aria-hidden="true" style="font-size: 28px;"></i>'
        + '</div>'
        + '<div class="flex-grow-1 px-2">'
        + '<div class="p-md-0 pt-3"> '
        + '<h6 class="text-sm mb-1">' + item.Name + '</h6>'
        + '<p class="text-sm font-weight-bold mb-0">' + item.CreatedBy + '</p>'
        + '<p class="text-sm mb-0">' + item.Created + '</p> '
        + '</div>'

        + '</div>  '
        + '</a>'
    return tr;
}
function ImageCustomer_Render(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        if (data && data.length > 0) {
            var promises = [];
            CIF_dynamicGallery = {};
            for (let i = 0; i < data.length; i++) {
                promises.push(new Promise(resolve => {
                    let el = '';
                    let dateNum = (data[i]?.CreatedNum).toString().substring(0, 8);
                    if (i == 0 || ConvertYMDInt_ToDate(data[i]?.CreatedNum) != ConvertYMDInt_ToDate(data[i - 1]?.CreatedNum)) {
                        el = ImageCustomer_RenderCollapseImg(data[i]);
                        $('#' + id).append(el);
                    }
                    else {
                        el = ImageCustomer_RenderEach(data[i]);
                        $('#CustImage_lightgallery' + dateNum).append(el);
                    }

                    ImageCustoemr_updateGalery(data[i]);
                    CustomerImage_GalleryEvent();
                    resolve();
                }));
            }
        }
        try {
            if (promises == undefined) return;
            Promise.all(promises).then((values) => {
                $(".description-rp").popup({
                    transition: "vertical flip",
                    on: "hover",
                    position: "bottom center"
                });
                if (sys_Custimgwatermark!="")
                $('.cuslightblock').each(function (i, obj) {
                    $(this).on('lgAfterAppendSlide ', function (event) {
                        const {index, prevIndex} = event.originalEvent.detail;
                        let _ob = $(".lg-object.lg-image[data-index='" + index + "']")[0];
                        let canvas = document.createElement('canvas');
                        canvas.className = "canvaswatermark";
                        canvas.height = _ob.height;
                        canvas.width = _ob.width;
                        let ctx3 = canvas.getContext("2d");
                        ctx3.clearRect(0, 0, canvas.width, canvas.height);
                        ctx3.fillStyle = "#99979708";
                        ctx3.fillRect(0, 0, canvas.width, canvas.height);
                        ctx3.fillStyle = "#9e9e9e61";
                        ctx3.font = '50px sans-serif';
                        ctx3.textBaseline = 'middle';
                        ctx3.textAlign = "center";
                        ctx3.fillText(sys_Custimgwatermark, canvas.width / 2, canvas.height / 2);
                        _ob.parentElement.appendChild(canvas);
                    });
                });
                
            });
        } catch (e) {

        }
    }

    ImageCustomer_EventSH();

}
function ImageCustoemr_updateGalery(item, isPop = 0) {
    if (item) {
        let e = {};
        let el = []
        let link_img = sys_HTTPImageRoot + item.CustomerID
            + '/' + item.FolderName + '/' + item.RealName;
        let link_imgthum = sys_HTTPImageRoot + item.CustomerID
            + '/' + item.FolderName + '/' + item.FeatureImage;

        let sub = '<div class="text-sm">' + item.CreatedBy + '</div>'
            + `<div>${GetDateTime_String_DMYHM(ConvertToDateRemove1900(INTYMDHMToDate(item.CreatedNum)) ?? '')}</div>`;
        let dateNum = (item?.CreatedNum).toString().substring(0, 8);
        e["src"] = link_img;
        e["thumb"] = link_imgthum;
        e["subHtml"] = sub;
        el.push(e);
        if (CIF_dynamicGallery[dateNum]) {
            let arrGel = [];
            if (isPop == 1) {
                arrGel = [...el, ...(CIF_dynamicGallery[dateNum]?.galleryItems ?? [])];
            }
            else {
                arrGel =[...(CIF_dynamicGallery[dateNum]?.galleryItems ?? []), ...el]
            }
            CIF_dynamicGallery[dateNum].refresh(arrGel);
        }
        else {
            
            let _objlight = document.getElementById('CustImage_lightgallery' + dateNum);
            var listplugins = [];
            if (Blodcus_blockshare == 1) listplugins = [lgZoom, lgAutoplay, lgFullscreen, lgRotate, lgThumbnail];
            else listplugins = [lgZoom, lgAutoplay, lgFullscreen, lgRotate, lgShare, lgThumbnail];
            CIF_dynamicGallery[dateNum] = lightGallery(_objlight, {
                speed: 300,
                showZoomInOutIcons: true,
                actualSize: false,
                thumbnail: true,
                plugins: listplugins,
                mode: 'lg-fade',
                dynamic: true,
                dynamicEl: el
            })
           
            
        }
    }
}
function ImageCustomer_RenderCollapseImg(item) {
    let tr = '';
    let dateNum = (item?.CreatedNum).toString().substring(0, 8);
    tr = '<div class="accordion" id="CustImage_Accordion' + dateNum + '">'
        + '<div class="accordion-item">'
        + '<p class="accordion-header" id="CustImage_AccHeading' + dateNum + '"> '
        + '<button class="accordion-button font-weight-bold collapsed text-dark" type="button" data-bs-toggle="collapse" '
        + 'data-bs-target="#CustImage_Collapse' + dateNum + '" aria-expanded="true" aria-controls="CustImage_Collapse' + dateNum + '">'
        + '<span class="text-sm text-dark">' + ConvertYMDInt_ToDate(item?.CreatedNum) + '</span>'
        + '<i class="collapse-close fas fa-chevron-down text-xs pt-1 position-absolute start-0 mb-1" aria-hidden="true"></i>'
        + '<i class="collapse-open fas fa-chevron-up text-xs pt-1 position-absolute start-0 mb-1" aria-hidden="true"></i>'
        + '</button>'
        
        + '</p>'
        + '<hr class="horizontal dark my-0">'
        + '<div id="CustImage_Collapse' + dateNum + '" class="accordion-collapse collapse collapsesticky show" aria-labelledby="CustImage_AccHeading' + dateNum + '" data-bs-parent="#CustImage_Accordion' + dateNum + '" style="">'
        + '<div class="accordion-body text-sm py-0">'
        + '<div class="cuslightblock" id="CustImage_lightgallery' + dateNum + '">'
        + ImageCustomer_RenderEach(item)
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>';
    return tr;
}

function ImageCustomer_EventSH() {
    $('.design_img_item .hide').unbind().click(function (e) {
        let id = $(this).attr('data-id');
        ImageCustomer_ExeSH(id);
        e.preventDefault();
        e.stopImmediatePropagation();
    });
    $('.design_img_item .show').unbind().click(function (e) {
        let id = $(this).attr('data-id');
        ImageCustomer_ExeSH(id);
        e.preventDefault();
        e.stopImmediatePropagation();
    });
}
function ImageCustomer_ExeSH(id) {
    AjaxLoad(url = "/Customer/CustomerImage/?handler=ExeSH"
        , data = {
            'imgid': id
        }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data.length > 0) {
                    let item = data[0];
                    let button = ImageCustomer_RenderIconSH(item.ID, item.MarkVisible);
                    if ($("#appsh_" + item.ID).length) {
                        $("#appsh_" + item.ID).replaceWith(button);
                        ImageCustomer_EventSH();
                    }
                }
            }
        }
    );
}
function ImageCustomer_RenderEach(item) {
    let button = ImageCustomer_RenderIconSH(item.ID, item.MarkVisible)
    let link_img = sys_HTTPImageRoot + item.CustomerID
        + '/' + item.FolderName + '/' + item.RealName;
    let link_feaute = sys_HTTPImageRoot + item.CustomerID
        + '/' + item.FolderName + '/' + item.FeatureImage;

    let link_imgthum = "'" + link_feaute + "'";
    let sub = '<div>' + item.CreatedBy + '</div>'
        + `<div>${GetDateTime_String_DMYHM(ConvertToDateRemove1900(INTYMDHMToDate(item.CreatedNum)) ?? '')}</div>`;
    let dateNum = (item?.CreatedNum).toString().substring(0, 8);
    let tr = '<a data-sub-html ="' + sub + '" data-id="' + item.ID + '" class="description-rp design_img_item CustImage_clsLightGallery" data-numCreated="' + dateNum + '" href="' + link_img
        + '" data-name="' + item.Name
        + '" data-realname="' + item.RealName
        + '" data-feature="' + item.FeatureImage

        + '" data-created="' + item.Created
        + '" data-content="' + item.Description + '" data-variation="inverted blueli" data-createdby="'
        + item.Name + '" style="background-image: url(' + link_imgthum + ')">'
        + button
        + '<img class="imgload" style="width: 0px;height: 0px;object-fit: cover;opacity: 0;"  src="' + link_img + '"/> '
        + '</a>';
    return tr;
}
function ImageCustomer_RenderIconSH(ID, MarkVisible) {
    //0: Phải chọn mới show, 1: phải chọn mới ẩn ( hình ảnh)
    let status = false;
    let result = '';
    if (Clicktoshow == "-1") {
        return result;
    }
    else {
        if (Clicktoshow == "0") {
            if (MarkVisible == "-1" || MarkVisible == "0") status = false;
            else status = true;
        }
        else {
            if (MarkVisible == "-1" || MarkVisible == "0") status = true;
            else status = false;

        }
    }
    if (status) result = `<i id="appsh_${ID}" data-id="${ID}" class="sh show"></i>`;
    else result = `<i id="appsh_${ID}" data-id="${ID}" class="sh hide"></i>`;
    return result;
}
function Image_BeginUpload(name) {
    if ($('#lightgallery').find('[data-name="' + name + '"]').length == 0) {
        let tr = '<a class="before_load"  data-name="' + name + '">'
            + '<div class="before_load_absolute"></div>'
            + '</a>';
        $('#lightgallery').prepend(tr);
        CustomerImage_GalleryEvent();
    }
}
function Image_FinishUpload(name) {
    $('#lightgallery').find('[data-name="' + name + '"]').remove();
}
function Image_ErrorUpload(name) {
    $('#lightgallery').find('[data-name="' + name + '"]')
        .removeClass('before_load')
        .addClass('error_load');
}

//#endregion

//#region // Folder
function Preapare_SH_FolderCustomer() {
    CurrentFolderName_Customer = '';
    $(".actionImg").hide();
    if (document.getElementById("uploadButton")) document.getElementById("uploadButton").style.display = "none";
    if (document.getElementById("btnEditImage")) document.getElementById("btnEditImage").style.display = "none";
    if (document.getElementById("btnSetImage")) document.getElementById("btnSetImage").style.display = "none";
    if (document.getElementById("btnEditImageOK")) document.getElementById("btnEditImageOK").style.display = "none";
    if (document.getElementById("btnEditImageCancel")) document.getElementById("btnEditImageCancel").style.display = "none";

    $(".actionFile").hide();
    if (document.getElementById("uploadButton_File")) document.getElementById("uploadButton_File").style.display = "none";
    if (document.getElementById("btnEditImage_File")) document.getElementById("btnEditImage_File").style.display = "none";
    if (document.getElementById("btnEditImageOK_File")) document.getElementById("btnEditImageOK_File").style.display = "none";
    if (document.getElementById("btnEditImageCancel_File")) document.getElementById("btnEditImageCancel_File").style.display = "none";
}
function Done_SH_FolderCustomer() {
    $("#lightgallery").show();
    $("#noticeDelete").hide();
    $("#lightgallery_File").show();
    $("#uploadButton_File").show();
    $("#btnEditImage_File").show();
    $("#uploadButton").show();
    $("#btnEditImage").show();
    $("#btnSetImage").show();
}
function Event_Click_Folder_Customer() {
    $('#TreeFolderName_Customer .foldername').unbind().click(function () {
        CurrentFolderID_Customer = $(this).attr('data_id');

        ContentIMG(CurrentFolderID_Customer);

        $('#TreeFolderName_Customer .foldername').each(function () {
            $(this).removeClass('active');
        })
        $(this).addClass('active');

        $('#seg_image').show();
        $('#seg_file').show();
        var textTab = $("#TreeFolderName_Customer .foldername.active")[0].getAttribute('data-foldername');
        var CurrentFolderID = $("#TreeFolderName_Customer .foldername.active")[0].getAttribute('data_id');

        if (textTab) {
            Done_SH_FolderCustomer();
        }
        else {
            textTab = "Image";
        }
        CurrentFolderName_Customer = textTab;
        $('.btn-sort').each(function () {
            $(this).removeClass('active');
            $(this).css('pointer-events', 'unset');
        })
        link_Upload_Image = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID + '&FolderName=' + CurrentFolderName_Customer + '&FolderID=' + CurrentFolderID + '&Type=Image';
        link_Upload_File = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID
            + '&FolderName=' + CurrentFolderName_Customer
            + '&FolderNameChild=' + "File"
            + '&Type=AttachmentFile';
        LoadImgByForm(CurrentFolderID_Customer);
    });
}
function Event_Click_Folder_Cust_TreatPlan() {
    $('#FolderTreatmentPlan .item').unbind().click(function () {
        CurrentFolderID_Customer = $(this).attr('data_id')
        ContentIMGByTreatPlan(CurrentFolderID_Customer);
        $('#FolderTreatmentPlan .item').each(function () {
            $(this).removeClass('active');
        })
        $(this).addClass('active');
        var textTab = $("#FolderTreatmentPlan .item.active")[0].getAttribute('data-foldername');
        var CurrentFolderID = $("#FolderTreatmentPlan .item.active")[0].getAttribute('data_id');
        if (textTab) {
            Done_SH_FolderCustomer();
        }
        else {
            textTab = "Image";
        }
        CurrentFolderName_Customer = textTab;
        $('.btn-sort').each(function () {
            $(this).removeClass('active');
            $(this).css('pointer-events', 'unset');
        })
        link_Upload_Image = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID + '&FolderName=' + CurrentFolderName_Customer + '&FolderID=' + CurrentFolderID + '&Type=Image';
        link_Upload_File = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID
            + '&FolderName=' + CurrentFolderName_Customer
            + '&FolderNameChild=' + "File"
            + '&Type=AttachmentFile';


        LoadImgByForm(CurrentFolderID_Customer);
    });
}
function EventClick_Folder_Edit() {
    $(".image_edit_folder").on('click', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        editFolder_ImageCustomer($(this).attr('data_id'), $(this).attr('data_name'));
    });
    $(".image_delete_folder").on('click', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        DeleteFolderImage_Customer($(this).attr('data_id'), $(this).attr('data_name'));
    });

}
function DeleteFolderImage_Customer(id, name) {
    const promise = notiConfirm();
    promise.then(function () { ExecuteDeleteFolderImage_Customer(id, name); }, function () { });
}
function ExecuteDeleteFolderImage_Customer(id, name) {
    AjaxLoad(url = "/Customer/CustomerImage/?handler=DeleteFolder"
        , data = { 'customer': ser_MainCustomerID, 'id': id, 'name': name }
        , async = true
        , error = function () { notiError_SW(); }
        , success = function (result) {
            if (result == "1") {
                notiSuccess();
                LoadFolderTree_Customer_File_Image();
                syslog_cutimgfo('d'
                    , name
                    , ser_MainCustomerID);
            } else {
                notiError_SW();
            }
        }
    );


}
function RenFolderImage_Customer_File_Image(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                
                let tr =
                    ` <li class="nav-item m-1 align-items-center ">
                        <a id="folder_image_${item.ID}" data_id="${item.ID}" data-foldername="${item.FolderName}" class="foldername text-sm item-menu detail nav-link cursor-pointer" data-hover data-bs-toggle="tab">
                            <div class="d-flex">
                                <div class="w-90">
                                    <p class="mb-0 fw-bold ellipsis_two_line text-dark text-sm">
                                        ${item.FolderName}
                                    </p>
                                    <span class="text-primary fw-bold" id="item_folder_${item.ID}">${item.Number != "0" ? `${item.Number}` : ``}</span>
                                    ${item.Note != "" ? `<span class="mb-0 fst-italic ellipsis_two_line text-dark text-sm pt-0">${item.Note}</span>` : ``}
                                </div>
                                <div data-bs-toggle="collapse" href="#folder${item.ID}" class="cursor-pointer ps-3 opacity-10 ms-auto" data-bs-toggle="tooltip" data-bs-placement="right" >
                                    <i class="fas fa-chevron-down fs-6"></i>
                                </div>
                            </div>
                            <div class="collapse" id="folder${item.ID}">
                                <div class="d-flex">
                                    <button data_name="${item.FolderName}" data_id="${item.ID}" class="image_edit_folder btn btn-dark btn-sm mt-2 ms-auto mx-1">${Outlang["Sua"]}</button>
                                    <button data_name="${item.FolderName}" data_id="${item.ID}" class="image_delete_folder btn btn-danger btn-sm mt-2 mx-1">${Outlang["Xoa"]}</button>
                                </div>
                            </div>
                        </a>
                        
                        <hr class="horizontal dark my-0 opacity-2">
                    </li>`;
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
    EventClick_Folder_Edit();
}
//#endregion

//#region // Sort
$('#list-button_sort').on('click', '.btn-sort', function (e) {
    $(this).addClass('active').siblings().removeClass('active');
})

function SortAscending() {
    let filterFile;
    let filterImage;

    filterFile = dataFileCustomer.sort(function (a, b) {
        var name1 = a.Name.toLowerCase();
        var name2 = b.Name.toLowerCase();
        return name1 < name2 ? -1 : name1 > name2 ? 1 : 0;
    })
    filterImage = dataImageCustomer.sort(function (a, b) {
        var name1 = a.Name.toLowerCase();
        var name2 = b.Name.toLowerCase();
        return name1 < name2 ? -1 : name1 > name2 ? 1 : 0;
    })
    FileCustomer_Render(filterFile, "lightgallery_File");
    ImageCustomer_Render(filterImage, "lightgallery");

}
function SortDescending() {
    let filterFile;
    let filterImage;
    filterFile = dataFileCustomer.sort(function (a, b) {
        var name1 = a.Name.toLowerCase();
        var name2 = b.Name.toLowerCase();
        return name1 > name2 ? -1 : name1 < name2 ? 1 : 0;
    })
    filterImage = dataImageCustomer.sort(function (a, b) {
        var name1 = a.Name.toLowerCase();
        var name2 = b.Name.toLowerCase();
        return name1 > name2 ? -1 : name1 < name2 ? 1 : 0;
    })
    FileCustomer_Render(filterFile, "lightgallery_File");
    ImageCustomer_Render(filterImage, "lightgallery");

}
function SortDateAscending() {
    try {
        let filterFile;
        let filterImage;
        filterFile = dataFileCustomer.sort(function (a, b) {
            var date1 = a.CreatedNum;
            var date2 = b.CreatedNum;
            return date1 < date2 ? -1 : date1 > date2 ? 1 : 0;
        })
        filterImage = dataImageCustomer.sort(function (a, b) {
            var date1 = a.CreatedNum;
            var date2 = b.CreatedNum;
            return date1 < date2 ? -1 : date1 > date2 ? 1 : 0;
        })
        FileCustomer_Render(filterFile, "lightgallery_File");
        ImageCustomer_Render(filterImage, "lightgallery");

    } catch (ex) { }
}
function HideShowLayout() {
    if ($('#form_area').is(":visible")) {
        $('#form_area').hide();
        $('#hideshow_layout .fa-compress').show();
        $('#hideshow_layout .fa-expand').hide();
    }
    else {
        $('#form_area').show();
        $('#hideshow_layout .fa-compress').hide();
        $('#hideshow_layout .fa-expand').show();
    }
}

function SortDateDescending() {
    try {
        let filterFile;
        let filterImage;
        filterFile = dataFileCustomer.sort(function (a, b) {
            var date1 = a.CreatedNum;
            var date2 = b.CreatedNum;
            return date1 > date2 ? -1 : date1 < date2 ? 1 : 0;
        })
        filterImage = dataImageCustomer.sort(function (a, b) {
            var date1 = a.CreatedNum;
            var date2 = b.CreatedNum;
            return date1 > date2 ? -1 : date1 < date2 ? 1 : 0;
        })
        FileCustomer_Render(filterFile, "lightgallery_File");
        ImageCustomer_Render(filterImage, "lightgallery");
    } catch (ex) { }
}
//#endregion

//#region // Load form template image
function LoadTemplateForm() {
    AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadTemplateForm"
        , data = {
        }
        , async = false
        , error = function () { notiError_SW(); }
        , success = function (result) {
            let item = JSON.parse(result);
            if (item.length > 0) {
                DataTemplateList_Default = item;
                ReloadTemplateForm();
            }
        }
    );
}

function ReloadTemplateForm() {
    if (CurrentFolderName_Customer != '' && DataTemplateList_Default.length > 0) {
        let NewDataTemplateList = [];
        let item = DataTemplateList_Default;
        for (let i = 0; i < item.length; i++) {
            let _temp = DataTemplateList.filter(word => word["TemplateID"] == item[i].TemplateID)
            if (_temp.length == 0) {
                NewDataTemplateList.push(item[i]);
            } else if (_temp.length > 0) {
                NewDataTemplateList.push(_temp[0]);
            }
        }

        DataTemplateList = NewDataTemplateList;

        if (DataTemplateList != undefined && DataTemplateList.length > 0) {
            RenderTemplateList(DataTemplateList, "TemplateList");
            for (let i = 0; i < DataTemplateList.length; i++) {
                if (DataTemplateList[i].ID) {
                    TemplateID = DataTemplateList[i].TemplateID;
                    break;
                }
            }
            if (TemplateID == 0) {
                TemplateID = DataTemplateList[0].TemplateID;
            }
            RenderDataTemplate(TemplateID);
        } else {
        }

    }
}

function RenderDataTemplate(T_ID) {
    if (DataTemplateList.length > 0) {
        let temp = DataTemplateList.filter(word => word["TemplateID"] == T_ID)[0];
        TemplateID = T_ID;
        DataTemplate = JSON.parse(temp.DataTemplate);
    }
    RenderImgByForm(DataTemplate, "templateForm");
}

function RenderTemplateList(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            let isChoose_ID = 0;
            let first = 0;
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let exist = "";
                if (item.ID) {
                    exist = "exist";
                }
                if (item.ID && first == 0) {
                    isChoose_ID = item.ID;
                    first = 1;
                }
                let tr = '<li class="nav-item"><a  data-id="' + item.TemplateID + '" class="text-sm px-2 tempItem ' + exist + (isChoose_ID == item.ID ? " active" : "") + ' text-sm p-1 ms-2">' + item.Name + '</a></li>'

                // let tr = '<div class="tempItem ui button ' + exist + (isChoose_ID == item.ID ? " ischoose" : "") + '" data-id="' + item.TemplateID + '">' + item.Name + '</div>';
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}

function RenderImgByForm(dtTemplate, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        let Data = dtTemplate;
        if (Data) {
            let key = 0;
            for (let i = 0; i < Data.rows; i++) {
                let div = '';
                let hr = '<hr class="horizontal m-0 p-0">';
                for (let j = 0; j < Data.columns; j++) {
                    let item = Data.data[key].content;
                    let header = (Data.data[key].header != "" && item.x != 0)
                        ? '<span class="mx-2 mb-2  d-flex justify-content-center text-dark" style="">' + Data.data[key].header + '</span>'
                        : '';
                    let hasHeader = (Data.data[key].header != "" && Data.position != "left") ? 'header_template' : '';
                    if (item.value == 1) {
                        let img = "";

                        if (dataImageCustomer != undefined && dataImageCustomer.find(x => x.ID == item.img) != undefined) {
                            let ImgCurrent = dataImageCustomer.find(x => x.ID == item.img);

                            let link_img = sys_HTTPImageRoot + ImgCurrent.CustomerID + '/' + ImgCurrent.FolderName + '/' + ImgCurrent.RealName;
                            img =
                                '<img class="imgform" src="' + link_img + '">'
                                + '<i data-id="' + ImgCurrent.ID + '" data-x="' + item.x + '" data-y="' + item.y + '" class="text-xs text-gradient text-danger button-delete-img-form vtt-icon vttech-icon-cancel-01"></i>'

                        } else {
                            img = '<div class="text-dark m-auto text_img text-center">' + item.placehoder + '</div>'
                        }
                        let style = (Data.position == "left" ? 'style="display:flex;"' : '');
                        let style_img = '';
                        div += '<div ' + style + ' class="d-inline-block img-content ' + hasHeader + '">' + header + '<div ' + style_img + ' data-x="' + item.x + '" data-y="' + item.y + '" class="selectdrop" >' + img + '</div></div>'
                    } else {
                        div += '<div class="d-inline-block img-content">' + header + '<div class="notdrop"></div></div>'
                    }

                    key++;
                }
                stringContent += hr + div;
            }
            $("#TemplateFormMain").show();
            $("#btnSetImage").attr("disabled", true);
        } else {
            $("#TemplateFormMain").hide();
            $("#btnSetImage").attr("disabled", false);
        }
        document.getElementById(id).innerHTML = stringContent;
    }

    //$(".selectdrop").lightGallery();
    DragDropEvent_Item_Img();
}


function LoadImgByForm(FolderID) {
    if (CurrentFolderName_Customer != '') {
        AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadImgByForm"
            , data = { 'FolderID': FolderID }
            , async = true
            , error = function () { notiError_SW(); }
            , success = function (result) {
                DataTemplateList = JSON.parse(result);
                let data = DataTemplateList[0];
                if (data != undefined) {
                    TemplateID = data.TemplateID;
                    if (data.DataTemplate) {
                        DataTemplate = JSON.parse(data.DataTemplate);
                    }
                    //ReloadTemplateForm();
                    LoadTemplateForm();
                    RenderImgByForm(DataTemplate, "templateForm");

                    EventImageInFormGalerryClick();
                } else {
                    $("#btnSetImage").attr("disabled", false);
                    $("#TemplateFormMain").hide();
                    $("#templateForm").empty();
                }
            }

        );


    }
}

function DragDropEvent_Item_Img() {
    $(".design_img_item").draggable({
        scroll: false,
        //axis: "x",
        containment: "#seg_image",
        helper: "clone",
        disable: false,
        start: function (event, ui) {
        },
        drag: function (event, ui) {
        },
        stop: function (event, ui) {
        }
    });

    $(".selectdrop").droppable({
        accept: ".design_img_item",
        class: {
            "ui-droppable-active": "ac",
            "ui-droppable-hover": "hv"
        },
        acivate: function (event, ui) {
        },
        over: function (event, ui) {
        },
        out: function (event, ui) {
        },
        drop: function (event, ui) {
            let idImg = ui.helper.attr('data-id');
            let x = $(this).attr('data-x');
            let y = $(this).attr('data-y');

            Add_Item_Image(idImg, x, y);
        },
        deactivate: function (event, ui) {
        },
    });
}

function Add_Item_Image(idImg, x, y) {
    if (DataTemplate) {
        let key = 0;
        for (var i = 0; i < DataTemplate.rows; i++) {
            for (var j = 0; j < DataTemplate.columns; j++) {
                let item = DataTemplate.data[key].content;
                if (item.x == x && item.y == y) {
                    item.img = idImg;
                }
                key++;
            }
        }
        ExecuteImgForm();
    }
}

function Img_Noti_Success_Save() {
    let _obj = document.getElementById('img_noti_success');
    _obj.classList.remove('show');
    void _obj.offsetWidth;
    _obj.classList.add('show');
}
function Img_Noti_Failure_Save() {
    let _obj = document.getElementById('img_noti_success');
    _obj.classList.remove('show');
    void _obj.offsetWidth;
    _obj.classList.add('show');
}

function ExecuteImgForm() {
    if (CurrentFolderID_Customer != '' && DataTemplate != undefined) {
        AjaxLoad(url = "/Customer/CustomerImage/?handler=InsertImgForm"
            , data = {
                'folderid': CurrentFolderID_Customer
                , 'templateid': TemplateID
                , 'data': JSON.stringify(DataTemplate)
            }
            , async = true
            , error = function () { notiError_SW(); }
            , success = function (result) {
                if (result == "1") {
                    Img_Noti_Success_Save();
                    ReloadImgTemplateList();
                    RenderImgByForm(DataTemplate, "templateForm");
                } else {
                    Img_Noti_Failure_Save();
                }
            }

        );


    }
}

function ReloadImgTemplateList() {
    if (CurrentFolderID_Customer != '' && DataTemplate != undefined && DataTemplateList.length) {
        for (let i = 0; i < DataTemplateList.length; i++) {
            if (DataTemplateList[i].TemplateID == TemplateID) {
                DataTemplateList[i].DataTemplate = JSON.stringify(DataTemplate);
            }
        }
    }
}

function EventImageInFormGalerryClick() {
    var $customImgFormEvents = $('#templateForm');
    $customImgFormEvents.on('onAfterOpen.lg', function (event, prevIndex, index) {

        try {
            _dim_galery_3 = document.getElementsByClassName('lg-backdrop')[0];
            _dim_galery_4 = document.getElementsByClassName('lg')[0];
            _dim_galery_3.style.display = "none";
            _dim_galery_4.style.display = "none";
            setTimeout(function () {
                if (CurrentEditImageForm == 0) {
                    _dim_galery_3.style.display = "block";
                    _dim_galery_4.style.display = "block";
                }
            }, 300);
        }
        catch (ex) {

        }
    });

    $("#templateForm").on('click', '.button-delete-img-form', function (e) {
        CurrentEditImageForm = 1;
        Remove_Item_Image($(this).attr('data-x'), $(this).attr('data-y'));
        e.stopImmediatePropagation();
    });
}
function Remove_Item_Image(x, y) {
    if (DataTemplate) {
        let key = 0;
        for (var i = 0; i < DataTemplate.rows; i++) {
            for (var j = 0; j < DataTemplate.columns; j++) {
                let item = DataTemplate.data[key].content;
                if (item.x == x && item.y == y) {
                    item.img = "";
                }
                key++;
            }
        }
        ExecuteImgForm();
    }
}
//#endregion // Load form template image

//#region //upload customer img

function AjaxUpload_CustLocalImg(url, inputid, isresize, success, error, before, complete, funmaxrange) {

    $('#' + inputid).unbind("change");
    $('#' + inputid).change(function () {
        var promises = [];
        let input = document.getElementById(inputid);
        let files = input.files;
        if (files.length <= 0 || files.length > 5) {
            if (funmaxrange != undefined && funmaxrange != null)
                funmaxrange();
        }
        else {
            for (let i = 0; i != files.length; i++) {
                let filesize = files[i].size;
                let fileext = files[i]?.name?.split('.')?.pop();
                let fileiphone = [".heif", ".heic"];
                let sizeInMB = (filesize / (1024 * 1024)).toFixed(2);
 
                if (sizeInMB < 2 || $.inArray(fileext, fileiphone)!=-1 || isresize == 0) {
                    promises.push(AjaxUpload_CustLocalImgExe(url, files[i], files[i].name, files[i].size, success, error, before));
                }
                else {
                    if (isresize == 1) {
                        CustLocalImagesize_Resize(files[i], before, files[i].name
                            , function (resizedImage, _filename) {
                                let _refile = new File([resizedImage], _filename, {
                                    type: resizedImage.type,
                                });
                                promises.push(AjaxUpload_CustLocalImgExe(url, _refile, _filename, _refile.size, success, error, before));
                            }
                        )
                    }
                }
            }
            Promise.all(promises).then((values) => { });
        }
    });
}
async function AjaxUpload_CustLocalImgExe(url, file, namefile, filesize, success, error, before) {
    return new Promise(resolve => {
        let formData = new FormData();
        formData.append("files", file);
        $.ajax(
            {
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (error != undefined && error != null && error.length != 0) error(namefile);
                },
                success: function (result, e) {
                    if (result != undefined) {
                        if (success != undefined && success != null && success.length != 0)
                            success(result, namefile, filesize);
                    }
                    resolve(true);
                },
                beforeSend: function (xhr, e) {
                    xhr.setRequestHeader("Authorization", "Bearer " + (localStorage.getItem("WebToken") != "" ? localStorage.getItem("WebToken") : getCookie("WebToken")));
                    if (before != undefined && before != null && before.length != 0)
                        before(namefile);
                }
            }
        );
    });
}
async function CustLocalImagesize_Resize(fileitem, before, namefile, onCallback) {
    return new Promise(resolve => {
        before(namefile);
        var reader = new FileReader();
        reader.onload = function (readerEvent) {
            var image = new Image();
            image.onload = function () {
                var canvas = document.createElement('canvas'),
                    max_size = Blodcusmax_size,
                    width = image.width,
                    height = image.height;
      
                if (width > height) {
                    if (width > max_size) {
                        height *= max_size / width;
                        width = max_size;
                    }
                } else {
                    if (height > max_size) {
                        width *= max_size / height;
                        height = max_size;
                    }
                }
 
                canvas.width = width;
                canvas.height = height;
                canvas.getContext('2d').drawImage(image, 0, 0, width, height);
                var dataUrl = canvas.toDataURL('image/jpeg');
                var resizedImage = dataURLToBlob(dataUrl);
                onCallback(resizedImage, namefile);
            }
            image.onerror = function (e) {
                debugger
            }
            image.src = readerEvent.target.result;
        }
        reader.readAsDataURL(fileitem);
    });
}
var dataURLToBlob = function (dataURL) {
    var BASE64_MARKER = ';base64,';
    if (dataURL.indexOf(BASE64_MARKER) == -1) {
        var parts = dataURL.split(',');
        var contentType = parts[0].split(':')[1];
        var raw = parts[1];

        return new Blob([raw], { type: contentType });
    }

    var parts = dataURL.split(BASE64_MARKER);
    var contentType = parts[0].split(':')[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;

    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}
//#endregion