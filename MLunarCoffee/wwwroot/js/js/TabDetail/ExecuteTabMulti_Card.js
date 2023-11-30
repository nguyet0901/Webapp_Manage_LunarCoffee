function Tab_Detail_Card_Render (data, id) {

    var myNode = document.getElementById(id);
    if (myNode !== null) {
        myNode.innerHTML = '';
        let stringContent = '';
        for ([key, value] of Object.entries(data)) {
            //let isHide = (Number(value.ValueUsed) != 0) ? 'block' : 'none';
            let cardholder = '';
            let cardnumber = value.Code ? value.Code : "";//'****' + ' ' + value.CustID  ;
            let cardUsed = '<div class="position-absolute end-0 form-check ">'
                + '<input id="card_detail_' + key + '" class="checkCard form-check-input pr-2" type="checkbox" value=' + key + ' id="card_detail_' + key + '">'
                + '</div>';
            let cardLEFT = '<div class="me-4 my-1 d-flex align-items-center">'
                + '<h6 class="text-white text-xs opacity-8 mb-0 pe-2">' + Outlang["Gia_tri"] + '</h6>'
                + '<h6 id="card_left_' + key + '" class="card_left fw-bolder text-white text-xs mb-0">'
                + formatNumber(value.ValueLeft) + '</h6>'
                + '</div>';
            if (Number(value.CustID) != ser_MainCustomerID) {
                cardholder =
                    `
                    <div class="my-1 d-flex align-items-center ">
                        <h6 class="text-white text-xs mb-0 pe-2">
                           <i class="fas fa-user text-xs"></i>
                        </h6>
                        <a class="ellipsis_one_line text-white text-sm card_info" title="${value.CustName}" target="_blank" href="/Customer/MainCustomer?CustomerID=${value.CustID}">
                           ${value.CustName}
                        </a>
                        
                    </div>
                     ` ;
            }

            let tr =
                 
                `<div id="card_detail_tab${key}" data-id="${key}" class="card_detail_tab bg-transparent p-2" style="min-width: 230px;">
                    <div class="maskcard overflow-hidden position-relative border-radius-xl">
                        <span class="mask bg-gradient-dark"></span>
                        <div class="card-body position-relative z-index-1 p-2 px-3 pe-0">
                            <div class="d-flex">
                                <div class="me-0">
                                    ${cardLEFT}
                                    <div class="d-flex">
                                        <a class="cursor-pointer me-2" data-bs-toggle="collapse" role="tab" data-bs-target="#carddetail${key}">
                                            <i class="text-white fas fa-chevron-down"></i>
                                        </a>
                                        <h6 class="mb-0 mt-1 fw-bold text-white text-xs pe-2">${value.Name}</h6>
                                    </div>

                                </div>
                                ${cardUsed}
                            </div>
                            <div class="collapse collapsesticky mt-1 me-3" id="carddetail${key}">
                                <div class="card card-plain">
                                    ${cardholder}
                                     <div class="my-1 d-flex align-items-center ">
                                        <h6 class="text-white text-xs  mb-0 pe-2">
                                            <i class="fas text-xs fa-qrcode"></i>
                                        </h6>
                                        <div title="${cardnumber}" class="text-white text-sm ellipsis_one_line">
                                           ${cardnumber}
                                        </div>
                        
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>` 
               
                 
                ;
            stringContent = stringContent + tr
        }
        myNode.innerHTML = stringContent;
    }
    Tab_Detail_Card_Event();
}

function Tab_Detail_Card_Event () {
    $(".checkCard").unbind().change(function () {
        if (tab_changing_flag == 1) {
            Tab_Count_Price_For_All();
        }
    });
}
function Tab_Detail_Disable_All_Card () {
    if (tab_changing_flag == 1) {
        tab_changing_flag = 0;
        $(".checkCard").attr("disabled", true);
        $(".card_detail_tab").addClass("card_hidden");
        var x = document.getElementsByClassName("checkCard");
        for (let i = 0; i < x.length; i++) {
            x[i].checked = false;
        }
        tab_changing_flag = 1;
        Tab_Count_Price_For_All();
    }
}

function Tab_Detail_Card_Changing_ByService () {

    let service_id = CurrentService_Choose;
    if ($('#IsCheckUsing')[0].checked == false || service_id == 0) {
        tab_changing_flag = 0;
        $(".checkCard").attr("disabled", true);
        $(".card_detail_tab").addClass("card_hidden");
        var x = document.getElementsByClassName("checkCard");
        for (let i = 0; i < x.length; i++) {
            x[i].checked = false;
        }
        tab_changing_flag = 1;
    }
    else {

        let service_string = ',' + service_id + ',';
        $(".checkCard").attr("disabled", true);
        $(".card_detail_tab").addClass("card_hidden");
        for ([key, value] of Object.entries(data_tab_card)) {
            let servicerange = ',' + value.ServiceRange + ',';
            if (servicerange.includes(service_string) || value.AllService == 1) {
                $("#card_detail_" + key).attr("disabled", false);
                $("#card_detail_tab" + key).removeClass("card_hidden");
            }
        }
    }
}