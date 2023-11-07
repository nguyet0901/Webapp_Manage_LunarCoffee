var zeroPad = (num, places) => String(num).padStart(places, '0')
jQuery.fn.outerHTML = function () {
    return jQuery('<div />').append(this.eq(0).clone()).html();
};
//#region   Resource Browser
function ReloadResourceForBrowser() {
    var h, a, f;
    a = document.getElementsByTagName('link');
    for (h = 0; h < a.length; h++) {
        f = a[h];
        if (f.rel.toLowerCase().match(/stylesheet/) && f.href) {
            var g = f.href.replace(/(&|\?)rnd=\d+/, '');
            f.href = g + (g.match(/\?/) ? '&' : '?');
            f.href += 'rnd=' + (new Date().valueOf());
        }
    } // for
}
//#endregion

function ScrollTableAnimation(tableid, line) {
    var w = $(window);
    window.scrollTo(1000, 2000);
}

// End responsive header master
// Refresh nice scroll

String.prototype.replaceAll = function (strReplace, strWith) {
    var esc = strReplace.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    var reg = new RegExp(esc, 'ig');
    return this.replace(reg, strWith);
};

function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();
    return div.firstChild;
}
var hexDigits = new Array
    ("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f");

//Function to convert rgb color to hex format
function rgb2hex(rgb) {
    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    if (rgb != null && rgb.length > 3) {
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }
    else return "";
}

function hex(x) {
    return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16] + hexDigits[x % 16];
}


function RenderListUserOnline(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let tr = "";
                if (Number(item.TimeAgo) <= 4320) {
                    tr = '<div class="event field">'
                        + '<div class="content ' + ((Number(item.IsOnline) == 1) ? "active" : "") + '">'
                        + '<div class="summary">' + item.UserName + '</div>'
                        + '<div class="userIsOnline "></div>'
                        + '<div class="date">' + FormatTimeAgo(Number(item.TimeAgo)) + '</div>'
                        + '</div>'
                        + '</div>'
                } else {
                    tr = "";
                }
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }
}

function FormatTimeAgo(minute) {
    if (minute < 60) {
        return minute + '&nbsp;' + OutLang['Phut'];
    }
    else if (minute >= 60 % minute < 1440) {
        return parseInt(minute / 60) + '&nbsp;' + OutLang['Gio'];
    }
    else {
        return parseInt(minute / 60 / 24) + '&nbsp;' + OutLang['Ngay'];
    }
}
function Remove_Char_Tiny_Useless(s) {
    if (s != undefined) {
        s = s.replaceAll('"', '');
        s = s.replaceAll('<p>', '');
        s = s.replaceAll('</p>', '');
        s = s.replaceAll('&nbsp;', '');
        s = s.replaceAll('<', '&lt;');
        return s;
    }
    else return "";
}

function getPageName(url) {
    var index = url.lastIndexOf("/") + 1;
    var filenameWithExtension = url.substr(index);
    var filename = filenameWithExtension.split(".")[0];
    return filename;
}

Object.size = function (obj) {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
};
function Is_MAC_OS() {
    try {
        if (navigator.userAgent.indexOf('Mac OS X') != -1) return true;
        else return false;
    }
    catch (ex){
        return false;
    }
}
function containsObject(obj, list) {
    let _oi;
    for (_oi = 0; _oi < list.length; _oi++) {
        if (isEqualObject(list[_oi], obj)) {
            return true;
        }
    }

    return false;
}
function isEqualObject(objA, objB) {
    // Tạo các mảng chứa tên các property
    var aProps = Object.getOwnPropertyNames(objA);
    var bProps = Object.getOwnPropertyNames(objB);
    // Nếu độ dài của mảng không bằng nhau,
    // thì 2 objects đó không bằnh nhau.
    if (aProps.length != bProps.length) {
        return false;
    }

    for (var i = 0; i < aProps.length; i++) {
        var propName = aProps[i];
        // Nếu giá trị của cùng một property mà không bằng nhau,
        // thì 2 objects không bằng nhau.
        if (objA[propName] !== objB[propName]) {
            return false;
        }
    }
    // Nếu code chạy đến đây,
    // tức là 2 objects được tính lằ bằng nhau.
    return true;
}
String.prototype.replaceAll = function (strTarget, strSubString) {
    var strText = this;
    var intIndexOfMatch = strText.indexOf(strTarget);
    while (intIndexOfMatch != -1) {
        strText = strText.replace(strTarget, strSubString)
        intIndexOfMatch = strText.indexOf(strTarget);
    }
    return (strText);
}

jQuery.loadScript = function (url, callback) {
    jQuery.ajax({
        url: url,
        dataType: 'script',
        success: callback,
        async: true
    });
}

Date.prototype.monthNames = ["January", "February", "March","April", "May", "June","July", "August", "September","October", "November", "December"];

Date.prototype.getMonthName = function () {
    return this.monthNames[this.getMonth()];
};
Date.prototype.getShortMonthName = function () {
    return this.getMonthName().substr(0, 3);
};
function detectMob() {
    const toMatch = [
        /Android/i,
        /webOS/i,
        /iPhone/i,
        /iPad/i,
        /iPod/i,
        /BlackBerry/i,
        /Windows Phone/i
    ];

    return toMatch.some((toMatchItem) => {
        return navigator.userAgent.match(toMatchItem);
    });
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();
    return div.firstChild;
}

function Convert_PX_To_MM(num) {
    try {
        let result = Math.floor(num * 0.264583);
        return result;
    }
    catch (ex){
        return num;
    }
}
function RandomNumber () {
    let min = 1000000000;
    let max = 9999999999;
    return Math.floor(Math.random() * (max - min + 1) + min)
}