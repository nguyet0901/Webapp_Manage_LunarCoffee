// Schedule //////////////
function addNewSchedule(customerID) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CustomerID=" + customerID + "&typeHistory=1" + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function editSchedule(id, customerid) {

    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CurrentID=" + id + "&CustomerID=" + customerid + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
function newAppointment() {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Appointment/AppointmentDetail?CustomerID=" + ser_MainCustomerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
///////////////////

// History //////////////

function addNewHistory(customerID) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/History/HistoryDetail?"
        + "MasterID=" + 0
        + "&CustomerID=" + customerID
        + "&IsAdvice=" + 0
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function editHistory(id, customerid) {
    $("#DetailModal_Content").empty();
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/History/HistoryDetail?"
        + "&CustomerID=" + customerid
        + "&IsAdvice=" + 0
        + "&MasterID=" + id
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}

// Advice //////////////

function addNewAdvice(customerID) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/History/HistoryDetail?"
        + "MasterID=" + 0
        + "&CustomerID=" + customerID
        + "&IsAdvice=" + 1
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function editAdvice(id, customerid) {
    $("#DetailModal_Content").empty();
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/History/HistoryDetail?"
        + "&CustomerID=" + customerid
        + "&IsAdvice=" + 1
        + "&MasterID=" + id
        + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}

// Treatment //////////////
function addNewTreatment(customerID) {

    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/TreatmentDetail?CustomerID=" + customerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}


function editTreatment(id, customerid) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/TreatmentDetail?CurrentID=" + id + "&CustomerID=" + customerid + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
///////////////////

// Status //////////////
function addNewStatus(customerID) {

    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/StatusDetail?CustomerID=" + customerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function editStatus(id, customerid) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/StatusDetail?CurrentID=" + id + "&CustomerID=" + customerid + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
//function ChangeTabService(id) {
//    $("#DetailModal_Content").html('');
//    $("#DetailModal_Content").load("/Customer/Service/ServiceChange?CurrentID=" + id + "&ver=" + versionofWebApplication);
//    $('#DetailModal').modal('show');
//}
//function ChangeTabServiceDentist(id) {
//    $("#DetailModal_Content").html('');
//    $("#DetailModal_Content").load("/Customer/Service/ServiceChangeDentist?CurrentID=" + id + "&ver=" + versionofWebApplication);
//    $('#DetailModal').modal('show');
//}
///////////////////

// Labo //////////////
function addNewLaboCus(customerID) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/LaboCusDetail?CustomerID=" + customerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
function editLaboCus(id, customerid) {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/LaboCusDetail?CurrentID=" + id + "&CustomerID=" + customerid + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
///////////////////

// Edit Customer ///////////////////
function editCustomer() {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/CustomerDetail?CustomerID=" + ser_MainCustomerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');

}
function editCustBranch() {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/BranchChangeDetail?&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
function printCustomerRecord() {
    syslog_cutcon('p', ser_MainCustomerID, '');
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load('/Print/print?Type=customer_record&DetailID=' + ser_MainCustomerID);
    $('#DetailModal').modal('show');
}
// Send SMS///////////////////
function addNewSMS() {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Sms/SmsDetail?Type=" + "SMSContentGeneral" + "&CustomerID=" + ser_MainCustomerID + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}
// Customergate///////////////////
function addMessageCustGate () {
    $("#General_Chatting").removeClass('d-none');
    $("#General_ChattingArea").html('');
    $("#General_ChattingArea").load("/Chatting/Message?CustID=" + ser_MainCustomerID);
}
function addNewMail() {
    $("#MainSendMail_Content").empty();
    $("#MainSendMail_Content").load('/Mail/MailDetail?CustomerID=' + ser_MainCustomerID + '&ver=' + versionofWebApplication,
        function () {
            $("#MainSendMail").addClass('active');
        }
    );
}

function Genin_ToTicket () {
    window.open('/Marketing/TicketAction?CustomerID=' + ser_MainCustomerID + '&TicketID=' + ser_TicketID +'&ver='+versionofWebApplication, 'blank');
}
function CloseMessageCustGate () {
    $("#General_Chatting").addClass('d-none');
    $("#General_Chatting .max").removeClass('d-none');
    $("#General_Chatting .min").addClass('d-none');
    $("#General_ChattingArea").html('');
    $("#General_Chatting").removeClass('max');
}
function MaxMessageCustGate () {
    $("#General_Chatting").addClass('max');
    $("#General_Chatting .max").addClass('d-none');
    $("#General_Chatting .min").removeClass('d-none');
}
function MinMessageCustGate () {
    $("#General_Chatting .max").removeClass('d-none');
    $("#General_Chatting .min").addClass('d-none');
    $("#General_Chatting").removeClass('max');
}
///////////////////

//print customer info///////////////////
function PrintCustomerStatus (customerID) {
    syslog_cutcon('p', customerID, '');
    window.open("/Print/print_TemplateInfo?id=" + customerID + "&branch=" + ser_MainBranchID + "&type=2&ver=" + versionofWebApplication, '_blank', 'location=yes,height=650px,width=1200px,scrollbars=yes,status=yes');
}
function PrintCustomerStatusNotData (customerID) {
    syslog_cutcon('p', customerID, '');
    window.open("/Print/print_TemplateInfo?id=" + customerID + "&branch=" + ser_MainBranchID + "&type=5&ver=" + versionofWebApplication, '_blank', 'location=yes,height=1200px,width=800px,scrollbars=yes,status=yes');
}
function PrintCustomerServiceList () {
    syslog_cutcon('p', ser_MainCustomerID, '');
    let ID = Number(ser_current_treatment_plant_id);
        $("#DetailModal_Content").html('');
        $("#DetailModal_Content").load('/Print/print?Type=treatment_plan&DetailID=' + ID);
        $('#DetailModal').modal('show');
}

function PrintAllServiceList () {
    syslog_cutcon('p', ser_MainCustomerID, '');
    let ID = Number(ser_MainCustomerID);
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load('/Print/print?Type=service_all&DetailID=' + ID);
    $('#DetailModal').modal('show');
}
///////////////////

///Call Customer

function callCustomer (cust_id) {

    if (ser_MainPhoneCustomer != "") {
        if (typeof CCF_OutcomCall !== 'undefined' && $.isFunction(CCF_OutcomCall)) {
            CCF_OutcomCall(encrypt_phone(ser_MainPhoneCustomer), cust_id, 0, 0);
        }
    }
}

function addEmail(cust_id) {
    if (cust_id != "") {

    }
}