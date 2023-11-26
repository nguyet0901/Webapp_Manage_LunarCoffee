var xhrRequest_Image;
var xhrRequest_File;
var dataImageCustomer;
var dataFileCustomer = [];
var CurrentEditDescriptionImage = 0;
var _dim_galery_1, _dim_galery_2

var _dim_galery_3, _dim_galery_4
var CurrentEditImageForm = 0;
var dataImageForm;
var DataTemplateList = [];
var DataTemplate = [];
var TemplateID = 0;


// #region Check limit for demo
function MinusLimitIfExist_ForDemo() {
   AjaxLoad(url = "/Customer/CustomerImage/?handler=MinusLimitIfExist_ForDemo"
      , data = {}
      , async = true
      , error = function () { notiError_SW(); }
      , success = function (result) {
         CheckLimitUpload_ForDemo_ForDemo();
      }

   );


}
function CheckLimitUpload_ForDemo_ForDemo() {
   AjaxLoad(url = "/Customer/CustomerImage/?handler=CheckLimit"
      , data = {}
      , async = true
      , error = function () { notiError_SW(); }
      , success = function (result) {
         if (result != "1") {
            $('#uploadButton').empty();
            document.getElementById("uploadButton").innerHTML = "Hết Số Lượt Upload";
         }
      }

   );
}

// #endregion
// #region EditImage

function EditImageGallery() {
   if (CurrentFolderName_Customer != '') {
      AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadImageByFolder"
         , data = { 'currentFolderID': CurrentFolderID_Customer }
         , async = true
         , error = function () { notiError_SW(); }
         , success = function (result) {
            let data = JSON.parse(result);
            if (data != undefined) {
               RenderImageGalleryToDelete(data, "lightgalleryTableEdit");
               Prepare_SH_WhenEdit_Image();
            }
         }

      );

   }
}
function RenderImageGalleryToDelete(data, id) {
   var myNode = document.getElementById(id);
   if (myNode != null) {
      myNode.innerHTML = '';
      let stringContent = '';
      if (data && data.length > 0) {
         let index = 0;

         for (var i = 0; i < data.length; i++) {
            let tr = "";
            let item = data[i];

            tr = tr + '<div class="ui segments image_edit_parent">'
               + '<div class="ui segment">'
               + '<img style="width: 100px;height: 100px;object-fit: cover;" src="' + item.link + '" />'

               + '</div>'
               + '<div class="ui segment" style="text-align:center;">'
               + '<div class="ui checkbox"><input data_id="' + item.id + '" data_name="' + item.realname + '" type = "checkbox"  name = "checkImageDelete"><label class="coloring red"></label></div>'
               + '</div>'
               + '</div>'

            stringContent = stringContent + tr;
         }
      }
      document.getElementById(id).innerHTML = stringContent;
   }
}
function CanceEditImage() {
   Done_SH_WhenEdit_Image();
}
function SaveEditImage() {
   let imagedelete = [];
   var x = document.getElementsByName("checkImageDelete");
   for (let i = 0; i < x.length; i++) {
      if (x[i].checked) {
         let element = {};
         element.id = x[i].attributes.data_id.value;
         element.value = x[i].attributes.data_name.value;
         imagedelete.push(element);
      }
   }
   AjaxLoad(url = "/Customer/CustomerImage/?handler=DeleteImage"
      , data = { 'data': JSON.stringify(imagedelete), "cusID": ser_MainCustomerID, "folder": CurrentFolderName_Customer }
      , async = true
      , error = function () { notiError_SW(); }
      , success = function (result) {
         if (result == "1") {
            Done_SH_WhenEdit_Image();
            Load_Image_Customer(CurrentFolderID_Customer);
            LoadImgByForm(CurrentFolderID_Customer);
            notiSuccess();
         }
         else {
            notiError_SW();
         }
      }

   );



}

// #endregion
// #region EditFile
function EditFileGallery() {
   if (CurrentFolderName_Customer != '') {
      AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadFileByFolder"
         , data = { 'currentFolderID': CurrentFolderID_Customer }
         , async = true
         , error = function () { notiError_SW(); }
         , success = function (result) {
            let data = JSON.parse(result);
            if (data != undefined) {
               RenderFileGalleryToDelete(data, "lightgalleryTableEdit_File");
               Prepare_SH_WhenEdit_File();
            }
         }

      );

   }
}
function CanceEditFile() {
   Done_SH_WhenEdit_File();
}
function SaveEditFile() {


   let imagedelete = [];
   var x = document.getElementsByName("checkFileDelete");
   for (let i = 0; i < x.length; i++) {
      if (x[i].checked) {
         let element = {};
         element.id = x[i].attributes.data_id.value;
         element.value = x[i].attributes.data_name.value;
         imagedelete.push(element);
      }
   }
   AjaxLoad(url = "/Customer/CustomerImage/?handler=DeleteFile"
      , data = { 'data': JSON.stringify(imagedelete), "cusID": ser_MainCustomerID, "folder": CurrentFolderName_Customer }
      , async = true
      , error = function () { notiError_SW(); }
      , success = function (result) {
         if (result == "1") {
            Done_SH_WhenEdit_File();
            Load_File_Customer(CurrentFolderID_Customer);
            notiSuccess();
         }
         else {
            notiError_SW();
         }
      }

   );




}
function RenderFileGalleryToDelete(data, id) {
   var myNode = document.getElementById(id);
   if (myNode != null) {
      myNode.innerHTML = '';
      let stringContent = '';
      if (data && data.length > 0) {

         for (var i = 0; i < data.length; i++) {
            let tr = "";
            let item = data[i];

            tr = tr + '<div class="ui segments file_edit_parent">'
               + '<div class="ui segment file_edit_child">'
               + GetImageTypeFile(item.type)
               + '<div class="fileCustomer">'
               + '<span>' + item.name + '</span></br>'
               + '<span>' + item.createdby + '</span></br>'
               + '<span>' + item.created + '</span>'
               + '</div>'
               + '</div>'
               + '<div class="ui segment" style="text-align:center;">'
               + '<div class="ui checkbox"><input data_id="' + item.id + '" data_name="' + item.realname + '" type = "checkbox"  name = "checkFileDelete"><label class="coloring red"></label></div>'
               + '</div>'
               + '</div>'

            stringContent = stringContent + tr;
         }
      }
      document.getElementById(id).innerHTML = stringContent;
   }
}

// #endregion
// #region Load

// Load Image
function Load_Image_Customer(id) {
   if (xhrRequest_Image && xhrRequest_Image.readyState != 4) {
      xhrRequest_Image.abort();
   }
   $('#lightgallery').hide();
   countImage = 0;
   xhrRequest_Image = AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadImageByFolder"
      , data = { 'currentFolderID': id }
      , async = false
      , error = function () {
         if (xhrRequest_Image && xhrRequest_Image.readyState != 4) {
            xhrRequest_Image.abort();
         }
      }
      , success = function (result) {
         let data = JSON.parse(result);
         dataImageCustomer = data;
         if (data != undefined) {
            RenderImageGallery(data, "lightgallery");
            EventImageGalerryClick();
            lightGallery(document.getElementById('lightgallery'));
            $('#lightgallery').show();

            countImage += data.length;
            DragDropEvent_Item_Img();
         }
         let count = countFile + countImage;
         if (count != 0) {
            $('#folder-' + id).text(count);
            $('#folder-' + id).css('display', 'block');
         }
      }

   );




}

function EventImageGalerryClick() {
   var $customEvents = $('#lightgallery');
   $customEvents.on('onAfterOpen.lg', function (event, prevIndex, index) {

      try {
         _dim_galery_1 = document.getElementsByClassName('lg-backdrop')[0];
         _dim_galery_2 = document.getElementsByClassName('lg')[0];
         _dim_galery_1.style.display = "none";
         _dim_galery_2.style.display = "none";
         setTimeout(function () {
            if (CurrentEditDescriptionImage == 0) {
               _dim_galery_1.style.display = "block";
               _dim_galery_2.style.display = "block";
            }
         }, 300);
      }
      catch (ex) {

      }
   });

   $("#lightgallery").on('click', '.button-edit-des', function (e) {
      CurrentEditDescriptionImage = 1;
      editDesciption_ImageCustomer($(this).attr('data-id'));
      e.stopImmediatePropagation();
   });

}
function RenderImageGallery(data, id) {
   var myNode = document.getElementById(id);
   if (myNode != null) {
      myNode.innerHTML = '';
      let stringContent = '';
      if (data && data.length > 0) {
         for (var i = 0; i < data.length; i++) {
            let item = data[i];
            let link_img = "'" + item.link + "'";
            let tr = '<a data-id="' + item.id + '" class="description-rp design_img_item" href="'
               + item.link + '" data-name="'
               + item.name + '" data-created="'
               + item.created + '" data-content="' + item.description + '" data-variation="inverted blueli" data-createdby="'
               + item.name + '" style="background-image: url(' + link_img + ')"><img style="width: 0px;height: 0px;object-fit: cover;opacity: 0;" src="' + item.link + '"/><i data-id="' + item.id + '" class="pencil alternate icon button-edit-des"></i>'
               + '</a>'
            //+ '<span class="ui fluid popup content-rp">' + item.description + '</span>'
            stringContent = stringContent + tr;
         }
      }
      else {
         stringContent = '<div class="sheet_cart_load_empty" style="pointer-events: none;" > no content </div>'
      }
      document.getElementById(id).innerHTML = stringContent;
      $(".description-rp").popup({
         transition: "vertical flip",
         on: "hover",
         position: "bottom center"
      });

   }
}
// Load File
function Load_File_Customer(id) {
   if (xhrRequest_File && xhrRequest_File.readyState != 4) {
      xhrRequest_File.abort();
   }
   countFile = 0;
   $('#lightgallery_File').hide();
   xhrRequest_File = AjaxLoad(url = "/Customer/CustomerImage/?handler=LoadFileByFolder"
      , data = { 'currentFolderID': id }
      , async = false
      , error = function () {
         if (xhrRequest_File && xhrRequest_File.readyState != 4) {
            xhrRequest_File.abort();
         }
      }
      , success = function (result) {
         let data = JSON.parse(result);
         dataFileCustomer = data;
         if (data != undefined) {
            RenderFileGallery(data, "lightgallery_File");
            $('#lightgallery_File').show();

            countFile += data.length;
         }

         let count = countFile + countImage;
         if (count != 0) {
            $('#folder-' + id).text(count);
            $('#folder-' + id).css('display', 'block');
         }
      }
   );




}
function RenderFileGallery(data, id) {
   var myNode = document.getElementById(id);
   if (myNode != null) {
      myNode.innerHTML = '';
      let stringContent = '';
      if (data && data.length > 0) {
         for (var i = 0; i < data.length; i++) {
            let item = data[i];
            let tr = '<a class="file_pdf" target="_blank" href="'
               + item.link + '" data-name="'
               + item.name + '" data-created="'
               + item.created + '" data-createdby="'
               + item.createdby + '"' + 'data-title="'
               + item.name + '" data-content="'
               + item.createdby + '\n'
               + item.created + '" >'
               + GetImageTypeFile(item.type)
               + '<div class="fileCustomer">'
               + '<span>' + item.name + '</span></br>'
               + '<span>' + item.createdby + '</span></br>'
               + '<span>' + item.created + '</span>'
               + '</div>'
               + '</a>'
            stringContent = stringContent + tr;
         }
      }
      else {
         stringContent = '<div class="sheet_cart_load_empty" style=" " >no content </div>'
      }
      document.getElementById(id).innerHTML = stringContent;
   }
}

function GetImageTypeFile(type) {
   switch (type) {
      case 'doc':
         return '<img src="/assests/img/ButtonImg/file_doc.png" style="width: 50px; height: 50px ;object-fit: cover;"/>'
         break;
      case 'pdf':
         return '<img src="/assests/img/ButtonImg/file_pdf.png" style="width: 50px; height: 50px;object-fit: cover;"/>'
         break;
      case 'xls':
         return '<img src="/assests/img/ButtonImg/file_xls.png" style="width: 50px; height: 50px object-fit: cover;"/>'
         break;
      default:
         break;
   }
}
// Load Folder
function Prepare_SH_WhenEdit_File() {
   document.getElementById("uploadButton_File").style.display = "none";
   document.getElementById("lightgallery_File").style.display = "none";
   if (document.getElementById("btnEditImage_File")) document.getElementById("btnEditImage_File").style.display = "none";
   document.getElementById("btnEditImageOK_File").style.display = "block";
   document.getElementById("btnEditImageCancel_File").style.display = "block";
   document.getElementById("lightgalleryTableEdit_File").style.display = "";
   document.getElementById("noticeDelete_File").style.display = "block";
   document.getElementById("noticeDelete_File_notification").style.display = "block";
}
function Done_SH_WhenEdit_File() {
   document.getElementById("uploadButton_File").style.display = "block";
   document.getElementById("lightgallery_File").style.display = "block";
   if (document.getElementById("btnEditImage_File")) document.getElementById("btnEditImage_File").style.display = "block";
   document.getElementById("btnEditImageOK_File").style.display = "none";
   document.getElementById("btnEditImageCancel_File").style.display = "none";
   document.getElementById("lightgalleryTableEdit_File").style.display = "none";
   document.getElementById("noticeDelete_File").style.display = "none";
   document.getElementById("noticeDelete_File_notification").style.display = "none";
}

function Prepare_SH_WhenEdit_Image() {
   $(".actionImg").hide();
   document.getElementById("uploadButton").style.display = "none";
   document.getElementById("lightgallery").style.display = "none";
   if (document.getElementById("btnEditImage")) document.getElementById("btnEditImage").style.display = "none";
   document.getElementById("btnEditImageOK").style.display = "block";
   document.getElementById("btnEditImageCancel").style.display = "block";
   document.getElementById("lightgalleryTableEdit").style.display = "";
   document.getElementById("noticeDelete").style.display = "block";
   document.getElementById("noticeDelete_notification").style.display = "block";
}
function Done_SH_WhenEdit_Image() {
   $(".actionImg").show();
   document.getElementById("uploadButton").style.display = "block";
   document.getElementById("lightgallery").style.display = "block";
   if (document.getElementById("btnEditImage")) document.getElementById("btnEditImage").style.display = "block";
   document.getElementById("btnEditImageOK").style.display = "none";
   document.getElementById("btnEditImageCancel").style.display = "none";
   document.getElementById("lightgalleryTableEdit").style.display = "none";
   document.getElementById("noticeDelete").style.display = "none";
   document.getElementById("noticeDelete_notification").style.display = "none";
}
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
   $("#lightgalleryTableEdit").hide();
   $("#noticeDelete").hide();
   $("#noticeDelete_notification").hide();
   $("#uploadButton").show();
   $("#btnEditImage").show();
   $("#btnSetImage").show();
   $("#btnEditImageOK").hide();
   $("#btnEditImageCancel").hide();

   $("#lightgallery_File").show();
   $("#lightgalleryTableEdit_File").hide();
   $("#noticeDelete_File").hide();
   $("#noticeDelete_File_notification").hide();
   $("#uploadButton_File").show();
   $("#btnEditImage_File").show();
   $("#btnEditImageOK_File").hide();
   $("#btnEditImageCancel_File").hide();

}
function Event_Click_Folder_Customer() {

   $('.ui.menu#TreeFolderName_Customer a.item').unbind().click(function () {

      CurrentFolderID_Customer = $(this).attr('data_id');

      ContentIMG(CurrentFolderID_Customer);

      $('#TreeFolderName_Customer a.item').each(function () {
         $(this).removeClass('active');
      })
      $(this)
         .addClass('active');

      $('#seg_image').show();
      $('#seg_file').show();
      countImage = 0;
      countFile = 0;

      var textTab = $("#TreeFolderName_Customer a.active.item")[0].getAttribute('data-foldername');

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

      link_Upload_Image = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID + '&FolderName=' + CurrentFolderName_Customer + '&Type=Image';
      link_Upload_File = '/Api/Upload/Upload?CustomerID=' + ser_MainCustomerID
         + '&FolderName=' + CurrentFolderName_Customer
         + '&FolderNameChild=' + "File"
         + '&Type=AttachmentFile';
      LoadImgByForm(CurrentFolderID_Customer);
   });
}
function Event_Click_Folder_Cust_TreatPlan() {

   $('#FolderTreatmentPlan a.item').unbind().click(function () {
      CurrentFolderID_Customer = $(this).attr('data_id')
      ContentIMGByTreatPlan(CurrentFolderID_Customer);

      $('#FolderTreatmentPlan a.item').each(function () {
         $(this).removeClass('active');
      })
      $(this)
         .addClass('active');

      countImage = 0;
      countFile = 0;

      var textTab = $("#FolderTreatmentPlan a.active.item")[0].getAttribute('data-foldername');

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

      link_Upload_Image = '/UploadClass/FileUploadHandler.ashx?CustomerID=' + ser_MainCustomerID + '&FolderName=' + CurrentFolderName_Customer + '&Type=Image';
      link_Upload_File = '/UploadClass/FileUploadHandler.ashx?CustomerID=' + ser_MainCustomerID
         + '&FolderName=' + CurrentFolderName_Customer
         + '&FolderNameChild=' + "File"
         + '&Type=AttachmentFile';


      LoadImgByForm(CurrentFolderID_Customer);
   });
}


function EventClick_Folder_Edit() {
   //$('.button_ellipsis').on('click',function (event) {
   //    $(this)
   //        .toggleClass('active')
   //        .siblings()
   //        .toggleClass('active');
   //    $(this).closest('.tree-item').children('a.item').toggleClass('collapse');
   //});
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
            let tr = '<div class="tree-item">'
               + '<div class="tree-item_button">'
               + '<div class="button button_ellipsis"><i class="ellipsis horizontal icon"></i></div>'
               + '<a class="image_edit_folder button_ellipsis_item" data_name="' + item.FolderName + '" data_id="' + item.ID + '"><i class="button_ellipsis_item MLU-icon MLUech-icon-edit"></i></a>'
               + '<a class="image_delete_folder button_ellipsis_item" data_name="' + item.FolderName + '" data_id="' + item.ID + '"><i class="button_ellipsis_item MLU-icon MLUech-icon-delete"></i></a>'
               + '</div>'
               + '<a id="folder_image_' + item.ID + '" data_id="' + item.ID + '" data-foldername="' + item.FolderName + '" class="item"><i class="MLU-icon MLUech-icon-folder_40"></i> <span>'
               + item.FolderName + '</span>'
               + '<div class="folder-count" id="folder-' + item.ID + '" style="' + (item.Number != 0 ? "display:block" : "display:none") + '" >' + (item.Number != 0 ? item.Number : "") + '</div>'
               + '</a></div>'
            stringContent = stringContent + tr;
         }
      }
      document.getElementById(id).innerHTML = stringContent;
   }
   EventClick_Folder_Edit();
}
// #endregion


$('#list-button_sort').on('click', '.btn-sort', function (e) {
   $(this).addClass('active').siblings().removeClass('active');
})

function SortAscending() {
   let filterFile;
   let filterImage;
   filterFile = dataFileCustomer.sort(function (a, b) {
      var name1 = a.name.toLowerCase();
      var name2 = b.name.toLowerCase();
      return name1 < name2 ? -1 : name1 > name2 ? 1 : 0;
   })
   filterImage = dataImageCustomer.sort(function (a, b) {
      var name1 = a.name.toLowerCase();
      var name2 = b.name.toLowerCase();
      return name1 < name2 ? -1 : name1 > name2 ? 1 : 0;
   })
   RenderFileGallery(filterFile, "lightgallery_File");
   RenderImageGallery(filterImage, "lightgallery");
   DragDropEvent_Item_Img();
}
function SortDescending() {
   let filterFile;
   let filterImage;
   filterFile = dataFileCustomer.sort(function (a, b) {
      var name1 = a.name.toLowerCase();
      var name2 = b.name.toLowerCase();
      return name1 > name2 ? -1 : name1 < name2 ? 1 : 0;
   })
   filterImage = dataImageCustomer.sort(function (a, b) {
      var name1 = a.name.toLowerCase();
      var name2 = b.name.toLowerCase();
      return name1 > name2 ? -1 : name1 < name2 ? 1 : 0;
   })
   RenderFileGallery(filterFile, "lightgallery_File");
   RenderImageGallery(filterImage, "lightgallery");
   DragDropEvent_Item_Img();
}
function SortDateAscending() {
   let filterFile;
   let filterImage;
   filterFile = dataFileCustomer.sort(function (a, b) {
      var date1 = a.creatednum.toLowerCase();
      var date2 = b.creatednum.toLowerCase();
      return date1 < date2 ? -1 : date1 > date2 ? 1 : 0;
   })
   filterImage = dataImageCustomer.sort(function (a, b) {
      var date1 = a.creatednum.toLowerCase();
      var date2 = b.creatednum.toLowerCase();
      return date1 < date2 ? -1 : date1 > date2 ? 1 : 0;
   })
   RenderFileGallery(filterFile, "lightgallery_File");
   RenderImageGallery(filterImage, "lightgallery");
   DragDropEvent_Item_Img();
}
function SortDateDescending() {
   let filterFile;
   let filterImage;
   filterFile = dataFileCustomer.sort(function (a, b) {
      var date1 = a.creatednum.toLowerCase();
      var date2 = b.creatednum.toLowerCase();
      return date1 > date2 ? -1 : date1 < date2 ? 1 : 0;
   })
   filterImage = dataImageCustomer.sort(function (a, b) {
      var date1 = a.creatednum.toLowerCase();
      var date2 = b.creatednum.toLowerCase();
      return date1 > date2 ? -1 : date1 < date2 ? 1 : 0;
   })
   RenderFileGallery(filterFile, "lightgallery_File");
   RenderImageGallery(filterImage, "lightgallery");
   DragDropEvent_Item_Img();
}
// #region // Insert image
function Insert_Image_File_Customer(type, folderid, name, realname) {
   AjaxLoad(url = "/Customer/CustomerImage/?handler=InsertImage"
      , data = {
         'type': type
         , 'folderid': folderid
         , 'name': name
         , 'realname': realname
      }
      , async = true
      , error = function () { notiError_SW(); }
      , success = function (result) {
         if (type == "image") Load_Image_Customer(folderid);
         else Load_File_Customer(folderid);
      }
   );


}
// #endregion


// #region // Load form template image
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
         console.log('form error or form not defined');
      }
      DragDropEvent_Item_Img();
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

            let tr = '<div class="tempItem ui button ' + exist + (isChoose_ID == item.ID ? " ischoose" : "") + '" data-id="' + item.TemplateID + '">' + item.Name + '</div>';
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
         $("#templateForm").css("grid-template-columns", "repeat(" + Data.columns + ", 1fr)");
         $("#templateForm").css("grid-template-rows", "repeat(" + Data.rows + ", 1fr)");
         for (var i = 0; i < Data.rows; i++) {
            let div = '';
            for (var j = 0; j < Data.columns; j++) {
               let item = Data.data[key].content;
               let header = Data.data[key].header != "" ? '<div class="title_template">' + Data.data[key].header + '</div>' : '';
               let hasHeader = (Data.data[key].header != "" && Data.position != "left") ? 'header_template' : '';
               if (item.value == 1) {
                  let img = "";

                   if (dataImageCustomer != undefined && dataImageCustomer.find(x => x.id === item.img) != undefined) {
                     let ImgCurrent = dataImageCustomer.find(x => x.id === item.img);
                     img =
                        //'<a href="' + ImgCurrent.link + '">'+
                        '<img class="imgform" src="' + ImgCurrent.link + '">'
                        + '<i data-id="' + ImgCurrent.id + '" data-x="' + item.x + '" data-y="' + item.y + '" class="delete alternate icon button-delete-img-form"></i>'
                        //+ ' </a>'
                        ;
                  } else {
                     img = '<div class="text_img">' + item.placehoder + '</div>'
                  }
                  let style = (Data.position == "left" ? 'style="display:flex;"' : '');
                  let style_img = '';//(Data.width && Data.height ? 'style="width:' + Data.width+';height:'+Data.height+'"' : '')
                  div += '<div ' + style + ' class="img-content ' + hasHeader + '">' + header + '<div ' + style_img + ' data-x="' + item.x + '" data-y="' + item.y + '" class="selectdrop" >' + img + '</div></div>'
               } else {
                  div += '<div class="img-content">' + header + '<div class="notdrop"></div></div>'
               }

               key++;
            }
            stringContent += div;
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
            if (resul == "1") {
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
// End #region // Load form template image