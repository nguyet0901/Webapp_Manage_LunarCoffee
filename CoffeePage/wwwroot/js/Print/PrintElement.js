
(function () {
    js_require('/assests/dist/PrintJs/print.min.js');
    css_require('/assests/dist/PrintJs/print.min.css')
})();

function PrintElement({ title, id, before, complete, type = 'html', message = '', style, cssLink}) {
    if (typeof printJS === 'function' && $("#" + id).length != 0) {
        var cssStylePrint = [
            '/css/root_element_grid.css',
            'https://kit.fontawesome.com/42d5adcbca.js',
            '/assets/css/soft-ui-dashboard.css'
        ]
        if (typeof cssLink === 'object') {
            cssStylePrint = [...cssLink,...cssStylePrint]
        }
        printJS({
            documentTitle: title,
            printable: id,
            onLoadingStart: function () {
                if (before && typeof before === 'function') before();
            },
            onLoadingEnd: function () {
                if (complete && typeof complete === 'function') complete();
            },
            type: type,
            scanStyles: true,
            css: type == 'html' ? cssStylePrint : [],
            style: style,
            showModal: message.length != 0 ? true : '',
            modalMessage: message
        })
    }
}