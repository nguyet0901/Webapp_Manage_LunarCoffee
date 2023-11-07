//const script = document.createElement('script')
//function loadJS(src) {

//	script.src = src;
//	document.head.append(script)
//}

//function loadlink_source(local,exter) {
//	return exter + local;
//}

//const fileref = document.createElement("link");
//function loadlink_source(filename) {
//    debugger
//    $.ajax({

//        url: filename,
//        async: false,
//        success: function (data) {
//            $("<style>").appendTo("head").html(data);
//        }
//    })
//}


function loadFile(path, type) {

    if (type == "js") {
        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", path);
    }
    else if (type == "css") {
        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", path /*+ "?ver="+(new Date()).getTime()*/);
    }
    document.getElementsByTagName("head")[0].appendChild(fileref);
}