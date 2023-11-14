/*
@license
dhtmlxScheduler v.5.1.6 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
e.locale = {
    date: {
        month_full: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
            "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
        month_short: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
            "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
        day_full: ["Chủ Nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5",
            "Thứ 6", "Thứ 7"],
        day_short: ["CN", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"]
    },
    labels: {
        dhx_cal_today_button: "Hôm Nay",
        day_tab: "Ngày",
        week_tab: "Tuần",
        month_tab: "Tháng",
        new_event: "New event",
        icon_save: "Save",
        icon_cancel: "Cancel",
        icon_details: "Details",
        icon_edit: "Edit",
        icon_delete: "Delete",
        confirm_closing: "",// Your changes will be lost, are your sure?
        confirm_deleting: "Event will be deleted permanently, are you sure?",
        section_description: "Description",
        section_time: "Time period",
        full_day: "Full day",

        /*recurring events*/
        confirm_recurring: "Do you want to edit the whole set of repeated events?",
        section_recurring: "Repeat event",
        button_recurring: "Disabled",
        button_recurring_open: "Enabled",
        button_edit_series: "Edit series",
        button_edit_occurrence: "Edit occurrence",

        /*agenda view extension*/
        agenda_tab: "Agenda",
        date: "Date",
        description: "Description",

        /*year view extension*/
        year_tab: "Năm",

        /* week agenda extension */
        week_agenda_tab: "Agenda",

        /*grid view extension*/
        grid_tab: "Grid",

        /* touch tooltip*/
        drag_to_create: "Drag to create",
        drag_to_move: "Drag to move",

        /* dhtmlx message default buttons */
        message_ok: "OK",
        message_cancel: "Cancel",

        /* wai aria labels for non-text controls */
        next: "Tới",
        prev: "Lui",
        year: "Năm",
        month: "Tháng",
        day: "Ngày",
        hour: "Giờ",
        minute: "Phút"
    }
}
});