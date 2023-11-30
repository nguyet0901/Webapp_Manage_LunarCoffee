function xoa_dau (str) {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
    str = str.replace(/Đ/g, "D");
    return str;
}
function formatHTML (str) {
    str = $('<p>' + str + '</p>').text();
    return str;
}
function decodeHtml (html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}
function extract_numberic (str) {
    try {
        str = str.match(/\d/g);
        str = str.join("");
        return str;
    }
    catch (err) {
        return "";
    }
}
function dataURLtoFile (dataurl, filename) {
    var arr = dataurl.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[arr.length - 1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, {type: mime});
}

//! Encrypt the phone number into 10 character string
function encrypt_phone (phone) {
    let result = "";
    if (phone != undefined && phone.length > 0) {
        let date = new Date();
        let day = ("0" + date.getDate()).slice(-2);
        let month = ("0" + (date.getMonth() + 1)).slice(-2);
        let year = date.getFullYear();

        let stringdate = year.toString() + month.toString() + day.toString() + phone;
        result = btoa(stringdate)
    }
    return result;
}

function decrypt_phone (phone_encrypt) {
    let result = "";
    if (phone_encrypt != undefined && phone_encrypt.length > 0) {
        let temp = atob(phone_encrypt);
        result = temp.substr(8);
    }
    return result;
}

function isFunction (func) {
    if (typeof func === 'function') return true;
    return false;
}
//#region // Cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
} 

function getCookie (cname) {
    try {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";

    }
    catch (ex) {
         return "";
    }

}
//#endregion


//#reigon // PASTE DATA
function catchPaste(evt, elem, callback) {
    if (evt.originalEvent && evt.originalEvent.clipboardData) {
        callback(evt.originalEvent.clipboardData.getData('text'));
    } else if (evt.clipboardData) {
        callback(evt.clipboardData.getData('text/plain'));
    } else if (window.clipboardData) {
        callback(window.clipboardData.getData('Text'));
    } else {
        setTimeout(function () {
            callback(elem.value)
        }, 100);
    }
}
//#endregion


//#region // Search Last Name

function getLastName(text = '') {
    //try{
    let name = text?.trim()?.split(' ');
    return name[name.length - 1] ?? "";
    //}
    //catch(ex){
    //    return '';
    //}
}

function CheckIsSeachLastName(fullname = '') {
    try{
        let name = fullname?.trim()?.split(' ');
        if (((name.length - 1) ?? 1) == 0 && name != xoa_dau(fullname)) return true;
        return false;
    }
    catch(ex){
        return fasle;
    }
}


//#endregion

//#region // Load image empty
function syschart_nodata (id) {
    try {
        let tagname = $("#" + id).get(0).tagName;
        if (tagname.toLowerCase() == "canvas") {
            let canvas = document.getElementById(id);
            let ctx3 = canvas.getContext("2d");
            ctx3.clearRect(0, 0, canvas.width, canvas.height);
            ctx3.fillStyle = "#99979708";
            ctx3.fillRect(0, 0, canvas.width, canvas.height);
            ctx3.fillStyle = "#9e9e9e61";
            ctx3.font = '20px sans-serif';
            ctx3.textBaseline = 'middle';
            ctx3.textAlign = "center";
            ctx3.fillText('No data available', canvas.width / 2, canvas.height / 2);
            
            //var base_image = new Image();
            //base_image.onload = function () {
            //   // let canvas = document.getElementById(id);
            //   // let ctx3 = canvas.getContext("2d");
            //   // ctx3.clearRect(0, 0, canvas.width, canvas.height);
            //   // ctx3.drawImage(base_image,
            //   //     canvas.width / 2 - base_image.width / 2,
            //   //     canvas.height / 2 - base_image.height / 2
            //   // );
            //   //// ctx3.drawImage(base_image, 0, 0, 100, 100 * base_image.height / base_image.width)

               

            //}
            //base_image.src = '/Image/empty.svg';
        }
    }
    catch (ex) {
    }
}
//#endregion