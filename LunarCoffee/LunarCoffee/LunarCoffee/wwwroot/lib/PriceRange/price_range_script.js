function PriceSliderInit(min, max, step, body, inputmin, inputmax) {
    $("#" + body).slider({
        range: true,
        orientation: "horizontal",
        min: min,
        max: max,
        values: [min, max],
        step: step,

        slide: function (event, ui) {
            if (ui.values[0] == ui.values[1]) {
                return false;
            }

            $("#" + inputmin).val(ui.values[0]);
            $("#" + inputmax).val(ui.values[1]);
        }
    });

    $("#" + inputmin).val($("#" + body).slider("values", 0));
    $("#" + inputmax).val($("#" + body).slider("values", 1));
    $("#" + inputmin).divide(); $("#" + inputmax).divide();
    //$('#price-range-submit').hide();
    var ischange = 1;
    $("#" + inputmin + ",#" + inputmax).on('change', function () {
        var min_price_range = parseInt($("#" + inputmin).val());
        var max_price_range = parseInt($("#" + inputmax).val());

        if (min_price_range > max_price_range) {
            $('#' + inputmax).val(min_price_range);
        }
        $("#" + body).slider({
            values: [min_price_range, max_price_range]
        });
    });


    //$("#" + inputmin + ",#" + inputmax).on("paste keyup", function () {

    //    //$('#price-range-submit').show();

    //    var min_price_range = parseInt($("#" + inputmin).val());

    //    var max_price_range = parseInt($("#" + inputmax).val());

    //    if (min_price_range == max_price_range) {

    //        max_price_range = min_price_range + 100;

    //        $("#" + inputmin).val(min_price_range);
    //        $("#" + inputmax).val(max_price_range);
    //    }
    //    $("#" + body).slider({
    //        values: [min_price_range, max_price_range]
    //    });

    //});

    //$("#slider-range,#price-range-submit").click(function () {

    //    //var min_price = $('#' + inputmin).val();
    //    //var max_price = $('#' + inputmax).val();

    //    // $("#searchResults").text("Here List of products will be shown which are cost between " + min_price  +" "+ "and" + " "+ max_price + ".");
    //});
}