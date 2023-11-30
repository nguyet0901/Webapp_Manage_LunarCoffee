

function Fill_Resize_Form_Print(div, parent, proportional) {
    var currentWidth = div.outerWidth();
    var currentHeight = div.outerHeight();

    var availableHeight = parent.innerHeight();
    var availableWidth = parent.innerWidth();

    var scaleX = availableWidth / currentWidth - 0.05;
    var scaleY = availableHeight / currentHeight - 0.05;

    var translationX = 0;
    var translationY = 0;

    if (proportional) {
        scaleX = Math.min(scaleX, scaleY);
        scaleY = scaleX;
    }
    else scaleX = 1;
 
    if (currentWidth > availableWidth && scaleX != 1) {
        translationX = Math.round((availableWidth - (currentWidth * scaleX)) / 2);
    }
    else scaleX = 1;

    translationY = Math.round((availableHeight - (currentHeight * scaleY)) / 2);
    div.css({
        "left": "0px",
        "top": "0px",
        "-webkit-transform": "translate(" + translationX + "px, "
            + 0 + "px) scale("
            + scaleX + ")",
        "-webkit-transform-origin": "0 0"
    });
}