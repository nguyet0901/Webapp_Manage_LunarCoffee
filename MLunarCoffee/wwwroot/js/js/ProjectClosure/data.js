var gdata_Package = {
    Pro: {
        title: 'Gói cao cấp',
        icon: '<i class="fas fa-star text-gradient text-primary"></i>',
        url: '/js/ProjectClosure/TrialPro.js',
        content: 'Demo gói cao cấp'
    },
    Standard: {
        title: 'Gói tiêu chuẩn',
        icon: '<i class="fas fa-star-half-alt text-gradient text-primary"></i>',
        url: '/js/ProjectClosure/TrialStandard.js',
        content: 'Demo gói tiêu chuẩn'
    },
    Basic: {
        title: 'Gói cơ bản',
        icon: '<i class="far fa-star text-gradient text-primary"></i>',
        url: '/js/ProjectClosure/TrialBasic.js',
        content: 'Demo gói cơ bản'
    }
}

var gdata_ModuleNK = {
    MainCustomer: {
        Title: "Quản lý khách hàng",
        SubTitle: "Quản lý thông tin tập trung tại 1 nơi",
        IsPath: "/Customer/MainCustomer",
        Type: [0],
        Steps: [41, 0.1,8,9, 1.2, 2.1, 30, 33,32,25,40]
    }
    ,
    Appointment: {
        Title: "Quản lý lịch hẹn",
        SubTitle: "Theo dõi & quản lý lịch hẹn khách hàng",
        IsPath: "/Desk/DeskMaster,/Appointment/AppointmentInDay/,/Appointment/AppointmentByDay/",
        Type: [0],
        Steps: [12, 13, 14]
    }
    ,
    CustomerCare: {
        Title: "Chăm sóc khách hàng",
        SubTitle: "Tự động phân loại chăm sóc theo loại",
        IsPath: "/CustomerCare/CustomerCare_RemindAppointment/,/CustomerCare/CustomerCare_Holiday/,/CustomerCare/CustomerCare_CheckInNotService/,/CustomerCare/CustomerCare_NotCheckIn/,/CustomerCare/CustomerCare_AfterTreatment/,/CustomerCare/CustomerCare_Complaint/,/CustomerCare/CustomerCare_AppointmentCancel/,/CustomerCare/CustomerCare_PeriodTime",
        Type: [0],
        Steps: [4, 6, 5, 7]
    }
    ,
    Account: {
        Title: "Kế toán",
        SubTitle: "Quản lý Thu chi - công nợ, hoa hồng",
        IsPath: "/Account/InvoicePayment/,/account/paymentsupplier/paymentlist",
        Type: [0],
        Steps: [39, 22, 44, 24]
    }
    ,
    AppointmentDoctor: {
        Title: "Lịch hẹn Bác sĩ / Chuyên Gia",
        SubTitle: "Đặt hẹn & quản lý lý lịch theo cá nhân",
        IsPath: "/Appointment/Appointment/",
        Type: [0],
        Steps: [10, 11, 52, 34]
    }
    ,
    Marketing: {
        Title: "Quản lý Marketing",
        SubTitle: "Theo dõi quá trình xử lý & chuyển đổi",
        IsPath: "/Marketing/TicketTagList,/Marketing/FilterCustomer/,/Marketing/Call/HistoryCall/,/Marketing/ticketlist/",
        Type: [0],
        Steps: [15, 16, 29, 17]
    }
    ,

    WareHouse: {
        Title: "Quản lý kho - Vật tư",
        SubTitle: "Quản lý xuất - nhập & vật tư điều trị",
        IsPath: "/WareHouse/Dash/DashWarehouse/,/WareHouse/Require/RequireList/,/warehouse/treatmentsale/exportlist/",
        Type: [0],
        Steps: [18, 35, 45, 19, 20,]
    }
    ,
    Service: {
        Title: "Quản lý dịch vụ & sản phẩm",
        SubTitle: "Cấu hình chi tiết dịch vụ & sản phẩm",
        IsPath: "/Service/ServiceList/",
        Type: [0],
        Steps: [24, 19]
    }
    ,
    Card: {
        Title: "Thẻ trả trước",
        SubTitle: "Cấu hình thẻ và chi tiết sử dụng",
        IsPath: "/Card/Setting/ServiceCardList/,/Card/Status/CardList/",
        Type: [0],
        Steps: [27, 28]
    }
    ,
    Report: {
        Title: "Báo cáo hệ thống",
        SubTitle: "Báo các các dữ liệu chi tiết",
        IsPath: "/Report/ReportGeneral/",
        Type: [0],
        Steps: [43, 48]
    }
    ,
    Employee: {
        Title: "Thiết lập tài khoản & phân quyền",
        SubTitle: "Phân quyền truy cập, thao tác",
        IsPath: "/Employee/UserList/,/Permission/PermissionGeneral/,/employee/employeelist/",
        Type: [0],
        Steps: [37, 36, 38]
    }
    ,
    Setting: {
        Title: "Hướng dẫn cấu hình",
        SubTitle: "Các cấu hình chi tiết",
        IsPath: "/setting/settinglistparam/",
        Type: [0],
        Steps: [51]
    } ,
    Discount: {
        Title: "Chương trình khuyến mãi",
        SubTitle: "Cấu hình chương trình theo nhóm dịch vụ, dịch vụ thời gian áp dụng",
        IsPath: "/discount/discountlist/",
        Type: [0],
        Steps: [46]
    }
    ,
    Voucher: {
        Title: "Voucher & Thẻ thành viên",
        SubTitle: "Cấu hình voucher , thẻ thành viên nhóm dịch vụ, dịch vụ thời gian áp dụng",
        IsPath: "/discount/discountvoucherlist/",
        Type: [0],
        Steps: [47]
    }
};
var gdata_ModuleTM = {
    MainCustomer: {
        Title: "Quản lý khách hàng",
        SubTitle: "Quản lý thông tin tập trung tại 1 nơi",
        IsPath: "/Customer/MainCustomer",
        Type: [0],
        Steps: [26, 0, 1, 2, 31]
    }
    ,
    Appointment: {
        Title: "Quản lý lịch hẹn",
        SubTitle: "Theo dõi & quản lý lịch hẹn khách hàng",
        IsPath: "/Desk/DeskMaster,/Appointment/AppointmentInDay/,/Appointment/AppointmentByDay/",
        Type: [0],
        Steps: [12, 13, 14]
    }
    ,
    CustomerCare: {
        Title: "Chăm sóc khách hàng",
        SubTitle: "Tự động phân loại chăm sóc theo loại",
        IsPath: "/CustomerCare/CustomerCare_RemindAppointment/,/CustomerCare/CustomerCare_Holiday/,/CustomerCare/CustomerCare_CheckInNotService/,/CustomerCare/CustomerCare_NotCheckIn/,/CustomerCare/CustomerCare_AfterTreatment/,/CustomerCare/CustomerCare_Complaint/,/CustomerCare/CustomerCare_AppointmentCancel/,/CustomerCare/CustomerCare_PeriodTime",
        Type: [0],
        Steps: [4, 6, 5, 7]
    }
    ,
    Account: {
        Title: "Kế toán",
        SubTitle: "Quản lý Thu chi - công nợ, hoa hồng",
        IsPath: "/Account/InvoicePayment/",
        Type: [0],
        Steps: [39, 22, 23]
    }
    ,
    AppointmentDoctor: {
        Title: "Lịch hẹn Bác sĩ / Chuyên Gia",
        SubTitle: "Đặt hẹn & quản lý lý lịch theo cá nhân",
        IsPath: "/Appointment/Appointment/",
        Type: [0],
        Steps: [10, 11, 52, 34]
    }
    ,
    Marketing: {
        Title: "Quản lý Marketing",
        SubTitle: "Theo dõi quá trình xử lý & chuyển đổi",
       IsPath: "/Marketing/TicketTagList,/Marketing/FilterCustomer/,/Marketing/Call/HistoryCall/,/Marketing/ticketlist/",
        Type: [0],
        Steps: [15, 16, 29, 17]
    }
    ,

    WareHouse: {
        Title: "Quản lý kho - Vật tư",
        SubTitle: "Quản lý xuất - nhập & vật tư điều trị",
        IsPath: "/WareHouse/Dash/DashWarehouse/,/WareHouse/Require/RequireList/",
        Type: [0],
        Steps: [18, 35, 19, 20,]
    }
    ,
    Service: {
        Title: "Quản lý dịch vụ & sản phẩm",
        SubTitle: "Cấu hình chi tiết dịch vụ & sản phẩm",
        IsPath: "/Service/ServiceList/",
        Type: [0],
        Steps: [23, 19]
    }
    ,
    Card: {
        Title: "Thẻ trả trước",
        SubTitle: "Cấu hình thẻ và chi tiết sử dụng",
        IsPath: "/Card/Setting/ServiceCardList/,/Card/Status/CardList/",
        Type: [0],
        Steps: [27, 28]
    }
    ,
    Report: {
        Title: "Báo cáo hệ thống",
        SubTitle: "Báo các các dữ liệu chi tiết",
        IsPath: "/Report/ReportGeneral/",
        Type: [0],
        Steps: [43, 48]
    }
    ,
    Employee: {
        Title: "Thiết lập tài khoản & phân quyền",
        SubTitle: "Phân quyền truy cập, thao tác",
        IsPath: "/Employee/UserList/,/Permission/PermissionGeneral/,/employee/employeelist/",
        Type: [0],
        Steps: [37, 36, 38]
    }
    ,
    Setting: {
        Title: "Hướng dẫn cấu hình",
        SubTitle: "Các cấu hình chi tiết",
        IsPath: "/setting/settinglistparam/",
        Type: [0],
        Steps: [51]
    }
     ,
    Discount: {
        Title: "Chương trình khuyến mãi",
        SubTitle: "Cấu hình chương trình theo nhóm dịch vụ, dịch vụ thời gian áp dụng",
        IsPath: "/discount/discountlist/",
        Type: [0],
        Steps: [46]
    }
    ,
    Voucher: {
        Title: "Voucher & Thẻ thành viên",
        SubTitle: "Cấu hình voucher , thẻ thành viên nhóm dịch vụ, dịch vụ thời gian áp dụng",
        IsPath: "/discount/discountvoucherlist/",
        Type: [0],
        Steps: [47]
    }
};
var gdata_TrialPackageTM = {
    2: "Basic", 3: "Standard", 4: "Pro"
    , 5: "Basic", 6: "Standard", 7: "Pro"
    , 9: "Basic", 10: "Standard", 11: "Pro"
    , 12: "Basic", 13: "Standard", 14: "Pro"
    , 15: "Basic", 16: "Standard", 17: "Pro"
    , 18: "Basic", 19: "Standard", 20: "Pro"
    , 21: "Basic", 22: "Standard", 23: "Pro"
}
var gdata_TrialPackageNK = {
       2: "Basic", 3: "Standard", 4: "Pro"
    , 5: "Basic", 6: "Standard", 7: "Pro"
    , 9: "Basic", 10: "Standard", 11: "Pro"
    , 12: "Basic", 13: "Standard", 14: "Pro"
    , 15: "Basic", 16: "Standard", 17: "Pro"
    , 18: "Basic", 19: "Standard", 20: "Pro"
    , 21: "Basic", 22: "Standard", 23: "Pro"
}

var gdata_Group = [
    {
        ID: 2,
        Name: "Quản trị viên",
        Type: "Basic",
        Description: "Cấu hình danh mục, cài đặt hệ thống.Cấu hình phân quyền cho user và quản lý các quy định khác",
        IsActive: true
    },
    {
        ID: 5,
        Name: "Lễ Tân",
        Type: "Basic",
        Description: "Lịch hẹn , checkedin và điều phối khách hàng.Lên dịch vụ,bảo hiểm , trả góp và các thao tác khác",
        IsActive: true
    },
    {
        ID: 9,
        Name: "Marketing",
        Type: "Basic",
        Description: "Quản lý telesale, chương trình giảm giá , voucher và quản lý dữ liệu",
        IsActive: true
    },
    {
        ID: 12,
        Name: "Bác sĩ",
        Type: "Basic",
        Description: "Quá trình điều trị, tái khám và tư vấn. Chẩn đoán và hồ sơ bệnh án , kế hoạch điều trị",
        IsActive: true
    },
    {
        ID: 15,
        Name: "Chăm sóc khách hàng",
        Type: "Basic",
        Description: "Nhắc lịch, chăm sóc sau điều trị, định kỳ, hẹn không đến , khách hàng phàn nàn complain",
        IsActive: true
    },
    //{
    //    ID: 18,
    //    Name: "Quản lý kho",
    //    Type: "Basic",
    //    Description: "Xuất nhập hàng trong kho.Xuất bán hàng,xuất điều trị khách hàng.Order nhà cung cấp",
    //    IsActive: false
    //},
    {
        ID: 21,
        Name: "Kế toán",
        Type: "Basic",
        Description: "Thu chi chi nhánh, công nợ khách hàng nhà cung cấp",
        IsActive: true
    },
    {
        ID: 3,
        Name: "Quản trị viên",
        Type: "Standard",
        Description: "Cấu hình danh mục, cài đặt hệ thống.Cấu hình phân quyền cho user và quản lý các quy định khác",
        IsActive: true
    },
    {
        ID: 6,
        Name: "Lễ Tân",
        Type: "Standard",
        Description: "Lịch hẹn , checkedin và điều phối khách hàng.Lên dịch vụ,bảo hiểm , trả góp và các thao tác khác",
        IsActive: true
    },
    {
        ID: 10,
        Name: "Marketing",
        Type: "Standard",
        Description: "Quản lý telesale, chương trình giảm giá , voucher và quản lý dữ liệu",
        IsActive: true
    },
    {
        ID: 13,
        Name: "Bác sĩ",
        Type: "Standard",
        Description: "Quá trình điều trị, tái khám và tư vấn. Chẩn đoán và hồ sơ bệnh án , kế hoạch điều trị",
        IsActive: true
    },
    {
        ID: 16,
        Name: "Chăm sóc khách hàng",
        Type: "Standard",
        Description: "Nhắc lịch, chăm sóc sau điều trị, định kỳ, hẹn không đến , khách hàng phàn nàn complain",
        IsActive: true
    },
    {
        ID: 19,
        Name: "Quản lý kho",
        Type: "Standard",
        Description: "Xuất nhập hàng trong kho.Xuất bán hàng,xuất điều trị khách hàng.Order nhà cung cấp",
        IsActive: true
    },
    {
        ID: 22,
        Name: "Kế toán",
        Type: "Standard",
        Description: "Thu chi chi nhánh, công nợ khách hàng nhà cung cấp",
        IsActive: true
    },

    {
        ID: 4,
        Name: "Quản trị viên",
        Type: "Pro",
        Description: "Cấu hình danh mục, cài đặt hệ thống.Cấu hình phân quyền cho user và quản lý các quy định khác",
        IsActive: true
    },
    {
        ID: 7,
        Name: "Lễ Tân",
        Type: "Pro",
        Description: "Lịch hẹn , checkedin và điều phối khách hàng.Lên dịch vụ,bảo hiểm , trả góp và các thao tác khác",
        IsActive: true
    },
    {
        ID: 11,
        Name: "Marketing",
        Type: "Pro",
        Description: "Quản lý telesale, chương trình giảm giá , voucher và quản lý dữ liệu",
        IsActive: true
    },
    {
        ID: 14,
        Name: "Bác sĩ",
        Type: "Pro",
        Description: "Quá trình điều trị, tái khám và tư vấn. Chẩn đoán và hồ sơ bệnh án , kế hoạch điều trị",
        IsActive: true
    },
    {
        ID: 17,
        Name: "Chăm sóc khách hàng",
        Type: "Pro",
        Description: "Nhắc lịch, chăm sóc sau điều trị, định kỳ, hẹn không đến , khách hàng phàn nàn complain",
        IsActive: true
    },
    {
        ID: 20,
        Name: "Quản lý kho",
        Type: "Pro",
        Description: "Xuất nhập hàng trong kho.Xuất bán hàng,xuất điều trị khách hàng.Order nhà cung cấp",
        IsActive: true
    },
    {
        ID: 23,
        Name: "Kế toán",
        Type: "Pro",
        Description: "Thu chi chi nhánh, công nợ khách hàng nhà cung cấp",
        IsActive: true
    }
];