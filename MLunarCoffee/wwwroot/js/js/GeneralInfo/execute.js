﻿
//#region // Layout
//function GeneralInfo_LayoutRender (data, id) {
//    var myNode = document.getElementById(id);
//    if (myNode != null) {
//        let stringContent = '';
//        if (data && data.length > 0) {
//            for (let i = 0; i < data.length; i++) {
//                let link = '';
//                let tr = '';
//                switch (data[i]) {
//                    case 'anam':
//                        link = '/Customer/GeneralInfo/CustomerAnamnesis';
//                        break;
//                    case 'complain':
//                        link = '/Customer/GeneralInfo/CustomerComplaint';
//                        break;
//                    case 'app':
//                        link = '/Customer/GeneralInfo/CustomerSchedule';
//                        break;
//                    case 'commit':
//                        link = '/Customer/GeneralInfo/CustomerCommitment';
//                        break;
//                    case 'tab':
//                        link = '/Customer/GeneralInfo/CustomerTab';
//                        break;
//                    case 'treat':
//                        link = '/Customer/GeneralInfo/CustomerTreat';
//                        break;
//                    case 'payment':
//                        link = '/Customer/GeneralInfo/CustomerPayment';
//                        break;

//                    default: link = '';
//                        break;
//                }
//                if (link != '') {
//                    tr = '<div  data-link=' + link + ' class="generalinfo_render mb-3 mt-lg-0 mt-4"></div>';
//                }
//                stringContent = stringContent + tr;
//            }
//        }
//        document.getElementById(id).insertAdjacentHTML('beforeend', stringContent);
//    }
//}
//function GeneralInfo_LayoutFill () {
//    $(".generalinfo_render").each(function (index) {
//        let link = $(this).attr('data-link');
//        $(this).load(link, function (e) { });
//    });
//}
//#endregion

//#region // Info
async function GeneralInfo_InfoRender (data) {
    return new Promise(resolve => {
        if (data != undefined) {
            $('#txtName').text(data.Name);
            let _age, birth;
            if (!data.Birthday.includes('1900')) {
                _age = Distance_Year_2Date(new Date(data.Birthday), new Date());
                birth = ConvertDateTimeUTC_DMY(data.Birthday);
            }
            else {
                _age = Distance_Year_2Date(new Date(data.BirthYear, 1, 1), new Date());
                birth = '';
            }
            if (_age == 0) {
                $('#txtBirthday').html(`${birth}${`<span class="ps-2 fw-bold fst-italic">~ ${(_age +1) + ' ' + Outlang["Tuoi"]}</span>` }`);
            }
            else {
                $('#txtBirthday').html(`${birth}${(_age > 0 && _age < 100) ? `<span class="ps-2 fw-bold fst-italic">${_age + ' ' + Outlang["Tuoi"]}</span>` : ``}`);
            }
            


            if (data.facebookurl != '') {
                $('#buttonfacebook').attr("data-link", GeneralInfo_CheckDataLinkOld(data.facebookurl, 'facebook'));
                $("#buttonfacebook").click(function () {
                    onclicksocial_icon($(this).attr('data-link'));
                });
            }
            else $('#buttonfacebook').addClass('nolink');
            if (data.instgramurl != '') {
                $('#buttoninstagram').attr("data-link", GeneralInfo_CheckDataLinkOld(data.instgramurl, 'instagram'));
                $("#buttoninstagram").click(function () {
                    onclicksocial_icon($(this).attr('data-link'));
                });
            }
            else $('#buttoninstagram').addClass('nolink');
            if (data.viberurl != '') {
                $('#buttonviber').attr("data-link", GeneralInfo_CheckDataLinkOld(data.viberurl, 'viber'));
                $("#buttonviber").click(function () {
                    onclicksocial_icon($(this).attr('data-link'));
                });
            }
            else $('#buttonviber').addClass('nolink');
            if (data.zalourl != '') {
                $('#buttonzalo').attr("data-link", GeneralInfo_CheckDataLinkOld(data.zalourl, 'zalo'));
                $("#buttonzalo").click(function () {
                    onclicksocial_icon($(this).attr('data-link'));
                });
            }
            else $('#buttonzalo').addClass('nolink');
            if (Number(data.Gender) == 60) {
                $('#txtgender').removeClass('fa-venus').addClass('fa-mars');
                $('#txtgender').css('color', '#2f89ff');
            }
            else {
                $('#txtgender').removeClass('fa-mars').addClass('fa-venus');
                $('#txtgender').css('color', '#ff6b85');
            }
            JsBarcode("#txtCustBarcode", xoa_dau(data.Cust_Code), {
                lineColor: "#031313",
                width: 2,
                height: 30,
                displayValue: false
            });

            $('#txtEmail').text(data.Email1);
            $('#txtCarrer').text(data.Carrer);
            $('#txtCustomerSource').text(data.CustomerSource + (data.CustomerBroker != '' ? ` - ${data.CustomerBroker}` : ''));
            $('#txtInfoNote').text(data.Note);
            $('#txtCusCreate').text(`${ConvertDateTimeUTC_DMY(data.created)}${data.BranchAlias != "" ? " | " + data.BranchAlias : ""}`);

            let CustAddress = data.Address != '' ? (data.Address + ', ') : '';
            let CustCommune = data.Commune != '' ? (data.Commune + ', ') : '';
            let CustDistrict = data.District != '' ? (data.District + ', ') : '';
            let CustCity = data.City != '' ? (data.City + ', ') : '';
            let CustNation = data.NationName != '' ? data.NationName : '';
             
            $('#txtAddress').text(CustAddress + CustCommune + CustDistrict + CustCity + CustNation);
            if (Number(data.AppID) == 0) {
                $('#divCheckAppointmentExtra').hide();
            }
            else {
                $('#divCheckAppointmentExtra').show();
            }

            
            if (data.Represent_Name != '') {
                $('#txtrepre_name').text(data.Represent_Name);
                $('#txtrepre_phone').text(data.Represent_Phone);
                $('#txtrepre_id').text(data.Represent_ID);
                $('#txtrepre_type').text(data.Represent_Type);
                $('#tabCust_guardianArea').removeClass('d-none');
            }
            if (ser_IsMobileApp == "1") {
                $("#txtCusAppUser").text(data.AppUser)
                $("#txtCusAppPassword").text(data.AppPassword);
                $("#txtCusAppLastSignin").text(ConvertDateTimeUTC_DMY(data.AppLastSignin));
                $('#tabCust_mobileArea').removeClass('d-none');
            }
             
 
        }
        resolve();
    });

}
async function GeneralInfo_InfoLoadGroup () {
    return new Promise(resolve => {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadCustomerGroup"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data[0].GroupName == "") {
                        $("#customerGroupName").html(Outlang["Chon_nhom_khach_hang"]);
                    } else {
                        $("#customerGroupName").html(data[0].GroupName);
                    }

                    if (data[0].Image == '' || data[0].Image == undefined) {
                        $('#avatarCustomerGroup').attr('src', Master_Default_Pic);
                    }
                    else {
                        $('#avatarCustomerGroup').attr('src', data[0].Image);
                    }
                    customerGroupID = data[0].ID;
                    $('#customerGroup').show();
                }
            }
        );
        resolve();
    });
}
async function GeneralInfo_CCLoad () {
    return new Promise(resolve => {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadCustCare"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data != undefined && data.length == 1) {
                        ser_CSKHID = Number(data[0].CSKHID);
                        ser_TeleID = Number(data[0].TeleID);
                        ser_ResponID = Number(data[0].EmpRespon);
 
                        if (Number(ser_TeleID) != 0) {
                            let obj = Fun_GetObject_ByID(DataEmployee, ser_TeleID);
                            let telename = obj != undefined ? obj.Name : '';
                            let teleimg = obj != undefined ? obj.Avatar : '';
                            $('#infotele_name').html(telename);
                            $('#infotele_img').attr('src', (teleimg != '' ? teleimg : Master_Default_Pic));
                        }
                        else {
                            $('#infotele_name').html('-');
                        }

                        if (Number(ser_CSKHID) != 0) {
                            let obj = Fun_GetObject_ByID(DataEmployee, ser_CSKHID);
                            let cskhname = obj != undefined ? obj.Name : '';
                            let cskhimg = obj != undefined ? obj.Avatar : '';
                            $('#infocskh_name').html(cskhname);
                            $('#infocskh_img').attr('src', (cskhimg != '' ? cskhimg : Master_Default_Pic));
                        }
                        else {
                            $('#infocskh_name').html(Outlang["Chon_cham_soc_vien"]);
                        }
               
                        if (Number(ser_ResponID) != 0) {
                            let obj = Fun_GetObject_ByID(DataEmployee, ser_ResponID);
                            let resname = obj != undefined ? obj.Name : '';
                            let resimg = obj != undefined ? obj.Avatar : '';
                            $('#infodocres_name').html(resname);
                            $('#infodocres_img').attr('src', (resimg != '' ? resimg : Master_Default_Pic));
                        }
                        else {
                            $('#infodocres_name').html(Outlang["Chon_bac_si"]);
                        }
                    }

                }
            }
        );
        resolve();
    });
}
function EditCustomerGroup () {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/CustomerGroupDetail?CustomerID=" + ser_MainCustomerID + "&GroupID=" + customerGroupID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
    return false;
}
function EditCustomerCare (type) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load(`/Customer/GeneralInfo/CustCareDetail?type=${type}`);
    $('#DetailModal').modal('show');
    return false;
}
//#endregion


//#region // Note
function GeneralInfo_LoadHistory () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadHistory"
        , data = {
            'CustomerID': ser_MainCustomerID
        }
        , async = true
        , error = function () {
            notiError_SW();
        }
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                GeneralInfo_NoteRender(data, "generalInfo_note");
            }
        }
    );
}
async function GeneralInfo_NoteRender (data, id) {
    return new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {

                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let itemEmp = item.Employee != 0 ? `<span class="bg-gradient-dark badge me-2 px-2 py-1 rounded-1 text-capitalize text-xs" ><i class="fas fa-user me-1"></i>${item.Employee}</span>` : "";
                    let tr = '<div class="pt-0 p-1 d-flex mb-1">'
                        + '<div class="w-100">'
                        + '<span class="text-sm text-dark">'
                        + '<span data-id="' + item.ID + '" class="text-sm fw-bold text-primary me-2 border-bottom border-primary cursor-pointer buttonEditClass">'+ConvertDateTimeUTC_YMD(item.Created)+'</span>'
                        + '<span class="badge text-xs text-capitalize px-2 py-1 me-2 rounded-1" style="color:' + item.ColorCode + ';background-color:' + item.ColorCode + '29">' + item.MasterStatusName + '</span>'
                        + itemEmp
                        + '<span class="content_line_clamp" style="white-space: pre-line">' + item.Content + '</span>'
                        + '</span>'
                        + '</div>'
                        + '</div>'

                    stringContent = stringContent + tr;
                }

            }
            document.getElementById(id).innerHTML = stringContent;
            GeneralInfo_HistoryEvent();
        }
        resolve();
    });
}
function GeneralInfo_HistoryEvent(){
    $('#generalInfo_note .buttonEditClass').unbind('click').on('click', function () {
        let ID = Number($(this).attr('data-id'));
        if (!isNaN(ID))
            editHistory(ID, ser_MainCustomerID);
        //else editAdvice(ID, ser_CustomerHistoryID);
    });
}
function GeneralInfo_HistoryDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/HistoryList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
//#endregion

//#region // Relation Ship
async function GeneralInfo_RelationRender (data,dataship, id) {
    return new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length >= 0) {
                for (let j = 0; j < data.length; j++) {
                    let item = data[j];
                    let custid = 0, custname = '', custimg = '', custcode = '';
                    if (item.CustID != ser_MainCustomerID) {
                        custid = item.CustID;
                        custname = item.CustName;
                        custimg = item.Image;
                        custcode = item.CustCode;

                    }
                    else if (item.CustID1 != ser_MainCustomerID) {
                        custid = item.CustID1;
                        custname = item.CustName1;
                        custimg = item.Image1;
                        custcode = item.CustCode1;

                    }
                    if (custid != 0) {
                        let tr =
                            `<li class="list-group-item border-0 align-items-center pb-2 cursor-pointer d-flex rounded-3 text-dark text-sm">
                                <img class="border avatar avatar-sm" onerror="Master_OnLoad_Error_Image(this)" src="${custimg != "" ? custimg : "/default.png"}">
                                <div class="ps-2">
                                    <div class="d-inline">
                                        <span class="fw-bold pe-1">${item.RelaName}</span>
                                    </div>
                                    <div class="d-inline">
                                         <a href="/Customer/MainCustomer?CustomerID=${custid}" target="_blank">${custcode}</a>
                                        <div class="fst-italic">${custname}</div>
                                     </div>
                                </div>
                            <hr class="horizontal dark my-0 mt-2">
                        </li>`
                        stringContent = stringContent + tr;
                    }



                }
                for (j = 0; j < dataship.length; j++) {
                    let item = dataship[j];
                    if (item.CustID != ser_MainCustomerID) {
                        let tr =
                            `<li class="list-group-item border-0 align-items-center pb-2 position-relative cursor-pointer d-flex rounded-3 text-dark text-sm">
                             <img class="border avatar avatar-sm" onerror="Master_OnLoad_Error_Image(this)" src="${item.Image != "" ? item.Image : "/default.png"}">
                                <div class="ps-2">
                                    <div class="d-inline">
                                        <a class="text-dark me-2 cursor-pointer" >
                                            <i class="fas fa-users"></i>
                                        </a>
                                        <a href="/Customer/MainCustomer?CustomerID=${item.CustID}" target="_blank">${item.CustCode}</a>
                                        <span class="ps-1">${item.CustName}</span>
                                    </div>
                                    <div class="d-inline">
                                        <div class="fst-italic">${item.Address != "" ? item.Address : "-"}</div>
                                     </div>
                                </div>

                            <hr class="horizontal dark my-0 mt-2">
                        </li>`
                        stringContent = stringContent + tr;
                    }
                }
            }

            document.getElementById(id).innerHTML = stringContent;
        }
        resolve();
    });
}
function GeneralInfo_RelationEdit () {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/RelationshipDetail?CustomerID=" + ser_MainCustomerID);
    $('#DetailModal').modal('show');
}
function GeneralInfo_RelationLoad () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadRelation"
        , data = {'custID': ser_MainCustomerID}
        , async = true
        , error = function () {notiError_SW()}
        , success = function (_result) {
            if (_result != "0") {
                let _datas = JSON.parse(_result);
                GeneralInfo_RelationRender(_datas.Table, _datas.Table1, "generalInfo_relation")

            }
        }
    );
}
//#endregion

//#region //Image
async function GeneralInfo_RenderImage (data, id) {
    return new Promise(resolve => {
        var myNode = document.getElementById(id);
        let dataGal = [];
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let link_img = sys_HTTPImageRoot + item.CustomerID
                        + '/' + item.FolderName + '/' + item.RealName;
                    let link_feaute = sys_HTTPImageRoot + item.CustomerID
                        + '/' + item.FolderName + '/' + item.FeatureImage;

                    let link_imgthum = "'" + link_feaute + "'";
                    let sub = `<div class="text-sm">${item?.CreatedBy}</div>
                        <div>${GetDateTime_String_DMYHM(ConvertToDateRemove1900(item?.Created) ?? "")}</div>`;

                    tr = `<a data-sub-html="" class="avatar  description-rp design_img_item GeneralInfo_Image" style="background-image: url(${link_imgthum})"> 
                            <img class="imgload" style="width: 0px;height: 0px;object-fit: cover;opacity: 0;"  src="${link_img}"/> 
                         </a>`;

                    stringContent = stringContent + tr;
                    //galery
                    
                    let e = {};
                    e["src"] = link_img ?? "/default.png";
                    e["thumb"] = link_feaute ?? "/default.png";
                    e["subHtml"] = sub;
                    dataGal.push(e);
                }

            }
            document.getElementById(id).innerHTML = stringContent;
        }
        _GeneralInfo_dynamicGallery.refresh(dataGal);
        GeneralInfo_Event();
        resolve();
    });
}

function GeneralInfo_Event() {
    $("#lightGallery .GeneralInfo_Image").unbind().click(function (event) {
        _GeneralInfo_dynamicGallery.openGallery($(this).index());
    });
}
function GeneralInfo_ImageDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/CustomerImage?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
//#endregion

//#region // App
function GeneralInfo_AddApp () {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CustomerID=" + ser_MainCustomerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function GeneralInfo_AppDetail () {
 
    let objs = $("#divCustomerTabContent [data-tab='/Customer/ScheduleList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
   
}
function GeneralInfo_LoadApp () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadApp"
        , data = {'custID': ser_MainCustomerID}
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                GeneralInfo_ShedulerRender(data, "generalInfo_app");
            }
        }
    );
}
async function GeneralInfo_ShedulerRender (data, id) {
    return new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let _avatar = GeneralInfo_ShedulerRender_Doctor(item.DoctorID);
                    let content = item.Content;
                    let typeappp = '';
                    if (item.IsCancel == 1) {
                        content = '<span class="pe-2 text-danger">' + Outlang["Lich_huy"] + ': ' + item.ContentCancel +'</span>';
                    }
                    else {
                        typeappp = item.LangKey != "" ? Outlang[item.LangKey] : item.TypeName;
                        content = `<span class="pe-2 text-dark">${typeappp}</span>` + content;
            
                    }
                    tr = '<li class="p-3 py-2 border-0 d-flex  mb-0">'
                        + '<div class="avatar avatar-sm me-2" style="min-width: 36px;">'
                        + '<img onerror="Master_OnLoad_Error_Image(this)" src="' + ((_avatar != '') ? _avatar : Master_Default_Pic) + '" class="border-radius-lg mt-2 ">'
                        + '</div>'
                        + '<div class="d-flex align-items-start flex-column justify-content-center">'
                        + '<span class="text-sm fw-bold">'
                        + '<span data-id="' + item.ID + '" class="text-sm fw-bold text-primary border-bottom border-primary cursor-pointer ' + `${item.IsCancel == 1 ? "" : "buttonEditClass"}` + '">'
                        + ConvertDateTimeUTC_DMYHM(item.Date_From)
                        + '</span>'
                        + '<span class="ms-2 text-normal text-dark">' + item.ShortName + '</span>'
                        + '</span>'
                        + '<p class="mb-0 mb-0 mt-n0 text-sm text-dark content_line_clamp">' + content + '</p>'
                        + '</div>'

                        + '</li>'
                        + ((i != data.length -1 )? '<hr class="horizontal dark my-1"  >' :'');

                    stringContent = stringContent + tr;
                }

            }
            document.getElementById(id).innerHTML = stringContent;
            GeneralInfo_ShedulerEvent();
        }
        resolve();
    });
}
function GeneralInfo_ShedulerEvent()
{
    $('#generalInfo_app .buttonEditClass').unbind('click').on('click', function ()
    {
        let ID = Number($(this).attr('data-id'));
        if (!isNaN(ID))
            editSchedule(ID, ser_MainCustomerID);
    });
}
function GeneralInfo_ShedulerRender_Doctor (DoctorID) {
    if (Number(DoctorID) != 0) {
        let obj = Fun_GetObject_ByID(DataEmployee, DoctorID);
        if (obj != null) {
            return obj.Avatar;
        }
    }
    return Master_Default_Pic;
}
function GeneralInfo_AddAnamnesis () {
    ser_PatientHistoryID = 0;
    $("#generalInfo_anamnesis").html('');
    $("#generalInfo_anamnesis").load("/Customer/Anamnesis/CustomerAnamnesisDetail?ver=" + versionofWebApplication);
}
function GeneralInfo_LoadAnamnesis () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadAnamnesis"
        , data = {'custID': ser_MainCustomerID}
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                if (data.length > 0) {
                    ser_PatientHistoryID = data[0].ID;
                    $("#generalInfo_anamnesis").html('');
                    $("#generalInfo_anamnesis").load("/Customer/Anamnesis/CustomerAnamnesisDetail?ver=" + versionofWebApplication);
                }

            }
        }
    );
}
//#endregion

//#region //Broker

function GeneralInfo_LoadBrokerPoint () {
    try {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadBroker"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length > 0) {
                        let TotalRow = data.length;
                        _GeneralInfo_DataBroker = sliceIntoChunks(JSON.parse(JSON.stringify(data)), 3);
                        $(".BrokerChangePage").attr("total-Page", Math.ceil(TotalRow / 3))
                        GeneralInfo_ExePageBroker();
                        GeneralInfo_EventBroker();
                    }
                }
            }
        );
    } catch (ex) {
    }
}
function GeneralInfo_ExePageBroker (CurrentPage = 0) {
    try {
        if (_GeneralInfo_DataBroker && _GeneralInfo_DataBroker.length > 0) {
            $('.BrokerChangePage').attr('data-page', CurrentPage);
            GeneralInfo_RenderBroker(_GeneralInfo_DataBroker[CurrentPage], "generalInfo_BodyBroker");
        }
    } catch (ex) {}   
}
async function GeneralInfo_RenderBroker (data, id) {
    new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                let LastIndex = data.length - 1;
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let tr = `
                        <li class="border-0 d-flex mb-0 py-0 px-0">
                            <div class="pt-1">
                                <div class="d-lg-flex">
                                    <span class="text-primary text-sm mb-0 fw-bold pe-1"><span>${item.Point > 0 ? `+` + item.Point : item.Point}</span></span>
                                    <h6 class="text-dark text-sm fw-bold mb-0 pe-1">${ConvertDateTimeUTC(item.Created)}</h6>
                                </div>
                                <div class="text-dark d-flex text-sm mt-0 mb-0 ${item.RLCustomerID == 0 ? `d-none` : ``}">
                                    <p class="mb-0 fw-bold GeneralInfo_FirstUpper">${Outlang["Sys_gioi_thieu"]} ${Outlang["Khach_hang"]}: </p>
                                    <p class="mb-0 ps-1">
                                        <a class="cursor-pointer fw-bold" target="_blank" href="/Customer/MainCustomer?CustomerID=${item.RLCustomerID}&ver=${versionofWebApplication}">${item.CustomerCode}</a>
                                        <span class="text-sm text-dark"> - ${item.CustomerName}</span>
                                    </p>
                                </div>
                                <div class="mb-0 mt-2 border-start border-success border-3">
                                    <span class="content_line_clamp text-dark text-sm ps-2">
                                        <span class="text-pre-wrap">${item.Content}</span>
                                    </span>
                                </div>
                            </div>
                        </li>
                        ${i == LastIndex ? `` : `<hr class="horizontal dark my-1">`}
                    `;
                    
                    stringContent = stringContent + tr;
                }
            }
            document.getElementById(id).innerHTML = stringContent;
        }
        resolve();
    });
}

function GeneralInfo_EventBroker () {
    $('.BrokerChangePage').unbind('click').click(function () {
        let TotalPage = $(this).attr('total-Page') ? Number($(this).attr('total-Page')) : 0;
        let Page = $(this).attr('data-page') ? Number($(this).attr('data-page')) : 0;
        let ValChange = $(this).attr('data-type') ? $(this).attr('data-type') : '';

        if (ValChange == "Pre") Page = (Page - 1 < 0 ? TotalPage - 1 : Page - 1);
        if (ValChange == "Nex") Page = (Page + 1 == TotalPage ? 0 : Page + 1);

        if (0 <= Page && Page < TotalPage) {
            GeneralInfo_ExePageBroker(Page);
        }
    })
}
//#endregion

//#region // Service
function GeneralInfo_ServiceDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/TabList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
function GeneralInfo_LoadService () {
    try {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadService"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length > 0) {
                        GeneralInfo_RenderService(data, "generalInfo_BodyService");
                    }
                }
            }
        );
    } catch (ex) {
    }
}
async function GeneralInfo_RenderService (data, id) {
    new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                let LastIndex = data.length - 1;
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let TeethDetail = Fun_GetTeeth_ByToken(sys_MainTeeth, item.TeethChoosing, item.TeethType);
                    let tr = `
                       <li class="border-0 d-flex mb-0 py-0 px-0">
                            <div class=" ">
                                <div class="d-lg-flex">
                                    <span class="text-primary text-sm mb-0 fw-bold pe-1">${item.IsProduct != 1 ? `${Outlang["Dv"]}` : `${Outlang["Sp"]}`}</span>
                                    <h6 class="text-dark text-sm font-weight-bold mb-0 pe-1">${ConvertDateTimeUTC(item.Created)} : </h6>
                                    <h6 class="text-dark text-sm  mb-0">${item.Name}</h6>
                                    <h6 class="text-dark text-sm  mb-0">${item.Quantity}</h6>
                                </div>
                                <div class="text-dark d-lg-flex text-sm mt-0 mb-0">
                                    <p class="mb-0 fw-bold">${Outlang["Sys_thanh_toan"]}: </p>
                                    <p class="mb-0 ps-1">${formatNumber(item.AmountPayment)} / ${formatNumber(item.Price)}</p>
                                </div>
                                <div class="mb-3 mt-2 border-start border-success border-3">
                                    <span class="content_line_clamp text-dark text-sm ps-2">
                                        ${TeethDetail != '' ? `<span class="pe-2 fw-bold">${TeethDetail}</span>` : ''}
                                        <span class="text-pre-wrap">${item.Note}</span>
                                    </span>
                                </div>
                            </div>
                        </li>
                        ${i == LastIndex ? `` : `<hr class="horizontal dark my-1">`}
                    `;
                    stringContent = stringContent + tr;
                }
            }
            document.getElementById(id).innerHTML = stringContent;
        }
        resolve();
    });
}
//#endregion

//#region //Treat
function GeneralInfo_TreatDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/TreatmentList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
function GeneralInfo_LoadTreat () {
    try {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadTreat"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length > 0) {
                        GeneralInfo_RenderTreat(data, "generalInfo_BodyTreat");
                    }
                }
            }
        );
    } catch (ex) {
    }
}
async function GeneralInfo_RenderTreat (data, id) {
    new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                let LastIndex = data.length - 1;
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let TeethDetail = Fun_GetTeeth_ByToken(sys_MainTeeth, item.TeethTreat, item.TeethType);
                    let _per = GeneralInfo_RenderTreatDetail(item.TimeTreatIndex, item.PercentOfService, item.PercentOfService_New, item.TeethChoosing, item.Quantity, item.StageCurrent);

                    let tr = `
                        <li class="border-0 d-flex mb-0 py-0 px-0">
                            <div class=" ">
                                <div class="d-lg-flex">
                                    <span class="text-primary text-sm mb-0 fw-bold pe-1">${_per}</span>
                                    <h6 class="text-dark text-sm fw-bold mb-0 pe-1">${ConvertDateTimeUTC(item.Created)} : </h6>
                                    <h6 class="text-dark text-sm  mb-0">${item.Name}</h6>
                                </div>
                                <div class="text-dark d-lg-flex text-sm mt-0 mb-0">
                                    <p class="mb-0 fw-bold">${Outlang["BS/PT"]} : </p>
                                    <p class="mb-0 ps-1">${GeneralInfo_GetNameEmp(item.Doctor, item.Assist)}</p>
                                </div>
                                <div class="mb-3 mt-2 border-start border-success border-3">
                                    <span class="content_line_clamp text-dark text-sm ps-2">
                                        ${TeethDetail != '' ? `<span class="pe-2 fw-bold">${TeethDetail}</span>` : ''}
                                        <span class="text-pre-wrap">${item.Note}</span>
                                    </span>

                                </div>
                            </div>
                        </li>
                        ${i == LastIndex ? `` : `<hr class="horizontal dark my-1">`}
                    `;
                    stringContent = stringContent + tr;
                }
            }
            document.getElementById(id).innerHTML = stringContent;
        }
        resolve();
    });
}
function GeneralInfo_RenderTreatDetail (TimeTreatIndex, PercentOfService, PercentOfService_New, TeethChoosing, Quantity, StageCurrent) {
    try {
        let result = '';

        if (ser_sys_DentistCosmetic == '0') {
            result = (TimeTreatIndex != 0 ? `<span class="text-sm text-dark me-1 fw-bold">${Outlang["Lan"]} ${TimeTreatIndex}</span>` : '') ;
        } else {
            let PercentCurr = 0;
            if (PercentOfService == 0 && PercentOfService_New == 0) {
                if (TeethChoosing == '') {
                    PercentCurr = (Quantity == 0) ? 0 : StageCurrent;
                }
                else {
                    PercentCurr = (Quantity == 0) ? 0 : StageCurrent / Quantity;
                }
            }
            else {PercentCurr = PercentOfService};
            result = `<span>${Math.round(PercentCurr * 100) / 100} %</span>`;
        }

        return result;
    } catch {
        return '';
    }
}
function GeneralInfo_GetNameEmp (Doctor, Assist) {
    try {
        let result = ``;
        let objDoc = Fun_GetObject_ByID(DataEmployee, Doctor);
        let objAss = Fun_GetObject_ByID(DataEmployee, Assist);
        let NameDoc = objDoc != undefined ? objDoc.Name : '';
        let NameAss = objAss != undefined ? objAss.Name : '';
        let mark = ' / ';
        if (NameDoc == 'unknown' || NameAss == 'unknown' || NameDoc == '' || NameAss == '') {mark = ''};
        if (NameDoc != '' || NameAss != '') {
            result = `
                <span class="text-sm text-dark  ">${NameDoc != 'unknown' ? NameDoc : ''}${mark}${NameAss != 'unknown' ? NameAss : ''}</span>
            `;
        }
        return result;
    } catch {
        return '';
    }
}
//#endregion

//#region //Payment
function GeneralInfo_PaymentDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/PaymentList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
function GeneralInfo_LoadPayment () {
    try {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadPayment"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length > 0) {
                        GeneralInfo_RenderPayment(data, "generalInfo_BodyPayment");
                    }
                }
            }
        );
    } catch (ex) {
    }
}
async function GeneralInfo_RenderPayment (data, id) {
    new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                myNode.innerHTML = '';
                let stringContent = '';
                if (data && data.length > 0) {
                    let LastIndex = data.length - 1;
                    for (var i = 0; i < data.length; i++) {
                        let item = data[i];
                        let tr = `
                            <li class="border-0 d-flex mb-0 px-3 py-1">
                                <div class="row w-100 ms-0">
                                    <div class="col-12 col-xl-8 px-0">
                                        <span class="text-xs text-dark">${ConvertDateTimeUTC(item.Created)}</span>
                                        <p class="text-sm text-dark fw-bold mb-0">${item.Invoice_Num}</p>

                                    </div>
                                    <div class="col-12 col-xl-4 px-0 text-lg-end">
                                        <span class="text-xs text-dark" style="max-width: fit-content;">${Outlang[item.MethodNameLangkey]}</span>
                                        <p class="text-sm text-primary mb-0">${formatNumber(item.Amount)}</p>
                                    </div>
                                </div>
                            </li>
                            ${i == LastIndex ? `` : `<hr class="horizontal dark my-1">`}
                        `;
                        stringContent = stringContent + tr;
                    }
                }
                document.getElementById(id).innerHTML = stringContent;
            }
            resolve();
        }, 30)
    })
}
//#endregion

//#region //Rating
function GeneralInfo_LoadRating() {
    try {
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadRating"
            , data = {
                'custID': ser_MainCustomerID,
                'branchID': ser_MainBranchID

            }
            , async = true
            , error = function () { notiError_SW() }
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    if (data.length > 0) {
                        GeneralInfo_RenderRating(data, "generalInfo_BodyRating");
                    }
 
                }
            }
        );
    } catch (ex) {
    }
}
async function GeneralInfo_RenderRating(data, id) {
    new Promise((resolve) => {
        setTimeout(() => {
            var myNode = document.getElementById(id);
            if (myNode != null) {
                myNode.innerHTML = '';
                let stringContent = '';
                if (data && data.length > 0) {
                    let LastIndex = data.length - 1;
                    for (var i = 0; i < data.length; i++) {
                        let item = data[i];
                        let rateType = '';
                        let rating = '';
                        if (item.RatType != "") {
                            rateType = Outlang[item.RatTypeLang] != undefined ? Outlang[item.RatTypeLang] : item.RatType;
                            rateType = `<p class="pe-1 text-sm text-dark fw-bold mb-0">${rateType}</p>:`;
                        }
                        if (Number(item.Star) > 3) {
                            rating = `<span class="pe-1 text-primary">${item.Star}</span><i class="fas fa-star ps-0 fw-bold text-warning"></i>`;
                        }
                        else {
                            rating = `<span class="pe-1 text-danger">${item.Star}</span><i class="fas fa-star ps-0 fw-bold text-danger"></i>`;
                        }
                         
                        let tr =
                            `<li class="border-0 d-flex mb-0 px-3 py-1">
                                <div class="w-100">
                                    <div class="d-flex align-items-center">
                                        <span class="text-sm">${rating}</span>
                                        <span class="ms-auto text-sm">${ConvertDateTimeUTC(item.Created)}</span>

                                    </div>
                                    <div class="d-inline">
                                        ${rateType}
                                        <span class="text-sm text-dark" style="max-width: fit-content;">${item.Message}</span>
                                    </div>
                                </div>
                            </li>
                            ${i == LastIndex ? `` : `<hr class="horizontal dark my-1">`}
                        `;
                        stringContent = stringContent + tr;
                    }
                }
                document.getElementById(id).innerHTML = stringContent;
            }
            resolve();
        }, 30)
    })
}

function GeneralInfo_RatingRenderStar(Star) {
    let result = ``;
    let star = !isNaN(Number(Star)) ? Number(Star) : 0;
    let fullStar = Math.floor(star);
    let partStar = star - fullStar;
    let percent = 100;
    for (let i = 0; i < 5; i++) {
        percent = fullStar > 0 ? 100 : (partStar > 0 ? partStar * 100 : 0)
        if (fullStar <= 0) partStar = 0;
        fullStar--;
        result += `<div class="GeneralInfo_RatingStar_item ms-2 position-relative d-flex ms-auto">
                               <span class="GeneralInfo_Ratingstar GeneralInfo_Ratingstar_under far fa-star d-inline-block position-absolute"></span>
                               <span class="GeneralInfo_Ratingstar GeneralInfo_Ratingstar_over fas fa-star d-inline-block position-absolute overflow-hidden" style="width: ${percent}%"></span>
                          </div>`
    }
    return result;
}

//#endregion
function onclicksocial_icon (link) {
    if (link != "") {
        var win = window.open(link, '_blank');
        win.focus();
    }
}
function GeneralInfo_LoadIni () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadIni"
        , data = {'custID': ser_MainCustomerID}
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                sys_MainTele = data.Tele;
                sys_MainEmp = data.EmpFull;

            }
        }
    );
}
//#endregion

//#region // ConsultStatus
function GeneralInfo_ConsultStatusDetail () {
    let objs = $("#divCustomerTabContent [data-tab='/Customer/StatusList?CustomerID=']");
    if (objs != undefined && objs.length == 1) {
        objs.tab('show')
        $("#divCustomerTabContent a").removeClass('active');
        objs.addClass('active');
    }
}
function GeneralInfo_ConsultStatus () {
    AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadConsultStatus"
        , data = {'custID': ser_MainCustomerID}
        , async = true
        , error = function () {notiError_SW()}
        , success = function (result) {
            if (result != "0") {
                let data = JSON.parse(result);
                GeneralInfo_ConsultRender(data, "generalInfo_consultStatus");
            }
        }
    );
}
async function GeneralInfo_ConsultRender (data, id) {
    return new Promise(resolve => {

        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            let lengthData = 0;
            if (data && data.length > 0) {
                lengthData = data.length - 1;
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    tr = `
                            <div class="row ${(i == 0 ? "" : "d-none")}" id="generalInfo_consultitem${i}">
                                <div class="col-8 d-flex">
                                    ${GeneralInfo_ConsultRenderPerson(item.EmployeeID, item.Created)}
                                </div>
                                <div class="col-4">
                                    <span class="badge ms-auto float-end" style="background-color: ${item.ColorCode};">
                                        ${item.ConsultStatus}
                                    </span>
                                </div>
                                <div class="col-12 pt-2">
                                    <span class="text-sm text-dark content_line_clamp">${item.Content}</span>
                                </div>
                            </div>
                        `;

                    stringContent = stringContent + tr;
                }

            }
            document.getElementById(id).innerHTML = stringContent;
            GeneralInfo_ConsultEvent(lengthData);
        }
        resolve();
    });
}
function GeneralInfo_ConsultRenderPerson (EmployeeID, Create) {
    let obj = Fun_GetObject_ByID(DataEmployee, EmployeeID);
    let name = (obj != null) ? obj.Name : 'unknown';
    let img = (obj != null && obj.Avatar != "") ? obj.Avatar : Master_Default_Pic;
    let result = `
                <div class="avatar icon icon-shape icon-sm me-3 shadow text-center me-3">
                    <img class="border-radius-lg shadow" src="${img}"/>
                </div>
                <div class="d-flex flex-column justify-content-center">
                    <h6 class="mb-0 text-sm text-dark">${name}</h6>
                    <p class="text-sm mb-0">${ConvertDateTimeUTC_DMY(Create)}</p>
                </div>`;
    return result;
}
function GeneralInfo_ConsultEvent (pagenum) {
    let IndexStatus = 0;
    $("#per_statuspreview").unbind("click").click(function () {
        IndexStatus += 1;
        (IndexStatus > pagenum ? IndexStatus = 0 : 0);
        $("#generalInfo_consultitem" + IndexStatus).removeClass("d-none").siblings().addClass("d-none");
    })
    $("#per_statusnext").unbind("click").click(function () {
        IndexStatus -= 1;
        (IndexStatus < 0 ? IndexStatus = pagenum : 0);
        $("#generalInfo_consultitem" + IndexStatus).removeClass("d-none").siblings().addClass("d-none");
    })
}
function GeneralInfo_CheckDataLinkOld(value, type) {
    let result = '';
    switch (type) {
        case 'facebook':
            result = value.split('facebook.com/').length > 1 ? value : 'https://facebook.com/' + value;
            break;
        case 'zalo':
            result = value.split('zalo.me/').length > 1 ? value : 'https://zalo.me/' + value;
            break;
        case 'viber':
            result = value.split('viber.com/').length > 1 ? value : 'https://viber.com/' + value;
            break;
        case 'instagram':
            result = value.split('instagram.com/').length > 1 ? value : 'https://instagram.com/' + value;
            break;
    }
    return result;
}
//#endregion

//#region //Properties
function GeneralInfo_LoadPro (IsNew = 1) {
    try {
        if (IsNew == 1) {
            _GeneralInfo_IndexProperti = 0;
        }
        AjaxLoad(url = "/Customer/GeneralInfo/?handler=LoadProperties"
            , data = {'custID': ser_MainCustomerID}
            , async = true
            , error = function () {notiError_SW()}
            , success = function (result) {
                if (result != "0") {
                    let data = JSON.parse(result);
                    data = GeneralInfo_SlidePro(data);
                    if (data.length > 0) {
                        _GeneralInfo_DataProperti = data;
                        //_GeneralInfo_IndexProperti = 0;
                        GeneralInfo_BeforeRenderPro();
                    }
                    GeneralInfo_EventInitPro();
                }
            }
        );
    } catch (ex) {
    }
}


function GeneralInfo_SlidePro (data) {
    try {
        let result = [];
        let _ChekDate = ',';
        if (data && data.length > 0) {
            for (let _i = 0; _i < data.length; _i++) {
                let item = data[_i];
                if (!_ChekDate.includes(',' + item["CreInt"] + ',')) {
                    _ChekDate += item["CreInt"] + ',';
                    let _DT = data.filter((word) => {return word["CreInt"] == item["CreInt"]});
                    _DT.sort((a, b) => a.ProID - b.ProID);
                    result.push(_DT);
                }
            }
        }
        return result;
    } catch {
        return [];
    }
}
function GeneralInfo_BeforeRenderPro () {
    GeneralInfo_RenderPro(_GeneralInfo_DataProperti[_GeneralInfo_IndexProperti], "GeneralInfo_BodyProperties");
}

async function GeneralInfo_RenderPro (data, id) {
    new Promise(resolve => {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    let item = data[i];
                    let tr = i == 0 ? `
                        <div class="d-lg-flex mb-2">
                            <h6 class="text-primary border-bottom border-primary cursor-pointer text-sm fw-bold mb-0 pe-1 btnProEdit" data-numdate="${item.CreInt}">${ConvertDateTimeUTC_DMYHM(item.Created)}</h6>
                            <h6 class="text-dark text-sm mb-0">: ${item.CreatedName}</h6>
                        </div>`
                        : ``;
                    tr += `
                        <div class="my-1 ${i == data.length - 1 ? "mb-3" : ""} border-3">
                            <span class="text-dark text-sm">
                                <span class="badge text-xs text-capitalize px-2 py-1 me-1 rounded-1 badge-primary" style="color: ${item.Color} !important;background-color: ${item.Color}2b !important;">${item.Name}</span> ${item.Value}
                            </span>
                        </div>
                        `;
                    stringContent = stringContent + tr;
                }
                document.getElementById(id).innerHTML = `<div>${stringContent}</div>`;
                GeneralInfo_EventPro();
            }
        }
        resolve();
    });
}
function GeneralInfo_EventInitPro () {
    let LengthDT = _GeneralInfo_DataProperti && _GeneralInfo_DataProperti.length > 0 ? _GeneralInfo_DataProperti.length - 1 : 0;
    $("#GeneralInfo_PropertiPreview").unbind("click").click(function () {
        if (LengthDT != 0) {
            _GeneralInfo_IndexProperti = _GeneralInfo_IndexProperti == 0 ? LengthDT : _GeneralInfo_IndexProperti - 1;
            GeneralInfo_BeforeRenderPro();
        }
    })
    $("#GeneralInfo_PropertiNext").unbind("click").click(function () {
        if (LengthDT != 0) {
            _GeneralInfo_IndexProperti = _GeneralInfo_IndexProperti == LengthDT ? 0 : _GeneralInfo_IndexProperti + 1;
            GeneralInfo_BeforeRenderPro();
        }
    })
}
function GeneralInfo_EventPro () {
    $("#GeneralInfo_BodyProperties .btnProEdit").unbind("click").click(function () {
        let NumDate = $(this).attr("data-numdate");
        if (NumDate != "0") {
            GeneralInfo_OpenDetailPro(NumDate);
        }
    })
}

function GeneralInfo_OpenDetailPro (DateCreated = "0") {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/Properties/PropertiesDetail?"
        + "CustomerID=" + ser_MainCustomerID
        + "&DateCreated=" + DateCreated
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}

//#endregion