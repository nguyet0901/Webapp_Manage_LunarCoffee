function PrintTemplate(id, branchid, type, versionofWebApplication, CustomerID) {

    $('#tablePrintForAll').empty();
    var formname = "";
    var width = 0;
    var height = 0;
    switch (Number(type)) {
        case 4://In Deposit
        case 6://In Invoice Labo
        case 7://In Invoice Card
        case 10://In điều trị
        case 11://In phiếu Order
        case 13://In Lịch hẹn
            height = 700;
            width = 1200;
            formname = "print_TemplateAll";
            break;
        case 1://In thanh toán dịch vụ
            height = 700;
            width = 1200;
            formname = "print_Template";
            break;
        case 3://In kế danh sách dịch vụ
            height = 700;
            width = 1200;
            formname = "print_TemplateServiceList";
            break;
        case 8: //thanh toán Thuốc
            height = 700;
            width = 1200;
            formname = "print_TemplateInvoicePrescription";
            break;
        case 9: // in toa thuốc
            height = 700;
            width = 1200;
            formname = "print_TemplatePrescription";
            break;
        case 12: //In kế hoạch điều trị
            height = 700;
            width = 1200;
            formname = "print_TemplateServiceList";
            break;
        case 20:
            height = 700;
            width = 1200;
            formname = "print_TemplateAll";
            break;
        case 21:
            height = 700;
            width = 1200;
            formname = "print_TemplateCustomerStatus";
            break;
        case 22:
            height = 700;
            width = 1200;
            formname = "print_TemplateCustomerPatient";
            break;
        case 23:
            height = 700;
            width = 1200;
            formname = "print_TemplateCustomerAssay";
            break;
        case 24:
            height = 700;
            width = 1200;
            formname = "print_TemplateCustomerPatientRecord";
            break;
        default:
            break;
        // code block
    }

    window.open("/Print/" + formname + "?id=" + id + "&branch=" + branchid + "&type=" + type + "&ver=" + versionofWebApplication + "&customer=" + ((CustomerID) ? (CustomerID) : ('0')), '_blank', 'location=yes,height=' + height + ',width=' + width +',scrollbars=yes,status=yes');
    
}
