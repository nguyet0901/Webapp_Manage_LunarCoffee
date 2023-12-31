﻿//#region // TOAST EXPORT EXCEL

const toastExportIcon = {
    before: `<div class="spinner-border spinner-border-sm text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>`,
    error: `<i class="fas fa-exclamation-circle text-danger me-2"></i>`,
    complete: `<i class="fas fa-check text-success me-2"></i>`
}

const toastExportBody = {
    before: Outlang["Dang_chuan_bi_file"],
    error: Outlang["Chuan_bi_file_that_bai"],
    complete: Outlang["Lay_du_lieu_thanh_cong"]
}

const toastExportExcel = {
    toast: $('#liveToastExport'),
    toastTitle: $('#liveToastExport_Title'),
    toastIcon: $('#liveToastExport_Icon'),
    toastBody: $('#liveToastExport_Body'),
    begin: function (callback) {
        this.toastBody.html(toastExportBody.before);
        this.toastIcon.html(toastExportIcon.before);
        this.toast.addClass("bg-gradient-primary");
        this.toast.show();
        if (typeof callback === 'function') callback();
    },
    error: function (callback) {
        this.toastBody.html(toastExportBody.error);
        this.toastIcon.html(toastExportIcon.error);
        this.toast.addClass("bg-gradient-danger");
        setTimeout(() => {
            this.close();
            if (typeof callback === 'function') callback();
        }, 3000)
    },
    complete: function (callback) {
        this.toastBody.html(toastExportBody.complete);
        this.toastIcon.html(toastExportIcon.complete);
        this.toast.addClass("bg-gradient-success").removeClass('bg-gradient-primary bg-gradient-danger');
        setTimeout(() => {
            this.close();
            if (typeof callback === 'function') callback();
        }, 3000)
    },
    close: function (callback) {
        this.toast.hide();
        this.toastBody.html();
        this.toastIcon.html();
        this.toast.removeClass('bg-gradient-primary bg-gradient-danger bg-gradient-success');
    }
}

//#endregion