window.onload = function () {
    var links = document.getElementsByTagName("a");
   // DisableToolTip(links);
    var images = document.getElementsByTagName("img");
  //  DisableToolTip(images);
};

function DisableToolTip(elements) {
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        element.onmouseover = function () {
            this.setAttribute("org_title", this.title);
            this.title = "";
        };
        element.onmouseout = function () {
            this.title = this.getAttribute("org_title");
        };
    }
}