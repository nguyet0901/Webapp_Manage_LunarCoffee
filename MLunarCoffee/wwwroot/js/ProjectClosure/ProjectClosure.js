(function ($) {
    let guideclosure = 0;
    
    let guidemo_domain = window.location.hostname;
    if (guidemo_domain != undefined && guidemo_domain != "") {
        guidemo_domain = guidemo_domain.toLocaleLowerCase();
        if (guidemo_domain == "localhost" || guidemo_domain == "tmdemo.vttechsolution.com" || guidemo_domain == "nkdemo.vttechsolution.com" || guidemo_domain == "pkdemo.vttechsolution.com") guideclosure = 1;
    }
    if (guideclosure == 1 && Master_roleID != 1) {
        let activetrial = false;
        let isusingtrial = localstorage_get("TrialVerLocal");
        let guidemo_isUsingLocal = (isusingtrial !== undefined && isusingtrial != "" && JSON.parse(isusingtrial).data !== "")
            ? JSON.parse(isusingtrial).data
            : false;
        switch (guidemo_domain) {
            case "localhost":
                {
                    activetrial = guidemo_isUsingLocal;
                    break;
                }
            case "tmdemo.vttechsolution.com":
                {
                    activetrial = true;
                    break;
                }
            case "nkdemo.vttechsolution.com":
                {
                    activetrial = true;
                    break;
                }
            case "pkdemo.vttechsolution.com":
                {
                    activetrial = true;
                    break;
                }
            default: break;
        }

        var gdata_Step = {};
        var Maindata_Module;
        js_require_notasync('/js/ProjectClosure/data.js', true);
        js_require_notasync('/js/ProjectClosure/mutation.js', true);
        $('head').append($('<link rel="stylesheet" type="text/css" />').attr('href', '/js/ProjectClosure/projectclosure.css'));
        var fileDataFunc = "/js/ProjectClosure/DataFunc.json";

        let keyTrial = '';
        if (Number(sys_dencos_Main) == 0) {
            keyTrial = gdata_TrialPackageTM[Master_roleID] != '' ? gdata_TrialPackageTM[Master_roleID] : 'Basic';
            Maindata_Module = gdata_ModuleTM;
        }

        else {
            keyTrial = gdata_TrialPackageNK[Master_roleID] != '' ? gdata_TrialPackageNK[Master_roleID] : 'Basic';
            Maindata_Module = gdata_ModuleNK;
        }


        

        if (activetrial) {
            gd_Mutation();
            gd_addchat();
            gd_trialgenre();
            $.getJSON(fileDataFunc, function (data) {
                gdata_Step = data;
                guidemo_getstart();
            });
        }

        //#region // Genre and event
        async function gd_trialgenre () {

            var myNode = document.getElementById("sidenav-collapse-main");
            if (myNode !== null) {
                var blockInfoPack = document.getElementById("blockInfoPackage");
                if (blockInfoPack !== null) blockInfoPack.remove();

                //#region // Tooltip
                let toolTipVer = ``;
                for ([key, value] of Object.entries(gdata_Package)) {
                    let eho = ((keyTrial === key) ? `<div class='fw-bold'>${value.title}</div>` : `<div class='opacity-5'>${value.title}</div>`);
                    if (key !== Object.keys(gdata_Package)[0]) eho = `<hr class='my-1' />` + eho;
                    toolTipVer = toolTipVer + eho;
                }
                toolTipVer = `<div><div class='text-start m-3'>${toolTipVer}</div></div>`
                //#endregion

                let stringTrial = `
                <div class="mx-3 mt-3 position-relative">
                    <div id="blockTrialName" class="btn btn-outline-primary w-100 border-2" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-html="true">
                        <div class="d-flex  align-items-center justify-content-between">
                            <span>Gói demo</span>
                            <span class="text-dark text-sm">${gdata_Package[keyTrial].title}</span>
                        </div>
                        <hr class="my-2"/>
                        <div class="d-flex  align-items-center justify-content-between mt-1">
                            <span>Nhóm</span>
                            <span class="text-dark  text-sm">${Master_roleGroupCurrent.slice(0, -1).slice(0, -1)}</span>
                        </div>
                        <hr class="my-2"/>
                        <div class="d-flex  align-items-center mt-1 justify-content-between">
                            <span>Hỗ trợ</span>
                            <span class="text-dark text-sm" style="letter-spacing: 2px;">1900.6627</span>
                        </div>

                    </div>


                </div>
            `;
                let stringGroup = gdata_Group
                    .filter((word) => {return keyTrial == word.Type})
                    .map((item) => {
                        return `<li class="guidemo_peruser__item" class="nav-item" role="presentation" data-id="${item.ID}">
                    <a class="text-sm nav-link cursor-pointer" data-hover="">${item.Name}</a>
                </li>`;
                    }).join(`<li><hr class="horizontal dark my-0 opacity-2"></li>`);
                let stringPermis = `
                <div class="mx-3">
                    <div class="position-relative">
                        <button class="w-100 btn btn-primary my-0 position-relative dropdown-toggle mb-0" data-bs-toggle="collapse" data-bs-target="#colListsGroup">
                            Chuyển chức năng
                        </button>
                        <div class="collapse position-absolute z-index-3 top-100 mt-2 start-0 w-100" id="colListsGroup">
                            <ul id="guidemo_peruser" class="nav nav-pills flex-column bg-white border-radius-lg py-3 px-2 shadow-lg">
                                ${stringGroup}
                            </ul>
                        </div>
                    </div>

                </div>
            `;
                myNode.insertAdjacentHTML(
                    "beforebegin",
                    `<div id="blockInfoPackage" >${stringTrial + stringPermis}</div>`
                );
                gd_trialevent(toolTipVer);
            }
        }
        function gd_trialevent (toolTipVer) {
            $("#guidemo_peruser .guidemo_peruser__item").on("click", function (e) {
                let GroupID = Number($(this).attr("data-id"));
                if (
                    isNaN(GroupID) ||
                    sys_userID_Main == undefined ||
                    sys_userID_Main == 0 ||
                    Number(Master_roleID) == GroupID
                ) return;
                gd_changegroup(GroupID);
            });
            setTimeout(() => {
                $("#blockTrialName").attr("title", toolTipVer);
                $("#blockTrialName").tooltip();
            }, 1000)
        }
        //#endregion

        //#region // Update Group
        function gd_changegroup (GroupID) {
            try {
                AjaxJWT(
                    url = "/api/Home/UpdateGroupUser",
                    data = JSON.stringify({
                        UserID: sys_userID_Main,
                        GroupID: GroupID,
                    }),
                    async = true,
                    success = function (result) {
                        if (result != "0") {
                            gd_updaterole();
                        }
                    }
                );
            }
            catch (ex) {

            }
        }
        async function gd_updaterole () {
            let objAuthor = author_get("author");
            let objFcm = author_get("fcm");
            if (objAuthor == undefined || objAuthor == "") return;
            let ip = await gd_detectip();

            if (ip == undefined || ip == "" || ip == "0") return;
            let {username, password} = JSON.parse(objAuthor);
            let {token} = objFcm != "" ? JSON.parse(objFcm) : {token: ""};

            AjaxApi((url = "/api/Author/Login"),
                (data = JSON.stringify({
                    UserName: username,
                    Password: "",
                    PasswordEnCrypt: password,
                    IP: ip,
                    TokenFCM: token,
                })),
                (async = true),
                (success = function (result) {

                    let obj = JSON.parse(result);
                    localStorage.setItem("WebToken", obj.Session);
                    localstorage_set("TokenTopic", obj.TokenTopic);
                    gd_updatebasedata();
                })
            );
        }
        async function gd_detectip () {
            return new Promise((resolve) => {
                AjaxApi(
                    url = "/api/Author/GetIP",
                    data = JSON.stringify({}),
                    async = true,
                    success = function (result) {
                        if (result != "0") {
                            let d = JSON.parse(result);
                            resolve(d.ip_encry);
                        } else {
                            resolve("0");
                        }
                    },
                    before = function (e) { }
                );
            });
        }
        function gd_updatebasedata () {
            AjaxJWT(url = "/api/Author/BaseData"
                , data = JSON.stringify({})
                , async = true
                , success = function (result) {
                    localstorage_set(session_base, JSON.parse(result));
                    gd_directpage()
                }
            );
        }
        function gd_directpage () {
            try {
                var href = '/Appointment/AppointmentInDay';
                if (session_base && session_base !== '') {
                    var Master_Data = JSON.parse(localstorage_get(session_base)).data;
                    if (Master_Data && Object.values(Master_Data).length !== 0) {
                        var PermissionTableMenu = JSON.parse(Master_Data.PermissionTable_Menu);
                        if (PermissionTableMenu && PermissionTableMenu[0].MenuIDText !== 'menuCustomerCreate')
                            href = PermissionTableMenu[0].MenuURL;
                        else href = PermissionTableMenu[1].MenuURL;
                    }
                }
                window.location.href = href;
            }
            catch (ex) {
                window.location.href = '/Appointment/AppointmentInDay';
            }
        }
        //#endregion

        //#region // Welcome and guide
        async function guidemo_getstart () {
            setTimeout(() => {
                gd_Titlerender();
                gd_DetailRender(function () {
                    gd_TitleAction();
                    gd_DetailEvent();
                });
            }, 500)
        }
        async function gd_DetailRender (callback) {
            try {
                var bodyMain = document.getElementById("BodyMain");
                if (bodyMain === null) return;
                let stringGroupUser = gdata_Group.filter((word) => {
                    return (word.IsActive && keyTrial == word.Type)
                }).map((item) => {
                    return `
                <div class="col-lg-4 col-md-12 my-3">
                    <div class="card cursor-pointer groupPerDemo__item mb-4 h-90" data-id="${item.ID}">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-12">
                                    <div class="numbers">
                                        <p class="text-lg  mb-0 fw-bold text-primary">${item.Name}</p>
                                        <hr class="horizontal dark my-1">
                                        <h5 class="des text-sm text-dark mt-2">
                                            ${item.Description}
                                        </h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                `
                }).join('');

                let stringModalWelcome = `
            <div class="modal fade" id="Welcome_Modal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true" >
                <div class="modal-dialog modal-xl">
                    <div id="Welcome_ModalContent" class="modal-content bg-gray-100">
                        <div class="container-fluid px-0">
                            <div class="card">
                                <div class="card-body p-5">
                                    <button type="button" class="btn-close text-danger fs-1 position-absolute top-1 end-1" data-bs-dismiss="modal" aria-label="Close" >
                                        <span aria-hidden="true">×</span>
                                    </button>
                                    <h4 class="mb-0 fw-bold">VTTECH xin chào Anh/Chị <strong class="text-primary text-uppercase">${sys_userName_Main}!</strong></h4>
                                    <p class="mb-0 mt-3 text-sm text-dark">Đây là <b>phiên bản demo</b>, nhằm mục đích hướng dẫn Anh/chị làm quen với hệ thống phần mềm Vttech.</p>
                                    <p class="mb-0 mt-0 text-sm text-dark">Chúng tôi xin được phép giới thiếu <b>một số chức năng cơ bản, cho từng nhóm user</b> trong quá trình vận hành.</p>
                                    <p class="mb-0 mt-0 text-sm fw-bold text-primary">Khi sử dụng chính thức,có thể có những tùy chỉnh để phù hợp với mô hình thực tế của Quý Anh/Chị.</p>
                                    <hr class="my-4"/>
                                    <div class="col-sm-8 mx-auto">
                                        <div id="groupPerDemo" class="row" >
                                            ${stringGroupUser}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            `;
                bodyMain.insertAdjacentHTML("beforeend", stringModalWelcome);
                callback();
            }
            catch (ex) {
                callback();
            }
        }
        function gd_TitleAction () {
            $('#Welcome_Modal').modal({backdrop: 'static', keyboard: false})
            let isFirstLogin = localstorage_get("TrialVerFirstLogin");
            if (isFirstLogin === '') $('#btnWelcome_Modal').trigger('click');
            localstorage_set("TrialVerFirstLogin", false);

            var pushState = history.pushState;
            history.pushState = function (state) {
                if (typeof history.onpushstate === "function") {
                    history.onpushstate({state: state});
                }
                gd_Titlerender(state);
                return pushState.apply(history, arguments);
            };
            window.addEventListener("popstate", function (e) {
                if (e.state) gd_Titlerender(e.state);
            });
        }
        function gd_DetailEvent () {
            $("#groupPerDemo .groupPerDemo__item").unbind('click').on("click", function (event) {
                let GroupID = Number($(this).attr("data-id"));
                if (isNaN(GroupID) ||
                    sys_userID_Main == undefined ||
                    sys_userID_Main == 0 ||
                    Number(Master_roleID) == GroupID)
                    return;
                gd_changegroup(GroupID);
            });

        }
        async function gd_Titlerender (path = "") {
            try {
                var btnGetstart = document.getElementById("GettingStart_BlockBtn");
                var modalGetstart = document.getElementById("GettingStart_Modal");
                var btnWelcome = document.getElementById("btnWelcome_Modal");
                if (btnGetstart !== null) btnGetstart.remove();
                if (modalGetstart !== null) modalGetstart.remove();
                if (btnWelcome !== null) btnWelcome.remove();

                let dataPath = gd_TitleDetectPath(path);
                let buttoninfo = `<button id="btnWelcome_Modal" type="button" class="btn" data-bs-toggle="modal" data-bs-target="#Welcome_Modal">
                        <i class="fas fa-info"></i>
                    </button>`;

                if (dataPath && Object.entries(dataPath).length !== 0) {
                    var bodyMain = document.getElementById("BodyMain");
                    if (bodyMain === null) return;
                    let stringButton = `

                <div id="GettingStart_BlockBtn" class="d-none d-md-block">
                    <button id="GettingStart_Btn" class="btn btn-primary btn-gettingstart" type="button">
                        <i class="fas fa-rocket me-2 fs-5 gettingstart-icon"></i>
                        <span>Bắt đầu sử dụng</span>
                    </button>
                </div>
                `;
                    let stringModal = `
                <div class="modal fade" id="GettingStart_Modal" data-bs-backdrop="static" role="dialog">
                    <div class="modal-dialog modal-xl">
                        <div id="GettingStart_ModalContent" class="modal-content bg-gray-100">
                            <div class="container-fluid px-0">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="card">
                                            <div class="card-body p-4 ps-0">
                                                <button type="button" class="btn-close text-dark position-absolute top-1 end-1" data-bs-dismiss="modal" aria-label="Close" >
                                                    <span aria-hidden="true">×</span>
                                                </button>
                                                <div class="row">
                                                    <div class="field col-12 col-md-6 col-lg-3 border-end-sm position-relative">
                                                        <div class="pb-3 border-bottom ps-4">
                                                            <h6 id="GettingStart_Title" class="mb-0 fw-bold ellipsis_one_line">#Title#</h6>
                                                            </br>
                                                            <p id="GettingStart_SubTitle" class="text-sm mb-0 mt-1">#SubTitle#</p>
                                                        </div>
                                                        <div id="GettingStart_ListStep" class="list-group list-group-flush mt-4 me-3 min-height-300 mb-5">
                                                        </div>
                                                        <div class="ps-4 text-xs position-absolute bottom-0 start-0">
                                                            <span class="me-1 fw-bold text-sm">
                                                                <i class="fas fa-phone-alt me-1"></i>Tổng đài hỗ trợ
                                                                <a href="tel:19006627" class="text-primary">  1900.6627</a>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="field col-12 col-md-6 col-lg-9">
                                                        <div id="GettingStart_StepDetail" class="my-3 ps-3 ">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div
                            </div>
                        </div>
                    </div>
                </div>
                `;
                    bodyMain.insertAdjacentHTML("beforeend",
                        `<div class="d-flex position-fixed top-0  start-50 translate-middle-x z-index-sticky">${stringButton}${buttoninfo}</div>`

                        + stringModal);
                    gd_TitleEvent();
                }
            }
            catch (ex) {
            }
        }
        function gd_TitleEvent () {
            $("#GettingStart_Btn").on("click", function () {
                let data = gd_TitleDetectPath();
                if (data && Object.entries(data).length != 0) {
                    $("#GettingStart_Modal").modal("show");
                    gd_DetailFill(data);
                }
            });
            $("#GettingStart_Modal").on("click", function (event) {
                if (!$(event.target).is("#GettingStart_ModalContent") && !$(event.target).closest("#GettingStart_ModalContent").length != 0) {
                    $("#GettingStart_Modal").modal("hide");
                }

            });
        }
        async function gd_DetailFill (data) {
            if (data && data.length != 0) {
                $("#GettingStart_Title").html(data.Title);
                $("#GettingStart_SubTitle").html(data.SubTitle);
                gd_DetailStep_Render(data.Steps, "GettingStart_ListStep");
            }
        }
        async function gd_DetailStep_Render (data, id) {
            new Promise((resolve) => {
                var myNode = document.getElementById(id);
                if (myNode !== null) {
                    myNode.innerHTML = "";
                    let stringContent = "";
                    if (data !== undefined && data.length !== 0) {
                        for (let i = 0; i < data.length; i++) {
                            let value = gdata_Step[data[i]];
                            if (value !== undefined) {
                                let tr = `
                            <div class="getstep_item fw-bold text-dark d-flex justify-content-start align-items-center py-2 text-sm ps-4 cursor-pointer mb-1" data-id="${data[i]}">
                                <span class="text-sm mb-0 ellipsis_one_line">${value.Title}</span>
                            </div>`;
                                stringContent += tr;
                            }
                        }
                    }
                    myNode.innerHTML = stringContent;
                    gd_DetailStep_Event();
                }
            });
        }
        function gd_DetailStep_Event () {
            $("#GettingStart_ListStep .getstep_item").on("click", function () {
                $("#GettingStart_StepDetail").empty().hide();
                let id = Number($(this).attr("data-id"));
                if (isNaN(id)) return;
                let dataPath = gd_TitleDetectPath();
                if (dataPath && Object.entries(dataPath).length) {
                    $(this).addClass("active").siblings().removeClass("active");
                    let item = gdata_Step[id];
                    if (item != undefined) {
                        let stringDetail = `
                        <div class="w-100 text-center">
                            <img src="${item.Media}?ran=${Math.random()}" class="mw-100 rounded-2"  />
                        </div>
                        <p class=" ellipsis_three_line mt-3 text-dark text-md" >${item.Content}</p>
                    `
                        $("#GettingStart_StepDetail").fadeIn(300);
                        $("#GettingStart_StepDetail").html(stringDetail);
                    }
                }
            });
            $("#GettingStart_ListStep .getstep_item:first-child").trigger("click");
        }
        function gd_TitleDetectPath (path = "") {
            try {
                if (Maindata_Module == undefined) return {};
                let getPath = (path != "" ? path : window.location.pathname).toLowerCase();
                for ([key, value] of Object.entries(Maindata_Module)) {
                    let arrPath = (value.IsPath).split(",");
                    for (let i = 0; i < arrPath.length; i++) {
                        let pathItem = (arrPath[i]).toLowerCase()
                        if (getPath == pathItem || getPath + "/" == pathItem) {
                            return value;
                        }
                    }
                }
                return {};
            } catch {
                return {};
            }
        }
        //#endregion

        //#region //Add chat
        async function gd_addchat () {
            await new Promise((resolve, reject) => {
                setTimeout(
                    () => {
                        var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
                        (function () {
                            var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
                            s1.async = true;
                            s1.src = 'https://embed.tawk.to/5e9563fd35bcbb0c9ab0cc4a/1gfq73g16';
                            s1.charset = 'UTF-8';
                            s1.setAttribute('crossorigin', '*');
                            s0.parentNode.insertBefore(s1, s0);

                        })();
                    }, 100)
            })
        }
        async function gd_Mutation () {
            await new Promise((resolve, reject) => {
                setTimeout(
                    () => {
                        //let urlData = `${gdata_Package[keyTrial].url}`;
                        let urlData = `/js/ProjectClosure/TrialBasic.js`;
                        js_require_notasync(urlData, true);
                        guidemo_stcontrol();
                        guidemo_dycontrol();
                        guidemo_mutabody();
                        resolve()
                    }
                    , 100)
            })
        }
        //#endregion
    }


})(jQuery);

