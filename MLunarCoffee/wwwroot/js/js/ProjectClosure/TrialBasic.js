﻿var BlockControl = (function () {
    let Data = {
        Static: [
            { Selector: "#sidenav-collapse-main #menuDesktopCategory a[href*='/employee/userlist']", ParrentSelector: ".nav-item", IsDisable: false, Note: "Sidebar - nhân vien, user" },
        ],
        Dynamic: [
            //#region //NavService
            //#region //Service
            {
                MutationParent: "ServicetList",
                ChildNode: [
                    { Selector: "#ServicetList #SerList_btnAddGroup", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAddGroup" },
                    { Selector: "#ServicetList #SerList_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#ServicetList #dtContentService tr th.servicelist_thHanlde", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentServiceBody",
                ChildNode: [
                    { Selector: "#ServicetList #dtContentServiceBody tr td.ServiceList_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "ServiceDetail_Div",
                ChildNode: [
                    { Selector: "#ServiceDetail_Div #divServiceDetailMain #ServiceDetail_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "ServiceDatail btnSave" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Medicine

            {
                MutationParent: "PrescriptionMedicineList",
                ChildNode: [
                    { Selector: "#PrescriptionMedicineList #Prescription_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#PrescriptionMedicineList #dtContentPrescriptionMedicine tr th.Prescription_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentPrescriptionMedicineBody",
                ChildNode: [
                    { Selector: "#PrescriptionMedicineList #dtContentPrescriptionMedicineBody tr td.Prescription_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "PrescriptionMedicineDetail",
                ChildNode: [
                    { Selector: "#PrescriptionMedicineDetail #Medicine_Container #Medicine_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "MedicineDetail - btnSave" },
                ],
                IsMutation: true
            },
            //#endregion
            //#endregion

            //#region //Nav Card
            //#region //PrepaidCard
            {
                MutationParent: "divServiceCardList",
                ChildNode: [
                    { Selector: "#divServiceCardList #SCD_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#divServiceCardList #SC_Content tr th.SC_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "divServiceCardList",
                ChildNode: [
                    { Selector: "#divServiceCardList #SC_ContentBody tr td.SC_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "divServiceCardDetail",
                ChildNode: [
                    { Selector: "#divServiceCardDetail #ServiceCard_Container #ServiceCard_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "PrepaidCardDetail - btnSave" },
                    { Selector: "#divServiceCardDetail #ServiceCard_Container #SC_BtnDelete", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "PrepaidCardDetail - btnDel" },
                ],
                IsMutation: true
            },
            //#endregion

            //#region //Card Status
            {
                MutationParent: "CLContentBody",
                ChildNode: [
                    { Selector: "#divCardList #CLContentBody tr td.CardList_rowBtnHandle .buttonMoveClass", IsDisable: false, ParrentSelector: "button", AttrMuta: null, Note: "tdHandle td btnMove" },
                ],
                IsMutation: true
            },
            //#endregion
            //#endregion

            //#region //Warehouse
            //#region //Inventory Manag
            {
                MutationParent: "Dw_detail",
                ChildNode: [
                    { Selector: "#Dw_detail #StockOutMain #StockInfo #DetailWare_WareReceiptDiv", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "InventoryDetail - WareReceipt" },
                    { Selector: "#Dw_detail #btnSaveDetail", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "InventoryDetail - btnSave" },
                    { Selector: "#Dw_detail #StockInMain #StockIn_AddNewProduct", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "InventoryDetail - btnAddProduct" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Require
            {
                MutationParent: "re_detail",
                ChildNode: [
                    { Selector: "#re_detail #btnSaveDetail", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnSave" },
                    { Selector: "#re_detail #btnDeleteDetail", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnDel" },
                    { Selector: "#re_detail #RequireDetail_btnAddProduct", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnAddProduct" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Warehouse Export
            {
                MutationParent: "ExportDetail_Container",
                ChildNode: [
                    { Selector: "#ExportDetail_Container #btnWareHouse_Delete", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnDel" },
                ],
                IsMutation: false
            },
            //#endregion
            //#region //Material 
            {
                MutationParent: "MaterialtList",
                ChildNode: [
                    { Selector: "#MaterialtList #MaterialList_btnAddType", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAddType" },
                    { Selector: "#MaterialtList #MaterialList_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#MaterialtList #dtContentMaterial tr th.MaterialList_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentMaterialBody",
                ChildNode: [
                    { Selector: "#MaterialtList #dtContentMaterialBody tr td.MaterialList_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "MaterialDetail",
                ChildNode: [
                    { Selector: "#MaterialDetail #btn_delete_material", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "MaterialDatail btnSave" },
                    { Selector: "#MaterialDetail #btn_save_material", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "MaterialDatail btnDel" },
                    { Selector: "#MaterialDetail #Material_btnAddUnit", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "MaterialDatail btnAddUnit" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Settings
            {
                MutationParent: "divMainPageWareHouse",
                ChildNode: [
                    { Selector: "#WSUnit_Container #WSUnit_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "Unit btnAdd" },
                    { Selector: "#WSUnit_Container #dtContentUnit tr th.WSUnit_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                    { Selector: "#WSUnit_Container #dtContentUnitBody tr td.WSUnit_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },

                    { Selector: "#WSSup_Container #WSSup_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "Supplier btnAdd" },
                    { Selector: "#WSSup_Container #dtContentSupplier tr th.WSSup_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                    { Selector: "#WSSup_Container #dtContentSupplierBody tr td.WSSup_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },

                    { Selector: "#WSEReason_Container #WSEReason_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "ExportReason btnAdd" },
                    { Selector: "#WSEReason_Container #dtContentWareHouseExportReason tr th.WSEReason_thHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                    { Selector: "#WSEReason_Container #dtContentWareHouseExportReasonBody tr td.WSEReason_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },

            //#endregion
            //#endregion

            //#region //Marketing
            //#region //Tiket file
            {
                MutationParent: "TFM_Main",
                ChildNode: [
                    { Selector: "#TFM_Main #TFM_ImportMain #EmployeeMarID", IsDisable: false, ParrentSelector: "div.field", AttrMuta: null, Note: "cbbemployee " },
                ],
                IsMutation: false
            },
            //#endregion
            //#region //Voucher
            {
                MutationParent: "DiscountVL_Container",
                ChildNode: [
                    { Selector: "#DiscountVL_Container #DiscountVL_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentDiscountVoucherBody",
                ChildNode: [
                    { Selector: "#DiscountVL_Container #dtContentDiscountVoucherBody tr td.DiscountVL_rowBtnHandle .buttonDeleteClass", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "voucherDetail",
                ChildNode: [
                    { Selector: "#voucherDetail #VoucherDetail_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnSave" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Promotion
            {
                MutationParent: "discountList",
                ChildNode: [
                    { Selector: "#discountList #discountbtnadd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentDiscountListBody",
                ChildNode: [
                    { Selector: "#discountList #dtContentDiscountListBody tr td.discountlist_rowBtnHandle .buttonDeleteClass", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "discountDetail",
                ChildNode: [
                    { Selector: "#discountDetail #DiscountDetail_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "PromotionDatail btnSave" },
                ],
                IsMutation: true
            },
            //#endregion
            //#endregion
            //#region //Staff & User
            //#region //Staff
            {
                MutationParent: "employeeList",
                ChildNode: [
                    { Selector: "#employeeList #employeeList_btnAddGroup", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAddGroup" },
                    { Selector: "#employeeList #dtContentEmpGroupList #grColumnDeleteEmpType", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAddGroup" },
                    { Selector: "#employeeList #employeeList_btnAdd", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#employeeList #dtContentService tr th.servicelist_thHanlde", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContentEmpGroupListBody",
                ChildNode: [
                    { Selector: "#employeeList #dtContentEmpGroupListBody tr td.employeeList_btnDelGroup", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "dtContentServiceBody",
                ChildNode: [
                    { Selector: "#ServicetList #dtContentServiceBody tr td.ServiceList_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Calendar
            {
                MutationParent: "WorkScheduleMain",
                ChildNode: [
                    { Selector: "#WorkScheduleMain #EWordSche_btnUpdate", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnUpdate" },
                    { Selector: "#WorkScheduleMain #WorkBranchID", IsDisable: true, ParrentSelector: "", AttrMuta: null, Note: "cbbBranhc" },
                    { Selector: "#WorkScheduleMain #WorkGroupEmpID", IsDisable: true, ParrentSelector: "", AttrMuta: null, Note: "cbbEmp" },
                    { Selector: "#WorkScheduleMain #WorkSearchEmployee", IsDisable: true, ParrentSelector: "", AttrMuta: null, Note: "txtSearch" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "popupExecute",
                ChildNode: [
                    { Selector: "#popupExecute #change_work_scheduler", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnAddGroup" },
                    { Selector: "#popupExecute #off_work_scheduler", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnAddGroup" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region //Permission
            {
                MutationParent: "divMainAuthorized",
                ChildNode: [
                    { Selector: "#divMainAuthorized .Permission_btnEdit", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnEdit" },
                  ],
                IsMutation: true
            },
            //#endregion
            //#endregion

            //#region //Config
            //#region //Category
            {
                MutationParent: "divMainSettingDetail",
                ChildNode: [
                    { Selector: "#divMainSettingDetail .setting_projectclosure", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "" },
                   
                
                ],
                IsMutation: true
            },

            //#endregion
            //#endregion
            //#region //My Info
            //#region //Doc code
            {
                MutationParent: "divmyinfodetail",
                ChildNode: [
                    { Selector: "#divmyinfodetail #empdetail_container #button_customer_detail_save", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "" },
                ],
                IsMutation: true
            },

            //#endregion
            //#endregion

            //#region //Labo
            {
                MutationParent: "ListLabo_Container",
                ChildNode: [
                    { Selector: "#ListLabo_Container #ListLabo_Add", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "btnAdd" },
                    { Selector: "#ListLabo_Container #dtContent_ListLabo tr th.ListLabo_thHanlde", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "thHandle th" },
                ],
                IsMutation: false
            },
            {
                MutationParent: "dtContent_ListLaboBody",
                ChildNode: [
                    { Selector: "#ListLabo_Container #dtContent_ListLaboBody tr td.ListLabo_rowBtnHandle", IsDisable: false, ParrentSelector: "", AttrMuta: null, Note: "tdHandle td" },
                ],
                IsMutation: true
            },
            {
                MutationParent: "detail_labo",
                ChildNode: [
                    { Selector: "#detail_labo #LaboDetail_btnSave", IsDisable: false, ParrentSelector: "", AttrMuta: ['style'], Note: "btnSave" },
                ],
                IsMutation: true
            },
            //#endregion
            //#region
            {
                MutationParent: "divMainPage",
                ChildNode: [
                    { Selector: "#Labo_Receiverman", IsDisable: false, ParrentSelector: ".field",  AttrMuta: ['style'], Note: "Cust Tab - Dich vu - them moi - nguoi chiu trach" },
                ],
                IsMutation: true
            },
            //#endregion
            //Modal Popup
            {
                MutationParent: "DetailModal_Content",
                ChildNode: [
                    { Selector: "#ServiceType_Container #btn_delete_service_type", ParrentSelector: "", IsDisable: false, Note: "Nav Service - Service - Service Type - btnDel" },
                    { Selector: "#ServiceType_Container #btn_save_service_type", ParrentSelector: "", IsDisable: false, Note: "Nav Service - Service - Service Type - btnSave" },
                    { Selector: "#CLE_DivCardDetail #CLE_ExecutedEdit", ParrentSelector: "", IsDisable: false, Note: "Nav Card - Card Status - btnSave" },
                    { Selector: "#MaterialType_Container #btn_delete_material_type", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Material - btnDel" },
                    { Selector: "#MaterialType_Container #btn_save_material_type", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Material - btnSave" },
                    { Selector: "#WSUD_Container #WSUD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Setting - UnitDetail - btnSave" },
                    { Selector: "#WSSD_Container #WSSD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Setting - QuotaDetail - btnSave" },
                    { Selector: "#WSSupD_Container #WSSupD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Setting - SupDetail - btnSave" },
                    { Selector: "#WSEReasonD_Container #WSEReasonD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Warehouse - Setting - ExportReasonDetail - btnSave" },
                    { Selector: "#DLD_Container #DLD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Marketing - Promotion - PromotionDetail - btnSave" },
                    { Selector: "#EGD_Container #EGD_btnSave", ParrentSelector: "", IsDisable: false, Note: "Nav Staff - Staff - StaffGroupDetail - btnSave" },
                    { Selector: "#empdetail_container #button_customer_detail_save", ParrentSelector: "", IsDisable: false, Note: "Nav Staff - Staff - StaffDetail - btnSave" },

                    { Selector: ".setting_projectclosure", ParrentSelector: "", IsDisable: false, Note: "Nav Config - Service interest - btnSave" },
                ],
                IsMutation: true
            },
 
        ]
    }
    return Data;
})();
