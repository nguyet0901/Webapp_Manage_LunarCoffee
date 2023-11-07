
function Render_Labo_Template_Update(data) {
    if (data != undefined && data != null && data.length > 0) {
        for (i = 0; i < data.length; i++) {
            var item = data[i];
            switch (item.type) {
                case "checkbox":
                    var myEle = document.getElementById(item.id);
                    if (myEle) {
                        $("#" + item.id).prop("checked", true);
                    }
                    break;
                case "checkother":
                    var myEle = document.getElementById(item.id);
                    if (myEle) {
                        $("#" + item.id).prop("checked", true);
                    }
                    var myEle1 = document.getElementById("other" + item.id);
                    if (myEle1) {
                        $('#other' + item.id).val(item.text);
                    }
                    break;
                case "note":
                    var myEle = document.getElementById(item.id);
                    if (myEle) {
                        $("#" + item.id).val(item.text);
                    }
                    break;
                default: break;
            }
        }
    }
}